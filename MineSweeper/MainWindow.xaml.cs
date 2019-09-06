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
        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid(5, 5);
        }

        private void InitializeGrid(int row, int colume)
        {
            int blockSize = 45;
            // Create the Grid
            Grid DynamicGrid = new Grid();
            DynamicGrid.Width = row * blockSize;
            DynamicGrid.HorizontalAlignment = HorizontalAlignment.Left;
            DynamicGrid.VerticalAlignment = VerticalAlignment.Top;
            DynamicGrid.ShowGridLines = true;
            DynamicGrid.Background = new SolidColorBrush(Colors.LightSteelBlue);

            // Create Columns
            for (int i = 0; i < colume; i++)
            {
                ColumnDefinition columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(blockSize);
                DynamicGrid.ColumnDefinitions.Add(columnDef);

                Button btn = new Button();
                //Grid.SetColumn(btn);



                //int product = numbers.Aggregate(1, (interim, next) => interim * next);
                //// Add first column header
                //TextBlock txtBlock1 = new TextBlock();
                //txtBlock1.Text = "Author Name";
                //txtBlock1.FontSize = 14;
                //txtBlock1.FontWeight = FontWeights.Bold;
                //txtBlock1.Foreground = new SolidColorBrush(Colors.Green);
                //txtBlock1.VerticalAlignment = VerticalAlignment.Top;
                //Grid.SetRow(txtBlock1, 0);
                //Grid.SetColumn(txtBlock1, 0);
            }

            //Create rows
            for (int i = 0; i < row; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(blockSize);
                DynamicGrid.RowDefinitions.Add(rowDef);

                //// Create first Row
                //TextBlock authorText = new TextBlock();
                //authorText.Text = "Mahesh Chand";
                //authorText.FontSize = 12;
                //authorText.FontWeight = FontWeights.Bold;
                //Grid.SetRow(authorText, 1);
                //Grid.SetColumn(authorText, 0);
                ////gridRow1.Height = new GridLength(45);
            }

 


            //// Add second column header
            //TextBlock txtBlock2 = new TextBlock();
            //txtBlock2.Text = "Age";
            //txtBlock2.FontSize = 14;
            //txtBlock2.FontWeight = FontWeights.Bold;
            //txtBlock2.Foreground = new SolidColorBrush(Colors.Green);
            //txtBlock2.VerticalAlignment = VerticalAlignment.Top;
            //Grid.SetRow(txtBlock2, 0);
            //Grid.SetColumn(txtBlock2, 1);

            //// Add third column header
            //TextBlock txtBlock3 = new TextBlock();
            //txtBlock3.Text = "Book";
            //txtBlock3.FontSize = 14;
            //txtBlock3.FontWeight = FontWeights.Bold;
            //txtBlock3.Foreground = new SolidColorBrush(Colors.Green);
            //txtBlock3.VerticalAlignment = VerticalAlignment.Top;
            //Grid.SetRow(txtBlock3, 0);
            //Grid.SetColumn(txtBlock3, 2);

            ////// Add column headers to the Grid
            //DynamicGrid.Children.Add(txtBlock1);
            //DynamicGrid.Children.Add(txtBlock2);
            //DynamicGrid.Children.Add(txtBlock3);



            //TextBlock ageText = new TextBlock();
            //ageText.Text = "33";
            //ageText.FontSize = 12;
            //ageText.FontWeight = FontWeights.Bold;
            //Grid.SetRow(ageText, 1);
            //Grid.SetColumn(ageText, 1);

            //TextBlock bookText = new TextBlock();
            //bookText.Text = "GDI+ Programming";
            //bookText.FontSize = 12;
            //bookText.FontWeight = FontWeights.Bold;
            //Grid.SetRow(bookText, 1);
            //Grid.SetColumn(bookText, 2);

            //// Add first row to Grid
            //DynamicGrid.Children.Add(authorText);
            //DynamicGrid.Children.Add(ageText);
            //DynamicGrid.Children.Add(bookText);

            //// Create second row
            //authorText = new TextBlock();
            //authorText.Text = "Mike Gold";
            //authorText.FontSize = 12;
            //authorText.FontWeight = FontWeights.Bold;
            //Grid.SetRow(authorText, 2);
            //Grid.SetColumn(authorText, 0);
            //ageText = new TextBlock();
            //ageText.Text = "35";
            //ageText.FontSize = 12;
            //ageText.FontWeight = FontWeights.Bold;
            //Grid.SetRow(ageText, 2);
            //Grid.SetColumn(ageText, 1);
            //bookText = new TextBlock();
            //bookText.Text = "Programming C#";
            //bookText.FontSize = 12;
            //bookText.FontWeight = FontWeights.Bold;
            //Grid.SetRow(bookText, 2);
            //Grid.SetColumn(bookText, 2);

            //// Add second row to Grid
            //DynamicGrid.Children.Add(authorText);
            //DynamicGrid.Children.Add(ageText);
            //DynamicGrid.Children.Add(bookText);

            // Display grid into a Window
            this.Content = DynamicGrid;


        }
    }
}
