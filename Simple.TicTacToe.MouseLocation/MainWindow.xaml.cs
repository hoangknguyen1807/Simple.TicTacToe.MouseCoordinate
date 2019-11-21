using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        const int startX = 30;
        const int startY = 30;
        const int width = 50;
        const int height = 50;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


            var line1 = new Line();
            line1.StrokeThickness = 1;
            line1.Stroke = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(line1);

            line1.X1 = startX + width;
            line1.Y1 = startY;

            line1.X2 = startX + width;
            line1.Y2 = startY + 3 * height;

            var line2 = new Line();
            line2.StrokeThickness = 1;
            line2.Stroke = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(line2);

            line2.X1 = startX + 2 * width;
            line2.Y1 = startY;

            line2.X2 = startX + 2 * width;
            line2.Y2 = startY + 3 * height;

            var line3 = new Line();
            line3.StrokeThickness = 1;
            line3.Stroke = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(line3);

            line3.X1 = startX;
            line3.Y1 = startY + height;

            line3.X2 = startX + 3 * width;
            line3.Y2 = startY + height;

            var line4 = new Line();
            line4.StrokeThickness = 1;
            line4.Stroke = new SolidColorBrush(Colors.Red);
            canvas.Children.Add(line4);

            line4.X1 = startX;
            line4.Y1 = startY + 2 * height;

            line4.X2 = startX + 3 * width;
            line4.Y2 = startY + 2 * height;

            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {

                var source = new BitmapImage(
                    new Uri(screen.FileName, UriKind.Absolute));
                Debug.WriteLine($"{source.Width} - {source.Height}");
                previewImage.Width = 300;
                previewImage.Height = 240;
                previewImage.Source = source;

                Canvas.SetLeft(previewImage, 200);
                Canvas.SetTop(previewImage, 20);

                // Bat dau cat thanh 9 manh

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!((i == 2) && (j == 2)))
                        {
                            var h = (int)source.Height;
                            var w = (int)source.Height;
                            //Debug.WriteLine($"Len = {len}");
                            var rect = new Int32Rect(j * w, i * h, w, h);
                            var cropBitmap = new CroppedBitmap(source,
                                rect);

                            var cropImage = new Image();
                            cropImage.Stretch = Stretch.Fill;
                            cropImage.Width = width;
                            cropImage.Height = height;
                            cropImage.Source = cropBitmap;
                            canvas.Children.Add(cropImage);
                            Canvas.SetLeft(cropImage, startX + j * (width + 2));
                            Canvas.SetTop(cropImage, startY + i * (height + 2));

                            cropImage.MouseLeftButtonDown += CropImage_MouseLeftButtonDown;
                            cropImage.PreviewMouseLeftButtonUp += CropImage_PreviewMouseLeftButtonUp;
                            cropImage.Tag = new Tuple<int, int>(i, j);
                            //cropImage.MouseLeftButtonUp
                        }
                    }
                }



            }
        }

        bool _isDragging = false;
        Image _selectedBitmap = null;
        Point _lastPosition;
        private void CropImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            var position = e.GetPosition(this);

            int x = (int)(position.X - startX) / (width + 2) * (width + 2) + startX;
            int y = (int)(position.Y - startY) / (height + 2) * (height + 2) + startY;

            Canvas.SetLeft(_selectedBitmap, x);
            Canvas.SetTop(_selectedBitmap, y);

            var image = sender as Image;
            var (i, j) = image.Tag as Tuple<int, int>;

            MessageBox.Show($"{i} - {j}");
        }

        private void CropImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _selectedBitmap = sender as Image;
            _lastPosition = e.GetPosition(this);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / height;
            int j = ((int)position.X - startX) / width;

            this.Title = $"{position.X} - {position.Y}, a[{i}][{j}]";

            if (_isDragging)
            {
                var dx = position.X - _lastPosition.X;
                var dy = position.Y - _lastPosition.Y;

                var lastLeft = Canvas.GetLeft(_selectedBitmap);
                var lastTop = Canvas.GetTop(_selectedBitmap);
                Canvas.SetLeft(_selectedBitmap, lastLeft + dx);
                Canvas.SetTop(_selectedBitmap, lastTop + dy);

                _lastPosition = position;
            }
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            int i = ((int)position.Y - startY) / height;
            int j = ((int)position.X - startX) / width;

            this.Title = $"{position.X} - {position.Y}, a[{i}][{j}]";

            var img = new Image();
            img.Width = 30;
            img.Height = 30;
            img.Source = new BitmapImage(
                new Uri("circle.png", UriKind.Relative));
            canvas.Children.Add(img);

            Canvas.SetLeft(img, startX + j * width);
            Canvas.SetTop(img, startY + i * height);
        }

        private void previewImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var animation = new DoubleAnimation();
            animation.From = 200;
            animation.To = 300;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1));
            animation.AutoReverse = true;
            animation.RepeatBehavior = RepeatBehavior.Forever;


            var story = new Storyboard();
            story.Children.Add(animation);
            Storyboard.SetTargetName(animation, previewImage.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Canvas.LeftProperty));
            story.Begin(this);
        }
    }
}
