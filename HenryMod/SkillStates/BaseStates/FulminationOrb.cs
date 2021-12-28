
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using RoR2.Orbs;
using UnityEngine;
using RoR2;

namespace HenryMod.SkillStates.BaseStates
{
    //orb meant to be used to implement chain lightning function for fulmination skill
   public class FulminationOrb : Orb
    {
		public static event Action<FulminationOrb> onFulminationOrbKilledOnAllBounces;
		public float speed = 100f;
		public float damageValue;
		public GameObject attacker;
		public GameObject inflictor;
		public int bouncesRemaining;
		public List<HealthComponent> bouncedObjects;
		public TeamIndex teamIndex;
		public bool isCrit;
		public ProcChainMask procChainMask;
		public float procCoefficient = 1f;
		public DamageColorIndex damageColorIndex;
		public float range = 20f;
		public float damageCoefficientPerBounce = 1f;
		public int targetsToFindPerBounce = 1;
		public DamageType damageType;
		private bool canBounceOnSameTarget;
		private bool failedToKill;
		private BullseyeSearch search;

		public override void Begin()
		{
			base.duration = 5f;
			GameObject chainEffect;
			chainEffect = Modules.Assets.electricChainEffect;

			EffectData effectData = new EffectData
			{
				origin = this.origin,
				genericFloat = base.duration,
			
				
			};
			effectData.SetHurtBoxReference(this.target);
			
			//Effect is currently ripped from the artificer's nanobomb effect, but will try to get our own custom effect working later

			EffectManager.SpawnEffect(Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/MageLightningOrbEffect"), effectData, true);
			//EffectManager.SpawnEffect(chainEffect, effectData, true);

		}
		public override void OnArrival()
		{
			if (this.target)
			{
				HealthComponent healthComponent = this.target.healthComponent;
				if (healthComponent)
				{
					DamageInfo damageInfo = new DamageInfo();
					damageInfo.damage = this.damageValue;
					damageInfo.attacker = this.attacker;
					damageInfo.inflictor = this.inflictor;
					damageInfo.force = Vector3.zero;
					damageInfo.crit = this.isCrit;
					damageInfo.procChainMask = this.procChainMask;
					damageInfo.procCoefficient = this.procCoefficient;
					damageInfo.position = this.target.transform.position;
					damageInfo.damageColorIndex = this.damageColorIndex;
					damageInfo.damageType = this.damageType;
					healthComponent.TakeDamage(damageInfo);
					GlobalEventManager.instance.OnHitEnemy(damageInfo, healthComponent.gameObject);
					GlobalEventManager.instance.OnHitAll(damageInfo, healthComponent.gameObject);
				}
				this.failedToKill |= (!healthComponent || healthComponent.alive);
				if (this.bouncesRemaining > 0)
				{
					for (int i = 0; i < this.targetsToFindPerBounce; i++)
					{
						if (this.bouncedObjects != null)
						{
							if (this.canBounceOnSameTarget)
							{
								this.bouncedObjects.Clear();
							}
							this.bouncedObjects.Add(this.target.healthComponent);
						}
						HurtBox hurtBox = this.PickNextTarget(this.target.transform.position);
						if (hurtBox)
						{
							FulminationOrb fulminationOrb = new FulminationOrb();
							fulminationOrb.search = this.search;
							fulminationOrb.origin = this.target.transform.position;
							fulminationOrb.target = hurtBox;
							fulminationOrb.attacker = this.attacker;
							fulminationOrb.inflictor = this.inflictor;
							fulminationOrb.teamIndex = this.teamIndex;
							fulminationOrb.damageValue = this.damageValue * this.damageCoefficientPerBounce;
							fulminationOrb.bouncesRemaining = this.bouncesRemaining - 1;
							fulminationOrb.isCrit = this.isCrit;
							fulminationOrb.bouncedObjects = this.bouncedObjects;
					
							fulminationOrb.procChainMask = this.procChainMask;
							fulminationOrb.procCoefficient = this.procCoefficient;
							fulminationOrb.damageColorIndex = this.damageColorIndex;
							fulminationOrb.damageCoefficientPerBounce = this.damageCoefficientPerBounce;
							fulminationOrb.speed = this.speed;
							fulminationOrb.range = this.range;
							fulminationOrb.damageType = this.damageType;
							fulminationOrb.failedToKill = this.failedToKill;
							OrbManager.instance.AddOrb(fulminationOrb);
						}
					}
					return;
				}
				if (!this.failedToKill)
				{
					Action<FulminationOrb> action = FulminationOrb.onFulminationOrbKilledOnAllBounces;
					if (action == null)
					{
						return;
					}
					action(this);
				}
			}
		}

		public HurtBox PickNextTarget(Vector3 position)
		{
			if (this.search == null)
			{
				this.search = new BullseyeSearch();
			}
			this.search.searchOrigin = position;
			this.search.searchDirection = Vector3.zero;
			this.search.teamMaskFilter = TeamMask.allButNeutral;
			this.search.teamMaskFilter.RemoveTeam(this.teamIndex);
			this.search.filterByLoS = false;
			this.search.sortMode = BullseyeSearch.SortMode.Distance;
			this.search.maxDistanceFilter = this.range;
			this.search.RefreshCandidates();
			HurtBox hurtBox = (from v in this.search.GetResults()
							   where !this.bouncedObjects.Contains(v.healthComponent)
							   select v).FirstOrDefault<HurtBox>();
			if (hurtBox)
			{
				this.bouncedObjects.Add(hurtBox.healthComponent);
			}
			return hurtBox;
		}

		
	}


}
