using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Simple.TicTacToe.MouseLocation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        BitmapImage X_CROSS;
        BitmapImage O_CIRCLE;

        Color lineColor = Colors.Black;
        int GRID_SIZE = 3;
        // X indicates the horizontal position
        const int startX = 30;
        // Y indicates the vertical position
        const int startY = 60;

        int isXTurn;
        int countUsedSquares;
        int totalSquares;
        int first_index_of_img;

        int[,] _mat = new int[10, 10];

        const int SQUARE_WIDTH = 100;
        const int SQUARE_HEIGHT = 100;
        const int STROKE_THICKNESS = 2;
        


        const int WinCondition = 3;
        List<Tuple<int, int>> ORIENT_HORIZONTAL;
        List<Tuple<int, int>> ORIENT_VERTICAL;
        List<Tuple<int, int>> ORIENT_SW_NE;
        List<Tuple<int, int>> ORIENT_NW_SE;

        List<int> sizeofGrid;
        private bool isLoading;

        private void DrawVerticalLine(int index, int sq_width, int sq_height)
        {
            var line = new Line();
            line.StrokeThickness = STROKE_THICKNESS;
            line.Stroke = new SolidColorBrush(lineColor);
            canvas.Children.Add(line);

            line.X1 = startX + index * sq_width;
            line.Y1 = startY;

            line.X2 = startX + index * sq_width;
            line.Y2 = startY + GRID_SIZE * sq_height;
        }

        private void DrawHorizontalLine(int index, int sq_width, int sq_height)
        {
            var line = new Line();
            line.StrokeThickness = STROKE_THICKNESS;
            line.Stroke = new SolidColorBrush(lineColor);
            canvas.Children.Add(line);

            line.X1 = startX;
            line.Y1 = startY + index * sq_height;

            line.X2 = startX + GRID_SIZE * sq_width;
            line.Y2 = startY + index * sq_height;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            X_CROSS = new BitmapImage(
                             new Uri("clear.png", UriKind.Relative));
            O_CIRCLE = new BitmapImage(
                             new Uri("circle.png", UriKind.Relative));
            isXTurn = 1;
            countUsedSquares = 0;
            totalSquares = GRID_SIZE * GRID_SIZE;
            sizeofGrid = new List<int>();
            first_index_of_img = -1;
            isLoading = false;

            for (int i = 3; i <= 6; i++)
            {
                sizeofGrid.Add(i);
            }

            labelStatus.Foreground = Brushes.Blue;
            labelStatus.Content = "X Turn";

            //comboBoxGridSize.ItemsSource = sizeofGrid;


            //// Line 1
            //{
            //    var line1 = new Line();
            //    line1.StrokeThickness = 1;
            //    line1.Stroke = new SolidColorBrush(Colors.Red);
            //    canvas.Children.Add(line1);

            //    line1.X1 = startX + SQUARE_WIDTH;
            //    line1.Y1 = startY;

            //    line1.X2 = startX + SQUARE_WIDTH;
            //    line1.Y2 = startY + 3 * SQUARE_HEIGHT;
            //}

            //// Line 2
            //{
            //    var line2 = new Line();
            //    line2.StrokeThickness = 1;
            //    line2.Stroke = new SolidColorBrush(Colors.Red);
            //    canvas.Children.Add(line2);

            //    line2.X1 = startX + 2 * SQUARE_WIDTH;
            //    line2.Y1 = startY;

            //    line2.X2 = startX + 2 * SQUARE_WIDTH;
            //    line2.Y2 = startY + 3 * SQUARE_HEIGHT;
            //}

            //// Line 3
            //{
            //    var line3 = new Line();
            //    line3.StrokeThickness = 1;
            //    line3.Stroke = new SolidColorBrush(Colors.Red);
            //    canvas.Children.Add(line3);

            //    line3.X1 = startX;
            //    line3.Y1 = startY + SQUARE_HEIGHT;

            //    line3.X2 = startX + 3 * SQUARE_WIDTH;
            //    line3.Y2 = startY + SQUARE_HEIGHT;
            //}

            //// Line 4
            //{
            //    var line4 = new Line();
            //    line4.StrokeThickness = 1;
            //    line4.Stroke = new SolidColorBrush(Colors.Red);
            //    canvas.Children.Add(line4);

            //    line4.X1 = startX;
            //    line4.Y1 = startY + 2 * SQUARE_HEIGHT;

            //    line4.X2 = startX + 3 * SQUARE_WIDTH;
            //    line4.Y2 = startY + 2 * SQUARE_HEIGHT;
            //}

            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    _mat[i, j] = 0;
                }
            }

            for (int i = 0; i <= GRID_SIZE; i++)
            {
                DrawHorizontalLine(i, SQUARE_WIDTH, SQUARE_HEIGHT);
            }

            for (int j = 0; j <= GRID_SIZE; j++)
            {
                DrawVerticalLine(j, SQUARE_WIDTH, SQUARE_HEIGHT);
            }

            ORIENT_HORIZONTAL = new List<Tuple<int, int>>();
            ORIENT_HORIZONTAL.Add(new Tuple<int, int>(0, -1));
            ORIENT_HORIZONTAL.Add(new Tuple<int, int>(0, 1));

            ORIENT_VERTICAL = new List<Tuple<int, int>>();
            ORIENT_VERTICAL.Add(new Tuple<int, int>(-1, 0));
            ORIENT_VERTICAL.Add(new Tuple<int, int>(1, 0));

            ORIENT_SW_NE = new List<Tuple<int, int>>();
            ORIENT_SW_NE.Add(new Tuple<int, int>(1, -1));
            ORIENT_SW_NE.Add(new Tuple<int, int>(-1, 1));

            ORIENT_NW_SE = new List<Tuple<int, int>>();
            ORIENT_NW_SE.Add(new Tuple<int, int>(-1, -1));
            ORIENT_NW_SE.Add(new Tuple<int, int>(1, 1));

            /*
            var screen = new OpenFileDialog();
            screen.Title = "Choose an image to set background";

            if (screen.ShowDialog() == true)
            {

                var source = new BitmapImage(
                    new Uri(screen.FileName, UriKind.Absolute));
                Debug.WriteLine($"{source.Width} - {source.Height}");
                previewImage.Width = 300;
                previewImage.Height = 300;
                previewImage.Source = source;

                Canvas.SetLeft(previewImage, 0);
                Canvas.SetTop(previewImage, 0);

                // Bat dau cat thanh 9 manh

                for (int i = 0; i < GRID_SIZE; i++)
                {
                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        if (!((i == 2) && (j == 2)))
                        {
                            var h = (int)source.Height;
                            var w = (int)source.Width;
                            //Debug.WriteLine($"Len = {len}");
                            var rect = new Int32Rect(j * w, i * h, w, h);
                            var cropBitmap = new CroppedBitmap(source,
                                rect);

                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = SQUARE_WIDTH;
                            cropImage.Height = SQUARE_HEIGHT;
                            cropImage.Source = cropBitmap;
                            canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + j * (SQUARE_WIDTH + 2));
                            Canvas.SetTop(cropImage, startY + i * (SQUARE_HEIGHT + 2));

                            cropImage.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
                            cropImage.PreviewMouseLeftButtonUp += CropImage_PreviewMouseLeftButtonUp;
                            cropImage.Tag = new Tuple<int, int>(i, j);
                            //cropImage.MouseLeftButtonUp
                        }
                    }
                }
            }*/
        }

        //bool _isDragging = false;
        //Image _selectedBitmap = null;
        //Point _lastPosition;

        //private void CropImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        //{
        //    _isDragging = false;
        //    var position = e.GetPosition(this);

        //    int x = (int)(position.X - startX) / (SQUARE_WIDTH + 2) * (SQUARE_WIDTH + 2) + startX;
        //    int y = (int)(position.Y - startY) / (SQUARE_HEIGHT + 2) * (SQUARE_HEIGHT + 2) + startY;

        //    Canvas.SetLeft(_selectedBitmap, x);
        //    Canvas.SetTop(_selectedBitmap, y);

        //    var image = sender as Image;
        //    var (i, j) = image.Tag as Tuple<int, int>;

        //    MessageBox.Show($"{i} - {j}");
        //}

        //private void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    _isDragging = true;
        //    _selectedBitmap = sender as Image;
        //    _lastPosition = e.GetPosition(this);
        //}

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / SQUARE_HEIGHT;
            int j = ((int)position.X - startX) / SQUARE_WIDTH;

            mouseCoordinate.Content = $"{position.X} - {position.Y}, a[{i}][{j}]";

            //if (_isDragging)
            //{
            //    var dx = position.X - _lastPosition.X;
            //    var dy = position.Y - _lastPosition.Y;

            //    var lastLeft = Canvas.GetLeft(_selectedBitmap);
            //    var lastTop = Canvas.GetTop(_selectedBitmap);
            //    Canvas.SetLeft(_selectedBitmap, lastLeft + dx);
            //    Canvas.SetTop(_selectedBitmap, lastTop + dy);

            //    _lastPosition = position;
            //}
        }

        private void ClickOnSquare(int i, int j, int X_Clicked)
        {
            bool toClearSquare = false;
            Image sq = null;
            if (isLoading && first_index_of_img > -1)
            {
                for (int k = first_index_of_img; k < canvas.Children.Count; k++)
                {
                    sq = canvas.Children[k] as Image;
                    
                    var (i1, j1) = sq.Tag as Tuple<int, int>;
                    if (i == i1 && j == j1)
                    {
                        toClearSquare = true;
                        break;
                    }
                }
            }

            if (toClearSquare == true)
            {
                switch (X_Clicked)
                {
                    case 1:
                        sq.Source = X_CROSS;
                        break;
                    case 2:
                        sq.Source = O_CIRCLE;
                        break;
                    default: // == 0
                        sq.Source = null;
                        break;
                }
            }
            else
            {
                var img = new Image
                {
                    Width = SQUARE_WIDTH - 3,
                    Height = SQUARE_HEIGHT - 3,
                    Tag = new Tuple<int, int>(i, j)
                };

                switch (X_Clicked)
                {
                    case 1:
                        img.Source = X_CROSS;
                        break;
                    case 2:
                        img.Source = O_CIRCLE;
                        break;
                }

                canvas.Children.Add(img);
                if (first_index_of_img == -1)
                    first_index_of_img = canvas.Children.IndexOf(img);

                Canvas.SetLeft(img, startX + j * SQUARE_WIDTH + 2);
                Canvas.SetTop(img, startY + i * SQUARE_HEIGHT + 2);
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / SQUARE_HEIGHT;
            int j = ((int)position.X - startX) / SQUARE_WIDTH;

            mouseCoordinate.Content = $"{position.X} - {position.Y}, a[{i}][{j}]";

            if (i < GRID_SIZE && j < GRID_SIZE
                && _mat[i, j] == 0)
            {
                if (isXTurn == 1)
                {
                    labelStatus.Foreground = Brushes.Red;
                    labelStatus.Content = "O Turn";
                }
                else
                {
                    labelStatus.Foreground = Brushes.Blue;
                    labelStatus.Content = "X Turn";
                }

                countUsedSquares++;

                ClickOnSquare(i, j, isXTurn);
                if (isXTurn == 1)
                {
                    _mat[i, j] = 1;
                }
                else
                {
                    _mat[i, j] = 2;
                }

                var gameOver = checkWin(i, j);

                if (gameOver)
                {
                    if (isXTurn == 1)
                    {
                        MessageBox.Show("X won!", "Game Over!");
                    }
                    else
                    {
                        MessageBox.Show("O won!", "Game Over!");
                    }
                }
                else
                {
                    if (countUsedSquares == totalSquares)
                    {
                        MessageBox.Show("Draw!", "Game Over!");
                        return;
                    }
                    isXTurn = isXTurn % 2 + 1;
                }

                //var img = new Image
                //{
                //    Width = SQUARE_WIDTH - 3,
                //    Height = SQUARE_HEIGHT - 3,
                //    Source = new BitmapImage(
                //    new Uri("circle.png", UriKind.Relative))
                //};
                //canvas.Children.Add(img);

                //Canvas.SetLeft(img, startX + j * SQUARE_WIDTH + 2);
                //Canvas.SetTop(img, startY + i * SQUARE_HEIGHT + 2);
            }
        }

        private bool checkWin(int i, int j)
        {
            var gameOver = false; // false - draw, true - somebody won

            gameOver = checkWinOrientation(ORIENT_HORIZONTAL, new Tuple<int, int>(i, j),
                new Tuple<int, int>(-1, -1), new Tuple<int, int>(GRID_SIZE, GRID_SIZE));
            if (gameOver == true)
            {
                return true;
            }

            gameOver = checkWinOrientation(ORIENT_VERTICAL, new Tuple<int, int>(i, j),
                new Tuple<int, int>(-1, -1), new Tuple<int, int>(GRID_SIZE, GRID_SIZE));

            if (gameOver == true)
            {
                return true;
            }

            gameOver = checkWinOrientation(ORIENT_SW_NE, new Tuple<int, int>(i, j),
               new Tuple<int, int>(GRID_SIZE, -1), new Tuple<int, int>(-1, GRID_SIZE));

            if (gameOver == true)
            {
                return true;
            }

            gameOver = checkWinOrientation(ORIENT_NW_SE, new Tuple<int, int>(i, j),
               new Tuple<int, int>(-1, -1), new Tuple<int, int>(GRID_SIZE, GRID_SIZE));

            if (gameOver == true)
            {
                return true;
            }

            return gameOver;
        }

        private bool checkWinOrientation(List<Tuple<int, int>> orientation, Tuple<int, int> curPos,
            Tuple<int, int> lowerBound, Tuple<int, int> upperBound)
        {
            int count = 1;
            var (i, j) = curPos;
            int curI, curJ;
            int di, dj;

            (curI, curJ) = (i, j);
            (di, dj) = orientation[0];

            while (curI + di != lowerBound.Item1 &&
                curJ + dj != lowerBound.Item2)
            {
                curI += di;
                curJ += dj;
                if (_mat[i, j] == _mat[curI, curJ])
                {
                    count++;
                }
                else
                    break;
            }

            (curI, curJ) = (i, j);
            (di, dj) = orientation[1];

            while (curI + di != upperBound.Item1 &&
                curJ + dj != upperBound.Item2)
            {
                curI += di;
                curJ += dj;
                if (_mat[i, j] == _mat[curI, curJ])
                {
                    count++;
                }
                else
                    break;
            }

            return (count >= WinCondition);
        }

        private void previewImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //var animation = new DoubleAnimation();
            //animation.From = 200;
            //animation.To = 300;
            //animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            //animation.AutoReverse = true;
            //animation.RepeatBehavior = RepeatBehavior.Forever;


            //var story = new Storyboard();
            //story.Children.Add(animation);
            //Storyboard.SetTargetName(animation, previewImage.Name);
            //Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            //story.Begin(this);
        }

        private void SaveMenu_Click(object sender, RoutedEventArgs e)
        {
            //StreamReader reader = new StreamReader(filename);
            //reader.Read();
            string filename;
            var screen = new SaveFileDialog();
            if (screen.ShowDialog() == true)
            {
                filename = screen.FileName;


                var writer = new StreamWriter(filename);
                // Dong dau tien la luot di hien tai
                writer.WriteLine((isXTurn == 1) ? "X" : "O");

                // Theo sau la ma tran bieu dien game
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        writer.Write($"{_mat[i, j]}");
                        if (j < GRID_SIZE - 1)
                        {
                            writer.Write(" ");
                        }
                    }
                    writer.WriteLine("");
                }

                writer.Close();
                MessageBox.Show("Game is saved!", "Saving");
            }
            else
            {
                MessageBox.Show("Game is not saved!", "Saving");
            }
        }

        private void LoadMenu_Click(object sender, RoutedEventArgs e)
        {
            isLoading = true;
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;
                countUsedSquares = 0;
                var reader = new StreamReader(filename);
                var firstLine = reader.ReadLine();
                if (firstLine == "X")
                    isXTurn = 1;
                else
                    isXTurn = 2;

                for (int i = 0; i < GRID_SIZE; i++)
                {
                    var tokens = reader.ReadLine().Split(
                        new string[] { " " }, StringSplitOptions.None);
                    // Model

                    for (int j = 0; j < GRID_SIZE; j++)
                    {
                        _mat[i, j] = int.Parse(tokens[j]);

                        if (_mat[i, j] == 0)
                        {
                            ClickOnSquare(i, j, 0);
                        }
                        else
                        {
                            ClickOnSquare(i, j, _mat[i, j]);
                            countUsedSquares++;
                        }
                    }
                }
                MessageBox.Show("Game is loaded");
            }
            else
            {
                MessageBox.Show("Game is not loaded!", "Loading");
            }
            isLoading = false;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            isXTurn = 1;
            countUsedSquares = 0;
            for (int i = 0; i < GRID_SIZE; i++)
            {
                for (int j = 0; j < GRID_SIZE; j++)
                {
                    _mat[i, j] = 0;
                }
            }
            for (int k = first_index_of_img; k < canvas.Children.Count; k++)
            {
                var sq = canvas.Children[k] as Image;
                sq.Source = null;
            }
        }
    }
}
