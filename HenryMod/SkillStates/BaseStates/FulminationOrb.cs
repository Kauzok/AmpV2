
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
		// Token: 0x06002711 RID: 10001 RVA: 0x0009D3D8 File Offset: 0x0009B5D8
		public override void Begin()
		{
			base.duration = 0.1f;
			GameObject chainEffect = new GameObject();
			chainEffect = Modules.Assets.electricChainEffect;

			EffectData effectData = new EffectData
			{
				origin = this.origin,
				genericFloat = base.duration
			};
			effectData.SetHurtBoxReference(this.target);
			EffectManager.SpawnEffect(chainEffect, effectData, true);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x0009D51C File Offset: 0x0009B71C
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

		// Token: 0x06002713 RID: 10003 RVA: 0x0009D7C8 File Offset: 0x0009B9C8
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

		// Token: 0x140000B7 RID: 183
		// (add) Token: 0x06002714 RID: 10004 RVA: 0x0009D89C File Offset: 0x0009BA9C
		// (remove) Token: 0x06002715 RID: 10005 RVA: 0x0009D8D0 File Offset: 0x0009BAD0
		public static event Action<FulminationOrb> onFulminationOrbKilledOnAllBounces;

		// Token: 0x0400217D RID: 8573
		public float speed = 100f;

		// Token: 0x0400217E RID: 8574
		public float damageValue;

		// Token: 0x0400217F RID: 8575
		public GameObject attacker;

		// Token: 0x04002180 RID: 8576
		public GameObject inflictor;

		// Token: 0x04002181 RID: 8577
		public int bouncesRemaining;

		// Token: 0x04002182 RID: 8578
		public List<HealthComponent> bouncedObjects;

		// Token: 0x04002183 RID: 8579
		public TeamIndex teamIndex;

		// Token: 0x04002184 RID: 8580
		public bool isCrit;

		// Token: 0x04002185 RID: 8581
		public ProcChainMask procChainMask;

		// Token: 0x04002186 RID: 8582
		public float procCoefficient = 1f;

		// Token: 0x04002187 RID: 8583
		public DamageColorIndex damageColorIndex;

		// Token: 0x04002188 RID: 8584
		public float range = 20f;

		// Token: 0x04002189 RID: 8585
		public float damageCoefficientPerBounce = 1f;

		// Token: 0x0400218A RID: 8586
		public int targetsToFindPerBounce = 1;

		// Token: 0x0400218B RID: 8587
		public DamageType damageType;

		// Token: 0x0400218C RID: 8588
		private bool canBounceOnSameTarget;

		// Token: 0x0400218D RID: 8589
		private bool failedToKill;

	

		// Token: 0x0400218F RID: 8591
		private BullseyeSearch search;

		// Token: 0x02000637 RID: 1591
		
	}


}
