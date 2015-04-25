using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using JetBrains.Annotations;

namespace SDN.WP
{
    internal static class Check
    {
        /// <summary>
        /// Checks the specified argument. If argument is null then exception is thrown.
        /// </summary>
        /// <param name="argument">Argument.</param>
        /// <param name="argumentName">Argument name.</param>
        [ContractAnnotation("argument:null => halt")]
        public static void ObjectIsNotNull<T>(T argument, [InvokerParameterName] string argumentName = null)
        {
            if (argument != null)
                return;

            if (argumentName == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Object of type {0} should not be null.", typeof(T)));

            throw new ArgumentNullException(argumentName);
        }

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
