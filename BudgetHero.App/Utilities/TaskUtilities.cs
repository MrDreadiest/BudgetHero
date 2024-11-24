using BudgetHero.App.Services.Common;
using BudgetHero.App.Services.Interfaces;
using BudgetHero.App.ViewModels.Interfaces;
using Serilog;

namespace BudgetHero.App.Utilities
{
    public static class TaskUtilities
    {
        /// <summary>
        /// Fire and forget safe async tasks
        /// </summary>
        /// <param name="task"></param>
        /// <param name="handler"></param>
        public static async void FireAndForgetSafeAsync(this Task task, IModalDisplayHandler? handler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during task execution.");
                handler?.HandleError(ex);
            }
        }

        /// <summary>
        /// Run task safely with IsBusy flag management and exception/message handling
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task RunWithBusyFlagAsync(
            this IBusyHandler obj,
            Func<Task> action,
            IMessage? message = null)
        {
            if (obj.IsBusy)
            {
                return;
            }

            bool errorOccurred = false;
            try
            {
                obj.IsBusy = true;
                await action();
            }
            catch (Exception ex)
            {
                errorOccurred = true;
                Log.Error(ex, "Error occurred during task execution.");
                obj.ModalDisplayHandler?.HandleError(ex);
            }
            finally
            {
                obj.IsBusy = false;
                if (message != null && !errorOccurred)
                {
                    obj.ModalDisplayHandler?.HandleMessage(message);
                }
            }
        }

        public static async Task RunWithBusyFlagAndConfirmationAsync(
            this IBusyHandler busyHandler,
            Func<Task> action,
            IConfirmation confirmation)
        {
            if (busyHandler.IsBusy)
            {
                return;
            }

            try
            {
                bool isConfirmed = await busyHandler.ModalDisplayHandler.HandleConfirmation(confirmation);

                if (!isConfirmed)
                    return;

                busyHandler.IsBusy = true;
                await action();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred during task execution.");
                busyHandler.ModalDisplayHandler.HandleError(ex);
            }
            finally
            {
                busyHandler.IsBusy = false;
            }
        }
    }
}
