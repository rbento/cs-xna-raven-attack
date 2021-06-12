// Copyright (c) Rodrigo Bento

using GameFramework;
using GameFramework.State;

using Microsoft.Xna.Framework.Audio;

namespace Game.Scenes.States
{
	public class MenuState : State<MenuScene>
	{
		protected float kTransitionDuration = 400;

		public MenuState(StateMachine<MenuScene> StateMachine)
			: base(StateMachine)
		{
			OptionSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seOption");
			SelectSfx = Core.Instance.Content.Load<SoundEffect>("Audio/seSelect");
		}

		public override void OnEnter()
		{
			TransitionTimeout = TimeManager.TotalMilliseconds + kTransitionDuration;
			TransitioningIn = true;
		}

		protected bool IsInTransition
		{
			get { return (TransitioningIn || TransitioningOut); }
		}

		protected SoundEffect OptionSfx { get; private set; }
		protected SoundEffect SelectSfx { get; private set; }

		protected bool TransitioningIn { get; set; }
		protected bool TransitioningOut { get; set; }

		protected float TransitionTimeout { get; set; }

		public int Option { get; set; }
	}
}
