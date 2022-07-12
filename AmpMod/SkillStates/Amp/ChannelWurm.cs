using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;

namespace AmpMod.SkillStates
{
    public class ChannelWurm : BaseChannelWurm
    {

        public override void OnEnter()
        {
            base.OnEnter();
        }

        protected override SummonWurm GetNextState()
        {
            return new SummonWurm();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
