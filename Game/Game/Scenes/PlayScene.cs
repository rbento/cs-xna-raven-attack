// Copyright (c) Rodrigo Bento

using Game.Scenes.States;

using GameFramework.Base;
using GameFramework.Diagnostics;
using GameFramework.Manager;
using GameFramework.State;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using System;

namespace Game.Scenes
{
	public class PlayScene : GameScene
	{
		private readonly StateMachine<PlayScene> mStateMachine;

		public PlayScene()
			: base()
		{
			mStateMachine = new StateMachine<PlayScene>(this);
		}

		public override void LoadContent()
		{
			GamePlaying = new GamePlaying(mStateMachine);
			GamePaused = new GamePaused(mStateMachine);
			GameOver = new GameOver(mStateMachine);
		}

		public override void UnloadContent()
		{
		}

		public override void OnEnter()
		{
			mStateMachine.ChangeState(GamePlaying);
		}

		public override void OnExit()
		{
			GC.Collect();
		}

		public override void CheckForInput(InputManager Input)
		{
			if (Input.KeyPressed(Keys.F12))
			{
				DebugDraw.ToggleVisibility();
			}

			mStateMachine.CheckForInput(Input);
		}

		public override void Update(GameTime GameTime)
		{
			mStateMachine.Update(GameTime);
		}

		public override void Draw(GameTime GameTime)
		{
			mStateMachine.Draw(GameTime);
		}

		public GamePlaying GamePlaying { get; private set; }

		public GamePaused GamePaused { get; private set; }

		public GameOver GameOver { get; private set; }
	}
}

