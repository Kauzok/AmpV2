using RoR2;
using EntityStates;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using AmpMod.Modules;
using AmpMod.SkillStates.Nemesis_Amp.Components;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    public class FireLightningSpear : BaseSkillState
    {
        private GameObject projectilePrefab;
        private string shootString = StaticValues.plasmaFireString;
        private bool hasFired;
        private NemLightningColorController lightningController;
        public QuickDash src;
        private ChildLocator childLocator;
        private ChargeLightningBeam beamCharge;
        private FireLightningBeam beamFire;
        private GameObject muzzleFlashPrefab;
        private GameObject lightningMuzzlePrefab;
        private GameObject stakeFlashEffect;
        private string spawnString = StaticValues.plasmaChargeString;
        private int growthBuffCount;
        private float damageCoefficient = Modules.StaticValues.lightningBallDamageCoefficient;
        private StackDamageController stackDamageController;
        private float baseDuration = .6f;
        private float waitDuration;
        private float duration;
        private const float WAIT_TIME_PERCENTAGE = .9f;
        private Transform muzzleTransform;
        private Transform muzzleObjectTransform;
        private Ray aimRay;
        private uint stopID;
        private Transform lightningMuzzleTransform;

        public override void OnEnter()
        {
            base.OnEnter();

            //checkIfBusy();

            childLocator = base.GetModelChildLocator();
            //base.characterBody.SetAimTimer(2f);
            this.aimRay = base.GetAimRay();
            duration = baseDuration / this.attackSpeedStat;
            waitDuration = WAIT_TIME_PERCENTAGE * duration;
            base.StartAimMode(this.aimRay, 3f, true);
            stackDamageController = base.GetComponent<StackDamageController>();
            growthBuffCount = base.characterBody.GetBuffCount(Modules.Buffs.damageGrowth);
            //base.PlayAnimation("Gesture, Override", "FirePlasmaBall", "BaseSkill.playbackRate", 4*this.duration);
            base.PlayAnimation("FullBody, Override", "ThrowStake", "BaseSkill.playbackRate", 4 * this.duration);
           
            muzzleTransform = childLocator.FindChild("HandR");

            base.GetModelTransform().GetComponent<Animator>().SetBool("IsUsingSkill", true);

            lightningController = base.GetComponent<NemLightningColorController>();
            muzzleFlashPrefab = lightningController.lightningStakeMuzzleVFX;
            projectilePrefab = lightningController.lightningStakePrefab;
            stakeFlashEffect = lightningController.lightningStakeFlashVFX;
            lightningMuzzlePrefab = lightningController.chargeLightningStakeVFX;
            //Debug.Log(lightningMuzzlePrefab);
            EffectManager.SpawnEffect(stakeFlashEffect, new EffectData
            {
                origin = muzzleTransform.position,
                scale = 1,
            },
            true);
            lightningMuzzleTransform = UnityEngine.Object.Instantiate(lightningMuzzlePrefab, muzzleTransform).transform;

           stopID = Util.PlaySound(spawnString, base.gameObject);
            
        }

        //can't use skill if charging beam


        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!hasFired)
            {
                //checkIfBusy();
                if (this.fixedAge > waitDuration)
                {
                    AkSoundEngine.StopPlayingID(stopID);
                    Util.PlaySound(shootString, base.gameObject);
                    stackDamageController.newSkillUsed = this;
                    stackDamageController.resetComboTimer();
                    if (base.isAuthority)
                    {
                        Fire();
                  
                        hasFired = true;
                    }

                }
               
            }
            
            if (this.hasFired && this.fixedAge >= duration)
            {
                //Debug.Log("exiting");
                this.outer.SetNextStateToMain();
            }
        }
        private void Fire()
        {
            if (lightningMuzzleTransform.gameObject) Destroy(lightningMuzzleTransform.gameObject);


            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                //Debug.Log(projectilePrefab);
                if (projectilePrefab)
                {
                    float calcedDamage = damageCoefficient + (damageCoefficient * growthBuffCount * Modules.StaticValues.growthDamageCoefficient);
                    //projectilePrefab.GetComponent<ProjectileController>().teamFilter = base.GetComponent<TeamFilter>();
                    //Debug.Log(calcedDamage);
                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = this.projectilePrefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = base.characterBody.damage * calcedDamage,
                        crit = base.RollCrit(),
                        speedOverride = 140,
                    };
                    ModifyProjectile(ref fireProjectileInfo);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }


            }

            //EffectManager.SimpleMuzzleFlash(muzzleFlashPrefab, base.gameObject, "HandR", true);
            muzzleObjectTransform = UnityEngine.Object.Instantiate(muzzleFlashPrefab, muzzleTransform).transform;
        }
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
            //resets primary back to fulmination
            if (base.skillLocator.primary && hasFired)
            {
                base.skillLocator.primary.UnsetSkillOverride(QuickDash.src, QuickDash.primaryOverrideSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }


            if (lightningMuzzleTransform)
            {
                Destroy(lightningMuzzleTransform.gameObject);
            }

            AkSoundEngine.StopPlayingID(stopID);
            if (!hasFired)
            {
                base.PlayAnimation("FullBody, Override", "BufferEmpty", "BaseSkill.playbackRate", 1f);
            }

            base.GetModelTransform().GetComponent<Animator>().SetBool("IsUsingSkill", false);

        }
    }
}
