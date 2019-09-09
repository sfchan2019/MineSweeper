# MineSweeper
This repository was created to submit my Mine Sweeper game

## Day 1
### General Implementation
* Invoke surrounding tiles using double click;
* Invoke a single tile using left click;
* Flag and unflag a tile using right click;
* Programmatically generate gameboard (grid);
* Programmatically generate menu;
* Randomly place mines to the gameboard;
* Different difficulty level (Easy, Normal, Difficult);
* Create Menu and Transition between menu and game;
* End game conditions - On clicked bomb, or On Finished Every Tile;
* Multiplayer Idea - Each player take turn to play, switch player turn if clicked on a bomb; The player with more discovered tiles win;
### Class Defintion
* Player - hold the player score and id;
* Board - manage most of the game object, and the gameboard;
* Tile - hold most of the function for tile i.e. Check tile, change tile image etc.;
* PlayerHUD - hold and display the player id and score;
* Menu - Create the menu programmatically and transit between the game;
