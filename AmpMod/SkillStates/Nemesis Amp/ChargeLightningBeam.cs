using RoR2;
using EntityStates;
using UnityEngine;
using AmpMod.Modules;
using AmpMod.SkillStates.Nemesis_Amp.Components;

namespace AmpMod.SkillStates.Nemesis_Amp
{

    class ChargeLightningBeam : BaseSkillState
    {
        [SerializeField]
        public float minChargeDuration = 0.5f;

        [SerializeField]
        public float baseDuration = 1.5f;

        private NemLightningColorController lightningController;
        private float duration;
        private Animator animator;
        private ChildLocator childLocator;
        private GameObject chargeEffectInstance;
        private GameObject chargeEffectPrefab;
        private bool charged;
        private float hoverAcceleration = 80f;
        private float hoverVelocity = -1f;
        private float unGroundedAge = 0f;
        private bool doHover;

        [Header("Sounds")]
        private string startSoundString = StaticValues.chargeBeamSoundString;
        private uint endLoopSoundID = 0;
        private float soundTimer;
        private bool hasBegunSound;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            charged = false;
            this.animator = base.GetModelAnimator();
            this.childLocator = base.GetModelChildLocator();
            base.characterBody.SetAimTimer(duration);
            lightningController = base.GetComponent<NemLightningColorController>();
            chargeEffectPrefab = lightningController.beamChargeVFX;
            //base.PlayAnimation("Gesture, Override", "ChargeBeam", "ChargeBeam.playbackRate", this.duration + 1f);
            //base.PlayAnimation("Gesture, Additive", "ChargeBeam", "ChargeBeam.playbackRate", this.duration + 1f);
            //base.PlayAnimation("FullBody, Override", "ChargeBeam", "ChargeBeam.playbackRate", this.duration + 1f);
            base.PlayAnimation("FullBody, Override", "ChargeBeam", "ChargeBeam.playbackRate", this.duration+.2f);
            endLoopSoundID = Util.PlaySound(startSoundString, base.gameObject);

            if (!isGrounded)
            {
                doHover = true;
            }

            if (this.childLocator)
            {   
                Transform transform = this.childLocator.FindChild("HandL") ?? base.characterBody.coreTransform;
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
                nextState.doHover = this.doHover;
                nextState.charge = charge;
                this.outer.SetNextState(nextState);
                charged = true;

            }
            if (isGrounded)
            {
                doHover = false;
            }
            //unGroundedAge += Time.fixedDeltaTime;

            if (!isGrounded && base.isAuthority && doHover)
            {
                base.characterMotor.velocity.y = 0f;
                float num = base.characterMotor.velocity.y;
                num = Mathf.MoveTowards(num, hoverVelocity, hoverAcceleration * Time.fixedDeltaTime);
                //base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num, base.characterMotor.velocity.z);
                base.characterMotor.velocity = new Vector3(base.characterMotor.velocity.x, num, base.characterMotor.velocity.z);
            }
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
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
            AkSoundEngine.StopPlayingID(endLoopSoundID);
            if (this.chargeEffectInstance)
            {
                Destroy(this.chargeEffectInstance);
            }

            if (!charged)

            {
                base.activatorSkillSlot.rechargeStopwatch = (0.9f * base.activatorSkillSlot.finalRechargeInterval);

                base.PlayAnimation("FullBody, Override", "BufferEmpty", "ChargeBeam.playbackRate", 1f);
            }
        }
    }
}
