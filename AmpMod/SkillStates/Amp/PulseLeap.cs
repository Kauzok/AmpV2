using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using EntityStates;
using R2API;
using UnityEngine.Networking;
using AmpMod;

namespace AmpMod.SkillStates
{
    class PulseLeap : BaseSkillState
    {
 
        private float speedCoefficient = 15f;
        private BlastAttack boltBlast;
        private string launchSound = Modules.StaticValues.boltExitString;
        private float launchDamage = Modules.StaticValues.boostDamageCoefficient;
        private GameObject launchEffect;



        public override void OnEnter()
        { 

            base.OnEnter();


            Util.PlaySound(launchSound, base.gameObject);
       
            
           

        }

        public void FireLaunchBlast()
        {
            boltBlast = new BlastAttack
            {
                attacker = base.gameObject,
                baseDamage = launchDamage * base.characterBody.damage,
                baseForce = 0f,
                attackerFiltering = AttackerFiltering.NeverHit,
                crit = base.characterBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.Stun1s,
                falloffModel = BlastAttack.FalloffModel.None,
                inflictor = base.gameObject,
                position = base.characterBody.corePosition,
                procChainMask = default(ProcChainMask),
                procCoefficient = 1f,
                radius = 6f,
                teamIndex = base.characterBody.teamComponent.teamIndex
            };

            boltBlast.AddModdedDamageType(Modules.DamageTypes.applyCharge);
            boltBlast.Fire();

            EffectData effectData = new EffectData
            {
                origin = base.transform.position,
                scale = 1.5f
            };
            launchEffect = Modules.Assets.boltExitEffect;
            //exitEffectPrefab = Modules.Assets.testLightningEffect;
            EffectManager.SpawnEffect(launchEffect, effectData, true);


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.characterMotor)
            {
                

                if (base.characterMotor.isGrounded)
                {
                    base.characterMotor.Motor.ForceUnground();
                    base.characterMotor.velocity = new Vector3(base.GetAimRay().direction.x * 50, 40*Math.Abs(base.GetAimRay().direction.y), base.GetAimRay().direction.z * 50);
                }
                else
                {
                    base.characterMotor.velocity = base.GetAimRay().direction * 50f;
                }
                
                
                FireLaunchBlast();
            }

           

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
          

            base.OnExit();
        }

        private void CharacterMotor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            if (base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
            {
                base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            }

            // TODO: May need to redo the flag assignment?

            base.characterMotor.onHitGround -= this.CharacterMotor_onHitGround;
        }

       

        //public override void UpdateAnimationParameters() => base.UpdateAnimationParameters();

    }
}
