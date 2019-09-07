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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        Board board;
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        void InitializeGame()
        {
            board = new Board(25, 16, 50);
            this.Content = board.GameBoard;
            this.SizeToContent = SizeToContent.WidthAndHeight;
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
        bool isS = false;


        public bool HasMine
        {
            get { return hasMine; }
            set
            {
                hasMine = value;
                button.Content = "M";
            }
        }

        public Tile(int row, int column, Board gameBoard)
        {
            this.column = column;
            this.row = row;
            this.gameBoard = gameBoard;
            this.id = column + row * gameBoard.Column;
            this.isFinish = false;
            button = new Button();
            hasMine = false;
            button.Click += OnTileClick;
            Grid.SetColumn(this.button, column);
            Grid.SetRow(this.button, row);
            gameBoard.GameBoard.Children.Add(button);
        }

        public void OnTileClick(object sender, RoutedEventArgs e)
        {
            this.isFinish = true;
            if (hasMine)
                MessageBox.Show("Boom");
            else
            {
                this.SweepMine();
            }
        }

        private void SweepMine()
        {
            this.button.IsEnabled = false;

            List<Tile> neighbour = GetNeighbourTile();
            int count = NumberOfMineNearby(neighbour);

            if (count != 0)
            {
                this.button.Content = count.ToString();
            }
            else
            {
                this.button.Content = "";
                foreach (Tile t in neighbour) {
                    t.Spread();
                } 
                //foreach (Tile t in neighbour)
                //{
                //    if (!t.isFinish && t.NumberOfMineNearby(t.GetNeighbourTile()) == 0)
                //    {
                //        t.SweepMine();
                //    }
                //}
            }
            this.button.IsEnabled = false;
        }

        private void Spread()
        {
            this.button.Content = "S";
        }

        List<int> NeighbourNumber(int i, int row, int column)
        {
            int x = i % column;
            int y = i / column;

            List<int> numbers = new List<int>();

            if (x != 0)                             //if not on the left column
                numbers.Add(i - 1);
            if (x != column - 1)                    //if not on the right column
                numbers.Add(i + 1);
            if (y != 0)                             //if not on the top row
                numbers.Add(i - column);
            if (y != 0 && x != 0)                   //if not on the top row AND not on the left column
                numbers.Add(i - column - 1);
            if (y != 0 && x != column - 1)          //if not on the top row AND not on the right column
                numbers.Add(i - column + 1);
            if (y != row - 1)                       //if not on the button row
                numbers.Add(i + column);
            if (y != row - 1 && x != column - 1)    //if not on the bottm row AND not on the right column
                numbers.Add(i + column + 1);
            if (y != row - 1 && x != 0)             //if not on the bottom row AND not on the left column
                numbers.Add(i + column - 1);
        
            return numbers;
        }

        private List<Tile> GetNeighbourTile()
        {
            List<Tile> tiles = new List<Tile>();
            List<int> numbers = NeighbourNumber(this.id, gameBoard.Row, gameBoard.Column);
            foreach (int i in numbers)
            {
                if (!gameBoard.Tiles[i].isFinish)
                    tiles.Add(gameBoard.Tiles[i]);
            }

            return tiles;
        }

        int NumberOfMineNearby(List<Tile> tiles)
        {
            int count = 0;
            foreach (Tile t in tiles)
            {
                if (t.hasMine)
                    count++;
            }
            return count;
        }
    }

    public class Board
    {
        List<Tile> tiles;
        Grid gameBoard;
        int row;
        int column;
        int mine;

        public List<Tile> Tiles { get { return tiles; } }
        public Grid GameBoard { get { return gameBoard; } }
        public int Row { get { return row; } set { row = value; } }
        public int Column { get { return column; } set { column = value; } }

        public Board(int row, int column, int mine)
        {
            this.row = row;
            this.column = column;
            this.mine = mine;
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
            {
                tiles[i].HasMine = true;
            }
        }
    }
}
