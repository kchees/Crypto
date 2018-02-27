using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Security.Cryptography;


namespace CryptoTest
{
    public partial class frmMain : Form
    {
        private AesCryptoServiceProvider m_cryptoService = new AesCryptoServiceProvider();
        FileInfo m_file = null;

        // fixed initialization vector (IV)
        private byte[] m_IV = { 0x88, 0x46, 0x01, 0x09, 0x5C, 0x08, 0x17, 0xE8, 0xE8, 0xFA, 0xB6, 0xC1, 0x31, 0xAC, 0xCC, 0x53 };



        public frmMain()
        {
            InitializeComponent();

            btnEncrypt.Enabled = false;
            btnDecrypt.Enabled = false;

            // set key size to 256 bits
            m_cryptoService.KeySize = 256;

            // set initialization vector
            m_cryptoService.IV = m_IV;

            //byte[] IV = m_cryptoService.IV;
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            string filePath = GetFilePath();
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                tbFilePath.Text = filePath;
            }
        }


        private string GetFilePath()
        {
            string path = string.Empty;

            // Create an instance of the open file dialog box.
            OpenFileDialog openFileDlg = new OpenFileDialog();

            // Set filter options and filter index to All files
            openFileDlg.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDlg.FilterIndex = 2;

            // Call the ShowDialog method to show the dialog box.
            DialogResult result = openFileDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = openFileDlg.FileName;
            }

            return path;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            PasswordDlg dlg = new PasswordDlg();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                byte[] key = getKeyFromPassword(dlg.Password);

                string error;
                if (!EncryptFile(tbFilePath.Text, key, out error))
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!string.IsNullOrEmpty(dlg.Error))
                    MessageBox.Show(dlg.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            PasswordDlg dlg = new PasswordDlg();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                byte[] key = getKeyFromPassword(dlg.Password);

                string decrypted, error;
                if (DecryptFile(tbFilePath.Text, key, out decrypted, out error))
                {
                    frmDecrypted decryptionForm = new frmDecrypted();
                    decryptionForm.Decrypted = decrypted;
                    decryptionForm.Show();
                }
                else
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (!string.IsNullOrEmpty(dlg.Error))
                    MessageBox.Show(dlg.Error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private byte[] getKeyFromPassword(string password)
        {
            int keyBytes = m_cryptoService.KeySize / 8;
            byte[] key = new byte[keyBytes];
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            int j = 0;
            for (int i = 0; i < keyBytes; i++)
            {
                key[i] = passwordBytes[j++];
                if (j >= passwordBytes.Length)
                    j = 0;
            }

            return key;
        }

        private bool EncryptFile(string filePath, byte[] key, out string error)
        {
            error = string.Empty;
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                error = string.Format("File {0} not found", filePath);
                return false;
            }
                

            bool success = true;

            // get the size of the file in bytes
            int fileSize = Convert.ToInt32(fi.Length);

            // create char buffer to read file contents into
            char[] buffer = new char[fileSize];

            // read file into buffer
            try
            {
                using (StreamReader reader = new StreamReader(filePath)) 
                {
                    reader.Read(buffer, 0, fileSize);
                }
            }
            catch (Exception /*e*/)
            {
                // some error occurred
                error = string.Format("Error reading file {0}", filePath);
                return false;
            }

            // Create Encryptor
            ICryptoTransform encryptor = m_cryptoService.CreateEncryptor(key, m_IV);

            // Create the streams used for encryption. 
            byte[] encrypted = null;

            try
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // write file contents to encryption stream
                            swEncrypt.Write(buffer, 0, fileSize);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                // some error occurred
                error = string.Format("Exception during encryption: {0}", e.Message);
                return false;
            }

            if (encrypted == null)
            {
                error = "Error during encryption";
                return false;
            }

            // overwrite file with encrypted data
            try
            {
                // create a new stream to write to the file
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(filePath)))
                {
                    writer.Write(encrypted);
                    writer.Flush();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                error = string.Format("Exception during encryption: {0}", e.Message);
                success = false;
            }

            return success;
        }

        private bool DecryptFile(string filePath, byte[] key, out string decrypted, out string error)
        {
            decrypted = string.Empty;
            error = string.Empty;
            FileInfo fi = new FileInfo(filePath);
            if (!fi.Exists)
            {
                error = string.Format("File {0} not found", filePath);
                return false;
            }

            bool success = true;

       
            // get the size of the encrypted file in bytes
            int fileSize = Convert.ToInt32(fi.Length);

            // create buffer to read file contents into
            byte[] buffer = new byte[fileSize];

            // read encrypted file data into buffer
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                {
                    reader.Read(buffer, 0, fileSize);
                }
            }
            catch (Exception /*e*/)
            {
                // some error occurred
                error = string.Format("Error reading file {0}", filePath);
                return false;
            }

            // Create a decrytor to perform the stream transform
            ICryptoTransform decryptor = m_cryptoService.CreateDecryptor(key, m_IV);

            // Create the streams used for decryption
            //byte[] decrypted = null;
            try
            {
                using (MemoryStream msDecrypt = new MemoryStream(buffer))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        /*
                        int size = Convert.ToInt32(csDecrypt.Length);
                        decrypted = new byte[size];

                        using (BinaryReader brDecrypt = new BinaryReader(csDecrypt))
                        {
                            brDecrypt.Read(decrypted, 0, size);
                        } 
                        */

                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception /*e*/)
            {
                // some error occurred
                error = string.Format("Invalid Password?");
                return false;
            }

            return success;
        }


        private void tbFilePath_TextChanged(object sender, EventArgs e)
        {
            bool enabled = (tbFilePath.Text.Trim().Length > 0);
            btnEncrypt.Enabled = enabled;
            btnDecrypt.Enabled = enabled;
        }

        



    }
}
