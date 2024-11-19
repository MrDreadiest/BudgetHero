namespace BudgetHero.App.Views.Content.Universal;

public partial class ActivityIndicator : ContentView
{
    public static readonly BindableProperty IsBusyProperty =
    BindableProperty.Create(
        nameof(IsBusy),
        typeof(bool),
        typeof(ActivityIndicator),
        false,
        propertyChanged: OnIsBusyChanged);

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public ActivityIndicator()
    {
        InitializeComponent();
    }

    private static void OnIsBusyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ActivityIndicator indicator && newValue is bool isBusy)
        {
            indicator.OnIsBusyChanged(isBusy);
        }
    }

    private void OnIsBusyChanged(bool isBusy)
    {
        // TODO: Dodatkowe animacje
    }
}