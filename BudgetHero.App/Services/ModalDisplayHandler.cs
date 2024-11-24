using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.Utilities;

namespace BudgetHero.App.Services
{
    public class ModalDisplayHandler : IModalDisplayHandler
    {
        SemaphoreSlim _semaphore = new(1, 1);

        public void HandleError(Exception ex)
        {
            DisplayAlert(ex).FireAndForgetSafeAsync();
        }

        public void HandleMessage(IMessage message)
        {
            DisplayAlert(message).FireAndForgetSafeAsync();
        }

        public async Task<bool> HandleConfirmation(IConfirmation confirmation)
        {
            return await DisplayAlert(confirmation);
        }

        private async Task DisplayAlert(IMessage message)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (Shell.Current is Shell shell)
                {
                    await shell.DisplayAlert(message.Title, message.Body, message.Accept);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<bool> DisplayAlert(IConfirmation confirmation)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (Shell.Current is Shell shell)
                {
                    bool isConfirmed = await shell.DisplayAlert(confirmation.Title, confirmation.Message, confirmation.Accept, confirmation.Cancel);
                    return isConfirmed;
                }

                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task DisplayAlert(Exception ex)
        {
            try
            {
                await _semaphore.WaitAsync();
                if (Shell.Current is Shell shell)
                {
                    await shell.DisplayAlert("Error", ex.Message, "OK");
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
