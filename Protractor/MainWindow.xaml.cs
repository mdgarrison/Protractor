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
using System.ComponentModel;
using System.Reflection;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double value_Opacity = 0.5;
        double value_BaseLine = 0.0;
        double value_PlumbLine = 0.0;
        double value_AngleIncrement = 0.01;
        public enum OperationEnum
        {
            [Description("Opacity")]
            Op_Opacity,
            [Description("Color")]
            Op_Color,
            [Description("BaseLine")]
            Op_BaseLine,
            [Description("PlumbLine")]
            Op_PlumbLine,
            [Description("Angle Increment")]
            Op_AngleIncrement,
            [Description("Exit")]
            Op_Exit
        };

        public OperationEnum currentOperation = OperationEnum.Op_Opacity;

        public MainWindow()
        {
            InitializeComponent();
            label.Content = GetEnumDescription(currentOperation);
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        private OperationEnum NextOperationEnum(OperationEnum currentValue)
        {
            OperationEnum result = currentValue;
            switch (currentValue)
            {
                case OperationEnum.Op_Opacity:
                    result = OperationEnum.Op_Color;
                    break;
                case OperationEnum.Op_Color:
                    result = OperationEnum.Op_BaseLine;
                    break;
                case OperationEnum.Op_BaseLine:
                    result = OperationEnum.Op_PlumbLine;
                    break;
                case OperationEnum.Op_PlumbLine:
                    result = OperationEnum.Op_AngleIncrement;
                    break;
                case OperationEnum.Op_AngleIncrement:
                    result = OperationEnum.Op_Exit;
                    break;
                case OperationEnum.Op_Exit:
                    result = OperationEnum.Op_Opacity;
                    break;
            }
            return result;
        }

        private string DisplayOpacity()
        {
            string result = String.Format("{0:0.00}", value_Opacity);
            return result;
        }

        private string DisplayBaseLine()
        {
            string result = String.Format("{0:0.00}", value_BaseLine);
            return result;
        }

        private string DisplayPlumbLine()
        {
            string result = String.Format("{0:0.00}", value_PlumbLine);
            return result;
        }

        private string DisplayAngleIncrement()
        {
            string result = String.Format("{0:0.00}", value_AngleIncrement);
            return result;
        }

        private void DisplayOperationText()
        {
            label.Content = GetEnumDescription(currentOperation);
            switch (currentOperation)
            {
                case OperationEnum.Op_Opacity:
                    label2.Content = DisplayOpacity();
                    break;
                case OperationEnum.Op_Color:
                    break;
                case OperationEnum.Op_BaseLine:
                    label2.Content = DisplayBaseLine();
                    break;
                case OperationEnum.Op_PlumbLine:
                    label2.Content = DisplayPlumbLine();
                    break;
                case OperationEnum.Op_AngleIncrement:
                    label2.Content = DisplayAngleIncrement();
                    break;
                case OperationEnum.Op_Exit:
                    break;
            }
        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {

            }
            else if (e.ChangedButton == MouseButton.Right && e.ButtonState == MouseButtonState.Pressed)
            {
                currentOperation = NextOperationEnum(currentOperation);
                label.Content = GetEnumDescription(currentOperation);
            }
            else
            {
                if (currentOperation == OperationEnum.Op_Exit)
                {
                    HandleExitOperation();
                }
                else
                {
                    this.DragMove();
                }
            }
        }

        private void Ellipse_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            HandleOperation(e);
        }

        private void HandleExitOperation()
        {
            Application.Current.Shutdown();
        }

        private void HandleBaseLineOperation(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                value_BaseLine += value_AngleIncrement;
                if (value_BaseLine >= 360.0)
                {
                    value_BaseLine = 0.0;
                }
            }
            else
            {
                value_BaseLine -= value_AngleIncrement;
                if (value_BaseLine <= 0.0)
                {
                    value_BaseLine = 359.0;
                }
            }
            label2.Content = DisplayBaseLine();
        }

        private void HandlePlumbLineOperation(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                value_PlumbLine += value_AngleIncrement;
                if (value_PlumbLine >= 360.0)
                {
                    value_PlumbLine = 0.0;
                }
            }
            else
            {
                value_PlumbLine -= value_AngleIncrement;
                if (value_PlumbLine <= 0.0)
                {
                    value_PlumbLine = 359.0;
                }
            }
            mainCircle.Opacity = value_Opacity;
            label2.Content = DisplayPlumbLine();
        }

        private void HandleAngleIncrementOperation(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                value_AngleIncrement += 0.05;
            }
            else
            {
                value_AngleIncrement -= 0.05;
                if (value_AngleIncrement <= 0.0)
                {
                    value_AngleIncrement = 0.0;
                }
            }
            label2.Content = DisplayAngleIncrement();
        }

        private void HandleOpacityOperation(MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                value_Opacity += 0.05;
                if (value_Opacity >= 1.0)
                {
                    value_Opacity = 0.1;
                }
            }
            else
            {
                value_Opacity -= 0.05;
                if (value_Opacity <= 0.1)
                {
                    value_Opacity = 1.0;
                }
            }
            mainCircle.Opacity = value_Opacity;
            label2.Content = DisplayOpacity();
        }

        private void HandleOperation(MouseWheelEventArgs e)
        {
            switch (currentOperation)
            {
                case OperationEnum.Op_Opacity:
                    HandleOpacityOperation(e);
                    break;
                case OperationEnum.Op_Color:
                    break;
                case OperationEnum.Op_BaseLine:
                    HandleBaseLineOperation(e);
                    break;
                case OperationEnum.Op_PlumbLine:
                    HandlePlumbLineOperation(e);
                    break;
                case OperationEnum.Op_AngleIncrement:
                    HandleAngleIncrementOperation(e);
                    break;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                switch(currentOperation)
                {
                    case OperationEnum.Op_BaseLine:
                        value_BaseLine = 270.0;
                        label2.Content = DisplayBaseLine();
                        break;
                    case OperationEnum.Op_PlumbLine:
                        value_PlumbLine = 270.0;
                        label2.Content = DisplayPlumbLine();
                        break;
                    case OperationEnum.Op_Opacity:
                        value_Opacity = 0.1;
                        mainCircle.Opacity = value_Opacity;
                        label2.Content = DisplayOpacity();
                        break;
                    case OperationEnum.Op_AngleIncrement:
                        value_AngleIncrement = 0.01;
                        label2.Content = DisplayAngleIncrement();
                        break;
                }

            }
            else if (e.Key == Key.Up)
            {
                switch (currentOperation)
                {
                    case OperationEnum.Op_BaseLine:
                        value_BaseLine = 90.0;
                        label2.Content = DisplayBaseLine();
                        break;
                    case OperationEnum.Op_PlumbLine:
                        value_PlumbLine = 90.0;
                        label2.Content = DisplayPlumbLine();
                        break;
                    case OperationEnum.Op_Opacity:
                        value_Opacity = 1.0;
                        mainCircle.Opacity = value_Opacity;
                        label2.Content = DisplayOpacity();
                        break;
                    case OperationEnum.Op_AngleIncrement:
                        value_AngleIncrement = 1.0;
                        label2.Content = DisplayAngleIncrement();
                        break;
                }
            }
            else if (e.Key == Key.Left)
            {
                switch (currentOperation)
                {
                    case OperationEnum.Op_BaseLine:
                        value_BaseLine = 0.0;
                        label2.Content = DisplayBaseLine();
                        break;
                    case OperationEnum.Op_PlumbLine:
                        value_PlumbLine = 0.0;
                        label2.Content = DisplayPlumbLine();
                        break;
                }
            }
            else if (e.Key == Key.Right)
            {
                switch (currentOperation)
                {
                    case OperationEnum.Op_BaseLine:
                        value_BaseLine = 180.0;
                        label2.Content = DisplayBaseLine();
                        break;
                    case OperationEnum.Op_PlumbLine:
                        value_PlumbLine = 180.0;
                        label2.Content = DisplayPlumbLine();
                        break;
                }
            }
            else if (e.Key == Key.O)
            {
                currentOperation = OperationEnum.Op_Opacity;
                DisplayOperationText();
                DisplayOpacity();
            }
            else if (e.Key == Key.B)
            {
                currentOperation = OperationEnum.Op_BaseLine;
                DisplayOperationText();
                DisplayBaseLine();
            }
            else if (e.Key == Key.P)
            {
                currentOperation = OperationEnum.Op_PlumbLine;
                DisplayOperationText();
                DisplayPlumbLine();
            }
            else if (e.Key == Key.A)
            {
                currentOperation = OperationEnum.Op_AngleIncrement;
                DisplayOperationText();
                DisplayAngleIncrement();
            }
            else if (e.Key == Key.E)
            {
                currentOperation = OperationEnum.Op_Exit;
                DisplayOperationText();
                label2.Content = String.Empty;
            }
            else if ((e.Key == Key.Enter) || (e.Key == Key.Return))
            {
                if (currentOperation == OperationEnum.Op_Exit)
                {
                    HandleExitOperation();
                }
            }
        }
    }
}
