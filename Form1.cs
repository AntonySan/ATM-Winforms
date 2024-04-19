using ATM_Winforms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Transactions;
using System.Windows.Forms;
namespace ATM_APP
{
    public partial class Intro : Form
    {

        ProgressBar progressBar = new ProgressBar();
        Label percent = new Label();
        System.Windows.Forms.Timer time = new System.Windows.Forms.Timer();
        
        public Intro()
        {
            this.Load += Intro_load;
            InitializeComponent();

          

            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
            progressBar.Location = new Point(50, 50);
            progressBar.Size = new Size(400, 100);


            percent.Location = new Point(50, 50);
            percent.AutoSize = true;
            this.Controls.Add(percent);
            this.Controls.Add(progressBar);

            time.Tick += Timer_Tick;

        }

        private void Intro_load(object sender, EventArgs e)
        {
            time.Start();
            progressBar.Step = 1;
        }
        private void InitializeComponent()
        {
            ClientSize = new Size(800, 950);
            Name = "Intro";
            StartPosition = FormStartPosition.CenterScreen;

        }
        int start = 0;
        private void Timer_Tick(object sender, EventArgs e)
        {
            start += 1;

            progressBar.Value = start;
            percent.Text = "" + start;


            if (progressBar.Value >= progressBar.Maximum)
            {

                time.Stop();
                progressBar.Value = progressBar.Maximum;


                InsertCardForm insertCardForm = new InsertCardForm();

                this.Hide();
                insertCardForm.Show();

            }


        }

    }
    public partial class InsertCardForm : Form
    {
        DatabaseManager databaseManager = new DatabaseManager();
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        //const string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\anton\\source\\repos\\ATM Winforms\\Database1.accdb\";Persist Security Info=True";
        //SqlConnection sqlConnection = new SqlConnection(connString);
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        

        File_Creator fileCreator = new File_Creator();
        
        public InsertCardForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ClientSize = new Size(800, 950);
            Name = "InsertCardForm";
            StartPosition = FormStartPosition.CenterScreen;
            AddButtons();
            AddLabel();



            DatabaseManager.ConnectToDatabase(this);
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("Transaction_id", "1234 5467 4444 1324");
            //parameters.Add("PIN", "1111");
            // SqlConnection sqlConnection = new SqlConnection(connString);


            //Dictionary<string, object> parameters11 = new Dictionary<string, object>();
            //parameters11.Add("Transaction", "11111");

            //DatabaseManager.WriteData(sqlConnection, "Transaction", parameters11);

            //sqlConnection.Close();


            //sqlConnection.Open();
            //    List<string> fields = new List<string>() { "Transaction", "card_id" };
            //    DatabaseManager.ReadData(sqlConnection, "Transaction", fields);
            //    sqlConnection.Close(); // Закриваємо перший DataReader



            //Encryption_Manager encryption_Manager = new Encryption_Manager();
            //encryption_Manager.Example();

            // ds.IsSSDSerialNumberValid(File_Creator.GetHardDriveSerialNumber());



        }

        private void AddButtons()
        {
            string[] ButtonText = { "Exit", "Enter" };

            Point[] ButtonLocation = { new Point(10, 10), new Point(400, 60) };

            Size[] ButtonSize = { new Size(100, 100), new Size(200, 200) };

            EventHandler[] ButtonEvent = { ExitButton_Click, EnterButton_Click };

            buttons = create_ui_element.CreateButton(2, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddLabel()
        {
            string[] LabelText = { "Exit", "Enter" };

            Point[] Labellocation = { new Point(0, 10), new Point(350, 60) };

            Size[] Labelsize = { new Size(100, 100), new Size(200, 200) };

            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation, Labelsize);
        }

        private void AddTextBox()
        {
            Point[] TextBoxlocation = { new Point(0, 10), new Point(350, 60) };

            Size[] TextBoxsize = { new Size(100, 100), new Size(200, 200) };

            textBoxes = create_ui_element.CreateTextBox(2, this, TextBoxlocation, TextBoxsize);
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("EXIT");
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ENTER");
        }
    }

    public partial class Log_In
    {
        public Log_In()
        {

        }

    }

    public partial class Main_Menu
    {
        public Main_Menu()
        {

        }
    }

    public partial class Money_Transfer
    {
        public Money_Transfer()
        {

        }

    }

    public partial class Payments
    {
        public Payments()
        {

        }
    }
}
