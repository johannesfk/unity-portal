# PI3DW Mini-Project - Portal

## Overview

I’ve always been fascinated by the game mechanics in the 2007 game Portal by Valve, so I’ve tried to make some parts of it in Unity. The player can shoot an orange and blue portal unto portalable surfaces. They can make physics objects go through the portals. They can also look through the portal to see what is on the other side. The player is also able to go through the portal and warp to the other portal.
I’ve focused on the mechanics and didn’t have time to add a player goal or completion state.

- Player - A first person controller with a camera attached controlled with WASD and the mouse.
- Portals - A pair of portals placed by the player, on portalable surfaces in the world.
- Objects - Physics objects that the player can push around and into portals.
- Level - A playing area built of simple shapes forming corners, corridors, and areas otherwise inaccessible without portals.

The portals work is by putting a camera at the same positional offset to the portal you are looking at, cloned at the other portal. This way it creates the same perspective.

## Project parts

### Scripts

- CameraMove - Used to move and rotate the camera and player controller.
- PlayerController - Provides behaviour to the player when going through portals.
- Crosshair - Shows what portals have been shot and indicates the center of the screen.
- PortalPair - Management of the amount of portals.
- PortalPlacement - Logic for where and how portals are placed.
- Portal - Logic for placing the portal correctly and behaviour for when an object enters the portal.
- PortalCamera - The custom render pipeline that creates the portal illusion.
- PortalableObject - Behaviour and logic for objects that can interact with portals.

## Time spent

| Task                                               | Time in hours |
| -------------------------------------------------- | ------------- |
| Setting up Unity, making GitHub project            | 0.5           |
| Research of game idea                              | 4             |
| Making camera movement controls, initial testing   | 2             |
| Player movement                                    | 2             |
| Portal placement                                   | 3             |
| Portal viewpoint                                   | 4             |
| Warping objects through portals                    | 3             |
| Warping player through portals                     | 1             |
| Bugfixing physics and collision of physics objects | 3             |
| Making readme                                      | 0.5           |
| Code documentation                                 | 0.5           |
| **Total**                                          | **23.5**      |

## References

- [How were the portals in Portal created? - DigiDigger](https://www.youtube.com/watch?v=_SmPR5mvH7w)
- [Fully Functional Portals in Unity URP - Daniel Ilett](https://youtu.be/PkGjYig8avo)
- Materials from Quixel
