using System.Threading;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using SDN.Shared;

namespace SDN.WP.UnitTests.Helpers
{
    public sealed class TestUiCheck : IUiCheck
    {
        private readonly int threadId;

        public TestUiCheck()
        {
            threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void IsUiThread()
        {
            Assert.AreEqual(threadId, Thread.CurrentThread.ManagedThreadId);
        }

        public void IsBackgroundThread()
        {
            Assert.AreEqual(threadId, Thread.CurrentThread.ManagedThreadId);
        }
    }
}
