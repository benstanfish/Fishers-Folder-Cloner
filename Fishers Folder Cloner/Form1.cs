using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Fishers_Folder_Cloner
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                dgv.CurrentCell.Value = fbd.SelectedPath;
            }
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dgv.RefreshEdit();
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                dgv.Rows.Insert(dgv.Rows.Count - 1, new DataGridViewRow[1]);
            }
        }

        public static void CloneFolders(DataGridView dgv)  
        {

            foreach (DataGridViewRow dgvr in dgv.Rows)
            {
                /*
                try
                {
                    DirectoryCopy(dgvr.Cells["CopyPath"].Value.ToString(), dgvr.Cells["TargetPath"].Value.ToString(), true);
                }
                catch(Exception e)
                {
                    // MessageBox.Show("There was an error: " + e);
                    continue;
                }
                */
                
                if (dgvr.Cells["CopyPath"].Value == null || dgvr.Cells["TargetPath"].Value == null)
                {
                    
                }
                else
                {
                    DirectoryCopy(dgvr.Cells["CopyPath"].Value.ToString(), dgvr.Cells["TargetPath"].Value.ToString(), true);
                }
                
            }

            MessageBox.Show("Copy complete.");

        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            /*
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            */

            int loc = sourceDirName.LastIndexOf("\\") + 1;
            string partialDirName = sourceDirName.Substring(loc, sourceDirName.Length - loc);

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            string newloc = Path.Combine(destDirName, partialDirName);
            if (!Directory.Exists(newloc))
            {
                Directory.CreateDirectory(newloc);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(newloc, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(newloc, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            CloneFolders(this.dgv);
        }
    }
}
