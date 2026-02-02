using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class DailyLogPage : ContentPage
    {
        public DailyLogPage(DailyLogViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var vm = (DailyLogViewModel)BindingContext;
            await vm.LoadLogCommand.ExecuteAsync(null);
        }
    }
}