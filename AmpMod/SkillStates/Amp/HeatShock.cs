using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;
using RoR2.Audio;


namespace AmpMod.SkillStates
{
    class PLASMASLASH : BaseSkillState
    {

        private float baseTotalDuration = .75f;
        private float baseChargeDuration = .5f;
        protected string hitboxName = "SpinSlash";
        private float chargeDuration;
        private float totalDuration;
        public OverlapAttack attack;
        protected DamageType damageType = DamageType.IgniteOnHit;
        protected float damageCoefficient = Modules.StaticValues.spinSlashDamageCoefficient;
        protected float procCoefficient = 1f;
        protected GameObject swingEffectPrefab = Modules.Assets.heatSwing;
        protected GameObject hitEffectPrefab = Modules.Assets.heatHit;
        protected float stopwatch;
        private ChildLocator childLocator;
        protected Animator animator;
        private bool hasMuzzleEffect;
       
        private Transform fireAOE;

        private DamageTrail fireTrail;
        private GameObject muzzleEffectPrefab = Modules.Assets.plasmaMuzzle;
        private float fireDamageCoefficient = Modules.StaticValues.fireTrailTickDamageCoefficient;
        protected float pushForce = 300f;
        protected Vector3 bonusForce = Vector3.zero;
        protected NetworkSoundEventIndex impactSound = Modules.Assets.heatShockHitSoundEvent.index;
        private bool hasFired = false;
        private string chargeSoundString = Modules.StaticValues.heatChargeString;
        public uint stopChargeSound;
        protected string swingSoundString = Modules.StaticValues.heatSwingString;
        private String muzzleString = "HeatSwing";
        private bool inGroundedState;
        private Transform swordMuzzle;
        public static float shortHopVelocity = 0f;
        private GameObject fireBeamPrefab = Modules.Projectiles.fireBeamPrefab;
        private float fireBeamDamage = Modules.StaticValues.fireBeamDamageCoefficient;


        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            chargeDuration = this.baseChargeDuration / this.attackSpeedStat;
            totalDuration = this.baseTotalDuration / this.attackSpeedStat;
            animator.SetBool("isUsingIndependentSkill", true);

            //Util.PlayAttackSpeedSound(this.chargeSoundString, base.gameObject, this.attackSpeedStat);
            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();
            childLocator = modelTransform.GetComponent<ChildLocator>();
            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.swordMuzzle = this.childLocator.FindChild("SwordPlace");
            }

            if (isGrounded)
            {

                inGroundedState = true;
                base.PlayAnimation("FullBody, Override", "SpinningSlash", "BaseSkill.playbackRate", this.totalDuration);//this.baseDuration);
                stopChargeSound = Util.PlaySound(chargeSoundString, base.gameObject);
                this.attack = new OverlapAttack();
                this.attack.damageType = this.damageType;
                this.attack.attacker = base.gameObject;
                this.attack.inflictor = base.gameObject;
                this.attack.teamIndex = base.GetTeam();
                this.attack.damage = this.damageCoefficient * base.characterBody.damage;
                this.attack.procCoefficient = this.procCoefficient;
                this.attack.hitEffectPrefab = this.hitEffectPrefab;
                this.attack.forceVector = this.bonusForce;
                this.attack.pushAwayForce = this.pushForce;
                this.attack.hitBoxGroup = hitBoxGroup;
                this.attack.isCrit = base.RollCrit();
                this.attack.impactSound = this.impactSound;
            }

            else if (!isGrounded)
            {
                stopChargeSound = Util.PlaySound(chargeSoundString, base.gameObject);
                inGroundedState = false;
                base.PlayAnimation("FullBody, Override", "SpinningSlash", "BaseSkill.playbackRate", this.totalDuration);

            }
      


        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {

        }
        
        private void FireAerialAttack()
        {
            if (base.isAuthority)
            {

                Ray aimRay = base.GetAimRay();
                AkSoundEngine.StopPlayingID(stopChargeSound, 0);
                if (fireBeamPrefab) {

                    Util.PlaySound(swingSoundString, base.gameObject);
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {

                        projectilePrefab = fireBeamPrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = fireBeamDamage*base.characterBody.damage,
                        //force = fireBeamPrefab.GetComponent<ProjectileSimple>().desiredForwardSpeed,
                        crit = base.RollCrit()
                    };
                    ModifyProjectile(ref fireProjectileInfo);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            }
        }

        private void FireGroundAttack()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                AkSoundEngine.StopPlayingID(stopChargeSound, 0);
                //Util.PlayAttackSpeedSound(this.swingSoundString, base.gameObject, this.attackSpeedStat);
                Util.PlaySound(swingSoundString, base.gameObject);
                if (base.isAuthority)
                {
                    this.PlaySwingEffect();
                    
                }
            }

            if (base.isAuthority)
            {
                attack.Fire();
               
                Debug.Log("Spawning Fire");
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            
            if (!hasMuzzleEffect)
            {
                hasMuzzleEffect = true;
                swordMuzzle = UnityEngine.Object.Instantiate<GameObject>(muzzleEffectPrefab, swordMuzzle).transform;
            }


            if (inGroundedState)
            {
                //fireTrail.transform.position = childLocator.FindChild("SwordPlace").position;
                if (fixedAge > chargeDuration && !hasFired)
                {
                    FireGroundAttack();
                    hasFired = true;
                    Debug.Log("firing");
                    if (swordMuzzle) EntityState.Destroy(swordMuzzle.gameObject);
                }
                if (fixedAge >= totalDuration)
                {
                    this.outer.SetNextStateToMain();
                }
            }
            else if (!inGroundedState)
            {
                if (base.characterMotor)
                {
                    if (!hasFired)
                    {
                        base.characterMotor.velocity.y = shortHopVelocity;
                    }
                    
                    if (fixedAge > chargeDuration && !hasFired)
                    {
                        FireAerialAttack();
                        hasFired = true;
                        if (swordMuzzle) EntityState.Destroy(swordMuzzle.gameObject);
                        Debug.Log("firing");
                        //base.characterMotor.velocity.y = base.characterMotor.velocity.y;
                    }
                    if (fixedAge >= totalDuration)
                    {
                        this.outer.SetNextStateToMain();
                    }
                }
            }
      

        }
        protected void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.muzzleString, true);
        }


        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("isUsingIndependentSkill", false);
        }
    }
}
