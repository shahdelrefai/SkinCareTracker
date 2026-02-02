using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SkinCareTracker.Services.Database;
using SkinCareTracker.ViewModels;
using SkinCareTracker.Views;

namespace SkinCareTracker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Database configuration
		var dbPath = Path.Combine(FileSystem.AppDataDirectory, "skincare.db");
		builder.Services.AddDbContext<AppDbContext>(options =>
			options.UseSqlite($"Data Source={dbPath}"),
			ServiceLifetime.Transient);

		builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<App>();

		// Product
		builder.Services.AddTransient<ProductRepository>();

		builder.Services.AddTransient<ProductsViewModel>();
		builder.Services.AddTransient<ProductsPage>();

		builder.Services.AddTransient<AddProductViewModel>();
		builder.Services.AddTransient<AddProductPage>();

		builder.Services.AddTransient<ProductDetailViewModel>();

		// Routines
		builder.Services.AddSingleton<RoutineRepository>();

		builder.Services.AddTransient<RoutinesViewModel>();
		builder.Services.AddTransient<RoutinesPage>();

		builder.Services.AddTransient<AddRoutineViewModel>();
		builder.Services.AddTransient<AddRoutinePage>();

		builder.Services.AddTransient<SelectProductViewModel>();
		builder.Services.AddTransient<SelectProductPage>();

		builder.Services.AddTransient<RoutineDetailViewModel>();
		builder.Services.AddTransient<RoutineDetailPage>();


	builder.Services.AddTransient<ProductDetailPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
