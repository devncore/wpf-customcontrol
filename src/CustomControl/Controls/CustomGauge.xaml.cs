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
    /// CustomGauge.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomGauge : UserControl
    {
        //가장 바깥
        double outer_r;
        //게이지
        double gauge_r;
        //긴 스케일 안쪽
        double large_r;
        //중간 스케일
        double tiny_r;

        //캔버스 중앙x
        double cx = 150;
        //캔버스 중앙y
        double cy = 150;

        //const int SecondMark = 3;   // Draw a tiny tick mark every 5 degrees.
        //for문시 눈금 시작위치 Offset(기본 90도)
        const int degreeOffset = 180;

        int gaugeDegree = 180;

        //라벨포함 총 반지름
        private double _Radius = 130;
        public double Radius
        {
            get { return _Radius; }
            set
            {
                _Radius = value;
                Draw();
            }
        }

        //큰 눈금 사이 값
        private int _Mark = 45;
        public int Mark
        {
            get { return _Mark; }
            set
            {
                _Mark = value;
                Draw();
            }
        }

        //작은 눈금 사이 값
        private int _SecondMark = 9;
        public int SecondMark
        {
            get { return _SecondMark; }
            set
            {
                _SecondMark = value;
                Draw();
            }
        }

        public CustomGauge()
        {
            InitializeComponent();
            Draw();
            btn.Click += Btn_Click;
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Draw();
            int degree = 0;

            if (!int.TryParse(tbx.Text, out degree) || degree < -90 || degree > 90)
                return;

            degree += 90;
            int min = 0;
            int max = 0;

            if (degree < 90)
            {
                min = degree;
                max = 90;
            }
            else
            {
                min = 90;
                max = degree;
            }

            for (int i = min; i < max; i++)
            {
                SetGauge(i);
            }
        }

        private void SetGauge(double degree)
        {
            double cos = (double)Math.Cos(DegreesToRadians(degree + degreeOffset));
            double sin = (double)Math.Sin(DegreesToRadians(degree + degreeOffset));

            Line line = new Line();
            line.Stroke = (Brush)new BrushConverter().ConvertFrom("#438AFD");
            line.StrokeThickness = 3;

            double gauge2X1 = cx + large_r * cos;
            double gauge2Y1 = cy + large_r * sin;
            double gauge2X2 = cx + gauge_r * cos;
            double gauge2Y2 = cy + gauge_r * sin;

            line.X1 = gauge2X1;
            line.X2 = gauge2X2;
            line.Y1 = gauge2Y1;
            line.Y2 = gauge2Y2;

            canvas.Children.Add(line);
        }

        private void Draw()
        {
            canvas.Children.Clear();
            SetScale();
        }

        private void DrawFill(double r1, double r2, double degree, string color)
        {
            double cos = (double)Math.Cos(DegreesToRadians(degree + degreeOffset));
            double sin = (double)Math.Sin(DegreesToRadians(degree + degreeOffset));
            double x1 = cx + r1 * cos;
            double y1 = cy + r1 * sin;

            double x2 = cx + r2 * cos;
            double y2 = cy + r2 * sin;

            Line line = new Line();
            line.Stroke = (Brush)new BrushConverter().ConvertFrom(color);
            line.StrokeThickness = 3;

            line.X1 = x1;
            line.X2 = x2;
            line.Y1 = y1;
            line.Y2 = y2;

            canvas.Children.Add(line);
        }

        private void SetScale()
        {
            //가장 바깥
            outer_r = Radius * 0.9;
            //게이지
            gauge_r = outer_r * 0.72;
            //긴 스케일 안쪽
            large_r = outer_r * 0.8;
            //중간 스케일
            tiny_r = outer_r * 0.88;

            //테두리 백그라운드, 게이지 백그라운드
            for (int ii = 0; ii <= gaugeDegree; ii++)
            {
                //테두리
                DrawFill(outer_r, large_r, ii, "#282949");
                //게이지
                DrawFill(large_r, gauge_r, ii, "#2F304D");
            }

            Brush color = (Brush)new BrushConverter().ConvertFrom("#777A9F");

            //스케일(눈금)
            for (int i = 0; i <= gaugeDegree; i += SecondMark)
            {
                double cos = (double)Math.Cos(DegreesToRadians(i + degreeOffset));
                double sin = (double)Math.Sin(DegreesToRadians(i + degreeOffset));
                double x2 = cx + large_r * cos;
                double y2 = cy + large_r * sin;


                Line line = new Line();
                line.Stroke = color;
                line.StrokeThickness = 1.5;

                double x1, y1;

                if (i % Mark == 0)
                {
                    x1 = cx + outer_r * cos;

                    y1 = cy + outer_r * sin;

                    double labelX = cx + Radius * cos;
                    double labeY = cy + Radius * sin;
                    //라벨
                    SetLabel(i, labelX, labeY);
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
            }
        }

        private void SetLabel(int degree, double label_x, double label_y)
        {
            Brush color = (Brush)new BrushConverter().ConvertFrom("#777A9F");

            Label label = new Label();
            label.Content = (degree - 90).ToString();
            label.Foreground = color;
            Canvas.SetLeft(label, label_x - XLengthToInt((degree - 90).ToString()));
            Canvas.SetTop(label, label_y - 13);
            canvas.Children.Add(label);
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }

        private int XLengthToInt(string value)
        {
            int returnValue = 0;

            if (value.Length == 1)
                returnValue = 8;
            else
                returnValue = 14;

            return returnValue;
        }
    }
}
