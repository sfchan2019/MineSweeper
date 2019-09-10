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

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        MultiplayerGame.Board board;
        //SinglePlayerGame.Board 
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
        public void InitializeMultiplayerGame(int row, int column, int mine)
        {
            board = new MultiplayerGame.Board(row, column, mine, this);
            board.GameboardEvent += OnGameover;
        }

        private void OnGameover(object sender, GameboardEventArgs e)
        {
            if (e.GameboardEvent != GAME_EVENT.GAMEOVER)
                return;
            Board gameBoard = sender as Board;
            MessageBox.Show("Player" + (gameBoard.Turn+1).ToString() + " wins!", "Congratulations!");
            InitializeMenu();
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            switch (menu.LevelOption.SelectedIndex)
            {
                //case 0: //Easy                 
                //    //InitializeMultiplayerGame(6, 6, 7);  //number of row, column and mine
                //    break;
                //case 1: //Normal
                //    //InitializeMultiplayerGame(16, 16, 50);
                //    break;
                //case 2: //Difficult
                //    //InitializeMultiplayerGame(25, 25, 100);
                //    break;
                case 0:
                    InitializeMultiplayerGame(8, 8, 10);
                    break;
                case 1:
                    InitializeMultiplayerGame(16, 16, 50);
                    break;
                default:
                    break;
            }
        }
    }
}
