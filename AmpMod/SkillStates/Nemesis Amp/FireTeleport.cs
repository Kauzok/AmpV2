using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using EntityStates;
using R2API;
using RoR2.Skills;
using UnityEngine;
using AmpMod.SkillStates.Amp.BaseStates;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class FireTeleport : BaseSkillFire
    {
        private Animator animator;
        private ChildLocator childLocator;
        private StackDamageController stackDamageController;
        private BlastAttack teleportBlast;
        private float teleportBlastDamage = Modules.StaticValues.teleportBlastDamageCoefficient;
        private float teleportBlastRadius = Modules.StaticValues.teleportBlastRadius;

        public override void OnEnter()
        {
            stackDamageController = base.GetComponent<StackDamageController>();
            base.OnEnter();
            

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();

            DoTeleport();
            FireTeleportBlast();

            
        }

        public void DoTeleport()
        {
            TeleportHelper.TeleportGameObject(base.gameObject, aimPosition);
        
        }


        public void FireTeleportBlast()
        {
            if (base.isAuthority)
            {

                teleportBlast = new BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = this.teleportBlastDamage * base.characterBody.damage,
                    baseForce = 0f,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    position = base.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = this.teleportBlastRadius,
                    teamIndex = base.characterBody.teamComponent.teamIndex

                };

                teleportBlast.AddModdedDamageType(Modules.DamageTypes.controlledChargeProc);
                teleportBlast.Fire();
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
