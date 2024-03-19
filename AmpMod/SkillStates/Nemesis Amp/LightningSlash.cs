using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using EntityStates;
using RoR2;
using UnityEngine.Networking;
using R2API;
using AmpMod.Modules;
using RoR2.Audio;


namespace AmpMod.SkillStates.Nemesis_Amp
{
    class LightningSlash : BaseSkillState
    {

        private float baseTotalDuration = .75f;
        private const float PREP_TIME_PERCENTAGE = .7f;
        protected string hitboxName = "Cleave";
        private bool hasFired;
        private float prepDuration;
        private float totalDuration;
        protected DamageType damageType = DamageType.Generic;
        protected float damageCoefficient = Modules.StaticValues.voidSlashDamageCoefficient;
        protected float procCoefficient = 1f;
        protected GameObject swingEffectPrefab = Assets.lightningBladeSlashVFX;
        protected GameObject hitEffectPrefab = Modules.Assets.lightningBladeHitVFX;
        private GameObject spawnBladeVFX = Assets.spawnLightningBladeVFX;
        protected float stopwatch;
        private ChildLocator childLocator;
        protected Animator animator;
        private bool hasMuzzleEffect;
        private Transform muzzleTransform;
        private StackDamageController stackDamageController;
        private GameObject muzzleEffectPrefab = Assets.lightningBladePrefab;
        protected float pushForce = 300f;
        protected Vector3 bonusForce = Vector3.zero;
        protected NetworkSoundEventIndex impactSound = Modules.Assets.heatShockHitSoundEvent.index;

        private String muzzleString = "SwingCenter";
        private bool inGroundedState;
        private Transform swordMuzzle;
        public static float shortHopVelocity = 0f;


        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            totalDuration = this.baseTotalDuration / this.attackSpeedStat;
            prepDuration = PREP_TIME_PERCENTAGE * totalDuration;
            Transform modelTransform = base.GetModelTransform();
            childLocator = modelTransform.GetComponent<ChildLocator>();
            base.PlayAnimation("FullBody, Override", "WideSlash", "BaseSkill.playbackRate", this.totalDuration + 1f);
            base.characterBody.SetAimTimer(totalDuration+1f);
            stackDamageController = base.GetComponent<StackDamageController>();
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
            muzzleTransform = this.childLocator.FindChild("LowerArmL");
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasMuzzleEffect)
            {
                hasMuzzleEffect = true;
                EffectManager.SpawnEffect(spawnBladeVFX, new EffectData
                {
                    origin = muzzleTransform.position,
                    scale = 1,
                },
                true);


                swordMuzzle = UnityEngine.Object.Instantiate<GameObject>(muzzleEffectPrefab, muzzleTransform).transform;
            }
            if (base.fixedAge > prepDuration && !hasFired)
            {
                FireGroundAttack();
                PlaySwingEffect();
                hasFired = true;
                
            }

            if (base.fixedAge > totalDuration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        protected void PlaySwingEffect()
        {
            EffectManager.SimpleMuzzleFlash(this.swingEffectPrefab, base.gameObject, this.muzzleString, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            if (swordMuzzle)
            {
                EntityState.Destroy(swordMuzzle.gameObject);
            }
        }

        private void FireAerialAttack()
        {
            if (base.isAuthority)
            {

                Ray aimRay = base.GetAimRay();
                
    
            }
        }

        private void FireGroundAttack()
        {
            HitBoxGroup hitBoxGroup = null;
            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                hitBoxGroup = Array.Find<HitBoxGroup>(modelTransform.GetComponents<HitBoxGroup>(), (HitBoxGroup element) => element.groupName == this.hitboxName);       
            }
            if (hitBoxGroup != null)
            {
                Debug.Log("found hitboxgroup");
            }
            
            OverlapAttack attack = new OverlapAttack();
            attack.damageType = damageType;
            attack.attacker = base.gameObject;
            attack.inflictor = base.gameObject;
            attack.teamIndex = base.GetTeam();
            attack.damage = damageCoefficient * base.characterBody.damage;
            attack.procCoefficient = procCoefficient;
            attack.hitEffectPrefab = hitEffectPrefab;
            attack.forceVector = bonusForce;
            attack.pushAwayForce = pushForce;
            attack.hitBoxGroup = hitBoxGroup;
            attack.isCrit = base.RollCrit();
            attack.impactSound = impactSound;

            if (base.isAuthority)
            {
               attack.Fire();
            }
            //attack.Fire();

        }
    }
}
