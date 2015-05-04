using System;
using System.Globalization;
using JetBrains.Annotations;

namespace SDN.Shared
{
    public static class Check
    {
        public static void StringIsMeanful(string argument, [InvokerParameterName] string argumentName = null)
        {
            VerifyArgumentNotNull<string>(argumentName);

            if (!string.IsNullOrWhiteSpace(argument))
            {
                return;
            }

            throw new ArgumentException(string.Format("Value of {0} should be meanful. Actual value: '{1}'", argumentName, argument ?? "<null>"), argumentName);
        }

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

            VerifyArgumentNotNull<T>(argumentName);

            throw new ArgumentNullException(argumentName);
        }

        private static void VerifyArgumentNotNull<T>(string argumentName)
        {
            if (argumentName == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Object of type {0} should not be null.", typeof(T)));
        }
    }
}
