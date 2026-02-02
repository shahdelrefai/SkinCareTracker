using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkinCareTracker.Models;
using SkinCareTracker.Services.Database;
using System.Collections.ObjectModel;

namespace SkinCareTracker.ViewModels
{
    [QueryProperty(nameof(ProductIdString), "id")]
    public partial class AddProductViewModel : ObservableObject
    {
        private readonly ProductRepository _repository;

        public AddProductViewModel(ProductRepository repository)
        {
            _repository = repository;

            // Initialize categories list
            Categories = new ObservableCollection<string>
            {
                "Cleanser",
                "Toner",
                "Serum",
                "Moisturizer",
                "Sunscreen",
                "Exfoliant",
                "Mask",
                "Eye Cream",
                "Spot Treatment",
                "Other"
            };

            // Set defaults
            PurchaseDate = DateTime.Now;
            ExpiryDate = DateTime.Now.AddYears(1);
        }

        // Receive as string to avoid InvalidCastException
        [ObservableProperty]
        private string? productIdString;

        private int? productId;
        public int? ProductId
        {
            get => productId;
            set => SetProperty(ref productId, value);
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string brand = string.Empty;

        [ObservableProperty]
        private string category = string.Empty;

        [ObservableProperty]
        private string ingredients = string.Empty;

        [ObservableProperty]
        private DateTime purchaseDate;

        [ObservableProperty]
        private DateTime expiryDate;

        [ObservableProperty]
        private string notes = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> categories;

        [ObservableProperty]
        private bool isSaving;

        partial void OnProductIdStringChanged(string? value)
        {
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out int id))
            {
                ProductId = id;
                LoadProductAsync(id);
            }
        }

        private async void LoadProductAsync(int id)
        {
            var product = await _repository.GetByIdForEditAsync(id);
            if (product != null)
            {
                Name = product.Name;
                Brand = product.Brand;
                Category = product.Category;
                Ingredients = product.Ingredients;
                PurchaseDate = product.PurchaseDate;
                ExpiryDate = product.ExpiryDate ?? DateTime.Now.AddYears(1);
                Notes = product.Notes;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            // Validate
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please enter a product name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Brand))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Please enter a brand", "OK");
                return;
            }

            IsSaving = true;
            try
            {
                var product = new Product
                {
                    Name = Name.Trim(),
                    Brand = Brand.Trim(),
                    Category = Category,
                    Ingredients = Ingredients,
                    PurchaseDate = PurchaseDate,
                    ExpiryDate = ExpiryDate,
                    Notes = Notes,
                    IsActive = true
                };

                if (ProductId.HasValue)
                {
                    product.Id = ProductId.Value;
                    await _repository.UpdateAsync(product);
                }
                else
                {
                    await _repository.AddAsync(product);
                }

                await Shell.Current.DisplayAlertAsync("Success", "Product saved!", "OK");
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
        private async Task ScanIngredientsAsync()
        {
            // Placeholder for future ingredient scanner
            await Shell.Current.DisplayAlertAsync("Coming Soon", "Ingredient scanner will be implemented in Phase 7", "OK");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}