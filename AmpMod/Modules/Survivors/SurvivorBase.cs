using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using AmpMod.SkillStates.Nemesis_Amp;
using AmpMod.SkillStates.Nemesis_Amp.Orbs;

namespace AmpMod.Modules.Survivors
{
    internal abstract class SurvivorBase
    {
        internal static SurvivorBase instance;

        internal abstract string bodyName { get; set; }

        internal abstract GameObject bodyPrefab { get; set; }
        internal abstract GameObject displayPrefab { get; set; }

        internal string fullBodyName => bodyName + "Body";

        internal abstract ConfigEntry<bool> characterEnabled { get; set; }

        internal abstract bool isAmp { get; set; }

        internal abstract bool isNemAmp { get; set; }

        internal abstract UnlockableDef characterUnlockableDef { get; set; }

        internal abstract BodyInfo bodyInfo { get; set; }

        internal abstract int mainRendererIndex { get; set; }
        internal abstract CustomRendererInfo[] customRendererInfos { get; set; }

        internal abstract Type characterMainState { get; set; }

        public virtual ItemDisplaysBase itemDisplays { get; } = null;

        internal abstract List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules { get; set; }

        internal virtual void Initialize()
        {
            instance = this;
            InitializeCharacter();
        }

        internal virtual void InitializeCharacter()
        {
            // this creates a config option to enable the character- feel free to remove if the character is the only thing in your mod
            characterEnabled = Modules.Config.CharacterEnableConfig(bodyName);

            if (characterEnabled.Value)
            {
                InitializeUnlockables();


                bodyPrefab = Modules.Prefabs.CreatePrefab(bodyName + "Body", "mdl" + bodyName, bodyInfo);
                if (isAmp)
                {
                    bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(typeof(AmpMain));
                }

                if (isNemAmp)
                {
                    bodyPrefab.GetComponent<EntityStateMachine>().mainStateType = new EntityStates.SerializableEntityStateType(typeof(NemAmpMain));
                }
               
                Modules.Prefabs.SetupCharacterModel(bodyPrefab, customRendererInfos, mainRendererIndex);

                displayPrefab = Modules.Prefabs.CreateDisplayPrefab(bodyName + "Display", bodyPrefab, bodyInfo);
                
                Modules.Prefabs.RegisterNewSurvivor(bodyPrefab, displayPrefab, Color.grey, bodyName.ToUpper(), characterUnlockableDef, 101f);

                InitializeHitboxes();
                InitializeSkills();
                InitializeSkins();
                InitializeItemDisplays();
                InitializeDoppelganger();

                // var wormSkill = bodyPrefab.AddComponent<SkillStates.SkillComponents.WormSkillComponent>();

                if (isAmp)
                {
                    bodyPrefab.AddComponent<AmpLightningController>();

                    var menuSound = displayPrefab.AddComponent<SkillStates.SkillComponents.PlayMenuSound>();
                    menuSound.soundString = "PlayLobbyEntrance";
                }

                if (isNemAmp)
                {
                    bodyPrefab.AddComponent<NemAmpLightningTracker>();
                    bodyPrefab.AddComponent<StackDamageController>();
                    bodyPrefab.AddComponent<NemAmpLightningController>();
                    bodyPrefab.AddComponent<NemAmpLightningEffectController>();
                }

                isAmp = false;
                isNemAmp = false;

            }
        }
      



        internal virtual void InitializeUnlockables()
        {
        }

        internal virtual void InitializeSkills()
        {
        }

        internal virtual void InitializeHitboxes()
        {
        }

        internal virtual void InitializeSkins()
        {
        }

        internal virtual void InitializeDoppelganger()
        {
            Modules.Prefabs.CreateGenericDoppelganger(instance.bodyPrefab, bodyName + "MonsterMaster", "Merc");
        }


        public virtual void InitializeItemDisplays()
        {
            CharacterModel characterBodyModel = bodyPrefab.GetComponentInChildren<CharacterModel>();

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();
            itemDisplayRuleSet.name = "idrs" + bodyName;

            characterBodyModel.itemDisplayRuleSet = itemDisplayRuleSet;

            if (itemDisplays != null)
            {
                RoR2.ContentManagement.ContentManager.onContentPacksAssigned += SetItemDisplays;
            }
        }


        public void SetItemDisplays(HG.ReadOnlyArray<RoR2.ContentManagement.ReadOnlyContentPack> obj)
        {
            CharacterModel characterBodyModel = bodyPrefab.GetComponentInChildren<CharacterModel>();

            itemDisplays.SetItemDisplays(characterBodyModel.itemDisplayRuleSet);
        }
    }
}