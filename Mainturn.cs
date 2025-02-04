
using BepInEx;
using BepInEx.Configuration;
using ChiefOfImmortal;
using ChiefOfImmortal.Definition.DataStruct.Preset;
using ChiefOfImmortal.Definition.Enum;
using ChiefOfImmortal.RunData.DataBase;
using HarmonyLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Turnrestrictions
{
    [BepInPlugin("Turnrestrictions", "Acension", "0.9.25")]
    public class Mainturn : BaseUnityPlugin
    {
        private static PresetIData presetIData;
        private static int countnum = 0;
        private static ConfigEntry<int> 跳转显示;
        private static ConfigEntry<int> 地图大小;
        private static ConfigEntry<int> 物品添加;
        private static Harmony harmony;
        void Awake()
        {
            base.Logger.LogError("Turnrestrictions");
            Mainturn.跳转显示 = base.Config.Bind<int>("AcensionMod", "跳转显示", 1, "默认 1 开启 0 关闭");
            Mainturn.地图大小 = base.Config.Bind<int>("AcensionMod", "地图大小", 1, "地图大小，填写1、2、3、4");
            Mainturn.物品添加 = base.Config.Bind<int>("AcensionMod", "物品添加", 1, "物品添加默认 1 开启 0 关闭"); 
            bool flag = Mainturn.跳转显示.Value == 1;
            if (flag)
            {
                string url = "https://space.bilibili.com/2187348";
                Process.Start(new ProcessStartInfo(url)
                {
                    UseShellExecute = true
                });
            }
            Mainturn.harmony = Harmony.CreateAndPatchAll(typeof(Mainturn), null);
            base.Logger.LogError("Turnrestrictions OK！");
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(儿口), "丁丈夕")]
        public static bool End()
        {
            return false;
            //刁义 乙 = new 刁义
            //{
            //    入士勺 = 乙一.FormGameEnd,
            //    入士广 = 一
            //};
            //GameTimeProcessComponent.Update
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(儿及), "丁上一")]
        public static bool MapSize(ref MapSizeLevel 一)
        {
            switch (Mainturn.地图大小.Value)
            {
                case 1:
                    一 = MapSizeLevel.Small;
                    break;
                case 2:
                    一 = MapSizeLevel.Medium;
                    break;
                case 3:
                    一 = MapSizeLevel.Large;
                    break;
                case 4:
                    一 = MapSizeLevel.Super;
                    break;
            }
            return true;
            //FormSelectTheWorld.OnOpen
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(匕十), "丁乞九")]
        public static bool SetItem3(ref PresetItemYaml 一)
        {
            //SubUIInitialMember.OnOpen        PresetItemYaml 一
            countnum = 1;
            if (presetIData == null)
            {
                GetPIDIs();
            }
            if (presetIData.JadeSlipInfos != null)
            {
                foreach (PresetItemJadeSlipInfo t in presetIData.JadeSlipInfos)
                {
                    一.JadeSlipList.Add(t);
                }
            }
            if (presetIData.DanFormulaInfos != null)
            {
                foreach (PresetItemDanFormulaInfo t in presetIData.DanFormulaInfos)
                {
                    一.DanFormulaList.Add(t);
                }
            }
            if (presetIData.EquipFormulaInfos != null)
            {
                foreach (PresetItemEquipFormulaInfo t in presetIData.EquipFormulaInfos)
                {
                    一.EquipFormulaList.Add(t);
                }
            }
            Console.WriteLine("---------丁乞九----------");
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(入丸), "丁于三")]
        public static bool SetItem(ref SectData 一)
        {
            if (Mainturn.物品添加.Value == 1)
            {
                if (Mainturn.countnum > 0)
                {
                    Mainturn.countnum = 0;
                    if (presetIData.JadeSlipInfos != null)
                    {
                        foreach (PresetItemJadeSlipInfo t in presetIData.JadeSlipInfos)
                        {
                            ItemData item = ItemJadeSlipInfo.GetEntityByKeyID(t.PresetUniqueID);
                            SetToItem(item, 一);
                        }
                    }
                    if (presetIData.DanFormulaInfos != null)
                    {
                        foreach (PresetItemDanFormulaInfo t in presetIData.DanFormulaInfos)
                        {
                            ItemData item = ItemDanFormulaInfo.GetEntityByKeyID(t.PresetUniqueID);
                            SetToItem(item, 一);
                        }
                    }
                    if (presetIData.EquipFormulaInfos != null)
                    {
                        foreach (PresetItemEquipFormulaInfo t in presetIData.EquipFormulaInfos)
                        {
                            ItemData item = ItemEquipFormulaInfo.GetEntityByKeyID(t.PresetUniqueID);
                            SetToItem(item, 一);
                        }
                    }
                }
            }
            return true;
            //ConversationLuaHelper.GiveAward
            //eturn BaseDataCenter<入乙>.Instance.十弓又(entityByKeyID2.Sect, 乙, RecordSourceType.NormalConversation, list, -1);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(几一), "丁么二")]
        public static bool SetTheory(ref 几一 __instance)
        {
            if (Mainturn.物品添加.Value == 1)
            {
                List<TheoryMod> theoryMods = presetIData.TeoryMods;
                if (TheoryData.CountEntities > 0)
                {
                    return false;
                }
                DRTheory[] allDataRows = GameEntry.八刀九.GetDataTable<DRTheory>().GetAllDataRows();
                foreach (DRTheory 一 in allDataRows)
                {
                    TheoryMod theoryMod = theoryMods.FirstOrDefault(drt => drt.id == 一.Id);
                    if (theoryMod != null)
                    {
                        Traverse.Create(一).Property("八凡乙").SetValue(theoryMod.count);
                        Traverse.Create(一).Property("八凡十").SetValue(theoryMod.indexs);
                        Traverse.Create(一).Property("八凡七").SetValue(theoryMod.value);
                    }
                    __instance.丁么丁(一);
                }
                return false;
            }
            //ProcedureNewGam.OnEnter
            return true;
        }
        public static void GetPIDIs()
        {
            string filePath = @"BepInEx/Json/PresetIData.json";
            string json = File.ReadAllText(filePath);
            presetIData = JsonConvert.DeserializeObject<PresetIData>(json);
        }
        public static void SetToItem(ItemData item, SectData 一)
        {
            一.InventoryChangeItem(item, 1, true, -1, false, RecordSourceType.None, null);
            Console.WriteLine(item.Index + " " + item.UniqueID + " " + item.Name);
        }
    }
}
