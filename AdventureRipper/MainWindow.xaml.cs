using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.InteropServices;

namespace AdventureRipper
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openMenu(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".LIB";
            dlg.Filter = "LIB Files (*.LIB)|*.LIB";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileName.Text = filename;
                using (BinaryReader b = new BinaryReader(File.Open(filename, FileMode.Open)))
                {
                    var header = new Header
                    {
                        id = new String(b.ReadChars(3)),
                        unknown = b.ReadByte(),
                        nFiles = b.ReadByte()
                    };
                    headerTextBlock.Text = header.id;
                    numFilesTextBlock.Text = header.nFiles.ToString();

                    if (headerTextBlock.Text.Equals("LIB"))
                    {
                        TreeViewItem rootItem = new TreeViewItem();
                        rootItem.Header = "LIB File";

                        List<FileEntry> fileTable = new List<FileEntry>();
                        for (int i = 0; i < header.nFiles; i++)
                        {
                            var fileEntry = new FileEntry
                            {
                                null1 = b.ReadByte(),
                                fileName = new String(b.ReadChars(12)).Replace("\0", string.Empty),
                                null2 = b.ReadByte(),
                                fileOffset = b.ReadBytes(3)

                            };
                            fileTable.Add(fileEntry);
                            rootItem.Items.Add(new TreeViewItem() { Header = fileEntry.fileName });
                        }
                        fileTableSizeTextBlock.Text = fileTable.Count.ToString();
                        fileTreeView.Items.Add(rootItem);
                    }

                }
            }
        }
    }

    struct Header
    {
        public String id;
        public byte unknown;
        public byte nFiles;
    }

    struct FileEntry
    {
        public byte null1;
        public String fileName;
        public byte null2;
        public byte[] fileOffset;
    }
}
