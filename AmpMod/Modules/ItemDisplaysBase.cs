using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmpMod.Modules
{
    public abstract class ItemDisplaysBase
    {

        public void SetItemDisplays(ItemDisplayRuleSet itemDisplayRuleSet)
        {

            List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();

            SetItemDisplayRules(itemDisplayRules);

            itemDisplayRuleSet.keyAssetRuleGroups = itemDisplayRules.ToArray();
        }

        protected abstract void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules);
    }
}
