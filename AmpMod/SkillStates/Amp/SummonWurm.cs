using AmpMod.Modules;
using EntityStates;
using RoR2;
using R2API;
using UnityEngine;
using RoR2.Skills;
using UnityEngine.Networking;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using AmpMod.SkillStates.Nemesis_Amp;
using RoR2.UI;
using static RoR2.UI.CrosshairController;
using EntityStates.VoidSurvivor.Weapon;
using R2API.Utils;
using AmpMod.SkillStates.SkillComponents;

namespace AmpMod.SkillStates
{
    public class SummonWurm : BaseSkillState
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
        private GameObject muzzleflashEffect;
        private GameObject fieldAimMuzzleEffect;
        private CrosshairUtils.OverrideRequest crosshairOverrideRequest;

        [Header("SFX Variables")]
        private string aimFieldString = StaticValues.fieldAimString;
        private string releaseFieldString = StaticValues.fieldReleaseString;
        private uint stopAimLoop;

        [Header("Functionality Variables")]
        public CharacterBody wormBody;
        private float strikeRadius = 12f;
        public static CharacterMaster wormMaster;
        private bool goodPlacement;
        private float duration = 25f;
        public static float maxSlopeAngle = 70f;
        public static float maxDistance = 200f;
        private StackDamageController stackDamageController;
        private bool hasMuzzles;


        public override void OnEnter()
        {
            base.OnEnter();
            animator = base.GetModelAnimator();
            childLocator = base.GetModelChildLocator();
            fieldIndicatorInstance = UnityEngine.Object.Instantiate<GameObject>(Modules.Assets.staticFieldIndicatorPrefab);

            base.PlayAnimation("FullBody, Override", "AimField", "BaseSkill.playbackRate", 1f);
            //Debug.Log(fieldIndicatorInstance);
            this.UpdateAreaIndicator();
            stopAimLoop = Util.PlaySound(aimFieldString, base.gameObject);

        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();

            this.stopwatch += Time.fixedDeltaTime;
            if ((this.stopwatch >= this.duration && base.isAuthority) || (!base.inputBank.skill4.down && base.isAuthority))
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

        #region update area
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
        #endregion

        private void BlastFire()
        {


            if (base.isAuthority)
            {


                Ray aimRay = base.GetAimRay();




                //create blastattack
                BlastAttack wormStrike = new BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = Modules.StaticValues.wormEatDamageCoefficient * base.characterBody.damage,
                    baseForce = 2f,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,
                    position = this.fieldIndicatorInstance.transform.position,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = this.strikeRadius,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                };
                wormStrike.AddModdedDamageType(DamageTypes.healShield);

                wormStrike.Fire();
            }

        }
        private void SpawnWorm(CharacterBody characterBody)
        {
            GameObject masterPrefab = Modules.Assets.melvinPrefab;

            //Util.PlaySound(summonSoundString, base.gameObject);

            EffectManager.SimpleMuzzleFlash(EntityStates.BrotherMonster.Weapon.FireLunarShards.muzzleFlashEffectPrefab, base.gameObject, "HandL", false);

            //Debug.Log("Spawning worm");


            if (NetworkServer.active)
            {
                var rotation = fieldIndicatorInstance.transform.rotation; 
                if (fieldIndicatorInstance.transform.rotation == new Quaternion(0, 0, 0, 0))
                {
                    rotation = new Quaternion(-1, -2, 1, 0);
                }
                MasterSummon wormSummon = new MasterSummon
                {
                    masterPrefab = masterPrefab,
                    ignoreTeamMemberLimit = false,
                    teamIndexOverride = TeamIndex.Player,
                    summonerBodyObject = characterBody.gameObject,
                    position = this.fieldIndicatorInstance.transform.position,
                    rotation = this.fieldIndicatorInstance.transform.rotation,

                };


                masterPrefab.GetComponent<CharacterMaster>().bodyPrefab.GetComponent<WormHealthTracker>().rotation = rotation;
                wormMaster = wormSummon.Perform();

                
                Debug.Log("field rotation is " + fieldIndicatorInstance.transform.rotation);
            }



            if (wormMaster)
            {
                wormBody = wormMaster.GetBody();
                wormMaster.gameObject.AddComponent<MasterSuicideOnTimer>().lifeTimer = 0f;

            }
        }
        public override void OnExit()
        {

            if (!this.outer.destroying)
            {
                if (this.goodPlacement)
                {
                    if (this.fieldIndicatorInstance && base.isAuthority)
                    {

                        Vector3 forward = this.fieldIndicatorInstance.transform.forward;
                        forward.y = 0f;
                        forward.Normalize();
                        Vector3 vector = Vector3.Cross(Vector3.up, forward);
                        bool crit = Util.CheckRoll(this.critStat, base.characterBody.master);

                        this.BlastFire();
                        this.SpawnWorm(base.characterBody);
                    }
                }
                else
                {
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

            AkSoundEngine.StopPlayingID(stopAimLoop, 0);
            base.OnExit();


        }
    }
}
