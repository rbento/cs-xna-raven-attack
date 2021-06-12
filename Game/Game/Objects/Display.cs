// Copyright (c) Rodrigo Bento

using Game.Data;

using GameFramework;
using GameFramework.Manager;
using GameFramework.Asset;
using GameFramework.Util;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Objects
{
	public class Display
	{
		private const float kPulseTimeout = 250;

		private readonly string mReadyText = "ready?";
		private readonly string mWarningText = "warning";

		private SpriteFont mSpriteFont;
		private GameImage mDisplayImage;

		private Vector2 mLivesOffset;
		private Vector2 mMultiplierOffset;
		private Vector2 mScoreOffset;
		private Vector2 mWaveOffset;
		private Vector2 mWarningOffset;

		private Vector2 mReadyPosition;

		private Vector2 mWarningPosition;
		private Color mWarningColor;

		private int mLives;
		private int mMultiplier;
		private int mScore;
		private int mWave;

		private float mNextWarningPulseTimeout;

		private bool bShowReady;
		private bool bShowWarning;

		public Display()
		{
			mDisplayImage = new GameImage("Images/txDisplay");

			mSpriteFont = Core.Instance.Content.Load<SpriteFont>("Fonts/sfAccess");

			mReadyPosition = new Vector2(360 - (mSpriteFont.MeasureString(mReadyText).X / 2), 270);

			mLivesOffset = new Vector2(-308, -4);
			mWaveOffset = new Vector2(-208, -4);
			mMultiplierOffset = new Vector2(-58, -4);
			mScoreOffset = new Vector2(40, -4);
			mWarningOffset = new Vector2(24, -12);
		}

		public void Cleanup()
		{
			mSpriteFont = null;
			mDisplayImage = null;
		}

		public void Update(GameTime GameTime)
		{
			float CurrentTime = TimeManager.Instance.TotalMilliseconds;

			if (bShowWarning && CurrentTime > mNextWarningPulseTimeout)
			{
				mWarningColor = (mWarningColor == Color.Yellow) ? Color.OrangeRed : Color.Yellow;

				mNextWarningPulseTimeout = CurrentTime + kPulseTimeout;
			}

			mDisplayImage.Position = Position;
		}

		public void Draw(GameTime gameTime)
		{
			mDisplayImage.Draw(gameTime);

			Core.SpriteBatch.DrawString(mSpriteFont, GetStringFormat(mLives), Position + mLivesOffset, Color.Orange);
			Core.SpriteBatch.DrawString(mSpriteFont, GetStringFormat(mWave), Position + mWaveOffset, Color.Orange);
			Core.SpriteBatch.DrawString(mSpriteFont, GetStringFormat(mMultiplier), Position + mMultiplierOffset, Color.Orange);
			Core.SpriteBatch.DrawString(mSpriteFont, GetStringFormat(mScore), Position + mScoreOffset, Color.Orange);

			if (bShowReady)
			{
				Core.SpriteBatch.DrawString(mSpriteFont, mReadyText, mReadyPosition, Color.Orange);
			}

			if (bShowWarning)
			{
				Core.SpriteBatch.DrawString(mSpriteFont, mWarningText, mWarningPosition, mWarningColor);
			}
		}

		private string GetStringFormat(int Value)
		{
			return $"{Value}";
		}

		public void ShowReady()
		{
			bShowReady = true;
		}

		public void HideReady()
		{
			bShowReady = false;
		}

		public void ShowWarningAt(Vector2 position)
		{
			mWarningPosition = position + mWarningOffset;
			bShowWarning = true;
		}

		public void HideWarning()
		{
			bShowWarning = false;
		}

		public int Lives
		{
			get { return mLives; }
			set { mLives = Maths.Clamp(value, 0, GameData.kMaxLives); }
		}

		public int Multiplier
		{
			get { return mMultiplier; }
			set { mMultiplier = Maths.Clamp(value, GameData.kMinScoreMultiplier, GameData.kMaxScoreMultiplier); }
		}

		public int Score
		{
			get { return mScore; }
			set { mScore = Maths.Clamp(value, 0, int.MaxValue); }
		}

		public int Wave
		{
			get { return mWave; }
			set { mWave = Maths.Clamp(value, 0, int.MaxValue); }
		}

		public Vector2 Position { get; set; }
	}
}

