// Copyright (c) Rodrigo Bento

using Game.Scenes.States;

using GameFramework.Asset;
using GameFramework.Base;
using GameFramework.Effect;
using GameFramework.Manager;
using GameFramework.State;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace Game.Scenes
{
	public class MenuScene : GameScene
	{
		private readonly StateMachine<MenuScene> mStateMachine;

		private GameImage mBackgroundImage;

		public MenuScene()
			: base()
		{
			mStateMachine = new StateMachine<MenuScene>(this);
		}

		public override void LoadContent()
		{
			mBackgroundImage = new GameImage("Images/txMenuBackground", 360, 270);

			MenuIdle = new MenuIdle(mStateMachine);
			MenuInfo = new MenuInfo(mStateMachine);
		}

		public override void UnloadContent()
		{
			mBackgroundImage = null;
		}

		public override void OnEnter()
		{
			mBackgroundImage.Hide();
			mBackgroundImage.FadeIn(EffectDuration.Fast, EffectDuration.Fast);

			mStateMachine.ChangeState(MenuIdle);
		}

		public override void OnExit()
		{
			MediaPlayer.Stop();
		}

		public override void CheckForInput(InputManager Input)
		{
			mStateMachine.State.CheckForInput(Input);
		}

		public override void Update(GameTime GameTime)
		{
			mStateMachine.Update(GameTime);
		}

		public override void Draw(GameTime GameTime)
		{
			mBackgroundImage.Draw(GameTime);
			mStateMachine.State.Draw(GameTime);
		}

		public MenuIdle MenuIdle { get; private set; }

		public MenuInfo MenuInfo { get; private set; }
	}
}



