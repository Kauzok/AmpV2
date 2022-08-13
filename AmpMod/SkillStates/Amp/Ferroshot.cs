using EntityStates;
using RoR2;
using UnityEngine;
using RoR2.Projectile;
using System.Collections;
using UnityEngine.Networking;
using BepInEx;

namespace AmpMod.SkillStates
{
    public class Ferroshot : BaseSkillState
    {
        //projectile vars
        public static float damageCoefficient = Modules.StaticValues.ferroshotDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        public static float force = 10;
        public static float recoil = 0f;
        public static float range = 600f;
        public static float launchForce = 150f;
        private static int numOfBullets = 6;
        private const float FIRE_TIME_PERCENTAGE = .25f;

        //soundbank items
        private string launchString = Modules.StaticValues.ferroshotLaunchAlterString;
        private string prepString = Modules.StaticValues.ferroshotPrepAlterString;
        private uint stopPrepID;

        private Animator animator;
        private ChildLocator childLocator;
        private Transform leftMuzzleTransform;

        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject ferroshotPrefabBasic = Modules.Assets.bulletPrepItem;

        private float totalDuration;
        private float fireDuration;
        private float chargeDuration;
        //private float fireTime;
        private bool hasFired;
        private float distanceFromHead = 0.5f;
        private GameObject[] bullets = new GameObject[numOfBullets];
        private bool hasMuzzleEffect;
        private GameObject muzzleEffectPrefab = Modules.Assets.ampBulletMuzzleEffect;
        private Transform fireMuzzleTransform;

        public override void OnEnter()
        {
            base.OnEnter();
            this.totalDuration = baseDuration / this.attackSpeedStat;
            fireDuration = FIRE_TIME_PERCENTAGE * totalDuration;
            chargeDuration = totalDuration - fireDuration;
            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            

            Ray aimRay = base.GetAimRay();
            base.PlayAnimation("Worm, Override", "ShootProjectile", "LorentzCannon.playbackRate", 1.2f*totalDuration);//1.7f*this.totalDuration);// 1.8f * this.duration);
            //Debug.Log("total duration = " + totalDuration);
            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.leftMuzzleTransform = this.childLocator.FindChild("LowerArmL");
               
            }



        }

        public override void OnExit()
        {
            if (fireMuzzleTransform)
            {
                EntityState.Destroy(fireMuzzleTransform.gameObject);
            }
           
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
        /// Updates the rotation and location of the bullet around the player
        /// </summary>
        private void updateRotAndLoc()
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
        /// Method for firing that spawns n bullets around the players over the base duration and then fires all of them at once
        /// </summary>
        IEnumerator Fire()
        {
            
            Ray aimRay;
            this.hasFired = true;
                

            if (base.isAuthority)
            {
                // Calculate the angle between each bullet
                float angle = (Mathf.PI / (numOfBullets - 1));

                for (int i = 0; i < numOfBullets; i++)
                {
                    // Get current aim ray of the character
                    aimRay = base.GetAimRay();

                    //Play sound, and get ID of last sound to cancel
                    stopPrepID = Util.PlaySound(prepString, base.gameObject);
                        
                        
                    // Spawn the prefab of the bolt
                    
                    bullets[i] = UnityEngine.Object.Instantiate<GameObject>(
                        ferroshotPrefabBasic, 
                        aimRay.origin + getLoc(modelLocator.modelTransform.eulerAngles.y * (Mathf.PI / 180), angle * i), 
                        Quaternion.LookRotation(aimRay.direction));

                    //NetworkServer.Spawn(bullets[i]);
                    // Set the bullet as a child of the character body
                    bullets[i].transform.parent = base.characterBody.transform;

                    yield return new WaitForSeconds((this.totalDuration - fireDuration) / numOfBullets);
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
                    // Weird break where the bullets would lock on too close so in order to avoid that
                    // I used a arbitrary value of 2.5 so anything too close wont fire wrong
                    if (somethingHit && hit.distance > 2.5f)
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
                    //play sound for each fire event
                    Util.PlaySound(launchString, base.gameObject);
                    // Destroy bullet after spawning projectile version
                    Destroy(bullets[i]);
                    yield return new WaitForSeconds(fireDuration / numOfBullets);
                }
                //cancel prep sfx
                AkSoundEngine.StopPlayingID(stopPrepID);
                //play ferroshot launch effect from soundbank
                //Util.PlaySound(launchString, base.gameObject);
                  
                    
                    
            }
            
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!hasMuzzleEffect)
            {
                hasMuzzleEffect = true;
                if (this.childLocator)
                {
                    //Transform transform = childLocator.FindChild("LowerArmL");
                    {
                     //  fireMuzzleTransform = Object.Instantiate<GameObject>(muzzleEffectPrefab, leftMuzzleTransform).transform;
                     //  Debug.Log("Spawning Muzzle Effect");

                    }

                }
            }

            if (base.fixedAge < this.totalDuration)
            {
                if (!hasFired)
                {
                    base.characterBody.StartCoroutine(Fire());
                }
                
                updateRotAndLoc();

            } else if (base.fixedAge >= this.totalDuration && base.isAuthority)
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