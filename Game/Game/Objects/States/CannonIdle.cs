// Copyright (c) Rodrigo Bento

using GameFramework.State;

namespace Game.Objects.States
{
	public class CannonIdle : CannonState
	{
		public CannonIdle(StateMachine<Cannon> StateMachine)
			: base(StateMachine)
		{
		}
	}
}
