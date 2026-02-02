using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using System.Collections.ObjectModel;

namespace SkinCareTracker.ViewModels
{
    [QueryProperty(nameof(DateString), "date")]
    public partial class AddFoodLogViewModel : ObservableObject
    {
        private readonly DailyLogRepository _repository;

        public AddFoodLogViewModel(DailyLogRepository repository)
        {
            _repository = repository;

            MealTypes = new ObservableCollection<string>
            {
                "Breakfast",
                "Lunch",
                "Dinner",
                "Snack"
            };
        }

        [ObservableProperty]
        private string dateString = string.Empty;

        [ObservableProperty]
        private string selectedMealType = "Breakfast";

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> mealTypes;

        [ObservableProperty]
        private bool isSaving;

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please enter what you ate", "OK");
                return;
            }

            IsSaving = true;
            try
            {
                var date = DateTime.Parse(DateString);
                var dailyLog = await _repository.GetOrCreateByDateAsync(date);

                var foodLog = new FoodLog
                {
                    MealType = SelectedMealType,
                    Description = Description.Trim(),
                    LoggedAt = DateTime.Now
                };

                await _repository.AddFoodLogAsync(dailyLog.Id, foodLog);

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
}