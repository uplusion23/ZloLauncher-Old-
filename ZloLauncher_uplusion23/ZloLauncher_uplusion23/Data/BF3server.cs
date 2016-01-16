using System.Collections.Generic;

namespace ZloLauncher_uplusion23.Data
{
    class BF3server
    {
        public int gamesize;                                                        // ServerListenerCap - cap0
        public string ip;                                                           // ServerListenerAddr - ip
        public string name;                                                         // ServerListenerName - name
        public int playercount;                                                     // ServerListenerPlayers - players
        public int port;                                                            // ServerListenerAddr - port
        public int state;                                                           // ServerListenerState - state
        public Dictionary<string, string> attr = new Dictionary<string, string>();  // ServerListenerAttr - name, value


        // initialize serverentry
        public BF3server()
        {
            gamesize = playercount = port = state = 0;
            ip = "?";
            name = "0";
            attr.Add("mod", "");
            attr.Add("preset", "");
            attr.Add("levellocation", "");
            attr.Add("level", "");
            attr.Add("punkbuster", "");
            attr.Add("settings1", "");
        }
    }
}
