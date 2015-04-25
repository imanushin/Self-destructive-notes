using System;

namespace Deltacorvi.PortableCommon
{
    /// <summary>
    /// Error message types.
    /// </summary>
    public enum ErrorMessage
    {
        /// <summary>
        /// Unknown error occurred.
        /// </summary>
        Generic = 0,

        /// <summary>
        /// The request is not supported.
        /// </summary>
        NotSupported = 50, //ERROR_NOT_SUPPORTED

        /// <summary>
        /// There is a time and/or date difference between the client and server.
        /// </summary>
        TimeDifference = 1398, //ERROR_TIME_SKEW

        /// <summary>
        /// The wait operation timed out.
        /// </summary>
        Timeout = 258, //WAIT_TIMEOUT

        /// <summary>
        /// The service cannot accept control messages at this time.
        /// </summary>
        ServiceCannotAcceptControls = 1061, //ERROR_SERVICE_CANNOT_ACCEPT_CTRL

        /// <summary>
        /// The process terminated unexpectedly.
        /// </summary>
        ProcessTerminatedUnexpectedly = 1067, //ERROR_PROCESS_ABORTED

        /// <summary>
        /// Log service encountered an error when attempting to write to a log container.
        /// </summary>
        AuditWriteFailed = 6640, //ERROR_LOG_CONTAINER_WRITE_FAILED

        /// <summary>
        /// The enumeration value is out of range.
        /// </summary>
        EnumerationValueIncorrect = 1781, //RPC_X_ENUM_VALUE_OUT_OF_RANGE

        /// <summary>
        /// Element not found.
        /// </summary>
        NotFound = 1168, //ERROR_NOT_FOUND

        /// <summary>
        /// Handle is in an invalid state.
        /// </summary>
        InvalidHandleState = 1168, //ERROR_INVALID_HANDLE_STATE

        /// <summary>
        /// The requested operation is successful. Changes will not be effective until the system is rebooted.
        /// </summary>
        RebootRequired = 3010, //ERROR_SUCCESS_REBOOT_REQUIRED

        /// <summary>
        /// Data of this type is not supported.
        /// </summary>
        UnsupportedType = 1630, //ERROR_UNSUPPORTED_TYPE

        /// <summary>
        /// The handle is invalid.
        /// </summary>
        InvalidHandle = 6, //ERROR_INVALID_HANDLE

        /// <summary>
        /// The specified program requires a newer version of Windows.
        /// </summary>
        OldSystemVersion = 1150, //ERROR_OLD_WIN_VERSION

        /// <summary>
        /// The data area passed to a system call is too small.
        /// </summary>
        InsufficientBuffer = 122, //ERROR_INSUFFICIENT_BUFFER

        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        FileNotFound = 2, //ERROR_FILE_NOT_FOUND

        /// <summary>
        /// The data is invalid.
        /// </summary>
        InvalidData = 13, //ERROR_INVALID_DATA

        /// <summary>
        /// The specified object is too large.
        /// </summary>
        ObjectTooLarge = 8312, //ERROR_DS_OBJ_TOO_LARGE

        /// <summary>
        /// The requested control is not valid for this service.
        /// </summary>
        IncorrectControlCode = 1052, //ERROR_INVALID_SERVICE_CONTROL

        /// <summary>
        /// The service is already registered.
        /// </summary>
        ServiceAlreadyRegistered = 1242, //ERROR_ALREADY_REGISTERED

        /// <summary>
        /// The specified network resource or device is no longer available.
        /// </summary>
        NetworkResourceNotAvailable = 55, //ERROR_DEV_NOT_EXIST

        /// <summary>
        /// The device is not ready for use.
        /// </summary>
        DeviceNotAvailable = 4319, //ERROR_DEVICE_NOT_AVAILABLE

        /// <summary>
        /// Invalid certificate key usage.
        /// </summary>
        InvalidKeyUsage = 13818, //ERROR_IPSEC_IKE_INVALID_KEY_USAGE

        /// <summary>
        /// Access is denied.
        /// </summary>
        AccessDenied = 5, //ERROR_ACCESS_DENIED

        /// <summary>
        /// A required privilege is not held by the client.
        /// </summary>
        InsufficientPrivileges = 1314, //ERROR_PRIVILEGE_NOT_HELD

        /// <summary>
        /// The system cannot find the path specified.
        /// </summary>
        PathNotFound = 3, //ERROR_PATH_NOT_FOUND

        /// <summary>
        /// The parameter is incorrect.
        /// </summary>
        IncorrectParameter = 87, //ERROR_INVALID_PARAMETER
        
        /// <summary>
        /// Default object disposed message.
        /// </summary>
        ObjectDisposed = -1,

        /// <summary>
        /// Default invalid operation message.
        /// </summary>
        InvalidOperation = -2,

        /// <summary>
        /// Throws the inner exception instead.
        /// </summary>
        ThrowDirectly = -3
    }

    /// <summary>
    /// Class for centralized exception management.
    /// </summary>
    public static class Exceptions
    {
        /// <summary>
        /// Throws the exception.
        /// </summary>
        public static void Throw(ErrorMessage error, string message = null)
        {
            if (error == ErrorMessage.ThrowDirectly)
                throw new InvalidOperationException(message ?? string.Empty);

            Exception inner = CheckDefaultExceptions(error) ?? new InvalidOperationException("Error code: " + error);
            throw new InvalidOperationException(message ?? string.Empty, inner);
        }

        /// <summary>
        /// Throws the exception.
        /// </summary>
        public static void Throw(ErrorMessage error, Exception inner)
        {
            if (error == ErrorMessage.ThrowDirectly)
                throw inner;

            Exception exception = CheckDefaultExceptions(error) ?? new InvalidOperationException("Error code: " + error);
            throw new InvalidOperationException(exception.Message, inner);
        }

        private static int ConvertErrorCode(ErrorMessage error)
        {
            int errorCode = (int)error;
            if (error == ErrorMessage.Generic)
                errorCode = 13816;
            return errorCode;
        }

        private static Exception CheckDefaultExceptions(ErrorMessage error)
        {
            if((int)error >= 0)
                return null;

            switch (error)
            {
                case ErrorMessage.ObjectDisposed:
                    return new ObjectDisposedException(string.Empty);
                case ErrorMessage.InvalidOperation:
                    return new InvalidOperationException();
                default:
                    return null;
            }
        }
    }
}
