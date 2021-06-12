// Copyright (c) Rodrigo Bento

using GameFramework.Asset;
using GameFramework.Base;
using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace Game.Objects
{
	public class Shot : GameActor
	{
		public const float kSpeed = 450f;

		private const int kWidth = 9;
		private const int kHeight = 9;

		private readonly Cannon mOwner;

		private GameImage mImage;

		public Shot(Cannon Owner)
			: base(kWidth, kHeight)
		{
			mOwner = Owner;
			mImage = new GameImage("Images/txShot");

			AddComponent<AABBCollision>(new AABBCollision(this));
			AddComponent<Physics>(new Physics(this));

			Angle = -MathHelper.PiOver2; // Up

			GetComponent<Physics>().Velocity = Maths.CreateBySizeAndAngle(kSpeed, Angle);
		}

		public override void Cleanup()
		{
			base.Cleanup();

			mImage = null;
		}

		public override void Update(GameTime GameTime)
		{
			Physics Physics = GetComponent<Physics>();

			if (mOwner.IsFiring)
			{
				/// Shot follows cannon momentum.
				Position += Physics.Velocity * TimeManager.DeltaTime;
			}
			else
			{
				/// Shot is locked and moves along with the cannon.
				Position = mOwner.Position;
			}

			mImage.Rotation = Angle;
			mImage.Position = Position;
		}

		public override void Draw(GameTime GameTime)
		{
			base.Draw(GameTime);

			if (mOwner.IsOperational || mOwner.IsFiring)
			{
				mImage.Draw(GameTime);
			}
		}
	}
}
