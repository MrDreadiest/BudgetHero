namespace BudgetHero.App.Resources.Languages
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public class ResourceKeyAttribute : Attribute
    {
        public string SingularKey { get; }
        public string PluralKey { get; }

        public ResourceKeyAttribute(string singularKey, string pluralKey)
        {
            SingularKey = singularKey;
            PluralKey = pluralKey;
        }
    }
}
