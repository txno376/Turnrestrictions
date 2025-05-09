
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
using UnityGameFramework.Runtime;

namespace Turnrestrictions
{
    [BepInPlugin("Turnrestrictions", "Acension", "0.11.37")]
    public class Mainturn : BaseUnityPlugin
    {
        private static ConfigEntry<int> 跳转显示;
        private static ConfigEntry<int> 物品添加;
        private static Harmony harmony;
        void Awake()
        {
            base.Logger.LogError("Turnrestrictions");
            Mainturn.跳转显示 = base.Config.Bind<int>("AcensionMod", "跳转显示", 1, "默认 1 开启 0 关闭");
            Mainturn.物品添加 = base.Config.Bind<int>("AcensionMod", "物品添加", 1, "默认 1 开启 0 关闭"); 
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
        [HarmonyPatch(typeof(几又), "厂刁厂")]
        public static bool SetItem2(ref 几又 __instance)
        {
            //SubUIPresetEditorCharacter.OnClickAddANewPreset  PresetItemYaml 一
            List<TheoryMods> theoryMods = presetIData.TheoryMods;
            DataTableComponent dataTableComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<DataTableComponent>();
            foreach (TheoryMods theoryMods2 in theoryMods)
            {
                string dataRowString = string.Format("\t{0}\t魅力+5\t\tTheory_Dan\t1\t1\t0\t0\t1\tFalse\t255\t0\t1\t0\t\t\t{1}\t\t{2}\t{3}\t\t{4}\t\t2\t\t\t\t\t", new object[]
                {
                    theoryMods2.id,
                    theoryMods2.count,
                    theoryMods2.indexs,
                    theoryMods2.type,
                    theoryMods2.value
                });
                dataTableComponent.GetDataTable<DRTheory>().AddDataRow(dataRowString, new object());
            }

            if (Mainturn.物品添加.Value == 1)
            {
                List<List<PresetItemData>> list = (List<List<PresetItemData>>)Traverse.Create(__instance).Field("入己么").GetValue();
                PresetIData pd = presetIData;
                if (pd.JadeSlipInfos != null)
                {
                    foreach (PresetItemJadeSlipInfo t in pd.JadeSlipInfos)
                    {
                        list[(int)t.ItemType].Add(t);
                    }
                }
                if (pd.DanFormulaInfos != null)
                {
                    foreach (PresetItemDanFormulaInfo t in pd.DanFormulaInfos)
                    {
                        list[(int)t.ItemType].Add(t);
                    }
                }
                if (pd.EquipFormulaInfos != null)
                {
                    foreach (PresetItemEquipFormulaInfo t in pd.EquipFormulaInfos)
                    {
                        list[(int)t.ItemType].Add(t);
                    }
                }
                Traverse.Create(__instance).Field("入己么").SetValue(list); 
                Console.WriteLine("---------厂刁厂----------");
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(匕义), "厂厂弓")]
        public static bool SetItem(ref SectData 一)
        {
            //		SectData.ClearIncomeAndConsumeInfo  (SectData 一)\nGameInitResType key = GameInitResType.Award;
            if (Mainturn.物品添加.Value == 1)
            {
                PresetIData pd = presetIData;
                if (pd.JadeSlipInfos != null)
                {
                    foreach (PresetItemJadeSlipInfo t in pd.JadeSlipInfos)
                    {
                        ItemData item = ItemJadeSlipInfo.GetEntityByKeyID(t.PresetUniqueID);
                        SetToItem(item, 一, 1);
                    }
                }
                if (pd.DanFormulaInfos != null)
                {
                    foreach (PresetItemDanFormulaInfo t in pd.DanFormulaInfos)
                    {
                        ItemData item = ItemDanFormulaInfo.GetEntityByKeyID(t.PresetUniqueID);
                        ItemData item2 = ItemDanInfo.GetEntityByKeyGroupKey(item.RelationID);
                        SetToItem(item2, 一, 5);
                    }
                }
                if (pd.EquipFormulaInfos != null)
                {
                    foreach (PresetItemEquipFormulaInfo t in pd.EquipFormulaInfos)
                    {
                        ItemData item = ItemEquipFormulaInfo.GetEntityByKeyID(t.PresetUniqueID);
                        ItemData item2 = ItemEquipInfo.GetEntityByKeyGroupKey(item.RelationID);
                        SetToItem(item2, 一, 5);
                    }
                }
            }
            Console.WriteLine("---------厂厂弓----------");
            return true;
        }
        private static PresetIData presetIData 
        {
            get 
            {
                if (_presetIData == null)
                {
                    GetPIDIs();
                }
                return _presetIData;
            }
        }
        private static PresetIData _presetIData = null;
        public static void GetPIDIs()
        {
            string filePath = @"Json/PresetIData.json";
            string json = File.ReadAllText(filePath);
            _presetIData = JsonConvert.DeserializeObject<PresetIData>(json);
            
        }
        public static void SetToItem(ItemData item, SectData 一, int num = 1)
        {
            一.InventoryChangeItem(item, num, true, -1, false, RecordSourceType.None, null);
        }
    }
}
