using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UserInterface
{
    public class Menu
    {
        Canvas canvas;
        ComboBox levelOption;
        ComboBoxItem easy;
        ComboBoxItem normal;
        ComboBoxItem difficult;
        ComboBoxItem pvpEasy;
        ComboBoxItem pvpNormal;
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
            levelOption.Width = 100;
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
            levelOption.Items.Add(difficult);

            pvpEasy = new ComboBoxItem();
            pvpEasy.Content = "PvP (Easy)";
            pvpEasy.IsSelected = true;
            levelOption.Items.Add(pvpEasy);

            pvpNormal = new ComboBoxItem();
            pvpNormal.Content = "PvP (Normal)";
            pvpNormal.IsSelected = true;
            levelOption.Items.Add(pvpNormal);


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
    public class TopBanner
    {
        double width;
        double height;
        Rectangle background;  //canvas width
        Rectangle leftField;   //canvas.width/7*3
        Rectangle rightField;   //canvas.width/7*3
        Canvas gameCanvas;

        Label leftName;
        Label leftScore;
        Label rightName;
        Label rightScore;
        Label winCondition;
        List<Label> Scores;
        List<Rectangle> indicators;

        public List<Rectangle> Indicators;
        public Label LeftName { get { return leftName; } set { leftName = value; } }
        public Label LeftScore { get { return leftScore; } set { leftScore = value; } }
        public Label RightName { get { return rightName; } set { rightName = value; } }
        public Label RightScore { get { return rightScore; } set { rightScore = value; } }
        public Label WinCondition { get { return winCondition; } set { winCondition = value; } }
        public Rectangle LeftField { get { return leftField; } set { leftField = value; } }
        public Rectangle RightField { get { return rightField; } set { rightField = value; } }

        public TopBanner(double width, double height, Canvas canvas)
        {
            this.gameCanvas = canvas;
            this.width = width;
            this.height = height;

            Initialize();
        }

        public void Initialize()
        {
            indicators = new List<Rectangle>();
            Scores = new List<Label>();
            background = new Rectangle()
            {
                Width = this.width,
                Height = this.height,
                Fill = Brushes.HotPink,
            };
            gameCanvas.Children.Add(background);

            leftField = new Rectangle()
            {
                Width = this.width / 7 * 3,
                Height = this.height,
                Fill = Brushes.Red,
                StrokeThickness = 5,
            };
            gameCanvas.Children.Add(leftField);
            indicators.Add(leftField);

            rightField = new Rectangle()
            {
                Width = this.width / 7 * 3,
                Height = this.height,
                Fill = Brushes.Blue,
                StrokeThickness = 5,
            };
            rightField.SetValue(Canvas.RightProperty, 0.0);
            gameCanvas.Children.Add(rightField);
            indicators.Add(RightField);

            leftName = new Label();
            leftName.FontSize = 35;
            leftName.Content = "Player1";
            gameCanvas.Children.Add(leftName);

            leftScore = new Label();
            leftScore.FontSize = 25;
            leftScore.Content = 0;
            leftScore.SetValue(Canvas.TopProperty, background.Height / 2);
            gameCanvas.Children.Add(leftScore);
            Scores.Add(LeftScore);

            rightName = new Label();
            rightName.FontSize = 35;
            rightName.SetValue(Canvas.RightProperty, 0.0);
            rightName.Content = "Player2";
            gameCanvas.Children.Add(rightName);

            rightScore = new Label();
            rightScore.FontSize = 25;
            rightScore.SetValue(Canvas.RightProperty, 0.0);
            rightScore.SetValue(Canvas.TopProperty, background.Height / 2);
            rightScore.Content = 0;

            gameCanvas.Children.Add(rightScore);
            Scores.Add(rightScore);

            winCondition = new Label();
            winCondition.FontSize = 40;
            winCondition.SetValue(Canvas.TopProperty, background.Height / 4);
            winCondition.SetValue(Canvas.RightProperty, background.Width / 2 - winCondition.FontSize / 1.5);
            winCondition.Content = 25;
            gameCanvas.Children.Add(winCondition);
        }

        public void RemoveIndicator(int i)
        {
            indicators[i].Stroke = null;
        }

        public void AddIndicator(int i)
        {
            indicators[i].Stroke = Brushes.GreenYellow;
        }

        public void UpdateScore(int turn, int newScore)
        {
            Scores[turn].Content = newScore;
        }
    }
}