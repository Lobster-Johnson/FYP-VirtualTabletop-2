#if ENABLE_UNET

//source code for the network manager object, modified to suit the project
//all of this is placeholder UI
namespace UnityEngine.Networking
{
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    public class NetworkManagerHUD : MonoBehaviour
    {
        public NetworkManager manager;
        [SerializeField]
        public bool showGUI = true;
        [SerializeField]
        public int offsetX;
        [SerializeField]
        public int offsetY;

        float W = Screen.width;
        float H = Screen.height;

        // Runtime variable
        bool showServer = false;
        public string playername;

        bool lanenabled = false;
        GUIStyle mainfont = new GUIStyle();

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            mainfont.fontSize = 40;
            mainfont.normal.textColor = Color.white;
        }

        void Update()
        {
            if (!showGUI)
                return;
            //get rid of these, keyboard controls aren't needed
            if (!NetworkClient.active && !NetworkServer.active && manager.matchMaker == null)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    manager.StartServer();
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    manager.StartHost();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    manager.StartClient();
                }
            }
            if (NetworkServer.active && NetworkClient.active)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    manager.StopHost();
                }
            }
        }

        void OnGUI()
        {
            if (!showGUI)
                return;

            int xpos = (int)(W * 0.35f);
            int ypos = (int)(H * 0.1f);
            int spacing = (int)(H * 0.075f);

            //get these to scale
            float buttonLength = W * 0.5f;
            float buttonHeight = H * 0.075f;

            if (!NetworkClient.active && !NetworkServer.active )
            {
                //put the title, student number etc here
                GUI.Label(new Rect(W * 0.5f, ypos, buttonLength * 0.5f, buttonHeight), "Multiplayer Virtual Tabletop", mainfont);
                ypos += spacing;
                GUI.Label(new Rect(W * 0.5f, ypos, buttonLength * 0.5f, buttonHeight), "DT228/4 C12474932", mainfont);
                ypos += spacing;
                //Enter the player name
                //GUI.Label(new Rect(xpos, ypos, buttonLength * 0.5f, buttonHeight), "Player Name");
                //playername = GUI.TextField(new Rect(xpos + (buttonLength * 0.5f), ypos, buttonLength * 0.5f, buttonHeight*0.5f), playername);
                //ypos += spacing;
            }

            if (!NetworkClient.active &&
                !NetworkServer.active &&
                manager.matchMaker == null &&
                lanenabled == false)
            {
                if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Play via LAN"))
                {
                    enableLan();
                }
                ypos += spacing;
                //if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "LAN Host(H)"))
                //{
                //    manager.StartHost();
                //}
                //ypos += spacing;

                //if (GUI.Button(new Rect(xpos, ypos, (buttonLength * 0.5f), buttonHeight), "LAN Client(C)"))
                //{
                //    manager.StartClient();
                //}
                //manager.networkAddress = GUI.TextField(new Rect(xpos + 200, ypos, (buttonLength * 0.5f), buttonHeight), manager.networkAddress);
                //ypos += spacing;

                //if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "LAN Server Only(S)"))
                //{
                //    manager.StartServer();
                //}
                //ypos += spacing;
                //if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Disable LAN"))
                //{
                //    lanenabled = false;
                //}
                //ypos += spacing;
            }

            if (!NetworkClient.active &&
                !NetworkServer.active &&
                manager.matchMaker == null &&
                lanenabled == true)
            {
                if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "LAN Host(H)"))
                {
                    manager.StartHost();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, (buttonLength * 0.5f), buttonHeight), "LAN Client(C)"))
                {
                    manager.StartClient();
                }
                manager.networkAddress = GUI.TextField(new Rect(xpos + (buttonLength * 0.5f), ypos, (buttonLength * 0.5f), buttonHeight), manager.networkAddress);
                ypos += spacing;

                //if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "LAN Server Only(S)"))
                //{
                //    manager.StartServer();
                //}
                //ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Disable LAN"))
                {
                    disableLan();
                }
                ypos += spacing;
            }

            //while running
            else
            {
                if (NetworkServer.active)
                {
                    GUI.Label(new Rect(0, ypos, buttonLength, buttonHeight), "Server: port=" + manager.networkPort);
                    ypos += spacing;
                }
                if (NetworkClient.active)
                {
                    GUI.Label(new Rect(0, ypos, buttonLength, buttonHeight), "Client: address=" + manager.networkAddress + " port=" + manager.networkPort);
                    ypos += spacing;
                }
            }

            //if the client is active but not ready
            if (NetworkClient.active && !ClientScene.ready)
            {
                //password code should go here
                if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Client Ready, Connect to host"))
                {
                    ClientScene.Ready(manager.client.connection);

                    if (ClientScene.localPlayers.Count == 0)
                    {
                        ClientScene.AddPlayer(0);
                    }
                }
                ypos += spacing;
            }

            ////if either the client or the server are active
            //if (NetworkServer.active || NetworkClient.active)
            //{
            //    if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Disconnect"))
            //    {
            //        manager.StopHost();
            //    }
            //    ypos += spacing;
            //}


            //if neither the server not the client are active, but matchmaker is on
            if (!NetworkServer.active && !NetworkClient.active && lanenabled == false)
            {
                ypos += 10;

                if (manager.matchMaker == null)
                {
                    if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Play via Internet"))
                    {
                        manager.StartMatchMaker();
                    }
                    ypos += spacing;
                }
                else
                {
                    if (manager.matchInfo == null)
                    {
                        if (manager.matches == null)
                        {
                            if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Create Internet Match"))
                            {
                                manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", manager.OnMatchCreate);
                            }
                            ypos += spacing;

                            GUI.Label(new Rect(xpos, ypos, buttonLength * 0.5f, buttonHeight), "Room Name:");
                            manager.matchName = GUI.TextField(new Rect(xpos + buttonLength * 0.5f, ypos, buttonLength * 0.5f, buttonHeight), manager.matchName);
                            ypos += spacing;
                            ypos += 10;

                            if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Find Internet Match"))
                            {
                                manager.matchMaker.ListMatches(0, 20, "", manager.OnMatchList);
                            }
                            ypos += spacing;
                        }
                        else
                        {
                            foreach (var match in manager.matches)
                            {
                                if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Join Match:" + match.name))
                                {
                                    manager.matchName = match.name;
                                    manager.matchSize = (uint)match.currentSize;
                                    manager.matchMaker.JoinMatch(match.networkId, "", manager.OnMatchJoined);
                                }
                                ypos += spacing;
                            }
                        }
                    }

                    if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Change MM server"))
                    {
                        showServer = !showServer;
                    }
                    if (showServer)
                    {
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, buttonHeight), "Local"))
                        {
                            manager.SetMatchHost("localhost", 1337, false);
                            showServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, buttonHeight), "Internet"))
                        {
                            manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                            showServer = false;
                        }
                        ypos += spacing;
                        if (GUI.Button(new Rect(xpos, ypos, 100, buttonHeight), "Staging"))
                        {
                            manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                            showServer = false;
                        }
                    }

                    ypos += spacing;

                    GUI.Label(new Rect(xpos, ypos, 300, buttonHeight), "MM Uri: " + manager.matchMaker.baseUri);
                    ypos += spacing;

                    if (GUI.Button(new Rect(xpos, ypos, buttonLength, buttonHeight), "Disable Match Maker"))
                    {
                        manager.StopMatchMaker();
                    }
                    ypos += spacing;
                }
            }
            //GUI.matrix = Matrix4x4.TRS(Vector3(0, 0, 0), Quaternion.identity, Vector3(rx, ry, 1));
        }
        void enableLan()
        {
            lanenabled = true;
        }

        void disableLan()
        {
            lanenabled = false;
        }

        public void disconnect()
        {
            if (NetworkServer.active || NetworkClient.active)
            {
                manager.StopHost();
            }
       }


    }
};
#endif //ENABLE_UNET

