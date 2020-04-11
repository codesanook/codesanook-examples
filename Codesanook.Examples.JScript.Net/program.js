//Import all required packages.
import System;
import System.Windows.Forms;
import System.Drawing;
import Accessibility;

//Define package or namespace.
package CodeSanook.Example.JScript.NET {
    //Create a main class that extends from System.Windows.Forms.Form.
    class MainForm extends System.Windows.Forms.Form {

        //Define Windows Forms controls.
        private var label: Label;
        private var textBox: TextBox;
        private var button: Button;
        private var panel: Panel;

        //Construct controls in the class's constructor. 
        function MainForm() {
            //Set name and size of the main form
            this.Text = "JScript.NET WinForm";
            this.ClientSize = new System.Drawing.Size(300, 180);

            //Set the main form to show at the center of the screen
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            //Create a label control
            label = new Label;
            label.Location = new Point(75, 30);
            label.Size = new System.Drawing.Size(150, 20);
            label.Text = "Please enter your name:";

            //Create a text box control
            textBox = new TextBox;
            textBox.Location = new Point(75, 80);
            textBox.Size = new System.Drawing.Size(150, 20);

            //Create a button control
            button = new Button;
            button.Location = new Point(100, 130);
            button.Size = new System.Drawing.Size(100, 20);
            button.Text = "Say hello";

            //Set event handle of button click event to buttonClicked function
            button.add_Click(this.buttonClicked);

            //Create a panel control
            panel = new Panel;
            panel.Location= new Point(0,0);
            panel.Size= new System.Drawing.Size(300, 180);

            //Add all children controls to the panel
            panel.Controls.Add(label);
            panel.Controls.Add(textBox);
            panel.Controls.Add(button);

            //Add the panel to the main form
            this.Controls.Add(panel);
        }

        //Event handler function of a button clicked
        function buttonClicked(sender : Object, e : System.EventArgs) {
            //Output results to Console and MessageBox UI
            Console.WriteLine("Sender type: {0}", sender.GetType().FullName);
            MessageBox.Show(String.Format("Hello {0}!!!", textBox.Text));
        }
    }
}

//Create the mainForm object with full name and lauch it with Application.Run 
Application.Run(new CodeSanook.Example.JScript.NET.MainForm());