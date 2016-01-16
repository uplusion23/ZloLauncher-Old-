using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZloLauncher_uplusion23.zloAPI
{
    class Battlefield3
    {
        int lastserver;
        public Battlefield3()
        {
            ZLO_Init();
            SetupEvents();
        }

        public void StartServer()
        {
            if (!ZLO_ListenServer())
            {
                Console.WriteLine("Cannot open Server port for listen, its fatal");
                return;
            }
            if (ZLO_ConnectMServer())
                Console.WriteLine("Connected to server master server");
            else
            {
                Console.WriteLine("Cannot connect to server master server, its fatal");
                return;
            }
        }

        public void StartClient()
        {
            if (ZLO_ConnectMClient())
            {
                Console.WriteLine("Connected to client master server");
                //ZLO_AuthClient("mail@mail.mail", "pass");//from zloemu.org
            }
            else
            {
                Console.WriteLine("Cannot connect to client master server, its fatal");
                return;
            }
        }

        ~Battlefield3()
        {
            ZLO_Close();
        }

        private void EventListener(int zevent)
        {
            /*
            Events:
//
//Client events
//
0 - Auth success
1 - Auth error
2 - Old LaucherDLL
//
3 - Server select ok
4 - server select not found - error 1
5 - server select full - error 2
6 - server select not ready - error 3
//
23 - server list begin
24 - server list end
27 - launcher(client) disconnected from master
28 - mclient timeouted and disconnected
//
36 - stats begin
37 - stats end
//
//Server events
//
29 - Server connected
30 - Server auth success
31 - Server auth fail
32 - launcher(server) disconnected from master
33 - mserver timeouted and disconnected
34 - launcher(server) connected to master
35 - Server disconnected
//
666 - BANNED
*/
            Console.WriteLine("Event {0}", zevent);
            if (zevent == 0)
            {
                string name = Marshal.PtrToStringAnsi(ZLO_GetName());
                ZLO_GetDogTags();
                ZLO_GetClanTag();

                Console.WriteLine("MemberID {0} name {1}", ZLO_GetID(), name);
                ZLO_GetVersion(1);
                ZLO_GetServerList();
                Program.username = name;

            }
            else if (zevent == 3)
            {
                Console.WriteLine("Server select ok");
                //uncomment to start the game based on selected server
                //int t = ZLO_RunMulti();
                //if (t != 0)
                //    Console.WriteLine("Run client error {0}", t);
            }
            else if (zevent == 24)
            { 
                ZLO_SelectServer(lastserver);
                ZLO_GetStats();
            }
            else if (zevent == 36)
                Console.WriteLine("Stats begin");
            else if (zevent == 37)
            {
              Program.UpdatePage();
              Console.WriteLine("Stats end");
            }
        }

        private void ClientListener(string type, string value)
        {
            Console.WriteLine("Client: [{0}] {1}", type, value);
        }

        private void ServerListener(int id, bool added)
        {
            if (added) lastserver = id;
            /*
                        if (added)
                            Console.WriteLine("server {0} added", id);
                        else
                            Console.WriteLine("server {0} removed", id);
            */
        }

        private void ServerListenerName(int id, string name)
        {
            //			Console.WriteLine("server {0} name = {1}", id, name);
        }

        private void ServerListenerAttr(int id, string name, string value)
        {
            //			Console.WriteLine("server {0} attr {1} = {2}", id, name, value);
        }

        private void ServerListenerCap(int id, int cap0, int cap1)
        {
            //			Console.WriteLine("server {0} cap0 = {1} cap1 = {2}", id, cap0, cap1);
        }

        private void ServerListenerState(int id, int state)
        {
            //			Console.WriteLine("server {0} state = {1}", id, state);
        }

        private void ServerListenerPlayers(int id, int players)
        {
            //			Console.WriteLine("server {0} players = {1}", id, players);
        }

        private void ServerListenerAddr(int id, string ip, int port)
        {
            //			Console.WriteLine("server {0} ip = {1} port = {2}", id, ip, port);
        }

        private void ZMessageListener(string msg)
        {
            Console.WriteLine("ZMessage {0}", msg);
        }

        private void VersionListener(int version)
        {
            Console.WriteLine("Launcher version from server {0}", version);
        }

        private void StatListener(string name, float value)
        {
            Console.WriteLine("name {0} value {1}", name, value);
        }
        private void DogTagListener(int advanced, int basic)
        {
            Console.WriteLine("dogtag : advanced : {0} Basic : {1}", advanced, basic);
        }
        private void ClanTagListener(string name)
        {
            Console.WriteLine("clantag : {0}", name);
            Program.clantag = name;
        }
        public void SetupEvents()
        {
            EventListenerInstance = new tEventListener(EventListener);
            ClientListenerInstance = new tClientListener(ClientListener);
            ServerListenerInstance = new tServerListener(ServerListener);
            ServerListenerNameInstance = new tServerListenerName(ServerListenerName);
            ServerListenerAttrInstance = new tServerListenerAttr(ServerListenerAttr);
            ServerListenerCapInstance = new tServerListenerCap(ServerListenerCap);
            ServerListenerStateInstance = new tServerListenerState(ServerListenerState);
            ServerListenerPlayersInstance = new tServerListenerPlayers(ServerListenerPlayers);
            ServerListenerAddrInstance = new tServerListenerAddr(ServerListenerAddr);
            ZMessageListenerInstance = new tZMessageListener(ZMessageListener);
            VersionListenerInstance = new tVersionListener(VersionListener);
            StatListenerInstance = new tStatListener(StatListener);
            DogTagsListenerInstance = new tDogTagsListener(DogTagListener);
            ClanTagListenerInstance = new tClanTagListener(ClanTagListener);

            ZLO_SetEventListener(EventListenerInstance);
            ZLO_SetClientListener(ClientListenerInstance);
            ZLO_SetServerListener(ServerListenerInstance);
            ZLO_SetServerListenerName(ServerListenerNameInstance);
            ZLO_SetServerListenerAttr(ServerListenerAttrInstance);
            ZLO_SetServerListenerCap(ServerListenerCapInstance);
            ZLO_SetServerListenerState(ServerListenerStateInstance);
            ZLO_SetServerListenerPlayers(ServerListenerPlayersInstance);
            ZLO_SetServerListenerAddr(ServerListenerAddrInstance);
            ZLO_SetZMessageListener(ZMessageListenerInstance);
            ZLO_SetVersionListener(VersionListenerInstance);
            ZLO_SetStatListener(StatListenerInstance);
            ZLO_SetDogTagsListener(DogTagsListenerInstance);
            ZLO_SetClanTagListener(ClanTagListenerInstance);

        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tDogTagsListener(int dta, int dtb);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tClanTagListener(string name);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tEventListener(int zevent);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tClientListener(string type, string value);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListener(int id, bool added);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListenerName(int id, string name);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListenerAttr(int id, string name, string value);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListenerCap(int id, int cap0, int cap1);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListenerState(int id, int state);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListenerPlayers(int id, int players);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tServerListenerAddr(int id, string ip, int port);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tZMessageListener(string msg);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tVersionListener(int version);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void tStatListener(string name, float value);


        private tDogTagsListener DogTagsListenerInstance;
        private tClanTagListener ClanTagListenerInstance;
        private tEventListener EventListenerInstance;
        private tClientListener ClientListenerInstance;
        private tServerListener ServerListenerInstance;
        private tServerListenerName ServerListenerNameInstance;
        private tServerListenerAttr ServerListenerAttrInstance;
        private tServerListenerCap ServerListenerCapInstance;
        private tServerListenerState ServerListenerStateInstance;
        private tServerListenerPlayers ServerListenerPlayersInstance;
        private tServerListenerAddr ServerListenerAddrInstance;
        private tZMessageListener ZMessageListenerInstance;
        private tVersionListener VersionListenerInstance;
        private tStatListener StatListenerInstance;

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_Init();

        //Events
        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetEventListener(tEventListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetClientListener(tClientListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListener(tServerListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListenerName(tServerListenerName l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListenerAttr(tServerListenerAttr l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListenerCap(tServerListenerCap l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListenerState(tServerListenerState l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListenerPlayers(tServerListenerPlayers l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetServerListenerAddr(tServerListenerAddr l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetZMessageListener(tZMessageListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetVersionListener(tVersionListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetStatListener(tStatListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetDogTagsListener(tDogTagsListener l);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ZLO_SetClanTagListener(tClanTagListener l);

        //Client
        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ZLO_ConnectMClient();

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_AuthClient(string login, string pass);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_GetServerList();

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_SelectServer(int id);


        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ZLO_GetID();
        //.net 4.5 fix

        [DllImport("Launcher.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ZLO_GetName();
        //

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_GetVersion(int launcher);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_GetStats();
        //// new api

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ZLO_RunCoop(int mission, int difficulty);

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ZLO_RunMulti();

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ZLO_RunSingle();
        //// new api

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_GetDogTags();

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_SetDogTags(int dta, int dtb);
        //// new api

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_GetClanTag();

        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_SetClanTag(string name);
        //


        //Server
        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ZLO_ListenServer();
        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool ZLO_ConnectMServer();
        //close api
        [DllImport("Launcher.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ZLO_Close();
    }
}

