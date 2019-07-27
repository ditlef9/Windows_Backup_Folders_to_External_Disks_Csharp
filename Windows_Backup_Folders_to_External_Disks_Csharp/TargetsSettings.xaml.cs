using System;
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
    /// Interaction logic for TargetsSettings.xaml
    /// </summary>
    public partial class TargetsSettings : UserControl
    {
        public TargetsSettings()
        {
            InitializeComponent();

            hideFeedback();
            updateFields();
        }

        /*- Feedback ------------------------------------------------------------------------- */
        private void hideFeedback()
        {
            stackPanelForm.Margin = new Thickness(10, 100, 0, 0);
            stackPanelFeedback.Visibility = Visibility.Collapsed;
        }
        private void showFeedback()
        {
            stackPanelForm.Margin = new Thickness(10, 140, 0, 0);
            stackPanelFeedback.Visibility = Visibility.Visible;
        }

        private void updateFields()
        {
            // Read settings file

            // Read file
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targetsSettings.txt";
            if (File.Exists(filePath))
            {
                // Read file
                string readFile = System.IO.File.ReadAllText(filePath);
                string[] separatorLineShift = new string[] { "\n" };
                string[] linesArray = readFile.Split(separatorLineShift, StringSplitOptions.None);

                string[] separatorEquals = new string[] { "=" };
                foreach (string line in linesArray)
                {

                    if (!(line.Equals("")))
                    {
                        // Do what you want with the file here
                        string[] currentLineArray = line.Split(separatorEquals, StringSplitOptions.None);

                        foreach (string nameValue in currentLineArray)
                        {
                            String name = currentLineArray[0].Trim();
                            String value = currentLineArray[1].Trim();

                            if (name.Equals("prefix"))
                            {
                                textBoxPrefix.Text = value;
                            }
                            else if (name.Equals("last_disk_counter"))
                            {
                                textBoxLastDiskCounter.Text = value;
                            }

                        } // foreach current line
                    } // lines not empty
                }// foreach lines 

            } // file exxists
        } // updateFields

        /* Button Save clicked -------------------------------------------------------- */
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            String textBoxPrefixString = textBoxPrefix.Text;
            String textBoxLastDiskCounterString = textBoxLastDiskCounter.Text;

            String input = "prefix = " + textBoxPrefixString + "\n" + 
                "last_disk_counter = " + textBoxLastDiskCounterString;

            // Write
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targetsSettings.txt";
            File.WriteAllText(filePath, input);

            // Feedback
            showFeedback();

        } // Button_Click
    }
}
