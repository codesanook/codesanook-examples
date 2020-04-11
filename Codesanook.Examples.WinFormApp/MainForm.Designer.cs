namespace Codesanook.Examples.WinFormApp
{
    partial class MainForm
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
            this.pingButton = new System.Windows.Forms.Button();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pingButton
            // 
            this.pingButton.Location = new System.Drawing.Point(117, 92);
            this.pingButton.Name = "pingButton";
            this.pingButton.Size = new System.Drawing.Size(75, 25);
            this.pingButton.TabIndex = 0;
            this.pingButton.Text = "Ping";
            this.pingButton.UseVisualStyleBackColor = true;
            this.pingButton.Click += new System.EventHandler(this.PingButtonClick);
            // 
            // addressTextBox
            // 
            this.addressTextBox.Location = new System.Drawing.Point(45, 50);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(221, 23);
            this.addressTextBox.TabIndex = 1;
            this.addressTextBox.Text = "codesanook.com";
            this.addressTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Host/IP address";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 136);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addressTextBox);
            this.Controls.Add(this.pingButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pingButton;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label label1;
    }
}

