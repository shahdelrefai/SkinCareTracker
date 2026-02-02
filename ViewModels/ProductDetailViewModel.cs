using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;

namespace SkinCareTracker.ViewModels
{
    [QueryProperty(nameof(ProductId), "id")]
    public partial class ProductDetailViewModel : ObservableObject
    {
        private readonly ProductRepository _repository;

        public ProductDetailViewModel(ProductRepository repository)
        {
            _repository = repository;
        }

        [ObservableProperty]
        private int productId;

        [ObservableProperty]
        private Product? product;

        [ObservableProperty]
        private bool isLoading;

        partial void OnProductIdChanged(int value)
        {
            LoadProductAsync();
        }

        [RelayCommand]
        private async Task LoadProductAsync()
        {
            IsLoading = true;
            try
            {
                Product = await _repository.GetByIdAsync(ProductId);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task EditAsync()
        {
            await Shell.Current.GoToAsync($"//products/addproduct?id={ProductId}");
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            bool confirm = await Shell.Current.DisplayAlertAsync(
                "Delete Product",
                $"Are you sure you want to delete {Product?.Name}?",
                "Delete",
                "Cancel");

            if (confirm)
            {
                await _repository.DeleteAsync(ProductId);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}