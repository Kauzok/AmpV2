using EntityStates;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using RoR2.Orbs;
using System.Collections.Generic;
using static HenryMod.SkillStates.BaseStates.FulminationOrb;

namespace HenryMod.SkillStates
{
    public class Fulmination : BaseState
    {
        public static float damageCoefficient = 4f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.65f;
        public static float throwForce = 80f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        GameObject victim;

     

        //lol idk i ripped this from the artificer's flamethrower code
        public override void OnEnter()
        {

            base.OnEnter();
            this.stopwatch = 0f;
            this.entryDuration = Fulmination.baseEntryDuration / this.attackSpeedStat;
            this.flamethrowerDuration = Fulmination.baseFlamethrowerDuration;
            Transform modelTransform = base.GetModelTransform();
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.entryDuration + this.flamethrowerDuration + 1f);
            }
            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.leftMuzzleTransform = this.childLocator.FindChild("MuzzleLeft");
                this.rightMuzzleTransform = this.childLocator.FindChild("MuzzleRight");
            }
            float num = this.flamethrowerDuration * Fulmination.tickFrequency;
            this.tickDamageCoefficient = Fulmination.totalDamageCoefficient / num;
            if (base.isAuthority && base.characterBody)
            {
                this.isCrit = Util.CheckRoll(this.critStat, base.characterBody.master);
            }
            base.PlayAnimation("Gesture, Additive", "PrepFlamethrower", "Flamethrower.playbackRate", this.entryDuration); ;
        }

        public override void OnExit()
        {
            Util.PlaySound(EntityStates.Mage.Weapon.Flamethrower.endAttackSoundString, base.gameObject);
            base.PlayCrossfade("Gesture, Additive", "ExitFlamethrower", 0.1f);


            base.OnExit();


        }
      

        //actual damaging attack here, declares a bullet-type (hitscan) attack and its details/properties

        private void FireGauntlet(string muzzleString)
        {
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
             
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    damage = this.tickDamageCoefficient * Fulmination.damageCoefficient,
                    force = EntityStates.Mage.Weapon.Flamethrower.force,
                    muzzleName = muzzleString,
                    hitEffectPrefab = EntityStates.Mage.Weapon.FireLaserbolt.impactEffectPrefab,
                    isCrit = this.isCrit,
                    radius = EntityStates.Mage.Weapon.Flamethrower.radius,
                    falloffModel = BulletAttack.FalloffModel.None,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = EntityStates.Mage.Weapon.Flamethrower.procCoefficientPerTick,
                    maxDistance = 50f,
                    smartCollision = true,
                    damageType = (Util.CheckRoll(EntityStates.Mage.Weapon.Flamethrower.ignitePercentChance, base.characterBody.master) ? DamageType.Shock5s : DamageType.Generic)
                }.Fire();
                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * -EntityStates.Mage.Weapon.Flamethrower.recoilForce, false, false);
                }
            }
        }


        //i think this is supposed to be the timer for how long the attack lasts, not sure though
        public override void FixedUpdate()
        {
            lightningEffectPrefab = Modules.Assets.electricStreamEffect;
            base.FixedUpdate();

            this.stopwatch += Time.fixedDeltaTime;
            if (this.stopwatch >= this.entryDuration && !this.hasBegunFlamethrower)
            {
                this.hasBegunFlamethrower = true;
                EffectData effectData = new EffectData
                {
                    origin = base.transform.position,
                    scale = 10f
                };

                Util.PlaySound(EntityStates.Mage.Weapon.Flamethrower.startAttackSoundString, base.gameObject);
                base.PlayAnimation("Gesture, Additive", "Flamethrower", "Flamethrower.playbackRate", this.flamethrowerDuration);
                EffectManager.SpawnEffect(lightningEffectPrefab, effectData, true);


                this.leftFlamethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(this.lightningEffectPrefab).transform;
            
                        this.rightFlamethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(this.lightningEffectPrefab).transform;
                
            
                        this.leftFlamethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.flamethrowerDuration;
                
                
                        this.rightFlamethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.flamethrowerDuration;
                    
                
                this.FireGauntlet("MuzzleCenter");
            }
            if (this.hasBegunFlamethrower)
            {
                this.flamethrowerStopwatch += Time.deltaTime;
                if (this.flamethrowerStopwatch > 1f / EntityStates.Mage.Weapon.Flamethrower.tickFrequency)
                {
                    this.flamethrowerStopwatch -= 1f / EntityStates.Mage.Weapon.Flamethrower.tickFrequency;
                    this.FireGauntlet("MuzzleCenter");
                }
                this.UpdateFlamethrowerEffect();
            }
            if (this.stopwatch >= this.flamethrowerDuration + this.entryDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

       //animation code, again ripped straight from artificer
        private void UpdateFlamethrowerEffect()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            Vector3 direction2 = aimRay.direction;
            if (this.leftFlamethrowerTransform)
            {
                this.leftFlamethrowerTransform.forward = direction;
            }
            if (this.rightFlamethrowerTransform)
            {
                this.rightFlamethrowerTransform.forward = direction2;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        [SerializeField]
        public GameObject lightningEffectPrefab;

        // Token: 0x04003674 RID: 13940
        public static GameObject impactEffectPrefab;

        // Token: 0x04003675 RID: 13941
        public static GameObject tracerEffectPrefab;

        // Token: 0x04003676 RID: 13942
        [SerializeField]
        public float maxDistance;

        // Token: 0x04003677 RID: 13943
        public static float radius;

        // Token: 0x04003678 RID: 13944
        public static float baseEntryDuration = 1f;

        // Token: 0x04003679 RID: 13945
        public static float baseFlamethrowerDuration = 2f;

        // Token: 0x0400367A RID: 13946
        public static float totalDamageCoefficient = 4f;

        // Token: 0x0400367B RID: 13947
        public static float procCoefficientPerTick;

        // Token: 0x0400367C RID: 13948
        public static float tickFrequency;

        // Token: 0x0400367D RID: 13949
        public static float force = 20f;

        // Token: 0x0400367E RID: 13950
        public static string startAttackSoundString;

        // Token: 0x0400367F RID: 13951
        public static string endAttackSoundString;

        // Token: 0x04003680 RID: 13952
        public static float ignitePercentChance;

        // Token: 0x04003681 RID: 13953
        public static float recoilForce;

        // Token: 0x04003682 RID: 13954
        private float tickDamageCoefficient;

        // Token: 0x04003683 RID: 13955
        private float flamethrowerStopwatch;

        // Token: 0x04003684 RID: 13956
        private float stopwatch;

        // Token: 0x04003685 RID: 13957
        private float entryDuration;

        // Token: 0x04003686 RID: 13958
        private float flamethrowerDuration;

        // Token: 0x04003687 RID: 13959
        private bool hasBegunFlamethrower;

        // Token: 0x04003688 RID: 13960
        private ChildLocator childLocator;

        // Token: 0x04003689 RID: 13961
        private Transform leftFlamethrowerTransform;

        // Token: 0x0400368A RID: 13962
        private Transform rightFlamethrowerTransform;

        // Token: 0x0400368B RID: 13963
        private Transform leftMuzzleTransform;

        // Token: 0x0400368C RID: 13964
        private Transform rightMuzzleTransform;

        // Token: 0x0400368D RID: 13965
        private bool isCrit;

        // Token: 0x0400368E RID: 13966
        private const float flamethrowerEffectBaseDistance = 16f;
  

        // Token: 0x040039DE RID: 14814
        public static GameObject chargePrefab;

        // Token: 0x040039DF RID: 14815
        public static GameObject muzzleFlashPrefab;

        // Token: 0x040039E0 RID: 14816
        public static float smallHopStrength;

        // Token: 0x040039E1 RID: 14817
        public static float antigravityStrength;

 

        // Token: 0x040039E3 RID: 14819
        public static float damageCoefficientPerBounce = 1.1f;

        // Token: 0x040039E4 RID: 14820
        public static float glaiveProcCoefficient;

        // Token: 0x040039E5 RID: 14821
        public static int maxBounceCount;

        // Token: 0x040039E6 RID: 14822
        public static float glaiveTravelSpeed;

        // Token: 0x040039E7 RID: 14823
        public static float glaiveBounceRange;

        // Token: 0x040039E8 RID: 14824
        public static string attackSoundString;

      

        // Token: 0x040039EC RID: 14828
        private GameObject chargeEffect;

        // Token: 0x040039ED RID: 14829
        private Transform modelTransform;

        // Token: 0x040039EE RID: 14830
        private HuntressTracker huntressTracker;

 

        // Token: 0x040039F0 RID: 14832
        private bool hasTriedToThrowGlaive;

        // Token: 0x040039F1 RID: 14833
        private bool hasSuccessfullyThrownGlaive;

        // Token: 0x040039F2 RID: 14834
        private HurtBox initialOrbTarget;
    }

}
