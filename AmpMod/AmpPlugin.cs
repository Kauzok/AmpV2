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
using HG.Reflection;
using System.Collections.ObjectModel;
using UnityEngine.AddressableAssets;

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

        //dictionary for swapping out stubbed shaders for in game shaders
        public static Dictionary<string, string> ShaderLookup = new Dictionary<string, string>()
        {
            {"stubbedshader/deferred/standard", "shaders/deferred/hgstandard"},
            {"fake ror/hopoo games/fx/hgcloud intersection remap", "shaders/fx/hgintersectioncloudremap" },
            {"fake ror/hopoo games/fx/hgcloud remap", "shaders/fx/hgcloudremap" }
        };


        
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
           // Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

         /*   //material shader autoconversion
            var materialAssets = Assets.mainAssetBundle.LoadAllAssets<Material>();
           
            foreach (Material material in materialAssets)
            {
                if (!material.shader.name.StartsWith("Stubbed")) { continue; }

                //var replacementShader = Addressables.LoadAssetAsync<Shader>(material.shader.name.ToLower()).WaitForCompletion();
                var replacementShader = LegacyResourcesAPI.Load<Shader>(ShaderLookup[material.shader.name.ToLower()]); //LegacyResourcesAPI.Load<Shader>(ShaderLookup[material.shader.name.ToLower()]);
                if (replacementShader) { material.shader = replacementShader; Debug.Log("Replacing Shader"); }
                if (!replacementShader) { Debug.Log("No Shader Found"); }
                
            } */

            // create your survivor here
            new Modules.Survivors.Amp().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            //RoR2.ContentManagement.ContentManager.onContentPacksAssigned += LateSetup;
            RoR2Application.onLoad += SetItemDisplays;



            Hook();
        }

        private void LateSetup(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            // have to set item displays later now because they require direct object references..
            Modules.Survivors.Amp.instance.SetItemDisplays();
            RoR2Application.onLoad += SetItemDisplays;
        }

        private static void SetItemDisplays()
        {
            ItemDisplays.PopulateDisplays();
            Modules.Survivors.Amp.instance.SetItemDisplays();
            
        }

        private void Hook()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
            On.RoR2.CharacterSpeech.BrotherSpeechDriver.DoInitialSightResponse += BrotherSpeechDriver_DoInitialSightResponse;
            On.RoR2.CharacterSpeech.BrotherSpeechDriver.OnBodyKill += BrotherSpeechDriver_OnBodyKill;

        }

        //Mithrix quotes for when Amp is present
        private void BrotherSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_DoInitialSightResponse orig, RoR2.CharacterSpeech.BrotherSpeechDriver self)
        {
            bool isAmpThere = false;

            ReadOnlyCollection<CharacterBody> characterBodies = CharacterBody.readOnlyInstancesList;
            for (int i = 0; i < characterBodies.Count; i++)
            {
                //BodyIndex AmpIndex = characterBodies[i].bodyIndex;
               // isAmpThere |= (AmpIndex == BodyCatalog.FindBodyIndex(Modules.Survivors.MyCharacter.instance.bodyName));

                string ampName = characterBodies[i].baseNameToken;
                isAmpThere |= (ampName == developerPrefix + "_AMP_BODY_NAME");
            }

            if (isAmpThere)
            {
                RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[] responsePool = new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[]
                {
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "MITHRIX_SEE_AMP_1"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "MITHRIX_SEE_AMP_2"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "MITHRIX_SEE_AMP_3"
                    },
                      new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = ""
                    }

                };

                self.SendReponseFromPool(responsePool);
            }

            orig(self);
           
        }

        //Same as above but for mithrix kill quotes, consider changing to bodyIndex check method used in brothersightresponse
        private void BrotherSpeechDriver_OnBodyKill(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_OnBodyKill orig, RoR2.CharacterSpeech.BrotherSpeechDriver self, DamageReport damageReport)
        {
            if (damageReport.victimBody)
            {
                if (damageReport.victimBody.baseNameToken == AmpPlugin.developerPrefix + "_AMP_BODY_NAME")
                {
                    RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[] responsePool = new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo[]
                    {
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "MITHRIX_KILL_AMP_1"
                    },
                    new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = "MITHRIX_KILL_AMP_2"
                    },
                         new RoR2.CharacterSpeech.CharacterSpeechController.SpeechInfo
                    {
                        duration = 1f,
                        maxWait = 4f,
                        mustPlay = true,
                        priority = 0f,
                        token = ""
                    }

                    };

                    self.SendReponseFromPool(responsePool);
                }
            }

            orig(self, damageReport);
        }

        private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
           
           
            //unused passive that makes body immune to shock damage
            /* if (self.body.baseNameToken == "HENRY_BODY")
            {
               if (info.damageType == DamageType.Shock5s)
                {
                    
                }
            } */

            
            //creates fulmination orb; essentially a copy of lightning orb but with the effect changed to our own; responsible for creating chain damage and effect
            if (info.HasModdedDamageType(DamageTypes.fulminationChain))
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
                lightningOrb2.damageColorIndex = DamageColorIndex.Default;
                lightningOrb2.range += (float)(2);
                HurtBox hurtBox2 = lightningOrb2.PickNextTarget(info.position);
                if (hurtBox2)
                {
                    lightningOrb2.target = hurtBox2;
                    OrbManager.instance.AddOrb(lightningOrb2);
                }
                
            }


            //apply charge if damageType is applycharge
            if (info.HasModdedDamageType(DamageTypes.applyCharge))
            {
                applyCharge(self, info);
            }

            //apply charge twice if damageType is apply2charge
            if (info.HasModdedDamageType(DamageTypes.apply2Charge))
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
            self.body.AddTimedBuff(Buffs.chargeBuildup, Modules.StaticValues.chargeDuration, Modules.StaticValues.chargeMaxStacks);

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

                            //create and fire charge blastattack centered on enemy 
                            BlastAttack chargeBlast;

                            chargeBlast = new BlastAttack
                            {
                                attacker = tracker.owner,
                                baseDamage = StaticValues.chargeDamageCoefficient * tracker.ownerBody.damage,
                                baseForce = 1f,
                                attackerFiltering = AttackerFiltering.NeverHitSelf,
                                crit = tracker.ownerBody.RollCrit(),
                                damageColorIndex = DamageColorIndex.Item,
                                damageType = DamageType.Shock5s,
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
