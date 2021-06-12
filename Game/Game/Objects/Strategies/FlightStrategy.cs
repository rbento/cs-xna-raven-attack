// Copyright (c) Rodrigo Bento

using Game.Data;

using GameFramework.Manager;
using GameFramework.State;
using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace Game.Objects.Strategies
{
	public abstract class FlightStrategy : IStrategy<Raven>
	{
		protected const float kAnchorUpdateTimeout = 7000f;
		protected const float kTargetUpdateTimeout = 400f;

		protected const float kMinPositionRadius = 20f;
		protected const float kMinTargetRadius = 50f;
		protected const float kMaxTargetRadius = 400f;

		protected const float kMaxSpeed = 250f;

		protected float mNextAnchorUpdateTimeout;
		protected float mNextTargetUpdateTimeout;

		public FlightStrategy(Raven Owner)
			: base()
		{
			this.Owner = Owner;
		}

		/// Randomly updates the raven's anchor point.
		protected void UpdateAnchor()
		{
			float CurrentTime = TimeManager.Instance.TotalMilliseconds;

			if (CurrentTime > mNextAnchorUpdateTimeout)
			{
				Owner.Anchor = GameData.GetRandomAnchorByRavenType(Owner.Type);
				mNextAnchorUpdateTimeout = CurrentTime + kAnchorUpdateTimeout;
			}
		}

		/// Randomly chooses the raven's target.
		protected void UpdateRandomTarget()
		{
			float CurrentTime = TimeManager.Instance.TotalMilliseconds;

			if (CurrentTime > mNextTargetUpdateTimeout)
			{
				float targetAngle = Randoms.NextInt(0, 1024) % 2 == 0 ? 0 : MathHelper.Pi;
				float targetRadius = Randoms.NextFloat(kMinTargetRadius, kMaxTargetRadius);

				Owner.RandomTarget = Owner.Anchor + Maths.CreateBySizeAndAngle(targetRadius, targetAngle);
				mNextTargetUpdateTimeout = CurrentTime + kTargetUpdateTimeout;
			}
		}

		public abstract void Fly();

		public Raven Owner { get; private set; }
	}
}
