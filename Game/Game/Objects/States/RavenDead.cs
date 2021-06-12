// Copyright (c) Rodrigo Bento

using GameFramework.State;

namespace Game.Objects.States
{
	public class RavenDead : RavenState
	{
		public RavenDead(StateMachine<Raven> StateMachine)
			: base(StateMachine)
		{
			Owner.Image.CurrentFrameIndex = 8;
			Owner.Image.Pause();
		}
	}
}
