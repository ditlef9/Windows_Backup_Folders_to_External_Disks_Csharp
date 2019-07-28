using Microsoft.WindowsAPICodePack.Dialogs;
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

namespace Windows_Backup_Folders_to_External_Disks_Csharp
{
    /// <summary>
    /// Interaction logic for TargetsIndex.xaml
    /// </summary>
    public partial class TargetsIndex : UserControl
    {
        private string currentDirectory;

        public TargetsIndex()
        {
            InitializeComponent();

            // Update folder lists
            updateDataGridTargets();
        }

        private void ButtonAddTarget_Click(object sender, RoutedEventArgs e)
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
                // Letter|Disk name|Total space mb|Total space human|Free space mb|Free space human|Used space mb|Used space human
                string inputToFile = dlg.FileName + "|-";

                // Total space and free space
                long totalSpaceBytes = 0;
                float totalSpaceMb = 0;
                float freeSpaceMb = 0;
                long freeSpaceBytes = 0;
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady && drive.Name == dlg.FileName)
                    {
                        totalSpaceBytes = drive.TotalSize;
                        totalSpaceMb = (totalSpaceBytes / 1024f) / 1024f;

                        freeSpaceBytes = drive.TotalFreeSpace;
                        freeSpaceMb = (freeSpaceBytes / 1024f) / 1024f;
                    }
                }
                float usedSpaceMb = totalSpaceMb - freeSpaceMb;
                long usetdSpaceBytes = totalSpaceBytes - freeSpaceBytes;

                // Convert to human readed format
                String totalSpaceHuman = sizeSuffix(totalSpaceBytes, 0);
                String freeSpaceHuman = sizeSuffix(freeSpaceBytes, 0);
                String usedSpaceHuman = sizeSuffix(usetdSpaceBytes, 0);

                inputToFile = inputToFile + "|" + totalSpaceMb + "|" + totalSpaceHuman + "|" + freeSpaceMb + "|" + freeSpaceHuman + "|" + usedSpaceMb + "|" + usedSpaceHuman;



                // Folder and file path
                string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targets.txt";

                // Read file (look for existing folders)
                if (File.Exists(filePath))
                {
                    string existingTargets = System.IO.File.ReadAllText(filePath);
                    inputToFile = inputToFile + "\n" + existingTargets;
                }

                // Write to file
                File.WriteAllText(filePath, inputToFile);

                // Update folderCount.txt
                string[] stringSeparators = new string[] { "\n" };
                string[] array = inputToFile.Split(stringSeparators, StringSplitOptions.None);

                string filePathfolderCount = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targetsCount.txt";
                File.WriteAllText(filePathfolderCount, array.Length.ToString());
            }


            // Update folder lists
            updateDataGridTargets();

        }




        /*- Update Data grid targets ----------------------------------------------------------------------- */
        private void updateDataGridTargets()
        {
            DataTable dt = new DataTable();
            DataColumn dataColumnLetter = new DataColumn("Letter", typeof(string));
            DataColumn dataColumnDiskName = new DataColumn("Disk name", typeof(string));
            DataColumn dataColumnTotalSpace = new DataColumn("Total space", typeof(string));
            DataColumn dataColumnFreeSpace = new DataColumn("Free space", typeof(string));
            DataColumn dataColumnUsedSpace = new DataColumn("Used space", typeof(string));

            dt.Columns.Add(dataColumnLetter);
            dt.Columns.Add(dataColumnDiskName);
            dt.Columns.Add(dataColumnTotalSpace);
            dt.Columns.Add(dataColumnFreeSpace);
            dt.Columns.Add(dataColumnUsedSpace);

            // Read file
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targets.txt";
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

                foreach (string line in existsingFoldersArray)
                {
                    if (!(line.Equals("")))
                    {

                        string[] stringLineSeparators = new string[] { "|" };
                        string[] lineArray = line.Split(stringLineSeparators, StringSplitOptions.None);

                        // Letter|Disk name|Total space mb|Total space human|Free space mb|Free space human|Used space mb|Used space human
                        DataRow dataRow = dt.NewRow();
                        dataRow[0] = lineArray[0]; // Letter
                        dataRow[1] = lineArray[1]; // Disk name
                        dataRow[2] = lineArray[3]; // Total space Human
                        dataRow[3] = lineArray[5]; // Free space Human
                        dataRow[4] = lineArray[7]; // Used space Human
                        dt.Rows.Add(dataRow);

                        countFolders++;
                    } // not empty
                } //foreach 


                // Create folderCount.txt
                string filePathfolderCount = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targetsCount.txt";
                File.WriteAllText(filePathfolderCount, countFolders.ToString());

            } // file exists

            // Add data to data grid
            dataGridTargets.ItemsSource = dt.DefaultView;



        } // updateDataGridFolders

        /*- Update Flow Document Reader Targets ------------------------------------------------------------- */
        private void DataGridTargets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGridSender = (DataGrid)sender;

            // Loop trough dataGridSender;
            String inputData = "";
            foreach (System.Data.DataRowView dataRow in dataGridSender.ItemsSource)
            {
                // Letter|Disk name|Total space mb|Total space human|Free space mb|Free space human|Used space mb|Used space human

                String letter           = dataRow[0].ToString();
                String diskName         = dataRow[1].ToString();
                // String totalSpaceMb     = dataRow[2].ToString();
                String totalSpaceHuman  = dataRow[2].ToString();
                // String freeSpaceMb      = dataRow[4].ToString();
                String freeSpaceHuman   = dataRow[3].ToString();
                // String usedSpaceMb      = dataRow[6].ToString();
                String usedSpaceHuman   = dataRow[4].ToString();




                if (!(letter.Equals("")))
                {
                    if (inputData.Equals(""))
                    {
                        inputData = letter + "|" + diskName + "|" + 0 + "|" + totalSpaceHuman + "|" + 0 + "|" + freeSpaceHuman + "|" + 0 + "|" + usedSpaceHuman;
                    }
                    else
                    {
                        inputData = inputData + "\n" + letter + "|" + diskName + "|" + 0 + "|" + totalSpaceHuman + "|" + 0 + "|" + freeSpaceHuman + "|" + 0 + "|" + usedSpaceHuman;
                    }


                } // folder name not null

            } // foreach data grid

            // Write to folders.txt
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "targets.txt";
            File.WriteAllText(filePath, inputData);


        }// DataGridTargets_SelectionChanged

        


        /*- Size suffix ------------------------------------------------------------------- */
        /* Converts MB to GB */
        static string sizeSuffix(long value, int decimalPlaces = 1)
        {
            string[] sizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + sizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                sizeSuffixes[mag]);
        } // sizeSuffix
    }
}
