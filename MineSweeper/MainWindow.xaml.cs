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
using MineSweeperGame;
using UserInterface;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        UserInterface.Menu menu;
        List<MineSweeperGame.MineSweeper> games = new List<MineSweeperGame.MineSweeper>();
        
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
            InitializeMenu();
        }

        public void Initialize()
        {
            games.Add(new MineSweeperGame.SP_GameBoard());
            games[0].GameboardEvent += OnSinglePlayerGameover;
            games.Add(new MineSweeperGame.MP_GameBoard());
            games[1].GameboardEvent += OnMultiplayerGameover;
        }
    
        public void InitializeMenu()
        {
            menu = new UserInterface.Menu(this);
            menu.StartButton.Click += OnStartButtonClick;
        }

        public void OnMultiplayerGameover(object sender, GameboardEventArgs e)
        {
            if (e.GameboardEvent != GAME_EVENT.GAMEOVER)
                return;
            MP_GameBoard gameBoard = sender as MP_GameBoard;
            MessageBox.Show("Player" + (gameBoard.Turn + 1).ToString() + " wins!", "Congratulations!");
            InitializeMenu();
        }

        public void OnSinglePlayerGameover(object sender, GameboardEventArgs e)
        {
            if (e.GameboardEvent != GAME_EVENT.GAMEOVER)
                return;
            MessageBox.Show("Gameover");
            InitializeMenu();
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            switch (menu.LevelOption.SelectedIndex)
            {
                case 0: //Easy                 
                    games[0].Initialize(6, 6, 7, this);
                    break;
                case 1: //Normal
                    games[0].Initialize(16, 16, 50, this);
                    break;
                case 2: //Difficult
                    games[0].Initialize(25, 25, 100, this);
                    break;
                case 3:
                    games[1].Initialize(8, 8, 10, this);
                    break;
                case 4:
                    games[1].Initialize(16, 16, 50, this);
                    break;
                default:
                    break;
            }
        }
    }
}
