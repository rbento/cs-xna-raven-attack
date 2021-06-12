// Copyright (c) Rodrigo Bento

using GameFramework.State;

namespace Game.Objects.States
{
	public class RavenIdle : RavenState
	{
		public RavenIdle(StateMachine<Raven> StateMachine)
			: base(StateMachine)
		{
		}
	}
}
