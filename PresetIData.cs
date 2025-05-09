using ChiefOfImmortal.Definition.DataStruct.Preset;
using System.Collections.Generic;

namespace Turnrestrictions
{
    public class PresetIData
    {
        public List<PresetItemJadeSlipInfo> JadeSlipInfos { get; set; } //功法
        public List<PresetItemDanFormulaInfo> DanFormulaInfos { get; set; } //丹方
        public List<PresetItemEquipFormulaInfo> EquipFormulaInfos { get; set; } //器方
        public List<TheoryMods> TheoryMods { get; set; } //天地规则
    }
    public class TheoryMods
    {
        public int id { get; set; }
        public string count { get; set; }
        public string indexs { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }
}
