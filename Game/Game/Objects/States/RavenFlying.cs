// Copyright (c) Rodrigo Bento

using Game.Objects.Strategies;

using GameFramework.State;

using Microsoft.Xna.Framework;

namespace Game.Objects.States
{
	public class RavenFlying : RavenState
	{
		public RavenFlying(StateMachine<Raven> StateMachine)
			: base(StateMachine)
		{
			SetFlightStrategy();

			Owner.Image.FromFrameIndex = 0;
			Owner.Image.ToFrameIndex = 5;
			Owner.Image.Unpause();
		}

		private void SetFlightStrategy()
		{
			switch (Owner.Type)
			{
				case RavenType.kEvader:
					{
						Owner.FlightStrategy = new FlyAsAnEvader(Owner);
					}
					break;
				case RavenType.kStriker:
					{
						Owner.FlightStrategy = new FlyAsAStriker(Owner);
					}
					break;
				case RavenType.kWanderer:
					{
						Owner.FlightStrategy = new FlyAsAWanderer(Owner);
					}
					break;
			}
		}

		public override void Update(GameTime GameTime)
		{
			Owner.FlightStrategy.Fly();

			KeepOnScreenBounds();
		}

		private void KeepOnScreenBounds()
		{
			while (Owner.Left < 0)
			{
				Owner.X += 1f;
			}

			while (Owner.Right > Core.GraphicsDevice.Viewport.Width)
			{
				Owner.X -= 1f;
			}
		}

		public override void Draw(GameTime GameTime)
		{
			Owner.Image.Draw(GameTime);

			DebugDraw.DrawLine(Owner.Target, Owner.Position, Color.DarkGray);
			DebugDraw.DrawLine(Owner.RandomTarget, Owner.Position, Color.Green);
		}
	}
}
