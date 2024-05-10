using ATM_Winforms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Transactions;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Text;
using System.Xml.Serialization;
using System.Net;
using Microsoft.Identity.Client.NativeInterop;
using System.Security.AccessControl;
using System.Runtime.CompilerServices;
using static ATM_Winforms.CompanyDetails;
using ATM_Winforms.Design_Forms;

namespace ATM_APP
{

    public partial class Base_Form : Form
    {
        private Label[] labels;
        private System.Windows.Forms.Timer timer;
        private Label time_Label, exit_Label;
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        public Base_Form(string name, string backgroundImagePath)
        {
            this.ClientSize = new Size(844, 572);
            Name = name;
            StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = new System.Drawing.Bitmap(backgroundImagePath);
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            AddTimeAndExitLabels();
        }
        private void AddTimeAndExitLabels()
        {
            string[] LabelText = { DateTime.Now.ToString("HH:mm"), "вихід" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20) };

            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
            time_Label = labels[0];
            exit_Label = labels[1];

            // Надаємо мітці "вихід" клікабельність
            exit_Label.Cursor = Cursors.Hand;
            exit_Label.Click += Exit_Label_Click;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000; // 1 хвилина (60 секунд)
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            labels[0].Text = DateTime.Now.ToString("HH:mm"); // Форматуємо дату тільки з годинами і хвилинами
        }

        private void Exit_Label_Click(object sender, EventArgs e)
        {
            if (this is Log_In || this is InsertCardForm || this is Main_Menu)
            {
                Application.Exit();
            }
            else
            {
                Main_Menu main_Menu = new Main_Menu();
                main_Menu.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

                main_Menu.Show();
                this.Hide();
            }
          
        }
    }


    public partial class InsertCardForm : Base_Form
 {
     DatabaseManager databaseManager = new DatabaseManager();
     Create_Ui_Element create_ui_element = new Create_Ui_Element();
     //const string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\anton\\source\\repos\\ATM Winforms\\Database1.accdb\";Persist Security Info=True";
     //SqlConnection sqlConnection = new SqlConnection(connString);
     private Button[] buttons;
  

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
         this.BackgroundImage = new System.Drawing.Bitmap(Resource_Paths.InsertCardForm);
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
       
         AddButtons();
            Update_DB update_DB = new Update_DB();
            update_DB.UpdateRates();
          string connString = Resource_Paths.DB_connectionString;

          DatabaseManager.ConnectToDatabase(this);
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
        string[] ButtonText = { "Вставити картку" };

        Point[] ButtonLocation = { new Point(262, 503)};

        Size[] ButtonSize = { new Size(305, 55) };

        EventHandler[] ButtonEvent = { ExitButton_Click};

        buttons = create_ui_element.CreateButton(2, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


    }

     private void ExitButton_Click(object sender, EventArgs e)
        {
            Log_In log_In_Form = new Log_In();
            log_In_Form.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            log_In_Form.Show();
            this.Hide();
        }



    }
    //Ready
    public partial class Log_In : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private  TextBox[] textBoxes;
        private static TextBox NumberCard_Tbx, Pin_Tbx;

        public Log_In() : base("Log In", Resource_Paths.LoginForm)
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
            NumberCard_Tbx = textBoxes[0];
            Pin_Tbx = textBoxes[1];
            NumberCard_Tbx.TextChanged += NumberCard_Tbx_TextChanged;
            Pin_Tbx.TextChanged += Pin_Tbx_TextChanged;
        
        }
        
        public static void NumberCard_Tbx_TextChanged(object sender, EventArgs e)
        {
            // Отримуємо текст з текстового поля
            string text = NumberCard_Tbx.Text;

            // Видаляємо всі пробіли з тексту, щоб зручніше було працювати з номером картки
            text = text.Replace(" ", "");

            // Форматуємо текст у вигляді "1111 1111 1111 1111"
            if (text.Length > 0)
            {
                StringBuilder formattedText = new StringBuilder(text);

                // Додаємо пробіли після кожних чотирьох цифр
                for (int i = 4; i < formattedText.Length; i += 5)
                {
                    formattedText.Insert(i, ' ');
                }

                // Встановлюємо відформатований текст у текстове поле
                NumberCard_Tbx.Text = formattedText.ToString();

                // Переміщаємо курсор на кінець тексту
                NumberCard_Tbx.SelectionStart = NumberCard_Tbx.Text.Length;
            }
            // Перевіряємо, чи введено більше 19 символів
            if (NumberCard_Tbx.Text.Length > 19)
            {
                // Якщо досягнуто максимальної довжини, видаляємо останній введений символ
                NumberCard_Tbx.Text = NumberCard_Tbx.Text.Remove(19);
                // Переміщаємо курсор на кінець тексту
                NumberCard_Tbx.SelectionStart = NumberCard_Tbx.Text.Length;
            }
        }
        private void Pin_Tbx_TextChanged(object sender, EventArgs e)
        {
            // Перевіряємо, чи введено більше 4 символів
            if (Pin_Tbx.Text.Length > 6)
            {
                // Якщо досягнуто максимальної довжини, видаляємо останній введений символ
                Pin_Tbx.Text = Pin_Tbx.Text.Remove(6);
                // Переміщаємо курсор на кінець тексту
                Pin_Tbx.SelectionStart = Pin_Tbx.Text.Length;
            }
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            VerifyCardAndPin();
           
            
        }
        private void VerifyCardAndPin()
        {
            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                // Відкриваємо з'єднання з базою даних
                connection.Open();

                // Складаємо SQL-запит для вибору запису з таблиці, де номер картки і пін співпадають з введеними користувачем
                string query = "SELECT COUNT(*) FROM ATM_info WHERE Card_number = @NumberCard AND PIN = @Pin";

                // Створюємо команду SQL
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Параметризуємо запит, передаючи значення з NumberCard_Tbx та Pin_Tbx
                    command.Parameters.AddWithValue("@NumberCard", NumberCard_Tbx.Text);
                    command.Parameters.AddWithValue("@Pin", Pin_Tbx.Text);

                    // Виконуємо запит та отримуємо результат
                    int count = (int)command.ExecuteScalar();
                   
                    // Перевіряємо, чи знайдено запис, що відповідає введеним даним
                    if (count > 0)
                    {
                        GetUserByCardNumberAndPassword();
                        Main_Menu main_Menu = new Main_Menu();
                        main_Menu.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

                        main_Menu.Show();
                        this.Hide();
                      
                    }
                    else
                    {
                        // Якщо запис не знайдено, виводиться повідомлення про помилку
                        MessageBox.Show("Неправильний номер картки або Пін!");
                        
                    }
                }
            }
        }
        public static void GetUserByCardNumberAndPassword()
        {
            
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
                {
                conn.Open();

                string query = "SELECT * FROM ATM_info WHERE Card_number = @CardNumber AND PIN = @Password";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Передача значень параметрів в запит
                    cmd.Parameters.AddWithValue("@CardNumber", NumberCard_Tbx.Text);
                    cmd.Parameters.AddWithValue("@Password", Pin_Tbx.Text);

                    SqlDataReader reader = cmd.ExecuteReader();

                    // Читання результатів запиту
                    if (reader.Read())
                    {
                    int id = (int)reader["Id"];
                    string retrievedCardNumber = reader["Card_number"].ToString();
                    string name = reader["Full_name"].ToString();
                    string retrievedPassword = reader["PIN"].ToString();
                    string expirationDate = reader["Expiration_date"].ToString();
                    string paymentSystem = reader["Payment_system"].ToString();
                    string balance = reader["balance"].ToString();
                    string address = reader["address"].ToString();
                    string issueDate = reader["Issue_Date"].ToString();
                    string cvv_cvc = reader["CVV/CVC "].ToString();
                    string cardStatus = reader["Card_Status"].ToString();
                    string spendingLimit = reader["Spending_Limit"].ToString();
                    string issuingBank = reader["Issuing_bank"].ToString();
                    string cardType = reader["Card_Type"].ToString();

                    // Створюємо об'єкт користувача та додаємо його до списку
                    User user = new User(id, retrievedCardNumber, name, retrievedPassword, expirationDate, paymentSystem, balance, address, issueDate, cvv_cvc, cardStatus, spendingLimit, issuingBank, cardType);
                    GlobalData.Users.Add(user);
                    }

                    reader.Close();
                }
        }


    }
    //Ready
    public partial class Main_Menu : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private Label FullNameLabel, CardNumberLabel,PaymentSystemLabel,ExpirationDateLabel;
        static Update_DB update_DB = new Update_DB();
        ExchangeRates exchangeRate = new ExchangeRates();

        public Main_Menu() : base( "Main Menu", Resource_Paths.MainForm)
        {
            GetExchangeRates(Resource_Paths.DB_connectionString,"EUR", "USD");
            AddButtons();
            AddUserLabel();
            AddExchangeRateLabels();
        }

        private void AddButtons()
        {
            string[] ButtonText = 
            { 
                "ЗНЯТТЯ ГОТІВКИ", "ІСТОРІЯ ПЛАТЕЖІВ", "ПОПОВНИТИ КАРТКУ", 
                "ШТРАФИ", "КОМУНАЛЬНІ ПЛАТЕЖІ", "ІНТЕРНЕТ", 
                "ПЕРЕКАЗ НА КАРТКУ", "ПЕРЕКАЗ ЗА РЕКВІЗИТАМИ", "БЛАГОДІЙНІСТЬ" 
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
               Cash_withdrawls_Click, ExitButton_Click, ReplenishTheCard_Click,
               Fines_Click, UtilityBills_Click, Internet_Click,
                TransferToTheCard_Click, TransferByRequisites_Click, Charity_Click
            };

            buttons = create_ui_element.CreateButton(9, this, ButtonText, ButtonLocation, ButtonSize, ButtonEvent);


        }

        private void AddUserLabel()
        {
            
            if (GlobalData.Users.Count >= 1)
            {
                User user = GlobalData.Users[0];
                string[] LabelText = { user.FullName, user.CardNumber, user.PaymentSystem, user.ExpirationDate };

                Point[] Labellocation = { new Point(163, 191), new Point(163, 215), new Point(163, 249), new Point(232, 249) };

                labels = create_ui_element.CreateLabel(4, this, LabelText, Labellocation);
                FullNameLabel = labels[0];
                CardNumberLabel = labels[1];
                PaymentSystemLabel = labels[2];
                ExpirationDateLabel = labels[3];

                FullNameLabel.Font = ExpirationDateLabel.Font = new Font("SEGUE UI",12);
                CardNumberLabel.Font = new Font("SEGUE UI",18,FontStyle.Regular);
                PaymentSystemLabel.Font = new Font("SEGUE UI", 12,FontStyle.Italic);
                MessageBox.Show(user.FullName);
            }
        }
        private void AddExchangeRateLabels()
        {
            // Перевіряємо, чи є хоча б один об'єкт в списку ExchangeRate
            if (ExchangeRateGlobal.ExchangeRate.Count >= 1)
            {
                for (int i = 0; i < ExchangeRateGlobal.ExchangeRate.Count; i++)
                {
                    CurrencyExchangeRate exchangeRate = ExchangeRateGlobal.ExchangeRate[i];

                    // Створюємо текст для міток з кожного об'єкту
                    string[] LabelText = { exchangeRate.PurchaseRate, exchangeRate.SaleRate };

                    // Кожна мітка матиме свої координати
                    Point[] LabelLocation = { new Point(638, 203 + i * 45), new Point(705, 203 + i * 45) };

                    // Створюємо мітки для кожного об'єкту
                    Label[] labels = create_ui_element.CreateLabel(2, this, LabelText, LabelLocation);

                    // Показуємо значення purchaseRate для поточного об'єкту
                    MessageBox.Show(exchangeRate.PurchaseRate);
                }
            }
        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            //Log_In log_In_Form = new Log_In();
            //log_In_Form.ShowDialog();
            //this.Close();
        }
        private void Cash_withdrawls_Click(object sendar, EventArgs e)
        {
            CashWithdrawalForm cashWithdrawalForm = new CashWithdrawalForm();
            cashWithdrawalForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            cashWithdrawalForm.Show();
            this.Hide();
        }
        private void ReplenishTheCard_Click(object sender, EventArgs e)
        {
            ReplenishTheCardForm replenishTheCardForm= new ReplenishTheCardForm();
            replenishTheCardForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            replenishTheCardForm.Show();
            this.Hide();
        }

        private void Fines_Click(object sender, EventArgs e)
        {
            FinesForm1 FinesForm = new FinesForm1();
            FinesForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            FinesForm.Show();
            this.Hide();
        }     
        private void UtilityBills_Click(object sender, EventArgs e)
        {
            UtilityBillsForm utilityBillsForm = new UtilityBillsForm();
            utilityBillsForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            utilityBillsForm.Show();
            this.Hide();
        }

        private void Internet_Click(object sender, EventArgs e)
        {
            InternetForm internetForm = new InternetForm();
            internetForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            internetForm.Show();
            this.Hide();
        }
        private void TransferToTheCard_Click(object sender, EventArgs e)
        {
            TransferToTheCardForm transferToTheCard = new TransferToTheCardForm();
            transferToTheCard.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            transferToTheCard.Show();
            this.Hide();
        }

        private void TransferByRequisites_Click(object senfer, EventArgs e)
        {

            TransferByRequisitesForm transferByRequisites = new TransferByRequisitesForm();
            transferByRequisites.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            transferByRequisites.Show();
            this.Hide();

        }

        private void Charity_Click(object senfer, EventArgs e)
        {

            CharityForm charityForm = new CharityForm();
            charityForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            charityForm.Show();
            this.Hide();

        }
        static void GetExchangeRates(string connectionString, params string[] currencies)
        {
            string query = "SELECT Currency, SaleRate, PurchaseRate FROM Exchange_Rates WHERE Currency IN (@Currency1, @Currency2)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Currency1", currencies[0]);
                    command.Parameters.AddWithValue("@Currency2", currencies[1]);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string currency = reader.GetString(0);
                            string saleRate = reader.GetString(1);
                            string purchaseRate = reader.GetString(2);

                            // Перевірка на наявність валюти в списку
                            if (!ExchangeRateGlobal.ExchangeRate.Any(x => x.Currency == currency))
                            {
                                CurrencyExchangeRate newRate = new CurrencyExchangeRate(currency, saleRate, purchaseRate);
                                ExchangeRateGlobal.ExchangeRate.Add(newRate);
                            }
                        }
                    }
                }
            }
        }


    }
    //Ready
    public partial class CashWithdrawalForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private Label CardBalance;
        private TextBox WithdrawAmount_Tbx, Pin_Tbx;
        private DatabaseManager databaseManager = new DatabaseManager();
        User user = GlobalData.Users[0];
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
            string[] LabelText = { user.Balance + "$" };

            Point[] Labellocation = { new Point(257, 239) };

            labels = create_ui_element.CreateLabel(1, this, LabelText, Labellocation);
            CardBalance = labels[0];
           
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
            WithdrawAmount_Tbx = textBoxes[0];
            Pin_Tbx = textBoxes[1];
            WithdrawAmount_Tbx.Font = Pin_Tbx.Font = new Font("Segue UI", 16);

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            int user_Balance = int.Parse(user.Balance);
            int user_WithdrawAmount = int.Parse(WithdrawAmount_Tbx.Text);
            if (Pin_Tbx.Text == user.Password && user_Balance >= user_WithdrawAmount)
            {
                user_Balance = user_Balance - user_WithdrawAmount;
                user.Balance = Convert.ToString(user_Balance);
                CardBalance.Text = user.Balance;
                UpdateUserBalance(user.CardNumber, user.Balance);
               
                databaseManager.UpdateCardData(user.CardNumber, user.CardType,user.IssueDate,user.ExpirationDate,
                    user.FullName,user.Address,user.CVV_CVC,user.Password,int.Parse(user.Balance),user.CardStatus,
                    user.PaymentSystem,user.SpendingLimit,user.IssuingBank);
                //тут треба анімація видання коштів 
                MessageBox.Show("Ви успішно зняли готівку");
               

            }
            else
            {
                MessageBox.Show("неправильний пін або недостаньо коштів");
            }
        }

       private void UpdateUserBalance(string cardNumber, string amount)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                // Складаємо SQL-запит для оновлення балансу користувача
                string query = "UPDATE ATM_info SET balance = @Amount WHERE Card_number = @CardNumber";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Передача значень параметрів в запит
                cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                cmd.Parameters.AddWithValue("@Amount", amount);

                // Виконуємо запит для оновлення балансу
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Баланс користувача успішно оновлено.");
                }
                else
                {
                    MessageBox.Show("Не вдалося оновити баланс користувача.");
                }
            }
        }
    }
    //Ready
    public partial class ReplenishTheCardForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private Label Balance_Label, FundsPaid_Label;
        private TextBox AmountToPay_Tbx;
        private Button Replenish_Btn;
        private DatabaseManager databaseManager = new DatabaseManager();
        User user = GlobalData.Users[0];
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
            Replenish_Btn = buttons[0];

        }

        private void AddLabel()
        {
            string[] LabelText =
            {
                
                user.Balance +"₴",
                "0₴"
            };

            Point[] Labellocation =
            {
                
                new Point(257, 239),
                new Point(260, 316)
            };



            labels = create_ui_element.CreateLabel(5, this, LabelText, Labellocation);
            Balance_Label = labels[0];
            FundsPaid_Label= labels[1];
            Balance_Label.Font = FundsPaid_Label.Font = new Font("Segue UI", 32);
            Balance_Label.ForeColor = FundsPaid_Label.ForeColor=Color.White;

            // Здвигаємо мітку FundsPaid_Label в ліво в залежності від довжини тексту
            int pixelsToShiftFundsPaid = (FundsPaid_Label.Text.Length - 1) * 5; // Здвиг на 15 пікселів за символ
            FundsPaid_Label.Location = new Point(FundsPaid_Label.Location.X - pixelsToShiftFundsPaid, FundsPaid_Label.Location.Y);

            // Здвигаємо мітку AmountToPay_Tbx в ліво в залежності від довжини тексту
            int pixelsToShifBalance= (Balance_Label.Text.Length -1) * 9; // Здвиг на 15 пікселів за символ
            Balance_Label.Location = new Point(Balance_Label.Location.X - pixelsToShifBalance, Balance_Label.Location.Y);

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
            AmountToPay_Tbx = textBoxes[0];
            AmountToPay_Tbx.BackColor = Color.FromArgb(191, 191, 191);
            AmountToPay_Tbx.Leave += AmountToPay_Tbx_Leave;

        }
        private async void ExitButton_Click(object sender, EventArgs e)
        {
            AmountToPay_Tbx.Enabled = false;
            await DepositMoneyAnimationAsync();
        }

        private async Task DepositMoneyAnimationAsync()
        {
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися підібрати більші суми
            int targetAmount = int.Parse(AmountToPay_Tbx.Text);
            int currentAmount = 0;
            Random rnd = new Random();

            while (currentAmount < targetAmount)
            {
                bool found = false;
                foreach (int amount in amounts)
                {
                    if (currentAmount + amount <= targetAmount)
                    {
                        currentAmount += amount;
                        FundsPaid_Label.Text = currentAmount.ToString() + "₴";
                        FundsPaid_Label.Refresh();
                        await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break; // якщо не можна знайти жодної суми, яка б підійшла, вийти з циклу
                }
            }

            GlobalData.ClearUsers();
            UpdateUserBalance(user.CardNumber, currentAmount);
            databaseManager.UpdateCardData(user.CardNumber, user.CardType, user.IssueDate, user.ExpirationDate,
                    user.FullName, user.Address, user.CVV_CVC, user.Password, int.Parse(user.Balance), user.CardStatus,
                    user.PaymentSystem, user.SpendingLimit, user.IssuingBank);
            try
            {
                Log_In.GetUserByCardNumberAndPassword();

                User user = GlobalData.Users[0];
                Balance_Label.Text = user.Balance + "₴";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            int pixelsToShiftFundsPaid = (FundsPaid_Label.Text.Length - 1) * 5;
            FundsPaid_Label.Location = new Point(FundsPaid_Label.Location.X - pixelsToShiftFundsPaid, FundsPaid_Label.Location.Y);

            Replenish_Btn.Enabled = false;
        }



        public static void UpdateUserBalance(string cardNumber, int amount)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                // Складаємо SQL-запит для оновлення балансу користувача
                string query = "UPDATE ATM_info SET balance = balance + @Amount WHERE Card_number = @CardNumber";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Передача значень параметрів в запит
                cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                cmd.Parameters.AddWithValue("@Amount", amount);

                // Виконуємо запит для оновлення балансу
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Баланс користувача успішно оновлено.");
                }
                else
                {
                    MessageBox.Show("Не вдалося оновити баланс користувача.");
                }
            }
        }

        private void AmountToPay_Tbx_Leave(object sender, EventArgs e)
        {
            int value;
            if (!int.TryParse(AmountToPay_Tbx.Text, out value) || value % 10 != 0)
            {
                MessageBox.Show("Введене число повинно бути кратним 10.");
                AmountToPay_Tbx.Focus(); // Повертаємо фокус на текстове поле
            }
        }



    }
    //Ready
    public partial class FinesForm1 : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        static private TextBox licencePlate;
        public FinesForm1() : base("Введіть номерний знак", Resource_Paths.FinesForm1)
        {
            AddButtons();
            
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
            licencePlate = textBoxes[0];
            licencePlate.BackColor = Color.FromArgb(224, 219, 219);
            licencePlate.TextChanged += TextBox_TextChanged; // Додати обробник події TextChanged
            
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                string text = textBox.Text.ToUpper(); // Перевести текст до верхнього регістру

                // Видалити всі символи, крім літер та цифр
                StringBuilder builder = new StringBuilder();
                foreach (char c in text)
                {
                    if (char.IsLetterOrDigit(c))
                    {
                        builder.Append(c);
                    }
                }
                text = builder.ToString();

                // Додати пробіли у відповідних позиціях
                if (text.Length > 2)
                {
                    text = text.Insert(2, " ");
                }
                if (text.Length > 7)
                {
                    text = text.Insert(7, " ");
                }

                textBox.Text = text; // Встановити відформатований текст у текстове поле
                textBox.SelectionStart = textBox.Text.Length; // Встановити позицію курсора в кінець тексту
            }
        }

       public static void ReadFinesData()
        {
            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Fines WHERE license_plates = @LicensePlates AND paid = 0"; // Витягуємо тільки неоплачені штрафи
                
                SqlCommand command = new SqlCommand(query, connection);

                // Передача значень параметрів в запит
                command.Parameters.AddWithValue("@LicensePlates", licencePlate.Text); // Вставте потрібне значення

                SqlDataReader reader = command.ExecuteReader();

                // Читання результатів запиту
                while (reader.Read())
                {
                    int id = (int)reader["Id"]; ;
                    string licensePlates = reader["license_plates"].ToString();
                    string fineDescription = reader["fine"].ToString();
                    string date = reader["date"].ToString();
                    int fineAmount = Convert.ToInt32(reader["fine_amount"]);
                    string paid = reader["paid"].ToString();
                    // Створюємо об'єкт штрафу та додаємо його до списку
                    Fine fine = new Fine(id, licensePlates, fineDescription, date, fineAmount,paid);
                    GlobalFines.Fines.Add(fine);
                }

                reader.Close();
            }
        }


        private void ExitButton_Click(object sender, EventArgs e)
        {
            ReadFinesData();
           
            if (GlobalFines.Fines.Count>=1)
            {
                MessageBox.Show(GlobalFines.Fines.Count.ToString());
                FinesForm2 FinesForm = new FinesForm2();
                FinesForm.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

                FinesForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("У вас немає штрафів");
            }
           
        }
    }
    //Ready
    public partial class FinesForm2 : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private Label AmountDue_Lab;
        User user = GlobalData.Users[0];
        int totalAmount = 0; // Змінна для збереження суми до сплати   
        private Dictionary<int, string> finePaymentStatus = new Dictionary<int, string>();
        private Panel panel = new Panel();
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
                
                "₴"
            };

            Point[] Labellocation =
            {
                
                new Point(516, 410)
            };



            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            AmountDue_Lab = labels[0];
            AmountDue_Lab.Font = new Font("Segue UI", 20);
            AmountDue_Lab.ForeColor = Color.Black;
        }
        private void AddFlowLayoutPanel()
        {
            totalAmount = 0;
            panel.AutoScroll = true;
            panel.Size = new Size(408, 192); // Розмір панелі
            panel.Location = new Point(218, 210); // Розташування панелі на формі
            panel.BackColor = Color.FromArgb(224, 219, 219);
            this.Controls.Add(panel); // Додайте панель до форми

            int checkBoxWidth = 300; // Задайте бажану фіксовану ширину чекбоксу
            int verticalSpacing = 10; // Задайте бажаний вертикальний інтервал між чекбоксами
                                      // Очистити панель від усіх елементів
            panel.Controls.Clear();
            if (GlobalFines.Fines.Count > 0)
            {
                foreach (var fine in GlobalFines.Fines)
                {
                    if (fine.Paid == "0") // Перевіряємо, чи штраф не оплачено
                    {
                        string part1 = fine.FineDescription;
                        string part2 = fine.FineAmount.ToString();
                        int fineID = fine.Id; // Отримання ідентифікатора штрафу

                        CheckBox checkBox = new CheckBox();
                        checkBox.Text = part1;
                        checkBox.Location = new Point(0, panel.Controls.Count * (checkBox.Height + verticalSpacing)); // Збільшення вертикального інтервалу
                        checkBox.Size = new Size(checkBoxWidth, 70);

                        // Встановлення фіксованої ширини чекбоксу
                        checkBox.Width = checkBoxWidth;

                        // Додаємо обробник події для події CheckedChanged
                        checkBox.CheckedChanged += (sender, e) =>
                        {
                            CheckBox cb = sender as CheckBox;
                            if (cb.Checked)
                            {
                                totalAmount += int.Parse(part2);

                                finePaymentStatus[fineID] = "1"; // Позначте штраф як оплачений
                                                                 // Встановлюємо чекбокс як неактивний
                             
                            }
                            else
                            {
                                totalAmount -= int.Parse(part2);
                                
                                finePaymentStatus[fineID] = "0"; // Оновлюємо статус сплачення штрафу на "не сплачено"
                            }
                            AmountDue_Lab.Text = totalAmount.ToString() + "₴";
                            AmountDue_Lab.Refresh();
                        };
                        checkBox.Refresh();
                        panel.Controls.Add(checkBox); // Додайте чекбокс до панелі
                      
                        // Додайте лейбл справа від чекбоксу
                        Label label = new Label();
                        label.Text = part2;
                        label.AutoSize = true;
                        label.Location = new Point(checkBox.Right + 50, checkBox.Top + 20); // Розташуйте лейбл справа від чекбоксу
                        panel.Controls.Add(label); // Додайте лейбл до панелі
                    }
                }
            }
        }
        private async void ExitButton_Click(object sender, EventArgs e)
                /// /// /// /// /// /// /// /// /// /// /// /// /// ///
        {      /// Форма для вибору типу оплати, картка чи готівка ///
              /// /// /// /// /// /// /// /// /// /// /// /// /// ///
            if (int.Parse(user.Balance) >= totalAmount)
            {
                
                await DepositMoneyAnimationAsync();

                MessageBox.Show("Ви успішно оплатили штрафи");
                
                
                GlobalFines.ClearFines();
                
                    // Оновлення бази даних лише після натискання кнопки
                foreach (var kvp in finePaymentStatus)
                {
                    int fineID = kvp.Key;
                    string isPaid = kvp.Value;

                    if (isPaid == "1")
                    {
                        // Оновлення бази даних про статус штрафу на "сплачено"
                        UpdateFinePaymentStatus(fineID, "1");
                    }
                    else
                    {
                        // Оновлення бази даних про статус штрафу на "не сплачено"
                        UpdateFinePaymentStatus(fineID, "0");
                    }
                }

                FinesForm1.ReadFinesData();
                if (GlobalFines.Fines.Count > 0)
                {
                    Fine fine = GlobalFines.Fines[0];
                }

                panel.Controls.Clear();
                AddFlowLayoutPanel();
                panel.Refresh();
       

                // Позначимо всі оплачені чекбокси як неактивні
                foreach (Control control in panel.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                        checkBox.Checked = false;
                    }
                }
            }
        }

        private void UpdateFinePaymentStatus(int fineID, string paid)
        {
            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = "UPDATE Fines SET paid = @Paid WHERE Id = @FineID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FineID", fineID);
                command.Parameters.AddWithValue("@Paid", paid);

                int rowsAffected = command.ExecuteNonQuery();

               
            }
        }
       
        private async Task DepositMoneyAnimationAsync()
        {
            MessageBox.Show("Animation");
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися віднімати більші суми
            int targetAmount = int.Parse(totalAmount.ToString());

            int currentAmount = targetAmount; // Починаємо зі суми штрафу
            Random rnd = new Random();

            while (currentAmount > 0) // Поки сума не стане 0
            {
                bool found = false;
                foreach (int amount in amounts)
                {
                    if (currentAmount - amount >= 0) // Перевірка, чи можна відняти цю суму
                    {
                        currentAmount -= amount; // Віднімаємо випадкову суму
                        AmountDue_Lab.Text = currentAmount.ToString() + "₴";
                        AmountDue_Lab.Refresh();
                        await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break; // якщо не можна відняти жодної суми, вийти з циклу
                }
            }


        }


    }
    //Ready
    public partial class UtilityBillsForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        int totalAmount = 0; // Змінна для збереження суми до сплати   
        private Dictionary<int, string> BillsPaymentStatus = new Dictionary<int, string>();
        private Label AmountDue_Lab;
        private Panel panel = new Panel();
       private static User user = GlobalData.Users[0];
        public UtilityBillsForm() : base("Комунальні платежі", Resource_Paths.UtilityBillsForm)
        {
            ReadUserBills();
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
                
                "0₴"
            };

            Point[] Labellocation =
            {
              
                new Point(516, 410)
            };



            labels = create_ui_element.CreateLabel(3, this, LabelText, Labellocation);
            AmountDue_Lab = labels[0];
            AmountDue_Lab.Font = new Font("Segue UI", 20);
            AmountDue_Lab.ForeColor = Color.Black;
        }
        private void AddFlowLayoutPanel()
        {
            totalAmount = 0;
            panel.AutoScroll = true;
            panel.Size = new Size(408, 192); // Розмір панелі
            panel.Location = new Point(218, 210); // Розташування панелі на формі
            panel.BackColor = Color.FromArgb(224, 219, 219);
            this.Controls.Add(panel); // Додайте панель до форми

            int checkBoxWidth = 300; // Задайте бажану фіксовану ширину чекбоксу
            int verticalSpacing = 10; // Задайте бажаний вертикальний інтервал між чекбоксами
                                      // Очистити панель від усіх елементів
            panel.Controls.Clear();
            MessageBox.Show(GlobalUtility_Bills.utility_Bills.Count.ToString());
            if (GlobalUtility_Bills.utility_Bills.Count > 0)
            {
                foreach (var bills in GlobalUtility_Bills.utility_Bills)
                {
                    if (bills.Paid == "0") // Перевіряємо, чи штраф не оплачено
                    {
                        string part1 = bills.Company_Name;
                        string part2 = bills.Amount.ToString();
                        int billsID = bills.Id; // Отримання ідентифікатора штрафу

                        CheckBox checkBox = new CheckBox();
                        checkBox.Text = part1;
                        checkBox.Location = new Point(0, panel.Controls.Count * (checkBox.Height + verticalSpacing)); // Збільшення вертикального інтервалу
                        checkBox.Size = new Size(checkBoxWidth, 70);

                        // Встановлення фіксованої ширини чекбоксу
                        checkBox.Width = checkBoxWidth;

                        // Додаємо обробник події для події CheckedChanged
                        checkBox.CheckedChanged += (sender, e) =>
                        {
                            CheckBox cb = sender as CheckBox;
                            if (cb.Checked)
                            {
                                totalAmount += int.Parse(part2);

                                BillsPaymentStatus[billsID] = "1"; // Позначте штраф як оплачений
                                                                 // Встановлюємо чекбокс як неактивний

                            }
                            else
                            {
                                totalAmount -= int.Parse(part2);

                                BillsPaymentStatus[billsID] = "0"; // Оновлюємо статус сплачення штрафу на "не сплачено"
                            }
                            AmountDue_Lab.Text = totalAmount.ToString() + "₴";
                            AmountDue_Lab.Refresh();
                        };
                        checkBox.Refresh();
                        panel.Controls.Add(checkBox); // Додайте чекбокс до панелі

                        // Додайте лейбл справа від чекбоксу
                        Label label = new Label();
                        label.Text = part2;
                        label.AutoSize = true;
                        label.Location = new Point(checkBox.Right + 50, checkBox.Top + 20); // Розташуйте лейбл справа від чекбоксу
                        panel.Controls.Add(label); // Додайте лейбл до панелі
                    }
                }
            }
        }
        private async void ExitButton_Click(object sender, EventArgs e)
                /// /// /// /// /// /// /// /// /// /// /// /// /// ///
        {      /// Форма для вибору типу оплати, картка чи готівка ///
              /// /// /// /// /// /// /// /// /// /// /// /// /// ///
            

                await DepositMoneyAnimationAsync();

                MessageBox.Show("Ви успішно оплатили Комуналку");


               
                GlobalUtility_Bills.ClearUtility_Bills();

                // Оновлення бази даних лише після натискання кнопки
                foreach (var kvp in BillsPaymentStatus)
                {
                    int billID = kvp.Key;
                    string isPaid = kvp.Value;

                    if (isPaid == "1")
                    {
                        // Оновлення бази даних про статус штрафу на "сплачено"
                        UpdateUserBillStatus(billID, "1");
                    }
                    else
                    {
                        // Оновлення бази даних про статус штрафу на "не сплачено"
                        UpdateUserBillStatus(billID, "0");
                    }
                }

                
                ReadUserBills();
                if(GlobalUtility_Bills.utility_Bills.Count > 0)
                {
                    Utility_Bills utility_Bills = GlobalUtility_Bills.utility_Bills[0];
                }

                panel.Controls.Clear();
                AddFlowLayoutPanel();
                panel.Refresh();


                // Позначимо всі оплачені чекбокси як неактивні
                foreach (Control control in panel.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                        checkBox.Checked = false;
                    }
                }
            
        }


        private async Task DepositMoneyAnimationAsync()
        {
            MessageBox.Show("Animation");
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися віднімати більші суми
            int targetAmount = int.Parse(totalAmount.ToString());

            int currentAmount = targetAmount; // Починаємо зі суми штрафу
            Random rnd = new Random();

            while (currentAmount > 0) // Поки сума не стане 0
            {
                bool found = false;
                foreach (int amount in amounts)
                {
                    if (currentAmount - amount >= 0) // Перевірка, чи можна відняти цю суму
                    {
                        currentAmount -= amount; // Віднімаємо випадкову суму
                        AmountDue_Lab.Text = currentAmount.ToString() + "₴";
                        AmountDue_Lab.Refresh();
                        await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break; // якщо не можна відняти жодної суми, вийти з циклу
                }
            }


        }

        public static void ReadUserBills()
        {
            

            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = @"SELECT ATM_info.Full_name, ATM_info.address, Utility_Сompanies.Id, Utility_Сompanies.Compani_name, Utility_Сompanies.Amount,Utility_Сompanies.paid
            FROM ATM_info
            JOIN Utility_Сompanies ON ATM_info.address = Utility_Сompanies.Address
            WHERE ATM_info.Card_number = @CardNumber AND ATM_info.Full_name = @FullName AND Utility_Сompanies.paid = 0";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CardNumber",user.CardNumber);
                command.Parameters.AddWithValue("@FullName", user.FullName);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Отримання даних з результатів запиту
                        int id = (int)reader["Id"];
                        string userFullName = reader["Full_name"].ToString();
                        string address = reader["address"].ToString();
                        string companyName = reader["Compani_name"].ToString();
                        int amountToPay = Convert.ToInt32(reader["Amount"]);
                        string is_paid = reader["paid"].ToString();

                        // Створення об'єкту з отриманих даних і додавання його до списку
                        Utility_Bills userData = new Utility_Bills(id, userFullName,companyName, amountToPay, address, is_paid);
                        GlobalUtility_Bills.utility_Bills.Add(userData);
                        MessageBox.Show(address);
                    }
                    
                }
            }
        }

        public static void UpdateUserBillStatus(int billID, string paid)
        {
            

            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = "UPDATE Utility_Сompanies SET paid = @Paid WHERE Id = @BillID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BillID", billID);
                command.Parameters.AddWithValue("@Paid", paid);

                int rowsAffected = command.ExecuteNonQuery();

                // Опціонально: перевірка кількості змінених рядків
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Дані успішно оновлені.");
                }
                else
                {
                    Console.WriteLine("Помилка під час оновлення даних.");
                }
            }
        }


    }
    //Ready
    public partial class InternetForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private TextBox accountNumber_Tbx, address_Tbx, transferAmount_Tbx;
        private Label FundsСontributed_Lab , paidAmount_Label;
        private static Internet internet = GlobalInternetData.internet[0];
        private User user = GlobalData.Users[0];
        private DatabaseManager databaseManager = new DatabaseManager();

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
            string[] LabelText = {  "0$", "0$" };

            Point[] Labellocation = { new Point(260, 414), new Point(510, 414) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
            FundsСontributed_Lab = labels[0];
            paidAmount_Label = labels[1];
            FundsСontributed_Lab.Font = paidAmount_Label.Font=  new Font("Segue UI", 32);
            FundsСontributed_Lab.ForeColor = paidAmount_Label.ForeColor = Color.White;
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
            accountNumber_Tbx = textBoxes[0];
            address_Tbx = textBoxes[1];
            transferAmount_Tbx = textBoxes[2];

            address_Tbx.Leave += TextBox_Leave;

        }

        private void TextBox_Leave(object sender, EventArgs e)
        {
            // Отримання тексту з текстових полів
             
            string address = address_Tbx.Text;
            GetInternetDataByAddressAndAccountNumber();

            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if(GlobalInternetData.internet.Count > 0)
            {
                paidAmount_Label.Text = internet.Transfer_Amount.ToString() + "₴";
            }
           
            
        }
        private async void ExitButton_Click(object sender, EventArgs e)
        {
            if (GlobalInternetData.internet.Count > 0)
            {

           
            int transferAmount = int.Parse(transferAmount_Tbx.Text);
            int requiredAmount = int.Parse(paidAmount_Label.Text.Substring(0, paidAmount_Label.Text.Length - 1));

            if (transferAmount >= requiredAmount)
            {
                // Відображення повідомлення з питанням про перенесення залишку
                DialogResult result = MessageBox.Show("Ви впевнені, що хочете перенести залишок на картку?", "Перенесення залишку", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    await DepositMoneyAnimationAsync();
                    MessageBox.Show("Ви успішно сплатили за послуги з інтернету");
                    UpdateInternetData(DateTime.Now.ToString(), 1);
                    ReplenishTheCardForm.UpdateUserBalance(user.CardNumber, transferAmount - requiredAmount);
                        databaseManager.UpdateCardData(user.CardNumber, user.CardType, user.IssueDate, user.ExpirationDate,
                    user.FullName, user.Address, user.CVV_CVC, user.Password, int.Parse(user.Balance), user.CardStatus,
                    user.PaymentSystem, user.SpendingLimit, user.IssuingBank);
                    }
                else
                {
                    MessageBox.Show("Заберіть решту");
                  
                    
                    int remainingAmount = transferAmount-  requiredAmount;
                    await DepositMoneyAnimationAsync();
                    FundsСontributed_Lab.Text = remainingAmount.ToString() + "₴";
                    FundsСontributed_Lab.Refresh();
                    // Оновлення даних про оплату в базі даних
                    UpdateInternetData(DateTime.Now.ToString(), 1);

                    MessageBox.Show($"Ви успішно сплатили {transferAmount}$ за послуги з інтернету. Залишок {remainingAmount}$ був перенесений на вашу картку.");
                }
            }
            else
            {
                MessageBox.Show("Введіть суму більшу або рівну суму до внесення");
            }
            }
            else
            {
                MessageBox.Show("У вас немає несплачених рахунків за інтернет");
            }
        }

        private async Task DepositMoneyAnimationAsync()
        {
            if (int.Parse(transferAmount_Tbx.Text)>= int.Parse(paidAmount_Label.Text.Substring(0, paidAmount_Label.Text.Length - 1))) 
            {
                int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися підібрати більші суми
                int targetAmount = int.Parse(transferAmount_Tbx.Text);
                int currentAmount = 0;
                Random rnd = new Random();

                while (currentAmount < targetAmount)
                {
                    bool found = false;
                    foreach (int amount in amounts)
                    {
                        if (currentAmount + amount <= targetAmount)
                        {
                            currentAmount += amount;
                            FundsСontributed_Lab.Text = currentAmount.ToString() + "₴";
                            FundsСontributed_Lab.Refresh();
                            await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        break; // якщо не можна знайти жодної суми, яка б підійшла, вийти з циклу
                    }
                }



            
            

            buttons[0].Enabled = false;
            }
        }
        public  void GetInternetDataByAddressAndAccountNumber()
        {
            GlobalInternetData.ClearInternet(); // Очищаємо список, щоб заповнити його новими даними

            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM Internet WHERE Address = @Address AND Account_number = @AccountNumber AND Paid = 0";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Address", address_Tbx.Text);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber_Tbx.Text);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["Id"];
                        string accNumber = reader["Account_number"].ToString();
                        int paid = (int)reader["Paid"];
                        int transferAmount = (int)reader["Transfer_Amount"];

                        // Створюємо об'єкт інтернет-послуги та додаємо його до списку
                        Internet internetData = new Internet(id, accNumber, paid, transferAmount);
                        GlobalInternetData.internet.Add(internetData);
                    }
                }
            }
        }
        public  void UpdateInternetData( string paymentDate, int paid)
        {
            using (SqlConnection connection = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                connection.Open();

                string query = @"UPDATE Internet SET Payment_Date = @PaymentDate, Paid = @Paid WHERE Account_number = @AccountNumber AND Id = @Id AND Address = @Address";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PaymentDate", paymentDate);
                command.Parameters.AddWithValue("@Paid", paid);
                command.Parameters.AddWithValue("@AccountNumber", accountNumber_Tbx.Text);
                command.Parameters.AddWithValue("@Id", internet.Id);
                command.Parameters.AddWithValue("@Address", address_Tbx.Text);

                command.ExecuteNonQuery();
            }
        }

    }
    //Ready
    public partial class TransferToTheCardForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private TextBox UserCard_Tbx, AmountTransfer_Tbx, Designation_Tbx;
        private Label FundsСontributed_Lab, paidAmount_Label;
        
        private DatabaseManager databaseManager = new DatabaseManager();
        private User user = GlobalData.Users[0]; 
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
            string[] LabelText = { "0$", "0$" };

            Point[] Labellocation = { new Point(260, 414), new Point(510, 414) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
            FundsСontributed_Lab = labels[0];
            paidAmount_Label = labels[1];
            FundsСontributed_Lab.Font = paidAmount_Label.Font = new Font("Segue UI", 32);
            FundsСontributed_Lab.ForeColor = paidAmount_Label.ForeColor = Color.White;
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
            UserCard_Tbx = textBoxes[0];
            AmountTransfer_Tbx = textBoxes[1];
            Designation_Tbx = textBoxes[2];
            UserCard_Tbx.TextChanged += UserCard_Tbx_TextChanged;
            UserCard_Tbx.Leave += UserCard_Tbx_Leave;
            AmountTransfer_Tbx.Leave += AmountTransfer_Tbx_Leave;

        }
        private void UserCard_Tbx_Leave(object sender, EventArgs e)
        {
            // Отримання тексту з текстових полів

            MessageBox.Show(UserCard_Tbx.Text);
            DatabaseManager.LoadCardRegistriesFromDatabase(UserCard_Tbx.Text);

            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if (GlobalCardlData.CardRegistries.Count == 0)
            {
                MessageBox.Show("Такої картки не існує");
            }
           


        }

        private void AmountTransfer_Tbx_Leave(object sender, EventArgs e)
        {
            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if (GlobalCardlData.CardRegistries.Count > 0)
            {
                paidAmount_Label.Text = AmountTransfer_Tbx.Text + "₴";
            }
        }
        private async void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ви хочете переказати кошти з картки чи готівкою?", "Переказ коштів", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Отримання об'єкта картки отримувача
            Card_Registry receiverCard = GlobalCardlData.CardRegistries[0];
            if (result == DialogResult.Yes)
            {
                // Логіка для переказу коштів з картки на картку

                // Перевірка наявності коштів на рахунку
                if (GlobalCardlData.CardRegistries.Count > 0)
                {
                 

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        await DepositMoneyAnimationAsync();
                        // Виконання переказу коштів

                        // Оновлення балансу користувача
                        UpdateUserBalance(user.CardNumber, transferAmount);

                        // Очищення списку користувачів та списку карток
                        GlobalData.ClearUsers();
                        // Перезавантаження даних користувача та картки отримувача
                        Log_In.GetUserByCardNumberAndPassword();
                        user = GlobalData.Users[0];
                        // Оновлення даних картки користувача в базі даних
                        databaseManager.UpdateCardData(user.CardNumber, user.CardType, user.IssueDate, user.ExpirationDate,
                            user.FullName, user.Address, user.CVV_CVC, user.Password, int.Parse(user.Balance), user.CardStatus,
                            user.PaymentSystem, user.SpendingLimit, user.IssuingBank);

                        

                        // Оновлення балансу картки отримувача
                        receiverCard.AccountBalance += transferAmount;
                        // Оновлення даних картки отримувача в базі даних
                        databaseManager.UpdateCardData(receiverCard.CardNumber, receiverCard.CardType, receiverCard.IssueDate, receiverCard.ExpirationDate,
                            receiverCard.CardholderName, receiverCard.CardholderAddress, receiverCard.CVV_CVC, receiverCard.PIN, receiverCard.AccountBalance, receiverCard.CardStatus,
                            receiverCard.PaymentSystem, receiverCard.SpendingLimit, receiverCard.IssuingBank);

                       
                        GlobalCardlData.ClearCardRegistries();
                      
                        
                        DatabaseManager.LoadCardRegistriesFromDatabase(UserCard_Tbx.Text);
                         receiverCard = GlobalCardlData.CardRegistries[0];

                        MessageBox.Show("Ви успішно здійснили переказ коштів на картку", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Сума переказу перевищує баланс рахунку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            else if (result == DialogResult.No)
            {
                // Перевірка наявності коштів на рахунку
                if (GlobalCardlData.CardRegistries.Count > 0)
                {

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        // Оновлення балансу користувача
                        await DepositMoneyAnimationAsync();

                        // Оновлення балансу картки отримувача
                        receiverCard.AccountBalance += transferAmount;
                        // Оновлення даних картки отримувача в базі даних
                        databaseManager.UpdateCardData(receiverCard.CardNumber, receiverCard.CardType, receiverCard.IssueDate, receiverCard.ExpirationDate,
                            receiverCard.CardholderName, receiverCard.CardholderAddress, receiverCard.CVV_CVC, receiverCard.PIN, receiverCard.AccountBalance, receiverCard.CardStatus,
                            receiverCard.PaymentSystem, receiverCard.SpendingLimit, receiverCard.IssuingBank);


                        GlobalCardlData.ClearCardRegistries();


                        DatabaseManager.LoadCardRegistriesFromDatabase(UserCard_Tbx.Text);
                        receiverCard = GlobalCardlData.CardRegistries[0];
                    }
                }
            }
        }


        public static void UpdateUserBalance(string cardNumber, int amount)
        {
            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();

                // Складаємо SQL-запит для оновлення балансу користувача
                string query = "UPDATE ATM_info SET balance = balance - @Amount WHERE Card_number = @CardNumber";

                SqlCommand cmd = new SqlCommand(query, conn);

                // Передача значень параметрів в запит
                cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                cmd.Parameters.AddWithValue("@Amount", amount);

                // Виконуємо запит для оновлення балансу
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Баланс користувача успішно оновлено.");
                }
                else
                {
                    MessageBox.Show("Не вдалося оновити баланс користувача.");
                }
            }
        }

        private void UserCard_Tbx_TextChanged(object sender, EventArgs e)
        {
            // Отримуємо текст з текстового поля
            string text = UserCard_Tbx.Text;

            // Видаляємо всі пробіли з тексту, щоб зручніше було працювати з номером картки
            text = text.Replace(" ", "");

            // Форматуємо текст у вигляді "1111 1111 1111 1111"
            if (text.Length > 0)
            {
                StringBuilder formattedText = new StringBuilder(text);

                // Додаємо пробіли після кожних чотирьох цифр
                for (int i = 4; i < formattedText.Length; i += 5)
                {
                    formattedText.Insert(i, ' ');
                }

                // Встановлюємо відформатований текст у текстове поле
                UserCard_Tbx.Text = formattedText.ToString();

                // Переміщаємо курсор на кінець тексту
                UserCard_Tbx.SelectionStart = UserCard_Tbx.Text.Length;
            }
            // Перевіряємо, чи введено більше 19 символів
            if (UserCard_Tbx.Text.Length > 19)
            {
                // Якщо досягнуто максимальної довжини, видаляємо останній введений символ
                UserCard_Tbx.Text = UserCard_Tbx.Text.Remove(19);
                // Переміщаємо курсор на кінець тексту
                UserCard_Tbx.SelectionStart = UserCard_Tbx.Text.Length;
            }
        }

        private async Task DepositMoneyAnimationAsync()
        {
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися підібрати більші суми
            int targetAmount = int.Parse(AmountTransfer_Tbx.Text);
            int currentAmount = 0;
            Random rnd = new Random();

            while (currentAmount < targetAmount)
            {
                bool found = false;
                foreach (int amount in amounts)
                {
                    if (currentAmount + amount <= targetAmount)
                    {
                        currentAmount += amount;
                        FundsСontributed_Lab.Text = currentAmount.ToString() + "₴";
                        FundsСontributed_Lab.Refresh();
                        await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break; // якщо не можна знайти жодної суми, яка б підійшла, вийти з циклу
                }
            }

            buttons[0].Enabled = false;
        }

    }
    //Ready
    public partial class TransferByRequisitesForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private Label FundsСontributed_Lab, paidAmount_Label;
        private TextBox IBAN_Tbx, AmountTransfer_Tbx, Designation_Tbx;
        private DatabaseManager databaseManager = new DatabaseManager();
        
        private User user = GlobalData.Users[0];
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
            string[] LabelText = { "0$", "0$" };

            Point[] Labellocation = { new Point(260, 414), new Point(510, 414) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
            FundsСontributed_Lab = labels[0];
            paidAmount_Label = labels[1];
            FundsСontributed_Lab.Font = paidAmount_Label.Font = new Font("Segue UI", 32);
            FundsСontributed_Lab.ForeColor = paidAmount_Label.ForeColor = Color.White;
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
            IBAN_Tbx = textBoxes[0];
            AmountTransfer_Tbx = textBoxes[1];
            Designation_Tbx = textBoxes[2];
            IBAN_Tbx.Leave += Iban_Tbx_Leave;
            AmountTransfer_Tbx.Leave += AmountTransfer_Tbx_Leave;
        }
        private async void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ви хочете переказати кошти з картки чи готівкою?", "Переказ коштів", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Отримання об'єкта картки отримувача
            
            CompanyDetails company = GlobalCompanyDetails.companies[0];
            if (result == DialogResult.Yes)
            {
                // Логіка для переказу коштів з картки на картку
                MessageBox.Show("Nothing Work");
                // Перевірка наявності коштів на рахунку
                if (GlobalCompanyDetails.companies.Count > 0)
                {
                    MessageBox.Show("ALL WORK");

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        await DepositMoneyAnimationAsync();
                        // Виконання переказу коштів

                        // Оновлення балансу користувача
                        TransferToTheCardForm.UpdateUserBalance(user.CardNumber, transferAmount);

                        // Очищення списку користувачів та списку карток
                        GlobalData.ClearUsers();
                        // Перезавантаження даних користувача та картки отримувача
                        Log_In.GetUserByCardNumberAndPassword();
                        user = GlobalData.Users[0];

                        // Оновлення балансу картки отримувача
                        company.AccountBalance += transferAmount;

                        databaseManager.UpdateCompanyData(company.IBAN, company.Company_Name, company.Country,
                            company.Address,company.ContactPerson, company.Phone, 
                            company.TIN, company.EDRPOU, company.AccountBalance);

                       GlobalCompanyDetails.ClearCompanyDetails();


                        
                        DatabaseManager.LoadCompanyDetailsFromDatabase(IBAN_Tbx.Text);
                        company = GlobalCompanyDetails.companies[0];

                        MessageBox.Show("Ви успішно здійснили переказ коштів на картку", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Сума переказу перевищує баланс рахунку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else if (result == DialogResult.No)
            {
                // Перевірка наявності коштів на рахунку
                if (GlobalCompanyDetails.companies.Count > 0)
                {

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        // Оновлення балансу користувача
                        await DepositMoneyAnimationAsync();

                        // Оновлення балансу картки отримувача
                        company.AccountBalance += transferAmount;
                        // Оновлення даних картки отримувача в базі даних
                        databaseManager.UpdateCompanyData(company.IBAN, company.Company_Name, company.Country,
                            company.Address, company.ContactPerson, company.Phone,
                            company.TIN, company.EDRPOU, company.AccountBalance);


                        GlobalCardlData.ClearCardRegistries();


                        DatabaseManager.LoadCompanyDetailsFromDatabase(IBAN_Tbx.Text);
                        company = GlobalCompanyDetails.companies[0];
                    }
                }
            }
        }

        private async Task DepositMoneyAnimationAsync()
        {
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися підібрати більші суми
            int targetAmount = int.Parse(AmountTransfer_Tbx.Text);
            int currentAmount = 0;
            Random rnd = new Random();

            while (currentAmount < targetAmount)
            {
                bool found = false;
                foreach (int amount in amounts)
                {
                    if (currentAmount + amount <= targetAmount)
                    {
                        currentAmount += amount;
                        FundsСontributed_Lab.Text = currentAmount.ToString() + "₴";
                        FundsСontributed_Lab.Refresh();
                        await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break; // якщо не можна знайти жодної суми, яка б підійшла, вийти з циклу
                }
            }

            buttons[0].Enabled = false;
        }
           
        private void AmountTransfer_Tbx_Leave(object sender, EventArgs e)
        {
           
            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if (GlobalCompanyDetails.companies.Count > 0)
            {
                paidAmount_Label.Text = AmountTransfer_Tbx.Text + "₴";
            }
        }

        private void Iban_Tbx_Leave(object sender, EventArgs e)
        {
            DatabaseManager.LoadCompanyDetailsFromDatabase(IBAN_Tbx.Text);
        
            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if (GlobalCompanyDetails.companies.Count == 0)
            {
                MessageBox.Show("Такої картки не існує");
            }
        }
    }
    public partial class CharityForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private Label FundsСontributed_Lab, paidAmount_Label;
        private TextBox FondName_Tbx, AmountTransfer_Tbx, Designation_Tbx;
        private DatabaseManager databaseManager = new DatabaseManager();
        private User user = GlobalData.Users[0];
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
            string[] LabelText = { "0$", "0$" };

            Point[] Labellocation = { new Point(260, 414), new Point(510, 414) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
            FundsСontributed_Lab = labels[0];
            paidAmount_Label = labels[1];
            FundsСontributed_Lab.Font = paidAmount_Label.Font = new Font("Segue UI", 32);
            FundsСontributed_Lab.ForeColor = paidAmount_Label.ForeColor = Color.White;
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
            FondName_Tbx = textBoxes[0];
            AmountTransfer_Tbx = textBoxes[1];
            Designation_Tbx = textBoxes[2];
            FondName_Tbx.Leave += FondName_Tbx_Leave;
            AmountTransfer_Tbx.Leave += AmountTransfer_Tbx_Leave;

        }
        private async void ExitButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Ви хочете переказати кошти з картки чи готівкою?", "Переказ коштів", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // Отримання об'єкта картки отримувача
            CharityFond charityFond = GlobalCharityFond.CharityFonds[0];
           
            if (result == DialogResult.Yes)
            {
              
                // Перевірка наявності коштів на рахунку
                if (GlobalCharityFond.CharityFonds.Count > 0)
                {
                  

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        await DepositMoneyAnimationAsync();
                        // Виконання переказу коштів

                        // Оновлення балансу користувача
                        TransferToTheCardForm.UpdateUserBalance(user.CardNumber, transferAmount);

                        // Очищення списку користувачів та списку карток
                        GlobalData.ClearUsers();
                        // Перезавантаження даних користувача та картки отримувача
                        Log_In.GetUserByCardNumberAndPassword();
                        user = GlobalData.Users[0];

                        // Оновлення балансу картки отримувача
                        charityFond.AccountBalance += transferAmount;

                        databaseManager.UpdateCharityFondData(charityFond.FondName,charityFond.RegistrationNumber,
                            charityFond.Country,charityFond.Address,charityFond.ContactPerson,charityFond.Phone,charityFond.Email,
                            charityFond.BankAccount,charityFond.AccountBalance);

                        GlobalCharityFond.ClearCharityFonds();


                       
                        DatabaseManager.LoadCharityFondFromDatabase(FondName_Tbx.Text);
                        charityFond = GlobalCharityFond.CharityFonds[0];

                        MessageBox.Show("Ви успішно здійснили переказ коштів на картку", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Сума переказу перевищує баланс рахунку", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else if (result == DialogResult.No)
            {
                // Перевірка наявності коштів на рахунку
                if (GlobalCharityFond.CharityFonds.Count > 0)
                {

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        // Оновлення балансу користувача
                        await DepositMoneyAnimationAsync();

                        charityFond.AccountBalance += transferAmount;

                        databaseManager.UpdateCharityFondData(charityFond.FondName, charityFond.RegistrationNumber,
                            charityFond.Country, charityFond.Address, charityFond.ContactPerson, charityFond.Phone, charityFond.Email,
                            charityFond.BankAccount, charityFond.AccountBalance);


                        GlobalCharityFond.ClearCharityFonds();


                        DatabaseManager.LoadCharityFondFromDatabase(FondName_Tbx.Text);
                        charityFond = GlobalCharityFond.CharityFonds[0];
                    }
                }
            }
        }

        private async Task DepositMoneyAnimationAsync()
        {
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися підібрати більші суми
            int targetAmount = int.Parse(AmountTransfer_Tbx.Text);
            int currentAmount = 0;
            Random rnd = new Random();

            while (currentAmount < targetAmount)
            {
                bool found = false;
                foreach (int amount in amounts)
                {
                    if (currentAmount + amount <= targetAmount)
                    {
                        currentAmount += amount;
                        FundsСontributed_Lab.Text = currentAmount.ToString() + "₴";
                        FundsСontributed_Lab.Refresh();
                        await Task.Delay(300); // Затримка для анімації без блокування основного потоку
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    break; // якщо не можна знайти жодної суми, яка б підійшла, вийти з циклу
                }
            }

            buttons[0].Enabled = false;
        }

        private void AmountTransfer_Tbx_Leave(object sender, EventArgs e)
        {

            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if (GlobalCharityFond.CharityFonds.Count > 0)
            {
                paidAmount_Label.Text = AmountTransfer_Tbx.Text + "₴";
            }
        }

        private void FondName_Tbx_Leave(object sender, EventArgs e)
        {
            
            DatabaseManager.LoadCharityFondFromDatabase(FondName_Tbx.Text);

            // Оновлення тексту мітки в залежності від вмісту текстових полів
            if (GlobalCharityFond.CharityFonds.Count == 0)
            {
                MessageBox.Show("Такої картки не існує");
            }
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
