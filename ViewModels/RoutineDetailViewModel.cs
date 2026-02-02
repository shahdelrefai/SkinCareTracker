using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using Newtonsoft.Json;

namespace SkinCareTracker.ViewModels
{
    [QueryProperty(nameof(RoutineId), "id")]
    public partial class RoutineDetailViewModel : ObservableObject
    {
        private readonly RoutineRepository _repository;

        public RoutineDetailViewModel(RoutineRepository repository)
        {
            _repository = repository;
        }

        [ObservableProperty]
        private int routineId;

        [ObservableProperty]
        private Routine? routine;

        [ObservableProperty]
        private string scheduleDaysText = string.Empty;

        [ObservableProperty]
        private string scheduleTimeText = string.Empty;

        [ObservableProperty]
        private string remindersEnabledText = string.Empty;

        [ObservableProperty]
        private bool isLoading;

        partial void OnRoutineIdChanged(int value)
        {
            LoadRoutineAsync();
        }

        [RelayCommand]
        private async Task LoadRoutineAsync()
        {
            IsLoading = true;
            try
            {
                Routine = await _repository.GetByIdAsync(RoutineId);
                
                if (Routine?.Schedule != null)
                {
                    // Parse schedule days
                    var days = JsonConvert.DeserializeObject<List<DayOfWeek>>(Routine.Schedule.ScheduledDays);
                    if (days != null && days.Any())
                    {
                        if (days.Count == 7)
                        {
                            ScheduleDaysText = "Every day";
                        }
                        else
                        {
                            ScheduleDaysText = string.Join(", ", days.Select(d => d.ToString().Substring(0, 3)));
                        }
                    }
                    
                    // Format time - convert TimeSpan to DateTime for AM/PM formatting
                    var timeAsDateTime = DateTime.Today.Add(Routine.Schedule.ScheduledTime);
                    ScheduleTimeText = timeAsDateTime.ToString("h:mm tt");
                    
                    // Set reminders text
                    RemindersEnabledText = Routine.Schedule.EnableReminders ? "On" : "Off";
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task EditAsync()
        {
            await Shell.Current.GoToAsync($"addroutine?id={RoutineId}");
        }

        [RelayCommand]
        private async Task ArchiveAsync()
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Archive Routine",
                $"Archive '{Routine?.Name}'? You can view it in the archived section.",
                "Archive",
                "Cancel");

            if (confirm)
            {
                await _repository.ArchiveAsync(RoutineId);
                await Shell.Current.GoToAsync("..");
            }
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Delete Routine",
                $"Permanently delete '{Routine?.Name}'? This cannot be undone.",
                "Delete",
                "Cancel");

            if (confirm)
            {
                await _repository.DeleteAsync(RoutineId);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}