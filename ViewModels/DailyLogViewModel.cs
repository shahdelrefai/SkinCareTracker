using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace SkinCareTracker.ViewModels
{
    public partial class DailyLogViewModel : ObservableObject
    {
        private readonly DailyLogRepository _dailyLogRepository;
        private readonly RoutineRepository _routineRepository;

        public DailyLogViewModel(DailyLogRepository dailyLogRepository, RoutineRepository routineRepository)
        {
            _dailyLogRepository = dailyLogRepository;
            _routineRepository = routineRepository;
            SelectedDate = DateTime.Today;
        }

        [ObservableProperty]
        private DateTime selectedDate;

        [ObservableProperty]
        private DailyLog? currentLog;

        [ObservableProperty]
        private ObservableCollection<RoutineCheckItem> scheduledRoutines = new();

        [ObservableProperty]
        private ObservableCollection<FoodLog> foodLogs = new();

        [ObservableProperty]
        private string skinObservation = string.Empty;

        [ObservableProperty]
        private int acneLevel = 5;

        [ObservableProperty]
        private int drynessLevel = 5;

        [ObservableProperty]
        private int oilinessLevel = 5;

        [ObservableProperty]
        private int rednessLevel = 5;

        [ObservableProperty]
        private ObservableCollection<SkinPhoto> skinPhotos = new();

        [ObservableProperty]
        private bool isLoading;

        partial void OnSelectedDateChanged(DateTime value)
        {
            LoadLogAsync();
        }

        [RelayCommand]
        private async Task LoadLogAsync()
        {
            IsLoading = true;
            try
            {
                CurrentLog = await _dailyLogRepository.GetOrCreateByDateAsync(SelectedDate);

                // Load scheduled routines
                var allRoutines = await _routineRepository.GetAllActiveAsync();
                ScheduledRoutines.Clear();

                foreach (var routine in allRoutines)
                {
                    var isCompleted = CurrentLog.CompletedRoutines
                        .Any(rc => rc.RoutineId == routine.Id);

                    ScheduledRoutines.Add(new RoutineCheckItem
                    {
                        Routine = routine,
                        IsCompleted = isCompleted,
                        CompletionId = CurrentLog.CompletedRoutines
                            .FirstOrDefault(rc => rc.RoutineId == routine.Id)?.Id
                    });
                }

                // Load food logs
                FoodLogs.Clear();
                foreach (var food in CurrentLog.FoodLogs.OrderBy(f => f.LoggedAt))
                {
                    FoodLogs.Add(food);
                }

                // Load skin remark
                if (CurrentLog.SkinRemark != null)
                {
                    SkinObservation = CurrentLog.SkinRemark.Observation;
                    AcneLevel = CurrentLog.SkinRemark.AcneLevel ?? 5;
                    DrynessLevel = CurrentLog.SkinRemark.DrynessLevel ?? 5;
                    OilinessLevel = CurrentLog.SkinRemark.OilinessLevel ?? 5;
                    RednessLevel = CurrentLog.SkinRemark.RednessLevel ?? 5;

                    SkinPhotos.Clear();
                    foreach (var photo in CurrentLog.SkinRemark.Photos.OrderBy(p => p.TakenAt))
                    {
                        SkinPhotos.Add(photo);
                    }
                }
                else
                {
                    SkinObservation = string.Empty;
                    AcneLevel = 5;
                    DrynessLevel = 5;
                    OilinessLevel = 5;
                    RednessLevel = 5;
                    SkinPhotos.Clear();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void PreviousDay()
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }

        [RelayCommand]
        private void NextDay()
        {
            SelectedDate = SelectedDate.AddDays(1);
        }

        [RelayCommand]
        private void Today()
        {
            SelectedDate = DateTime.Today;
        }

        [RelayCommand]
        private async Task ToggleRoutineCompletionAsync(RoutineCheckItem item)
        {
            if (CurrentLog == null) return;

            if (item.IsCompleted && item.CompletionId.HasValue)
            {
                // Uncomplete
                await _dailyLogRepository.RemoveRoutineCompletionAsync(item.CompletionId.Value);
                item.IsCompleted = false;
                item.CompletionId = null;
            }
            else
            {
                // Complete
                var completion = new RoutineCompletion
                {
                    RoutineId = item.Routine.Id,
                    CompletedAt = DateTime.Now,
                    CompletedStepIds = JsonConvert.SerializeObject(
                        item.Routine.Steps.Select(s => s.Id).ToList()
                    )
                };

                var saved = await _dailyLogRepository.AddRoutineCompletionAsync(CurrentLog.Id, completion);
                item.IsCompleted = true;
                item.CompletionId = saved.Id;
            }
        }

        [RelayCommand]
        private async Task AddFoodLogAsync()
        {
            await Shell.Current.GoToAsync($"addfoodlog?date={SelectedDate:yyyy-MM-dd}");
        }

        [RelayCommand]
        private async Task RemoveFoodLogAsync(FoodLog foodLog)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Delete Food Log",
                "Remove this food entry?",
                "Delete",
                "Cancel");

            if (confirm)
            {
                await _dailyLogRepository.RemoveFoodLogAsync(foodLog.Id);
                FoodLogs.Remove(foodLog);
            }
        }

        [RelayCommand]
        private async Task SaveSkinRemarkAsync()
        {
            if (CurrentLog == null) return;

            var remark = new SkinRemark
            {
                Observation = SkinObservation,
                AcneLevel = AcneLevel,
                DrynessLevel = DrynessLevel,
                OilinessLevel = OilinessLevel,
                RednessLevel = RednessLevel
            };

            await _dailyLogRepository.SaveSkinRemarkAsync(CurrentLog.Id, remark);
            await Shell.Current.DisplayAlertAsync("Saved", "Skin remark saved!", "OK");
        }

        [RelayCommand]
        private async Task AddPhotoAsync()
        {
            if (CurrentLog?.SkinRemark == null)
            {
                await SaveSkinRemarkAsync();
                await LoadLogAsync();
            }

            if (CurrentLog?.SkinRemark == null) return;

            try
            {
                var fileResult = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Select a skin photo"
                });
                
                if (fileResult == null) return;

                // Create unique filename (store ONLY the filename, not full path)
                var fileName = $"skin_{DateTime.Now:yyyyMMddHHmmss}.jpg";
                var localPath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // Save to local storage
                using (var stream = await fileResult.OpenReadAsync())
                using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                {
                    await stream.CopyToAsync(fs);
                }

                // Save ONLY the filename to database (not the full path)
                // This way the path is reconstructed at runtime and survives app reinstalls
                var skinPhoto = new SkinPhoto
                {
                    FilePath = fileName,
                    TakenAt = DateTime.Now,
                    Caption = string.Empty
                };

                var saved = await _dailyLogRepository.AddSkinPhotoAsync(CurrentLog.SkinRemark.Id, skinPhoto);
                SkinPhotos.Add(saved);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to add photo: {ex.Message}", "OK");
            }
        }
        [RelayCommand]
        private async Task RemovePhotoAsync(SkinPhoto photo)
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Delete Photo",
                "Remove this photo?",
                "Delete",
                "Cancel");

            if (confirm)
            {
                await _dailyLogRepository.RemoveSkinPhotoAsync(photo.Id);
                SkinPhotos.Remove(photo);
            }
        }

        [RelayCommand]
        private async Task ViewPhotoAsync(SkinPhoto photo)
        {
            if (photo == null) return;
            
            var fullPath = Path.IsPathRooted(photo.FilePath) 
                ? photo.FilePath 
                : Path.Combine(FileSystem.AppDataDirectory, photo.FilePath);
            
            await Shell.Current.GoToAsync($"photoviewer?photoPath={Uri.EscapeDataString(fullPath)}");
        }
    }

    public partial class RoutineCheckItem : ObservableObject
    {
        [ObservableProperty]
        private Routine routine = null!;

        [ObservableProperty]
        private bool isCompleted;

        [ObservableProperty]
        private int? completionId;
    }
}