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
        public static float launchForce = 150f;
        private static int numOfBullets = 6;

        private Animator animator;

        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject ferroshotPrefabBasic = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("Spike");

        private float duration;
        //private float fireTime;
        private bool hasFired;
        private string muzzleString;
        private float distanceFromHead = 0.5f;
        private GameObject[] bullets = new GameObject[numOfBullets];

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / this.attackSpeedStat;
            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            Ray aimRay = base.GetAimRay();

            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);// 3f * this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        /// <summary>
        /// Get the offset from the players head for the bullets to spawn
        /// </summary>
        /// <param name="yDir"> Radian angle for rotating the point around </param>
        /// <param name="angle"> Angle direction for placing the bullet </param>
        /// <returns> Vector 3 for offset from aimray </returns>
        private Vector3 getLoc(float yDir, float angle)
        {
            // Calculate x = Cos(yDir) * X
            float x = Mathf.Cos(yDir) * Mathf.Cos(angle);
            // Calcualte y = Y
            float y = Mathf.Sin(angle);
            // Calculate z = -Sin(yDir) * X
            float z = -Mathf.Sin(yDir) * Mathf.Cos(angle);

            return new Vector3(x, y, z) * distanceFromHead;
        }

        /// <summary>
        /// Updates the pla
        /// </summary>
        private void updateRot()
        {
            // Get player aimray for settin the direction the bullets point
            Ray aimRay = base.GetAimRay();

            // Calculate the angle between each bullet
            float angle = (Mathf.PI / (numOfBullets - 1));

            for (int i = 0; i < numOfBullets; i ++)
            {
                if (bullets[i] != null)
                {
                    bullets[i].transform.position = aimRay.origin + getLoc(modelLocator.modelTransform.eulerAngles.y * (Mathf.PI / 180), angle * i);

                    // I could want the model locator not 
                    bullets[i].transform.rotation = Quaternion.LookRotation(aimRay.direction);
                }
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator Fire()
        {
            if (!this.hasFired)
            {
                Ray aimRay;
                this.hasFired = true;
                Util.PlaySound("HenryBombThrow", base.gameObject);
                if (base.isAuthority)
                {
                    // Calculate the angle between each bullet
                    float angle = (Mathf.PI / (numOfBullets - 1));

                    for (int i = 0; i < numOfBullets; i++)
                    {
                        // Get current aim ray of the character
                        aimRay = base.GetAimRay();

                        // Spawn the prefab of the bolt
                        bullets[i] = UnityEngine.Object.Instantiate<GameObject>(
                            ferroshotPrefabBasic, 
                            aimRay.origin + getLoc(modelLocator.modelTransform.eulerAngles.y * (Mathf.PI / 180), angle * i), 
                            Quaternion.LookRotation(aimRay.direction));

                        // Set the bullet as a child of the character body
                        bullets[i].transform.parent = base.characterBody.transform;

                        yield return new WaitForSeconds(this.duration / numOfBullets);
                    }

                    aimRay = base.GetAimRay();

                    // Calculate a raycast intersection for where the player is looking
                    RaycastHit hit;
                    Vector3 direction;
                    bool somethingHit = Physics.Raycast(aimRay.origin, aimRay.direction, out hit);


                    // Loop through all of the bullets and delete them spawning a projectile in
                    // there place facing the interction point or if no intersection the players direction
                    for (int i = 0; i < numOfBullets; i++)
                    {
                        if (somethingHit)
                        {
                            // Calculate the directional vector for the bullet to hit the collision point and normalized to a magnitude of 1
                            direction = (hit.point - bullets[i].transform.position).normalized;
                        }
                        else
                        {
                            direction= aimRay.direction;
                        }

                        // Spawn the projectile at the gameobjects current location
                        ProjectileManager.instance.FireProjectile(Modules.Projectiles.ferroshotPrefab,
                        bullets[i].transform.position,
                        Quaternion.LookRotation(direction),
                        base.gameObject,
                        damageCoefficient * this.damageStat,
                        launchForce,
                        base.RollCrit(),
                        DamageColorIndex.Default,
                        null,
                        launchForce);

                        // Destroy bullet after spawning projectile version
                        Destroy(bullets[i]);
                    }
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge < this.duration)
            {
                base.characterBody.StartCoroutine(Fire());
                updateRot();
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