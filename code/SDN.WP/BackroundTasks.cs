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
        public static void StartAnalyticAsync()
        {
            YandexMetrica.Start(36342);

            /*99370811*/
        }
    }
}
