// Copyright (c) Rodrigo Bento

using GameFramework.State;

using Microsoft.Xna.Framework;

namespace Game.Objects.States
{
	public class CannonDying : CannonState
	{
		public CannonDying(StateMachine<Cannon> StateMachine)
			: base(StateMachine)
		{
			Owner.Image.CurrentFrameIndex = 1;
			Owner.Image.FromFrameIndex = 1;
			Owner.Image.ToFrameIndex = 4;
			Owner.Image.Unpause();
		}

		public override void Update(GameTime GameTime)
		{
			if (Owner.Image.CurrentFrameIndex == 4)
			{
				StateMachine.ChangeState(new CannonDead(StateMachine));
			}

			Owner.Image.Position = Owner.Position;
			Owner.Image.Update(GameTime);
		}

		public override void Draw(GameTime GameTime)
		{
			Owner.Image.Draw(GameTime);
		}
	}
}
