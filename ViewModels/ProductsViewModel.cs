using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using System.Collections.ObjectModel;

namespace SkinCareTracker.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly ProductRepository _repository;
        public ProductsViewModel(ProductRepository repository)
        {
            _repository = repository;
        }
        
        [ObservableProperty]
        private ObservableCollection<Product> products = new();

        [ObservableProperty]
        private string searchText = string.Empty;

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
                foreach (var item in list)
                    Products.Add(item);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task SearchAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                await LoadAsync();
                return;
            }
            IsLoading = true;
            try
            {
                var list = await _repository.SearchAsync(SearchText);
                Products.Clear();
                foreach (var item in list)
                    Products.Add(item);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            await Shell.Current.GoToAsync("addproduct");
        }

        [RelayCommand]
        private async Task ViewAsync(Product product)
        {
            await Shell.Current.GoToAsync($"productdetail?id={product.Id}");
        }
    }
}