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
        Game.Board board;
        Game.Menu menu;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeMenu();
        }
        public void InitializeMenu()
        {
            menu = new Game.Menu(this);
            menu.StartButton.Click += startButton_Click;
        }
        public void InitializeGame(int row, int column, int mine)
        {
            board = new Game.Board(row, column, mine, this);
            board.GameboardEvent += OnGameover;
        }

        private void OnGameover(object sender, RoutedEventArgs e)
        {
            InitializeMenu();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            switch (menu.LevelOption.SelectedIndex)
            {
                case 0:
                    InitializeGame(6, 6, 20);
                    break;
                case 1:
                    InitializeGame(25, 16, 60);
                    break;
                case 2:
                    InitializeGame(25, 25, 120);
                    break;
            }
        }
    }
}
