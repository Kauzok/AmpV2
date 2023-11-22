using UnityEngine;
using RoR2;
using EntityStates;
using RoR2.Projectile;
using RoR2.UI;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using AmpMod.Modules;
using UnityEngine.Networking;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class AimStaticField : BaseState
    {
        [Header("Timing Variables")]
        private float stopwatch;

        [Header("VFX/Animation Variables")]
        private Animator animator;
        private ChildLocator childLocator;
        private GameObject fieldIndicatorInstance;
        public static GameObject goodCrosshairPrefab = EntityStates.Mage.Weapon.PrepWall.goodCrosshairPrefab;
        public static GameObject projectilePrefab;
        public static GameObject badCrosshairPrefab = EntityStates.Mage.Weapon.PrepWall.badCrosshairPrefab;
        private Transform rightAimMuzzleTransform;
        private Transform rightMuzzleTransformSpawn;
        private Transform leftAimMuzzleTransform;
        private Transform leftMuzzleTransformSpawn;
        private GameObject muzzleflashEffect;
        private GameObject fieldAimMuzzleEffect;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;
        private NemLightningColorController lightningController;

        [Header("SFX Variables")]
        private string aimFieldString = StaticValues.fieldAimString;
        private string releaseFieldString = StaticValues.fieldReleaseString;
        private uint stopAimLoop;

        [Header("Functionality Variables")]
        private bool goodPlacement;
        private float duration = 25f;
        public static float maxSlopeAngle = 70f;
        public static float maxDistance = 400f;
        public static float damageCoefficient = Modules.StaticValues.staticFieldTickDamageCoefficient;
        private StackDamageController stackDamageController;
        private bool hasMuzzles;


        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            animator = base.GetModelAnimator();
            childLocator = base.GetModelChildLocator();
            rightAimMuzzleTransform = childLocator.FindChild("HandR");
            leftAimMuzzleTransform = childLocator.FindChild("HandL");
            rightMuzzleTransformSpawn = childLocator.FindChild("HandR");
            leftMuzzleTransformSpawn = childLocator.FindChild("HandL");

            lightningController = base.GetComponent<NemLightningColorController>();

            fieldAimMuzzleEffect = lightningController.fieldAimVFX;
            projectilePrefab = lightningController.fieldPrefab;
            muzzleflashEffect = lightningController.fieldMuzzleVFX;

            fieldIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.staticFieldIndicatorPrefab);
            base.PlayAnimation("FullBody, Override", "AimField", "BaseSkill.playbackRate", 1f);
            //Debug.Log(fieldIndicatorInstance);
            this.UpdateAreaIndicator();
            animator.SetBool("isAiming", true);

            stopAimLoop = Util.PlaySound(aimFieldString, base.gameObject);

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!hasMuzzles)
            {
               rightAimMuzzleTransform = UnityEngine.Object.Instantiate<GameObject>(fieldAimMuzzleEffect, rightAimMuzzleTransform).transform;
               leftAimMuzzleTransform = UnityEngine.Object.Instantiate<GameObject>(fieldAimMuzzleEffect, leftAimMuzzleTransform).transform;
               hasMuzzles = true;
            }
            this.stopwatch += Time.fixedDeltaTime;
            if ((this.stopwatch >= this.duration && base.isAuthority) || (!base.inputBank.skill3.down && base.isAuthority))
            {
                this.outer.SetNextStateToMain();
            }
            UpdateAreaIndicator();

        }

        public override void Update()
        {
            base.Update();
            //this.UpdateAreaIndicator();
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
            animator.SetBool("isAiming", false);
            if (!this.outer.destroying)
            {
                if (this.goodPlacement)
                {
                    
                    base.PlayAnimation("FullBody, Override", "Release Field", "BaseSkill.playbackRate", 1f);
                    animator.SetBool("hasFired", true);
                    float baseDamage = (StaticValues.growthDamageCoefficient * base.GetBuffCount(Buffs.damageGrowth) * damageCoefficient) + damageCoefficient;
                    Util.PlaySound(releaseFieldString, base.gameObject);
                    //Util.PlaySound(PrepWall.fireSoundString, base.gameObject);
                    if (this.fieldIndicatorInstance && base.isAuthority)
                    {
                        //Debug.Log(muzzleflashEffect);
                        //EffectManager.SimpleMuzzleFlash(muzzleflashEffect, base.gameObject, "HandL", true);
                        //EffectManager.SimpleMuzzleFlash(muzzleflashEffect, base.gameObject, "HandR", true);

                        
                        UnityEngine.Object.Instantiate(muzzleflashEffect, rightMuzzleTransformSpawn);

                        UnityEngine.Object.Instantiate(muzzleflashEffect, leftMuzzleTransformSpawn);
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
                    //animator.SetBool("failedToFire", true);
                    base.skillLocator.utility.AddOneStock();
                    base.PlayCrossfade("FullBody, Override", "BufferEmpty", 0.2f);
                }
            }
            EntityState.Destroy(this.fieldIndicatorInstance.gameObject);
            CrosshairUtils.OverrideRequest overrideRequest = this.crosshairOverrideRequest;
            if (overrideRequest != null)
            {
                overrideRequest.Dispose();
            }

            if (rightAimMuzzleTransform)
            {
                EntityState.Destroy(rightAimMuzzleTransform.gameObject);
            }
            if (leftAimMuzzleTransform)
            {
                EntityState.Destroy(leftAimMuzzleTransform.gameObject);
            }
            hasMuzzles = false;


            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();
            AkSoundEngine.StopPlayingID(stopAimLoop, 0);
            base.OnExit();


        }
    }

}
