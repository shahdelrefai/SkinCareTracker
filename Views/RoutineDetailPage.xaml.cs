using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class RoutineDetailPage : ContentPage
    {
        public RoutineDetailPage(RoutineDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}