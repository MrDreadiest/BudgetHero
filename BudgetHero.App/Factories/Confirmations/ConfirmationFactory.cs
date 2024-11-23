using BudgetHero.App.Resources.Languages;
using BudgetHero.App.Services.Common;
using System.Reflection;

namespace BudgetHero.App.Factories.Confirmations
{
    public static class ConfirmationFactory
    {
        public static IConfirmation CreateConfirmation<TResource>(string actionResourceKey, bool isPlural = false)
        {
            var resourceKeyAttribute = typeof(TResource).GetCustomAttribute<ResourceKeyAttribute>();
            if (resourceKeyAttribute == null)
            {
                throw new InvalidOperationException($"Type {typeof(TResource).Name} has no ResourceKeyAttribute assigned.");
            }

            string action = AppResource.ResourceManager.GetString(actionResourceKey);
            string resource = AppResource.ResourceManager.GetString(
                isPlural ? resourceKeyAttribute.PluralKey : resourceKeyAttribute.SingularKey
            );

            string templateKey = isPlural
                ? nameof(AppResource.General_DisplayAlert_Confirmation_Message_Template_Plural)
                : nameof(AppResource.General_DisplayAlert_Confirmation_Message_Template_Singular);

            string messageTemplate = AppResource.ResourceManager.GetString(templateKey);
            string message = string.Format(messageTemplate, action, resource);

            return new Confirmation(message);
        }

        public static IConfirmation CreateAddConfirmation<TResource>(bool isPlural = false) =>
            CreateConfirmation<TResource>(nameof(AppResource.General_DisplayAlert_Confirmation_Action_Add), isPlural);

        public static IConfirmation CreateDeleteConfirmation<TResource>(bool isPlural = false) =>
            CreateConfirmation<TResource>(nameof(AppResource.General_DisplayAlert_Confirmation_Action_Delete), isPlural);

        public static IConfirmation CreateUpdateConfirmation<TResource>(bool isPlural = false) =>
            CreateConfirmation<TResource>(nameof(AppResource.General_DisplayAlert_Confirmation_Action_Update), isPlural);

    }
}
