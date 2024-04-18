using System;
using System.Drawing;
using System.Windows.Forms;

namespace ATM_APP
{
    internal class Create_Ui_Element
    {
         Ui_Element ui_Element = new Ui_Element();

        public Button[] CreateButton(short arraySize, Form form, string[] ButtonText, Point[] ButtonLocation, Size[] ButtonSize, EventHandler[] ButtonEvent)
        {
            Button[] buttons = new Button[arraySize];
            string[] Button_text = ButtonText;

            Point[] Button_location = ButtonLocation;

            Size[] Button_size = ButtonSize;

            EventHandler[] Button_Event = ButtonEvent;

            for (short i = 0; i < Button_text.Length; i++)
            {
                buttons[i] = ui_Element.CreateButton(Button_text[i], Button_location[i], Button_size[i], Button_Event[i]);
                
            }
            form.Controls.AddRange(buttons);
            return buttons;
        }

         public Label[] CreateLabel(short arraySize, Form form, string[] LabelText, Point[] LabelLocation, Size[] LabelSize)
        {
            Label[] labels = new Label[arraySize];

            string[] Label_text = LabelText;

            Point[] Label_location = LabelLocation;

            Size[] Label_size = LabelSize;

            for (short i = 0; i< LabelText.Length; i++)
            {
                labels[i] = ui_Element.CreateLabel(Label_text[i], Label_location[i], Label_size[i]);
                
            }
            form.Controls.AddRange(labels);
            return labels;
        }

        public TextBox[] CreateTextBox(short arraySize, Form form,  Point[] TextBoxLocation, Size[] TextBoxSize)
        {
            TextBox[] textBoxes  = new TextBox[arraySize];

            Point[] TextBox_Location = TextBoxLocation;

            Size[] TexBox_size = TextBoxSize;

            for(short i = 0; i<TextBoxLocation.Length; i++)
            {
                textBoxes[i] = ui_Element.CreateTextBox(TextBoxLocation[i], TextBoxSize[i]);
            }

            form.Controls.AddRange(textBoxes);  
            return textBoxes;

        }
        
    }
}
