using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class AddFoodLogPage : ContentPage
    {
        public AddFoodLogPage(AddFoodLogViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}