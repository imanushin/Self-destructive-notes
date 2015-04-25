using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDN.WP
{
    internal static class StackTraceHelper
    {
        public static string GetCurrentStackTrace()
        {
            var frames = new StackTrace().GetFrames();

            if (frames == null)
            {
                return string.Empty;
            }

            var callingFrames = frames.Skip(1).ToArray();

            return string.Join(Environment.NewLine, callingFrames.Select(f => f.GetMethod().Name));
        }
    }
}
