# MineSweeper
This repository was created to submit my first academy project - Mine Sweeper

## Game Instruction
### Single player
* Click on a tile in the grid, the number on the tile indicates the number of mine around this tile;
* Avoid and successfully flag all the mine from the board to win;

### Multiplayer 
* This is a turn-based game, each player take turn to find the mine;
* The number on the tile indicates the number of mine around the tile;
* Player that has successfully found the mine will earn the chance to move again;
* In multiplayer game mode, player that flag out most of the mine win the game;

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
* Using event handler to handle the event of mine being clicked;

### Class Defintion
* MineSweeper - The abstruct/base class to create the gameboard for either mulitplayer or single player gameboard;
* SP_GameBoard - The Single Player Game Board, is designed to avoid all the mines;
* MP_GameBoard - The Multiplayer Game Board, is designed to see which player first to flagged most of the mines;
* Tile - The abstruct/base class to created the tile for the gameboard either multi or single player;
* SP_Tile - The tile used for single player game, this type of tile also handle the double click(invoke neighbour tiles) and right click(flag);
* MP_Tile - The tile used for multiplaye game, this type of tile only handles the left mouse click(CheckHasObject) and draw a flag when the mine is found; 
* Player - This class store the player id and score;
* GameboardEventArgs - This is a derived class of EventArgs, this event args is passed across when a gameboard event is raised, the arg is a selection of enum (GAME_EVENT);
* GAME_EVENT - This is an enum, a list of game event that can be used to check what type of game event has raised;
* MainWindow - This class is the entry point of the program also to create the window application;

## Reflection
* It was not too difficult to create the main game features and single player game mode, but as the game expanded the program get more complicated;
* Some functions were placed in a class that it should not be, it was not difficult to move them away the class or namespace but remain the connection;
* Spent half of the time to otimize the game and trying to apply ing SOLID principle, still some of the functions cannot follow the this principle;
* The game assests are not otimized, and some of the UI elemnts are not positioning very well;
* However, a fully functional mine sweeper game has been created in a short time and it has two different game mode;
* For project like this in the future that uses WPF, XAML can be used more often and work together with C# to make C# programming easier;
