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
        internal static BuffDef controlledCharge;
        internal static BuffDef noSurge;
        internal static BuffDef overCharge;
        internal static BuffDef damageGrowth;
        internal static BuffDef nemAmpAtkSpeed;
        internal static BuffDef shieldDamageBoost;
        internal static BuffDef sandedDebuff;
        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            //charge debuff
            sandedDebuff = AddNewBuff("AmpSanded", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texChargeDebuffAlt"), new Color32(0, 105, 105, 105), false, true);
            chargeBuildup = AddNewBuff("AmpChargeBuildup", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texChargeDebuffAlt"), new Color32(0, 145, 255, 255), true, true);
            electrified = AddNewBuff("AmpElectrified", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texElectrified"), new Color32(76, 206, 255, 255), false, true);
            overCharge = AddNewBuff("AmpOverCharge", LegacyResourcesAPI.Load<BuffDef>("BuffDefs/TeslaField").iconSprite, new Color32(0, 145, 255, 255), false, false);
            shieldDamageBoost = AddNewBuff("AmpShieldBoost", null, new Color32(0, 145, 255, 255), false, false, true);
            controlledCharge = AddNewBuff("NemesisAmpContolledCharge", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texControlledChargeAlt"), new Color32(180, 71, 255, 255), true, true);
            damageGrowth = AddNewBuff("NemesisAmpDamageGrowth", null, new Color32(61, 0, 0, 0), true, false, true);
            nemAmpAtkSpeed = AddNewBuff("nemAmpAtkSpeed", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texFieldAtkSpeedCombined"), new Color32(180, 71, 255, 255), false, false);
            
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
            buffDef.isHidden = false;

            buffDefs.Add(buffDef);

            return buffDef;
        }

        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff, bool isHidden)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;
            buffDef.isHidden = isHidden;

            buffDefs.Add(buffDef);

            return buffDef;
        }
    }
}