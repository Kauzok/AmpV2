using RoR2;
using EntityStates;
using UnityEngine;
using AmpMod.Modules;
using R2API;
using UnityEngine.Networking;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using System;

namespace AmpMod.SkillStates.Nemesis_Amp
{
   public class FireLightningBeam : BaseState
    {
        [SerializeField]
        public float baseDuration = .3f;

        private float duration;
        private float radius = Modules.StaticValues.chargeBeamRadius;
        private float surgeBuffCount;
        private Transform muzzleHandTransform;
        private GameObject muzzleFlashEffect;
        private NemLightningColorController lightningController;

        private float minDamageCoefficient = Modules.StaticValues.chargeBeamMinDamageCoefficient;
        private float maxDamageCoefficient = Modules.StaticValues.chargeBeamMaxDamageCoefficient;
        private float additionalPierceDamageCoefficient = StaticValues.additionalPierceDamageCoefficient;
        public float charge;
        private float maxForce = 2000f;
        private float baseDamage;
        private ChildLocator childLocator;
        private bool hasFired;
        private float waitDuration;
        public bool doHover;

        public static event Action<FireLightningBeam> onFireBeam;
        public static event Action<FireLightningBeam> onPierce;

        [SerializeField]
        public float selfForce;

        private StackDamageController stackDamageController;


        [Header("Sounds")]
        private string fireSoundString = StaticValues.fireBeamSoundString;

        public float piercedCount { get; private set; }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.doHover);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.doHover = reader.ReadBoolean();
        }

        public override void OnEnter()
        {
            Action<FireLightningBeam> action = FireLightningBeam.onFireBeam;
            if (action!= null)
            {
                action(this);
            }
            piercedCount = 1;
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();

            childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            lightningController = base.GetComponent<NemLightningColorController>();
            muzzleFlashEffect = lightningController.beamMuzzleVFX;

            muzzleHandTransform = childLocator.FindChild("HandL");
            //Debug.Log("muzzle rotation = " + muzzleHandTransform.rotation.eulerAngles.x + " " + muzzleHandTransform.rotation.eulerAngles.y + " "+ muzzleHandTransform.rotation.eulerAngles.z);
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.waitDuration = this.duration / 1.6f;
            base.characterBody.SetAimTimer(this.duration + .3f);
            this.PlayFireAnimation();
            surgeBuffCount = base.GetBuffCount(Buffs.damageGrowth);
            //base.PlayAnimation("Gesture, Override", "FireBeam", "BaseSkill.playbackRate", this.duration);
            base.PlayAnimation("FullBody, Override", "FireBeam", "BaseSkill.playbackRate", this.duration + .8f);
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();


        }


        private void PlayFireAnimation()
        {

        }

        private void ModifyBullet(BulletAttack bulletAttack)
        {
            //bulletAttack.sniper = true;
            bulletAttack.falloffModel = BulletAttack.FalloffModel.None;

            bulletAttack.modifyOutgoingDamageCallback = delegate (BulletAttack _bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo)
            {
                Action<FireLightningBeam> action = FireLightningBeam.onPierce;
                if (action != null)
                {
                    action(this);
                }
                _bulletAttack.damage += (baseDamage * this.additionalPierceDamageCoefficient);
                //_bulletAttack.sniper = true;
               piercedCount++;
            };
        }




        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!isGrounded && base.isAuthority && doHover)
            {
                base.characterMotor.velocity.y = 0f;

            }

            if (!hasFired && base.fixedAge > this.waitDuration)
            {
                Util.PlaySound(fireSoundString, base.gameObject);
                if (base.isAuthority)
                {
                    this.Fire();
                    hasFired = true;
                }

            }

            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                //Debug.Log(base.GetComponent<EntityStateMachine>().networkIdentity.hasAuthority + "for authority");
                this.outer.SetNextStateToMain();
            }

        }
        private void Fire()
        {
            if (base.isAuthority)
            {
                
                Ray aimRay = base.GetAimRay();

                float calcedDamage = Util.Remap(this.charge, 0f, 1f, this.minDamageCoefficient, this.maxDamageCoefficient);
                float num2 = this.charge * this.maxForce;
                //Debug.Log("calced damage is " + calcedDamage);
                //Debug.Log("firing");
                float beamDamage = (StaticValues.growthDamageCoefficient * surgeBuffCount * calcedDamage) + calcedDamage;
                BulletAttack beamAttack = new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    damage = base.characterBody.damage * beamDamage,
                    force = num2,
                    muzzleName = "HandL",
                    //hitEffectPrefab = impactEffectPrefab,
                    tracerEffectPrefab = lightningController.beamObject,
                    isCrit = base.characterBody.RollCrit(),
                    radius = this.radius, 
                    falloffModel = BulletAttack.FalloffModel.None,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = 1f,
                    maxDistance = 160f,
                    smartCollision = true,
                    damageType = DamageType.Generic,
                   
                };
                beamAttack.AddModdedDamageType(DamageTypes.controlledChargeProc);
                baseDamage = base.characterBody.damage * beamDamage;
                ModifyBullet(beamAttack);
                beamAttack.Fire();

                //UnityEngine.Object.Instantiate<GameObject>(muzzleFlashEffect, muzzleHandTransform);

                //EffectManager.SimpleMuzzleFlash(muzzleFlashEffect, base.gameObject, "HandL", true);
                Transform handTransform = childLocator.FindChild("HandL").transform;
                EffectManager.SpawnEffect(muzzleFlashEffect, new EffectData
                {
                    origin = handTransform.position,
                    rotation = handTransform.rotation,
                },
                true);
                //base.PlayAnimation("Gesture, Additive", "FireBeam", "BaseSkill.playbackRate", this.duration);

                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * (-this.selfForce * this.charge), false, false);
                }
            }
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
        public override void OnExit()
        {
            base.OnExit();
        }


	}
}
