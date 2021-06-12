// Copyright (c) Rodrigo Bento

using GameFramework;
using GameFramework.Manager;
using Game.Scenes;

namespace Game
{
#if WINDOWS || XBOX
	static class Program
	{
		static void Main()
		{
			using (Core Core = Core.Instance)
			{
				Core.AddScene(SceneType.Menu, new MenuScene());
				Core.AddScene(SceneType.Play, new PlayScene());

				Core.Run();
			}
		}
	}
#endif
}

