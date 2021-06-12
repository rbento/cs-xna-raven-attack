// Copyright (c) Rodrigo Bento

using GameFramework.State;

namespace Game.Objects.States
{
	public class CannonDead : CannonState
	{
		public CannonDead(StateMachine<Cannon> StateMachine)
			: base(StateMachine)
		{
			Owner.Image.CurrentFrameIndex = 4;
			Owner.Image.Pause();
		}
	}
}
