using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace AmpMod.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef chargeBuildup;


        internal static List<BuffDef> buffDefs = new List<BuffDef>();

        internal static void RegisterBuffs()
        {
            //charge debuff
            chargeBuildup = AddNewBuff("AmpChargeBuildup", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texChargeDebuffAlt"), new Color32(0, 145, 255, 255), true, true);
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