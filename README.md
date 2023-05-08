# A C# Unity Procedural Generation Project ManEXE

This project is a computer game using the Unity 3D platform. The game uses a procedural generation mechanism utilizing the marching cubes algorithm.  

## Plan for the game

Description of main game loop.

In game you have ability to explore levels of 2 types. First type is large open level in boundaries of which player can freely move in all directions. Second type is an underground dungeon with intertwined corridors and rooms.

Levels can change during the game (for example some assets can be destroyed).

Player can walk, run, jump, climb slopes, attack, use active skills, use potions, craft or interact with items.

Player can meet enemies during his exploration of level and then he will have to fight. Player can have multiple types of weapons, armor, or other equipment that can help him defeat his foes. Other than that player has active and passive skills that he can use to improve his advantage.

Enemies can be of many types such as humanoids (humans, undead), quadrupeds (wolves, bears), arachnids (giant spiders).

Player and enemies can also interact with the surroundings to their advantage.
Player can take items from defeated enemies for later use.

There are multiple ways to finish the level depending on objectives. If level is finished successfully then player will be taken to map menu where his next destination can be decided. Map screen will reveal to the player the part of procedurally generated map that he is aware of. Depending on the methods used to finish the level options for next one can change.

If player had died during the level he will lose one of his lives and his items.  Then new level will be generated. If amount of lives reaches 0 then player will lose most of his progress and world will be generated anew.
