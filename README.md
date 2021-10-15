# Raven Attack

A 2D Fixed Shooter game demonstrating use of Design Patterns, Math, Physics and Steering Behaviors.

###  Details
---

| Year | Genre         | Players | Toolset             | Language | Dependencies       | Platform |
| ---- | --------------| ------- | ------------------- | -------- | ------------------ | -------- |
| 2011 | Fixed Shooter | 1       | 3ds Max             | C#       | XNA Game Studio    | Win32    |
|      |               |         | Photoshop           |          | [GameFramework][1] |          |
|      |               |         | [SFXR][3]           |          |                    |          |
|      |               |         | Visual Studio 2019  |          |                    |          |

###  Screenshots
---

|  |  |  |
| --- | --- | --- |
| <img src="https://github.com/rbento/cs-xna-raven-attack/blob/main/Screenshots/raven-attack-01.png" width="252" height="160" alt="Gameplay 01" /> | <img src="https://github.com/rbento/cs-xna-raven-attack/blob/main/Screenshots/raven-attack-02.png" width="252" height="160" alt="Gameplay 02" /> | <img src="https://github.com/rbento/cs-xna-raven-attack/blob/main/Screenshots/raven-attack-03.png" width="252" height="160" alt="Gameplay 03" /> |

### Video
---

[Raven Attack on YouTube][4]

###  Remarks
---

- Infinite levels with increasing difficulty.
- Raven movement is powered by steering behaviors (wander, flee, seek) to find the next target position.
- Bombs have random weight and fall with gravity pull.
- Raven and Cannon textures were modeled in 3ds Max and exported, then edited in Photoshop.
- Object-oriented approach with Design Patterns like Singleton, State and Strategy.
- AABB collision detection.

### Updates
---

**2021**

- Solution was updated and is now compatible with Visual Studio 2019.
- XNA Game Studio 4.0 is now a [modified version from FlatRedBall][2].

###  Notice
---

This project is personal, non-commercial and is archived on GitHub.

[1]: https://github.com/rbento/cs-xna-game-framework
[2]: https://flatredball.com/visual-studio-2019-xna-setup
[3]: https://www.drpetter.se/project_sfxr.html
[4]: https://youtu.be/LFwflhdiSIM
