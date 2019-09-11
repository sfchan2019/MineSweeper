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

## Day 2
### General Implemetation
* Created Turn-based multiplayer feature;
* Player that flagged more than half of the mines win the game;
* Seperated Menu to a new project as UI;
* Seperated Single Player feature to a new project as Single Player Mine Sweeper;
* Created HUD to show the score of the player and the win condition, but need to polish;
* Took away the double click - Invoke surrounding tiles feature;
* Changed the mine image to flag image, so bonus can be given to player if they spot a mine;
* Created event handler for CollectObject (Finding the bome);

### Things to do
* Polish the UIs;
* Need to seperate HUD class from Multiplayer project;
* Need to initialize UI outside the Board class;
* Expose some functions to MainWindow class so it can be modify by the user (May use event handler);

### Things may need to do
* Add single player feature back to the game;

## Day3
### Implementation
* Added single player feature back to the game;
* Seperated UI elements from the game namespace, placed in UserInterface namespace;
* Created an abstruct board class for single player and multiplayer game board;
* Created an abstruct tile class for single player and multiplayer game tile;
* Relocated some of the classes, the Tile hierachy and Board hierachy are now in the game namespace;
* Using event handler to handle the clicked mine event;
