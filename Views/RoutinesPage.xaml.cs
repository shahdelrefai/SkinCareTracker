using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class RoutinesPage : ContentPage
    {
        public RoutinesPage(RoutinesViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var vm = (RoutinesViewModel)BindingContext;
            await vm.LoadCommand.ExecuteAsync(null);
        }
    }
}