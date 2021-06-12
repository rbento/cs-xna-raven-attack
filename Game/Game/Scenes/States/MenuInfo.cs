// Copyright (c) Rodrigo Bento

using GameFramework.Asset;
using GameFramework.Effect;
using GameFramework.Manager;
using GameFramework.State;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Scenes.States
{
	public class MenuInfo : MenuState
	{
		private readonly GameImage mMenuInfoImage;

		public MenuInfo(StateMachine<MenuScene> StateMachine)
			: base(StateMachine)
		{
			mMenuInfoImage = new GameImage("Images/txMenuInfo", 360, 270);
		}

		public override void OnEnter()
		{
			base.OnEnter();

			mMenuInfoImage.Hide();
			mMenuInfoImage.FadeIn(EffectDuration.Fast, EffectDuration.Fast);
		}

		public override void CheckForInput(InputManager Input)
		{
			if (IsInTransition) return;

			if (Input.KeyPressed(Keys.Escape) || Input.KeyPressed(Keys.Enter))
			{
				GoBack();
			}
		}

		public override void Update(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			if (TransitioningIn && CurrentTime > TransitionTimeout)
			{
				TransitioningIn = false;
			}

			else if (TransitioningOut && CurrentTime > TransitionTimeout && mMenuInfoImage.IsDoneFading())
			{
				TransitioningOut = false;
				StateMachine.ChangeState(Owner.MenuIdle);
			}
		}

		public override void Draw(GameTime GameTime)
		{
			mMenuInfoImage.Draw(GameTime);
		}

		private void GoBack()
		{
			mMenuInfoImage.FadeOut(EffectDuration.Fast, EffectDuration.Fast);
			TransitionTimeout = TimeManager.TotalMilliseconds + kTransitionDuration;
			TransitioningOut = true;
		}
	}
}

