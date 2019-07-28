using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
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
            drawLogTable();
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
            dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            dispatcherTimer.Start();


        } // startBackupProcess



        /*- Stop backup process ---------------------------------------------------------------- */
        private void stopBackupProcess()
        {
            
        } // stopBackupProcess


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Path
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            // Status
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



            // Look for directories and files
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "sources.txt";
            if (File.Exists(filePath))
            {

                // Read file
                string readSources = System.IO.File.ReadAllText(filePath);
                string[] stringSeparators = new string[] { "\n" };
                string[] sourcesArray = readSources.Split(stringSeparators, StringSplitOptions.None);

                // Loop trough file
                foreach (string sourcePath in sourcesArray)
                {
                    if (!(sourcePath.Equals("")) && Directory.Exists(sourcePath))
                    {
                        // Make list of folders inside source, example \\10.0.0.1\speilfiler\x and  \\10.0.0.1\speilfiler\y
                        String sourcePathCleanName = sourcePath; // sourcePath = \\10.0.0.1
                        sourcePathCleanName = sourcePathCleanName.Replace("\\", "");
                        sourcePathCleanName = sourcePathCleanName.Replace(":", "");

                        

                        string filePathCurrentSource = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "sourcesDirectories" + sourcePathCleanName + ".txt";
                        String inputDirectories = "";

                        // Loop trough folder
                        string[] directoriesEntries = Directory.GetDirectories(sourcePath);
                        foreach (string currentRootDirectory in directoriesEntries)
                        {
                            // Do something with fileName
                            if (inputDirectories.Equals(""))
                            {
                                inputDirectories = currentRootDirectory;
                            }
                            else
                            {
                                inputDirectories = inputDirectories + "\n" + currentRootDirectory;
                            }

                            // We are now in \\10.0.0.1\speilfiler\x
                            // Loop trough directory and check for files
                            string[] filesEntries = Directory.GetFiles(currentRootDirectory, "*", SearchOption.AllDirectories);
                            foreach (string files in filesEntries)
                            {
                                inputDirectories = inputDirectories + "\n - " + files;


                                // Check if file is copied
                                String cleanFile = files;
                                cleanFile = cleanFile.Replace("\\", "");
                                cleanFile = cleanFile.Replace(":", "");

                                String copyFile = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "backupCompleted" + "\\" + cleanFile + ".temp";
                                if (!(File.Exists(copyFile)))
                                {
                                    // Make copy
                                    backupFile(files);

                                    // Write to log
                                    // [disk] [case number] [files]
                                    String logFile = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "log.txt";
                                    string readLog = "";
                                    if (File.Exists(logFile))
                                    {
                                        readLog = System.IO.File.ReadAllText(logFile);
                                    }
                                    String datetime = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
                                    String inputLog = datetime + "|" + currentRootDirectory + "|" + files + "\n" + readLog;
                                    File.WriteAllText(logFile, inputLog);

                                    // Make copyfile
                                    File.WriteAllText(copyFile, "");


                                    // Update folder lists
                                    drawLogTable();

                                } // copy doesnt exist
                            }
                        }
                        File.WriteAllText(filePathCurrentSource, inputDirectories);


                    } // sourceName
                } // foreach sources
            } // if source exits


        } // dispatcherTimer_Tick



        /*- Backup file ----------------------------------------------------------------- */
        private void backupFile(string files)
        {
            throw new NotImplementedException();
        } // backupFile

        /*- Draw log table ---------------------------------------------------------------------- */
        private void drawLogTable() {


            Table oTable = new Table();


            // Create n columns and add them to the table's Columns collection.
            int numberOfColumns = 3;
            for (int x = 0; x < numberOfColumns; x++){
                oTable.Columns.Add(new TableColumn());
            }

            // Create and add an empty TableRowGroup Rows.
            oTable.RowGroups.Add(new TableRowGroup());

            // Add the table head row.
            oTable.RowGroups[0].Rows.Add(new TableRow());

            // Configure the table head row
            TableRow currentRow = oTable.RowGroups[0].Rows[0];
            var brushConverter = new BrushConverter();
            currentRow.Background = (Brush)brushConverter.ConvertFrom("#FFe2e2e2"); // grey background
            currentRow.Foreground = (Brush)brushConverter.ConvertFrom("#FF000000"); // black text
            currentRow.FontFamily = new FontFamily("Segoe UI"); ;
            currentRow.FontSize = 16;
            currentRow.FontWeight = FontWeights.Bold;


            // Add the header row with content,
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Date time"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Directory"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("File"))));

            // Read file and add rows
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string filePath = userPath + "\\" + "WindowsBackupFoldersToExternalDisk" + "\\" + "config" + "\\" + "log.txt";
            if (File.Exists(filePath)) {

                // Read file
                string existingFolders = System.IO.File.ReadAllText(filePath);
                string[] stringSeparators = new string[] { "\n" };
                string[] existsingFoldersArray = existingFolders.Split(stringSeparators, StringSplitOptions.None);

                // Loop trough file
                String lastDirectory = "";
                int countLines = 1;

                Brush brushEven = (Brush)brushConverter.ConvertFrom("#FFf3f3f3"); // grey background
                Brush brushOdd = (Brush)brushConverter.ConvertFrom("#FFf8f8f8"); // grey background
                String styleHandler = "even";
                foreach (string line in existsingFoldersArray)
                {
                    if (!(line.Equals(""))) {

                        string[] stringLineSeparators = new string[] { "|" };
                        string[] lineArray = line.Split(stringLineSeparators, StringSplitOptions.None);

                        String dateTime  = lineArray[0]; // Datetime
                        String directory = lineArray[1]; // Directory
                        String file      = lineArray[2].Replace(directory.ToString(), ""); // File

                        if (lastDirectory.Equals(directory.ToString()))
                        {
                            directory = "";
                        }

                        // Add new row
                        oTable.RowGroups[0].Rows.Add(new TableRow());
                        currentRow = oTable.RowGroups[0].Rows[countLines];

                        //Configure the row layout
                        if (styleHandler.Equals("odd")){
                            currentRow.Background = brushOdd;
                            styleHandler = "even";
                        }
                        else { 
                            currentRow.Background = brushEven;
                            styleHandler = "odd";
                        }
                        currentRow.FontFamily = new FontFamily("Segoe UI"); ;
                        currentRow.FontSize = 16;

                        //Add the country name in the first cell
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(dateTime))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(directory))));
                        currentRow.Cells.Add(new TableCell(new Paragraph(new Run(file))));


                        // Last directory
                        lastDirectory = directory;
                        countLines = countLines+1;
                    } // not empty
                } //foreach 


            } // file exists


            //Add the given flow document to the window
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.Blocks.Add(oTable);
            contentControlDashboardLog.Content = flowDocument;

        }
    }
}
