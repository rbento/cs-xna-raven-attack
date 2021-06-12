// Copyright (c) Rodrigo Bento

using GameFramework;
using GameFramework.State;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game.Objects.States
{
	public class CannonSpawning : CannonState
	{
		private const float kSpawnTimeout = 1500f;

		private readonly SoundEffect mSpawnSfx;
		private readonly float mSpawnTimeout;

		private bool mPlayedSpawnSfx;

		public CannonSpawning(StateMachine<Cannon> StateMachine)
			: base(StateMachine)
		{
			mSpawnSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seCannonSpawn");
			mSpawnTimeout = TimeManager.TotalMilliseconds + kSpawnTimeout;

			Owner.Image.CurrentFrameIndex = 0;
			Owner.Image.Pause();
		}

		public override void Update(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			if (CurrentTime > mSpawnTimeout)
			{
				StateMachine.ChangeState(new CannonOperational(StateMachine));
				Owner.Image.Color = Color.White;
			}
			else
			{
				if (!mPlayedSpawnSfx)
				{
					mSpawnSfx.Play();
					mPlayedSpawnSfx = true;
				}

				Owner.Image.Color = (Owner.Image.Color == Color.White) ? Color.Red : Color.White;
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
