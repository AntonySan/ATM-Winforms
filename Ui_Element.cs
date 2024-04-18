using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATM_APP
{
    internal class Ui_Element
    {
        public Button CreateButton(string text, Point location, Size size, EventHandler eventHandler)
        {
            Button button = new Button();
            button.Text = text;
            button.Location = location;
            button.Size = size;
            button.Click += eventHandler;
           return button;
        }

        public Label CreateLabel(string text, Point location, Size size)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = location;
            label.Size = size;
            return label;
        }

        public TextBox CreateTextBox(Point location, Size size)
        {
            TextBox textBox = new TextBox();
            textBox.Location = location;
            textBox.Size = size;
            return textBox;
        }
    }
}
