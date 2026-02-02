using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using System.Collections.ObjectModel;

namespace SkinCareTracker.ViewModels
{
    public partial class RoutinesViewModel : ObservableObject
    {
        private readonly RoutineRepository _repository;

        public RoutinesViewModel(RoutineRepository repository)
        {
            _repository = repository;
        }

        [ObservableProperty]
        private ObservableCollection<Routine> activeRoutines = new();

        [ObservableProperty]
        private ObservableCollection<Routine> archivedRoutines = new();

        [ObservableProperty]
        private ObservableCollection<RoutineGroup> allRoutinesGrouped = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private bool showArchived;

        [ObservableProperty]
        private bool hasActiveRoutines;

        [ObservableProperty]
        private bool hasArchivedRoutines;

        partial void OnShowArchivedChanged(bool value)
        {
            UpdateGroupedRoutines();
        }

        [RelayCommand]
        private async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var active = await _repository.GetAllActiveAsync();
                ActiveRoutines.Clear();
                foreach (var routine in active)
                    ActiveRoutines.Add(routine);

                var archived = await _repository.GetArchivedAsync();
                ArchivedRoutines.Clear();
                foreach (var routine in archived)
                    ArchivedRoutines.Add(routine);

                HasActiveRoutines = ActiveRoutines.Any();
                HasArchivedRoutines = ArchivedRoutines.Any();
                UpdateGroupedRoutines();
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateGroupedRoutines()
        {
            AllRoutinesGrouped.Clear();

            // Add active routines group
            var activeGroup = new RoutineGroup("ACTIVE ROUTINES", new ObservableCollection<Routine>(ActiveRoutines));
            AllRoutinesGrouped.Add(activeGroup);

            // Add archived routines group if toggle is on
            if (ShowArchived && ArchivedRoutines.Count > 0)
            {
                var archivedGroup = new RoutineGroup("ARCHIVED ROUTINES", new ObservableCollection<Routine>(ArchivedRoutines));
                AllRoutinesGrouped.Add(archivedGroup);
            }
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            await Shell.Current.GoToAsync("addroutine");
        }

        [RelayCommand]
        private async Task ViewAsync(Routine routine)
        {
            await Shell.Current.GoToAsync($"routinedetail?id={routine.Id}");
        }

        [RelayCommand]
        private void ToggleArchived()
        {
            ShowArchived = !ShowArchived;
        }
    }
}