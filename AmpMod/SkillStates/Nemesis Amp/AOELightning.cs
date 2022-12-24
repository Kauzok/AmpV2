using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using System.Linq;
using EntityStates;
using RoR2.Skills;
using RoR2.Orbs;
using UnityEngine.Networking;


namespace AmpMod.SkillStates.Nemesis_Amp
{
    class AOELightning : BaseSkillState
    {
        private float lightningRadius = Modules.StaticValues.stormRadius;
        private float baseStrikeDamage = Modules.StaticValues.baseBoltDamageCoefficient;
        private HurtBox[] lightningTargets;
        private float duration;

        public override void OnEnter()
        {
            base.OnEnter();

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
                int controlledChargeCount = lightningTargets[i].healthComponent.body.GetBuffCount(Modules.Buffs.controlledCharge);
                OrbManager.instance.AddOrb(new NemAmpLightningOrb
                {
                    attacker = base.gameObject,
                    damageColorIndex = DamageColorIndex.Default,
                    //damageValue is based on base damage + additional damage for every stack of controlled charge
                    damageValue = (this.characterBody.damage * 4f) + (base.characterBody.damage * controlledChargeCount),
                    isCrit = Util.CheckRoll(this.characterBody.crit, this.characterBody.master),
                    procChainMask = default(ProcChainMask),
                    procCoefficient = 1f,
                    target = lightningTargets[i]
                });;

                lightningTargets[i].healthComponent.body.ClearTimedBuffs(Modules.Buffs.controlledCharge);
            }


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
