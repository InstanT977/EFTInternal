using UnityEngine;

namespace EscapeFromGod
{
    class CSpeedhack : MonoBehaviour
    {
        void Update()
        {
            if(CGameWorld.IsPlaying && Settings.CSpeedHack.bSpeedHack)
            {
                CPlayer.LocalPlayer.MovementContext.SpeedHack = Settings.CSpeedHack.speedMultipler;
            }
        }
    }
}
