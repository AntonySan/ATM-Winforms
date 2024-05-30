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
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0; // Товщина межі кнопки (регулюйте за потребою)
            return button;
        }

        public Label CreateLabel(string text, Point location)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = location;
            label.Font = new Font("Segue UI", 12, FontStyle.Regular);
            label.BackColor = Color.Transparent;
            label.AutoSize = true;
            return label;
        }

        public TextBox CreateTextBox(Point location, Size size)
        {
            TextBox textBox = new TextBox();
            textBox.Location = location;
            textBox.Size = size;
            textBox.Multiline = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("Segue UI",20);
            return textBox;
        }
       
       public ComboBox CreateComboBox(Point location, Size size, string[] items)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Location = location;
            comboBox.Size = size;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList; // Пример настройки стиля выпадающего списка
            comboBox.Font = new Font("Segoe UI", 12); // Пример настройки шрифта

            if (items != null)
            {
                comboBox.Items.AddRange(items); // Добавляем элементы в ComboBox
            }

            return comboBox;
        }
    }
}
