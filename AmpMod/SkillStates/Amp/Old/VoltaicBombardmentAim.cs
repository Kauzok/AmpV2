using UnityEngine;
using AmpMod.SkillStates.Amp;
using AmpMod.SkillStates.BaseStates;
using AmpMod.SkillStates.Amp.BaseStates;

namespace AmpMod.SkillStates
{
	
	public class VoltaicBombardmentAim : BaseSkillAim
	{
        private GameObject chargeEffect;

        public override void OnEnter()
        {
            this.chargeEffectPrefab = base.GetComponent<AmpMod.Modules.AmpLightningController>().bombardmentMuzzleEffect;
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

        protected override BaseSkillFire GetNextState()
        {
            return new VoltaicBombardmentFire();
        }
    }
}

