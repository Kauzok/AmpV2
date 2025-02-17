﻿using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2;
using RoR2.Skills;

namespace AmpMod.SkillStates
{

    class CancelWurm  : BaseSkillState
    {
        private GenericSkill specialSlot;
        public static SkillDef cancelSkillDef;
        

        public override void OnEnter()
        {
            base.OnEnter();

            if (SummonWurm_Old.wormMaster)
            {
                //SummonWurm.wormMaster.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;
                SummonWurm_Old.wormMaster.TrueKill();
            }
            

        }

         
        public override void OnExit()
        {
            base.OnExit();

           cancelSkillDef = SummonWurm_Old.cancelSkillDef;

           specialSlot = base.skillLocator.special;

            if (specialSlot && cancelSkillDef)
            {
                this.specialSlot.UnsetSkillOverride(SummonWurm_Old.src, cancelSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
          
            Debug.Log("Exiting");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();


            this.outer.SetNextStateToMain();

        }
    }
}
