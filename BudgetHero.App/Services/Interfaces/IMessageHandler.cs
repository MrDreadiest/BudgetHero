using BudgetHero.App.Services.Common;

namespace BudgetHero.App.Services.Interfaces
{
    public interface IMessageHandler
    {
        void HandleMessage(IMessage message);
    }
}
