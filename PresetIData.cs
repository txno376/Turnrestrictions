using ChiefOfImmortal.Definition.DataStruct.Preset;
using System.Collections.Generic;

namespace Turnrestrictions
{
    public class PresetIData
    {
        public List<PresetItemJadeSlipInfo> JadeSlipInfos { get; set; } //功法
        public List<PresetItemDanFormulaInfo> DanFormulaInfos { get; set; } //丹方
        public List<PresetItemEquipFormulaInfo> EquipFormulaInfos { get; set; } //器方
        public List<TheoryMod> TeoryMods { get; set; } //天地规则
    }
    public class TheoryMod
    {
        public int id { get; set; }
        public int[] count { get; set; }
        public int[] indexs { get; set; }
        public float[] value { get; set; }
    }
}
