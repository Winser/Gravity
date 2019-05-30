using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gravity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer UpdateTimer;
        private Space space;
        private DrawParams drawParams;
        private DateTime lastTime;
        public MainWindow()
        {
            InitializeComponent();
            space = new Space();
            drawParams = new DrawParams();
            CheckBox_Speed.IsChecked = drawParams.DrawSpeed;
            CheckBox_Axel.IsChecked = drawParams.DrawAxel;
            TextBox_Speed.Text = space.DefaultSpeed.ToString();

            DispatcherTimer dTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(30), DispatcherPriority.Render, RenderTimerTick, this.Dispatcher);
            dTimer.Start();

            lastTime = DateTime.Now;
            UpdateTimer = new Timer(10);
            UpdateTimer.Elapsed += TimerUpdateTick;
            UpdateTimer.Enabled = true;
        }

        private void TimerUpdateTick(object sender, ElapsedEventArgs e)
        {
            UpdateTimer.Enabled = false;
            DateTime now = DateTime.Now;
            space.Update((now - lastTime).Milliseconds);
            lastTime = now;
            UpdateTimer.Enabled = true;
        }

        private void RenderTimerTick(object sender, EventArgs e)
        {
            drawParams.DrawSpeed = CheckBox_Speed.IsChecked.Value;
            drawParams.DrawAxel = CheckBox_Axel.IsChecked.Value;
            Canvas_Space.Children.Clear();
            space.Draw(Canvas_Space, drawParams);
            TextBox_TargetData.Text = space.Target?.ToString();
        }

        private void Canvas_Space_MouseMove(object sender, MouseEventArgs e)
        {
            space.SetTarget(new Vector(e.GetPosition(Canvas_Space)));
        }

        private void TextBox_Speed_TextChanged(object sender, TextChangedEventArgs e)
        {
            double speed;
            if (double.TryParse(TextBox_Speed.Text, out speed))
            {
                if (speed > 10000000)
                    speed = 10000000;
                space.DefaultSpeed = speed;
            }
        }
    }

    public class DrawParams
    {
        public bool DrawSpeed = false;
        public bool DrawAxel = false;
    }
}
