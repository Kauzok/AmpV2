using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using System.Collections.Generic;
using RoR2.Projectile;
using R2API;
using AmpMod.Modules;
using System.Linq;
using System.Collections.ObjectModel;

namespace AmpMod.SkillStates.Amp.BaseStates
{
    public abstract class BaseSkillFire : BaseSkillState
    {
        public Vector3 aimPosition;
        public Quaternion aimRotation;
    }
}
