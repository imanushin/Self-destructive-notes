using System.Diagnostics;

namespace SDN.Shared
{
    public interface IUiCheck
    {
        // [Conditional("DEBUG")]
        void IsUiThread();

        // [Conditional("DEBUG")]
        void IsBackgroundThread();
    }
}