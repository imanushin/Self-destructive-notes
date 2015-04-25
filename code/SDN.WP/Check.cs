using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SDN.WP
{
    internal sealed class Check
    {
        [Conditional("DEBUG")]
        public static void IsUiThread()
        {
            var isUiThread = Deployment.Current.Dispatcher.CheckAccess();

            True(isUiThread, "Current thread ({0}) is not UI thread", Thread.CurrentThread.ManagedThreadId);
        }

        [Conditional("DEBUG")]
        public static void IsBackgroundThread()
        {
            var isUiThread = Thread.CurrentThread.IsBackground;

            True(isUiThread, "Current thread ({0}) is not UI thread", Thread.CurrentThread.ManagedThreadId);
        }

        [Conditional("DEBUG")]
        public static void True(bool value, string message, params object[] args)
        {
            if (value)
            {
                return;
            }

            var userMessage = string.Format(message, args);
            var resultMessage = userMessage + Environment.NewLine + "Upper stack:" + Environment.NewLine + StackTraceHelper.GetCurrentStackTrace();

            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }

            throw new InvalidOperationException(resultMessage);
        }
    }
}
