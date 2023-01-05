using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using System.Linq;
using EntityStates;
using RoR2.Skills;
using RoR2.Orbs;
using UnityEngine;
using UnityEngine.Networking;
using AmpMod.Modules;

namespace AmpMod.SkillStates.Nemesis_Amp
{
    class AOELightning : BaseSkillState
    {
        private float lightningRadius = Modules.StaticValues.stormRadius;
        private float baseStrikeDamage = Modules.StaticValues.baseBoltDamageCoefficient;
        private float perBuffStrikeDamage = Modules.StaticValues.additionalBoltDamageCoefficient;
        private GameObject lightningEffect;
        private HurtBox[] lightningTargets;
        private float duration;
        private float surgeBuffCount;
        private StackDamageController stackDamageController;

        public override void OnEnter()
        {
            base.OnEnter();
            stackDamageController = base.GetComponent<StackDamageController>();
            surgeBuffCount = base.GetBuffCount(Modules.Buffs.damageGrowth);

            //lightningEffect = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OrbEffects/LightningStrikeOrbEffect");
            //find all enemy hurtboxes within a radius
            if (NetworkServer.active)
            {
                BullseyeSearch lightningSearch = new BullseyeSearch();
                lightningSearch.filterByDistinctEntity = true;
                lightningSearch.filterByLoS = false;
                lightningSearch.maxDistanceFilter = lightningRadius;
                lightningSearch.minDistanceFilter = 0f;
                lightningSearch.minAngleFilter = 0f;
                lightningSearch.maxAngleFilter = 180f;
                lightningSearch.sortMode = BullseyeSearch.SortMode.Distance;
                lightningSearch.teamMaskFilter = TeamMask.GetUnprotectedTeams(base.GetTeam());
                lightningSearch.searchOrigin = base.characterBody.corePosition;
                lightningSearch.viewer = null;
                lightningSearch.RefreshCandidates();
                lightningSearch.FilterOutGameObject(base.gameObject);
                IEnumerable<HurtBox> results = lightningSearch.GetResults();
                this.lightningTargets = results.ToArray<HurtBox>();
            }
            

            //for every hurtbox, summon a lightning bolt on them
            for (int i = 0; i < lightningTargets.Length; i++)
            {

                if (NetworkServer.active)
                {
                    int controlledChargeCount = lightningTargets[i].healthComponent.body.GetBuffCount(Modules.Buffs.controlledCharge);
                    //base lightning strike damage w/o the growth damage buff
                    float calcedLightningStrikeDamage = (controlledChargeCount * perBuffStrikeDamage) + baseStrikeDamage;

                    //add in the growth damage buff to calculation
                    float baseDamage = calcedLightningStrikeDamage + calcedLightningStrikeDamage * (base.characterBody.GetBuffCount(Buffs.damageGrowth) * StaticValues.growthDamageCoefficient);
                    OrbManager.instance.AddOrb(new NemAmpLightningStrikeOrb
                    {
                        attacker = base.gameObject,
                        damageColorIndex = DamageColorIndex.Default,
                        //damageValue is based on base damage + additional damage for every stack of controlled charge
                        damageValue = base.characterBody.damage * baseDamage,
                        isCrit = Util.CheckRoll(this.characterBody.crit, this.characterBody.master),
                        procChainMask = default(ProcChainMask),
                        procCoefficient = 1f,
                        //orbEffect = lightningEffect,
                        target = lightningTargets[i]
                    }); ;

                    //clear stacks of controlled charge on the enemies who've been hit
                    lightningTargets[i].healthComponent.body.ClearTimedBuffs(Buffs.controlledCharge);

                }
               

                
            }
            stackDamageController.newSkillUsed = this;
            stackDamageController.resetComboTimer();


        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority)
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
