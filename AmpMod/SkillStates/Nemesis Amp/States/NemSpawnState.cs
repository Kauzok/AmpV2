using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using UnityEngine;
using UnityEngine.Networking;
using R2API;
using AmpMod.Modules;
using static RoR2.UI.CrosshairController;

namespace AmpMod.SkillStates.Nemesis_Amp.Components
{
    internal class NemSpawnState : BaseState
    {
        public static float lightningSpawnDuration = 5f;
        private float spawnNormalDuration = 4f;
        private Transform modelTransform;
        private Animator animator;
        private float waitDuration = 1.5f;
        private ChildLocator childLocator;
        private float strikeRadius = 10f;
        private CharacterBody characterBody;
        private CameraTargetParams.AimRequest aimRequest;
        private CharacterModel characterModel;
        private bool hasTeleported;
        private bool hasFired;
        private bool spawnIndicator => Config.NemStormRangeIndicator.Value;
        private GameObject indicatorInstance;
        private GameObject indicatorPrefab;
        private bool spawnWithLightning => Run.instance.stageClearCount == 0;
        private bool isBlue;
        
        public override void OnEnter()
        {
            base.OnEnter();
            this.animator = base.GetModelAnimator();
            this.modelTransform = base.GetModelTransform();
            this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            childLocator = base.GetModelChildLocator();
            characterBody = base.GetComponent<CharacterBody>();
            //Debug.Log("spawning effect");

            //make charactermodel invisible upon first spawning in; this will happen regardless of if we're doing a spawn with lightning or not
            if (this.characterModel)
            {
                 this.characterModel.invisibilityCount++;
            }

            //determine if we're blue (can't use lightningcolorcontroller here for some reason 
            if (this.characterModel.GetComponent<ModelSkinController>().skins[this.characterBody.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME" && !Config.NemOriginPurpleLightning.Value)
            {
                isBlue = true;
                indicatorPrefab = Assets.stormRangeIndicatorBlue;

            }
            else
            {
                indicatorPrefab = Assets.stormRangeIndicator;
            }

            //don't change camera params for normal spawn
            if (spawnWithLightning)
            {
                if (NetworkServer.active) base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, NemSpawnState.lightningSpawnDuration * 1.5f);
            }

            //change camera params for normal spawn
            else if (!spawnWithLightning)
            {
                if (base.cameraTargetParams)
                {
                    this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
                }
                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                }
            }

       
      

        }
        private void spawnTPEffect()
        {
            this.hasTeleported = true;
            
            this.spawnNormalDuration = SpawnTeleporterState.initialDelay;
            TeleportOutController.AddTPOutEffect(this.characterModel, 1f, 0f, this.spawnNormalDuration);
            GameObject teleportEffectPrefab = Run.instance.GetTeleportEffectPrefab(base.gameObject);
            if (teleportEffectPrefab)
            {
                EffectManager.SimpleEffect(teleportEffectPrefab, base.transform.position, Quaternion.identity, true);
            }
            Util.PlaySound(SpawnTeleporterState.soundString, base.gameObject);
        }

        private void FireBlast()
        {
            BlastAttack lightningStrike = new BlastAttack
            {
                attacker = base.gameObject,
                baseDamage = 1000f * base.characterBody.damage,
                baseForce = 2f,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                crit = base.characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = base.gameObject,
                position = base.gameObject.transform.position,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1f,
                radius = this.strikeRadius,
                teamIndex = base.characterBody.teamComponent.teamIndex
            };
            lightningStrike.Fire();
            Util.PlaySound("Play_item_use_lighningArm", base.gameObject);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //controls spawn of the range indicator
            if (spawnIndicator && !indicatorInstance && base.isAuthority)
            {
                indicatorInstance = UnityEngine.Object.Instantiate<GameObject>(indicatorPrefab, base.characterBody.corePosition, Quaternion.identity);
                indicatorInstance.transform.parent = base.gameObject.transform;
            }

            //if on the first stage spawn with lightning bolt
            if (spawnWithLightning)
            {
                if (base.fixedAge >= this.waitDuration && !hasFired)
                {
                    if (this.characterModel)
                    {
                        this.characterModel.invisibilityCount--;
                    }

                    if (base.isAuthority)
                    {
                        if (isBlue)
                        {
                            if (childLocator.FindChild("SpawnEffectBlue")) childLocator.FindChild("SpawnEffectBlue").gameObject.SetActive(true);
                        }
                        else
                        {
                            if (childLocator.FindChild("SpawnEffect")) childLocator.FindChild("SpawnEffect").gameObject.SetActive(true);
                        }
                        FireBlast();

                        
                    }

                    base.PlayAnimation("Spawn, Override", "Spawn", "Spawn.playbackRate", 4f);
                    hasFired = true;

                }

                if (base.fixedAge >= lightningSpawnDuration && base.isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }

            }

            //if not on first stage spawn with normal teleporter vfx
            else if (!spawnWithLightning)
            {
                if (base.fixedAge >= waitDuration && !hasTeleported)
                {
                    spawnTPEffect();
                    this.characterModel.invisibilityCount--;
                }
                if (base.fixedAge >= this.spawnNormalDuration && base.isAuthority)
                {
                    this.outer.SetNextStateToMain();
                    return;
                }
            }



        }

        public override void OnExit()
        {
            base.OnExit();
            if (!spawnWithLightning)
            {
                CameraTargetParams.AimRequest aimRequest = this.aimRequest;
                if (aimRequest != null)
                {
                    aimRequest.Dispose();
                }

                base.characterBody.RemoveBuff(RoR2Content.Buffs.HiddenInvincibility);
                base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 3f);
            }

            //final check to remove invisibility in case the player is still invisible for whatever reason
            if (this.characterModel)
            {
                this.characterModel.invisibilityCount = 0;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
