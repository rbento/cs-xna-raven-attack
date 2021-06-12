// Copyright (c) Rodrigo Bento

using Game.Data;

using GameFramework;
using GameFramework.Asset;
using GameFramework.Effect;
using GameFramework.Manager;
using GameFramework.State;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Scenes.States
{
	public class MenuIdle : MenuState
	{
		private const float kInfoTransitionDuration = 1000;

		private readonly TiledImage mMenuOptionsImage;

		public MenuIdle(StateMachine<MenuScene> StateMachine)
			: base(StateMachine)
		{
			mMenuOptionsImage = new TiledImage("Images/txMenuOptions", 106, 73, 3)
			{
				X = 91,
				Y = 465
			};
		}

		public override void OnEnter()
		{
			base.OnEnter();

			mMenuOptionsImage.Hide();
			mMenuOptionsImage.FadeIn(EffectDuration.Fast, EffectDuration.Fast);
		}

		public override void CheckForInput(InputManager Input)
		{
			if (IsInTransition) return;

			if (Input.KeyPressed(Keys.Escape))
			{
				Core.Exit();
			}

			if (Input.KeyPressed(Keys.Enter))
			{
				SelectOption();
			}

			if (Input.KeyPressed(Keys.Up))
			{
				OptionSfx.Play();
				Option--;
			}

			else if (Input.KeyPressed(Keys.Down))
			{
				OptionSfx.Play();
				Option++;
			}

			Option = (Option < 0) ? 2 : (Option > 2) ? 0 : Option;
		}

		public override void Update(GameTime GameTime)
		{
			float CurrentTime = TimeManager.TotalMilliseconds;

			if (TransitioningIn && CurrentTime > TransitionTimeout)
			{
				TransitioningIn = false;
			}

			else if (TransitioningOut && CurrentTime > TransitionTimeout)
			{
				TransitioningOut = false;

				switch (Option)
				{
					case 0:
						{
							GameData.IsGameStarting = true;
							SceneManager.Instance.ChangeScene(SceneType.Play, EffectDuration.Fast);
						}
						break;

					case 1: { StateMachine.ChangeState(Owner.MenuInfo); } break;
					case 2: { Core.Exit(); } break;
				}
			}
		}

		public override void Draw(GameTime GameTime)
		{
			if (mMenuOptionsImage == null)
			{
				return;
			}

			mMenuOptionsImage.CurrentFrameIndex = Option;
			mMenuOptionsImage.Draw(GameTime);
		}

		private void SelectOption()
		{
			if (IsInTransition) return;

			SelectSfx.Play();
			mMenuOptionsImage.FadeOut(EffectDuration.Fast, EffectDuration.Fast);
			TransitionTimeout = TimeManager.TotalMilliseconds + ((Option == 1) ? kInfoTransitionDuration : kTransitionDuration);
			TransitioningOut = true;
		}
	}
}
