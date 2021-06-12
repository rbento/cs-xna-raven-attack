// Copyright (c) Rodrigo Bento

using GameFramework.State;

using Microsoft.Xna.Framework;

namespace Game.Objects.States
{
	public class RavenDying : RavenState
	{
		public RavenDying(StateMachine<Raven> StateMachine)
			: base(StateMachine)
		{
			Owner.Image.CurrentFrameIndex = 6;
			Owner.Image.FromFrameIndex = 6;
			Owner.Image.ToFrameIndex = 8;
			Owner.Image.Unpause();
		}

		public override void Update(GameTime GameTime)
		{
			if (Owner.Image.CurrentFrameIndex == 8)
			{
				StateMachine.ChangeState(new RavenDead(StateMachine));
			}
		}

		public override void Draw(GameTime GameTime)
		{
			Owner.Image.Draw(GameTime);
		}
	}
}
