using System;
using System.Collections.Generic;
using System.Data;
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

using Microsoft.WindowsAPICodePack.Dialogs;

namespace Windows_Backup_Folders_to_External_Disks_Csharp
{
    /// <summary>
    /// Interaction logic for UserControlFolders.xaml
    /// </summary>
    public partial class UserControlFolders : UserControl
    {
        private string currentDirectory;
        List<Paragraph> buttonActionDeleteList = new List<Paragraph>();

        public UserControlFolders()
        {
            InitializeComponent();

            // Update folder lists
            updateDataGridFolders();
        }



        /*- Butt new folder click ----------------------------------------------------------- */
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var dlg = new CommonOpenFileDialog();
            dlg.Title = "My Title";
            dlg.IsFolderPicker = true;
            dlg.InitialDirectory = currentDirectory;

            dlg.AddToMostRecentlyUsedList = false;
            dlg.AllowNonFileSystemItems = false;
            dlg.DefaultDirectory = currentDirectory;
            dlg.EnsureFileExists = true;
            dlg.EnsurePathExists = true;
            dlg.EnsureReadOnly = false;
            dlg.EnsureValidNames = true;
            dlg.Multiselect = false;
            dlg.ShowPlacesList = true;

            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // Input text
                string inputToFile = dlg.FileName;

                // Folder and file path
                string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "folders.txt";

                // Read file (look for existing folders)
                if (File.Exists(filePath))
                {
                    string existingFolders = System.IO.File.ReadAllText(filePath);
                    inputToFile = dlg.FileName + "\n" + existingFolders;
                }

                // Write to file
                File.WriteAllText(filePath, inputToFile);

                // Update folderCount.txt
                string[] stringSeparators = new string[] { "\n" };
                string[] array = inputToFile.Split(stringSeparators, StringSplitOptions.None);

                string filePathfolderCount = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "folderCount.txt";
                File.WriteAllText(filePathfolderCount, array.Length.ToString());
            }


            // Update folder lists
            updateDataGridFolders();

        } // add folder

        /*- Update Data grid folders ----------------------------------------------------------------------- */
        private void updateDataGridFolders()
        {
            DataTable dt = new DataTable();
            DataColumn dataColumnName = new DataColumn("Name", typeof(string));
            dt.Columns.Add(dataColumnName);

            // Read file
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "folders.txt";
            if (File.Exists(filePath))
            {

                // Read file
                string existingFolders = System.IO.File.ReadAllText(filePath);
                string[] stringSeparators = new string[] { "\n" };
                string[] existsingFoldersArray = existingFolders.Split(stringSeparators, StringSplitOptions.None);

                // Loop trough file
                int countFolders = 0;
                var bc = new BrushConverter();
                Brush evenBrush = (Brush)bc.ConvertFrom("#FFe0e0e0");
                Brush oddBrush = (Brush)bc.ConvertFrom("#FFf7f7f7");
                Brush linkBrush = (Brush)bc.ConvertFrom("#FF1509d8");

                foreach (string folderName in existsingFoldersArray)
                {
                    if (!(folderName.Equals("")))
                    {
                        DataRow dataRow = dt.NewRow();
                        dataRow[0] = folderName;
                        dt.Rows.Add(dataRow);

                        countFolders++;
                    } // not empty
                } //foreach 


                // Create folderCount.txt
                string filePathfolderCount = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "folderCount.txt";
                File.WriteAllText(filePathfolderCount, countFolders.ToString());

            } // file exists

            // Add data to data grid
            dataGridFolders.ItemsSource = dt.DefaultView;



        } // updateDataGridFolders

        /*- Update Flow Document Reader Folders ------------------------------------------------------------- */
        private void DataGridFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGridSender = (DataGrid)sender;

            // Loop trough dataGridSender;
            String inputFolders = "";
            foreach (System.Data.DataRowView dataRow in dataGridSender.ItemsSource)
            {
                String folderName = dataRow[0].ToString();
                if (!(folderName.Equals("")))
                {
                    if (inputFolders.Equals(""))
                    {
                        inputFolders = folderName;
                    }
                    else
                    {
                        inputFolders = inputFolders + "\n" + folderName;
                    }


                } // folder name not null

            } // foreach data grid

            // Write to folders.txt
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "folders.txt";
            File.WriteAllText(filePath, inputFolders);


        } // DataGridFolders_SelectionChanged
    }




}
