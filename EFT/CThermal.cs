using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeFromGod
{
    public class CThermal : MonoBehaviour
    {
        private static bool isset;

        public static void ThermalVision()
        {
            if (Settings.CThermal.bThermal && !isset)
            {
                CGameWorld.MainCamera.GetComponent<ThermalVision>().On = true;
                CGameWorld.MainCamera.GetComponent<ThermalVision>().enabled = true;
                isset = true;
                return;
            }
            if (!Settings.CThermal.bThermal && isset)
            {
                CGameWorld.MainCamera.GetComponent<ThermalVision>().On = false;
                CGameWorld.MainCamera.GetComponent<ThermalVision>().enabled = true;
                isset = false;
            }
        }
        void Update()
        {
            if (!CGameWorld.IsPlaying)
                return;
            ThermalVision();
        }
    }
}
