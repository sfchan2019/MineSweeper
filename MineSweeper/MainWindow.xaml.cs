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


namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        Board board;
        Menu mainMenu;
        public Menu MainMenu { get { return mainMenu; } }
        public MainWindow()
        {
            InitializeComponent();
            InitializeMenu();
            //InitializeGame(25, 16, 40);
        }

        public void InitializeGame(int row, int column, int mine)
        {
            board = new Board(row, column, mine, this);
        }

        public void InitializeMenu()
        {
            mainMenu = new Menu(this);
        }
    }

    public class Tile
    {
        Button button;
        Board gameBoard;
        int row;
        int column;
        int id;
        bool hasMine;
        bool isFinish;

        public bool HasMine { get { return hasMine; }}

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
            button.PreviewMouseRightButtonDown += OnRightClickTile;
            Grid.SetColumn(this.button, column);
            Grid.SetRow(this.button, row);
            gameBoard.GameBoard.Children.Add(button);
        }

        public void OnRightClickTile(object sender, RoutedEventArgs e)
        {
            FlagTile();
        }

        public void FlagTile()
        {
            isFinish = !isFinish;
            if (isFinish)
                SetTileImage("F");
            else
                SetTileImage("");
        }       

        public void OnLeftClickTile(object sender, RoutedEventArgs e)
        {
            if (isFinish)
                return;
            this.isFinish = true;
            if (hasMine)
                gameBoard.Gameover();
            else
                SweepMine();
        }

        private void SweepMine()
        {
            this.button.IsEnabled = false;

            int count = CountNearMine();
            if (count != 0)
                SetTileImage(count.ToString());
            else
                SetTileImage("");
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
                temp.button.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        public void SetTileImage(string text)
        {
            switch (text)
            {
                case "M":
                    button.Content = new Image() { Source = gameBoard.MineImage };
                    break;
                default:
                    button.Content = text;
                    break;
            }
        }

        public void SetMine(bool mine)
        {
            if (mine)
            {
                this.hasMine = true;
                //this.button.Content = "M";
            }
            else
            {
                this.hasMine = false;
                this.button.Content = "";
            }
            
        }
    }

    public class Board
    {
        private BitmapImage mineImage;
        private MainWindow gameWindow;
        List<Tile> tiles;
        Grid gameBoard;
        int row;
        int column;
        int mine;

        public BitmapImage MineImage { get { return mineImage; } }
        public List<Tile> Tiles { get { return tiles; } }
        public Grid GameBoard { get { return gameBoard; } }
        public int Row { get { return row; } set { row = value; } }
        public int Column { get { return column; } set { column = value; } }

        public Board(int row, int column, int mine, MainWindow window)
        {
            this.gameWindow = window;
            this.row = row;
            this.column = column;
            this.mine = mine;

            mineImage = new BitmapImage(new Uri("Resources/mine.bmp", UriKind.Relative));

            Initialize();
        }

        public void Initialize()
        {
            tiles = new List<Tile>();
            gameBoard = new Grid();
            int blockSize = 45;

            // Create the Grid
            gameBoard = new Grid();
            gameBoard.Width = column * blockSize;
            gameBoard.Height = row * blockSize;
            gameBoard.HorizontalAlignment = HorizontalAlignment.Left;
            gameBoard.VerticalAlignment = VerticalAlignment.Top;
            gameBoard.ShowGridLines = true;
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

            gameWindow.Content = this.gameBoard;
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
                if (t.HasMine)
                    t.SetTileImage("M");
            }
        }

        public void Gameover()
        {
            ShowAllMine();
            MessageBoxResult result = MessageBox.Show("Boom", "Gameover!");
            if (result == MessageBoxResult.OK)
                //Initialize();
                gameWindow.InitializeMenu();
        }
    }

    public class Menu
    {
        Canvas canvas;
        ComboBox levelOption;
        ComboBoxItem easy;
        ComboBoxItem medium;
        ComboBoxItem difficut;
        Button startButton;
        
        private MainWindow gameWindow;
        public Menu(MainWindow window)
        {
            gameWindow = window;

            Initialize();
        }

        public void Initialize()
        {
            //    <Canvas HorizontalAlignment="Left" Height="Auto" VerticalAlignment="Top" Width="Auto">
            //    <ComboBox x:Name="levelOption" Height="22" Canvas.Left="284" Canvas.Top="221" Width="100">
            //    <ComboBoxItem IsSelected="True">Easy</ComboBoxItem>
            //    <ComboBoxItem>Medium</ComboBoxItem>
            //    <ComboBoxItem>Difficult</ComboBoxItem>
            //    </ComboBox>
            //    <Button x:Name="startButton" Content="Start" Canvas.Left="300" Canvas.Top="260" Width="75" Click="startButton_Click"/>
            //</Canvas>
            canvas = new Canvas();
            canvas.Height = 400;
            canvas.Width = 250;
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;

            levelOption = new ComboBox();
            levelOption.Height = 22;
            levelOption.Width = 100;
            levelOption.HorizontalAlignment = HorizontalAlignment.Center;
            levelOption.VerticalAlignment = VerticalAlignment.Center;

            easy = new ComboBoxItem();
            easy.Content = "Easy";
            levelOption.Items.Add(easy);

            medium = new ComboBoxItem();
            medium.Content = "Medium";
            levelOption.Items.Add(medium);

            difficut = new ComboBoxItem();
            difficut.Content = "Difficult";
            levelOption.Items.Add(difficut);

            canvas.Children.Add(levelOption);

            startButton = new Button();
            startButton.Width = 75;
            startButton.Content = "Start";
            startButton.Click += startButton_Click;
            canvas.Children.Add(startButton);

            gameWindow.Content = this.canvas;
            gameWindow.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            switch (levelOption.SelectedIndex)
            {
                case 0:
                    gameWindow.InitializeGame(6, 6, 20);
                    break;
                case 1:
                    gameWindow.InitializeGame(25, 16, 60);
                    break;
                case 2:
                    gameWindow.InitializeGame(25, 25, 120);
                    break;
            }
        }
    }
}
