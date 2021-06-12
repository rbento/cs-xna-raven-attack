// Copyright (c) Rodrigo Bento

using Game.Objects;

using GameFramework.Util;

using Microsoft.Xna.Framework;

using System.Collections.Generic;

namespace Game.Data
{
	public sealed class GameData
	{
		public const int kStartingLives = 3;
		public const int kStartingBombs = 3;

		public const int kMaxLives = 99;
		public const int kMaxBombs = 49;
		public const int kMaxRavens = 3;

		public const int kMinScoreMultiplier = 1;
		public const int kMaxScoreMultiplier = 99;

		public const float kScoreBonus = 100;

		public const float kRavenSpeedIncrement = 5f;

		public const int kMaxRavenCount = 8;
		public const int kMaxColors = 10;
		public const int kMaxColorIndex = kMaxColors - 1;

		private const int kMaxAnchorRows = 3;
		private const int kMaxAnchorCols = 3;

		private const int kAnchorInitialX = 120;
		private const int kAnchorInitialY = 280;

		private const int kAnchorGapX = 240;
		private const int kAnchorGapY = 100;

		public static Vector2[,] Anchors { get; private set; } = CreateAnchors();
		public static Color[] Colors { get; private set; } = CreateColors();

		private GameData()
		{
		}

		private static Vector2[,] CreateAnchors()
		{
			Vector2[,] Anchors = new Vector2[3, 3];

			int X = kAnchorInitialX;
			int Y = kAnchorInitialY;

			for (int Row = 0; Row < kMaxAnchorRows; ++Row)
			{
				for (int Col = 0; Col < kMaxAnchorCols; ++Col)
				{
					Anchors[Row, Col] = new Vector2(X, Y);
					X += kAnchorGapX;
				}

				X = kAnchorInitialX;
				Y -= kAnchorGapY;
			}

			return Anchors;
		}

		private static Color[] CreateColors()
		{
			Color[] Colors = new Color[kMaxColors];

			Colors[0] = Color.White;
			Colors[1] = Color.Wheat;
			Colors[2] = Color.Aquamarine;
			Colors[3] = Color.Goldenrod;
			Colors[4] = Color.DarkGray;
			Colors[5] = Color.LightBlue;
			Colors[6] = Color.LightSeaGreen;
			Colors[7] = Color.LightSalmon;
			Colors[8] = Color.Orange;
			Colors[9] = Color.Crimson;

			return Colors;
		}

		public static Vector2 GetRandomAnchorByRavenType(RavenType Type)
		{
			int Row = (int)Type;
			int Col = Randoms.NextInt(0, kMaxAnchorCols);

			return Anchors[Row, Col];
		}

		public static Color GetColor(int Index)
		{
			if (Index < 0 || Index >= kMaxColors)
			{
				return Color.Black;
			}

			return Colors[Index];
		}

		public static Queue<Raven> CreateWave()
		{
			Queue<Raven> Wave = new Queue<Raven>();

			int Counter = 0;

			while (Counter < kMaxRavenCount)
			{
				Wave.Enqueue(new Raven(RavenType.kNone));
				Counter++;
			}

			return Wave;
		}

		public static bool IsGameStarting { get; set; }
	}
}

