using EFT.Interactive;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EscapeFromGod
{
    class CItems : MonoBehaviour
    {
        private static List<Throwable>.Enumerator Grenade;
        private static List<LootItem>.Enumerator GameLootItemList;
        private static List<LootItem>.Enumerator CorpseList;
        private static IEnumerator<WorldInteractiveObject> Objects;
        private static Dictionary<Vector3, float> LootableContainerWorth;

        private Vector3 w2s;
        private Vector3 w2s2;
        private Vector3 w2s3;
        private Vector3 w2s4;
        private float distance;
        private float distance2;
        private float distance3;
        private float LootableWorth;

        void Update()
        {
            if (CGameWorld.IsPlaying)
            {
                GameLootItemList = CGameWorld.activeGameworld.LootItems.GetValuesEnumerator().GetEnumerator();
                CorpseList = CGameWorld.activeGameworld.LootItems.GetValuesEnumerator().GetEnumerator();
                Grenade = CGameWorld.activeGameworld.Grenades.GetValuesEnumerator().GetEnumerator();
                Objects = LocationScene.GetAllObjects<WorldInteractiveObject>(false).GetEnumerator();
            }
        }
        void OnGUI()
        {
            if (Event.current.type != EventType.Repaint)
                return;
            if (CGameWorld.IsPlaying && Settings.CItems.bItemESP)
                Items();
            if (CGameWorld.IsPlaying && Settings.CItems.bGrenadesESP)
                Grenades();
            if (CGameWorld.IsPlaying && Settings.CItems.bCorpseESP)
                Corpses();
            if (CGameWorld.IsPlaying && Settings.CItems.bLootableContainer)
                LootableContainers();
        }
        void Grenades()
        {
            if (CGameWorld.activeGameworld.Grenades.Count > 0)
            {
                while (Grenade.MoveNext())
                {
                    w2s2 = CGameWorld.MainCamera.WorldToScreenPoint(Grenade.Current.transform.position);
                    w2s2.y = Screen.height - w2s2.y;
                    if (w2s2.z > 0.01f)
                    {
                        distance2 = (float)Math.Floor(Vector3.Distance(Grenade.Current.transform.position, CGameWorld.MainCamera.transform.position));
                        Print.DrawFont(w2s2, $"Grenade [{distance2}m]", Print.colorCodeEnum.Red);
                    }
                }
            }
        }
        void Items()
        {
            if (CGameWorld.activeGameworld.LootItems.Count > 0)
            {
                while (GameLootItemList.MoveNext())
                {
                    w2s = CGameWorld.MainCamera.WorldToScreenPoint(GameLootItemList.Current.transform.position);
                    w2s.y = Screen.height - w2s.y;
                    if (w2s.z > 0.01f)
                    {
                        distance = (float)Math.Floor(Vector3.Distance(GameLootItemList.Current.transform.position, CGameWorld.MainCamera.transform.position));
                        if (distance >= Settings.CItems.itemDistance)
                            continue;
                        if (GameLootItemList.Current.Item.Template.CreditsPrice >= Settings.CItems.itemPrice || GameLootItemList.Current.Item.QuestItem)
                        {
                            if (GameLootItemList.Current.Item.QuestItem)
                                Print.DrawFont(w2s, $"{GameLootItemList.Current.Item.ShortName.Localized()}[{distance}m] Quest", Print.colorCodeEnum.Blue);
                            else
                                Print.DrawFont(w2s, $"{GameLootItemList.Current.Item.ShortName.Localized()}[{distance}m] Price {GameLootItemList.Current.Item.Template.CreditsPrice.ToString()}", Print.colorCodeEnum.Yellow);
                        }
                    }
                }
            }
        }
        void SetLootableContainers()
        {
            if (LocationScene.GetAllObjects<WorldInteractiveObject>(false).Count() > 0)
            {
                LootableContainerWorth = new Dictionary<Vector3, float>();
                while (Objects.MoveNext())
                {
                    if (Objects.Current is LootableContainer)
                    {

                        LootableContainer t = Objects.Current as LootableContainer;
                        if (LootableContainerWorth.ContainsKey(t.transform.position))
                            return;
                        foreach (EFT.InventoryLogic.Item item in t.ItemOwner.RootItem.GetAllItems(false))
                        {
                            LootableWorth += item.Template.CreditsPrice;
                        }
                        LootableContainerWorth.Add(t.transform.position, (LootableWorth / 1000));
                        LootableWorth = 0f;
                    }
                }
            }
        }
        void Corpses()
        {
            if (CGameWorld.activeGameworld.LootItems.Count > 0)
            {
                while (CorpseList.MoveNext())
                {
                    w2s3 = CGameWorld.MainCamera.WorldToScreenPoint(CorpseList.Current.transform.position);
                    w2s3.y = Screen.height - w2s3.y;
                    if (w2s3.z > 0.01f)
                    {
                        if (CorpseList.Current is Corpse || CorpseList.Current is ObservedCorpse)
                        {
                            distance3 = (float)Math.Floor(Vector3.Distance(CorpseList.Current.transform.position, CGameWorld.MainCamera.transform.position));
                            Print.DrawFont(w2s3, $"Corpse at [{distance3}m]", Print.colorCodeEnum.Grey);
                        }
                    }
                }
            }
        }
        void LootableContainers()
        {
            SetLootableContainers();
            if (LootableContainerWorth.Count > 0)
            {
                var t = LootableContainerWorth.GetEnumerator();
                while (t.MoveNext())
                {
                    if (t.Current.Value < Settings.CItems.containerPrice)
                        return;
                    w2s4 = CGameWorld.MainCamera.WorldToScreenPoint(t.Current.Key);
                    w2s4.y = Screen.height - w2s4.y;
                    if (w2s4.z > 0.01f)
                    {
                        Print.DrawFont(w2s4, $"Lootable Container at [{(float)Math.Floor(Vector3.Distance(t.Current.Key, CGameWorld.MainCamera.transform.position))}m], <{Functions.Numerical.takeNDigits((int)t.Current.Value, 3)}k>", Print.colorCodeEnum.DarkGreen);
                    }
                }
            }
        }

    }
}
