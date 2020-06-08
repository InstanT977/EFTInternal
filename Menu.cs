using EscapeFromGod.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace EscapeFromGod
{
    public class Menu : MonoBehaviour
    {
        private bool editing = false;

        private static Rect MenuWindow = new Rect(10, 10, 200, 140);
        private static Rect MenuESPWindow = new Rect(220, 10, 200, 250);
        private static Rect MenuItemsWindow = new Rect(430, 10, 200, 310);
        private static Rect MenuWorldWindow = new Rect(640, 10, 200, 160);
        private static Rect MenuPlayerWindow = new Rect(850, 10, 200, 160);
        private static Rect MenuAimbotWindow = new Rect(1060, 10, 200, 250);
        private static Rect MenuDebugWindow = new Rect(1270, 10, 200, 130);
        private CGameWorld m_gameworld;
        private CPlayer m_player;
        private CItems m_items;
        private CExtract m_Extract;
        private CAimbot m_aimbot;
        private CDoors m_doors;
        private CSpeedhack m_Speed;
        private CThermal m_thermal;
        private NamelessScreen m_namelessScreen;

        private bool MenuVisible = true;
        private bool MenuESPVisible = true;
        private bool MenuItemsVisible = true;
        private bool MenuWorldVisible = true;
        private bool MenuPlayerVisible = true;
        private bool MenuAimbotVisible = true;
        private bool MenuDebugVisible = true;
        void Awake()
        {
            try
            {
                m_gameworld = gameObject.AddComponent<CGameWorld>();
                m_player = gameObject.AddComponent<CPlayer>();
                m_items = gameObject.AddComponent<CItems>();
                m_Extract = gameObject.AddComponent<CExtract>();
                m_aimbot = gameObject.AddComponent<CAimbot>();
                m_doors = gameObject.AddComponent<CDoors>();
                m_Speed = gameObject.AddComponent<CSpeedhack>();
                m_thermal = gameObject.AddComponent<CThermal>();
                m_namelessScreen = gameObject.AddComponent<NamelessScreen>();

#if Private
                Logs.Add("Sucessfully added every component", Logs.logLevelEnum.Low);
#endif
            }
            catch { }

        }
        void Update()
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    MenuVisible = !MenuVisible;
                    MenuWorldVisible = !MenuWorldVisible;
                    MenuESPVisible = !MenuESPVisible;
                    MenuItemsVisible = !MenuItemsVisible;
                    MenuPlayerVisible = !MenuPlayerVisible;
                    MenuAimbotVisible = !MenuAimbotVisible;
                    MenuDebugVisible = !MenuDebugVisible;
                }
            }
            catch { }
        }
        void OnGUI()
        {
            try
            {
                if (MenuVisible)
                    MenuWindow = GUI.Window(0, MenuWindow, DrawMenu, new GUIContent("Menu"));
                if (MenuESPVisible)
                    MenuESPWindow = GUI.Window(1, MenuESPWindow, DrawESP, new GUIContent("ESP"));
                if (MenuItemsVisible)
                    MenuItemsWindow = GUI.Window(2, MenuItemsWindow, DrawItems, new GUIContent("Items"));
                if (MenuWorldVisible)
                    MenuWorldWindow = GUI.Window(3, MenuWorldWindow, DrawWorld, new GUIContent("World"));
                if (MenuPlayerVisible)
                    MenuPlayerWindow = GUI.Window(4, MenuPlayerWindow, DrawPlayer, new GUIContent("Player"));
                if (MenuAimbotVisible)
                    MenuAimbotWindow = GUI.Window(5, MenuAimbotWindow, DrawAimbot, new GUIContent("Aimbot"));
#if Private
                if (MenuDebugVisible)
                    MenuDebugWindow = GUI.Window(6, MenuDebugWindow, DrawDebug, new GUIContent("Debug"));
#endif
            }
            catch { }
        }
        void DrawMenu(int id)
        {
            try
            {
                switch (id)
                {
                    case 0:
                        GUI.DragWindow(new Rect(0, 0, MenuWindow.width, 20));
                        Print.Menu.Checkbox("Draw ESP", ref MenuESPVisible);
                        Print.Menu.Checkbox("Draw Items", ref MenuItemsVisible);
                        Print.Menu.Checkbox("Draw World", ref MenuWorldVisible);
                        Print.Menu.Checkbox("Draw Player", ref MenuPlayerVisible);
#if Private
                        Print.Menu.Checkbox("Draw Debug", ref MenuDebugVisible);
#endif
                        break;
                    default:
                        break;
                }
            }
            catch { }

        }
        void DrawESP(int id)
        {
            try
            {
                switch (id)
                {
                    case 1:
                        GUI.DragWindow(new Rect(0, 0, MenuESPWindow.width, 20));
                        Print.Menu.Checkbox("Draw Informations", ref Settings.CPlayer.bDrawInformation);
                        Print.Menu.Checkbox("Draw Lines", ref Settings.CPlayer.bDrawInformation);
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CPlayer.MaxInfoDistance, 0f, 2000f, "Max Info Distance");
                        Print.Menu.Checkbox("Draw Skeleton", ref Settings.CPlayer.bDrawSkeleton);
                        Print.Menu.Checkbox("Draw 2DBox", ref Settings.CPlayer.bDraw2DBox);
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CPlayer.MaxDrawDistance, 0f, 2000f, "Max Draw Distance");

                        break;
                    default:
                        break;
                }
            }
            catch { }

        }
        void DrawItems(int id)
        {
            try
            {
                switch (id)
                {
                    case 2:
                        GUI.DragWindow(new Rect(0, 0, MenuItemsWindow.width, 20));
                        Print.Menu.Checkbox("Draw Items", ref Settings.CItems.bItemESP);
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CItems.itemPrice, 0f, 150000f, "Price Filter");
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CItems.itemDistance, 0f, 2000f, "Distance Filter");
                        //Category
                        Print.Menu.Checkbox("Draw corpses", ref Settings.CItems.bCorpseESP);
                        Print.Menu.Checkbox("Draw grenades", ref Settings.CItems.bGrenadesESP);
                        Print.Menu.Checkbox("Draw Container", ref Settings.CItems.bLootableContainer);
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CItems.containerPrice, 0f, 200000f, "Container Price Filter");
                        break;
                    default:
                        break;
                }
            }
            catch { }

        }
        void DrawWorld(int id)
        {
            try
            {
                switch (id)
                {
                    case 3:
                        GUI.DragWindow(new Rect(0, 0, MenuWorldWindow.width, 20));
                        Print.Menu.Checkbox("Extract Points", ref Settings.CExtract.bExtract);
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CExtract.ExtractDistance, 0f, 2000f, "Extract Points Distance");
                        Print.Menu.Checkbox("Door Unlocker", ref Settings.CDoors.bDoorUnlocker);
                        Print.Menu.Checkbox("Night Vision", ref Settings.CThermal.bThermal);
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }
        void DrawPlayer(int id)
        {
            try
            {
                switch (id)
                {
                    case 4:
                        GUI.DragWindow(new Rect(0, 0, MenuPlayerWindow.width, 20));
                        Print.Menu.Checkbox("NoRecoil", ref Settings.CPlayer.bNoRecoil);
                        Print.Menu.Checkbox("Max Skills", ref Settings.CPlayer.bMaxSkill);
                        //CPlayer.bWallBang = GUILayout.Toggle(CPlayer.bWallBang, "WallBang");
                        Print.Menu.Checkbox("SpeedHack", ref Settings.CSpeedHack.bSpeedHack);
                        Print.Menu.Slider.Horizontal.Float(ref Settings.CSpeedHack.speedMultipler, 1f, 5f, "SpeedHack mutiplier");
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }
        void DrawAimbot(int id)
        {
            try
            {
                switch (id)
                {
                    case 5:
                        Print.Menu.Checkbox("AimBot On/Off", ref Settings.CAimbot.bAimbot);
                        Print.Menu.Label("Aimbot Key :");
                        if (GUILayout.Button(Settings.CAimbot.selectedKey))
                        {
                            editing = !editing;
                        }
                        if (editing)
                        {
                            MenuAimbotWindow = new Rect(1060, 10, 200, 340);
                            for (int x = 0; x < Settings.CAimbot.aimbotKeys.Length; x++)
                            {
                                if (GUILayout.Button(Settings.CAimbot.aimbotKeys[x]))
                                {
                                    Settings.CAimbot.selectedKey = Settings.CAimbot.aimbotKeys[x];
                                    MenuAimbotWindow = new Rect(1060, 10, 200, 250);
                                    editing = false;
                                }
                            }
                        }

                        Print.Menu.Checkbox("IsVisible", ref Settings.CAimbot.playerVisible);
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CAimbot.aimFov, 5f, 20f, "Aimbot FOV");
                        Print.Menu.Slider.Horizontal.Int(ref Settings.CAimbot.aimbotDistance, 0f, 300f, "Aimbot Distance");
                        break;
                    default:
                        break;
                }
            } catch { }
        }
        #if Private
        void DrawDebug(int id)
{
    try
    {
        switch (id)
        {
            case 6:
                GUI.DragWindow(new Rect(0, 0, MenuDebugWindow.width, 20));
                Print.Menu.Label("IsPlaying = ", CGameWorld.IsPlaying.ToString());
                Print.Menu.Label("Aimbot Debug : ", Settings.CAimbot.aimDebugger);
                Print.Menu.Label("Door Debug : ", CDoors.DoorLogger);
                GUILayout.Label("No BattlEye Bypass found :)");

                break;
            default:
                break;
        }
    }
    catch { }
}
        #endif
    }
}