using System.Text;

namespace Anketa;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnRequiredFieldChanged(object? sender, TextChangedEventArgs e)
	{
		UpdateShowResultButton();
	}

	private void OnFrequencyChanged(object? sender, CheckedChangedEventArgs e)
	{
		UpdateShowResultButton();
	}

	// Кнопка результата доступна, только когда начато заполнение обязательных полей
	private void UpdateShowResultButton()
	{
		ShowResultButton.IsEnabled =
			!string.IsNullOrWhiteSpace(NameEntry.Text) ||
			!string.IsNullOrWhiteSpace(PhoneEntry.Text);
		ErrorLabel.IsVisible = false;
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

	private void OnShowResultClicked(object? sender, EventArgs e)
	{
		var errors = Validate();
		if (errors.Count > 0)
		{
			ErrorLabel.Text = string.Join(Environment.NewLine, errors);
			ErrorLabel.IsVisible = true;
			ResultBorder.IsVisible = false;
			return;
		}

		ErrorLabel.IsVisible = false;
		ResultLabel.Text = BuildSummary();
		ResultBorder.IsVisible = true;
	}

	private List<string> Validate()
	{
		var errors = new List<string>();

		if (string.IsNullOrWhiteSpace(NameEntry.Text))
			errors.Add("• Укажите имя");

		var digits = (PhoneEntry.Text ?? string.Empty).Count(char.IsDigit);
		if (digits < 10)
			errors.Add("• Телефон должен содержать не меньше 10 цифр");

		if (GetSelectedFrequency() is null)
			errors.Add("• Выберите, как часто вы нас посещаете");

		if (NewsSwitch.IsToggled && !(EmailEntry.Text ?? string.Empty).Contains('@'))
			errors.Add("• Для рассылки нужен корректный e-mail");

		return errors;
	}

	private string? GetSelectedFrequency()
	{
		RadioButton[] radios = [FirstTimeRadio, MonthlyRadio, WeeklyRadio, DailyRadio];
		return radios.FirstOrDefault(r => r.IsChecked)?.Value?.ToString();
	}

	private string BuildSummary()
	{
		var extras = new List<string>();
		if (DessertsCheckBox.IsChecked) extras.Add("десерты");
		if (BreakfastCheckBox.IsChecked) extras.Add("завтраки");
		if (TakeawayCheckBox.IsChecked) extras.Add("берёт с собой");

		var sb = new StringBuilder();
		sb.AppendLine($"Имя: {NameEntry.Text!.Trim()}");
		sb.AppendLine($"Телефон: {PhoneEntry.Text!.Trim()}");
		sb.AppendLine($"Дата визита: {VisitDatePicker.Date:dd.MM.yyyy}");
		sb.AppendLine($"Любимый напиток: {DrinkPicker.SelectedItem ?? "не выбран"}");
		sb.AppendLine($"Частота визитов: {GetSelectedFrequency()}");
		sb.AppendLine($"Кроме напитков: {(extras.Count > 0 ? string.Join(", ", extras) : "ничего")}");
		sb.AppendLine($"Оценка: {RatingSlider.Value:F0} из 10");
		sb.AppendLine($"Чашек в неделю: {CupsStepper.Value:F0}");
		sb.AppendLine(NewsSwitch.IsToggled
			? $"Рассылка: да, на {EmailEntry.Text!.Trim()}"
			: "Рассылка: нет");
		sb.Append($"Пожелания: {(string.IsNullOrWhiteSpace(CommentEditor.Text) ? "—" : CommentEditor.Text.Trim())}");
		return sb.ToString();
	}
}
