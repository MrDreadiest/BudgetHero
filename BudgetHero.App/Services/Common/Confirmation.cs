using BudgetHero.App.Resources.Languages;

namespace BudgetHero.App.Services.Common
{
    public class Confirmation : IConfirmation
    {
        public string Title { get; set; } = AppResource.General_DisplayAlert_Confirmation_Title;
        public string Message { get; set; } = string.Empty;
        public string Accept { get; set; } = AppResource.General_DisplayAlert_Confirmation_Accept;
        public string Cancel { get; set; } = AppResource.General_DisplayAlert_Confirmation_Cancel;

        public Confirmation(string message)
        {
            Message = message;
        }

        public Confirmation(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
