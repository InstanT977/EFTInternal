using EFT;
using EscapeFromGod.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EscapeFromGod
{
    public class CAimbot : MonoBehaviour
    {
        private static Dictionary<float, Player> AimbotPlayer;
        private static RaycastHit raycastHit;
        private static GamePlayer Target;
        private static int temp_int;
        private static KeyCode keycodeAimbot = KeyCode.Mouse3;

        public void Update()
        {
            #region KeyCode Switcher
            temp_int = Array.IndexOf(Settings.CAimbot.aimbotKeys, Settings.CAimbot.selectedKey);
            if (temp_int == 0)
                keycodeAimbot = KeyCode.Mouse0;
            else if (temp_int == 1)
                keycodeAimbot = KeyCode.Mouse3;
            else if (temp_int == 2)
                keycodeAimbot = KeyCode.Mouse4;
            else if (temp_int == 3)
                keycodeAimbot = KeyCode.Mouse5;
            else if (temp_int == 4)
                keycodeAimbot = KeyCode.Mouse6;
            else
                Logs.Add("No Key setup for Aimbot!", Logs.logLevelEnum.High);

            #endregion

            if (CGameWorld.IsPlaying && Settings.CAimbot.bAimbot)
            {
                Aimbot_Method();
            }
        }

        public void Aimbot_Method()
        {
            if (Input.GetKey(keycodeAimbot))
            {
                AimbotPlayer = new Dictionary<float, Player>();
                foreach (Player player in CGameWorld.activeGameworld.RegisteredPlayers)
                {
                    if (!(player == null) && !(player == CPlayer.LocalPlayer) && player.HealthController != null && player.HealthController.IsAlive)
                    {
                        var p = new GamePlayer(player);
                        if (p.NextBone != Vector3.zero && p.Fov <= Settings.CAimbot.aimFov && p.Distance > 3f)
                        {
                            if (Settings.CAimbot.playerVisible)
                            {
                                if (IsVisible(player.gameObject, p.NextBone) && !AimbotPlayer.ContainsKey(p.Distance))
                                    AimbotPlayer.Add(p.Distance, player);
                                else
                                    return;
                            }
                            else if (!AimbotPlayer.ContainsKey(p.Distance))
                                AimbotPlayer.Add(p.Distance, player);
                        }
                    }

                }
                if (AimbotPlayer.Count > 0)
                    FilterPlayer();
            }
        }

        private bool IsVisible(GameObject obj, Vector3 Position)
        {
            return Physics.Linecast(GetShootPos(), Position, out raycastHit) && raycastHit.collider && raycastHit.collider.gameObject.transform.root.gameObject == obj.transform.root.gameObject;
        }

        public static Vector3 GetShootPos()
        {
            if (CPlayer.LocalPlayer == null)
            {
                return Vector3.zero;
            }
            Player.FirearmController firearmController = CPlayer.LocalPlayer.HandsController as Player.FirearmController;
            if (firearmController == null)
            {
                return Vector3.zero;
            }
            return firearmController.Fireport.position + Camera.main.transform.forward * 1f;
        }
        public void FilterPlayer()
        {
            Target = new GamePlayer(AimbotPlayer[AimbotPlayer.Keys.Min()]);
            AimAtPos(Target.NextBone, Target.Distance, Target.Veloctiy);
        }
        public void AimAtPos(Vector3 pos, float distance, Vector3 Velocity)
        {
            Vector3 b = GetShootPos();
            float num = distance / CPlayer.LocalPlayer.Weapon.CurrentAmmoTemplate.InitialSpeed;
            pos.x += Velocity.x * num;
            pos.y += Velocity.y * num;
            Vector3 eulerAngles = Quaternion.LookRotation((pos - b).normalized).eulerAngles;
            if (eulerAngles.x > 180f)
            {
                eulerAngles.x -= 360f;
            }
            CPlayer.LocalPlayer.MovementContext.Rotation = new Vector2(eulerAngles.y, eulerAngles.x);
        }
    }
}

