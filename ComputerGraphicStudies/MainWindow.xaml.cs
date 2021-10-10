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

namespace ComputerGraphicStudies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Globalne obiekty.
        Point start;
        Shape newestShape;
        bool mouseLeftButtonClicked;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(canvas);
            mousePosition.Content = $"({pt.X:N0}, {pt.Y:N0})";

            if (mouseLeftButtonClicked)
            {
                if (newestShape is Line)
                {
                    Line line = newestShape as Line;
                    line.X1 = start.X;
                    line.Y1 = start.Y;
                    line.X2 = pt.X;
                    line.Y2 = pt.Y;
                }
                else
                {
                    newestShape.SetValue(Canvas.TopProperty, Math.Min(pt.Y, start.Y));
                    newestShape.SetValue(Canvas.LeftProperty, Math.Min(pt.X, start.X));
                    newestShape.Width = Math.Abs(pt.X - start.X);
                    newestShape.Height = Math.Abs(pt.Y - start.Y);
                }
            }
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(canvas);
            Shape shape;

            switch (shapeType.SelectedValue)
            {
                case "Prostokąt":
                    shape = new Rectangle();
                    break;
                case "Elipsa":
                    shape = new Ellipse();
                    break;
                default:
                    shape = new Line();
                    break;
            }

            shape.Fill = new SolidColorBrush(Colors.Pink);
            shape.Stroke = new SolidColorBrush(Colors.Red);
            shape.StrokeThickness = 3;
            if (!(shape is Line))
            {
                shape.SetValue(Canvas.LeftProperty, start.X);
                shape.SetValue(Canvas.TopProperty, start.Y);
            }

            newestShape = shape;
            start = e.GetPosition(canvas);
            canvas.Children.Add(shape);
            mouseLeftButtonClicked = true;
        }

        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseLeftButtonClicked = false;
            Mouse.Capture(null);
        }
    }
}
