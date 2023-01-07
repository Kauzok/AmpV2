using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpStormSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new NemAmpStormSkillDef.InstanceData
            {
                nemAmpTracker = skillSlot.GetComponent<NemAmpLightningTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            NemAmpLightningTracker nemAmpTracker = ((NemAmpStormSkillDef.InstanceData)skillSlot.skillInstanceData).nemAmpTracker;
            return nemAmpTracker != null && nemAmpTracker.enemyInStormRange;
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return NemAmpStormSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && NemAmpStormSkillDef.HasTarget(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {

            public NemAmpLightningTracker nemAmpTracker;
        }
    }
}
