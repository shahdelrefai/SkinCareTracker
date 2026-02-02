using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class ProductDetailPage : ContentPage
    {
        public ProductDetailPage(ProductDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}