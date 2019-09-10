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



namespace SinglePlayerGame
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
            System.Windows.Controls.Grid.SetColumn(this.button, column);
            System.Windows.Controls.Grid.SetRow(this.button, row);
            gameBoard.GameBoard.Children.Add(button);
        }

        public void OnRightClickTile(object sender, RoutedEventArgs e)
        {
            FlagTile();
        }

        public void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsFinish)
                return;
            if (e.ChangedButton == MouseButton.Left)
                InvokeNeighbourTiles(AllNeighbourNumber(this.id));
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
            this.isFinish = true;
            gameBoard.FinishCount++;
            if (hasMine)
            {
                gameBoard.Gameover();
                button.Content = new Image() { Source = gameBoard.MineImage };
            }
            else
            {
                SweepMine();
            }
        }

        private void SweepMine()
        {

            this.button.MouseRightButtonDown -= OnRightClickTile;
            int count = CountNearMine();
            if (count != 0)
            {
                SetTileImage(count.ToString());
            }
            else
            {
                this.button.IsEnabled = false;
            }
        }

        List<int> AllNeighbourNumber(int i)
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
            List<int> numbers = AllNeighbourNumber(this.id);
            int count = 0;

            foreach (int i in numbers)
            {
                Tile temp = gameBoard.Tiles[i];
                if (temp.hasMine)
                    count++;
            }
            if (count == 0) //if no mine is around, automatically check all the neighbours.
                InvokeNeighbourTiles(numbers);
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
        Canvas canvas;

        public BitmapImage FlagImage { get { return flagImage; } }
        public BitmapImage MineImage { get { return mineImage; } }
        public List<Tile> Tiles { get { return tiles; } }
        public Grid GameBoard { get { return gameBoard; } }
        public int Row { get { return row; } set { row = value; } }
        public int Column { get { return column; } set { column = value; } }
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
            canvas = new Canvas();
            double topPadding = 100.0f;
            tiles = new List<Tile>();
            gameBoard = new Grid();
            float blockSize = 50 - row;
            finishCount = 0;

            canvas.Width = column * blockSize;
            canvas.Height = row * blockSize + topPadding;

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

            canvas.Children.Add(this.gameBoard);
            //gameWindow.Content = this.gameBoard;
            gameWindow.Content = this.canvas;
            gameWindow.SizeToContent = SizeToContent.WidthAndHeight;
        }

        public List<int> RandomNumber(int num_of_mine)
        {
            List<int> numbers = new List<int>();
            Random random = new Random();
            for (int i = 0; i < num_of_mine; i++)
            {
                int rand = random.Next(0, (row * column) - 1);
                numbers.Add(rand);
            }
            return numbers;
        }

        public void SetMine(List<int> numbers, List<Tile> tiles)
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
            ShowAllMine();
            RaiseEvent(new GameboardEventArgs());
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
}
