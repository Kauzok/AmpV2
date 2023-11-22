using System;
using RoR2;
using EntityStates;
using AmpMod.SkillStates.Nemesis_Amp.Components;
using UnityEngine;
using RoR2.Projectile;
using AmpMod.Modules;
using System.Collections;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class FluxBlades : BaseSkillState
    {
        private float damageCoefficient = Modules.StaticValues.bladeDamageCoefficient;
        public GameObject bladePrefab;
        public static GameObject bladeMuzzleObject;
        public GameObject fireEffect;
        public string spawnSoundString = StaticValues.fluxBladesSpawnString;
        private Animator animator;
        public static float baseDuration = 1.5f;
        private float baseChargeTime = .6f;
        private float chargeTime;
        private float recoverDuration;
        private NemLightningColorController lightningController;
        private float duration;
        private float surgeBuffCount;
        private float launchForce = 90f;
        private float baseGrowDuration = .5f;
        private float growDuration;
        private ChildLocator childLocator;
        private int numOfBullets;
        private GameObject[] bullets;
        private bool hasFired;
        private float distanceFromHead = 0.8f;
        private int numBulletSpawn;
        private ProjectileImpactExplosion projectileImpactExplosion;
        private String soundString = StaticValues.fluxBladesFireString;
        private float totalDuration;
        private float fireDuration;
        private StackDamageController stackDamageController;
        private float chargeDuration;
        private const float FIRE_TIME_PERCENTAGE = .1f;
        private const float RECOVER_TIME_PERCENTAGE = .25f;

        public override void OnEnter()
        {
            base.OnEnter();

            stackDamageController = base.GetComponent<StackDamageController>();

            surgeBuffCount = base.GetBuffCount(Buffs.damageGrowth);

            lightningController = base.GetComponent<NemLightningColorController>();

            bladePrefab = lightningController.bladePrefab;
            bladeMuzzleObject = lightningController.bladePrepVFX;
            fireEffect = lightningController.bladeFireVFX;

            projectileImpactExplosion = bladePrefab.GetComponent<ProjectileImpactExplosion>();

            if (surgeBuffCount == 10)
            {
                numOfBullets = 6;
            }
            else
            {
                numOfBullets = 3;
            }

            numBulletSpawn = numOfBullets - 1;
            
            bullets = new GameObject[numOfBullets];

           /* if (base.GetBuffCount(Buffs.damageGrowth) == StaticValues.growthBuffMaxStacks)
            {
                projectileImpactExplosion.bonusBlastForce = new Vector3(-4000f, -4000f, -4000f);
                projectileImpactExplosion.blastDamageCoefficient = .333f;
                projectileImpactExplosion.SetExplosionRadius(12f);
                projectileImpactExplosion.totalDamageMultiplier = 1f;
                //projectileImpactExplosion.explosionEffect = Assets.BladeImplosionEffect;
            } */

            //this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            animator = base.GetModelAnimator();
            this.totalDuration = baseDuration / this.attackSpeedStat;
            fireDuration = FIRE_TIME_PERCENTAGE * totalDuration;
            recoverDuration = RECOVER_TIME_PERCENTAGE * totalDuration;
            chargeDuration = totalDuration - fireDuration - recoverDuration;
            growDuration = .5f * chargeDuration;
            // base.PlayAnimation("Gesture, Override", "LaunchVortex", "BaseSkill.playbackRate", duration);
            animator.SetBool("isUsingIndependentSkill", true);

            Util.PlaySound(spawnSoundString, base.gameObject);
            

            Transform modelTransform = base.GetModelTransform();

            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                //this.leftMuzzleTransform = this.childLocator.FindChild("HandL");

            }

        }

        private void updateRotAndLoc()
        {
            // Get player aimray for settin the direction the bullets point
            Ray aimRay = base.GetAimRay();

            // Calculate the angle between each bullet
            float angle = (Mathf.PI / (numOfBullets - 1));

            for (int i = 0; i < numOfBullets; i++)
            {
                if (bullets[i] != null)
                {
                    bullets[i].transform.position = aimRay.origin + getLoc(modelLocator.modelTransform.eulerAngles.y * (Mathf.PI / 180), angle * i);

                    // I could want the model locator not 
                    bullets[i].transform.rotation = Quaternion.LookRotation(aimRay.direction);
                }

            }
        }

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


        IEnumerator Fire()
        {

            Ray aimRay;
            this.hasFired = true;


            if (base.isAuthority)
            {
                // Calculate the angle between each bullet
                float angle = (Mathf.PI / (numOfBullets - 1));
                int i = 0;
                for (i = 0; i < numOfBullets; i++)
                {
                    // Get current aim ray of the character
                    aimRay = base.GetAimRay();

                    //Play sound, and get ID of last sound to cancel
                    //stopPrepID = Util.PlaySound(prepString, base.gameObject);


                    // Spawn the prefab of the bolt

                    bullets[i] = UnityEngine.Object.Instantiate<GameObject>(
                        bladeMuzzleObject,
                        aimRay.origin + getLoc(modelLocator.modelTransform.eulerAngles.y * (Mathf.PI / 180), angle * i),
                        Quaternion.LookRotation(aimRay.direction));

                    //NetworkServer.Spawn(bullets[i]);
                    // Set the bullet as a child of the character body
                    bullets[i].transform.parent = base.characterBody.transform;

                    ScaleParticleSystemDuration component = bullets[i].GetComponent<ScaleParticleSystemDuration>();
                    ObjectScaleCurve component2 = bullets[i].GetComponent<ObjectScaleCurve>();
                    if (component)
                    {
                        component.newDuration = this.growDuration;
                    }
                    if (component2)
                    {
                        component2.timeMax = this.growDuration;
                    }

                    //time between projectile spawns and the first launch
                    if (i != numBulletSpawn)
                    {
                        yield return new WaitForSeconds(0f);
                    }
                    else
                    {
                        yield return new WaitForSeconds(chargeDuration);
                    }
                }

                aimRay = base.GetAimRay();

                // Calculate a raycast intersection for where the player is looking
                RaycastHit hit;
                Vector3 direction;
                bool somethingHit = Physics.Raycast(aimRay.origin, aimRay.direction, out hit);


                // Loop through all of the bullets and delete them spawning a projectile in
                // there place facing the interction point or if no intersection the players direction
                for (int k = 0; k < numOfBullets; k++)
                {
                    // Weird break where the bullets would lock on too close so in order to avoid that
                    // I used a arbitrary value of 2.5 so anything too close wont fire wrong
                    if (somethingHit && hit.distance > 2.5f)
                    {
                        // Calculate the directional vector for the bullet to hit the collision point and normalized to a magnitude of 1
                        direction = (hit.point - bullets[k].transform.position).normalized;
                    }
                    else
                    {
                        direction = aimRay.direction;
                    }




                    // Spawn the projectile at the gameobjects current location
                    ProjectileManager.instance.FireProjectile(bladePrefab,
                    bullets[k].transform.position,
                    Quaternion.LookRotation(direction),
                    base.gameObject,
                    damageCoefficient * this.damageStat,
                    launchForce,
                    base.RollCrit(),
                    DamageColorIndex.Default,
                    null,
                    launchForce);
                    //play sound for each fire event
                    ///Util.PlaySound(launchString, base.gameObject);
                    // Destroy bullet after spawning projectile version
                    Destroy(bullets[k]);
                    //yield return new WaitForSeconds(fireDuration / numOfBullets);
                    //time between projectile launches
                    //EffectManager.SimpleMuzzleFlash(fireEffect, bullets[k].gameObject, null, true);
                    EffectManager.SpawnEffect(fireEffect, new EffectData
                    {
                        scale = 1f,
                        origin = bullets[k].gameObject.transform.position,
                        rotation = bullets[k].gameObject.transform.rotation,
                    }, true) ;
                    Util.PlayAttackSpeedSound(soundString, base.gameObject, this.attackSpeedStat);
                    yield return new WaitForSeconds(fireDuration / numOfBullets);
                }
                //cancel prep sfx
               // AkSoundEngine.StopPlayingID(stopPrepID);
                //play ferroshot launch effect from soundbank
                //Util.PlaySound(launchString, base.gameObject);



            }

        }
        /*     public override InterruptPriority GetMinimumInterruptPriority()
             {
                 return InterruptPriority.PrioritySkill;
             }*/

        public override InterruptPriority GetMinimumInterruptPriority()
        {

            //if (base.fixedAge >= this.chargeDuration) return InterruptPriority.Any;

            return InterruptPriority.Skill;
        }



        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge < this.totalDuration)
            {
                if (!hasFired)
                {
                    base.characterBody.StartCoroutine(Fire());
                    stackDamageController.newSkillUsed = this;
                    stackDamageController.resetComboTimer();
                }

                updateRotAndLoc();

            }
            else if (base.fixedAge >= this.totalDuration && base.isAuthority)
            {

                this.outer.SetNextStateToMain();
                return;
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }
}
