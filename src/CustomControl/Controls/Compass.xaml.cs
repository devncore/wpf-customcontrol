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

namespace CustomControl.Controls
{
    /// <summary>
    /// Compass.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Compass : UserControl
    {
        private Line needle;
        private Line needle2;
        private int angle = 0;
        public Compass()
        {
            InitializeComponent();
            DrawCircle();
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            DrawCircle();
            RotateTransform rotate = new RotateTransform(angle, needle.X1, needle.Y1);
            needle.RenderTransform = rotate;
            angle += 30;

            if (angle == 360)
                angle = 0;
        }

        private void DrawCircle()
        {
            canvas.Children.Clear();
            double cx = canvas.Width / 2;
            double cy = canvas.Height / 2;

            double letter_r = 130;

            const int large_tick_freq = 45; // Draw a large tick mark every 45 degrees.
            const int tiny_tick_freq = 5;   // Draw a tiny tick mark every 5 degrees.
            const int degreeOffset = 270;
            double outer_r = letter_r * 0.9;
            double large_r = outer_r * 0.8;
            double tiny_r = outer_r * 0.88;

            needle = new Line();

            needle.Stroke = Brushes.Red;
            needle.StrokeThickness = 3;
            needle.X1 = cx;
            needle.X2 = cx + 30;
            needle.Y1 = cy;
            needle.Y2 = cy + 30;

            canvas.Children.Add(needle);

            for (int ii = 0; ii < 360; ii++)
            {
                double cos = (double)Math.Cos(DegreesToRadians(ii + degreeOffset));
                double sin = (double)Math.Sin(DegreesToRadians(ii + degreeOffset));
                double x1 = cx + outer_r * cos;
                double y1 = cy + outer_r * sin;

                double x2 = cx + large_r * cos;
                double y2 = cy + large_r * sin;

                Line line = new Line();
                line.Stroke = (Brush)new BrushConverter().ConvertFrom("#282949");
                line.StrokeThickness = 3;

                line.X1 = x1;
                line.X2 = x2;
                line.Y1 = y1;
                line.Y2 = y2;
                canvas.Children.Add(line);
            }

            Brush color = (Brush)new BrushConverter().ConvertFrom("#777A9F");


            for (int i = tiny_tick_freq; i <= 360; i += tiny_tick_freq)
            {

                double cos = (double)Math.Cos(DegreesToRadians(i + degreeOffset));
                double sin = (double)Math.Sin(DegreesToRadians(i + degreeOffset));
                double x2 = cx + large_r * cos;
                double y2 = cy + large_r * sin;

                Line line = new Line();
                line.Stroke = color;
                line.StrokeThickness = 1.5;

                double x1, y1;

                if (i % large_tick_freq == 0)
                {
                    x1 = cx + outer_r * cos;
                    y1 = cy + outer_r * sin;
                    Label label = new Label();
                    label.Content = (DegreeType)i;
                    label.Foreground = color;
                    Canvas.SetLeft(label, x2);
                    Canvas.SetTop(label, y2);
                    canvas.Children.Add(label);
                }
                else
                {
                    x1 = cx + tiny_r * cos;
                    y1 = cx + tiny_r * sin;
                }

                line.X1 = x1;
                line.X2 = x2;
                line.Y1 = y1;
                line.Y2 = y2;

                canvas.Children.Add(line);

                Console.WriteLine($"[{i}]" + line.X1 + ", " + line.X2 + ", " + line.Y1 + ", " + line.Y2);
            }
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
