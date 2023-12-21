using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using RoR2.Skills;
using UnityEngine;
using R2API;
using RoR2.Orbs;
using UnityEngine.Networking;
using AmpMod.Modules;
using AmpMod.SkillStates.Nemesis_Amp.Orbs;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using R2API.Networking.Interfaces;

namespace AmpMod.SkillStates.Nemesis_Amp
{
     class LightningStream : BaseSkillState
    {

        [Header("Attack Variables")]
        private float lightningTickDamage = Modules.StaticValues.lightningStreamPerSecondDamageCoefficient / 3f;
        private StackDamageController stackDamageController;
        private NemAmpLightningTetherController lightningEffectController;
        private float procCoefficient = Modules.StaticValues.lightningStreamProcCoefficient;
        private float baseTickTime = Modules.StaticValues.lightningStreamBaseTickTime;
        private float tickTime;
        private float tickTimer;

        [Header("Tracking/Effect Variables")]
        private NemAmpLightningTracker tracker;
        private HurtBox targetHurtbox;
        private bool lightningTetherActive;
        private Transform rightMuzzleTransform;
        private Transform nexusMuzzleTransform;
        private GameObject muzzleEffect = Assets.lightningStreamMuzzleEffect;
        private GameObject muzzleFlashEffect = Assets.lightningStreamMuzzleFlash;
        private GameObject muzzleFlashObject;
        private bool muzzleHasFlashed;
        private NemLightningColorController lightningController;

        [Header("Animation Variables")]
        private Animator animator;
        private ChildLocator childLocator;

        [Header("Sounds")]
        private string startSoundString = StaticValues.lightningStreamStartString;
        private string loopSoundString = StaticValues.lightningStreamLoopSoundString;
        private string endSoundString = StaticValues.lightningStreamEndString;
        private uint endLoopSoundID = 0;
        private float soundTimer;
        private bool hasBegunSound;

        public class SyncDamage : INetMessage
        {

            HurtBoxReference target;
            bool isChaining;
            bool procCharged;
            NetworkInstanceId bodyID;
            Transform muzzleTransform;
            double damageVal;

            //use this to sync rightmuzzletransform 
            public SyncDamage()
            {

            }
            private NemAmpLightningLockOrb createDmgOrb(GameObject attacker)
            {
                var targetHurtbox = target.ResolveHurtBox();
                if (targetHurtbox)
                {
                    Debug.Log("seeing target hurtbox on syncdamage as " + targetHurtbox);
                    // Debug.Log("creating orb on hurtbox");
                    //Debug.Log(targetHurtbox.gameObject.GetComponent<CharacterBody>().name + "is target");
                }

                if (!targetHurtbox)
                {
                    Debug.Log("hurtbox for orb not found");
                }

                NemAmpLightningLockOrb lockOrb = new NemAmpLightningLockOrb();

                var body = attacker.GetComponent<CharacterBody>();

                lockOrb.damageValue = (float) damageVal;
                //lockOrb.origin = muzzleTransform.position;
                lockOrb.isCrit = body.RollCrit();
                lockOrb.damageType = DamageType.Generic;
                lockOrb.teamIndex = body.teamComponent.teamIndex;
                lockOrb.attacker = attacker;
                lockOrb.procCoefficient = .2f;
                lockOrb.damageColorIndex = DamageColorIndex.Default;
                lockOrb.target = targetHurtbox;
                lockOrb.bouncedObjects = new List<HealthComponent>();
                lockOrb.range = 30f;
                lockOrb.damageCoefficientPerBounce = .8f;
                lockOrb.nemLightningColorController = attacker.GetComponent<NemLightningColorController>();

                Debug.Log("lockorb damage value is " + lockOrb.damageValue);
                return lockOrb;
            }

            public SyncDamage(HurtBoxReference target, Transform muzzleTransform, NetworkInstanceId bodyID, double damageVal, bool isChaining, bool procCharged)
            {

                this.target = target;
                this.muzzleTransform = muzzleTransform;
                this.damageVal = damageVal;
                this.bodyID = bodyID;
                this.isChaining = isChaining;
                this.procCharged = procCharged;
            }

            //we then read the targethurtbox as a hurtboxreference
            public void Deserialize(NetworkReader reader)
            {

                this.target = reader.ReadHurtBoxReference();
                this.bodyID = reader.ReadNetworkId();
                this.muzzleTransform = reader.ReadTransform();
                this.damageVal = reader.ReadDouble();
                this.isChaining = reader.ReadBoolean();
                this.procCharged = reader.ReadBoolean();

            }

            //do damage
            public void OnReceived()
            {

                var lightningOrb = createDmgOrb(Util.FindNetworkObject(bodyID));

                lightningOrb.procControlledCharge = this.procCharged;

                if(isChaining)
                {
                    lightningOrb.bouncesRemaining = 2;
                }
                else
                {
                    lightningOrb.isChaining = false;
                    lightningOrb.bouncesRemaining = 0;
                }

                OrbManager.instance.AddOrb(lightningOrb);

            }

            public void Serialize(NetworkWriter writer)
            {
                writer.Write(target);
                writer.Write(bodyID);
                writer.Write(muzzleTransform);
                writer.Write(damageVal);
                writer.Write(isChaining);
                writer.Write(procCharged);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            lightningEffectController = base.GetComponent<NemAmpLightningTetherController>();
            lightningEffectController.isAttacking = true;

            lightningController = base.GetComponent<NemLightningColorController>();
            muzzleEffect = lightningController.streamMuzzleVFX;
            muzzleFlashEffect = lightningController.streamMuzzleFlashVFX;

            tracker = base.GetComponent<NemAmpLightningTracker>();

            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.animator = modelTransform.GetComponent<Animator>();
            }
             
            this.tickTime = this.baseTickTime / this.attackSpeedStat;

         
            if (tracker.GetTrackingTarget())
            {
                if (NetworkServer.active)
                {
                    //Debug.Log("on network checking target");
                    //rightMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;
                    this.targetHurtbox = tracker.GetTrackingTarget();
                    //Debug.Log("now target hurtbox is on network: " + tracker.GetTrackingTarget());
                }

                this.targetHurtbox = tracker.GetTrackingTarget();
                //Debug.Log(tracker.GetTrackingTarget()) ;
                //Debug.Log("now target hurtbox is on client: " + targetHurtbox);

                animator.SetBool("NemIsFulminating", true);

                //this call gets an error, what the hell?
                
                rightMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;


                base.PlayAnimation("RightArm, Override", "ShootLightning", "BaseSkill.playbackRate", 0.4f);

                this.nexusMuzzleTransform = UnityEngine.Object.Instantiate<GameObject>(muzzleEffect, rightMuzzleTransform).transform;

            }

            //base.PlayAnimation("Spawn, Override", "Spawn", "Spawn.playbackRate", 4f);
            //animations


        }

        private void FireLightning()
        {
            /*if (!NetworkServer.active && tickTimer <= 0f)
            {
                this.targetHurtbox = tracker.GetTrackingTarget();
            }

            if (!NetworkServer.active || tickTimer > 0f)
            {
                //Debug.Log("returning");
                return;
                
            } */

            if (tickTimer <= 0f)
            {
                this.targetHurtbox = tracker.GetTrackingTarget();
            }

            if (tickTimer > 0f)
            {
                //Debug.Log("returning");
                return;

            }

            //Debug.Log("not returning");

            this.tickTimer = this.tickTime;


            this.targetHurtbox = tracker.GetTrackingTarget();
            

            if (targetHurtbox)
            {
                bool isChaining = (base.GetBuffCount(Buffs.damageGrowth) == StaticValues.growthBuffMaxStacks);
                bool procCharged = (Util.CheckRoll(procCoefficient * 100f, base.characterBody.master));
                double damageVal = lightningTickDamage * damageStat + ((StaticValues.growthDamageCoefficient * base.GetBuffCount(Buffs.damageGrowth)) * lightningTickDamage * damageStat);

                SyncDamage sync = new SyncDamage(HurtBoxReference.FromHurtBox(targetHurtbox), rightMuzzleTransform, base.gameObject.GetComponent<NetworkIdentity>().netId, damageVal, isChaining, procCharged);
                sync.Send(R2API.Networking.NetworkDestination.Server);

                //if (NetworkServer.active) Debug.Log("firing orb on server");

            }

            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            soundTimer += Time.fixedDeltaTime;

            this.tickTimer -= Time.fixedDeltaTime;

            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.baseTickTime + 1f);
            }

            //if (this.tracker && base.isAuthority)
            if (this.tracker)
            {
                if (!muzzleHasFlashed && this.targetHurtbox)
                {
                    muzzleFlashObject = UnityEngine.Object.Instantiate<GameObject>(muzzleFlashEffect, rightMuzzleTransform);
                    muzzleHasFlashed = true;
                }

                if (!muzzleHasFlashed && this.targetHurtbox)
                {
                    muzzleFlashObject = UnityEngine.Object.Instantiate<GameObject>(muzzleFlashEffect, rightMuzzleTransform);
                    muzzleHasFlashed = true;
                }

                if (targetHurtbox && !lightningTetherActive)
                {
                    /*  rightMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;
                      SyncTransform sync = new SyncTransform(rightMuzzleTransform);
                      sync.Send(R2API.Networking.NetworkDestination.Clients);
                      sync.Send(R2API.Networking.NetworkDestination.Server); */
                    rightMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;
                    lightningEffectController.CreateLightningTether(base.gameObject, rightMuzzleTransform);
                    lightningTetherActive = true;
                }

                if (soundTimer >= .1f && !hasBegunSound && targetHurtbox)
                {
                    endLoopSoundID = Util.PlaySound(loopSoundString, base.gameObject);
                    hasBegunSound = true;
                }




            }
            
            this.FireLightning();

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();

            
            if ((base.isAuthority && !base.inputBank.skill1.down) || (base.isAuthority && this.targetHurtbox && this.targetHurtbox.healthComponent.health <= 0) || (base.isAuthority && !this.targetHurtbox) || !this.skillLocator.isActiveAndEnabled)
            {
                this.outer.SetNextStateToMain();
            }
        }

      /*  private NemAmpLightningLockOrb createDmgOrb()
        {
            if (targetHurtbox)
            {
               // Debug.Log("creating orb on hurtbox");
                //Debug.Log(targetHurtbox.gameObject.GetComponent<CharacterBody>().name + "is target");
            }

            if (!targetHurtbox)
            {
               Debug.Log("hurtbox for orb not found");
            }

            Debug.Log("creating damage orb");
            targetHurtbox = tracker.GetTrackingTarget();
            NemAmpLightningLockOrb lockOrb = new NemAmpLightningLockOrb();

            rightMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;
            //SyncTransform sync = new SyncTransform(rightMuzzleTransform);
           // sync.Send(R2API.Networking.NetworkDestination.Clients);
            //sync.Send(R2API.Networking.NetworkDestination.Server);
            lockOrb.damageValue = lightningTickDamage * damageStat + ((StaticValues.growthDamageCoefficient * base.GetBuffCount(Buffs.damageGrowth)) * lightningTickDamage * damageStat);
            lockOrb.origin = rightMuzzleTransform.position;
            lockOrb.isCrit = base.characterBody.RollCrit();
            lockOrb.damageType = DamageType.Generic;
            lockOrb.teamIndex = teamComponent.teamIndex;
            lockOrb.attacker = base.gameObject;
            lockOrb.procCoefficient = .2f;
            lockOrb.damageColorIndex = DamageColorIndex.Default;
            lockOrb.target = targetHurtbox;
            lockOrb.bouncedObjects = new List<HealthComponent>();
            lockOrb.range = tracker.maxTrackingDistance;
            lockOrb.damageCoefficientPerBounce = .8f;
            lockOrb.nemLightningColorController = this.lightningController;

            Debug.Log("lockorb damage value is " + lockOrb.damageValue);
            return lockOrb;
        } */


        public override void OnExit()
        {
            base.OnExit();
            AkSoundEngine.StopPlayingID(endLoopSoundID, 0);
            //Util.PlaySound(endSoundString, base.gameObject);

            lightningEffectController.isAttacking = false;
            //Debug.Log("Exiting");
            this.FireLightning();
            //Debug.Log("lightning fire method completed");
            if (nexusMuzzleTransform)
            {
                EntityState.Destroy(nexusMuzzleTransform.gameObject);
            }

            lightningEffectController.DestroyLightningTether();
            lightningTetherActive = false;
            animator.SetBool("NemIsFulminating", false);

            if (muzzleFlashObject)
            {
                //Destroy(muzzleFlashObject);
            }

        }
        /*
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.targetHurtbox));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            this.targetHurtbox = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
        */
        
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

   
    }
}
