using BepInEx;
using R2API.Utils;
using RoR2;
using System.Security;
using System.Security.Permissions;
using HenryMod;
using EntityStates;
using UnityEngine;
using HenryMod.SkillStates;

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


            orig(self, info);

        }
        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
           
            if (self)
            {

                if (self.HasBuff(Modules.Buffs.chargeDebuff))
                {
                    
                    new BlastAttack
                    {
                        attacker = self.gameObject.GetComponent<Tracker>().owner,
                        baseDamage = Modules.StaticValues.chargeDamageCoefficient * self.gameObject.GetComponent<Tracker>().ownerBody.damage,
                        baseForce = 2f,
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

                    Destroy(self.gameObject.GetComponent<Tracker>());
                    self.RemoveBuff(Modules.Buffs.chargeDebuff);
                    
                }


                if (self.HasBuff(Modules.Buffs.chargeBuildup))
                {
                    int chargeCount = self.GetBuffCount(Modules.Buffs.chargeBuildup);

                    if (chargeCount >= 3)
                    {
                        self.AddBuff(Modules.Buffs.chargeDebuff);
                        for (int i = 0; i < chargeCount; i++)
                        {
                            self.RemoveBuff(Modules.Buffs.chargeBuildup);
                        }
                        
                    }

                }


            }


            orig(self);

        }

    }
}
