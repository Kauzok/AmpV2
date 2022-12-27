using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using System.Collections.Generic;
using RoR2.Projectile;
using R2API;
using AmpMod.Modules;
using AmpMod.SkillStates.BaseStates;
using System.Linq;
using System.Collections.ObjectModel;
using AmpMod.SkillStates.Amp.BaseStates;

namespace AmpMod.SkillStates.Amp
{
    public class VoltaicBombardmentFire : BaseSkillFire
    {
        public GameObject muzzleflashEffectPrefab;
        public float baseDuration;
        public Vector3 boltPosition;
        public Quaternion lightningRotation;
        private float duration = 1f;
        private AmpLightningController lightningController;
        private Vector3 strikePosition;
        public float charge;
        static public float lightningChargeTimer = .5f;
        public GameObject lightningStrikeEffect;
        public GameObject lightningStrikeExplosion;
        private float overchargeDuration = Modules.StaticValues.overChargeDuration;
        bool hasFired;
        public float strikeRadius = 12f;
        protected Animator animator;
        private string AmpName = "NT_AMP_BODY_NAME";


        public override void OnEnter()
        {
            boltPosition = this.aimPosition;
            lightningRotation = this.aimRotation;

            base.OnEnter();
            lightningController = base.GetComponent<AmpLightningController>();

            lightningStrikeEffect = lightningController.bombardmentEffect;

            animator = base.GetModelAnimator();
            hasFired = false;
            this.duration = this.baseDuration / this.attackSpeedStat;
            animator.SetBool("HasFired", true);
            base.PlayAnimation("LeftArm, Override", "SummonLightning", "Spell.playbackRate", .5f);
            strikePosition = this.boltPosition + Vector3.up * 10;

            if (this.muzzleflashEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(this.muzzleflashEffectPrefab, base.gameObject, "HandL", false);
            }
   
            

        }



        /* public void LightningSearch()
         {

             /* BullseyeSearch search = new BullseyeSearch
              {
                  teamMaskFilter = TeamMask.all,
                  filterByLoS = false,
                  searchOrigin = strikePosition,
                  searchDirection = Random.onUnitSphere,
                  sortMode = BullseyeSearch.SortMode.Distance,
                  maxDistanceFilter = 12f,
                  maxAngleFilter = 360f
              };
              Debug.Log("searching");

              search.RefreshCandidates();

              HurtBox target = search.GetResults().FirstOrDefault<HurtBox>();
              if (target)
              {
                  if (target.healthComponent && target.healthComponent.body)
                  {
                      if (target.healthComponent.body.baseNameToken == AmpName)
                      {
                          base.characterBody.AddTimedBuff(Modules.Buffs.overCharge, overchargeDuration);

                      }
                  }
              } 
             ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(base.characterBody.teamComponent.teamIndex);
             float maxDistance = 144f;
             Vector3 position = strikePosition;
             for (int i = 0; i < teamMembers.Count; i++)
             {
                 if ((teamMembers[i].transform.position - position).sqrMagnitude <= maxDistance)
                 {
                     CharacterBody body = teamMembers[i].GetComponent<CharacterBody>();
                     if (body)
                     {
                         Debug.Log("Body Found");
                         AmpPlugin.logger.LogMessage("dababy found");
                     }
                     if (NetworkServer.active)
                     {
                         AmpPlugin.logger.LogMessage("networkserver active");
                         //base.characterBody.AddTimedBuff(Modules.Buffs.overCharge, overchargeDuration);
                     }
                     if (body && NetworkServer.active)
                     {
                         Debug.Log("Adding Overcharge");
                         body.AddTimedBuff(Modules.Buffs.overCharge, overchargeDuration);
                         AmpPlugin.logger.LogMessage("adding overcharge");
                         //body.AddBuff(Modules.Buffs.overCharge);

                         if (body.HasBuff(Modules.Buffs.overCharge))
                         {
                             Debug.Log("buff applied");
                         }
                     }


                 }
             }
         }*/


        public CharacterBody[] lightningSearch()
        {
            List<CharacterBody> buffList = new List<CharacterBody>();

            ReadOnlyCollection<TeamComponent> teamMembers = TeamComponent.GetTeamMembers(base.characterBody.teamComponent.teamIndex);
            float maxDistance = 144f;
            Vector3 position = strikePosition;
            for (int i = 0; i < teamMembers.Count; i++)
            {
                if ((teamMembers[i].transform.position - position).sqrMagnitude <= maxDistance)
                {
                    CharacterBody body = teamMembers[i].GetComponent<CharacterBody>();
                    buffList.Add(body);
                }
            }
            CharacterBody[] bodyList = buffList.ToArray();
            return bodyList;

        }

        //used to set delay for attack
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (!hasFired)
            {
                //if age of fixedupdate is > .5 seconds 
                if (base.fixedAge > lightningChargeTimer)
                {
                    hasFired = true;
                    Fire();

                   if (NetworkServer.active)
                    {
                        //base.characterBody.AddTimedBuff(Modules.Buffs.overCharge, overchargeDuration);
                    }

                    if (NetworkServer.active)
                    {
                        //Debug.Log("networkserver active");
                       // Debug.Log(strikePosition);
                        CharacterBody[] result = lightningSearch();
                        foreach (CharacterBody i in result)
                        {
                            //Debug.Log("adding buff");
                            i.AddTimedBuff(Modules.Buffs.overCharge, overchargeDuration);
                            
                        }
                    } 
                    
                   
                   

                }

                //i dont know why but this line is necessary for the .5 second delay to actually work
                duration = .5f;
            }

                if (base.isAuthority && base.fixedAge >= this.duration)
                {
                    this.outer.SetNextStateToMain();
                } 
            }

        

        public override void OnExit()
        {
            base.OnExit();
            animator.SetBool("HasFired", false);
        }

        private void Fire()
        {
            

            if (base.isAuthority)
            {
                

                Ray aimRay = base.GetAimRay();

                //lightningStrikeEffect = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/ImpactEffects/LightningStrikeImpact");
                lightningStrikeEffect = lightningController.bombardmentEffect;

                //effect data for lightning)
                EffectData lightning = new EffectData
                {
                    origin = this.boltPosition,
                    scale = 5f,
                    
                };

                //spawns lightning/lightningexplosion effects
                EffectManager.SpawnEffect(lightningStrikeEffect, lightning, true);


                //create blastattack
                BlastAttack lightningStrike = new BlastAttack
                {
                    attacker = base.gameObject,
                    baseDamage = Modules.StaticValues.lightningStrikeCoefficient * base.characterBody.damage,
                    baseForce = 2f,
                    attackerFiltering = AttackerFiltering.NeverHitSelf,
                    crit = base.characterBody.RollCrit(),
                    damageColorIndex = DamageColorIndex.Item,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = base.gameObject,

                    //blastattack is positioned 10 units above where the reticle is placed
                    position = strikePosition,
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    radius = this.strikeRadius,
                    teamIndex = base.characterBody.teamComponent.teamIndex
                };
                lightningStrike.AddModdedDamageType(Modules.DamageTypes.apply2Charge);
                
                
                BlastAttack.Result result = lightningStrike.Fire();

                //foreach (hitC)

            }
            
        }


        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.boltPosition);
            writer.Write(this.strikePosition);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.boltPosition = reader.ReadVector3();
            this.strikePosition = reader.ReadVector3();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }

}


        




