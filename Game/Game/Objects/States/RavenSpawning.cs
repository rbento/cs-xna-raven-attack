// Copyright (c) Rodrigo Bento

using GameFramework;
using GameFramework.State;
using GameFramework.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game.Objects.States
{
	public class RavenSpawning : RavenState
	{
		public const float kSpawnAnimationSpeed = 20f;
		public const float kSpawnOffset = 750f;
		public const float kSpawnTimeout = 600f;

		private readonly SoundEffect mSpawnSfx;
		private readonly float mSpawnTimeout;

		private bool mPlayedSpawnSfx;
		private float mSpawnOffset;

		public RavenSpawning(StateMachine<Raven> StateMachine, float Timeout)
			: base(StateMachine)
		{
			mSpawnOffset = kSpawnOffset;
			mSpawnSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seRavenSpawn");
			mSpawnTimeout = Timeout;

			Owner.Image.CurrentFrameIndex = Randoms.NextInt(0, 6);
			Owner.Image.Pause();
		}

		public override void Update(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			if (CurrentTime > mSpawnTimeout)
			{
				if (!mPlayedSpawnSfx)
				{
					mSpawnSfx.Play();
					mPlayedSpawnSfx = true;
				}

				/// Decreases the distance offset until it reaches the spawn point.
				mSpawnOffset -= kSpawnAnimationSpeed;

				/// Spawn sequence is finished.
				if (mSpawnOffset <= 0)
				{
					StateMachine.ChangeState(new RavenFlying(StateMachine));
				}
			}
		}

		public override void Draw(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			/// Draw two images meeting at the spawn point.
			if (CurrentTime > mSpawnTimeout)
			{
				Owner.Image.Position = Owner.Position + Maths.CreateBySizeAndAngle(mSpawnOffset, 0);
				Owner.Image.Draw(GameTime);

				Owner.Image.Position = Owner.Position + Maths.CreateBySizeAndAngle(mSpawnOffset, MathHelper.Pi);
				Owner.Image.Draw(GameTime);
			}
		}
	}
}
