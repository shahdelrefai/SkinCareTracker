using SkinCareTracker.Views;

namespace SkinCareTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register routes for navigation
		Routing.RegisterRoute("addproduct", typeof(AddProductPage));
		Routing.RegisterRoute("productdetail", typeof(Views.ProductDetailPage));
		Routing.RegisterRoute("addroutine", typeof(Views.AddRoutinePage));
		Routing.RegisterRoute("routinedetail", typeof(Views.RoutineDetailPage));
		Routing.RegisterRoute("selectproduct", typeof(Views.SelectProductPage));
	}
}
