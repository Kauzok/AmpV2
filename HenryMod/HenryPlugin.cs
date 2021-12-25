using BepInEx;
using R2API.Utils;
using RoR2;
using R2API;
using System.Security;
using System.Security.Permissions;
using HenryMod;
using EntityStates;
using UnityEngine;
using HenryMod.SkillStates;
using RoR2.Orbs;
using System.Collections.Generic;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace HenryMod
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
    })]

    public class HenryPlugin : BaseUnityPlugin
    {
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.NeonThink.Battlemage";
        public const string MODNAME = "Battlemage";
        public const string MODVERSION = "1.0.0";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string developerPrefix = "NT";

        public static HenryPlugin instance;

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

            
            if (info.HasModdedDamageType(Modules.DamageTypes.fulminationChain))
            {
                float damageCoefficient2 = 0.8f;
                float damageValue2 = Util.OnHitProcDamage(info.damage, info.attacker.GetComponent<CharacterBody>().damage, damageCoefficient2);
                LightningOrb lightningOrb2 = new LightningOrb();
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
                lightningOrb2.lightningType = LightningOrb.LightningType.Ukulele;
                lightningOrb2.damageColorIndex = DamageColorIndex.Item;
                lightningOrb2.range += (float)(2);
                HurtBox hurtBox2 = lightningOrb2.PickNextTarget(info.position);
                if (hurtBox2)
                {
                    lightningOrb2.target = hurtBox2;
                    OrbManager.instance.AddOrb(lightningOrb2);
                }
            }

            if (info.HasModdedDamageType(Modules.DamageTypes.applyCharge))
            {
                if (self.GetComponent<CharacterBody>().GetBuffCount(Modules.Buffs.chargeBuildup) < 3)
                {
                    self.gameObject.GetComponent<CharacterBody>().AddBuff(Modules.Buffs.chargeBuildup);
                }
            }


            if (info.HasModdedDamageType(Modules.DamageTypes.apply2Charge))
            {
                if (self.GetComponent<CharacterBody>().GetBuffCount(Modules.Buffs.chargeBuildup) < 3)
                {
                    self.gameObject.GetComponent<CharacterBody>().AddBuff(Modules.Buffs.chargeBuildup);
                }

                else if (self.GetComponent<CharacterBody>().GetBuffCount(Modules.Buffs.chargeBuildup) < 2)
                {
                    self.gameObject.GetComponent<CharacterBody>().AddBuff(Modules.Buffs.chargeBuildup);
                    self.gameObject.GetComponent<CharacterBody>().AddBuff(Modules.Buffs.chargeBuildup);
                }
       

            }


            /* if (R2API.DamageAPI.HasModdedDamageType(info))
            {
                self.body.AddTimedBuff(Modules.Buffs.chargeBuildup, Modules.StaticValues.chargeDuration, Modules.StaticValues.chargeMaxStacks);
            } */



            orig(self, info);
            
        }

        //hook for checking if body has debuff
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
           
            if (self)
            {
                /*
                if (self.HasBuff(Modules.Buffs.chargeDebuff))
                {
                   new BlastAttack
                    {
                        attacker = self.gameObject.GetComponent<Tracker>().owner,
                        baseDamage = Modules.StaticValues.chargeDamageCoefficient * self.gameObject.GetComponent<Tracker>().ownerBody.damage,
                        baseForce = 1f,
                        attackerFiltering = AttackerFiltering.NeverHit,
                        crit = self.gameObject.GetComponent<Tracker>().ownerBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.Generic,
                        falloffModel = BlastAttack.FalloffModel.None,
                        inflictor = self.gameObject.GetComponent<Tracker>().owner,
                        position = self.corePosition,
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 1f,
                        radius = 10f,
                        teamIndex = self.gameObject.GetComponent<Tracker>().ownerBody.teamComponent.teamIndex
                    }.Fire();
               

                   //Destroy(self.gameObject.GetComponent<Tracker>());
                    self.RemoveBuff(Modules.Buffs.chargeDebuff);
                    
                } */


                if (self.HasBuff(Modules.Buffs.chargeBuildup))
                {
                    int chargeCount = self.GetBuffCount(Modules.Buffs.chargeBuildup);

                    if (chargeCount >= 3)
                    {
                        GameObject chargeExplosion;

                        EffectData effectData = new EffectData
                        {
                            origin = self.corePosition,
                            scale = 10f
                        };
                          chargeExplosion = Modules.Assets.electricExplosionEffect;
                          EffectManager.SpawnEffect(chargeExplosion, effectData, true);


                            new BlastAttack
                        {
                            attacker = self.gameObject.GetComponent<Tracker>().owner,
                            baseDamage = Modules.StaticValues.chargeDamageCoefficient * self.gameObject.GetComponent<Tracker>().ownerBody.damage,
                            baseForce = 1f,
                            attackerFiltering = AttackerFiltering.NeverHit,
                            crit = self.gameObject.GetComponent<Tracker>().ownerBody.RollCrit(),
                            damageColorIndex = DamageColorIndex.Item,
                            damageType = DamageType.Generic,
                            falloffModel = BlastAttack.FalloffModel.None,
                            inflictor = self.gameObject.GetComponent<Tracker>().owner,
                            position = self.corePosition,
                            procChainMask = default(ProcChainMask),
                            procCoefficient = 1f,
                            radius = 12f,
                            teamIndex = self.gameObject.GetComponent<Tracker>().ownerBody.teamComponent.teamIndex
                        }.Fire();
                        

                      /*  new BlastAttack
                        {
                            attacker = self.gameObject.GetComponent<Tracker>().owner,
                            baseDamage = 1.5f * self.gameObject.GetComponent<Tracker>().ownerBody.damage,
                            baseForce = 1f,
                            attackerFiltering = AttackerFiltering.NeverHit,
                            crit = self.gameObject.GetComponent<Tracker>().ownerBody.RollCrit(),
                            damageColorIndex = DamageColorIndex.Item,
                            damageType = DamageType.Generic,
                            falloffModel = BlastAttack.FalloffModel.None,
                            inflictor = self.gameObject.GetComponent<Tracker>().owner,
                            position = self.corePosition,
                            procChainMask = default(ProcChainMask),
                            procCoefficient = 1f,
                            radius = 10f,
                            teamIndex = self.gameObject.GetComponent<Tracker>().ownerBody.teamComponent.teamIndex
                        }.Fire(); */

                        //self.AddBuff(Modules.Buffs.chargeDebuff);
                        self.ClearTimedBuffs(Modules.Buffs.chargeBuildup);
                      
                        
                    }
                  /*  else if(chargeCount > 3)
                    {
                        for (int i = self.GetBuffCount(Modules.Buffs.chargeBuildup); i > 3; i--)
                        {
                            self.RemoveBuff(Modules.Buffs.chargeBuildup);
                        }
                    } */


                }


            }


            orig(self);

        }

    }
}
