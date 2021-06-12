// Copyright (c) Rodrigo Bento

using Game.Objects.States;
using Game.Objects.Strategies;

using GameFramework.Asset;
using GameFramework.Base;
using GameFramework.State;

using Microsoft.Xna.Framework;

namespace Game.Objects
{
	public enum RavenType
	{
		kStriker, kWanderer, kEvader, kNone
	}

	public class Raven : GameActor
	{
		public const float kMinSpeed = 200f;
		public const float kMaxSpeed = 800f;

		private const int kFrameWidth = 52;
		private const int kFrameHeight = 26;
		private const int kFrameCount = 9;
		private const int kFrameDuration = 100;

		public Raven(RavenType Type)
			: base(kFrameWidth, kFrameHeight)
		{
			this.Type = Type;

			AddComponent<AABBCollision>(new AABBCollision(this));
			AddComponent<Physics>(new Physics(this));

			GetComponent<Physics>().Mass = 1.8f;
			GetComponent<Physics>().MaxSpeed = kMinSpeed;

			Image = new AnimatedImage("Images/txRaven", kFrameWidth, kFrameHeight, kFrameCount, kFrameDuration);

			StateMachine = new StateMachine<Raven>(this);
			StateMachine.ChangeState(new RavenIdle(StateMachine));
		}

		public override void Cleanup()
		{
			base.Cleanup();

			Image = null;
		}

		public override void Update(GameTime GameTime)
		{
			StateMachine.Update(GameTime);

			Image.Color = Color;
			Image.Position = Position;
			Image.Update(GameTime);
		}

		public override void Draw(GameTime GameTime)
		{
			base.Draw(GameTime);

			StateMachine.Draw(GameTime);
		}

		public void SpawnAt(Vector2 Position, float Timeout)
		{
			base.Position = Position;
			Anchor = Position;

			StateMachine.ChangeState(new RavenSpawning(StateMachine, Timeout));
		}

		public void Die()
		{
			StateMachine.ChangeState(new RavenDying(StateMachine));
		}

		public bool IsFlying
		{
			get { return StateMachine.State is RavenFlying; }
		}

		public bool IsDying
		{
			get { return StateMachine.State is RavenDying; }
		}

		public bool IsDead
		{
			get { return StateMachine.State is RavenDead; }
		}
		public bool IsSpawning
		{
			get { return StateMachine.State is RavenSpawning; }
		}

		public StateMachine<Raven> StateMachine { get; private set; }

		public FlightStrategy FlightStrategy { get; set; }

		public AnimatedImage Image { get; private set; }

		public RavenType Type { get; set; }

		public Color Color { get; set; } = Color.White;

		public Vector2 Anchor { get; set; }

		public Vector2 RandomTarget { get; set; }

		public Vector2 Target { get; set; }
	}
}
