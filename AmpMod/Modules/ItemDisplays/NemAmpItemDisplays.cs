using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;


namespace AmpMod.Modules
{
    internal class NemAmpItemDisplays : ItemDisplaysBase
    {
        protected override void SetItemDisplayRules(List<ItemDisplayRuleSet.KeyAssetRuleGroup> itemDisplayRules)
        {


            // add item displays here
            // HIGHLY recommend using KingEnderBrine's ItemDisplayPlacementHelper mod for this
            #region Item Displays
            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Jetpack,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBugWings"),
                           childName = "Chest",
localPos = new Vector3(-0.00216F, 0.13812F, -0.15467F),
localAngles = new Vector3(21.4993F, 358.6616F, 358.3334F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.GoldGat,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldGat"),
childName = "ShoulderPadR",
localPos = new Vector3(0.00776F, -0.07367F, 0.00211F),
localAngles = new Vector3(314.1872F, 212.9417F, 318.6503F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.BFG,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBFG"),
childName = "UpperArmR",
localPos = new Vector3(-0.33394F, 0.1344F, -0.33429F),
localAngles = new Vector3(15.3757F, 146.7972F, 228.0725F),
localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.CritGlasses,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlasses"),
childName = "Head",
localPos = new Vector3(-0.00032F, 0.1025F, 0.13666F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.22F, 0.22F, 0.22F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Syringe,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySyringeCluster"),
childName = "UpperArmR",
localPos = new Vector3(-0.01599F, 0.00382F, 0.08396F),
localAngles = new Vector3(1.21826F, 50.96681F, 88.4743F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Behemoth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBehemoth"),
childName = "Chest",
localPos = new Vector3(0.11493F, -0.01107F, -0.22586F),
localAngles = new Vector3(355.8263F, 174.6551F, 131.9726F),
localScale = new Vector3(0.08F, 0.08F, 0.08F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Missile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncher"),
childName = "ShoulderPadL",
localPos = new Vector3(-0.02707F, -0.09218F, 0.28945F),
localAngles = new Vector3(355.2081F, 238.9531F, 243.7685F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Dagger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDagger"),
childName = "ShoulderPadR",
localPos = new Vector3(-0.17466F, 0.02338F, -0.12015F),
localAngles = new Vector3(306.6986F, 157.9853F, 174.8173F),
localScale = new Vector3(1.1F, 1.1F, 1.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Hoof,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHoof"),
childName = "CalfL",
localPos = new Vector3(-0.00008F, 0.40747F, -0.07673F),
localAngles = new Vector3(67.13473F, 0.62827F, 359.9727F),
localScale = new Vector3(0.1F, 0.1F, 0.08F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ChainLightning,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkulele"),
childName = "Chest",
localPos = new Vector3(0.00238F, -0.10468F, -0.16977F),
localAngles = new Vector3(0F, 180F, 89.3997F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.GhostOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMask"),
childName = "Head",
localPos = new Vector3(0F, 0.09432F, 0.10769F),
localAngles = new Vector3(351.7344F, 0F, 0F),
localScale = new Vector3(0.55F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Mushroom,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroom"),
childName = "UpperArmR",
localPos = new Vector3(0.01952F, 0.16037F, 0.00401F),
localAngles = new Vector3(314.5463F, 164.8076F, 112.986F),
localScale = new Vector3(0.0501F, 0.0501F, 0.0501F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.AttackSpeedOnCrit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWolfPelt"),
childName = "Head",
localPos = new Vector3(-0.00293F, 0.09875F, 0.11907F),
localAngles = new Vector3(358.4554F, 0F, 0F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BleedOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTip"),
childName = "Chest",
localPos = new Vector3(-0.10199F, 0.26749F, 0.06012F),
localAngles = new Vector3(34.45827F, 127.2111F, 93.75602F),
localScale = new Vector3(0.2615F, 0.2615F, 0.2615F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.WardOnLevel,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarbanner"),
childName = "Chest",
localPos = new Vector3(0.02089F, -0.16212F, -0.1851F),
localAngles = new Vector3(0F, 0F, 90F),
localScale = new Vector3(0.3162F, 0.3162F, 0.3162F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.HealOnCrit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScythe"),
childName = "Chest",
localPos = new Vector3(0.01432F, 0.23696F, -0.18612F),
localAngles = new Vector3(323.9545F, 90F, 270F),
localScale = new Vector3(0.14F, 0.14F, 0.14F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.HealWhileSafe,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySnail"),
childName = "ShoulderPadR",
localPos = new Vector3(-0.00962F, -0.08207F, 0.127F),
localAngles = new Vector3(331.7745F, 31.3836F, 172.4236F),
localScale = new Vector3(0.0289F, 0.0289F, 0.0289F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Clover,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayClover"),
childName = "HandR",
localPos = new Vector3(-0.03194F, 0.06859F, -0.03316F),
localAngles = new Vector3(76.40464F, 227.5206F, 11.19991F),
localScale = new Vector3(0.22F, 0.22F, 0.22F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BarrierOnOverHeal,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAegis"),
childName = "LowerArmR",
localPos = new Vector3(0.02688F, 0.09459F, 0.03533F),
localAngles = new Vector3(86.54781F, 162.2884F, 306.1439F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.GoldOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBoneCrown"),
childName = "Head",
localPos = new Vector3(-0.00444F, 0.11979F, 0.02136F),
localAngles = new Vector3(345.3948F, 0F, 0F),
localScale = new Vector3(0.7F, 1F, 0.8F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.WarCryOnMultiKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPauldron"),
childName = "UpperArmL",
localPos = new Vector3(0.07242F, 0.18826F, -0.03425F),
localAngles = new Vector3(64.04032F, 104.0662F, 16.86393F),
localScale = new Vector3(0.7094F, 0.7094F, 0.7094F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBuckler"),
childName = "LowerArmR",
localPos = new Vector3(-0.01208F, 0.10486F, -0.04722F),
localAngles = new Vector3(348.5351F, 192.6147F, 88.45072F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IceRing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayIceRing"),
childName = "LowerArmL",
localPos = new Vector3(-0.00798F, 0.01301F, -0.00779F),
localAngles = new Vector3(274.3965F, 90F, 270F),
localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
                    }
                }


            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Icicle,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                   {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFrostRelic"),
childName = "Base",
localPos = new Vector3(0.00805F, -0.13065F, -0.01686F),
localAngles = new Vector3(274.3965F, 90F, 270F),
localScale = new Vector3(0.6F, 0.6F, 0.6F),
                            limbMask = LimbFlags.None
                        }
                   }
                }


            }); ;

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FireRing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireRing"),
childName = "LowerArmL",
localPos = new Vector3(-0.00504F, 0.05132F, -0.00779F),
localAngles = new Vector3(274.3965F, 90F, 270F),
localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.UtilitySkillMagazine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
childName = "ShoulderPadL",
localPos = new Vector3(0.11458F, 0.14996F, -0.09358F),
localAngles = new Vector3(354.5014F, 273.2482F, 77.42171F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAfterburnerShoulderRing"),
childName = "ShoulderPadR",
localPos = new Vector3(-0.09324F, 0.06945F, -0.1289F),
localAngles = new Vector3(7.19265F, 276.6938F, 91.64787F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.JumpBoost,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaxBird"),
childName = "Head",
localPos = new Vector3(0F, -0.14269F, -0.03232F),
localAngles = new Vector3(355.4514F, 0F, 0F),
localScale = new Vector3(0.5253F, 0.5253F, 0.5253F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ArmorReductionOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarhammer"),
childName = "LowerArmL",
localPos = new Vector3(-0.01434F, 0.56247F, 0.0692F),
localAngles = new Vector3(272.7897F, 80.10728F, 345.7039F),
localScale = new Vector3(0.07F, 0.07F, 0.07F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.NearbyDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDiamond"),
childName = "HandR",
localPos = new Vector3(0.02705F, 0.02238F, -0.02235F),
localAngles = new Vector3(355.8105F, 55.14807F, 20.46667F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ArmorPlate,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRepulsionArmorPlate"),
childName = "CalfL",
localPos = new Vector3(0.01965F, 0.15993F, 0.0539F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(0.1971F, 0.1971F, 0.1971F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.CommandMissile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileRack"),
childName = "Chest",
localPos = new Vector3(0F, 0.2985F, -0.0663F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(0.3362F, 0.3362F, 0.3362F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Feather,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFeather"),
childName = "CalfL",
localPos = new Vector3(0.04979F, 0.24137F, 0.0472F),
localAngles = new Vector3(270F, 180F, 0F),
localScale = new Vector3(0.02F, 0.02F, 0.02F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Crowbar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCrowbar"),
childName = "Chest",
localPos = new Vector3(-0.12814F, 0.33562F, -0.14458F),
localAngles = new Vector3(3.41875F, 0F, 0F),
localScale = new Vector3(0.1936F, 0.1936F, 0.1936F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FallBoots,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
childName = "CalfL",
localPos = new Vector3(-0.00171F, 0.37593F, 0.0236F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1485F, 0.1485F, 0.1485F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravBoots"),
childName = "CalfR",
localPos = new Vector3(-0.0061F, 0.37379F, 0.02877F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1485F, 0.1485F, 0.1485F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LightningStrikeOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayChargedPerforator"),
childName = "HandR",
localPos = new Vector3(0.07525F, 0.11108F, 0.00022F),
localAngles = new Vector3(86.18515F, 326.8553F, 119.6914F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarSecondaryReplacement,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdClaw"),
childName = "Chest",
localPos = new Vector3(-0.08783F, 0.20251F, -0.06389F),
localAngles = new Vector3(356.8295F, 274.5803F, 332.9426F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarSpecialReplacement,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdHeart"),
childName = "Base",
localPos = new Vector3(-0.55805F, 1.08812F, -0.3895F),
localAngles = new Vector3(357.0966F, 320.2206F, 279.2867F),
localScale = new Vector3(.3F, .3F, .3F),
                            limbMask = LimbFlags.None
                        }
        }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ParentEgg,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayParentEgg"),
childName = "Chest",
localPos = new Vector3(-0.00168F, -0.06031F, 0.251F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
        }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ExecuteLowHealthElite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGuillotine"),
childName = "ThighR",
localPos = new Vector3(0.09472F, 0.12447F, -0.03862F),
localAngles = new Vector3(90F, 90F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });



            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.EquipmentMagazine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBattery"),
childName = "Chest",
localPos = new Vector3(0.02425F, 0.13611F, 0.15243F),
localAngles = new Vector3(0.14326F, 270.6027F, 292.832F),
localScale = new Vector3(0.0773F, 0.0773F, 0.0773F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.NovaOnHeal,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
childName = "Head",
localPos = new Vector3(0.0374F, 0.11705F, 0.04278F),
localAngles = new Vector3(277.0377F, 102.4834F, 298.3542F),
localScale = new Vector3(0.5349F, 0.5349F, 0.5349F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDevilHorns"),
childName = "Head",
localPos = new Vector3(-0.03357F, 0.11058F, 0.03584F),
localAngles = new Vector3(285.0471F, 327.8107F, 350.5691F),
localScale = new Vector3(-0.5349F, 0.5349F, 0.5349F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Infusion,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInfusion"),
childName = "Pelvis",
localPos = new Vector3(-0.16642F, 0.1265F, 0.04465F),
localAngles = new Vector3(338.9243F, 93.64492F, 76.4723F),
localScale = new Vector3(0.5253F, 0.5253F, 0.5253F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Medkit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMedkit"),
childName = "Chest",
localPos = new Vector3(-0.15259F, -0.19996F, -0.05241F),
localAngles = new Vector3(289.9938F, 10.22671F, 265.3182F),
localScale = new Vector3(0.4907F, 0.4907F, 0.4907F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Bandolier,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBandolier"),
childName = "UpperArmL",
localPos = new Vector3(0.00577F, 0.24715F, -0.0033F),
localAngles = new Vector3(73.56965F, 67.07795F, 58.78687F),
localScale = new Vector3(0.14841F, 0.22512F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BounceNearby,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHook"),
childName = "ShoulderPadL",
localPos = new Vector3(0.0186F, 0.05796F, 0.11975F),
localAngles = new Vector3(34.8842F, 321.6253F, 343.9465F),
localScale = new Vector3(0.214F, 0.214F, 0.214F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IgniteOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasoline"),
childName = "ThighL",
localPos = new Vector3(-0.09836F, 0.1101F, -0.00003F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.3165F, 0.3165F, 0.3165F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.StunChanceOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStunGrenade"),
childName = "ThighR",
localPos = new Vector3(0.02125F, 0.33559F, 0.10588F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.5672F, 0.5672F, 0.5672F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Firework,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFirework"),
childName = "HandR",
localPos = new Vector3(0.01559F, -0.0182F, 0.06461F),
localAngles = new Vector3(65.18207F, 335.7274F, 308.2704F),
localScale = new Vector3(0.1194F, 0.1194F, 0.1194F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarDagger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarDagger"),
childName = "Chest",
localPos = new Vector3(0.13051F, 0.18623F, -0.16518F),
localAngles = new Vector3(76.20583F, 120.8108F, 114.4994F),
localScale = new Vector3(0.6F, 0.6F, 0.6F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Knurl,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKnurl"),
childName = "LowerArmL",
localPos = new Vector3(-0.04283F, 0.05167F, 0.05764F),
localAngles = new Vector3(74.8007F, 51.35652F, 148.5256F),
localScale = new Vector3(0.03F, 0.03F, 0.03F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BeetleGland,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeetleGland"),
childName = "ShoulderPadL",
localPos = new Vector3(0.00491F, 0.05476F, -0.03538F),
localAngles = new Vector3(338.5466F, 259.3557F, 308.004F),
localScale = new Vector3(0.0553F, 0.0553F, 0.0553F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySoda"),
childName = "Pelvis",
localPos = new Vector3(0.02407F, 0.24205F, 0.09372F),
localAngles = new Vector3(307.719F, 251.0168F, -0.00001F),
localScale = new Vector3(0.1655F, 0.1655F, 0.1655F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SecondarySkillMagazine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDoubleMag"),
childName = "HandL",
localPos = new Vector3(-0.0018F, 0.0002F, 0.097F),
localAngles = new Vector3(84.2709F, 200.5981F, 25.0139F),
localScale = new Vector3(0.0441F, 0.0441F, 0.0441F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.StickyBomb,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStickyBomb"),
childName = "HandL",
localPos = new Vector3(0.01259F, 0.07307F, 0.02023F),
localAngles = new Vector3(7.98341F, 179.2016F, 187.4493F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.TreasureCache,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKey"),
childName = "Pelvis",
localPos = new Vector3(-0.06682F, 0.20573F, -0.11042F),
localAngles = new Vector3(35.68779F, 120.6982F, 328.0128F),
localScale = new Vector3(0.4092F, 0.4092F, 0.4092F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BossDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAPRound"),
childName = "Pelvis",
localPos = new Vector3(-0.0821F, 0.10425F, 0.21427F),
localAngles = new Vector3(52.82666F, 83.23834F, 96.21431F),
localScale = new Vector3(0.2279F, 0.2279F, 0.2279F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SlowOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBauble"),
childName = "Pelvis",
localPos = new Vector3(-0.08808F, 0.12767F, -0.31122F),
localAngles = new Vector3(294.5231F, 137.0405F, 33.50193F),
localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ExtraLife,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippo"),
childName = "Chest",
localPos = new Vector3(-0.00021F, -0.16487F, 0.08576F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.KillEliteFrenzy,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrainstalk"),
childName = "Head",
localPos = new Vector3(-0.00943F, 0.05471F, 0.01169F),
localAngles = new Vector3(0F, 0F, 1.68564F),
localScale = new Vector3(0.2F, 0.2638F, 0.12F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.RepeatHeal,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCorpseFlower"),
childName = "UpperArmR",
localPos = new Vector3(-0.0393F, 0.1484F, 0F),
localAngles = new Vector3(270F, 90F, 0F),
localScale = new Vector3(0.1511F, 0.1511F, 0.1511F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.AutoCastEquipment,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFossil"),
childName = "Chest",
localPos = new Vector3(0.10815F, 0.09989F, 0.17011F),
localAngles = new Vector3(346.5463F, 99.10973F, 344.9635F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.IncreaseHealing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(0.04661F, 0.16595F, 0.00566F),
localAngles = new Vector3(0F, 90F, 0F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAntler"),
childName = "Head",
localPos = new Vector3(-0.05835F, 0.16848F, 0F),
localAngles = new Vector3(0F, 90F, 0F),
localScale = new Vector3(0.2F, 0.2F, -0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.TitanGoldDuringTP,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGoldHeart"),
childName = "Chest",
localPos = new Vector3(-0.1095F, 0.09338F, 0.16165F),
localAngles = new Vector3(335.0033F, 343.2951F, 0F),
localScale = new Vector3(0.1191F, 0.1191F, 0.1191F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintWisp,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrokenMask"),
childName = "Chest",
localPos = new Vector3(0.15254F, -0.16028F, -0.05407F),
localAngles = new Vector3(0.97396F, 98.30422F, 5.76073F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BarrierOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBrooch"),
childName = "HandL",
localPos = new Vector3(-0.00255F, 0.052F, 0.0085F),
localAngles = new Vector3(55.10138F, 60.29852F, 81.93764F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.TPHealingNova,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlowFlower"),
childName = "UpperArmL",
localPos = new Vector3(0.06325F, 0.19529F, 0.04935F),
localAngles = new Vector3(0F, 73.1449F, 0F),
localScale = new Vector3(0.2731F, 0.2731F, 0.0273F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarUtilityReplacement,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdFoot"),
childName = "Head",
localPos = new Vector3(-0.0001F, 0.20271F, -0.09547F),
localAngles = new Vector3(358.9964F, 270.3312F, 341.6729F),
localScale = new Vector3(0.2833F, 0.2833F, 0.2833F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Thorns,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRazorwireLeft"),
childName = "UpperArmL",
localPos = new Vector3(0F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.4814F, 0.4814F, 0.4814F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarPrimaryReplacement,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBirdEye"),
childName = "Head",
localPos = new Vector3(-0.00094F, 0.09537F, 0.14295F),
localAngles = new Vector3(281.2693F, 177.9445F, 181.9358F),
localScale = new Vector3(0.2866F, 0.2866F, 0.2866F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.NovaOnLowHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayJellyGuts"),
childName = "Head",
localPos = new Vector3(-0.04838F, -0.04542F, 0.02843F),
localAngles = new Vector3(323.3276F, 28.46559F, 326.7364F),
localScale = new Vector3(0.1035F, 0.1035F, 0.1035F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LunarTrinket,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBeads"),
childName = "UpperArmL",
localPos = new Vector3(0F, 0.3249F, 0.0381F),
localAngles = new Vector3(0F, 0F, 90F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Plant,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayInterstellarDeskPlant"),
childName = "UpperArmR",
localPos = new Vector3(-0.0663F, 0.2266F, 0F),
localAngles = new Vector3(4.9717F, 270F, 54.4915F),
localScale = new Vector3(0.0429F, 0.0429F, 0.0429F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Bear,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBear"),
childName = "Chest",
localPos = new Vector3(0F, 0.04043F, 0.18494F),
localAngles = new Vector3(17.52866F, 0F, 0F),
localScale = new Vector3(0.08F, 0.08F, 0.08F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.DeathMark,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathMark"),
childName = "LowerArmR",
localPos = new Vector3(0.02439F, 0.09255F, 0.03987F),
localAngles = new Vector3(86.01828F, 139.9502F, 280.3236F),
localScale = new Vector3(0.02F, 0.02F, 0.02F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ExplodeOnDeath,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWilloWisp"),
childName = "Pelvis",
localPos = new Vector3(0.02643F, 0.24766F, 0.07132F),
localAngles = new Vector3(37.60441F, 245.8178F, 292.6664F),
localScale = new Vector3(0.02F, 0.02F, 0.02F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Seed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySeed"),
childName = "Head",
localPos = new Vector3(-0.13972F, 0.1366F, -0.02599F),
localAngles = new Vector3(344.0657F, 196.8238F, 275.5892F),
localScale = new Vector3(0.0275F, 0.0275F, 0.0275F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SprintOutOfCombat,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWhip"),
childName = "Pelvis",
localPos = new Vector3(-0.02611F, 0.28274F, -0.02699F),
localAngles = new Vector3(323.115F, 277.3284F, 280.2216F),
localScale = new Vector3(0.2845F, 0.2845F, 0.2845F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            /*        itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
                    {
                        keyAsset = RoR2Content.Items.CooldownOnCrit,
                        displayRuleGroup = new DisplayRuleGroup
                        {
                            rules = new ItemDisplayRule[]
                            {
                                new ItemDisplayRule
                                {
                                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                                    followerPrefab = ItemDisplays.LoadDisplay("DisplaySkull"),
        childName = "Chest",
        localPos = new Vector3(0F, 0.3997F, 0F),
        localAngles = new Vector3(270F, 0F, 0F),
        localScale = new Vector3(0.2789F, 0.2789F, 0.2789F),
                                    limbMask = LimbFlags.None
                                }
                            }
                        }
                    }); */

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Phasing,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayStealthkit"),
childName = "CalfL",
localPos = new Vector3(0.00341F, 0.2032F, -0.06842F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.1454F, 0.2399F, 0.16F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.PersonalShield,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldGenerator"),
childName = "Chest",
localPos = new Vector3(-0.0201F, 0.2064F, 0.08641F),
localAngles = new Vector3(304.1204F, 90F, 270F),
localScale = new Vector3(0.1057F, 0.1057F, 0.1057F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ShockNearby,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeslaCoil"),
childName = "Chest",
localPos = new Vector3(-0.00037F, 0.28243F, -0.09069F),
localAngles = new Vector3(297.6866F, 1.3864F, 358.5596F),
localScale = new Vector3(0.3229F, 0.3229F, 0.3229F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ShieldOnly,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
childName = "Head",
localPos = new Vector3(0.06739F, 0.21865F, 0F),
localAngles = new Vector3(348.1819F, 268.0985F, 0.3896F),
localScale = new Vector3(0.3521F, 0.3521F, 0.3521F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShieldBug"),
childName = "Head",
localPos = new Vector3(-0.06723F, 0.21789F, 0F),
localAngles = new Vector3(11.8181F, 268.0985F, 359.6104F),
localScale = new Vector3(0.3521F, 0.3521F, -0.3521F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.AlienHead,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayAlienHead"),
childName = "Chest",
localPos = new Vector3(-0.14007F, 0.21892F, -0.14836F),
localAngles = new Vector3(284.1172F, 243.7966F, 260.89F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.HeadHunter,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySkullCrown"),
childName = "Head",
localPos = new Vector3(0F, 0.14872F, 0.02744F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.35F, 0.13716F, 0.00979F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.EnergizedOnEquipmentUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWarHorn"),
childName = "Pelvis",
localPos = new Vector3(-0.05547F, 0.20815F, -0.10787F),
localAngles = new Vector3(29.4309F, 16.53837F, 69.36177F),
localScale = new Vector3(0.2732F, 0.2732F, 0.2732F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FlatHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySteakCurved"),
childName = "Head",
localPos = new Vector3(0.07405F, 0.22105F, 0.01153F),
localAngles = new Vector3(290.989F, 105.3815F, 159.0259F),
localScale = new Vector3(0.1245F, 0.1155F, 0.1155F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Tooth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayToothMeshLarge"),
childName = "Chest",
localPos = new Vector3(0F, 0.17562F, 0.11017F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Pearl,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPearl"),
childName = "LowerArmR",
localPos = new Vector3(-0.05539F, 0.08447F, 0.12244F),
localAngles = new Vector3(10.45553F, 118.5796F, 156.7527F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.ShinyPearl,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShinyPearl"),
childName = "Head",
localPos = new Vector3(-0.01163F, 0.09499F, 0.00845F),
localAngles = new Vector3(90F, 270F, 0F),
localScale = new Vector3(0.08F, 0.08F, 0.08F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BonusGoldPackOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTome"),
childName = "ThighR",
localPos = new Vector3(0.01359F, 0.20639F, -0.13381F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.04F, 0.04F, 0.04F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Squid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySquidTurret"),
childName = "ThighL",
localPos = new Vector3(0.00284F, 0.31334F, 0.08841F),
localAngles = new Vector3(83.32327F, 313.9159F, 307.5729F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Icicle,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
childName = "Base",
localPos = new Vector3(-0.55831F, 1.17972F, -0.22025F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.Talisman,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTalisman"),
childName = "Base",
localPos = new Vector3(0.68753F, 1.44374F, 0.07828F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.LaserTurbine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserTurbine"),
childName = "UpperArmR",
localPos = new Vector3(0.04115F, 0.20006F, 0.04842F),
localAngles = new Vector3(0.60686F, 41.09515F, 171.5579F),
localScale = new Vector3(0.2159F, 0.2159F, 0.2159F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FocusConvergence,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFocusedConvergence"),
childName = "Base",
localPos = new Vector3(0.34013F, 1.50159F, -0.44441F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.075F, 0.075F, 0.075F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            /*  itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
              {
                  keyAsset = RoR2Content.Items.Incubator,
                  displayRuleGroup = new DisplayRuleGroup
                  {
                      rules = new ItemDisplayRule[]
                      {
                          new ItemDisplayRule
                          {
                              ruleType = ItemDisplayRuleType.ParentedPrefab,
                              followerPrefab = ItemDisplays.LoadDisplay("DisplayAncestralIncubator"),
  childName = "Chest",
  localPos = new Vector3(0F, 0.3453F, 0F),
  localAngles = new Vector3(353.0521F, 317.2421F, 69.6292F),
  localScale = new Vector3(0.0528F, 0.0528F, 0.0528F),
                              limbMask = LimbFlags.None
                          }
                      }
                  }
              }); */

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.FireballsOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFireballsOnHit"),
childName = "UpperArmL",
localPos = new Vector3(-0.07605F, 0.20341F, 0.11634F),
localAngles = new Vector3(5.38191F, 328.2237F, 187.3559F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.SiphonOnLowHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySiphonOnLowHealth"),
childName = "Chest",
localPos = new Vector3(-0.15127F, 0.00172F, 0.16343F),
localAngles = new Vector3(16.31473F, 292.905F, 359.4807F),
localScale = new Vector3(0.0385F, 0.0385F, 0.0385F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.BleedOnHitAndExplode,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBleedOnHitAndExplode"),
childName = "ThighR",
localPos = new Vector3(-0.03324F, 0.05154F, -0.1223F),
localAngles = new Vector3(347.7571F, 265.3516F, 3.32358F),
localScale = new Vector3(0.03F, 0.03F, 0.03F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.MonstersOnShrineUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMonstersOnShrineUse"),
childName = "ThighR",
localPos = new Vector3(-0.01023F, 0.15514F, 0.11021F),
localAngles = new Vector3(352.4521F, 260.6884F, 3.55067F),
localScale = new Vector3(0.0246F, 0.0246F, 0.0246F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Items.RandomDamageZone,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRandomDamageZone"),
childName = "UpperArmL",
localPos = new Vector3(0.0709F, 0.4398F, 0.00551F),
localAngles = new Vector3(0F, 270F, 0F),
localScale = new Vector3(0.0465F, 0.0465F, 0.0465F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ImmuneToDebuff,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
childName = "ThighL",
localPos = new Vector3(-0.10228F, 0.07624F, 0.01783F),
localAngles = new Vector3(347.541F, 280.8244F, 172.9314F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Fruit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFruit"),
childName = "Chest",
localPos = new Vector3(-0.08222F, 0.13888F, -0.2122F),
localAngles = new Vector3(20.62047F, 16.95431F, 348.2455F),
localScale = new Vector3(0.2118F, 0.2118F, 0.2118F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixRed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
childName = "Head",
localPos = new Vector3(0.05395F, 0.13826F, 0.01988F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteHorn"),
childName = "Head",
localPos = new Vector3(-0.04968F, 0.13623F, 0.01976F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(-0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixBlue,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
childName = "Head",
localPos = new Vector3(-0.04386F, 0.18245F, 0.08936F),
localAngles = new Vector3(302.1855F, 276.2199F, 87.06351F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteRhinoHorn"),
childName = "Head",
localPos = new Vector3(0.04413F, 0.17753F, 0.08742F),
localAngles = new Vector3(302.3546F, 60.28681F, 281.598F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixWhite,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteIceCrown"),
childName = "Head",
localPos = new Vector3(0F, 0.22142F, 0F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.0265F, 0.0265F, 0.0265F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixPoison,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteUrchinCrown"),
childName = "Head",
localPos = new Vector3(0.00357F, 0.16665F, -0.00841F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(0.03F, 0.03F, 0.04F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.AffixHaunted,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteStealthCrown"),
childName = "Head",
localPos = new Vector3(-0.00001F, 0.18915F, -0.01667F),
localAngles = new Vector3(287.4809F, 179.9641F, 180.032F),
localScale = new Vector3(0.05F, 0.04F, 0.05F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.CritOnUse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayNeuralImplant"),
childName = "Head",
localPos = new Vector3(0.00119F, 0.10916F, 0.20605F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.2326F, 0.2326F, 0.2326F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.DroneBackup,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRadio"),
childName = "ThighR",
localPos = new Vector3(0.09274F, 0.12986F, -0.01083F),
localAngles = new Vector3(0F, 90F, 0F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Lightning,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLightningArmRight"),
childName = "UpperArmR",
localPos = new Vector3(-0.04714F, 0.03238F, 0.05162F),
localAngles = new Vector3(11.69168F, 6.25077F, 153.9781F),
localScale = new Vector3(0.3413F, 0.3413F, 0.3413F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.BurnNearby,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayPotion"),
childName = "Pelvis",
localPos = new Vector3(0.00004F, 0.09193F, 0.20961F),
localAngles = new Vector3(329.2559F, 44.32134F, 356.4506F),
localScale = new Vector3(0.0307F, 0.0307F, 0.0307F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.CrippleWard,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEffigy"),
childName = "Head",
localPos = new Vector3(-0.00016F, 0.1248F, -0.09442F),
localAngles = new Vector3(0F, 180F, 0F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.QuestVolatileBattery,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBatteryArray"),
childName = "Chest",
localPos = new Vector3(0.00002F, 0.31405F, -0.10771F),
localAngles = new Vector3(22.7542F, -0.00003F, 0.00966F),
localScale = new Vector3(0.2188F, 0.2188F, 0.2188F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.GainArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayElephantFigure"),
childName = "CalfR",
localPos = new Vector3(0F, 0.31112F, 0.12182F),
localAngles = new Vector3(77.5634F, 0F, 0F),
localScale = new Vector3(0.6279F, 0.6279F, 0.6279F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Recycle,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRecycler"),
childName = "Chest",
localPos = new Vector3(0.01729F, 0.24379F, -0.0772F),
localAngles = new Vector3(0F, 90F, 0F),
localScale = new Vector3(0.04F, 0.04F, 0.04F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.FireBallDash,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEgg"),
childName = "Pelvis",
localPos = new Vector3(-0.04243F, 0.1127F, 0.21963F),
localAngles = new Vector3(298.6992F, 257.1026F, 88.729F),
localScale = new Vector3(0.1891F, 0.1891F, 0.1891F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Cleanse,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWaterPack"),
childName = "Chest",
localPos = new Vector3(0F, 0.1996F, -0.0489F),
localAngles = new Vector3(0F, 180F, 0F),
localScale = new Vector3(0.0821F, 0.0821F, 0.0821F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Tonic,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTonic"),
childName = "Pelvis",
localPos = new Vector3(0.0963F, 0.07218F, 0.23053F),
localAngles = new Vector3(21.84666F, 41.13854F, 26.51692F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Gateway,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVase"),
childName = "Chest",
localPos = new Vector3(0.01359F, 0.0844F, -0.22266F),
localAngles = new Vector3(280.3294F, 0F, 0F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Meteor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMeteor"),
childName = "Base",
localPos = new Vector3(-0.66093F, 1.10617F, -0.25284F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Saw,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySawmerangFollower"),
childName = "Base",
localPos = new Vector3(-0.61797F, 1.1794F, -0.30405F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Blackhole,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGravCube"),
childName = "Base",
localPos = new Vector3(-0.51293F, 1.04766F, -0.27009F),
localAngles = new Vector3(1F, 1F, 1F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.Scanner,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScanner"),
childName = "ShoulderPadL",
localPos = new Vector3(0.09174F, -0.01101F, -0.05007F),
localAngles = new Vector3(27.55769F, 327.4694F, 273.591F),
localScale = new Vector3(0.0861F, 0.0861F, 0.0861F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.DeathProjectile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDeathProjectile"),
childName = "SpineBase",
localPos = new Vector3(0.0794F, 0.11545F, -0.16416F),
localAngles = new Vector3(5.17468F, 164.1196F, 116.5866F),
localScale = new Vector3(0.0596F, 0.0596F, 0.0596F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.LifestealOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLifestealOnHit"),
childName = "Head",
localPos = new Vector3(0.29729F, 0.10779F, 0.0494F),
localAngles = new Vector3(6.47669F, 253.2807F, 71.72951F),
localScale = new Vector3(0.1246F, 0.1246F, 0.1246F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = RoR2Content.Equipment.TeamWarCry,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
           {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTeamWarCry"),
childName = "ThighL",
localPos = new Vector3(-0.10228F, 0.07624F, 0.01783F),
localAngles = new Vector3(347.541F, 280.8244F, 172.9314F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
           }
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.AttackSpeedAndMoveSpeed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCoffee"),
childName = "CalfL",
localPos = new Vector3(-0.0908F, 0.14693F, 0.03274F),
localAngles = new Vector3(0.00424F, 280.9181F, 175.2504F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.BearVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBearVoid"),
childName = "Chest",
localPos = new Vector3(0F, 0.04043F, 0.18494F),
localAngles = new Vector3(17.52866F, 0F, 0F),
localScale = new Vector3(0.08F, 0.08F, 0.08F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.BleedOnHitVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTriTipVoid"),
childName = "Chest",
localPos = new Vector3(-0.16512F, 0.32029F, 0.06909F),
localAngles = new Vector3(37.35773F, 117.0752F, 95.88142F),
localScale = new Vector3(0.18F, 0.18F, 0.18F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ChainLightningVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayUkuleleVoid"),
childName = "Chest",
localPos = new Vector3(0.00238F, -0.10468F, -0.16977F),
localAngles = new Vector3(0F, 180F, 89.3997F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });


            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CloverVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayCloverVoid"),
childName = "HandR",
localPos = new Vector3(-0.03194F, 0.06859F, -0.03316F),
localAngles = new Vector3(76.40464F, 227.5206F, 11.19991F),
localScale = new Vector3(0.22F, 0.22F, 0.22F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CritDamage,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLaserSight"),
childName = "Head",
localPos = new Vector3(0.12561F, 0.12805F, 0.03201F),
localAngles = new Vector3(326.8664F, 87.09376F, 350.8085F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.CritGlassesVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGlassesVoid"),
childName = "Head",
localPos = new Vector3(0F, 0.09457F, 0.12826F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.23F, 0.23F, 0.23F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ElementalRingVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVoidRing"),
childName = "LowerArmL",
localPos = new Vector3(-0.00504F, 0.05132F, -0.00779F),
localAngles = new Vector3(274.3965F, 90F, 270F),
localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.EquipmentMagazineVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayFuelCellVoid"),
childName = "Chest",
localPos = new Vector3(0.02425F, 0.14172F, 0.15369F),
localAngles = new Vector3(16.41007F, 2.9059F, 93.76837F),
localScale = new Vector3(0.0773F, 0.0773F, 0.0773F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ExplodeOnDeathVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayWillowWispVoid"),
childName = "Pelvis",
localPos = new Vector3(0.02643F, 0.24766F, 0.07132F),
localAngles = new Vector3(37.60441F, 245.8178F, 292.6664F),
localScale = new Vector3(0.02F, 0.02F, 0.02F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ExtraLifeVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHippoVoid"),
childName = "Chest",
localPos = new Vector3(-0.00021F, -0.16487F, 0.08576F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.FragileDamageBonus,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDelicateWatch"),
childName = "LowerArmR",
localPos = new Vector3(-0.00842F, 0.16839F, 0.01589F),
localAngles = new Vector3(272.3073F, 97.48304F, 114.376F),
localScale = new Vector3(0.5F, 0.58864F, 0.5F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.FreeChest,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShippingRequestForm"),
childName = "ThighR",
localPos = new Vector3(0.10175F, 0.18744F, 0.00835F),
localAngles = new Vector3(278.1933F, 158.5136F, 112.816F),
localScale = new Vector3(0.6F, 0.6F, 0.6F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.GoldOnHurt,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRollOfPennies"),
childName = "ShoulderPadL",
localPos = new Vector3(0.03157F, -0.03613F, -0.10953F),
localAngles = new Vector3(9.41159F, 2.24264F, 163.2021F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HalfAttackSpeedHalfCooldowns,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarShoulderNature"),
childName = "ShoulderPadR",
localPos = new Vector3(-0.07798F, 0.09197F, -0.01783F),
localAngles = new Vector3(355.5224F, 311.8624F, 271.9178F),
localScale = new Vector3(0.7F, 0.7F, 0.8F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HalfSpeedDoubleHealth,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayLunarShoulderStone"),
childName = "ShoulderPadL",
localPos = new Vector3(0.07642F, 0.04547F, 0.0197F),
localAngles = new Vector3(359.6674F, 237.2707F, 220.9129F),
localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.HealingPotion,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayHealingPotion"),
childName = "CalfL",
localPos = new Vector3(-0.07741F, 0.18127F, -0.01628F),
localAngles = new Vector3(12.79356F, 281.5201F, 177.9985F),
localScale = new Vector3(0.05F, 0.05F, 0.05F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.ImmuneToDebuff,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRainCoatBelt"),
childName = "SpineBase",
localPos = new Vector3(-0.00644F, 0.00252F, 0.00051F),
localAngles = new Vector3(352.848F, 357.5392F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.LunarSun,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySunHead"),
childName = "Head",
localPos = new Vector3(-0.00672F, 0.09852F, 0.02464F),
localAngles = new Vector3(347.541F, 280.8244F, 172.9314F),
localScale = new Vector3(1.1F, 1.1F, 1.1F),
                            limbMask = LimbFlags.None
                        },

                             new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplaySunHeadNeck"),
childName = "Head",
localPos = new Vector3(-0.01181F, -0.07889F, 0.01684F),
localAngles = new Vector3(0F, 263.2526F, 0F),
localScale = new Vector3(1.1F, 1.1F, 1.1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MinorConstructOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDefenseNucleus"),
childName = "Base",
localPos = new Vector3(0.64205F, 0.98806F, -0.82301F),
localAngles = new Vector3(353.7775F, 359.0963F, 181.6518F),
localScale = new Vector3(0.4F, 0.4F, 0.4F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MissileVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMissileLauncherVoid"),
childName = "ShoulderPadL",
localPos = new Vector3(-0.02707F, -0.09218F, 0.28945F),
localAngles = new Vector3(355.2081F, 238.9531F, 243.7685F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MoreMissile,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayICBM"),
childName = "Chest",
localPos = new Vector3(0.03783F, 0.13871F, -0.22535F),
localAngles = new Vector3(328.8997F, 72.82346F, 33.2137F),
localScale = new Vector3(0.18F, 0.18F, 0.18F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MoveSpeedOnKill,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGrappleHook"),
childName = "ThighR",
localPos = new Vector3(0.09177F, 0.0704F, 0.00021F),
localAngles = new Vector3(351.6463F, 236.6809F, 194.0625F),
localScale = new Vector3(0.15F, 0.15F, 0.15F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.MushroomVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMushroomVoid"),
childName = "UpperArmR",
localPos = new Vector3(0.01952F, 0.16037F, 0.00401F),
localAngles = new Vector3(314.5463F, 164.8076F, 112.986F),
localScale = new Vector3(0.0501F, 0.0501F, 0.0501F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.OutOfCombatArmor,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayOddlyShapedOpal"),
childName = "HandR",
localPos = new Vector3(-0.04025F, 0.03954F, -0.00301F),
localAngles = new Vector3(348.3311F, 259.3435F, 158.67F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.PermanentDebuffOnHit,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayScorpion"),
childName = "SpineBase",
localPos = new Vector3(0.01342F, 0.11352F, -0.09678F),
localAngles = new Vector3(2.28321F, 5.84307F, 357.8635F),
localScale = new Vector3(0.8F, 0.8F, 0.8F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.PrimarySkillShuriken,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayShuriken"),
childName = "HandR",
localPos = new Vector3(0.00666F, 0.00313F, -0.00914F),
localAngles = new Vector3(281.6902F, 134.4164F, 203.4885F),
localScale = new Vector3(0.4F, 0.4F, 0.35356F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RandomEquipmentTrigger,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBottledChaos"),
childName = "ThighL",
localPos = new Vector3(-0.00746F, 0.1784F, 0.04867F),
localAngles = new Vector3(25.93376F, 172.2732F, 177.925F),
localScale = new Vector3(0.15F, 0.15F, 0.15F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RandomlyLunar,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayDomino"),
childName = "Base",
localPos = new Vector3(0.63866F, 0.98656F, -0.38707F),
localAngles = new Vector3(290.7207F, 332.4745F, 55.1987F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.RegeneratingScrap,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayRegeneratingScrap"),
childName = "SpineBase",
localPos = new Vector3(0.14362F, 0.07328F, -0.0003F),
localAngles = new Vector3(4.99725F, 266.408F, 1.31588F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.SlowOnHitVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBaubleVoid"),
childName = "Pelvis",
localPos = new Vector3(-0.13901F, 0.22321F, 0.23239F),
localAngles = new Vector3(351.5663F, 225.8273F, 47.86088F),
localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.StrengthenBurn,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGasTank"),
childName = "CalfR",
localPos = new Vector3(0.07355F, 0.24262F, -0.02652F),
localAngles = new Vector3(357.0444F, 269.3301F, 169.0066F),
localScale = new Vector3(0.15F, 0.15F, 0.15F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.TreasureCacheVoid,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayKeyVoid"),
childName = "Pelvis",
localPos = new Vector3(-0.06682F, 0.20573F, -0.11042F),
localAngles = new Vector3(35.68779F, 120.6982F, 328.0128F),
localScale = new Vector3(0.4092F, 0.4092F, 0.4092F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Items.VoidMegaCrabItem,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMegaCrabItem"),
childName = "Pelvis",
localPos = new Vector3(0.12435F, 0.18167F, 0.07322F),
localAngles = new Vector3(312.5259F, 86.62113F, 45.95373F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunter,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayBlunderbuss"),
childName = "Base",
localPos = new Vector3(0.80368F, 0.54514F, -0.52292F),
localAngles = new Vector3(84.07719F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F),
                            limbMask = LimbFlags.None
                        },
                             new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTricornGhost"),
childName = "Head",
localPos = new Vector3(0.00309F, 0.25918F, -0.04587F),
localAngles = new Vector3(19.50595F, 0F, 0F),
localScale = new Vector3(0.8F, 0.8F, 0.8F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });


            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.BossHunterConsumed,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayTricornUsed"),
childName = "Head",
localPos = new Vector3(0.00309F, 0.25918F, -0.04587F),
localAngles = new Vector3(19.50595F, 0F, 0F),
localScale = new Vector3(0.8F, 0.8F, 0.8F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.GummyClone,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayGummyClone"),
childName = "ShoulderPadL",
localPos = new Vector3(0.04681F, -0.00541F, 0.12906F),
localAngles = new Vector3(33.40055F, 198.9697F, 198.0236F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.Molotov,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayMolotov"),
childName = "Chest",
localPos = new Vector3(0.0019F, 0.25274F, -0.06465F),
localAngles = new Vector3(28.42361F, 66.84879F, 347.0397F),
localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.MultiShopCard,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayExecutiveCard"),
childName = "Pelvis",
localPos = new Vector3(0.01007F, 0.2525F, 0.0717F),
localAngles = new Vector3(351.4009F, 307.2755F, 125.1998F),
localScale = new Vector3(0.7F, 0.7F, 0.7F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Elites.Earth.eliteEquipmentDef,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayEliteMendingAntlers"),
childName = "Head",
localPos = new Vector3(0.0071F, 0.14804F, 0.01249F),
localAngles = new Vector3(359.955F, 359.2637F, 0.10586F),
localScale = new Vector3(0.5F, 0.5F, 0.5F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });

            itemDisplayRules.Add(new ItemDisplayRuleSet.KeyAssetRuleGroup
            {
                keyAsset = DLC1Content.Equipment.VendingMachine,
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
{
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = ItemDisplays.LoadDisplay("DisplayVendingMachine"),
childName = "Pelvis",
localPos = new Vector3(-0.1178F, 0.24194F, 0.00198F),
localAngles = new Vector3(331.4462F, 106.3427F, 35.58772F),
localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
}
                }
            });



            #endregion
        }
    }
}
