using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SkinChangeResponse = RoR2.CharacterSelectSurvivorPreviewDisplayController.SkinChangeResponse;


namespace AmpMod.Modules
{
    public static class Skins
    {
        public enum ampCSSEffect
        {
            DEFAULT,
            REDSPRITE
        }

        public static List<GameObject> allGameObjectActivations = new List<GameObject>();
        public static CharacterSelectSurvivorPreviewDisplayController ampCSSPreviewController;
        public static SkinChangeResponse[] defaultResponses;

        public static void AddCSSSkinChangeResponse(SkinDef def, ampCSSEffect cssEffect)
        {

            //duplicating a skinchangeresponse from defaults that I set up in editor
            SkinChangeResponse newSkinResponse = defaultResponses[(int)cssEffect];
            newSkinResponse.triggerSkin = def;

            //gotta do this song and dance instead of simply adding our own custom skinchangeresponses because for some reason adding to unityevents in code doesn't work
            //or at least didn't work last time i tried
            SkinChangeResponse[] addedSkinchange = new SkinChangeResponse[] {
                newSkinResponse
            };

            ampCSSPreviewController.skinChangeResponses = ampCSSPreviewController.skinChangeResponses.Concat(addedSkinchange).ToArray();
        }

        public static SkinDef.GameObjectActivation[] getActivations(params GameObject[] activatedObjects)
        {

            List<SkinDef.GameObjectActivation> GameObjectActivations = new List<SkinDef.GameObjectActivation>();

            for (int i = 0; i < allGameObjectActivations.Count; i++)
            {

                bool activate = activatedObjects.Contains(allGameObjectActivations[i]);

                GameObjectActivations.Add(new SkinDef.GameObjectActivation
                {
                    gameObject = allGameObjectActivations[i],
                    shouldActivate = activate
                });
            }

            return GameObjectActivations.ToArray();
        }


        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root)
        {
            return CreateSkinDef(skinName, skinIcon, rendererInfos, mainRenderer, root, null);
        }

        internal static SkinDef CreateSkinDef(string skinName, Sprite skinIcon, CharacterModel.RendererInfo[] rendererInfos, SkinnedMeshRenderer mainRenderer, GameObject root, UnlockableDef unlockableDef)
        {
            SkinDefInfo skinDefInfo = new SkinDefInfo
            {
                BaseSkins = Array.Empty<SkinDef>(),
                GameObjectActivations = new SkinDef.GameObjectActivation[0],
                Icon = skinIcon,
                MeshReplacements = new SkinDef.MeshReplacement[0],
                MinionSkinReplacements = new SkinDef.MinionSkinReplacement[0],
                Name = skinName,
                NameToken = skinName,
                ProjectileGhostReplacements = new SkinDef.ProjectileGhostReplacement[0],
                RendererInfos = rendererInfos,
                RootObject = root,
                UnlockableDef = unlockableDef
            };

            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef skinDef = ScriptableObject.CreateInstance<RoR2.SkinDef>();
            skinDef.baseSkins = skinDefInfo.BaseSkins;
            skinDef.icon = skinDefInfo.Icon;
            skinDef.unlockableDef = skinDefInfo.UnlockableDef;
            skinDef.rootObject = skinDefInfo.RootObject;
            skinDef.rendererInfos = skinDefInfo.RendererInfos;
            skinDef.gameObjectActivations = skinDefInfo.GameObjectActivations;
            skinDef.meshReplacements = skinDefInfo.MeshReplacements;
            skinDef.projectileGhostReplacements = skinDefInfo.ProjectileGhostReplacements;
            skinDef.minionSkinReplacements = skinDefInfo.MinionSkinReplacements;
            skinDef.nameToken = skinDefInfo.NameToken;
            skinDef.name = skinDefInfo.Name;

            On.RoR2.SkinDef.Awake -= DoNothing;

            return skinDef;
        }

        private static void DoNothing(On.RoR2.SkinDef.orig_Awake orig, RoR2.SkinDef self)
        {
        }

        internal struct SkinDefInfo
        {
            internal SkinDef[] BaseSkins;
            internal Sprite Icon;
            internal string NameToken;
            internal UnlockableDef UnlockableDef;
            internal GameObject RootObject;
            internal CharacterModel.RendererInfo[] RendererInfos;
            internal SkinDef.MeshReplacement[] MeshReplacements;
            internal SkinDef.GameObjectActivation[] GameObjectActivations;
            internal SkinDef.ProjectileGhostReplacement[] ProjectileGhostReplacements;
            internal SkinDef.MinionSkinReplacement[] MinionSkinReplacements;
            internal string Name;
        }
    }
}