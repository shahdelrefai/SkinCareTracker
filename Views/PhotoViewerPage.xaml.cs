namespace SkinCareTracker.Views
{
    [QueryProperty(nameof(PhotoPath), "photoPath")]
    public partial class PhotoViewerPage : ContentPage
    {
        private bool _isZoomed = false;

        public PhotoViewerPage()
        {
            InitializeComponent();
        }

        private string? _photoPath;
        public string? PhotoPath
        {
            get => _photoPath;
            set
            {
                _photoPath = value;
                LoadPhoto();
            }
        }

        private void LoadPhoto()
        {
            if (!string.IsNullOrEmpty(_photoPath) && File.Exists(_photoPath))
            {
                var bytes = File.ReadAllBytes(_photoPath);
                FullImage.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
            }
        }

        private void OnDoubleTap(object? sender, TappedEventArgs e)
        {
            // Toggle zoom on double tap
            if (_isZoomed)
            {
                FullImage.Scale = 1;
                _isZoomed = false;
            }
            else
            {
                FullImage.Scale = 2.5;
                _isZoomed = true;
            }
        }

        private async void OnCloseClicked(object? sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
