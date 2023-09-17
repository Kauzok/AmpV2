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
            if (spawnWithLightning)
            {
                if (NetworkServer.active) base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, NemSpawnState.lightningSpawnDuration * 1.5f);
                if (this.characterModel)
                {
                    this.characterModel.invisibilityCount++;
                }
                if (this.characterModel.GetComponent<ModelSkinController>().skins[this.characterBody.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME")
                {
                    isBlue = true;
                }
            }

            else
            {
                if (base.cameraTargetParams)
                {
                    this.aimRequest = base.cameraTargetParams.RequestAimType(CameraTargetParams.AimType.Aura);
                }
                this.characterModel.invisibilityCount++;
                if (NetworkServer.active)
                {
                    base.characterBody.AddBuff(RoR2Content.Buffs.HiddenInvincibility);
                }
            }
        
       
      

        }
        private void spawnTPEffect()
        {
            this.hasTeleported = true;
            this.characterModel.invisibilityCount--;
            this.spawnNormalDuration = SpawnTeleporterState.initialDelay;
            TeleportOutController.AddTPOutEffect(this.characterModel, 1f, 0f, this.spawnNormalDuration);
            GameObject teleportEffectPrefab = Run.instance.GetTeleportEffectPrefab(base.gameObject);
            if (teleportEffectPrefab)
            {
                EffectManager.SimpleEffect(teleportEffectPrefab, base.transform.position, Quaternion.identity, false);
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

                //blastattack is positioned 10 units above where the reticle is placed
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

            if (spawnWithLightning)
            {
                if (base.fixedAge >= this.waitDuration && base.isAuthority && !hasFired)
                {
                    if (this.characterModel)
                    {
                        this.characterModel.invisibilityCount--;
                    }
                    if (isBlue)
                    {
                        if (childLocator.FindChild("SpawnEffectBlue")) childLocator.FindChild("SpawnEffectBlue").gameObject.SetActive(true);
                    }
                    else
                    {
                        if (childLocator.FindChild("SpawnEffect")) childLocator.FindChild("SpawnEffect").gameObject.SetActive(true);
                    }

                    FireBlast();

                    base.PlayAnimation("Spawn, Override", "Spawn", "Spawn.playbackRate", 4f);
                    hasFired = true;
                }
            }

            else if (!spawnWithLightning)
            {
                if (base.fixedAge >= waitDuration && base.isAuthority && !hasTeleported)
                {
                    spawnTPEffect();
                }
            }


            if (base.fixedAge >= lightningSpawnDuration && base.isAuthority && spawnWithLightning)
            {
                this.outer.SetNextStateToMain();
                return;
            }

            if (base.fixedAge >= this.spawnNormalDuration && base.isAuthority && !spawnWithLightning)
            {
                this.outer.SetNextStateToMain();
                return;
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
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
