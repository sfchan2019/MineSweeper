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
using UserInterface;

namespace MineSweeperGame
{
    public enum GAME_EVENT
    {
        GAMEOVER, COLLECT_OBJECT,
    }
    public class GameboardEventArgs : EventArgs
    {
        GAME_EVENT e;
        public GAME_EVENT GameboardEvent { get { return e; } }
        public GameboardEventArgs(GAME_EVENT e) {this.e = e; }
    }
    public abstract class Tile
    {
        public delegate void GameEvent(Object sender, GameboardEventArgs e);
        public GameEvent GameEventHandler;
        public void RaiseEvent(GameboardEventArgs e)
        {
            if (GameEventHandler != null)
                GameEventHandler(this, e);
        }

        protected Button button;
        protected MineSweeper gameBoard;
        protected int row;
        protected int column;
        protected int tileID;
        protected bool hasMine;
        protected bool isFinish;

        public bool HasMine { get { return hasMine; } }
        public bool IsFinish { get { return isFinish; } }

        public virtual void OnLeftClickTile(object sender, RoutedEventArgs e)
        {

        }

        public virtual void OnRightClickTile(object sender, RoutedEventArgs e) { }

        public virtual void OnDoubleClickTile(object sender, MouseButtonEventArgs e) { }

        public virtual bool CheckHasObject()
        {
            if (isFinish)
                return false;
            else
            {
                this.isFinish = true;
                if (this.hasMine)
                {
                    this.RaiseEvent(new GameboardEventArgs(GAME_EVENT.COLLECT_OBJECT));
                    return true;
                }
                else
                {
                    List<int> numbers = GetNeighbourIndecies(this.tileID);
                    int count = CountObjFromGroup(numbers);
                    if (count == 0)
                        InvokeGroupOfTile(numbers);
                    else
                        SetTileImage(count.ToString()); 
                }
            }
            return false;
        }

        public virtual void OnCollectObject(Object sender, GameboardEventArgs e)
        {
            if (e.GameboardEvent != GAME_EVENT.COLLECT_OBJECT)
                return;
            gameBoard.Players[gameBoard.Turn].Score++;
            gameBoard.TopBannerHUD.UpdateScore(gameBoard.Turn, gameBoard.Players[gameBoard.Turn].Score);
            SetTileImage("F");
            if(gameBoard.Players[gameBoard.Turn].Score > gameBoard.Mine/2)
                gameBoard.RaiseEvent(new GameboardEventArgs(GAME_EVENT.GAMEOVER));
        }

        public virtual void InvokeGroupOfTile(List<int> numbers)
        {
            this.button.IsEnabled = false;
            foreach (int i in numbers)
            {
                Tile temp = gameBoard.Tiles[i];
                temp.CheckHasObject();
            }
        }

        public virtual int CountObjFromGroup(List<int> numbers)
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

        protected List<int> GetNeighbourIndecies(int i)
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

        public virtual void SetTileImage(string text)
        {
            switch (text)
            {
                case "F":   //Flag
                    if (gameBoard.Turn == 0)
                        button.Content = new Image() { Source = gameBoard.RedFlagImage };
                    else if (gameBoard.Turn == 1)
                        button.Content = new Image() { Source = gameBoard.BlueFlagImage };
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

    public abstract class MineSweeper
    {
        public delegate void GameboardEventHandler(object sender, GameboardEventArgs e);
        public event GameboardEventHandler GameboardEvent;
        public void RaiseEvent(GameboardEventArgs e)
        {
            if (GameboardEvent != null)
                GameboardEvent(this, e);
        }

        protected BitmapImage mineImage;
        protected BitmapImage flagImage;
        protected BitmapImage redFlagImage;
        protected BitmapImage blueFlagImage;
        protected Window gameWindow;
        protected List<Tile> tiles;
        protected Grid gameBoard;
        protected int row;
        protected int column;
        protected int mine;
        protected int turn = 0;
        protected double topPadding = 100;
        protected float blockSize;
        protected List<Player> players;
        protected Canvas gameCanvas;
        protected TopBanner topBannerHUD;

        public BitmapImage RedFlagImage { get { return redFlagImage; } }
        public BitmapImage BlueFlagImage { get { return blueFlagImage; } }
        public BitmapImage MineImage { get { return mineImage; } }
        public BitmapImage FlagImage { get { return flagImage; } }
        public List<Tile> Tiles { get { return tiles; } }
        public Grid GameBoard { get { return gameBoard; } }
        public int Row { get { return row; } set { row = value; } }
        public int Mine { get { return mine; } set { mine = value; } }
        public int Column { get { return column; } set { column = value; } }
        public int Turn { get { return turn; } set { turn = value; } }
        public List<Player> Players { get { return players; } }
        public double TopPadding { get { return topPadding; } }
        public Canvas GameCanvas { get { return gameCanvas; } }
        public TopBanner TopBannerHUD { get { return topBannerHUD; } }

        //
        public virtual void Initialize(int row, int column, int mine, Window window)
        {
            this.gameWindow = window;
            this.row = row;
            this.column = column;
            this.mine = mine;

            mineImage = new BitmapImage(new Uri("Resources/mine.bmp", UriKind.Relative));
            redFlagImage = new BitmapImage(new Uri("Resources/flag0.bmp", UriKind.Relative));
            blueFlagImage = new BitmapImage(new Uri("Resources/flag1.bmp", UriKind.Relative));
            flagImage = new BitmapImage(new Uri("Resources/flag.bmp", UriKind.Relative));

            gameCanvas = new Canvas();
            tiles = new List<Tile>();
            gameBoard = new Grid();
            blockSize = 50 - row;
            turn = -1;
            players = new List<Player>();

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
        }

        //Create a list of number that represent mine on the grid
        protected HashSet<int> RandomNumber(int num_of_mine)
        {
            HashSet<int> numbers = new HashSet<int>();
            Random random = new Random();
            while (numbers.Count < num_of_mine)
            {
                numbers.Add(random.Next(0, (row * column) - 1));
            }
            return numbers;
        }

        //Given a list of indices of the grid, put mine in to the tile
        protected void SetMine(HashSet<int> numbers, List<Tile> tiles)
        {
            foreach (int i in numbers)
                tiles[i].SetMine(true);
        }
    }

    public class MP_Tile : Tile
    {
        public MP_Tile(int row, int column, MineSweeper gameBoard)
        {
            this.column = column;
            this.row = row;
            this.gameBoard = gameBoard;
            this.tileID = column + row * gameBoard.Column;
            this.isFinish = false;
            button = new Button();
            hasMine = false;
            button.Click += OnLeftClickTile;
            button.Background = Brushes.SkyBlue;
            System.Windows.Controls.Grid.SetColumn(this.button, column);
            System.Windows.Controls.Grid.SetRow(this.button, row);
            gameBoard.GameBoard.Children.Add(button);
            GameEventHandler += OnCollectObject;
        }

        public override void OnLeftClickTile(object sender, RoutedEventArgs e)
        {
            base.OnLeftClickTile(sender, e);
            if (isFinish)
                return;
            if (!CheckHasObject())
                (gameBoard as MP_GameBoard).SwitchPlayerTurn();
        }
    }

    public class SP_GameBoard : MineSweeper
    {
        int finishCount;
        public int FinishCount { get { return finishCount; } set { finishCount = value; } }
        public SP_GameBoard() { }

        public override void Initialize(int row, int column, int mine, Window window)
        {
            base.Initialize(row, column, mine, window);
            gameBoard.Background = new SolidColorBrush(Colors.LightGray);

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
                    Tile tile = new SP_Tile(i, j, this);
                    tiles.Add(tile);
                }
            }
            SetMine(RandomNumber(mine), tiles);

            gameCanvas.Children.Add(this.gameBoard);
            gameWindow.Content = this.gameCanvas;
            gameWindow.SizeToContent = SizeToContent.WidthAndHeight;
        }

        public void ShowAllMine()
        {
            foreach (Tile t in tiles)
            {
                if (t.HasMine)
                    t.SetTileImage("M");
            }
        }
    }

    public class SP_Tile : Tile
    {
        SP_GameBoard board;
        public SP_Tile(int row, int column, MineSweeper gameBoard)
        {
            this.column = column;
            this.row = row;
            this.gameBoard = gameBoard;
            this.tileID = column + row * gameBoard.Column;
            this.isFinish = false;
            board = gameBoard as SP_GameBoard;
            button = new Button();
            hasMine = false;
            button.Click += OnLeftClickTile;
            button.MouseDoubleClick += OnDoubleClickTile;
            button.MouseRightButtonDown += OnRightClickTile;
            System.Windows.Controls.Grid.SetColumn(this.button, column);
            System.Windows.Controls.Grid.SetRow(this.button, row);
            gameBoard.GameBoard.Children.Add(button);
            GameEventHandler += OnCollectObject;
        }

        public override void OnCollectObject(object sender, GameboardEventArgs e)
        {
            if (e.GameboardEvent != GAME_EVENT.COLLECT_OBJECT)
                return;
            board.ShowAllMine();
            board.RaiseEvent(new GameboardEventArgs(GAME_EVENT.GAMEOVER));
        }

        public override void OnLeftClickTile(object sender, RoutedEventArgs e)
        {
            base.OnLeftClickTile(sender, e);
            if (isFinish)
                return;
            this.isFinish = true;
            button.MouseRightButtonDown -= OnRightClickTile;
            board.FinishCount++;
            if (CheckHasObject())
                RaiseEvent(new GameboardEventArgs(GAME_EVENT.COLLECT_OBJECT));
        }

        public override bool CheckHasObject()
        {
            if (HasMine)
                return true;
            else
            {
                List<int> numbers = GetNeighbourIndecies(this.tileID);
                int count = CountObjFromGroup(numbers);
                if (count == 0)
                {
                    this.button.IsEnabled = false;
                    InvokeGroupOfTile(numbers);
                }
                else
                {
                    SetTileImage(count.ToString());
                    return false;
                }
            }

            return false;
        }

        public override void InvokeGroupOfTile(List<int> numbers)
        {
            foreach (int i in numbers)
            {
                SP_Tile temp = gameBoard.Tiles[i] as SP_Tile;
                if (temp.hasMine && !temp.isFinish)
                {
                    temp.button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    return;
                }
            }
            foreach (int i in numbers)
            {
                SP_Tile temp = gameBoard.Tiles[i] as SP_Tile;
                temp.button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public override void OnDoubleClickTile(object sender, MouseButtonEventArgs e)
        {
            base.OnDoubleClickTile(sender, e);

            if (e.ChangedButton == MouseButton.Left)
                InvokeGroupOfTile(GetNeighbourIndecies(tileID));
        }

        public override void OnRightClickTile(object sender, RoutedEventArgs e)
        {
            base.OnRightClickTile(sender, e);
            FlagTile();
        }

        public void FlagTile()
        {
            if (!isFinish)
            {
                SetTileImage("F");
                button.MouseDoubleClick -= OnDoubleClickTile;
                board.FinishCount++;
                if (hasMine)
                    board.Mine--;
                if (board.Mine == 0)
                    board.RaiseEvent(new GameboardEventArgs(GAME_EVENT.GAMEOVER));
            }
            else
            {
                SetTileImage("U");
                button.MouseDoubleClick += OnDoubleClickTile;
                board.FinishCount--;
                if (hasMine)
                    board.Mine++;
            }
            isFinish = !isFinish;
        }

        public override void SetTileImage(string text)
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
                    button.Background = Brushes.SkyBlue;
                    break;
            }
        }
    }

    public class MP_GameBoard : MineSweeper
    {
        public MP_GameBoard() { }

        public override void Initialize(int row, int column, int mine, Window window)
        {
            //assign values to variables
            base.Initialize(row, column, mine, window);

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
                    Tile tile = new MP_Tile(i, j, this);
                    tiles.Add(tile);
                }
            }
            SetMine(RandomNumber(mine), tiles);

            topBannerHUD = new TopBanner(this.gameCanvas.Width, topPadding, this.gameCanvas);
            topBannerHUD.WinCondition.Content = mine / 2 + 1;
            for (int i = 0; i < 2; i++)
            {
                players.Add(new Player(i, this));
            }

            gameCanvas.Children.Add(this.gameBoard);
            gameWindow.Content = this.gameCanvas;
            gameWindow.SizeToContent = SizeToContent.WidthAndHeight;

            SwitchPlayerTurn();
        }

        public void SwitchPlayerTurn()
        {
            bool bTurn = Convert.ToBoolean(turn);
            topBannerHUD.RemoveIndicator(Convert.ToInt32(bTurn));
            bTurn = !bTurn;
            topBannerHUD.AddIndicator(Convert.ToInt32(bTurn));
            turn = Convert.ToInt32(bTurn);
        }
    }

    public class Player
    {
        int score;
        int id;
        MP_GameBoard gameBoard;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        public int Id { get { return id; } }
        public Player(int id, MP_GameBoard gameboard)
        {
            this.id = id;
            this.gameBoard = gameboard;
        }
    }
}
