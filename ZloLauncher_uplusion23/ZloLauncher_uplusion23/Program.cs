using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ZloLauncher_uplusion23
{
    public class Program
    {
        public static string username = "";
        public static string pass = "";
        public static string clantag = "";
        public static string email = "";
        public static bool remember = false;

        public static void UpdatePage()
        {
            MainWindow.LogIn();
        }
    }
}
