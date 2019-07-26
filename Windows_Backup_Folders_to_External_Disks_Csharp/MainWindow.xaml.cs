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

namespace Windows_Backup_Folders_to_External_Disks_Csharp
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

        private void ButtonNavigationDashboard_Click(object sender, RoutedEventArgs e)
        {
            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 100 };
            userControlDashboard.Visibility = Visibility.Visible;
        }

        private void ButtonNavigationFolders_Click(object sender, RoutedEventArgs e)
        {
            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 140 };
            userControlFolders.Visibility = Visibility.Visible;
        }

        private void ButtonNavigationOverview_Click(object sender, RoutedEventArgs e)
        {
            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 180 };
            userControlBackupOverview.Visibility = Visibility.Visible;
        }

        private void collapseAllMainUserControls()
        {
            userControlDashboard.Visibility = Visibility.Collapsed;
            userControlFolders.Visibility = Visibility.Collapsed;
            userControlBackupOverview.Visibility = Visibility.Collapsed;
        }
    }
}
