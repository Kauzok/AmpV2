using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using System.Collections;

namespace HenryMod.SkillStates
{
    public class Ferroshot : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.ferroshotDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        public static float force = 10;
        public static float recoil = 0f;
        public static float range = 600f;
        private Ray ferroshotRay;
        private Vector3 newpos;
        public static float launchForce = 150f;

        private Animator animator;

        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = Ferroshot.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);// 3f * this.duration);
        }

        public override void OnExit()
        {

            base.OnExit();
        }

        IEnumerator Fire()
        {
            if (!this.hasFired)
            {

                this.hasFired = true;
                Util.PlaySound("HenryBombThrow", base.gameObject);

                if (base.isAuthority)
                {
                    for(int i = 0; i<6; i++)
                    {
                        Ray aimRay = base.GetAimRay();

                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.ferroshotPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        Ferroshot.damageCoefficient * this.damageStat,
                        Ferroshot.launchForce,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        Ferroshot.launchForce);

                        yield return new WaitForSeconds(.1f);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                CoroutineRunner.RunCoroutine(Fire());
                //this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}