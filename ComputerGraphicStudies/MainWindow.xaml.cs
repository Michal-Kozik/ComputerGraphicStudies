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

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
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

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

            if (newestShape != null)
            {
                newestShape.StrokeThickness = 1;
            }
            newestShape = shape;
            start = e.GetPosition(canvas);
            shape.MouseLeftButtonDown += Shape_MouseLeftButtonDown;
            shape.MouseMove += Shape_MouseMove;
            canvas.Children.Add(shape);
            mouseLeftButtonClicked = true;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mouseLeftButtonClicked = false;
            Mouse.Capture(null);
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            Point dropPosition = e.GetPosition(canvas);
            if (newestShape is Line)
            {
                Line line = newestShape as Line;
                double x2 = dropPosition.X - line.X1;
                double y2 = dropPosition.Y - line.Y1;
                line.X1 = dropPosition.X;
                line.Y1 = dropPosition.Y;
                line.X2 += x2;
                line.Y2 += y2;
            }
            else
            {
                Canvas.SetLeft(newestShape, dropPosition.X);
                Canvas.SetTop(newestShape, dropPosition.Y);
            }
        }

        private void EnableMoving(object sender, RoutedEventArgs e)
        {
            canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
            canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
        }

        private void DisableMoving(object sender, RoutedEventArgs e)
        {
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
        }

        private void CreateShape(object sender, RoutedEventArgs e)
        {
            //dodac warunek czy inputy sa puste.
            // Linia nie jest rysowana.
            Shape shape;
            double x1 = Double.Parse(x1Input.Text);
            double y1 = Double.Parse(y1Input.Text);
            double x2 = Double.Parse(x2Input.Text);
            double y2 = Double.Parse(y2Input.Text);

            switch (shapeType.SelectedValue)
            {
                case "Prostokąt":
                    shape = new Rectangle();
                    shape.Name = "Rectangle";
                    break;
                case "Elipsa":
                    shape = new Ellipse();
                    shape.Name = "Ellipse";
                    break;
                default:
                    shape = new Line();
                    shape.Name = "Line";
                    break;
            }

            shape.Fill = new SolidColorBrush(Colors.Pink);
            shape.Stroke = new SolidColorBrush(Colors.Red);
            shape.StrokeThickness = 3;
            if (!(shape is Line))
            {
                shape.SetValue(Canvas.LeftProperty, Double.Parse(x1Input.Text));
                shape.SetValue(Canvas.TopProperty, Double.Parse(y1Input.Text));
                shape.Width = x2 - x1;
                shape.Height = y2 - y1;
            }
            else
            {
                Line line = shape as Line;
                line.X1 = x1;
                line.Y1 = y1;
                line.X2 = x2;
                line.Y2 = y2;
            }

            newestShape = shape;
            canvas.Children.Add(shape);
        }

        private void Shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (newestShape != null)
            {
                newestShape.StrokeThickness = 1;
            }
            newestShape = sender as Shape;
            newestShape.StrokeThickness = 3;
        }

        private void Shape_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(newestShape, newestShape, DragDropEffects.Move);
            }
        }
    }
}
