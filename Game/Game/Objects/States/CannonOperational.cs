// Copyright (c) Rodrigo Bento

using GameFramework.State;

using Microsoft.Xna.Framework;

namespace Game.Objects.States
{
	public class CannonOperational : CannonState
	{
		public CannonOperational(StateMachine<Cannon> StateMachine)
			: base(StateMachine)
		{
			Owner.Image.CurrentFrameIndex = 0;
			Owner.Image.Pause();
		}

		public override void Update(GameTime GameTime)
		{
			KeepOnScreenBounds();

			Owner.Image.Position = Owner.Position;
		}

		private void KeepOnScreenBounds()
		{
			while (Owner.Left < 5)
			{
				Owner.X += 1f;
			}

			while (Owner.Right > (Core.GraphicsDevice.Viewport.Width - 5))
			{
				Owner.X -= 1f;
			}
		}

		public override void Draw(GameTime GameTime)
		{
			Owner.Image.Draw(GameTime);
		}
	}
}
