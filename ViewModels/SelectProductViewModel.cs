using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using SkinCareTracker.Views;
using System.Collections.ObjectModel;

namespace SkinCareTracker.ViewModels
{
    public partial class SelectProductViewModel : ObservableObject
    {
        private readonly ProductRepository _repository;

        public SelectProductViewModel(ProductRepository repository)
        {
            _repository = repository;
        }

        [ObservableProperty]
        private ObservableCollection<Product> products = new();

        [ObservableProperty]
        private Product? selectedProduct;

        [ObservableProperty]
        private string instructions = string.Empty;

        [ObservableProperty]
        private int waitTimeMinutes = 0;

        [ObservableProperty]
        private bool isLoading;

        [RelayCommand]
        private async Task LoadAsync()
        {
            IsLoading = true;
            try
            {
                var list = await _repository.GetAllActiveAsync();
                Products.Clear();
                foreach (var product in list)
                    Products.Add(product);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void SelectProduct(Product product)
        {
            SelectedProduct = product;
        }

        [RelayCommand]
        private async Task AddToRoutineAsync()
        {
            if (SelectedProduct == null)
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please select a product", "OK");
                return;
            }

            // Navigate back and pass data to AddRoutineViewModel
            var navStack = Shell.Current.Navigation.NavigationStack;
            if (navStack.Count > 0)
            {
                var previousPage = navStack[navStack.Count - 2];
                if (previousPage is AddRoutinePage addRoutinePage)
                {
                    var vm = (AddRoutineViewModel)addRoutinePage.BindingContext;
                    vm.AddProductAsStep(SelectedProduct, Instructions, WaitTimeMinutes);
                }
            }

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}