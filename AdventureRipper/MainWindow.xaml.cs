using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using AdventureRipper.Model.Files;
using AdventureRipper.Model.Files.Image;
using AdventureRipper.Model.Files.Image.LBV;
using AdventureRipper.Model.Files.Image.RRM;
using AdventureRipper.Model.Files.Image.VGS;
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
            fileTreeView.Items.Clear();
        }


        private void openMenu(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            var dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".LIB";
            dlg.Filter = "LIB Files (*.LIB)|*.LIB|RRM image|*.RRM|VGS image|*.VGS";


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
                    numFilesTextBlock.Text = resource.NFiles.ToString();
                    fileTreeView.Items.Add(rootItem);
                }
                else if (Path.GetExtension(filename).Equals(".RRM"))
                {
                    RRMImage image = new RRMImage(filename);
                    showImage(image);
                }
                else if (Path.GetExtension(filename).Equals(".VGS"))
                {
                    VGSImage image = new VGSImage(filename);
                }
            }
        }

        private void showImage(ImageFile image)
        {
            imgFoto.Source = MainWindow.loadBitmap(image.ToBitmap());
                    imgFoto.Width = image.Width;
                    imgFoto.Height = image.Height;
                

        }

        private void btnFoto_Click(object sender, RoutedEventArgs e)
        {
            object item = fileTreeView.SelectedItem;
            if (item is FileEntry)
            {
                var entry = (FileEntry) item;
                byte[] data = resource.GetFile(entry.ResourceIdx);
                var dlg = new Microsoft.Win32.SaveFileDialog();
                bool? result = dlg.ShowDialog();
                // Process save file dialog box results
                if (result == true)
                {
                    string filename = dlg.FileName;
                    try
                    {
                        var stream = new FileStream(filename, FileMode.Create, FileAccess.Write);
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


        private void FileTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            if (tree.SelectedItem is TreeViewItem)
            {
                var item = tree.SelectedItem as TreeViewItem;

                //MessageBox.Show("sdadas:" + tree.SelectedItem);
            }
            else if (tree.SelectedItem is string)
            {
                //MessageBox.Show(tree.SelectedItem.ToString());
            }
            else if (tree.SelectedItem is FileEntry)
            {
                fileNameTextBlock.Text = (tree.SelectedItem as FileEntry).FileName.ToString();
                offsetTextBlock.Text = (tree.SelectedItem as FileEntry).FileOffset.ToString();
                FileEntry f = (FileEntry) tree.SelectedItem;
                if (Path.GetExtension(f.FileName).Equals(".LBV"))
                {
                    LBVImage image = new LBVImage(resource.GetFile(f.ResourceIdx), f.FileName, resource.FileName);
                    showImage(image);  
                }


                  

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

        private void btnZoom_Click(object sender, RoutedEventArgs e)
        {
            imgFoto.Width = imgFoto.Width*2;
            imgFoto.Height = imgFoto.Height*2;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                String dirPath = dialog.SelectedPath;
                string[] files = Directory.GetFiles(dirPath);
                var rootItem = new TreeViewItem();
                rootItem.Header = Path.GetFileName(dirPath);
                int numFiles = 0;
                foreach(string f in files)
                {
                    if(Path.GetExtension(f).Equals(".RRM"))
                    {
                        FileEntry file = new RRMImage(f);
                        numFiles++;
                        rootItem.Items.Add(file);
                    }

                }
                if (numFiles > 0)
                {
                    fileTreeView.Items.Add(rootItem);
                }
            }
        }
    }
}