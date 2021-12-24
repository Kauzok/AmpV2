using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using EntityStates;
using RoR2.Projectile;
using HenryMod.Modules;
using UnityEngine.Networking;
using RoR2.Orbs;

namespace HenryMod.SkillStates.Henry
{
    public class VoltaicBombardmentFire : BaseSkillState
    {
        public GameObject projectilePrefab = Modules.Projectiles.lightningPrefab;
        public GameObject muzzleflashEffectPrefab;
        public float baseDuration;
        public Vector3 boltPosition;
        public Quaternion spellRotation;
        private float duration = 1f;
        public float charge;
        private float lightningChargeTimer;
        public GameObject lightningStrikeEffect;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;

            //base.PlayAnimation("Gesture, Override", "ThrowSpell", "Spell.playbackRate", this.duration);

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, "HandL", false);
            }
            this.Fire();    
            
        }

        public override void FixedUpdate()
        {
            EffectData effectData = new EffectData
            {
                origin = this.boltPosition,
                scale = 10f,

            };
            lightningStrikeEffect = Modules.Assets.electricExplosionEffect;

            EffectManager.SpawnEffect(lightningStrikeEffect, effectData, true);

            lightningChargeTimer = 2f;
            if (NetworkServer.active)
            {
                if (lightningChargeTimer > 0f)
                {
                    lightningChargeTimer -= Time.fixedDeltaTime;

                    if (lightningChargeTimer <= 0f)
                    {
                        Fire();
                          
                    }
                    
                    
                }

                if (base.isAuthority && base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                } 
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            

            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();

                BlastAttack lightningStrike = new BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = Modules.StaticValues.lightningStrikeCoefficient * base.characterBody.damage,
                    baseForce = 2f,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.Linear,
                    inflictor = base.gameObject,
                    position = this.boltPosition,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = 7f,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                };
                lightningStrike.Fire(); 


            }
        } 

            /* if (this.projectilePrefab != null)
               {
                   FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                   {
                       projectilePrefab = this.projectilePrefab,
                       position = this.boltPosition,
                       rotation = this.spellRotation,
                       owner = base.gameObject,
                       damage = this.damageStat,
                       force = 5f,
                       crit = base.RollCrit()
                   };

                   ProjectileManager.instance.FireProjectile(fireProjectileInfo); 
               } */

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }

}


        




