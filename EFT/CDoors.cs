using EFT;
using EFT.Interactive;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EscapeFromGod
{
    class CDoors : MonoBehaviour
    {
        public static string DoorLogger = "";

        private static IEnumerator<EFT.InventoryLogic.Item> PlayerItems;
        private static IEnumerator<WorldInteractiveObject> Objects;
        private static EFT.InventoryLogic.Item PlayerKey;
        private float distance;
        private readonly float safedistance = 4f;
        private static bool SettingKey;
        private string DoorKeyId = "";
        private string PlayerKeyId = "";

        void Update()
        {
            if (CGameWorld.IsPlaying)
            {
                Objects = LocationScene.GetAllObjects<WorldInteractiveObject>(false).GetEnumerator();
            }
            if (CGameWorld.IsPlaying && Settings.CDoors.bDoorUnlocker && Input.GetKeyDown(KeyCode.Mouse4))
                UnlockDoor();
        }
        void SetMechanicalKeys()
        {
            //you're gonna have to figure that one out :)
        }
        private IEnumerator RevokeMechanicalKeys(float time)
        {
            yield return new WaitForSeconds(time);
            PlayerKey.Template._id = PlayerKeyId;
            PlayerKey = null;
            DoorKeyId = null;
            SettingKey = false;
            DoorLogger = "RevokedKey";
        }

        void GetDoorId()
        {
            while (Objects.MoveNext())
            {
                if (Objects.Current is Door)
                {
                    distance = (float)Math.Floor(Vector3.Distance(Objects.Current.transform.position, CGameWorld.MainCamera.transform.position));
                    if (distance <= safedistance)
                    {
                        if (Objects.Current.KeyId.Length > 0)
                        {
                            DoorLogger = "Found a locked door";
                            DoorKeyId = Objects.Current.KeyId;
                        }
                    }


                }
            }
        }
        void UnlockDoor()
        {
            if (!SettingKey)
            {
                DoorLogger = "Started UnlockDoor()";
                if (DoorKeyId == null)
                    GetDoorId();
                SetMechanicalKeys();
                SettingKey = true;
            }
        }
    }
}
