using BepInEx;
using R2API.Utils;
using RoR2;
using R2API;
using System.Security;
using System.Security.Permissions;
using EntityStates;
using UnityEngine;
using AmpMod.SkillStates;
using RoR2.Orbs;
using System.Collections.Generic;
using AmpMod.SkillStates.BaseStates;
using AmpMod.Modules;
using UnityEngine.Networking;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace AmpMod
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "DamageAPI",
        "OrbAPI",
        "EffectAPI",
    })]

    public class AmpPlugin : BaseUnityPlugin
    {
  
        public const string MODUID = "com.NeonThink.Amp";
        public const string MODNAME = "Amp";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "NT";

        public static AmpPlugin instance;

        private void Awake()
        {
            instance = this;

            // load assets and read config
            Modules.Assets.Initialize();
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // create your survivor here
            new Modules.Survivors.MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;


            Hook();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            Modules.Survivors.MyCharacter.instance.SetItemDisplays();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
        }


        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if (self.body.HasBuff(Modules.Buffs.invulnerableBuff))
            {
                info.rejected = true;
            }

           
            //unused passive that makes body immune to shock damage
            /* if (self.body.baseNameToken == "HENRY_BODY")
            {
               if (info.damageType == DamageType.Shock5s)
                {
                    
                }
            } */

            
            //creates fulmination orb; essentially a copy of lightning orb but with the effect changed to our own; responsible for creating chain damage and effect
            if (info.HasModdedDamageType(Modules.DamageTypes.fulminationChain))
            {
                float damageCoefficient2 = 0.8f;
                float damageValue2 = Util.OnHitProcDamage(info.damage, info.attacker.GetComponent<CharacterBody>().damage, damageCoefficient2);
                FulminationOrb lightningOrb2 = new FulminationOrb();
                lightningOrb2.origin = info.position;
                lightningOrb2.damageValue = damageValue2;
                lightningOrb2.isCrit = info.crit;
                lightningOrb2.bouncesRemaining = 2;
                lightningOrb2.teamIndex = info.attacker.GetComponent<CharacterBody>().teamComponent.teamIndex;
                lightningOrb2.attacker = info.attacker;
                lightningOrb2.bouncedObjects = new List<HealthComponent>
                     {
                        self.GetComponent<HealthComponent>()
                      };
                lightningOrb2.procChainMask = info.procChainMask;
                lightningOrb2.procChainMask.AddProc(ProcType.ChainLightning);
                lightningOrb2.procCoefficient = 0.2f;
                lightningOrb2.damageColorIndex = DamageColorIndex.Item;
                lightningOrb2.range += (float)(2);
                HurtBox hurtBox2 = lightningOrb2.PickNextTarget(info.position);
                if (hurtBox2)
                {
                    lightningOrb2.target = hurtBox2;
                    OrbManager.instance.AddOrb(lightningOrb2);
                }
            }

            
            //apply charge if damageType is applycharge
            if (info.HasModdedDamageType(Modules.DamageTypes.applyCharge))
            {
                applyCharge(self, info);
            }

            //apply charge twice if damageType is apply2charge
            if (info.HasModdedDamageType(Modules.DamageTypes.apply2Charge))
            {
                applyCharge(self, info);
                applyCharge(self, info);
            }

   

            orig(self, info);


        }

        //applies custom debuff/tracker component
        public void applyCharge(HealthComponent self, DamageInfo info)
        {
            if (self.gameObject.GetComponent<Tracker>() == null)
            {
                self.gameObject.AddComponent<Tracker>();
                
                //assigns tracker values for purposes of creating the charge explosion's damage properties if/when the body gains 3 stacks of charge
                self.gameObject.GetComponent<Tracker>().owner = info.attacker.gameObject;
                self.gameObject.GetComponent<Tracker>().ownerBody = info.attacker.GetComponent<CharacterBody>();
                self.gameObject.GetComponent<Tracker>().victim = self.gameObject;
            }

            //apply one stack of chargebuildup
            self.body.AddTimedBuff(Modules.Buffs.chargeBuildup, Modules.StaticValues.chargeDuration, Modules.StaticValues.chargeMaxStacks);

        }



           

        //hook for checking if body has chargedebuff
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
           
            if (self)
            {

                if (NetworkServer.active)
                {
                    //check if body has chargebuildup buff
                    if (self.HasBuff(Buffs.chargeBuildup))
                    {

                        int chargeCount = self.GetBuffCount(Modules.Buffs.chargeBuildup);
                        //if body has more than 3 stacks of charge, make new blastattack with effect
                        if (chargeCount >= 3)
                        {

                            GameObject chargeExplosion;

                            EffectData effectData = new EffectData
                            {
                                origin = self.corePosition,
                            };

                            //set and spawn charge explosion effect
                            chargeExplosion = Assets.chargeExplosionEffect;
                            EffectManager.SpawnEffect(chargeExplosion, effectData, true);

                            var tracker = self.gameObject.GetComponent<Tracker>();

                            if (!tracker)
                            {
                                Debug.Log("no tracker found");
                            }
                            //the networking of this blastattack is very messed up, prolly cuz of tracker vals, so fix that
                            BlastAttack chargeBlast;

                            chargeBlast = new BlastAttack
                            {
                                attacker = tracker.owner,
                                baseDamage = StaticValues.chargeDamageCoefficient * tracker.ownerBody.damage,
                                baseForce = 1f,
                                attackerFiltering = AttackerFiltering.NeverHit,
                                crit = tracker.ownerBody.RollCrit(),
                                damageColorIndex = DamageColorIndex.Item,
                                damageType = DamageType.Generic,
                                falloffModel = BlastAttack.FalloffModel.None,
                                inflictor = tracker.owner,
                                position = self.corePosition,
                                procChainMask = default(ProcChainMask),
                                procCoefficient = 1f,
                                radius = 7f,
                                teamIndex = tracker.ownerBody.teamComponent.teamIndex
                            };

                            chargeBlast.Fire();



                            //removes stacks of charge
                            self.ClearTimedBuffs(Modules.Buffs.chargeBuildup);

                        }



                    }
                }


            }


            orig(self);

        }

    }
}
