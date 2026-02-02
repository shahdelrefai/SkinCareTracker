using SkinCareTracker.ViewModels;

namespace SkinCareTracker.Views
{
    public partial class AddRoutinePage : ContentPage
    {
        public AddRoutinePage(AddRoutineViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}