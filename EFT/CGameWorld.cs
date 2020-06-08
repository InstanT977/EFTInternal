using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using EFT;
using UnityEngine.SceneManagement;

namespace EscapeFromGod
{
    public class CGameWorld : MonoBehaviour
    {
        public static Scene scene;
        public static GameWorld activeGameworld;
        public static bool IsPlaying = false;
        public static Camera MainCamera = new Camera();
        public static Vector2 wts = new Vector2();

        public static GameWorld FindActiveGameWorld()
        {
                return Comfort.Common.Singleton<GameWorld>.Instance;
        }

        void Update()
        {
            if (!activeGameworld)
                activeGameworld = FindActiveGameWorld();
            scene = SceneManager.GetActiveScene();
            if (activeGameworld &&
                scene.name != "EnvironmentUIScene" &&
                scene.name != "MenuUiScene" &&
                scene.name != "" &&
                scene.name != "MainScene" &&
                (activeGameworld.RegisteredPlayers.Count > 1
                || activeGameworld.AllLoot.Count > 1
                ) && (GClass1792.CheckCurrentScreen(EFT.UI.Screens.EScreenType.BattleUI)  ///GClass1792 for EmuTark
                || GClass1792.CheckCurrentScreen(EFT.UI.Screens.EScreenType.Inventory)
                ))
                IsPlaying = true;
            else if (GClass1792.CheckCurrentScreen(EFT.UI.Screens.EScreenType.ExitStatus))
                IsPlaying = false;
            else
            {
                IsPlaying = false;
            }
            MainCamera = Camera.main;
        }
    }
}