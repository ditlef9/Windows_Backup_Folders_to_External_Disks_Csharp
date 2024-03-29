﻿using System;
using System.Collections.Generic;
using System.IO;
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

            createFoldersAndFiles();
        }

        /*- Create folders and files ----------------------------------------------------- */
        public void createFoldersAndFiles()
        {
            // Folder and file path
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Root
            string folderPath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk";
            if (!(Directory.Exists(folderPath)))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Config
            folderPath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config";
            if (!(Directory.Exists(folderPath)))
            {
                Directory.CreateDirectory(folderPath);
            }

            // targetsSettings.txt
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targetsSettings.txt";
            if (!(File.Exists(filePath))) { 
                    String input = "prefix = H\nlast_disk_counter = 1";
                    File.WriteAllText(filePath, input);
            }

            // BackupCompleted
            folderPath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "backupCompleted";
            if (!(Directory.Exists(folderPath)))
            {
                Directory.CreateDirectory(folderPath);
            }

        }

        /*- Navigation --------------------------------------------------------------------- */
        private void ButtonNavigationDashboard_Click(object sender, RoutedEventArgs e)
        {
            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 100 };
            userControlDashboard.Visibility = Visibility.Visible;
        }

        private void ButtonNavigationTargets_Click(object sender, RoutedEventArgs e)
        {

            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 140 };
            userControlTargets.Visibility = Visibility.Visible;
        }
        private void ButtonNavigationSources_Click(object sender, RoutedEventArgs e)
        {
            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 180 };
            userControlSources.Visibility = Visibility.Visible;

        }
        private void ButtonNavigationOverview_Click(object sender, RoutedEventArgs e)
        {
            collapseAllMainUserControls();
            stackPanelNavigationActive.Margin = new System.Windows.Thickness { Top = 220 };
            userControlBackupOverview.Visibility = Visibility.Visible;
        }


        public void collapseAllMainUserControls()
        {
            userControlDashboard.Visibility = Visibility.Collapsed;
            userControlTargets.Visibility = Visibility.Collapsed;
            userControlSources.Visibility = Visibility.Collapsed;
            userControlBackupOverview.Visibility = Visibility.Collapsed;
           
        }

    }
}
