using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using RoR2.Skills;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.SkillComponents
{


    class WormSkillOverride : NetworkBehaviour
    {
        public object wormSkill;
        public GenericSkill specialSlot;
        public SkillDef cancelSkillDef;
        public bool hasUnset;
        

        [ClientRpc]
        public void RpcUnSetOverride()
        {
            if (this.specialSlot && this.cancelSkillDef)
            {
                this.specialSlot.UnsetSkillOverride(wormSkill, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            
        }


    }
}
