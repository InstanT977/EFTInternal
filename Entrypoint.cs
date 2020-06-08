using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using Boo.Lang;
using EFT;
using EscapeFromGod.Functions;
using System.Windows.Forms;

namespace EscapeFromGod
{
    public static class Entrypoint
    {
        unsafe static void Main()
        {
            Logs.Add("Application Version : " + UnityEngine.Application.version, Logs.logLevelEnum.Debug);

            try
            {
                GameObject main = new GameObject();
                main.AddComponent<Menu>();
                GameObject.DontDestroyOnLoad(main);
            }
            catch 
            {
                Logs.Add("Coudn't proprely inject into the game (Error during Setup)", Logs.logLevelEnum.Critical);
                Environment.Exit(-1);
            }
        }
    }
}
