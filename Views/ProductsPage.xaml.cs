using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class ProductsPage : ContentPage
    {
        public ProductsPage(ProductsViewModel vm)
        {
            InitializeComponent();
            BindingContext = vm;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var vm = (ProductsViewModel)BindingContext;
            await vm.LoadCommand.ExecuteAsync(null);
        }
    }
}