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
using AdventureRipper.Model.Files;
using AdventureRipper.Model.Resource;
using AdventureRipper.Model.Resource.LIB;

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
                    LibResource resource = new LibResource(b);
                    TreeViewItem rootItem = new TreeViewItem();
                    rootItem.Header = resource.Header;
                    foreach(FileEntry f in resource.Files)
                    {
                            
                            rootItem.Items.Add(new TreeViewItem() { Header = f.FileName });
                     }
                    fileTableSizeTextBlock.Text = resource.NFiles.ToString();
                    fileTreeView.Items.Add(rootItem);


                }
            }
        }
    }


}
