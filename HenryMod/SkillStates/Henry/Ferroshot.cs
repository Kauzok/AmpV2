using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using System.Collections;
using UnityEngine.Networking;
using BepInEx;

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
        public static int numOfBullets = 6;

        private Animator animator;

        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject ferroshotPrefabBasic = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("Spike");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private GameObject[] bullets = new GameObject[numOfBullets];

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            Ray aimRay = base.GetAimRay();

            //GameObject bullet = UnityEngine.Object.Instantiate<GameObject>(
            //ferroshotPrefabBasic,
            //aimRay.origin + new Vector3(0, 0, 10),
            //Quaternion.LookRotation(aimRay.direction));

            //ferroshotPrefabBasic.AddComponent<CustomController>();
          


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
                GameObject bullet;
                if (base.isAuthority)
                {
                    
                    for(int i = 0; i<6; i++)
                    {
                        // Get current aim ray of the character
                        Ray aimRay = base.GetAimRay();

                        MonoBehaviour.print("Spawinign");

                        // Spawn the prefab of the bolt under the character as a parent 
                        bullets[i] = UnityEngine.Object.Instantiate<GameObject>(
                            ferroshotPrefabBasic, 
                            aimRay.origin + new Vector3(0,4,0), 
                            Quaternion.LookRotation(aimRay.direction));

                        //NetworkServer.Spawn(bullets[i]);

                        // Spawn the projectile
                        /*ProjectileManager.instance.FireProjectile(Modules.Projectiles.ferroshotPrefab,
                        aimRay.origin,
                        Util.QuaternionSafeLookRotation(aimRay.direction),
                        base.gameObject,
                        damageCoefficient * this.damageStat,
                        launchForce,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        launchForce);*/

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
                base.characterBody.StartCoroutine(Fire());
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

public class CustomController : NetworkBehaviour
{
    public float waitDuration;
    public float speed;

    //new private Rigidbody rigidbody;
    private float age = 0.0f;
    private bool moved = false;

    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        //rigidbody.velocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        age += Time.fixedDeltaTime;
        if (!moved && age > waitDuration)
        {
            //rigidbody.velocity = transform.forward * speed;
            moved = true;
        }
    }
}