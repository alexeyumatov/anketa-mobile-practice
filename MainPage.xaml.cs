namespace Anketa;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnRatingChanged(object? sender, ValueChangedEventArgs e)
	{
		// Слайдер дробный, а оценка — целое число
		RatingSlider.Value = Math.Round(e.NewValue);
	}

	private void OnCupsChanged(object? sender, ValueChangedEventArgs e)
	{
		CupsLabel.Text = $"Чашек кофе в неделю: {e.NewValue:F0}";
	}

	private void OnNewsToggled(object? sender, ToggledEventArgs e)
	{
		EmailEntry.IsVisible = e.Value;
		if (!e.Value)
			EmailEntry.Text = string.Empty;
	}
}
