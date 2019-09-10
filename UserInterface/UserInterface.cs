using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            //levelOption.Items.Add(easy);

            normal = new ComboBoxItem();
            normal.Content = "Normal";
            //levelOption.Items.Add(normal);

            difficult = new ComboBoxItem();
            difficult.Content = "Difficult";
            //levelOption.Items.Add(difficult);

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
}
