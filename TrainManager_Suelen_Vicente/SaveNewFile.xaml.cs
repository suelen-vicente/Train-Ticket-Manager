using System;
using System.IO;
using System.Windows;

namespace TrainManager_Suelen_Vicente
{
    public partial class SaveNewFile : Window
    {
        public string json;
        public FileManager fileManager;

        public SaveNewFile(string json, FileManager fileManager)
        {
            this.json = json;
            this.fileManager = fileManager;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnSaveNewFile_Click(object sender, RoutedEventArgs e)
        {
            if (txtFileName.Text != null && txtFileName.Text != "")
            {
                fileManager.setCurrentFile(txtFileName.Text + ".json");

                MessageBox.Show(fileManager.saveFile(json));

                Close();
            }
            else
            {
                MessageBox.Show("File name is mandatory.");
                txtFileName.Text = "";
                txtFileName.Focus();
            }

        }
    }
}
