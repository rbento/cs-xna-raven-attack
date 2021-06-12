// Copyright (c) Rodrigo Bento

using Game.Data;
using Game.Objects;
using Game.Objects.States;

using GameFramework;
using GameFramework.Base;
using GameFramework.Manager;
using GameFramework.State;
using GameFramework.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Collections.Generic;

namespace Game.Scenes.States
{
	public class GamePlaying : State<PlayScene>
	{
		private const float kScoreMultiplierTimeout = 4000f;
		private const float kGameStartTimeout = 3200f;
		private const float kCannonSpawnTimeout = 1700f;
		private const float kReadyDisplayTimeout = 1500f;

		private const float kMinBombSpawnTimeout = 3000f;
		private const float kMaxBombSpawnTimeout = 500f;
		public const float kBombSpawnSpan = 100f;
		public const float kMaxBombPositionDiff = 8f;

		private readonly Display mDisplay;
		private readonly Cannon mCannon;
		private readonly Shot mShot;

		private readonly List<Bomb> mBombs;
		private readonly List<Bomb> mDeadBombs;

		private readonly Vector2[,] mAnchors;

		private readonly Song mSong;

		private readonly SoundEffect mCannonExplosionSfx;
		private readonly SoundEffect mCannonShotSfx;
		private readonly SoundEffect mRavenHitSfx;
		private readonly SoundEffect mLifeGainedSfx;
		private readonly SoundEffect mBombDropSfx;
		private readonly SoundEffect mWarningSfx;
		private readonly SoundEffect mMultiplierSfx;

		private Queue<Raven> mWave;
		private Raven[] mRavens;

		private float mBombSpawnTimeout;
		private float mNextBombSpawnTimeout;
		private float mNextMultiplierTimeout;
		private float mReadyDisplayTimeout;
		private float mCannonSpawnTimeout;
		private float mGameStartTimeout;

		private float mRavenSpeed;

		private int mBombAmount;
		private int mWavesDefeated;
		private int mLives;
		private int mScore;
		private int mMultiplier;
		private int mRavenColor;

		private bool bHasCannonSpawned;
		private bool bHasGameStarted;
		private bool bHasWarningAlertBeenPlayed;

		public GamePlaying(StateMachine<PlayScene> StateMachine)
			: base(StateMachine)
		{
			mAnchors = GameData.Anchors;
			mCannon = new Cannon();
			mRavens = new Raven[GameData.kMaxRavens];
			mShot = new Shot(mCannon);
			mWave = new Queue<Raven>();
			mBombs = new List<Bomb>();
			mDeadBombs = new List<Bomb>();

			mDisplay = new Display
			{
				Position = new Vector2(360, 14)
			};

			mSong = Core.Instance.Content.Load<Song>("Audio/soKosmonauts");
			mCannonShotSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seCannonShot");
			mCannonExplosionSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seCannonExplosion");
			mLifeGainedSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seLifeGained");
			mRavenHitSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seRavenHit");
			mBombDropSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seBombDrop");
			mWarningSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seWarning");
			mMultiplierSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seMultiplier");
		}

		public void Reset()
		{
			mRavenColor = 0;
			mRavenSpeed = Raven.kMinSpeed;
			mWavesDefeated = 0;
			mScore = 0;
			mMultiplier = GameData.kMinScoreMultiplier;
			mLives = GameData.kStartingLives;
			mBombAmount = GameData.kStartingBombs;

			ResetCannon();
			ResetShot();
			ResetWave();
			ResetBombs();

			mBombSpawnTimeout = kMinBombSpawnTimeout;
			mNextBombSpawnTimeout = TimeManager.TotalMilliseconds + kMinBombSpawnTimeout;
			mNextMultiplierTimeout = TimeManager.TotalMilliseconds + kScoreMultiplierTimeout;

			bHasGameStarted = false;
			bHasCannonSpawned = false;
		}

		public override void OnEnter()
		{
			if (GameData.IsGameStarting)
			{
				Reset();

				float CurrentTime = TimeManager.TotalMilliseconds;

				mGameStartTimeout = CurrentTime + kGameStartTimeout;
				mCannonSpawnTimeout = CurrentTime + kCannonSpawnTimeout;
				mReadyDisplayTimeout = CurrentTime + kReadyDisplayTimeout;

				GameData.IsGameStarting = false;
			}

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Volume = 0.7f;

			if (MediaPlayer.State == MediaState.Stopped)
			{
				MediaPlayer.Play(mSong);
			}
			else
			{
				MediaPlayer.Resume();
			}
		}

		public override void CheckForInput(InputManager Input)
		{
			if (Input.KeyPressed(Keys.Escape))
			{
				StateMachine.PushState(Owner.GamePaused);
			}

			if (mCannon.IsOperational)
			{
				if (Input.KeyHeld(Keys.Left))
				{
					mCannon.MoveLeft();
				}

				else if (Input.KeyHeld(Keys.Right))
				{
					mCannon.MoveRight();
				}

				if (Input.KeyPressed(Keys.Space))
				{
					if (!mCannon.IsFiring)
					{
						mCannon.IsFiring = true;
						mCannonShotSfx.Play();
					}
				}
			}
		}

		public override void Update(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			DebugInfo();

			UpdateGameState(GameTime);
			UpdateScoreMultiplier(CurrentTime);
			UpdateBombSpawning(CurrentTime);
			UpdateRavens(GameTime);
			UpdateBombs(GameTime);
			UpdateDisplay(GameTime);
		}
		private void UpdateGameState(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			/// Plays the initial sequence when the game is starting.
			UpdateStartSequence(CurrentTime);

			mCannon.Update(GameTime);
			mShot.Update(GameTime);

			/// If the shot went out of bounds (top of the screen), place it back within the cannon.
			if (mShot.Bottom < 0)
			{
				ResetShot();
			}

			/// When at zero lives, plays the warning sound and place the warning text beside the cannon.
			if (mLives == 0)
			{
				if (!bHasWarningAlertBeenPlayed)
				{
					mWarningSfx.Play();
					bHasWarningAlertBeenPlayed = true;
				}

				mDisplay.ShowWarningAt(mCannon.Position);
			}
			else
			{
				bHasWarningAlertBeenPlayed = false;
				mDisplay.HideWarning();
			}

			/// When hit, either
			if (mCannon.IsDead)
			{
				/// Respawn.
				if (mLives >= 0)
				{
					mCannon.Spawn();
				}
				/// Game Over.
				else
				{
					StateMachine.PushState(Owner.GameOver);
				}
			}
		}

		private void UpdateBombs(GameTime GameTime)
		{
			AABBCollision CannonCollision = mCannon.GetComponent<AABBCollision>();

			foreach (Bomb bomb in mBombs)
			{
				bomb.Update(GameTime);

				/// If the bomb went out of bounds (bottom of the screen).
				if (bomb.Top > Core.GraphicsDevice.Viewport.Height)
				{
					mDeadBombs.Add(bomb);
				}
				/// Or else if it has collided with the cannon, destroy it.
				/// Player loses 1 life.
				/// Score multiplier is reset.
				else if (mCannon.IsOperational && CannonCollision.CollidesWith(bomb))
				{
					mCannonExplosionSfx.Play();
					mLives--;
					mCannon.Die();
					mMultiplier = GameData.kMinScoreMultiplier;
					mDeadBombs.Add(bomb);
				}
			}

			/// Cleanup all dead bombs.
			foreach (Bomb bomb in mDeadBombs)
			{
				mBombs.Remove(bomb);
				bomb.Cleanup();
			}

			mDeadBombs.Clear();
		}

		private void UpdateRavens(GameTime GameTime)
		{
			AABBCollision ShotCollision = mShot.GetComponent<AABBCollision>();

			for (int i = 0; i < GameData.kMaxRavens; ++i)
			{
				if (mRavens[i] != null)
				{
					mRavens[i].Target = mCannon.Position;
					mRavens[i].Update(GameTime);

					/// If this raven has collided with the shot...
					/// The raven dies.
					/// Score is increased according to the current multiplier.
					if (mRavens[i].IsFlying && ShotCollision.CollidesWith(mRavens[i]))
					{
						mRavenHitSfx.Play();
						mRavens[i].Die();
						ResetShot();
						mScore += (int)GameData.kScoreBonus * mMultiplier;
					}
					/// If the raven is dead...
					/// Cleanup and spawn more ravens...
					else if (mRavens[i].IsDead)
					{
						mRavens[i].Cleanup();
						mRavens[i] = null;
						SpawnRavens();
					}
				}
			}
		}
		private void UpdateBombSpawning(float CurrentTime)
		{
			if (CurrentTime > mNextBombSpawnTimeout)
			{
				/// Bombs will always come from the raven closest to the cannon.
				for (int i = 0; i < GameData.kMaxRavens; ++i)
				{
					if (mRavens[i] != null && mRavens[i].IsFlying)
					{
						CreateBombs(mRavens[i].Position);
						mBombDropSfx.Play();
					}
				}

				mNextBombSpawnTimeout = CurrentTime + mBombSpawnTimeout;
			}
		}

		private void UpdateScoreMultiplier(float CurrentTime)
		{
			if (CurrentTime > mNextMultiplierTimeout)
			{
				mMultiplier = Maths.Clamp(mMultiplier + 1, GameData.kMinScoreMultiplier, GameData.kMaxScoreMultiplier);
				mNextMultiplierTimeout = CurrentTime + kScoreMultiplierTimeout;
				mMultiplierSfx.Play();
			}
		}

		private void UpdateStartSequence(float CurrentTime)
		{
			if (!bHasGameStarted)
			{
				if (CurrentTime > mGameStartTimeout)
				{
					SpawnRavens();
					mDisplay.HideReady();
					bHasGameStarted = true;
				}
				else
				{
					if (CurrentTime > mReadyDisplayTimeout)
					{
						mDisplay.ShowReady();
					}

					if (!bHasCannonSpawned && CurrentTime > mCannonSpawnTimeout)
					{
						mCannon.Spawn();
						bHasCannonSpawned = true;
					}
				}
			}
		}

		public override void Draw(GameTime GameTime)
		{
			mCannon.Draw(GameTime);
			mShot.Draw(GameTime);

			foreach (Bomb bomb in mBombs)
			{
				bomb.Draw(GameTime);
			}

			foreach (Raven raven in mRavens)
			{
				if (raven != null)
				{
					raven.Draw(GameTime);
				}
			}

			mDisplay.Draw(GameTime);

			/// Debug Draw: Anchor points.
			for (int i = 0; i < 3; ++i)
				for (int j = 0; j < 3; ++j)
					DebugDraw.DrawPoint(mAnchors[i, j], Color.White);
		}

		private void UpdateDisplay(GameTime GameTime)
		{
			mDisplay.Lives = mLives;
			mDisplay.Multiplier = mMultiplier;
			mDisplay.Score = mScore;
			mDisplay.Wave = mWavesDefeated + 1;
			mDisplay.Update(GameTime);
		}

		private void ResetBombs()
		{
			if (mBombs.Count == 0)
			{
				return;
			}

			Bomb[] Bombs = mBombs.ToArray();

			for (int i = 0; i < Bombs.Length; ++i)
			{
				Bombs[i] = null;
			}

			mBombs.Clear();
		}

		private void ResetWave()
		{
			while (mWave.Count > 0)
			{
				mWave.Dequeue();
			}

			mWave.Clear();

			mRavens = new Raven[GameData.kMaxRavens];
			mWave = GameData.CreateWave();
		}

		private void ResetCannon()
		{
			mCannon.Reset();
			mCannon.X = 360;
			mCannon.Y = 526;
		}

		private void ResetShot()
		{
			mCannon.IsFiring = false;
		}

		private void CreateBombs(Vector2 RavenPosition)
		{
			if (mBombs.Count == 0)
			{
				int Counter = 0;
				while (Counter < mBombAmount)
				{
					float BombPositionDiff = Randoms.NextFloat(-kMaxBombPositionDiff, kMaxBombPositionDiff);
					Vector2 BombPosition = new Vector2(RavenPosition.X + BombPositionDiff, RavenPosition.Y);
					mBombs.Add(new Bomb(BombPosition));
					Counter++;
				}
			}
		}

		private void SpawnRavens()
		{
			/// When an enemy wave is defeated.
			/// A new wave is created.
			/// Player gains a life.
			/// Raven color is changed and their speed is increased.
			/// The bomb amount is increased and the time between attacks is decreased.
			if (WaveWasDefeated())
			{
				mWave = GameData.CreateWave();
				mLives = Maths.Clamp(mLives + 1, 0, GameData.kMaxLives);
				mBombAmount = Maths.Clamp(mBombAmount + 1, GameData.kStartingBombs, GameData.kMaxBombs);
				mBombSpawnTimeout = Maths.Clamp(mBombSpawnTimeout - kBombSpawnSpan, kMaxBombSpawnTimeout, kMinBombSpawnTimeout);
				mRavenSpeed = Maths.Clamp(mRavenSpeed + GameData.kRavenSpeedIncrement, Raven.kMinSpeed, Raven.kMaxSpeed);
				mRavenColor = Maths.Wrap(mRavenColor + 1, 0, GameData.kMaxColorIndex);
				mWavesDefeated++;
				mLifeGainedSfx.Play();
			}

			/// Spawns an enqueued raven (current wave).
			if (mWave.Count > 0)
			{
				float SpawnTimeout = TimeManager.TotalMilliseconds;

				for (int i = 0; i < GameData.kMaxRavens; ++i)
				{
					if (mRavens[i] == null)
					{
						SpawnTimeout += RavenSpawning.kSpawnTimeout;

						mRavens[i] = mWave.Dequeue();
						mRavens[i].Type = (RavenType)i;
						mRavens[i].Color = GameData.GetColor(mRavenColor);
						mRavens[i].SpawnAt(mAnchors[i, Randoms.NextInt(0, 3)], SpawnTimeout);
						mRavens[i].GetComponent<Physics>().MaxSpeed = mRavenSpeed;
					}
				}
			}
		}

		private bool WaveWasDefeated()
		{
			return (mWave.Count == 0
				&& mRavens[0] == null
				&& mRavens[1] == null
				&& mRavens[2] == null);
		}

		private void DebugInfo()
		{
			int WaveCount = mWave.Count;
			int BombCount = mBombs.Count;

			Debugger.AddToLeftViewport("Waves Defeated", mWavesDefeated);
			Debugger.AddToLeftViewport("Wave Count", WaveCount);
			Debugger.AddToLeftViewport("Bomb Count", BombCount);
			Debugger.AddToLeftViewport("Bomb Amount", mBombAmount);
			Debugger.AddToLeftViewport("Bomb Timeout", mBombSpawnTimeout);
			Debugger.AddToLeftViewport("Next Bombs At", mNextBombSpawnTimeout);
			Debugger.AddToLeftViewport("Lives", mLives);
			Debugger.AddToLeftViewport("Score", mScore);
			Debugger.AddToLeftViewport("Multiplier", mMultiplier);
			Debugger.AddToLeftViewport("Raven Speed", mRavenSpeed);
		}
	}
}
