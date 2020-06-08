using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EFT;
using System.Runtime.InteropServices;
using System.Reflection;
using EFT.InventoryLogic;
using EscapeFromGod.Functions;
using Mono.Posix;

namespace EscapeFromGod
{
    public class CPlayer : MonoBehaviour
    {
        #region MenuSettings / Vars
        private static class D
        {
            public static Vector3 w2s;
            public static int PlayerWorth;
            public static Dictionary<EHumanBones, Vector3> bones;
            public static List<Transform> boneList;
            public static Vector3 vectorA = Vector3.zero;
            public static Vector3 vectorB = Vector3.zero;
            public static Vector3 vectorC = Vector3.zero;
            public static Vector3[] w2sl = new Vector3[8];
            public static Vector3[] bounds = new Vector3[8];
            public static Vector2[] box2D = new Vector2[4];
        }
        private static bool bSkillDump;
        private Dictionary<EHumanBones, Vector3> playerBones;
        private static Dictionary<ESkillId, int> LocalSkills = new Dictionary<ESkillId, int>();
        private static IEnumerator<EFT.InventoryLogic.Item> PlayerItems;
        private static IEnumerator<EFT.InventoryLogic.Item> PlayerContainer;

        public static List<Player> Players = new List<Player>();
        public static Dictionary<Player, bool> PlayersBot = new Dictionary<Player, bool>();
        public static Dictionary<Player, string> PlayersName = new Dictionary<Player, string>();
        public static Dictionary<Player, string> PlayersGun = new Dictionary<Player, string>();
        public static Dictionary<Player, float> PlayersDis = new Dictionary<Player, float>();
        public static Dictionary<Player, int> PlayersWorth = new Dictionary<Player, int>();
        public static Player LocalPlayer = null;
        public static Vector2 vec2DrawLine = new Vector2(Screen.width / 2, Screen.height);

        #endregion


        public void Update()
        {
            if (!CGameWorld.IsPlaying || !Settings.CPlayer.bDraw)
                return;

            //filter player
            try
            {
                Players = CGameWorld.activeGameworld.RegisteredPlayers;
            }
            catch { return; }
            foreach (Player player in Players)
            {
                if (!player)
                    continue;
                if (player.PointOfView == EPointOfView.FirstPerson && LocalPlayer != player)
                {
                    LocalPlayer = player;
                    continue;
                }
                try
                {
                    PlayerItems = player.Profile.Inventory.AllPlayerItems.GetEnumerator();
                    EFT.InventoryLogic.EquipmentSlot[] Inventory = new EFT.InventoryLogic.EquipmentSlot[] { (EFT.InventoryLogic.EquipmentSlot)5 };
                    PlayerContainer = player.Profile.Inventory.GetItemsInSlots(Inventory).GetEnumerator();
                    while (PlayerItems.MoveNext())
                    {
                        D.PlayerWorth += PlayerItems.Current.Template.CreditsPrice;
                    }
                    while (PlayerContainer.MoveNext())
                    {
                        D.PlayerWorth -= PlayerItems.Current.Template.CreditsPrice;
                    }
                }
                catch { }
                PlayersWorth[player] = (D.PlayerWorth / 1000);
                PlayersDis[player] = (float)Math.Floor(Vector3.Distance(player.Transform.position, CGameWorld.MainCamera.transform.position));
                PlayersBot[player] = player.Profile.Info.RegistrationDate <= 100;
                PlayersName[player] = PlayersBot[player] ? player.Profile.Info.Settings.Role.ToString("F") : player.Profile.Info.Nickname;
                PlayersGun[player] = player.HandsController.Item.ShortName.Localized();
                D.PlayerWorth = 0;
            }
            if (LocalPlayer == null)
                return;
            NoRecoil();
            MaxStats();
        }
        private void MaxStats()
        {
            if (!bSkillDump)
            {
                foreach (GClass1060 gclass in CPlayer.LocalPlayer.Skills.Skills)
                {
                    LocalSkills.Add(gclass.Id, gclass.Level);
                }
                bSkillDump = true;
            }
            if (CPlayer.LocalPlayer != null && CGameWorld.IsPlaying)
            {
                foreach (GClass1060 gclass in CPlayer.LocalPlayer.Skills.Skills)
                {
                    if ((gclass.Id == ESkillId.Strength || gclass.Id == ESkillId.Endurance || gclass.Id == ESkillId.BotReload) && Settings.CPlayer.bMaxSkill)
                    {
                        gclass.SetLevel(50);
                    }
                    else if ((gclass.Id == ESkillId.Strength || gclass.Id == ESkillId.Endurance || gclass.Id == ESkillId.BotReload) && !Settings.CPlayer.bMaxSkill)
                    {
                        gclass.SetLevel(LocalSkills[gclass.Id]);
                    }
                }
            }
        }
        private void NoRecoil()
        {
            if (Settings.CPlayer.bNoRecoil)
            {
                LocalPlayer.ProceduralWeaponAnimation.Shootingg.Intensity = 0;
                LocalPlayer.ProceduralWeaponAnimation.Shootingg.RecoilStrengthXy = new Vector2(0, 0);
                LocalPlayer.ProceduralWeaponAnimation.Shootingg.RecoilStrengthZ = new Vector2(0, 0);
                LocalPlayer.ProceduralWeaponAnimation.WalkEffectorEnabled = false;
                LocalPlayer.ProceduralWeaponAnimation._shouldMoveWeaponCloser = false;
                LocalPlayer.ProceduralWeaponAnimation.MotionReact.SwayFactors.x = 0;
                LocalPlayer.ProceduralWeaponAnimation.MotionReact.SwayFactors.y = 0;
                LocalPlayer.ProceduralWeaponAnimation.MotionReact.SwayFactors.z = 0;
                LocalPlayer.ProceduralWeaponAnimation.Breath.Intensity = 0;
                LocalPlayer.ProceduralWeaponAnimation.Walk.Intensity = 0;
                LocalPlayer.ProceduralWeaponAnimation.Shootingg.Stiffness = 0;
                LocalPlayer.ProceduralWeaponAnimation.MotionReact.Intensity = 0;
                LocalPlayer.ProceduralWeaponAnimation.ForceReact.Intensity = 0;
                return;
            }
        }
        public void OnGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (!CGameWorld.IsPlaying || !Settings.CPlayer.bDraw)
                return;
            try
            {
                foreach (Player player in Players)
                {
                    if (!player)
                        continue;
                    D.w2s = CGameWorld.MainCamera.WorldToScreenPoint(player.Transform.position);
                    if (D.w2s.z < 0.01f)
                        continue;
                    D.w2s.y = NamelessScreen.screenHeight - D.w2s.y;
                    if (Settings.CPlayer.MaxInfoDistance < PlayersDis[player])
                        continue;
                    if (Settings.CPlayer.bDrawInformation)
                    {
                        if (PlayersBot[player] && player.Profile.Info.Settings.IsBoss())
                            Print.DrawFont(new Vector2(D.w2s.x, D.w2s.y), $"BOSS - {PlayersName[player]}[{PlayersDis[player]}]m \n {PlayersGun[player]}", Settings.CPlayer.bossColor);
                        else if (PlayersBot[player])
                            Print.DrawFont(new Vector2(D.w2s.x, D.w2s.y), $"{PlayersName[player]}[{PlayersDis[player]}m] <{PlayersWorth[player]}k> \n {PlayersGun[player]}", Settings.CPlayer.botColor);
                        else
                            Print.DrawFont(new Vector2(D.w2s.x, D.w2s.y), $"{PlayersName[player]}[{PlayersDis[player]}m] <{PlayersWorth[player]}k> \n {PlayersGun[player]} : {player.Profile.Info.Level}", Settings.CPlayer.playerColor);
                    }
                    if (Settings.CPlayer.bDrawLines && PlayersDis[player] < Settings.CPlayer.MaxInfoDistance)
                        DrawLine(player, vec2DrawLine);
                    if ((Settings.CPlayer.bDraw2DBox || Settings.CPlayer.bDrawSkeleton) && PlayersDis[player] < Settings.CPlayer.MaxDrawDistance)
                    {
                        playerBones = GetBones(player);
                        if (playerBones.Count != 0)
                        {
                            if (Settings.CPlayer.bDraw2DBox)
                                DrawBoxes(player);
                            if (Settings.CPlayer.bDrawSkeleton)
                                DrawSkeleton(playerBones);
                        }
                    }
                }
            }
            catch { }

        }
        public static Dictionary<EHumanBones, Vector3> GetBones(Player player)
        {
            D.bones = new Dictionary<EHumanBones, Vector3>();
            if (!player || player.PlayerBody == null || player.PlayerBody.SkeletonRootJoint == null)
                return D.bones;
            D.boneList = player.PlayerBody.SkeletonRootJoint.Bones.Values.ToList();
            if (D.boneList.Count == 0)
                return D.bones;
            if (CGameWorld.MainCamera == null)
                return D.bones;

            for (int i = 0; i < D.boneList.Count; i++)
            {
                Transform bone = D.boneList[i];
                if (bone == null || !neededBones.Contains((EHumanBones)i))
                    continue;
                D.w2s = CGameWorld.MainCamera.WorldToScreenPoint(bone.position);
                D.w2s.y = NamelessScreen.screenHeight - D.w2s.y;
                D.bones[(EHumanBones)i] = D.w2s;
            }
            return D.bones;
        }

        private static List<EHumanBones> neededBones = new List<EHumanBones>()
        {
            EHumanBones.HumanPelvis,EHumanBones.HumanLThigh1,EHumanBones.HumanLThigh2,EHumanBones.HumanLCalf,EHumanBones.HumanLFoot,EHumanBones.HumanLToe,
            EHumanBones.HumanPelvis,EHumanBones.HumanRThigh1,EHumanBones.HumanRThigh2,EHumanBones.HumanRCalf,EHumanBones.HumanRFoot,EHumanBones.HumanRToe,
            EHumanBones.HumanSpine1,EHumanBones.HumanSpine2,EHumanBones.HumanSpine3,EHumanBones.HumanNeck,EHumanBones.HumanHead,
            EHumanBones.HumanLCollarbone,EHumanBones.HumanLForearm1,EHumanBones.HumanLForearm2,EHumanBones.HumanLForearm3,EHumanBones.HumanLPalm,EHumanBones.HumanLDigit11,EHumanBones.HumanLDigit12,EHumanBones.HumanLDigit13,
            EHumanBones.HumanRCollarbone,EHumanBones.HumanRForearm1,EHumanBones.HumanRForearm2,EHumanBones.HumanRForearm3,EHumanBones.HumanRPalm,EHumanBones.HumanRDigit11,EHumanBones.HumanRDigit12,EHumanBones.HumanRDigit13
        };

        private void DrawSkeleton(Dictionary<EHumanBones, Vector3> bones)
        {
            if (bones.Count == 0)
                return;
            //Lower-Left-Body
            ConnectBones(bones, EHumanBones.HumanPelvis, EHumanBones.HumanLThigh1);
            ConnectBones(bones, EHumanBones.HumanLThigh1, EHumanBones.HumanLThigh2);
            ConnectBones(bones, EHumanBones.HumanLThigh2, EHumanBones.HumanLCalf);
            ConnectBones(bones, EHumanBones.HumanLCalf, EHumanBones.HumanLFoot);
            ConnectBones(bones, EHumanBones.HumanLFoot, EHumanBones.HumanLToe);
            //Lower-Right-Body
            ConnectBones(bones, EHumanBones.HumanPelvis, EHumanBones.HumanRThigh1);
            ConnectBones(bones, EHumanBones.HumanRThigh1, EHumanBones.HumanRThigh2);
            ConnectBones(bones, EHumanBones.HumanRThigh2, EHumanBones.HumanRCalf);
            ConnectBones(bones, EHumanBones.HumanRCalf, EHumanBones.HumanRFoot);
            ConnectBones(bones, EHumanBones.HumanRFoot, EHumanBones.HumanRToe);
            //Body
            ConnectBones(bones, EHumanBones.HumanPelvis, EHumanBones.HumanSpine1);
            ConnectBones(bones, EHumanBones.HumanSpine1, EHumanBones.HumanSpine2);
            ConnectBones(bones, EHumanBones.HumanSpine2, EHumanBones.HumanSpine3);
            ConnectBones(bones, EHumanBones.HumanSpine3, EHumanBones.HumanNeck);
            ConnectBones(bones, EHumanBones.HumanNeck, EHumanBones.HumanHead);
            //Arms
            //Left
            ConnectBones(bones, EHumanBones.HumanSpine3, EHumanBones.HumanLCollarbone);
            ConnectBones(bones, EHumanBones.HumanLCollarbone, EHumanBones.HumanLForearm1);
            ConnectBones(bones, EHumanBones.HumanLForearm1, EHumanBones.HumanLForearm2);
            ConnectBones(bones, EHumanBones.HumanLForearm2, EHumanBones.HumanLForearm3);
            ConnectBones(bones, EHumanBones.HumanLForearm3, EHumanBones.HumanLPalm);
            ConnectBones(bones, EHumanBones.HumanLPalm, EHumanBones.HumanLDigit11);
            ConnectBones(bones, EHumanBones.HumanLDigit11, EHumanBones.HumanLDigit12);
            ConnectBones(bones, EHumanBones.HumanLDigit12, EHumanBones.HumanLDigit13);

            //Right
            ConnectBones(bones, EHumanBones.HumanSpine3, EHumanBones.HumanRCollarbone);
            ConnectBones(bones, EHumanBones.HumanRCollarbone, EHumanBones.HumanRForearm1);
            ConnectBones(bones, EHumanBones.HumanRForearm1, EHumanBones.HumanRForearm2);
            ConnectBones(bones, EHumanBones.HumanRForearm2, EHumanBones.HumanRForearm3);
            ConnectBones(bones, EHumanBones.HumanRForearm3, EHumanBones.HumanRPalm);
            ConnectBones(bones, EHumanBones.HumanRPalm, EHumanBones.HumanRDigit11);
            ConnectBones(bones, EHumanBones.HumanRDigit11, EHumanBones.HumanRDigit12);
            ConnectBones(bones, EHumanBones.HumanRDigit12, EHumanBones.HumanRDigit13);
        }
        private static void ConnectBones(Dictionary<EHumanBones, Vector3> bones, EHumanBones start, EHumanBones stop)
        {
            if (!bones.ContainsKey(start) || !bones.ContainsKey(stop))
                return;
            D.vectorA = bones[start];
            if (D.vectorA.z < 0.01f)
                return;
            D.vectorB = bones[stop];
            if (D.vectorB.z < 0.01f)
                return;
            Print.DrawLine(D.vectorA, D.vectorB, 1f, Settings.CPlayer.skeletonColor);
        }
        private void DrawBoxes(Player player)
        {
            D.boneList = player.PlayerBody.SkeletonRootJoint.Bones.Values.ToList();
            if (D.boneList.Count == 0)
                return;
            D.vectorA = D.boneList[0].position;
            D.vectorB = D.vectorA;
            D.boneList.RemoveAt(0);
            D.vectorC = Vector3.zero;
            for (int i = 0; i < D.boneList.Count; i++)
            {
                Transform bone = D.boneList[i];
                if (bone == null || !neededBones.Contains((EHumanBones)i))
                    continue;
                D.vectorC = bone.position;
                if (D.vectorA.x > D.vectorC.x)
                    D.vectorA.x = D.vectorC.x;
                if (D.vectorA.y > D.vectorC.y)
                    D.vectorA.y = D.vectorC.y;
                if (D.vectorA.z > D.vectorC.z)
                    D.vectorA.z = D.vectorC.z;

                if (D.vectorB.x < D.vectorC.x)
                    D.vectorB.x = D.vectorC.x;
                if (D.vectorB.y < D.vectorC.y)
                    D.vectorB.y = D.vectorC.y;
                if (D.vectorB.z < D.vectorC.z)
                    D.vectorB.z = D.vectorC.z;
            }
            D.bounds = new Vector3[8]
            {
                //Lower parts
                new Vector3(D.vectorA.x, D.vectorA.y, D.vectorA.z), //LB
                new Vector3(D.vectorB.x, D.vectorA.y, D.vectorA.z), //RB
                new Vector3(D.vectorA.x, D.vectorA.y, D.vectorB.z), //LF
                new Vector3(D.vectorB.x, D.vectorA.y, D.vectorB.z), //RF
                //Upper parts
                new Vector3(D.vectorA.x, D.vectorB.y, D.vectorA.z), //LB
                new Vector3(D.vectorB.x, D.vectorB.y, D.vectorA.z), //RB
                new Vector3(D.vectorA.x, D.vectorB.y, D.vectorB.z), //LF
                new Vector3(D.vectorB.x, D.vectorB.y, D.vectorB.z), //RF
            };
            D.w2sl = new Vector3[8];
            for (int i = 0; i < 8; i++)
            {
                D.w2sl[i] = CGameWorld.MainCamera.WorldToScreenPoint(D.bounds[i]);
                D.w2sl[i].y = NamelessScreen.screenHeight - D.w2sl[i].y;
            }
            if (Settings.CPlayer.bDraw2DBox)
            {
                //Draw2DBox

                D.vectorA = D.w2sl[0];
                D.vectorB = D.vectorA;
                for (int i = 1; i < 8; i++)
                {
                    if (D.w2sl[i].z < 0.01f)
                        continue;
                    if (D.vectorA.x > D.w2sl[i].x)
                        D.vectorA.x = D.w2sl[i].x;
                    if (D.vectorA.y > D.w2sl[i].y)
                        D.vectorA.y = D.w2sl[i].y;
                    if (D.vectorB.x < D.w2sl[i].x)
                        D.vectorB.x = D.w2sl[i].x;
                    if (D.vectorB.y < D.w2sl[i].y)
                        D.vectorB.y = D.w2sl[i].y;
                }
                D.box2D = new Vector2[4]
                {
                    new Vector3(D.vectorA.x, D.vectorA.y),
                    new Vector3(D.vectorB.x, D.vectorA.y),
                    new Vector3(D.vectorA.x, D.vectorB.y),
                    new Vector3(D.vectorB.x, D.vectorB.y)
                };
                Print.DrawLine(D.box2D[0], D.box2D[1], 2f, Settings.CPlayer.BoxColor);
                Print.DrawLine(D.box2D[0], D.box2D[2], 2f, Settings.CPlayer.BoxColor);
                Print.DrawLine(D.box2D[2], D.box2D[3], 2f, Settings.CPlayer.BoxColor);
                Print.DrawLine(D.box2D[3], D.box2D[1], 2f, Settings.CPlayer.BoxColor);
            }
        }
        private void DrawLine(Player player, Vector2 screen)
        {
            Vector3 w2s = Camera.main.WorldToScreenPoint(player.PlayerBones.RootJoint.position);
            if (w2s.z < 0.01f)
                return;
            var distance = (float)Math.Floor(Vector3.Distance(player.Transform.position, CGameWorld.MainCamera.transform.position));
            Print.DrawLine(screen, new Vector2(w2s.x, Screen.height - w2s.y), 2f, Settings.CPlayer.linesColor);
        }
    }
}