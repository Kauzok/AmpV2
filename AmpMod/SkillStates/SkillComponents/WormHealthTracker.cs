using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Skills;

namespace AmpMod.SkillStates.SkillComponents
{
    class WormHealthTracker : MonoBehaviour
    {
        public object wormSkill;
        public GenericSkill specialSlot;
        public SkillDef cancelSkillDef;
        public CharacterBody wormBody;
        public CharacterMaster wormMaster;

        public void Awake()
        {


        }

        private void FixedUpdate()
        {
            if (wormBody.healthComponent.health <= 0f && wormMaster.inventory.GetItemCount(RoR2Content.Items.ExtraLife) == 0 && wormMaster.inventory.GetItemCount(DLC1Content.Items.ExtraLifeVoid) == 0)
            {
                //Debug.Log("Worm has died");
                this.specialSlot.UnsetSkillOverride(wormSkill, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
        }
    }
}
