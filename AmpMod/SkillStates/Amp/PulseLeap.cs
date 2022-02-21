using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using EntityStates;
using R2API;
using UnityEngine.Networking;

namespace AmpMod.SkillStates
{
    class PulseLeap : BaseSkillState
    {
 
        private BlastAttack boostBlast;
        private string launchSound = Modules.StaticValues.boltExitString;
        private float launchDamage = Modules.StaticValues.boostDamageCoefficient;
        private GameObject launchEffect;
        private float aerialBoostCoefficient = 50f;
        private float groundXZBoostCoefficient = 50f;
        private float groundYBoostCoefficient = 30f;
        private float initialGroundedHopCoefficient = 10f;


        public override void OnEnter()
        { 

            base.OnEnter();
            

            Util.PlaySound(launchSound, base.gameObject);
           

        }

        //create blastAttack on launch
        public void FireLaunchBlast()
        {
            boostBlast = new BlastAttack
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

            boostBlast.AddModdedDamageType(Modules.DamageTypes.applyCharge);
            boostBlast.Fire();

            EffectData effectData = new EffectData
            {
                origin = base.transform.position,
                scale = 1.5f
            };
            launchEffect = Modules.Assets.boltExitEffect;
            //exitEffectPrefab = Modules.Assnhbvets.testLightningEffect;
            EffectManager.SpawnEffect(launchEffect, effectData, true);


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.characterMotor)
            {
                

                //makes it so boost uses absolute value of y component when grounded so they cant just aim downwards and make the explosion w/o launching
                if (base.characterMotor.isGrounded)
                {
                   
                 base.characterMotor.velocity.y = initialGroundedHopCoefficient;
                    

                    base.characterMotor.Motor.ForceUnground();
                    //base.characterMotor.velocity.y = 0;

                    //launch in direction of aimray
                    base.characterMotor.velocity += new Vector3(base.GetAimRay().direction.x * groundXZBoostCoefficient, Math.Abs(base.GetAimRay().direction.y)*groundYBoostCoefficient, base.GetAimRay().direction.z * groundXZBoostCoefficient);
                }

                else
                {
                    //set y velocity = 0 to make aerial boosting actually useful
                    base.characterMotor.velocity.y = 0;

                    //launch in direction of aimray
                    base.characterMotor.velocity += base.GetAimRay().direction * aerialBoostCoefficient;

                }
                
                
                FireLaunchBlast();
            }

            

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        //hook to charactermotor to remove fall damage
        private void CharacterMotor_onHitGround(ref CharacterMotor.HitGroundInfo hitGroundInfo)
        {
            if (base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
            {
                base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            }

 

            base.characterMotor.onHitGround -= this.CharacterMotor_onHitGround;
        }


        public override void OnExit()
        {

            //remove fall damage check
            if (NetworkServer.active && !base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
            {
                base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                base.characterMotor.onHitGround += this.CharacterMotor_onHitGround;
            }

            base.OnExit();
        }

 

    }
}
