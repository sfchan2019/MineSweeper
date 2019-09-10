using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;



namespace MultiplayerGame
{
    public class Tile
    {
        System.Windows.Controls.Button button;
        Board gameBoard;
        int row;
        int column;
        int id;
        bool hasMine;
        bool isFinish;

        public bool HasMine { get { return hasMine; } }
        public bool IsFinish { get { return isFinish; } }

        public Tile(int row, int column, Board gameBoard)
        {
            this.column = column;
            this.row = row;
            this.gameBoard = gameBoard;
            this.id = column + row * gameBoard.Column;
            this.isFinish = false;
            button = new Button();
            hasMine = false;
            button.Click += OnLeftClickTile;
            button.MouseDoubleClick += OnDoubleClick;
            button.MouseRightButtonDown += OnRightClickTile;
            button.Background = Brushes.SkyBlue;
            System.Windows.Controls.Grid.SetColumn(this.button, column);
            System.Windows.Controls.Grid.SetRow(this.button, row);
            gameBoard.GameBoard.Children.Add(button);
        }

        public void OnRightClickTile(object sender, RoutedEventArgs e)
        {
            //FlagTile();
        }

        public void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (IsFinish)
            //    return;
            //if(e.ChangedButton == MouseButton.Left)
            //InvokeNeighbourTiles(AllNeighbourNumber(this.id));
        }

        public void FlagTile()
        {
            if (!isFinish)
            {
                SetTileImage("F");
                gameBoard.FinishCount++;
            }
            else
            {
                SetTileImage("U");
                gameBoard.FinishCount--;
            }
            isFinish = !isFinish;
        }

        public void OnLeftClickTile(object sender, RoutedEventArgs e)
        {
            if (isFinish)
                return;
            if (!CheckHasObject())
                gameBoard.SwitchPlayerTurn();
        }

        public bool CheckHasObject()
        {
            if (isFinish)
                return false;
            else
            {
                this.isFinish = true;
                if (this.hasMine)
                {
                    //Fire Event -- Score , change image, return true;
                    //gameBoard.Players[gameBoard.Turn].Score++;
                    //button.Content = new Image() { Source = gameBoard.MineImage };
                    //gameBoard.RaiseEvent(new GameboardEventArgs());
                    CollectObject();
                    return true;
                }
                else
                {
                    List<int> numbers = GetNeighbourIndecies(this.id);
                    int count = CountObjFromGroup(numbers);
                    if (count == 0)
                    {
                        InvokeGroupOfTile(numbers);
                    }
                    else
                    {
                        SetTileImage(count.ToString());
                        return false;
                    }
                }
            }
            return false;
        }

        public void CollectObject()
        {
            //Fire Event -- Score , change image, return true;
            gameBoard.Players[gameBoard.Turn].Score++;
            button.Content = new Image() { Source = gameBoard.MineImage };
            gameBoard.Mine--;
            if(gameBoard.Mine <= 0)
                gameBoard.RaiseEvent(new GameboardEventArgs());
        }

        public void InvokeGroupOfTile(List<int> numbers)
        {
            this.button.IsEnabled = false;
            foreach (int i in numbers)
            {
                Tile temp = gameBoard.Tiles[i];
                temp.CheckHasObject();
            }
        }

        public int CountObjFromGroup(List<int> numbers)
        {
            int count = 0;

            foreach (int i in numbers)  //Count number of mine
            {
                Tile temp = gameBoard.Tiles[i];
                if (temp.hasMine)
                    count++;
            }
            return count;
        }

        List<int> GetNeighbourIndecies(int i)
        {
            int x = i % this.gameBoard.Column;
            int y = i / this.gameBoard.Column;

            List<int> numbers = new List<int>();

            if (x != 0)                             //if not on the left column
                numbers.Add(i - 1);
            if (x != this.gameBoard.Column - 1)                    //if not on the right column
                numbers.Add(i + 1);
            if (y != 0)                             //if not on the top row
                numbers.Add(i - this.gameBoard.Column);
            if (y != 0 && x != 0)                   //if not on the top row AND not on the left column
                numbers.Add(i - this.gameBoard.Column - 1);
            if (y != 0 && x != this.gameBoard.Column - 1)          //if not on the top row AND not on the right column
                numbers.Add(i - this.gameBoard.Column + 1);
            if (y != this.gameBoard.Row - 1)                       //if not on the button row
                numbers.Add(i + this.gameBoard.Column);
            if (y != this.gameBoard.Row - 1 && x != this.gameBoard.Column - 1)    //if not on the bottm row AND not on the right column
                numbers.Add(i + this.gameBoard.Column + 1);
            if (y != this.gameBoard.Row - 1 && x != 0)             //if not on the bottom row AND not on the left column
                numbers.Add(i + this.gameBoard.Column - 1);

            return numbers;
        }

        private int CountNearMine()
        {
            List<int> numbers = GetNeighbourIndecies(this.id);
            int count = 0;

            foreach (int i in numbers)
            {
                Tile temp = gameBoard.Tiles[i];
                if (temp.hasMine)
                    count++;
            }
            //if (count == 0) //if no mine is around, automatically check all the neighbours.
            //    InvokeNeighbourTiles(numbers);
            return count;
        }

        private void InvokeNeighbourTiles(List<int> numbers)
        {
            foreach (int i in numbers)
            {
                Tile temp = gameBoard.Tiles[i];
                if (temp.hasMine && !temp.isFinish)
                {
                    temp.button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                }
            }
            foreach (int i in numbers)
            {
                Tile temp = gameBoard.Tiles[i];
                temp.button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public void SetTileImage(string text)
        {
            switch (text)
            {
                case "M":   //Mine
                    button.Content = new Image() { Source = gameBoard.MineImage };
                    break;
                case "F":   //Flag
                    button.Content = new Image() { Source = gameBoard.FlagImage };
                    break;
                case "U":   //Unflag
                    button.Content = "";
                    button.Background = Brushes.LightGray;
                    break;
                default:
                    button.Content = text;
                    button.Background = Brushes.Gold;
                    break;
            }
        }

        public void SetMine(bool mine)
        {
            this.hasMine = mine;
        }
    }

    public class GameboardEventArgs : EventArgs
    {
        public GameboardEventArgs()
        {

        }
    }

    public class Board
    {
        public delegate void GameboardEventHandler(object sender, GameboardEventArgs e);
        public event GameboardEventHandler GameboardEvent;    
        public void RaiseEvent(GameboardEventArgs e)
        {
            if (GameboardEvent != null)
                GameboardEvent(this, e);
        }

        BitmapImage mineImage;
        BitmapImage flagImage;
        Window gameWindow;
        List<Tile> tiles;
        Grid gameBoard;
        int row;
        int column;
        int mine;
        int finishCount;
        int turn = 0;
        double topPadding = 100;
        float blockSize;
        List<Player> players;
        Canvas gameCanvas;

        public BitmapImage FlagImage { get { return flagImage; } }
        public BitmapImage MineImage { get { return mineImage; } }
        public List<Tile> Tiles { get { return tiles; } }
        public Grid GameBoard { get { return gameBoard; } }
        public int Row { get { return row; } set { row = value; } }
        public int Mine { get { return mine; } set { mine = value; } }
        public int Column { get { return column; } set { column = value; } }
        public int Turn { get { return turn; } set { turn = value; } }
        public List<Player> Players { get { return players; } }
        public double TopPadding { get { return topPadding; } }
        public Canvas GameCanvas { get { return gameCanvas; } }
        public int FinishCount
        {
            get { return finishCount; }
            set
            {
                finishCount = value;
                if (finishCount == tiles.Count)
                    MessageBox.Show("Win");
            }
        } 

        public Board(int row, int column, int mine, Window window)
        {
            this.gameWindow = window;
            this.row = row;
            this.column = column;
            this.mine = mine;

            mineImage = new BitmapImage(new Uri("Resources/mine.bmp", UriKind.Relative));
            flagImage = new BitmapImage(new Uri("Resources/flag.bmp", UriKind.Relative));

            Initialize();
        }

        public void Initialize()
        {
            gameCanvas = new Canvas();
            tiles = new List<Tile>();
            gameBoard = new Grid();
            blockSize = 50 - row;
            finishCount = 0;
            turn = -1;

            gameCanvas.Width = column * blockSize;
            gameCanvas.Height = row * blockSize + topPadding;

            // Create the Grid
            gameBoard = new Grid();
            gameBoard.Width = column * blockSize;
            gameBoard.Height = row * blockSize;
            gameBoard.SetValue(Canvas.TopProperty, topPadding);
            gameBoard.HorizontalAlignment = HorizontalAlignment.Left;
            gameBoard.VerticalAlignment = VerticalAlignment.Top;
            gameBoard.Background = new SolidColorBrush(Colors.LightSteelBlue);

            //Create rows
            for (int i = 0; i < row; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(blockSize);
                gameBoard.RowDefinitions.Add(rowDef);

                // Create Columns
                for (int j = 0; j < column; j++)
                {
                    ColumnDefinition columnDef = new ColumnDefinition();
                    columnDef.Width = new GridLength(blockSize);
                    gameBoard.ColumnDefinitions.Add(columnDef);
                    Tile tile = new Tile(i, j, this);
                    tiles.Add(tile);
                }
            }
            SetMine(RandomNumber(mine), tiles);

            players = new List<Player>();
            players.Add(new Player(0, this));
            players.Add(new Player(1, this));

            gameCanvas.Children.Add(this.gameBoard);
            gameWindow.Content = this.gameCanvas;
            gameWindow.SizeToContent = SizeToContent.WidthAndHeight;

            SwitchPlayerTurn();
        }

        public HashSet<int> RandomNumber(int num_of_mine)
        {
            HashSet<int> numbers = new HashSet<int>();
            Random random = new Random();
            while (numbers.Count < num_of_mine)
            {
                numbers.Add(random.Next(0, (row * column) - 1));
            }
            return numbers;
        }

        public void SetMine(HashSet<int> numbers, List<Tile> tiles)
        {
            foreach (int i in numbers)
                tiles[i].SetMine(true);
        }

        public void ShowAllMine()
        {
            foreach (Tile t in tiles)
            {
                if (t.HasMine && !t.IsFinish)
                    t.SetTileImage("M");
            }
        }

        public void Gameover()
        {
            MessageBoxResult result = MessageBox.Show("Player" + turn + ": " + players[turn].Score.ToString(), "Gameover!");
            if (turn == 0)
                turn = 1;
            else if (turn == 1)
                turn = 0;
            //RaiseEvent(new RoutedEventArgs());
            //RaiseEvent(new GameboardEventArgs());
        }

        public void SwitchPlayerTurn()
        {
            if (turn == 0)
            {
                players[turn].PlayerHUDs.RemoveBorder();
                turn = 1;
                players[turn].PlayerHUDs.AddBorder();
            }
            else if (turn == 1)
            {
                players[turn].PlayerHUDs.RemoveBorder();
                turn = 0;
                players[turn].PlayerHUDs.AddBorder();
            }
            else
            {
                turn = 0;
                players[turn].PlayerHUDs.AddBorder();
            }
        }
    }

    public class PlayerHUD
    {
        Board gameBoard;
        Player player;
        Label scoreLabel;
        Label nameLabel;
        Image turnFlag;
        Rectangle background;
        int id;

        public PlayerHUD(Board gameBoard, Player player)
        {
            this.player = player;
            this.id = player.Id;
            this.gameBoard = gameBoard;
            Initialize();
        }
        public virtual void Initialize()
        {
            CreateBackground(Brushes.Red);
            CreateNameLabel("Player" + id.ToString());
            CreateScoreLabel();
            player.PlayerHUDs = this;
        }

        public void CreateNameLabel(string name)
        {
            nameLabel = new Label();
            nameLabel.Content = name;
            if(id == 0)
                nameLabel.SetValue(Canvas.LeftProperty, 20.0);
            else
            nameLabel.SetValue(Canvas.RightProperty, 20.0);
            nameLabel.SetValue(Canvas.TopProperty, 0.0);
            nameLabel.FontSize = 30;
            gameBoard.GameCanvas.Children.Add(nameLabel);
        }

        public void CreateScoreLabel()
        {
            scoreLabel = new Label();
            scoreLabel.Content = player.Score;
            if (id == 0)
                scoreLabel.SetValue(Canvas.LeftProperty, 20.0);
            else
                scoreLabel.SetValue(Canvas.RightProperty, 20.0);
            scoreLabel.SetValue(Canvas.TopProperty, 50.0);
            scoreLabel.FontSize = 26;
            gameBoard.GameCanvas.Children.Add(scoreLabel);
        }

        public void CreateBackground(SolidColorBrush colour)
        {
            background = new Rectangle()
            {
                Width = gameBoard.GameCanvas.Width / 2,
                Height = gameBoard.TopPadding,
                StrokeThickness = 5,
            };
            if (id == 0)
            {
                background.Fill = Brushes.Red;
            }
            else
            {
                background.Fill = Brushes.Blue;

                background.SetValue(Canvas.RightProperty, 0.0);
            }
            gameBoard.GameCanvas.Children.Add(background);
        }

        public void CreateTurnFlag()
        {
            turnFlag = new Image() { Source = gameBoard.FlagImage };
            turnFlag.SetValue(Canvas.LeftProperty, gameBoard.GameCanvas.Width / 2 - 40);
            turnFlag.SetValue(Canvas.TopProperty, gameBoard.TopPadding/2);
            gameBoard.GameCanvas.Children.Add(turnFlag);
        }

        public void AddBorder()
        {
            background.Stroke = Brushes.GreenYellow;
        }

        public void RemoveBorder()
        {
            background.Stroke = null;
        }

        public void UpdateScore(int score)
        {
            scoreLabel.Content = score.ToString();
        }
    }

    public class Menu
    {
        Canvas canvas;
        ComboBox levelOption;
        ComboBoxItem easy;
        ComboBoxItem normal;
        ComboBoxItem difficult;
        Button startButton;
        Window gameWindow;
        ImageBrush imageBrush;

        public Button StartButton { get { return startButton; } }
        public ComboBox LevelOption { get { return levelOption; } }

        public Menu(Window window)
        {
            gameWindow = window;
            imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("Resources/background.bmp", UriKind.Relative));
            Initialize();
        }

        public void Initialize()
        {
            canvas = new Canvas();
            canvas.Height = 250;
            canvas.Width = 250;
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            canvas.Background = imageBrush;

            levelOption = new ComboBox();
            levelOption.Height = 22;
            levelOption.Width = 75;
            levelOption.HorizontalAlignment = HorizontalAlignment.Center;
            levelOption.VerticalAlignment = VerticalAlignment.Center;
            levelOption.SetValue(Canvas.TopProperty, (double)150);
            levelOption.SetValue(Canvas.LeftProperty, (double)80);

            easy = new ComboBoxItem();
            easy.Content = "Easy";
            levelOption.Items.Add(easy);

            normal = new ComboBoxItem();
            normal.Content = "Normal";
            levelOption.Items.Add(normal);

            difficult = new ComboBoxItem();
            difficult.Content = "Difficult";
            difficult.IsSelected = true;
            levelOption.Items.Add(difficult);

            canvas.Children.Add(levelOption);

            startButton = new Button();
            startButton.Width = 75;
            startButton.Content = "Start";
            startButton.SetValue(Canvas.TopProperty, (double)180);
            startButton.SetValue(Canvas.LeftProperty, (double)80);
            canvas.Children.Add(startButton);

            gameWindow.Content = this.canvas;
            gameWindow.SizeToContent = SizeToContent.WidthAndHeight;
        }
    }

    public class Player
    {
        int score;
        int id;
        PlayerHUD playerHUDs;
        Board gameBoard;
        public PlayerHUD PlayerHUDs { get { return playerHUDs; } set { playerHUDs = value; }}

        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                playerHUDs.UpdateScore(score);
            }
        }
        public int Id { get { return id; } }

        public Player(int id, Board gameboard)
        {
            this.id = id;
            this.gameBoard = gameboard;
            Initialize();
        }

        public void Initialize()
        {
            playerHUDs = new PlayerHUD(gameBoard, this);
        }
    }
}
