// Copyright (c) Rodrigo Bento

using Game.Data;

using GameFramework.Asset;
using GameFramework.Effect;
using GameFramework.Manager;
using GameFramework.State;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game.Scenes.States
{
	public class GamePaused : GameState
	{
		private readonly TiledImage mOptionsImage;

		private bool bOptionSelected;

		public GamePaused(StateMachine<PlayScene> StateMachine)
			: base(StateMachine)
		{
			mOptionsImage = new TiledImage("Images/txGamePausedOptions", 106, 73, 3)
			{
				X = 91,
				Y = 465
			};
		}

		public override void OnEnter()
		{
			Option = 0;
			bOptionSelected = false;
		}

		public override void CheckForInput(InputManager Input)
		{
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

		private void SelectOption()
		{
			if (bOptionSelected)
			{
				return;
			}

			bOptionSelected = true;
			SelectSfx.Play();
		}

		public override void Update(GameTime GameTime)
		{
			if (bOptionSelected)
			{
				switch (Option)
				{
					case 0:
						{
							StateMachine.PopState();
						}
						break;

					case 1:
						{
							GameData.IsGameStarting = true;
							StateMachine.PopState();
						}
						break;

					case 2:
						{
							SceneManager.Instance.ChangeScene(SceneType.Menu, EffectDuration.Fast, EffectDuration.Fast);
						}
						break;
				}
			}
		}

		public override void Draw(GameTime GameTime)
		{
			mOptionsImage.CurrentFrameIndex = Option;
			mOptionsImage.Draw(GameTime);
		}
	}
}

