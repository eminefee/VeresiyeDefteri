namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtConnectionString = new TextBox();
            btnSave = new Button();
            cmbEnvironment = new ComboBox();
            SuspendLayout();
            // 
            // txtConnectionString
            // 
            txtConnectionString.Location = new Point(271, 190);
            txtConnectionString.Name = "txtConnectionString";
            txtConnectionString.Size = new Size(214, 23);
            txtConnectionString.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(321, 260);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // cmbEnvironment
            // 
            cmbEnvironment.FormattingEnabled = true;
            cmbEnvironment.Location = new Point(271, 128);
            cmbEnvironment.Name = "cmbEnvironment";
            cmbEnvironment.Size = new Size(214, 23);
            cmbEnvironment.TabIndex = 2;
            cmbEnvironment.SelectedIndexChanged += cmbEnvironment;

            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cmbEnvironment);
            Controls.Add(btnSave);
            Controls.Add(txtConnectionString);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtConnectionString;
        private Button btnSave;
        private ComboBox cmbEnvironment;
    }
}
