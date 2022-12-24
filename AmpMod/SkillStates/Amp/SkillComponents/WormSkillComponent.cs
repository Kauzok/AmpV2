using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.Networking;
using RoR2;
using RoR2.Skills;
using EntityStates;
using UnityEngine;


namespace AmpMod.SkillStates.SkillComponents
{
    internal class WormSkillComponent : NetworkBehaviour
    {
        public object wormSkill;
        public GenericSkill specialSlot;
        public SkillDef cancelSkillDef;


        [ClientRpc]
        public void RpcUnSetOverride()
        {
            if (this.specialSlot && this.cancelSkillDef && Util.HasEffectiveAuthority((base.GetComponent<EntityStateMachine>().networkIdentity)))
            {
                this.specialSlot.UnsetSkillOverride(wormSkill, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }

        }
    }
}
