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
using MultiplayerGame;
using UserInterface;
using MineSweeperInterface;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        IMineSweeperGame multiGame;
        IMineSweeperGame singleGame;
        UserInterface.Menu menu;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeMenu();
        }
        public void InitializeMenu()
        {
            menu = new UserInterface.Menu(this);
            menu.StartButton.Click += OnStartButtonClick;
        }

        public void InitializeGame(int row, int column, int mine, bool multiplayer)
        {
            if (multiplayer)
            {
                multiGame = new MultiplayerGame.Board(row, column, mine, this);
                MultiplayerGame.Board temp = multiGame as MultiplayerGame.Board;
                temp.GameboardEvent += OnGameover;
            }
            else
            {
                singleGame = new SinglePlayerGame.Board(row, column, mine, this);
                SinglePlayerGame.Board temp = singleGame as SinglePlayerGame.Board;
                //temp.GameboardEvent += OnGameover;
            }
        }

        public void OnGameover(object sender, GameboardEventArgs e)
        {
            if (e.GameboardEvent != GAME_EVENT.GAMEOVER)
                return;
            Board gameBoard = sender as Board;
            MessageBox.Show("Player" + (gameBoard.Turn + 1).ToString() + " wins!", "Congratulations!");
            InitializeMenu();
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            switch (menu.LevelOption.SelectedIndex)
            {
                case 0: //Easy                 
                    InitializeGame(6, 6, 7, false);  //number of row, column and mine
                    break;
                case 1: //Normal
                    InitializeGame(16, 16, 50, false);
                    break;
                case 2: //Difficult
                    InitializeGame(25, 25, 100, false);
                    break;
                case 3:
                    InitializeGame(8, 8, 10, true);
                    //games[1].Initialize();
                    break;
                case 4:
                    InitializeGame(16, 16, 50, true);
                    //games[1].Initialize();
                    break;
                default:
                    break;
            }
        }
    }
}
