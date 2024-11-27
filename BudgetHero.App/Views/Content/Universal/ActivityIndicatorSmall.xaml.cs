namespace BudgetHero.App.Views.Content.Universal;

public partial class ActivityIndicatorSmall : ContentView
{
    public static readonly BindableProperty IsBusyProperty =
BindableProperty.Create(
    nameof(IsBusy),
    typeof(bool),
    typeof(ActivityIndicatorSmall),
    false,
    propertyChanged: OnIsBusyChanged);

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public ActivityIndicatorSmall()
    {
        InitializeComponent();
    }

    private static void OnIsBusyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ActivityIndicatorSmall indicator && newValue is bool isBusy)
        {
            indicator.OnIsBusyChanged(isBusy);
        }
    }

    private void OnIsBusyChanged(bool isBusy)
    {
        // TODO: Dodatkowe animacje
    }
}