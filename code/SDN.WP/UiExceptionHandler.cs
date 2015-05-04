using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using JetBrains.Annotations;
using SDN.Shared;
using SDN.WP.Resources;

namespace SDN.WP
{
    internal static class UiExceptionHandler
    {
        public async static Task Suppress(this Task operationToSuppress, [NotNull] string messageText)
        {
            Check.StringIsMeanful(messageText, "messageText");

            try
            {
                await operationToSuppress;
            }
            catch (Exception ex)
            {
#if DEBUG
                messageText = messageText + ex;
#endif
                BackroundTasks.ReportErrorAsync(null, messageText, ex);

                MessageBox.Show(messageText, AppResources.Error, MessageBoxButton.OK);
            }
        }
    }
}
