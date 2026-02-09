## Delivery 02 - Stealth Game

### Description

The goal is developing a simple top-down stealth game. There must be a main character moving around while dodging enemies with a vision range and angle. The main character should get from point A to point B to complete the level.

### Features

 - Game start and end logic: 3 scenes
   - Title scene (press ENTER to start) -> NAME: Title
   - Gameplay scene (stealth gameplay logic) -> NAME: Gameplay
   - Ending scene (show best hi-score) -> NAME: Ending
 - Enemy patrolling movement from point A to point B
   - Loop movement, turn 180º when reached target point and return
   - Chase player on detection
   - Return to patrolling if player out of range
 - Enemy vision range (circle area) and angle (vision cone)
   - Blocking elements where player can hide: Walls and columns
   - Alarm with visual elements in case player is detected
   - Draw enemy gizmos and player distance value
   - Draw on-game vision arc shape
 - Minimum 2 enemies with different patterns:
   - Patrolling: Move from point A to point B (mandatory)
   - Waiting: Static position but rotating to look around, needs a exposure time
   - Another proposed pattern
   
#### Additional Features
 - Player total move distance counter and level complete time shown in UI
 - Hi-score system (time to beat level)

### Controls

 - Keyboard:
   - WASD and Arrow Keys for player movement
   - ENTER to start/restart game
   - ESCAPE to exit game and close program
 - Mouse:
   - Click on any button added to the game

### Developers

 - Alejandro Belmonte ([sarkamist](https://github.com/sarkamist/))
 - Pau Bofi ([PauBofi](https://github.com/PauBofi))
 - Luca De Marco ([LukeByMark](https://github.com/LukeByMark))
 - Laia Campos ([Loyan06](https://github.com/Loyan06))
 - Francina Suñer ([r3daveng3r](https://github.com/r3daveng3r))

### License

This game sources are licensed under MIT license. Check [LICENSE](LICENSE) for further details.
