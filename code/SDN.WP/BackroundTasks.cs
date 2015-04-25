using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Yandex.Metrica;

namespace SDN.WP
{
    internal static class BackroundTasks
    {
        public static async Task StartAnalyticAsync()
        {
            await Task.Run(() => YandexMetrica.Start(36342));
        }

        public static async Task ReportErrorAsync(string pageData, string message, Exception error)
        {
            await Task.Run(() => ReportError(pageData, message, error));
        }

        private static void ReportError(string pageData, string message, Exception error)
        {
            var resultMessage = string.Format("Page {0} (message: {1})", pageData, message);

            YandexMetrica.ReportError(resultMessage, error);
        }
    }
}
