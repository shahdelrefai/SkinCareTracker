using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class SelectProductPage : ContentPage
    {
        public SelectProductPage(SelectProductViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var vm = (SelectProductViewModel)BindingContext;
            await vm.LoadCommand.ExecuteAsync(null);
        }
    }
}