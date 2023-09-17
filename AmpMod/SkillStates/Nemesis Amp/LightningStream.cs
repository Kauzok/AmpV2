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

        public class SyncTarget : INetMessage
        {
            NetworkInstanceId playerNetId;
            NetworkInstanceId targetdNetId;
            HurtBox targetHurtbox;
            HurtBoxReference targetHurtboxReference;

            public SyncTarget()
            {

            }

            public SyncTarget(NetworkInstanceId playerId, NetworkInstanceId trackedId, HurtBox targetHurtbox)
            {
                this.playerNetId = playerId;
                this.targetdNetId = trackedId;
                this.targetHurtbox = targetHurtbox;
            }

            //we then read the targethurtbox as a hurtboxreference
            public void Deserialize(NetworkReader reader)
            {
                playerNetId = reader.ReadNetworkId();
                targetdNetId = reader.ReadNetworkId();
                targetHurtboxReference = reader.ReadHurtBoxReference();
                
                
            }


            //give the syncmessage the hurtbox you want to sync TO (will use hurtbox == playerObject.gettrackingtarget)
            public void OnReceived()
            {
                if (NetworkServer.active)
                {
                    Debug.Log("SyncTarget: Host ran this. Skip.");
                    return;
                }

                GameObject playerObject = Util.FindNetworkObject(playerNetId);
                GameObject targetObject = Util.FindNetworkObject(targetdNetId);
                CharacterBody targetBody = targetObject.GetComponent<CharacterBody>();

                //sets target reference to what the player is tracking at the given time from clients
                //have to somehow modify the current instance of targetHurtbox in use
                HurtBox hurtbox = targetHurtboxReference.ResolveHurtBox();
                hurtbox =  playerObject.GetComponent<NemAmpLightningTracker>().GetTrackingTarget();

            }

            //start by writing the current targethurtbox to network as a hurtboxreference
            public void Serialize(NetworkWriter writer)
            {
                writer.Write(playerNetId);
                writer.Write(targetdNetId);
                writer.Write(HurtBoxReference.FromHurtBox(targetHurtbox));

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
            //lightningEffectController.lightningTetherVFX = lightningController.streamVFX;

            //Util.PlaySound(startSoundString, base.gameObject);

            tracker = base.GetComponent<NemAmpLightningTracker>();

            Transform modelTransform = base.GetModelTransform();
            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.animator = modelTransform.GetComponent<Animator>();
            }

            this.tickTime= this.baseTickTime / this.attackSpeedStat;

            //Debug.Log(base.characterBody.bodyIndex);
         
            if (tracker.GetTrackingTarget())
            {
                if (NetworkServer.active)
                {
                    this.targetHurtbox = tracker.GetTrackingTarget();
                }

                this.targetHurtbox = tracker.GetTrackingTarget();
                animator.SetBool("NemIsFulminating", true);
                //base.PlayAnimation("RightArm, Override", "ShootLightning", "BaseSkill.playbackRate", 0.4f);
                rightMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;
                base.PlayAnimation("RightArm, Override", "ShootLightning", "BaseSkill.playbackRate", 0.4f);

                this.nexusMuzzleTransform = UnityEngine.Object.Instantiate<GameObject>(muzzleEffect, rightMuzzleTransform).transform;
                //Debug.Log(nexusMuzzleTransform);
                 //Debug.Log(BodyCatalog.FindBodyIndex("NemAmpBody") + "isnemindex");
                //nexusMuzzleTransform = childLocator.FindChild("LightningNexusMuzzle").transform;
                //Debug.Log("starting at " + rightMuzzleTransform.position);
            }

            //base.PlayAnimation("Spawn, Override", "Spawn", "Spawn.playbackRate", 4f);
            //animations


        }

        private void FireLightning()
        {
            if (!NetworkServer.active && tickTimer <= 0f)
            {
                this.targetHurtbox = tracker.GetTrackingTarget();
            }

            if (!NetworkServer.active || tickTimer > 0f)
            {
                //Debug.Log("returning");
                return;
                
            }


            Debug.Log("not returning");

            this.tickTimer = this.tickTime;

            this.targetHurtbox = tracker.GetTrackingTarget();
           

            if (targetHurtbox)
            {
                Debug.Log(targetHurtbox.gameObject + "is target");
                NemAmpLightningLockOrb lightningOrb = createDmgOrb();
                lightningOrb.procControlledCharge = false;

                if (Util.CheckRoll(procCoefficient * 100f, base.characterBody.master))
                {
                    lightningOrb.procControlledCharge = true;
                }


                if (base.GetBuffCount(Buffs.damageGrowth) == StaticValues.growthBuffMaxStacks)
                {
                    //lightningOrb.isChaining = true;
                    lightningOrb.bouncesRemaining = 2;
                }
                else
                {
                    lightningOrb.isChaining = false;
                    lightningOrb.bouncesRemaining = 0;
                }
                OrbManager.instance.AddOrb(lightningOrb);
                Debug.Log("firing");
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

            if (this.tracker && base.isAuthority)
            //if (this.tracker)
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

            //Debug.Log("lightning fire method completed");

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();

            
            if ((base.isAuthority && !base.inputBank.skill1.down) || (base.isAuthority && this.targetHurtbox && this.targetHurtbox.healthComponent.health <= 0) || (base.isAuthority && !this.targetHurtbox) || !this.skillLocator.isActiveAndEnabled)
            {
                Debug.Log("Setting skill to exit");
                this.outer.SetNextStateToMain();
            }
        }

        private NemAmpLightningLockOrb createDmgOrb()
        {
            if (targetHurtbox)
            {
                Debug.Log("creating orb on hurtbox");
            }

            if (!targetHurtbox)
            {
                Debug.Log("hurtbox for orb not found");
            }

            
            return new NemAmpLightningLockOrb
            {
                origin = rightMuzzleTransform.position,
                damageValue = lightningTickDamage * damageStat + ((StaticValues.growthDamageCoefficient * base.GetBuffCount(Buffs.damageGrowth)) * lightningTickDamage * damageStat),
                isCrit = base.characterBody.RollCrit(),
                damageType = DamageType.Generic,
                teamIndex = teamComponent.teamIndex,
                attacker = base.gameObject,
                procCoefficient = .2f,
                damageColorIndex = DamageColorIndex.Default,
                target = targetHurtbox,
                bouncedObjects = new List<HealthComponent>(),
                range = tracker.maxTrackingDistance,
                damageCoefficientPerBounce = .8f,
                nemLightningColorController = this.lightningController,
            };
        }


        public override void OnExit()
        {
            base.OnExit();
            AkSoundEngine.StopPlayingID(endLoopSoundID, 0);
            //Util.PlaySound(endSoundString, base.gameObject);

            lightningEffectController.isAttacking = false;
            Debug.Log("Exiting");
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
