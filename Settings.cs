

using UnityEngine;

namespace EscapeFromGod
{
    public static class Settings
    {
        public static class CAimbot
        {
            public static bool bAimbot = true;
            public static bool playerVisible = false;
            public static string aimDebugger;
            public static int aimFov = 10;
            public static int aimbotDistance = 200;
            public static Color fovDrawColor = new Color(1f, 1f, 1f, 0.5f);

            public static string[] aimbotKeys = new string[5] {"Left Mouse", "Mouse3", "Mouse4", "Mouse5", "Mouse6"};
            public static string selectedKey = "Mouse4";
        }
        public static class CItems
        {
            public static int itemPrice = 50000;
            public static int itemDistance = 200;
            public static int containerPrice = 50000;
            public static bool bItemESP = true;
            public static bool bGrenadesESP = true;
            public static bool bCorpseESP = true;
            public static bool bLootableContainer = true;
            public static Color grenadeColor = Color.red;
        }
        public static class CThermal
        {
            public static bool bThermal = false;
        }
        public static class CSpeedHack
        {
            public static float speedMultipler = 1.7f;
            public static bool bSpeedHack = false;
        }
        public static class CPlayer
        {
            public static int MaxInfoDistance = 1000;
            public static int MaxDrawDistance = 1000;
            public static bool bDraw = true;

            public static bool bDrawLines = true;
            public static Print.colorCodeEnum linesColor = Print.colorCodeEnum.Yellow;

            public static bool bDrawInformation = true;
            public static Print.colorCodeEnum bossColor = Print.colorCodeEnum.Purple;
            public static Print.colorCodeEnum botColor = Print.colorCodeEnum.Blue;
            public static Print.colorCodeEnum playerColor = Print.colorCodeEnum.Cyan;

            public static bool bDraw2DBox = false;
            public static Print.colorCodeEnum BoxColor = Print.colorCodeEnum.Red;

            public static bool bDrawSkeleton = true;
            public static Print.colorCodeEnum skeletonColor = Print.colorCodeEnum.Purple;

            public static bool bNoRecoil = true;
            public static bool bMaxSkill = false;
        }
        public static class CExtract
        {
            public static bool bExtract = true;
            public static int ExtractDistance = 750;
        }
        public static class CDoors
        {
            public static bool bDoorUnlocker = false;
            public static string[] unlockerKeys = new string[5] { "Left Mouse", "Mouse3", "Mouse4", "Mouse5", "Mouse6" };
        }
    }
}
