using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace WpfApp1;

public partial class MainWindow : Window
{
    private const int motorBoundary = 10;
    private const double OFFSET = 0.02;
    private int X = default;
    private int Y = default;
    private int Z = default;
    private TranslateTransform3D? transform3D;

    MainTable table;
    public MainWindow()
    {
        InitializeComponent();
        table = new MainTable();
    }

    private static int GetConcreteValue(int v, double max)
    {
        int res = 0;
        if (v > 0)
        {
            res = v / (int)((max / 2) / motorBoundary);
        }

        return res;
    }

    private async void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var maxX = viewport.ActualWidth;
        var maxY = viewport.ActualHeight;
        var maxZ = viewport.ActualWidth;
        Point mousePosition = e.GetPosition(viewport);
        Point pointToScreen = PointToScreen(mousePosition);
        double x = (pointToScreen.X);
        double y = (pointToScreen.Y);
        var actX = GetConcreteValue((int)x, maxX);
        if (IsValidMotorBoundary(actX))
        {
            await MoveWithDelay(actX, 1);
        }

        var actY = GetConcreteValue((int)y, maxY);
        if (IsValidMotorBoundary(actY))
        {
            await MoveWithDelay(actY, 2);
        }

        var actZ = GetConcreteValue((int)y, maxZ);
        if (IsValidMotorBoundary(actZ))
        {
            await MoveWithDelay(actZ, 3);
        }
    }

    private async void MoveX_Click(object sender, RoutedEventArgs e)
    {
        movex.IsEnabled = false;
        var pos = moveXValue.Text;
        _ = int.TryParse(pos, out var destPos);
        await MoveWithDelay(destPos, 1);
        movex.IsEnabled = true;
    }

    private async void MoveY_Click(object sender, RoutedEventArgs e)
    {
        movey.IsEnabled = false;
        var pos = moveYValue.Text;
        _ = int.TryParse(pos, out var destPos);
        await MoveWithDelay(destPos, 2);
        movey.IsEnabled = true;
    }

    private async void MoveZ_Click(object sender, RoutedEventArgs e)
    {
        movez.IsEnabled = false;
        var pos = moveZValue.Text;
        _ = int.TryParse(pos, out var destPos);
        await MoveWithDelay(destPos, 3);
        movez.IsEnabled = true;
    }

    private async Task MoveWithDelay(int destPos, int axis)
    {
        if (axis == 1 && IsValidMotorBoundary(destPos) && X != destPos)
        {
            await Animate(axis, destPos);
            X = destPos;
            xstate.Text = destPos.ToString();
        }
        else if (axis == 2 && IsValidMotorBoundary(destPos) && Y != destPos)
        {
            await Animate(axis, destPos);
            Y = destPos;
            ystate.Text = destPos.ToString();
        }
        else if (axis == 3 && IsValidMotorBoundary(destPos) && Z != destPos)
        {
            await Animate(axis, destPos);
            Z = destPos;
            zstate.Text = destPos.ToString();
        }
        else if (!IsValidMotorBoundary(destPos))
        {
            MessageBox.Show("Not valid motor boundary on axis x/y/z!");
            return;
        }
        else
        {
        }
    }

    private async Task Animate(int axis, int destPos)
    {
        if (axis == 1)
            await MoveByAxis(axis, destPos, X);
        if (axis == 2)
            await MoveByAxis(axis, destPos, Y);
        if (axis == 3)
            await MoveByAxis(axis, destPos, Z);
    }

    private async Task MoveByAxis(int axis, int destPos, int currVal)
    {
        if (currVal < destPos)
        {
            for (int i = currVal + 1; i <= destPos; i++)
            {
                await Move(axis, direction: 1);
            }
        }
        else if (destPos < currVal)
        {
            for (int i = currVal - 1; i >= destPos; i--)
            {
                await Move(axis, direction: -1);
            }
        }
    }

    private async Task Move(int axis, int direction)
    {
        for (int j = 1; j < 6; j++)
        {
            transform3D = new()
            {
                OffsetX = axis == 1 ? direction * (j * OFFSET) : 0,
                OffsetY = axis == 2 ? direction * (j * OFFSET) : 0,
                OffsetZ = axis == 3 ? direction * (j * OFFSET) : 0
            };
            table.Transform(transform3D);
            mainTable.Positions = table.Point3Ds;
            await Task.Delay(GetDelay());
        }
    }
    private static bool IsValidMotorBoundary(int destPos)
    {
        return
            (destPos >= 0 && destPos <= motorBoundary) ||
            (destPos <= 0 && destPos >= -1 * motorBoundary);
    }

    private void CheckNumberContent(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new("[0-9-]+");
        e.Handled = !regex.IsMatch(e.Text);
    }

    private TimeSpan GetDelay()
    {
        var speed = motorsSpeed.Value;
        var ret = speed switch
        {
            1 => 100,
            2 => 80,
            3 => 60,
            4 => 40,
            5 => 20,
            _ => 10
        };

        return TimeSpan.FromMilliseconds(ret);
    }
}
