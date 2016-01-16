using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Awesomium;
using Awesomium.Core;
using Awesomium.Windows.Data;
using Awesomium.Windows.Controls;

namespace ZloLauncher_uplusion23
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow AppWindow;
        zloAPI.Battlefield3 dll = new zloAPI.Battlefield3();
        public MainWindow()
        {
            InitializeComponent();
            AppWindow = this;
            launcherWindow.DocumentReady += WebViewOnDocumentReady;
            dll.StartClient();
            Program.remember = Properties.Settings.Default.remember;
            if (Program.remember)
            {
                Program.pass = Properties.Settings.Default.password;
                Program.email = Properties.Settings.Default.email;
            }
            else
            {
                Program.pass = "";
                Program.email = "";
            }
        }

        private void WebViewOnDocumentReady(object sender, UrlEventArgs urlEventArgs)
        {
            JSObject appObject = launcherWindow.CreateGlobalJavascriptObject("app");

            if (!appObject)
                return;

            // NativeViewInitialized is not called in a Javascript Execution Context (JEC).
            // We explicitly dispose any created or acquired JSObjects.
            using (appObject)
            {
                appObject.BindAsync("Exit", exitEverything);
                appObject.BindAsync("Login", zlo_Login);
                appObject.BindAsync("Remember", zlo_Remember);
                appObject.BindAsync("SinglePlayer", zlo_Singleplayer);
            }
        }

        private void exitEverything(JSValue[] arguments)
        {
            Properties.Settings.Default.remember = Program.remember;
            Properties.Settings.Default.password = Program.pass;
            Properties.Settings.Default.email = Program.email;
            Application.Current.Shutdown();
        }

        private void zlo_Remember(JSValue[] arguments)
        {
            Program.remember = true;
            string email = Program.email;
            string pass = Program.pass;
            if (email != null && pass != null)
            {
                try
                {
                    zloAPI.Battlefield3.ZLO_AuthClient(email, pass);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Login failed! Please try again.");
                }
                finally
                {
                }
                Console.WriteLine("Loggedin - " + arguments[0].ToString());
            }
        }

        private void zlo_Login(JSValue[] arguments)
        {
            try
            {
                zloAPI.Battlefield3.ZLO_AuthClient(arguments[0].ToString(), arguments[1].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Login failed! Please try again.");
            }
            finally
            {
                Program.pass = arguments[1].ToString();
                Program.email = arguments[0].ToString();
            }
            Console.WriteLine("Loggedin - " + arguments[0].ToString());
        }

        private void zlo_Singleplayer(JSValue[] arguments)
        {
            zloAPI.Battlefield3.ZLO_RunSingle();
        }

        public static void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public static void LogIn()
        {
            AppWindow.Dispatcher.Invoke((Action)(() =>
            {
                AppWindow.launcherWindow.ExecuteJavascript("LoggedIn('" + Program.username.ToString() + "', '" + Program.clantag.ToString() + "');");
            }));
            
        }
    }
}
