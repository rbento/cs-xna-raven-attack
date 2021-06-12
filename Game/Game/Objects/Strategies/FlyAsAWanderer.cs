// Copyright (c) Rodrigo Bento

using GameFramework.Base;
using GameFramework.Manager;
using GameFramework.Util;

using Microsoft.Xna.Framework;

using System;

namespace Game.Objects.Strategies
{
	public class FlyAsAWanderer : FlightStrategy
	{
		public FlyAsAWanderer(Raven Owner)
			: base(Owner)
		{
		}

		/// Always seeks a random target, disregarding the cannon position.
		public override void Fly()
		{
			UpdateAnchor();
			UpdateRandomTarget();

			Physics Physics = Owner.GetComponent<Physics>();

			Vector2 Wander = (Owner.RandomTarget - Owner.Position);

			float TargetRange = Math.Abs(Wander.Length());

			if (TargetRange > kMinPositionRadius)
			{
				Vector2 Propulsion = Maths.CreateBySizeAndAngle(Physics.MaxSpeed, Maths.AngleInRadians(Wander)) / Physics.Mass;

				Vector2 Velocity = Propulsion * TimeManager.Instance.DeltaTime;
				Maths.Truncate(ref Velocity, Physics.MaxSpeed);

				Physics.Velocity = Velocity;

				Owner.Position += Physics.Velocity;
				Owner.Y = Owner.Anchor.Y;
			}
		}
	}
}
