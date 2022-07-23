using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;
using EntityStates;
using R2API;
using KinematicCharacterController;
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
        private ChildLocator childLocator;
        private Vector3 boostVelocity;
        private float charge = 1f;

        public float minLungeSpeed = 30f;

        public float maxLungeSpeed = 100f;


        public override void OnEnter()
        { 

            base.OnEnter();

            

            Transform modelTransform = base.GetModelTransform();
            childLocator = modelTransform.GetComponent<ChildLocator>();

            if (!base.characterMotor.isGrounded && base.GetAimRay().direction.y < .5) {
            base.PlayAnimation("FullBody, Override", "BoostAerial", "ShootGun.playbackRate", .1f);
            }
            else
            {
               base.PlayAnimation("Fulminate, Override", "BoostGrounded", "ShootGun.playbackRate", .1f);
            }
            
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
                attackerFiltering = AttackerFiltering.NeverHitSelf,
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
                origin = childLocator.FindChild("FootL").position,
                scale = 1.5f,
            };
            launchEffect = Modules.Assets.boltExitEffect;
            //exitEffectPrefab = Modules.Assnhbvets.testLightningEffect;
            EffectManager.SpawnEffect(launchEffect, effectData, true);


        }

        private void DoBoost()
        {
            if (base.characterMotor.isGrounded)
            {
                //makes it so boost uses absolute value of y component when grounded so they cant just aim downwards and make the explosion w/o launching
                base.characterMotor.velocity.y = initialGroundedHopCoefficient;


                base.characterMotor.Motor.ForceUnground();
                //base.characterMotor.velocity.y = 0;

                //launch in direction of aimray
                base.characterMotor.velocity += new Vector3(base.GetAimRay().direction.x * groundXZBoostCoefficient, Math.Abs(base.GetAimRay().direction.y) * groundYBoostCoefficient, base.GetAimRay().direction.z * groundXZBoostCoefficient);
            }

            else
            {
                //set y velocity = 0 to make aerial boosting actually useful
                base.characterMotor.velocity.y = 0;

                //launch in direction of aimray
                base.characterMotor.velocity += base.GetAimRay().direction * aerialBoostCoefficient;

            }


        }

        public static Vector3 CalculateLungeVelocity(Vector3 currentVelocity, Vector3 aimDirection, float charge, float lungeSpeed)
        {
            currentVelocity = ((Vector3.Dot(currentVelocity, aimDirection) < 0f) ? Vector3.zero : Vector3.Project(currentVelocity, aimDirection));
            return currentVelocity + aimDirection * lungeSpeed;
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.characterMotor)
            {

                base.characterMotor.velocity = boostVelocity;

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

 

            base.characterMotor.onHitGroundServer -= this.CharacterMotor_onHitGround;
        }


        public override void OnExit()
        {

            //remove fall damage check
            if (NetworkServer.active && !base.characterBody.bodyFlags.HasFlag(CharacterBody.BodyFlags.IgnoreFallDamage))
            {
                base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                base.characterMotor.onHitGroundServer += this.CharacterMotor_onHitGround;
            }

            base.OnExit();
        }

 

    }
}
