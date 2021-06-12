// Copyright (c) Rodrigo Bento

using Game.Objects.States;

using GameFramework.Asset;
using GameFramework.Base;
using GameFramework.State;
using GameFramework.Util;

using Microsoft.Xna.Framework;

namespace Game.Objects
{
	public class Cannon : GameActor
	{
		private const int kFrameCount = 5;
		private const int kFrameDuration = 130;
		private const int kFrameHeight = 19;
		private const int kFrameWidth = 24;

		private readonly StateMachine<Cannon> mStateMachine;

		private const float kSpeed = 4.75f;

		public Cannon()
			: base(kFrameWidth, kFrameHeight)
		{
			mStateMachine = new StateMachine<Cannon>(this);
			Image = new AnimatedImage("Images/txCannon", kFrameWidth, kFrameHeight, kFrameCount, kFrameDuration);

			AddComponent<AABBCollision>(new AABBCollision(this));
			AddComponent<Physics>(new Physics(this));

			Reset();
		}

		public override void Reset()
		{
			mStateMachine.ChangeState(new CannonIdle(mStateMachine));
		}

		public override void Cleanup()
		{
			base.Cleanup();

			Image = null;
		}

		public override void Update(GameTime GameTime)
		{
			mStateMachine.Update(GameTime);
		}

		public override void Draw(GameTime GameTime)
		{
			base.Draw(GameTime);

			mStateMachine.Draw(GameTime);
		}

		public void MoveLeft()
		{
			X = Maths.Clamp(X - kSpeed, 17, 703);
		}

		public void MoveRight()
		{
			X = Maths.Clamp(X + kSpeed, 17, 703);
		}

		public void Spawn()
		{
			mStateMachine.ChangeState(new CannonSpawning(mStateMachine));
		}

		public void Die()
		{
			mStateMachine.ChangeState(new CannonDying(mStateMachine));
		}

		public bool IsOperational
		{
			get { return mStateMachine.State is CannonOperational; }
		}

		public bool IsSpawning
		{
			get { return mStateMachine.State is CannonSpawning; }
		}

		public bool IsDying
		{
			get { return mStateMachine.State is CannonDying; }
		}

		public bool IsDead
		{
			get { return mStateMachine.State is CannonDead; }
		}

		public AnimatedImage Image { get; private set; }

		public bool IsFiring { get; set; }
	}
}
