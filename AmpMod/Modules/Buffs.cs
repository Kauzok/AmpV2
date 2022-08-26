using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace AmpMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef chargeBuildup;
        internal static BuffDef noFulmination;
        internal static BuffDef electrified;
        internal static BuffDef noSurge;
        internal static BuffDef overCharge;

        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            //charge debuff
            chargeBuildup = AddNewBuff("AmpChargeBuildup", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texChargeDebuffAlt"), new Color32(0, 145, 255, 255), true, true);
            electrified = AddNewBuff("AmpElectrified", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texElectrified"), new Color32(76, 206, 255, 255), false, true);
            overCharge = AddNewBuff("AmpOverCharge", LegacyResourcesAPI.Load<BuffDef>("BuffDefs/TeslaField").iconSprite, new Color32(0, 145, 255, 255), false, false);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}