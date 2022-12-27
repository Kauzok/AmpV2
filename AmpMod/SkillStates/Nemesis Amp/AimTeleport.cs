using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using RoR2.Skills;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;
using AmpMod.SkillStates.BaseStates;
using AmpMod.SkillStates.Amp.BaseStates;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class AimTeleport : BaseSkillAim
    {
        public override void OnEnter()
        {
            base.OnEnter();
            this.chargeEffectPrefab = base.GetComponent<AmpMod.Modules.AmpLightningController>().bombardmentMuzzleEffect;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.lightningRadius = 15f;
            
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();

            /*if (this.chargeEffect)
            {
                this.chargeEffect.SetActive(false);
            }*/
        }


        protected override BaseSkillFire GetNextState()
        {
            return new FireTeleport();
        }
    }
}

