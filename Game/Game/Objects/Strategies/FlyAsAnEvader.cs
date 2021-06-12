// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Manager;
using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace Game.Objects.Strategies
{
	public class FlyAsAnEvader : FlightStrategy
	{
		public FlyAsAnEvader(Raven Owner)
			: base(Owner)
		{
		}

		/// Tries to evade the cannon position while attracted to a random target.
		public override void Fly()
		{
			UpdateAnchor();
			UpdateRandomTarget();

			Physics Physics = Owner.GetComponent<Physics>();

			Vector2 ToTarget = Owner.RandomTarget - Owner.Position;
			Vector2 AwayFromCannon = -(Owner.Target - Owner.Position) * 0.5f;
			Vector2 Flee = ToTarget + AwayFromCannon;
			Vector2 Propulsion = Maths.CreateBySizeAndAngle(Physics.MaxSpeed, Maths.AngleInRadians(Flee)) / Physics.Mass;

			Vector2 Velocity = Propulsion * TimeManager.Instance.DeltaTime;
			Maths.Truncate(ref Velocity, Physics.MaxSpeed);

			Physics.Velocity = Velocity;

			Owner.Position += Physics.Velocity;
			Owner.Y = Owner.Anchor.Y;
		}
	}
}
