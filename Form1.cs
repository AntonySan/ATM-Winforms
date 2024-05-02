using ATM_Winforms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Transactions;
using System.Windows.Forms;


namespace ATM_APP { 
public partial class Base_Form : Form 
{
        public Base_Form(string name, string backgroundImagePath)
        {
            this.ClientSize = new Size(844, 572);
            Name = name;
            StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = new System.Drawing.Bitmap(backgroundImagePath);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

        }
       
    }


public partial class InsertCardForm : Base_Form
 {
     DatabaseManager databaseManager = new DatabaseManager();
     Create_Ui_Element create_ui_element = new Create_Ui_Element();
     //const string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\anton\\source\\repos\\ATM Winforms\\Database1.accdb\";Persist Security Info=True";
     //SqlConnection sqlConnection = new SqlConnection(connString);
     private Button[] buttons;
     private Label[] labels;
     private TextBox[] textBoxes;


     File_Creator fileCreator = new File_Creator();

     public InsertCardForm() : base("InsertCard", Resource_Paths.LoginForm)
        {
         InitializeComponent();
     }

     private void InitializeComponent()
     {
         this.ClientSize = new Size(828, 599);
         Name = "InsertCardForm";
         StartPosition = FormStartPosition.CenterScreen;
         this.BackgroundImage = new System.Drawing.Bitmap(@"C:\Users\anton\source\repos\ATM Winforms\Form imagines\InsertCardForm.png");
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
       // AddTextBox();
         AddButtons();
         //AddLabel();

          //string connString = "Data Source=.\\sqlexpress;Initial Catalog=ATM;Integrated Security=True";

          //DatabaseManager.ConnectToDatabase(this);
          //Dictionary<string, object> parameters = new Dictionary<string, object>();
          //parameters.Add("Transaction_id", "1234 5467 4444 1324");
          //parameters.Add("PIN", "1111");
          // SqlConnection sqlConnection = new SqlConnection(connString);

          //DatabaseManager.WriteData(sqlConnection, "Transaction1", parameters);




//Encryption_Manager encryption_Manager = new Encryption_Manager();
//encryption_Manager.Example();

// ds.IsSSDSerialNumberValid(File_Creator.GetHardDriveSerialNumber());



    }

    private void AddButtons()
    {
        string[] ButtonText = { "Exit" };

        Point[] ButtonLocation = { new Point(262, 503)};

        Size[] ButtonSize = { new Size(305, 55) };

        EventHandler[] ButtonEvent = { ExitButton_Click};

        buttons = create_ui_element.CreateButton(2, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


    }

    private void AddLabel()
    {
        string[] LabelText = { "Exit", "Enter" };

        Point[] Labellocation = { new Point(0, 10), new Point(350, 60) };

        labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
    }

    private void AddTextBox()
    {
        Point[] TextBoxlocation = { new Point(350, 270), new Point(350, 0) };

        Size[] TextBoxsize = { new Size(100,32), new Size(0, 0) };

        textBoxes = create_ui_element.CreateTextBox(2, this, TextBoxlocation, TextBoxsize);

    }

    private void ExitButton_Click(object sender, EventArgs e)
    {
        //Log_In log_In_Form = new Log_In();
        //log_In_Form.ShowDialog();
        this.Close();
    }


}  

public partial class Log_In : Base_Form
  {
      Create_Ui_Element create_ui_element = new Create_Ui_Element();
      private Button[] buttons;
      private Label[] labels;
      private TextBox[] textBoxes;
      public Log_In() : base("Log In",Resource_Paths.LoginForm) 
      {
          InitializeComponent();
          AddTextBox();
          AddButtons();
      }

      private void InitializeComponent()
      {
          this.ClientSize = new Size(828, 599);
          Name = "Log_In";
          StartPosition = FormStartPosition.CenterScreen;
          this.BackgroundImage = new System.Drawing.Bitmap(@"C:\Users\anton\source\repos\ATM Winforms\Form imagines\LoginForm.png");
          this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      }

      private void AddButtons()
      {
          string[] ButtonText = { "Exit" };

          Point[] ButtonLocation = { new Point(262, 457) };

          Size[] ButtonSize = { new Size(304, 54) };

          EventHandler[] ButtonEvent = { ExitButton_Click };

          buttons = create_ui_element.CreateButton(2, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


      }



      private void AddTextBox()
      {
          Point[] TextBoxlocation = { new Point(262, 272), new Point(262, 357) };
          Size[] TextBoxsize = { new Size(304, 32), new Size(304, 32) };

          textBoxes = create_ui_element.CreateTextBox(2, this, TextBoxlocation, TextBoxsize);

          //textBoxes[1].BackColor = Color.Black;
      }

      private void ExitButton_Click(object sender, EventArgs e)
      {
          Log_In log_In_Form = new Log_In();
          log_In_Form.ShowDialog();
          this.Close();
      }
  }        
public partial class Main_Menu : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public Main_Menu() : base( "Main Menu", Resource_Paths.MainForm)
        {
            AddButtons();
            AddLabel();
        }

        

        private void AddButtons()
        {
            string[] ButtonText = 
            { 
                "Exit", "Exit", "Exit", 
                "Exit", "Exit", "Exit", 
                "Exit", "Exit", "Exit" 
            };

            Point[] ButtonLocation = 
            {
                new Point(43, 325), new Point(312, 325), new Point(581, 325),
                new Point(43, 400), new Point(312, 400), new Point(581, 400), 
                new Point(43, 475), new Point(312, 475) , new Point(581, 475)
            };

            Size[] ButtonSize = 
            { 
                new Size(220, 53), new Size(220, 53), new Size(220, 53),
                new Size(220, 53), new Size(220, 53), new Size(220, 53), 
                new Size(220, 53), new Size(220, 53), new Size(220, 53)
            };

            EventHandler[] ButtonEvent = 
            { 
                ExitButton_Click, ExitButton_Click, ExitButton_Click, 
                ExitButton_Click, ExitButton_Click, ExitButton_Click,
                ExitButton_Click, ExitButton_Click, ExitButton_Click 
            };

            buttons = create_ui_element.CreateButton(9, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {                                       
            string[] LabelText = { "15:35", "вихід" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20) };

            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

public partial class TransferToTheCardForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public TransferToTheCardForm() : base("Переказ на картку", Resource_Paths.TransferToTheCardForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 476)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation = 
            {
              new Point(267, 241),
              new Point(267, 313), 
              new Point(267, 385) 
            };

            Size[] TextBoxsize = 
            { 
                new Size(310, 30), 
                new Size(310, 30), 
                new Size(310, 30) 
            };

            textBoxes = create_ui_element.CreateTextBox(3, this, TextBoxlocation, TextBoxsize);
            
           
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }

    }

    public partial class CharityForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public CharityForm() : base("Благодійність", Resource_Paths.CharityForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 481)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation =
            {
              new Point(267, 226),
              new Point(267, 293),
              new Point(267, 360)
            };

            Size[] TextBoxsize =
            {
                new Size(310, 30),
                new Size(310, 30),
                new Size(310, 30)
            };

            textBoxes = create_ui_element.CreateTextBox(3, this, TextBoxlocation, TextBoxsize);


        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

    public partial class CashWithdrawalForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public CashWithdrawalForm() : base("Зняття готівки", Resource_Paths.CashWithdrawalForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 476)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід", "300₴" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20), new Point(257, 239) };

           

            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            labels[2].Font = new Font("Segue UI",32);
            labels[2].ForeColor = Color.White;
        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation =
            {
              new Point(267, 324),
              new Point(267, 396)
              
            };

            Size[] TextBoxsize =
            {
                new Size(310, 30),
                new Size(310, 30)
            };

            textBoxes = create_ui_element.CreateTextBox(2, this, TextBoxlocation, TextBoxsize);


        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }
    public partial class ReplenishTheCardForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public ReplenishTheCardForm() : base("Поповнити картку", Resource_Paths.ReplenishTheCardForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 496)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);
           

        }

        private void AddLabel()
        {
            string[] LabelText = 
            {
                "15:35", 
                "вихід", 
                "300₴",
                "300₴" 
            };

            Point[] Labellocation = 
            { 
                new Point(24, 20),
                new Point(780, 20),
                new Point(257, 239),
                new Point(260, 316)
            };



            labels = create_ui_element.CreateLabel(5, this, LabelText, Labellocation);
            labels[2].Font = new Font("Segue UI", 32);
            labels[2].ForeColor = Color.White;
            labels[3].Font = new Font("Segue UI", 32);
            labels[3].ForeColor = Color.White;
        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation =
            {
              new Point(396, 239),

            };

            Size[] TextBoxsize =
            {
                new Size(100, 43),
            };

            textBoxes = create_ui_element.CreateTextBox(1, this, TextBoxlocation, TextBoxsize);
            textBoxes[0].BackColor = Color.FromArgb(191, 191, 191);

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

    public partial class UtilityBillsForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public UtilityBillsForm() : base("Комунальні платежі", Resource_Paths.UtilityBillsForm)
        {
            AddButtons();
            AddLabel();
            AddFlowLayoutPanel();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 476)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText =
            {
                "15:35",
                "вихід",
                "300₴"
            };

            Point[] Labellocation =
            {
                new Point(24, 20),
                new Point(780, 20),
                new Point(516, 410)
            };



            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            labels[2].Font = new Font("Segue UI", 20);
            labels[2].ForeColor = Color.Black;
        }
        private void AddFlowLayoutPanel()
        {
            Panel panel = new Panel();
            panel.AutoScroll = true;
            panel.Size = new Size(408, 262); // Розмір панелі
            panel.Location = new Point(218, 140); // Розташування панелі на формі
            panel.BackColor = Color.FromArgb(224, 219, 219);
            this.Controls.Add(panel); // Додайте панель до форми

            int checkBoxWidth = 300; // Задайте бажану фіксовану ширину чекбоксу
            int verticalSpacing = 50; // Задайте бажаний вертикальний інтервал між чекбоксами

            for (int i = 0; i < 20; i++)
            {
                string part1 = "НазваааааааааааааааНазвааааааааааааааа\nНазвааааааааааааааа\nНа";
                string part2 = "Сума";

                CheckBox checkBox = new CheckBox();
                checkBox.Text = part1;
                checkBox.Location = new Point(0, i * (checkBox.Height + verticalSpacing)); // Збільшення вертикального інтервалу
                checkBox.AutoSize = true; // Зробіть чекбокс автоматично змінюваним за розміром

                // Встановлення фіксованої ширини чекбоксу
                checkBox.Width = checkBoxWidth;

                panel.Controls.Add(checkBox); // Додайте чекбокс до панелі

                // Додайте лейбл справа від чекбоксу
                Label label = new Label();
                label.Text = part2;
                label.AutoSize = true;
                label.Location = new Point(checkBox.Right + 70, checkBox.Top); // Розташуйте лейбл справа від чекбоксу
                panel.Controls.Add(label); // Додайте лейбл до панелі
            }
        }
            private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

    public partial class FinesForm1 : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public FinesForm1() : base("Введіть номерний знак", Resource_Paths.FinesForm1)
        {
            AddButtons();
            AddLabel();
            AddTextBox();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 417)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText =
            {
                "15:35",
                "вихід"
                
            };

            Point[] Labellocation =
            {
                new Point(24, 20),
                new Point(780, 20)
               
            };



            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);

        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation =
            {
               new Point(295, 253),

            };

            Size[] TextBoxsize =
            {
                new Size(245, 64),
            };

            textBoxes = create_ui_element.CreateTextBox(1, this, TextBoxlocation, TextBoxsize);
            textBoxes[0].BackColor = Color.FromArgb(224, 219, 219);

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }


    public partial class FinesForm2 : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public FinesForm2() : base("Сплатити штраф", Resource_Paths.FinesForm2)
        {
            AddButtons();
            AddLabel();
            AddFlowLayoutPanel();
        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 476)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText =
            {
                "15:35",
                "вихід",
                "300₴"
            };

            Point[] Labellocation =
            {
                new Point(24, 20),
                new Point(780, 20),
                new Point(516, 410)
            };



            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            labels[2].Font = new Font("Segue UI", 20);
            labels[2].ForeColor = Color.Black;
        }
        private void AddFlowLayoutPanel()
        {
            Panel panel = new Panel();
            panel.AutoScroll = true;
            panel.Size = new Size(408, 262); // Розмір панелі
            panel.Location = new Point(218, 140); // Розташування панелі на формі
            panel.BackColor = Color.FromArgb(224, 219, 219);
            this.Controls.Add(panel); // Додайте панель до форми

            int checkBoxWidth = 300; // Задайте бажану фіксовану ширину чекбоксу
            int verticalSpacing = 50; // Задайте бажаний вертикальний інтервал між чекбоксами

            for (int i = 0; i < 20; i++)
            {
                string part1 = "НазваааааааааааааааНазвааааааааааааааа\nНазвааааааааааааааа\nНа";
                string part2 = "Сума";

                CheckBox checkBox = new CheckBox();
                checkBox.Text = part1;
                checkBox.Location = new Point(0, i * (checkBox.Height + verticalSpacing)); // Збільшення вертикального інтервалу
                checkBox.AutoSize = true; // Зробіть чекбокс автоматично змінюваним за розміром

                // Встановлення фіксованої ширини чекбоксу
                checkBox.Width = checkBoxWidth;

                panel.Controls.Add(checkBox); // Додайте чекбокс до панелі

                // Додайте лейбл справа від чекбоксу
                Label label = new Label();
                label.Text = part2;
                label.AutoSize = true;
                label.Location = new Point(checkBox.Right + 70, checkBox.Top); // Розташуйте лейбл справа від чекбоксу
                panel.Controls.Add(label); // Додайте лейбл до панелі
            }







        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

    public partial class InternetForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public InternetForm() : base("Інтернет", Resource_Paths.InternetForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();


        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 476)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід", "150$" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20), new Point(260, 414) };


            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            labels[2].Font = new Font("Segue UI",32);
            labels[2].ForeColor = Color.White;
        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation =
            {
              new Point(267, 219),
              new Point(267, 281),
              new Point(267, 343)
            };

            Size[] TextBoxsize =
            {
                new Size(310, 30),
                new Size(310, 30),
                new Size(310, 30)
            };

            textBoxes = create_ui_element.CreateTextBox(3, this, TextBoxlocation, TextBoxsize);
     
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

    public partial class TransferByRequisitesForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public TransferByRequisitesForm() : base("Переказ за реквізитами", Resource_Paths.TransferByRequisitesForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();


        }
        private void AddButtons()
        {
            string[] ButtonText =
            {
                "Exit"
            };

            Point[] ButtonLocation =
            {
                new Point(267, 476)
            };

            Size[] ButtonSize =
            {
                new Size(310, 53)
            };

            EventHandler[] ButtonEvent =
            {
                ExitButton_Click
            };

            buttons = create_ui_element.CreateButton(1, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід", "150$" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20), new Point(260, 414) };


            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            labels[2].Font = new Font("Segue UI", 32);
            labels[2].ForeColor = Color.White;
        }
        private void AddTextBox()
        {
            Point[] TextBoxlocation =
            {
              new Point(267, 219),
              new Point(267, 281),
              new Point(267, 343)
            };

            Size[] TextBoxsize =
            {
                new Size(310, 30),
                new Size(310, 30),
                new Size(310, 30)
            };

            textBoxes = create_ui_element.CreateTextBox(3, this, TextBoxlocation, TextBoxsize);

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

    public partial class PaymentHistoryForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        public PaymentHistoryForm() : base("Історія платежів", Resource_Paths.PaymentHistoryForm)
        {
            AddLabel();
            AddPanel();


        }
        

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
        
        }

        private void AddPanel()
        {
            // Створюємо флоу-панель
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
             // Заповнює батьківський контейнер
            flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight; // Змінюємо напрямок розташування
            flowLayoutPanel.Size = new Size(465,167);
            flowLayoutPanel.Location = new Point(190,405);
            flowLayoutPanel.BackColor = Color.Transparent;
            // Додаємо флоу-панель до форми
            Controls.Add(flowLayoutPanel);

            // Додаємо мітки до флоу-панелі
            for (int i = 0; i < 9; i++)
            {
                Label label = new Label();
                label.Text = $"Мітка {i + 1}";
                flowLayoutPanel.Controls.Add(label);
            }

            // Налаштовуємо властивості флоу-панелі
            flowLayoutPanel.WrapContents = true; // Забороняємо автоматичний перенос елементів на новий рядок
           // flowLayoutPanel.AutoSize = true; // Автоматично встановлюємо розмір флоу-панелі залежно від вмісту
        }




        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
    }

}
