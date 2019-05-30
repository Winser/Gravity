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
        public MainWindow()
        {
            InitializeComponent();
            space = new Space();
            drawParams = new DrawParams();
            DispatcherTimer dTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(30), DispatcherPriority.Render, RenderTimerTick, this.Dispatcher);
            dTimer.Start();

            UpdateTimer = new Timer(0.0001);
            UpdateTimer.Elapsed += TimerUpdateTick;
            UpdateTimer.Enabled = true;
        }

        private void TimerUpdateTick(object sender, ElapsedEventArgs e)
        {
            UpdateTimer.Enabled = false;
            space.Update();
            UpdateTimer.Enabled = true;
        }

        private void RenderTimerTick(object sender, EventArgs e)
        {
            drawParams.DrawSpeed = CheckBox_Speed.IsChecked.Value;
            drawParams.DrawAxel = CheckBox_Axel.IsChecked.Value;
            Canvas_Space.Children.Clear();
            space.Draw(Canvas_Space, drawParams);
        }

        private void Slider_Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Space.Speed = Slider_Speed.Value;
        }
    }

    public class DrawParams
    {
        public bool DrawSpeed = false;
        public bool DrawAxel = false;
    }
}
