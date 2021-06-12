// Copyright (c) Rodrigo Bento

using GameFramework.State;

namespace Game.Objects.States
{
	public class CannonState : State<Cannon>
	{
		public CannonState(StateMachine<Cannon> StateMachine)
			: base(StateMachine)
		{
		}
	}
}
