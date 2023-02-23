using RoR2;
using EntityStates;
using UnityEngine;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{

    class ChargeLightningBeam : BaseSkillState
    {
        [SerializeField]
        public float minChargeDuration = 0.5f;

        [SerializeField]
        public float baseDuration = 1.5f;

        private float duration;
        private Animator animator;
        private ChildLocator childLocator;
        private GameObject chargeEffectInstance;
        private GameObject chargeEffectPrefab = Assets.chargeBeamMuzzleEffect;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            base.characterBody.SetAimTimer(duration);
            base.PlayAnimation("Gesture, Override", "ChargeBeam", "BaseSkill.playbackRate", 0.4f);
            if (this.childLocator)
            {
                Transform transform = base.characterBody.coreTransform;//this.childLocator.FindChild("MuzzleBetween") ?? base.characterBody.coreTransform;
                if (transform && this.chargeEffectPrefab)
                {
                    this.chargeEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.chargeEffectPrefab, transform.position, transform.rotation);
                    this.chargeEffectInstance.transform.parent = transform;
                    ScaleParticleSystemDuration component = this.chargeEffectInstance.GetComponent<ScaleParticleSystemDuration>();
                    ObjectScaleCurve component2 = this.chargeEffectInstance.GetComponent<ObjectScaleCurve>();
                    if (component)
                    {
                        component.newDuration = this.duration;
                    }
                    if (component2)
                    {
                        component2.timeMax = this.duration;
                    }
                }

            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            float charge = this.CalcCharge();
            if (base.isAuthority && ((!base.IsKeyDownAuthority() && base.fixedAge >= this.minChargeDuration) || base.fixedAge >= this.duration))
            {
                FireLightningBeam nextState = this.GetNextState();
                nextState.charge = charge;
                this.outer.SetNextState(nextState);
  

            }
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        private FireLightningBeam GetNextState()
        {
            return new FireLightningBeam();
        }

        protected float CalcCharge()
        {
            return Mathf.Clamp01(base.fixedAge / this.duration);
        }

        public override void OnExit()  
        {
            base.OnExit();
            if (this.chargeEffectInstance)
            {
                Destroy(this.chargeEffectInstance);
            }
        }
    }
}
