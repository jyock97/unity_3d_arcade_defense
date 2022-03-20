# GDD

## Description

Top-down view game where the Player needs to defend a tower in the middle of the map. There are 3 types of enemies that will spawn around the tower and try to destroy it. Also, the Player can build a defense tower and ground spikes that help defeat the enemies.

## Player Actions

| **Actions**               | **Keys, Buttons**                     |
| ------------------------- | ------------------------------------- |
| UI Actions                |                                       |
| *Select*                  | W, A, S, D, Mouse Hover               |
| *Close Pause Menu*        | Esc                                   |
| *Action*                  | Enter, Mouse Left Click when hovering |
|                           |                                       |
| Gameplay Actions          |                                       |
| *Movement*                | Right Mouse Click                     |
| *Dash*                    | Space                                 |
| *Attack*                  | Left Mouse Click                      |
| *Select Build 1*          | Q                                     |
| *Select Build 2*          | W                                     |
| *Build Selected Building* | Left Mouse Click                      |
|                           |                                       |
| *Open Pause Menu*         | Esc                                   |

## Mechanics

1. Gameplay

   1. Scoreboard

      There is a global scoreboard of all the previous games with the best five scores.

   2. Current Score

      The is a current score for the current play session.

   3. Save

      The game is capable of saving the Scoreboard.

      If the current play session hasn't ended yet. Save the next values in order to recover the play session later on:

      - Enemy types and positions.
      - Current difficulty.
      - All building positions.
      - Nexo life.
      - Player position.

   4. Resume game

      If there is a previous save the session.
      Load that session and resume the gameplay.
   
   5. Difficulty System
   
      The game will happen in rounds. Every end of the round or when a fixed amount of time happens. The next round will start. Every round increases the number of enemy groups to spawn. 
      
   6. Enemy spawns
   
      Every 15 seconds, spawns a group of enemies around the outer circle of the map (Where the player can not reach)The amount of enemies and the type of them is going to be decided by the difficulty system.
   
2. Player

   1. Life

      The Player has a numeric life value.

   2. Damage

      The Player receives damage by a fixed amount, becoming Invulnerable.

   3. Invulnerability

      The Player doesn't receive damage for an amount of time.

   4. Death

      If the Player’s Life gets to 0, the game end.

   5. Money

      By killing enemies, the player gains currency.

      Enemy01 gives 1

      Enemy02 gives 2

      Enemy03 gives 3

   6. Movement

      The Player moves on a plane using pathfinding.

   7. Dash

      The Player becomes Invulnerable, moves with a high speed to the mouse point direction.

   8. Attack

      The Player attacks with a Staff dealing a fixed amount of damage.

   9. Build

      The Player is able to build one building at the Mouse point position if the next conditions happen.

      - There is no collision with another Building, Player of Enemy.

3. Enemies

   1. Enemy01

      A Creature with a fixed amount of Life

      Attack: Jump to the Player dealing damage when touching him.

   2. Enemy02

      A Creature with a fixed amount of Life

      Attack: Enemy shoots a projectile to the last Player position, doing splash damage at that location.

   3. Enemy03

      A Creature with a fixed amount of Life

      Attack: Enemy shoots a projectile to the last Player position, doing splash damage at that location.

4. Buildings

   1. Nexo

      Center building, if the enemies destroy it, game over.

   2. Turret

      Structure with a fixed range. Attacks all enemies in range every fixed amount of seconds.

   3. Ground Spikes

      Structure that doesn't block the path of enemies but makes damage per second.


## VFX

1. Entity Movement (Player, Enemy01, Enemy02, Enemy03)
2. Player Attack
3. Player Damage
4. Enemy01 Attack
5. Enemy02 Attack
6. Enemy03 Attack
7. Build a structure
8. Turret Attack
9. Ground Spikes Damage

## UI

1. Menus
   1. Start Menu
   2. Pause Menu
   3. Game Over Menu
   4. Options Menu
2. GUI
   1. Player Life
   2. Enemies Life
   3. Nexo Life
   4. Enemy Damage
   5. Current Score
   6. Current Money

## Level Design

The map is very simple, with the Nexo at the center. The Player is able to walkway the center by a fixed amount. All enemies spawn randomly outside of the walkable area of the Player.

## Assets

For this project, the next free [3D assets](For this Project, the next free 3D assets will be usedhttps://assetstore.unity.com/packages/templates/tutorials/3d-game-kit-115747 ) will be used.

## Milestones

### M1

* Player
  * Life
  * Damage
  * Invulnerability
  * Death
  * Movement
  * Dash
  * Attack

### M2

* Buildings
  * Nexo
  * Turret
  * Ground Spikes
* Player
  * Money
  * Build

### M3

* Enemy01
* Enemy02
* Enemy03

### M4

* Gameplay

### M5

* VFX

### M6

* Menus

