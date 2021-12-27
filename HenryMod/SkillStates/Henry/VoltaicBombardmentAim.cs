using System;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;
using UnityEngine.Networking;
using EntityStates.Captain;
using HenryMod.SkillStates;
using HenryMod.SkillStates.BaseStates;
using HenryMod.SkillStates.Henry;


namespace HenryMod.SkillStates
{
	// Token: 0x02000B11 RID: 2833
	public class VoltaicBombardmentAim : BaseLightningAim
	{
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = null;
            this.chargeSoundString = "Play_mage_m2_charge";
            this.lightningRadius = 15f;

            base.OnEnter();

            /*ChildLocator childLocator = base.GetModelChildLocator();
            if (childLocator)
            {
                this.chargeEffect = childLocator.FindChild("HealAimEffect").gameObject;
                this.chargeEffect.SetActive(true);
            }*/
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

        protected override VoltaicBombardmentFire GetNextState()
        {
            return new VoltaicBombardmentFire();
        }
    }
}

