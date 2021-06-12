// Copyright (c) Rodrigo Bento

using GameFramework.Asset;
using GameFramework.Base;
using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace Game.Objects
{
	public class Bomb : GameActor
	{
		private const float kMinMass = 0.3f;
		private const float kMaxMass = 0.5f;

		private const float kGravity = 1.6f;

		private const int kWidth = 7;
		private const int kHeight = 7;

		private GameImage mImage;

		private Vector2 mGravity;

		public Bomb(Vector2 Position)
			: base(kWidth, kHeight, Position)
		{
			mImage = new GameImage("Images/txBomb");
			mGravity = Maths.CreateBySizeAndAngle(kGravity, MathHelper.PiOver2);

			AddComponent<AABBCollision>(new AABBCollision(this));
			AddComponent<Physics>(new Physics(this));

			GetComponent<Physics>().Mass = Randoms.NextFloat(kMinMass, kMaxMass);
		}

		public override void Cleanup()
		{
			base.Cleanup();
			mImage = null;
		}

		public override void Update(GameTime GameTime)
		{
			Physics Physics = GetComponent<Physics>();

			/// Falls considering gravity and random weight.
			Physics.Velocity += (mGravity / Physics.Mass) * TimeManager.DeltaTime;
			Position += Physics.Velocity;

			Angle = Maths.AngleInRadians(Physics.Velocity);

			mImage.Position = Position;
		}

		public override void Draw(GameTime GameTime)
		{
			base.Draw(GameTime);

			mImage.Draw(GameTime);
		}
	}
}
