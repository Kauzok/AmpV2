using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Skills;
using JetBrains.Annotations;


namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class NemAmpOrbTrackingSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new NemAmpOrbTrackingSkillDef.InstanceData
            {
                nemAmpTracker = skillSlot.GetComponent<NemAmpLightningTracker>()
            };
        }

        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            NemAmpLightningTracker nemAmpTracker = ((NemAmpOrbTrackingSkillDef.InstanceData)skillSlot.skillInstanceData).nemAmpTracker;
            return nemAmpTracker != null && nemAmpTracker.GetTrackingTarget();
        }

        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return NemAmpOrbTrackingSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }

        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && NemAmpOrbTrackingSkillDef.HasTarget(skillSlot);
        }

        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {

            public NemAmpLightningTracker nemAmpTracker;
        }
    }
}
