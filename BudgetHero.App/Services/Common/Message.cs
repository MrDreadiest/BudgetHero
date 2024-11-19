namespace BudgetHero.App.Services.Common
{
    public class Message : IMessage
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Accept { get; } = "OK";

        public Message(string title, string body)
        {
            Title = title;
            Body = body;
        }
    }
}
