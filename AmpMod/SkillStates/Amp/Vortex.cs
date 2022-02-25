using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using UnityEngine;
using RoR2;
using RoR2.Projectile;

namespace AmpMod.SkillStates
{
    class Vortex : BaseSkillState
    {
        public static float tickDamage = Modules.StaticValues.vortexDamageCoefficient;
        public float duration;
        public GameObject Vortexprefab = Modules.Projectiles.vortexPrefab;
        public float explodeDamage = Modules.StaticValues.vortexExplosionCoefficient;
        public Vector3 blastPosition;

        public override void OnEnter()
        {
            base.OnEnter();

            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);

           
                       

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Fire();

            if (base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }
        private void Fire()
        {
            if (base.isAuthority)
            {
                Ray aimRay = base.GetAimRay();
                if (Vortexprefab != null)
                {
                    var vortexDamage = Modules.Assets.vortexBlackholePrefab.GetComponent<RadialDamage>();
                    vortexDamage.blastDamage = tickDamage;
                    vortexDamage.charBody = base.characterBody;
                    vortexDamage.radius = 5f;
                    vortexDamage.finalBlastDamage = explodeDamage;
                    vortexDamage.attacker = base.gameObject;
                    vortexDamage.duration = Modules.Assets.vortexBlackholePrefab.GetComponent<ProjectileSimple>().lifetime;
                    blastPosition = vortexDamage.transform.position;


                    FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
                    {
                        projectilePrefab = Vortexprefab,
                        position = aimRay.origin,
                        rotation = Util.QuaternionSafeLookRotation(aimRay.direction),
                        owner = base.gameObject,
                        damage = 0f,
                        force = Vortexprefab.GetComponent<ProjectileSimple>().desiredForwardSpeed,
                        crit = base.RollCrit()
                    };
                    ModifyProjectile(ref fireProjectileInfo);
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            
            }
        }

        protected virtual void ModifyProjectile(ref FireProjectileInfo projectileInfo)
        {
        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }

}
