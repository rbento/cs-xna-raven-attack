// Copyright (c) Rodrigo Bento

using GameFramework;
using GameFramework.Manager;
using GameFramework.State;

using Microsoft.Xna.Framework.Audio;

namespace Game.Scenes.States
{
	public class GameState : State<PlayScene>
	{
		protected float kTransitionDuration = 400;

		public GameState(StateMachine<PlayScene> StateMachine)
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

		protected float TransitionTimeout { get; private set; }

		protected int Option { get; set; }
	}
}
