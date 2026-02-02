using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class AddProductPage : ContentPage
    {
        public AddProductPage(AddProductViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}