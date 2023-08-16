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
        public static float duration = 5f;
        private Transform modelTransform;
        private Animator animator;
        private float waitDuration = 1.5f;
        private ChildLocator childLocator;
        private float strikeRadius = 10f;
        private CharacterBody characterBody;
        private CharacterModel characterModel;
        private bool hasFired;
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
            if (NetworkServer.active) base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, NemSpawnState.duration * 1.5f);
            if (this.characterModel)
            {
                this.characterModel.invisibilityCount++;
            }
            if (this.characterModel.GetComponent<ModelSkinController>().skins[this.characterBody.skinIndex].nameToken == AmpPlugin.developerPrefix + "_NEMAMP_BODY_MASTERY_SKIN_NAME")
            {
                isBlue = true;
            }
       
            

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

            if (base.fixedAge >= NemSpawnState.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
