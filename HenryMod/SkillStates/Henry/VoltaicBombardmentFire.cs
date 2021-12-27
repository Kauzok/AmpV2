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
using R2API;

namespace HenryMod.SkillStates.Henry
{
    public class VoltaicBombardmentFire : BaseSkillState
    {
        public GameObject muzzleflashEffectPrefab;

        public float baseDuration;
        public Vector3 boltPosition;
        public Quaternion lightningRotation;
        private float duration = 1f;
        public float charge;
       static public float lightningChargeTimer = .5f;
        public GameObject lightningStrikeEffect;
        public GameObject lightningStrikeExplosion;
        bool hasFired;
        public float strikeRadius = 12f;
        public GameObject projectilePrefab = Modules.Projectiles.bombPrefab;

        public override void OnEnter()
        {

            base.OnEnter();
          

            hasFired = false;
            this.duration = this.baseDuration / this.attackSpeedStat;

            //base.PlayAnimation("Gesture, Override", "ThrowSpell", "Spell.playbackRate", this.duration);

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, "HandL", false);
            }
            
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

                if (!hasFired)
                {
                    if (base.fixedAge > lightningChargeTimer)
                    {
                    hasFired = true;
                    Fire();
                }

                this.duration = .5f;
                }
            

                if (base.isAuthority && base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
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

                lightningStrikeEffect = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact");
               
                lightningStrikeExplosion = Resources.Load<GameObject>("Prefabs/Effects/MageLightningBombExplosion");


                EffectData lightning = new EffectData
                {
                    origin = this.boltPosition,
                    scale = 1f,
                };
                
                EffectData lightningExplosion = new EffectData
                {
                    origin = this.boltPosition,
                    scale = 20f,

                };

                EffectManager.SpawnEffect(lightningStrikeEffect, lightning, true);
                EffectManager.SpawnEffect(lightningStrikeExplosion, lightningExplosion, true);

                BlastAttack lightningStrike = new BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = Modules.StaticValues.lightningStrikeCoefficient * base.characterBody.damage,
                    baseForce = 2f,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    position = this.boltPosition + Vector3.up * 10,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = this.strikeRadius,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                };
                lightningStrike.AddModdedDamageType(Modules.DamageTypes.apply2Charge);

                lightningStrike.Fire();


                /*
                   FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {

                        projectilePrefab = Modules.Projectiles.bombPrefab,
                        position = this.boltPosition + Vector3.up * 10f,
                        rotation = ,
                        owner = base.gameObject,
                        damage = this.damageStat,
                        force = 5f,
                        crit = base.RollCrit(),
                        speedOverride = 10f,

                        
                       
                  };
                   ProjectileManager.instance.FireProjectile(fireProjectileInfo);

                 */

            }
        } 

           

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }

}


        




