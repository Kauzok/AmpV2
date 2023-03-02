using System;
using System.Collections.Generic;
using System.Text;
using AmpMod.SkillStates.Amp.BaseStates;
using UnityEngine;
using RoR2;
using EntityStates;
using RoR2.Projectile;
using UnityEngine.Networking;
using R2API;
using RoR2.UI;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class AimStaticField : BaseState
    {
        private float duration = 25f;
        private Animator animator;
        private ChildLocator childLocator;
        private GameObject fieldIndicatorInstance;
        private ProjectileDotZone projectileDotZone;
        private bool goodPlacement;
        private float stopwatch;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;
        public static float maxSlopeAngle = 70f;
        public static float maxDistance = 400f;
        public static GameObject goodCrosshairPrefab = EntityStates.Mage.Weapon.PrepWall.goodCrosshairPrefab;
        public static GameObject projectilePrefab = Modules.Projectiles.fieldProjectilePrefab;
        public static GameObject badCrosshairPrefab = EntityStates.Mage.Weapon.PrepWall.badCrosshairPrefab;
        public static float damageCoefficient = Modules.StaticValues.staticFieldTickDamageCoefficient;
        private StackDamageController stackDamageController;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            animator = base.GetModelAnimator();
            childLocator = base.GetComponent<ChildLocator>();
            fieldIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.staticFieldIndicatorPrefab);
            base.PlayAnimation("Gesture, Override", "AimField", "BaseSkill.playbackRate", 0.4f);
            //Debug.Log(fieldIndicatorInstance);
            this.UpdateAreaIndicator();

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
            if ((this.stopwatch >= this.duration && base.isAuthority) || (!base.inputBank.skill3.down && base.isAuthority))
            {
                this.outer.SetNextStateToMain();
            }

        }

        public override void Update()
        {
            base.Update();
            this.UpdateAreaIndicator();
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Pain;
        }

        private void UpdateAreaIndicator()
        {
            bool flag = this.goodPlacement;
            this.goodPlacement = false;
            this.fieldIndicatorInstance.SetActive(true);
            if (this.fieldIndicatorInstance)
            {
                float num = maxDistance;
                float num2 = 0f;
                Ray aimRay = base.GetAimRay();
                RaycastHit raycastHit;
                if (Physics.Raycast(CameraRigController.ModifyAimRayIfApplicable(aimRay, base.gameObject, out num2), out raycastHit, num + num2, LayerIndex.world.mask))
                {
                    this.fieldIndicatorInstance.transform.position = raycastHit.point;
                    this.fieldIndicatorInstance.transform.up = raycastHit.normal;
                    this.fieldIndicatorInstance.transform.forward = -aimRay.direction;
                    this.goodPlacement = (Vector3.Angle(Vector3.up, raycastHit.normal) < maxSlopeAngle);
                }
                if (flag != this.goodPlacement || this.crosshairOverrideRequest == null)
                {
                    CrosshairUtils.OverrideRequest overrideRequest = this.crosshairOverrideRequest;
                    if (overrideRequest != null)
                    {
                        overrideRequest.Dispose();
                    }
                    GameObject crosshairPrefab = this.goodPlacement ? goodCrosshairPrefab : badCrosshairPrefab;
                    this.crosshairOverrideRequest = CrosshairUtils.RequestOverrideForBody(base.characterBody, crosshairPrefab, CrosshairUtils.OverridePriority.Skill);
                }
            }
            this.fieldIndicatorInstance.SetActive(this.goodPlacement);
        }

        public override void OnExit()
        {
            if (!this.outer.destroying)
            {
                if (this.goodPlacement)
                {
                    animator.SetBool("hasFired", true);
                    float baseDamage = (StaticValues.growthDamageCoefficient * base.GetBuffCount(Buffs.damageGrowth) * damageCoefficient) + damageCoefficient;
                    base.PlayAnimation("Gesture, Override", "ReleaseField", "ShootGun.playbackRate", .5f);
                    //Util.PlaySound(PrepWall.fireSoundString, base.gameObject);
                    if (this.fieldIndicatorInstance && base.isAuthority)
                    {
                        //EffectManager.SimpleMuzzleFlash(muzzleflashEffect, base.gameObject, "MuzzleLeft", true);
                        //EffectManager.SimpleMuzzleFlash(muzzleflashEffect, base.gameObject, "MuzzleRight", true);
                        Vector3 forward = this.fieldIndicatorInstance.transform.forward;
                        forward.y = 0f;
                        forward.Normalize();
                        Vector3 vector = Vector3.Cross(Vector3.up, forward);
                        bool crit = Util.CheckRoll(this.critStat, base.characterBody.master);
                        
                        ProjectileManager.instance.FireProjectile(projectilePrefab, this.fieldIndicatorInstance.transform.position + Vector3.up, Util.QuaternionSafeLookRotation(vector), base.gameObject, this.damageStat * baseDamage, 0f, crit, DamageColorIndex.Default, null, -1f);
                        ProjectileManager.instance.FireProjectile(projectilePrefab, this.fieldIndicatorInstance.transform.position + Vector3.up, Util.QuaternionSafeLookRotation(-vector), base.gameObject, this.damageStat * baseDamage, 0f, crit, DamageColorIndex.Default, null, -1f);
                    }
                }
                else
                {
                    animator.SetBool("failedToFire", true);
                    base.skillLocator.utility.AddOneStock();
                    base.PlayCrossfade("Gesture, Override", "BufferEmpty", 0.2f);
                }
            }
            EntityState.Destroy(this.fieldIndicatorInstance.gameObject);
            CrosshairUtils.OverrideRequest overrideRequest = this.crosshairOverrideRequest;
            if (overrideRequest != null)
            {
                overrideRequest.Dispose();
            }

            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
            base.OnExit();


        }
    }

}
