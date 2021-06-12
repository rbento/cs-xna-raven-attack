// Copyright (c) Rodrigo Bento

using GameFramework.State;

namespace Game.Objects.States
{
	public class RavenState : State<Raven>
	{
		public RavenState(StateMachine<Raven> StateMachine)
			: base(StateMachine)
		{
		}
	}
}
