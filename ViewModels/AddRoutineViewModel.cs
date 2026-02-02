using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace SkinCareTracker.ViewModels
{
    [QueryProperty(nameof(RoutineId), "id")]
    public partial class AddRoutineViewModel : ObservableObject
    {
        private readonly RoutineRepository _routineRepository;
        private readonly ProductRepository _productRepository;

        public AddRoutineViewModel(RoutineRepository routineRepository, ProductRepository productRepository)
        {
            _routineRepository = routineRepository;
            _productRepository = productRepository;

            TimeOfDayOptions = new ObservableCollection<string>
            {
                "Morning",
                "Evening",
                "Anytime"
            };

            SelectedDays = new ObservableCollection<DayOfWeek>();
            Steps = new ObservableCollection<RoutineStepViewModel>();
            Steps.CollectionChanged += (s, e) => OnPropertyChanged(nameof(HasSteps));
        }

        public bool HasSteps => Steps.Count > 0;

        [ObservableProperty]
        private int? routineId;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string selectedTimeOfDay = "Morning";

        [ObservableProperty]
        private ObservableCollection<string> timeOfDayOptions;

        [ObservableProperty]
        private ObservableCollection<RoutineStepViewModel> steps;

        [ObservableProperty]
        private ObservableCollection<DayOfWeek> selectedDays;

        [ObservableProperty]
        private TimeSpan scheduledTime = new TimeSpan(8, 0, 0);

        [ObservableProperty]
        private bool enableReminders = true;

        [ObservableProperty]
        private bool isSaving;

        // Day selection using a collection
        public ObservableCollection<SelectableDayViewModel> DaysOfWeek { get; } = new(
            Enum.GetValues<DayOfWeek>().Select(d => new SelectableDayViewModel(d))
        );

        private List<DayOfWeek> GetSelectedDays() =>
            DaysOfWeek.Where(d => d.IsSelected).Select(d => d.Day).ToList();

        private void SetSelectedDays(IEnumerable<DayOfWeek> days)
        {
            foreach (var day in DaysOfWeek)
                day.IsSelected = days.Contains(day.Day);
        }

        partial void OnRoutineIdChanged(int? value)
        {
            if (value.HasValue)
            {
                LoadRoutineAsync(value.Value);
            }
        }

        private async void LoadRoutineAsync(int id)
        {
            var routine = await _routineRepository.GetByIdAsync(id);
            if (routine != null)
            {
                Name = routine.Name;
                SelectedTimeOfDay = routine.TimeOfDay;

                Steps.Clear();
                foreach (var step in routine.Steps.OrderBy(s => s.Order))
                {
                    Steps.Add(new RoutineStepViewModel
                    {
                        Product = step.Product,
                        ProductId = step.ProductId,
                        Instructions = step.Instructions,
                        WaitTimeMinutes = step.WaitTimeMinutes,
                        Order = step.Order
                    });
                }

                if (routine.Schedule != null)
                {
                    ScheduledTime = routine.Schedule.ScheduledTime;
                    EnableReminders = routine.Schedule.EnableReminders;

                    var days = JsonConvert.DeserializeObject<List<DayOfWeek>>(routine.Schedule.ScheduledDays);
                    if (days != null)
                    {
                        SetSelectedDays(days);
                    }
                }
            }
        }

        [RelayCommand]
        private async Task AddStepAsync()
        {
            await Shell.Current.GoToAsync("selectproduct");
        }

        [RelayCommand]
        private void RemoveStep(RoutineStepViewModel step)
        {
            Steps.Remove(step);
            UpdateStepOrders();
        }

        [RelayCommand]
        private void MoveStepUp(RoutineStepViewModel step)
        {
            var index = Steps.IndexOf(step);
            if (index > 0)
            {
                Steps.Move(index, index - 1);
                UpdateStepOrders();
            }
        }

        [RelayCommand]
        private void MoveStepDown(RoutineStepViewModel step)
        {
            var index = Steps.IndexOf(step);
            if (index < Steps.Count - 1)
            {
                Steps.Move(index, index + 1);
                UpdateStepOrders();
            }
        }

        private void UpdateStepOrders()
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                Steps[i].Order = i + 1;
            }
        }

        public void AddProductAsStep(Product product, string instructions, int waitTime)
        {
            Steps.Add(new RoutineStepViewModel
            {
                Product = product,
                ProductId = product.Id,
                Instructions = instructions,
                WaitTimeMinutes = waitTime,
                Order = Steps.Count + 1
            });
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please enter a routine name", "OK");
                return;
            }

            if (Steps.Count == 0)
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please add at least one step", "OK");
                return;
            }

            IsSaving = true;
            try
            {
                var routine = new Routine
                {
                    Name = Name.Trim(),
                    TimeOfDay = SelectedTimeOfDay,
                    StartDate = DateTime.Now,
                    IsArchived = false,
                    Steps = Steps.Select((s, index) => new RoutineStep
                    {
                        ProductId = s.ProductId,
                        Order = index + 1,
                        Instructions = s.Instructions,
                        WaitTimeMinutes = s.WaitTimeMinutes
                    }).ToList(),
                    Schedule = new RoutineSchedule
                    {
                        ScheduledDays = JsonConvert.SerializeObject(GetSelectedDays()),
                        ScheduledTime = ScheduledTime,
                        EnableReminders = EnableReminders
                    }
                };

                if (RoutineId.HasValue)
                {
                    routine.Id = RoutineId.Value;
                    await _routineRepository.UpdateAsync(routine);
                }
                else
                {
                    await _routineRepository.AddAsync(routine);
                }

                await Shell.Current.DisplayAlertAsync("Success", "Routine saved!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlertAsync("Error", $"Failed to save: {ex.Message}", "OK");
            }
            finally
            {
                IsSaving = false;
            }
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    public partial class RoutineStepViewModel : ObservableObject
    {
        [ObservableProperty]
        private Product? product;

        [ObservableProperty]
        private int productId;

        [ObservableProperty]
        private string instructions = string.Empty;

        [ObservableProperty]
        private int waitTimeMinutes;

        [ObservableProperty]
        private int order;
    }

    public partial class SelectableDayViewModel : ObservableObject
    {
        public SelectableDayViewModel(DayOfWeek day) => Day = day;

        public DayOfWeek Day { get; }

        public string DisplayName => Day.ToString();

        public string ShortName => Day.ToString()[..3]; // Mon, Tue, etc.

        [ObservableProperty]
        private bool isSelected;
    }
}