//import package ต่างๆ ที่จะใช้งาน
import System;
import System.Windows.Forms;
import System.Drawing;
import Accessibility;

package CodeSanook.Example.JScript.NET {
    class MainForm extends System.Windows.Forms.Form {
        private var label: Label;
        private var textBox: TextBox;
        private var button: Button;
        private var panel: Panel;

        function MainForm() {
            this.Text = "JScript.NET Window Form demo";
            this.Width  = 300;
            this.Height = 350;
            this.ClientSize = new System.Drawing.Size(300, 350);

            //ให้ตัว form แสดงผลที่กลางหน้าจอ
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            label = new Label;
            label.Location = new Point(10, 10);
            label.Size = new System.Drawing.Size(80, 20);
            label.Name = "lblName";
            label.Text = "Your name";
            label.Anchor = AnchorStyles.Left | AnchorStyles.Top;

            textBox = new TextBox;
            textBox.Location = new Point(10, 30);
            textBox.Size = new System.Drawing.Size(90, 20);
            textBox.Name = "txtName";
            textBox.Anchor = AnchorStyles.Left;

            button = new Button;
            button.Location = new Point(200, 260);
            button.Size = new System.Drawing.Size(90, 20);
            button.Name = "Name button";
            button.Text = "Your name";
            button.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            button.add_Click(this.buttonClicked);

            panel = new Panel;
            panel.Location= new Point(0,0);
            panel.Size= new System.Drawing.Size(300,300);
            panel.Name= "panel";
            panel.Anchor= AnchorStyles.Top | AnchorStyles.Left;

            //add all child controls to panel
            panel.Controls.Add(label);
            panel.Controls.Add(textBox);
            panel.Controls.Add(button);

            //add the panel to main form
            this.Controls.Add(panel);
        }

        function buttonClicked(sender : Object, e : System.EventArgs) {
            Console.WriteLine("sender name {0}", sender.Name);
            MessageBox.Show(String.Format("Hello {0}", textBox.Text));
        }
    }
}

//สร้าง mainForm object โดยชื่อ full name และ run ขึ้นมา
Application.Run(new CodeSanook.Example.JScript.NET.MainForm());