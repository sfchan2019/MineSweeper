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
        MultiplayerGame.Board board;
        MultiplayerGame.Menu menu;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeMenu();
        }
        public void InitializeMenu()
        {
            menu = new MultiplayerGame.Menu(this);
            menu.StartButton.Click += OnStartButtonClick;
        }
        public void InitializeGame(int row, int column, int mine)
        {
            board = new MultiplayerGame.Board(row, column, mine, this);
            board.GameboardEvent += OnGameover;
        }

        private void OnGameover(object sender, EventArgs e)
        {
            InitializeMenu();
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            switch (menu.LevelOption.SelectedIndex)
            {
                case 0: //Easy                 
                    InitializeGame(6, 6, 7);  //number of row, column and mine
                    break;
                case 1: //Normal
                    InitializeGame(25, 16, 60);
                    break;
                case 2: //Difficult
                    InitializeGame(25, 25, 100);
                    break;
            }
        }
    }
}
