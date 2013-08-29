using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AdventureRipper.Model.Files;
using AdventureRipper.Model.Files.Image.RRM;
using AdventureRipper.Model.Resource;
using AdventureRipper.Model.Resource.LIB;
using Microsoft.Win32;

namespace AdventureRipper
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Resource resource;


        public MainWindow()
        {
            InitializeComponent();
        }


        private void openMenu(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            var dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".LIB";
            dlg.Filter = "LIB Files (*.LIB)|*.LIB|RRM image|*.RRM";


            // Display OpenFileDialog by calling ShowDialog method 
            bool? result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileName.Text = filename;
                if(Path.GetExtension(filename).Equals(".LIB"))
                {
                    resource = new LibResource(filename);
                    var rootItem = new TreeViewItem();
                    rootItem.Header = resource.FileName;
                    foreach (FileEntry f in resource.Files)
                    {
                        //var leafItem = new TreeViewItem() {Header = f.FileName};
                        //leafItem.Items.Add(new TreeViewItem() {Header = f.FileOffset});
                        rootItem.Items.Add(f);
                    }
                    fileTableSizeTextBlock.Text = resource.NFiles.ToString();
                    fileTreeView.Items.Add(rootItem);
                }
                else if (Path.GetExtension(filename).Equals(".RRM"))
                {
                    RRMImage image = new RRMImage(filename);
                    image.ToBitmap();
                    imgFoto.Source = MainWindow.loadBitmap(image.ToBitmap());
                }
            }
        }

        private void btnFoto_Click(object sender, RoutedEventArgs e)
        {
            object item = fileTreeView.SelectedItem;
            if (item is FileEntry)
            {
                FileEntry entry = (FileEntry) item;
                byte[] data = resource.GetFile(entry.ResourceIdx);
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                Nullable<bool> result = dlg.ShowDialog();
                // Process save file dialog box results
                if (result == true)
                {
                    string filename = dlg.FileName;
                    try
                    {
                        FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                    }
                    catch(Exception exception)
                    {
                        Console.WriteLine("Exception " + exception.ToString());
                    }
                }
            }
        }


        private void fileTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            if (tree.SelectedItem is TreeViewItem)
            {
                var item = tree.SelectedItem as TreeViewItem;

                MessageBox.Show("sdadas:" + tree.SelectedItem);
            }
            else if (tree.SelectedItem is string)
            {
                MessageBox.Show(tree.SelectedItem.ToString());
            }
        }

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                //DeleteObject(ip);
            }

            return bs;
        }
    }
}