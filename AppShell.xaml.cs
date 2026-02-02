using SkinCareTracker.Views;

namespace SkinCareTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes for navigation
		Routing.RegisterRoute("addproduct", typeof(AddProductPage));
		Routing.RegisterRoute("productdetail", typeof(ProductDetailPage));
		Routing.RegisterRoute("addroutine", typeof(AddRoutinePage));
		Routing.RegisterRoute("routinedetail", typeof(RoutineDetailPage));
		Routing.RegisterRoute("selectproduct", typeof(SelectProductPage));
		Routing.RegisterRoute("addfoodlog", typeof(AddFoodLogPage));
		Routing.RegisterRoute("photoviewer", typeof(PhotoViewerPage));
	}
}
