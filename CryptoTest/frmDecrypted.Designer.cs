namespace CryptoTest
{
    partial class frmDecrypted
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbDecrypted = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbDecrypted
            // 
            this.tbDecrypted.Location = new System.Drawing.Point(12, 12);
            this.tbDecrypted.Multiline = true;
            this.tbDecrypted.Name = "tbDecrypted";
            this.tbDecrypted.Size = new System.Drawing.Size(807, 556);
            this.tbDecrypted.TabIndex = 0;
            // 
            // frmDecrypted
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 580);
            this.Controls.Add(this.tbDecrypted);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDecrypted";
            this.Text = "Decrypted";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbDecrypted;
    }
}