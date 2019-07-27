using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace Windows_Backup_Folders_to_External_Disks_Csharp
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {

        public Dashboard()
        {
            InitializeComponent();


        }

        /*- Start/Stop backup process -------------------------------------------------------- */
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            // Read status of button
            if (buttonStart.Content.Equals("Start"))
            {
                buttonStart.Content = "Stop";
                startBackupProcess();

            }
            else
            {
                buttonStart.Content = "Start";
                stopBackupProcess();
            }
        }
        /*- Start backup process ---------------------------------------------------------------- */
        private void startBackupProcess()
        {
            labelDashboardStatus.Content = "Starting backup program!";


            DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();


        } // startBackupProcess



        /*- Stop backup process ---------------------------------------------------------------- */
        private void stopBackupProcess()
        {
            
        } // stopBackupProcess


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            int currentSecond = DateTime.Now.Second;
            if (currentSecond == 1 || currentSecond == 6 | currentSecond == 11 || currentSecond == 16 | currentSecond == 21 || currentSecond == 26 | currentSecond == 31 || currentSecond == 36 | currentSecond == 41 || currentSecond == 46 | currentSecond == 51 || currentSecond == 56)
            {
                labelDashboardStatus.Content = "Running" + "";
            }
            else if (currentSecond == 2 || currentSecond == 7 || currentSecond == 12 || currentSecond == 17 || currentSecond == 22 || currentSecond == 27 || currentSecond == 32 || currentSecond == 37 || currentSecond == 42 || currentSecond == 47 || currentSecond == 52 || currentSecond == 57)
            {
                labelDashboardStatus.Content = "Running" + ".";
            }
            else if (currentSecond == 3 || currentSecond == 8 || currentSecond == 13 || currentSecond == 18 || currentSecond == 23 || currentSecond == 28 || currentSecond == 33 || currentSecond == 38 || currentSecond == 43 || currentSecond == 48 || currentSecond == 53 || currentSecond == 58)
            {
                labelDashboardStatus.Content = "Running" + "..";
            }
            else if (currentSecond == 4 || currentSecond == 9 || currentSecond == 14 || currentSecond == 19 || currentSecond == 24 || currentSecond == 29 || currentSecond == 34 || currentSecond == 39 || currentSecond == 44 || currentSecond == 49 || currentSecond == 54 || currentSecond == 59)
            {
                labelDashboardStatus.Content = "Running" + "...";
            }
            else if (currentSecond == 5 || currentSecond == 10 || currentSecond == 15 || currentSecond == 20 || currentSecond == 25 || currentSecond == 30 || currentSecond == 35 || currentSecond == 40 || currentSecond == 55 || currentSecond == 60)
            {
                labelDashboardStatus.Content = "Running" + "....";
            }
        }
    }
}
