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
using System.Security.Cryptography.Xml;
using Google.Apis.Drive.v3;
using Google.Apis.Sheets.v4.Data;
using Color = System.Drawing.Color;
using System.Security.Cryptography;
using ATM_CryptoGuardian;


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
        ReportManager reportManager = new ReportManager();
     //const string connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\anton\\source\\repos\\ATM Winforms\\Database1.accdb\";Persist Security Info=True";
     //SqlConnection sqlConnection = new SqlConnection(connString);
     private Button[] buttons;
  

        File_Creator fileCreator = new File_Creator();

     public InsertCardForm() : base("InsertCard", Resource_Paths.InsertCardForm)
        {
            //EncryptData();
        
            InitializeComponent();
         //  TimedDailyReport();
           // TimedMonthlyReport();
            

     }

     private void TimedDailyReport()
        {
            // Ініціалізація таймера
            System.Timers.Timer timer = new System.Timers.Timer();

            // Встановлення інтервалу таймера (24 години)
            timer.Interval = TimeSpan.FromHours(24).TotalMilliseconds;

            // Встановлення часу спрацювання події (о 12:00)
            DateTime now = DateTime.Now;
            DateTime nextNoon = new DateTime(now.Year, now.Month, now.Day, 0,0, 0);
            if (now > nextNoon)
                nextNoon = nextNoon.AddDays(1);
            double intervalToNoon = (nextNoon - now).TotalMilliseconds;
            timer.Interval = intervalToNoon;

            // Додавання обробника події
            timer.Elapsed += (sender, e) => {
                // Виконання необхідного коду
                GlobalData.GetAllUser();
                GlobalFines.GetAllFines();
                GlobalUtility_Bills.GetAllUserBills();
                GlobalInternetData.GetAllInternetData();
                GlobalTransactionData.GetAllTransactions();
                GlobalCompanyDetails.GetAllCompanyDetails();
                GlobalCharityFond.GetAllCharityFond();
                reportManager.GenerateAndUploadReports();
                ClearAllData();
            };

            // Запуск таймера
            timer.Start();

            Console.WriteLine("Додаток запущено. Натисніть Enter для завершення.");
            Console.ReadLine();
        }
        private void TimedMonthlyReport()
        {
            // Ініціалізація таймера
            System.Timers.Timer timer = new System.Timers.Timer();

            // Метод для обчислення інтервалу до наступного першого числа місяця о 00:00
            double CalculateIntervalToNextMonth()
            {
                DateTime now = DateTime.Now;
                DateTime nextFirstOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0).AddMonths(1);
                return (nextFirstOfMonth - now).TotalMilliseconds;
            }

            // Встановлення початкового інтервалу таймера
            timer.Interval = CalculateIntervalToNextMonth();

            // Додавання обробника події
            timer.Elapsed += (sender, e) =>
            {
                // Виконання необхідного коду
                GlobalData.GetAllUser();
                GlobalFines.GetAllFines();
                GlobalUtility_Bills.GetAllUserBills();
                GlobalInternetData.GetAllInternetData();
                GlobalTransactionData.GetAllTransactions();
                GlobalCompanyDetails.GetAllCompanyDetails();
                GlobalCharityFond.GetAllCharityFond();
                reportManager.GenerateAndUploadReports();
                ClearAllData();
                // Повторне встановлення інтервалу таймера до наступного першого числа місяця
                timer.Interval = CalculateIntervalToNextMonth();
            };

            // Запуск таймера
            timer.Start();

            Console.WriteLine("Додаток запущено. Натисніть Enter для завершення.");
            Console.ReadLine();
        }
        
        


        private void ClearAllData()
        {
            GlobalData.ClearUsers();
            GlobalFines.ClearFines();
            GlobalUtility_Bills.ClearUtility_Bills();
            GlobalInternetData.ClearInternet();
            GlobalTransactionData.ClearTransactions();
            GlobalCompanyDetails.ClearCompanyDetails();
            GlobalCharityFond.ClearCharityFonds();
        }

        private void InitializeComponent()
     {
         this.ClientSize = new Size(828, 599);
         Name = "InsertCardForm";
         StartPosition = FormStartPosition.CenterScreen;
       
         this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
       
         AddButtons();
            Update_DB update_DB = new Update_DB();
            update_DB.UpdateRates();
          string connString = Resource_Paths.DB_connectionString;

          DatabaseManager.ConnectToDatabase(this);

          




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
                connection.Open();
              
                string query = "SELECT PIN FROM ATM_info WHERE Card_number = @NumberCard";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumberCard", NumberCard_Tbx.Text);
                    string storedHash = (string)command.ExecuteScalar();

                    if (storedHash != null && Hashing_Service.BCryptVerifyPassword(Pin_Tbx.Text, storedHash))
                    {
                        GetUserByCardNumberAndPassword();
                        Main_Menu main_Menu = new Main_Menu();
                        main_Menu.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed
                        main_Menu.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неправильний номер картки або Пін!");
                    }
                }
            }
        }

        public static void GetUserByCardNumberAndPassword()
        {
           
            RSAParameters alicePrivateKey = RSAKeyManager.LoadPrivateKey();

            using (SqlConnection conn = new SqlConnection(Resource_Paths.DB_connectionString))
            {
                conn.Open();
              
                string query = "SELECT * FROM ATM_info WHERE Card_number = @CardNumber";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CardNumber", NumberCard_Tbx.Text);
                    SqlDataReader reader = cmd.ExecuteReader();

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

     


    }
    //Ready
    public partial class Main_Menu : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        private Label FullNameLabel, CardNumberLabel, PaymentSystemLabel, ExpirationDateLabel;
        static Update_DB update_DB = new Update_DB();
        ExchangeRates exchangeRate = new ExchangeRates();

        public Main_Menu() : base("Main Menu", Resource_Paths.MainForm)
        {
            GetExchangeRates(Resource_Paths.DB_connectionString, "EUR", "USD");
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
               Cash_withdrawls_Click, PaymentHistory_Click, ReplenishTheCard_Click,
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

                FullNameLabel.Font = ExpirationDateLabel.Font = new Font("SEGUE UI", 12);
                CardNumberLabel.Font = new Font("SEGUE UI", 18, FontStyle.Regular);
                PaymentSystemLabel.Font = new Font("SEGUE UI", 12, FontStyle.Italic);
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


        private void PaymentHistory_Click(object sender, EventArgs e)
        {
           
          CloseForms(new PaymentHistoryForm());
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
            ReplenishTheCardForm replenishTheCardForm = new ReplenishTheCardForm();
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

        private void CloseForms(Form form)
        {
            
            form.FormClosed += (s, args) => this.Close(); // Додати обробник події FormClosed

            form.Show();
            this.Hide();
        }

    }   
    //Ready
    public partial class CashWithdrawalForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Button WithdrawMoney_Btn;
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

            this.FormClosed += Close_Form;

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
            WithdrawMoney_Btn = buttons[0];

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
        private async void ExitButton_Click(object sender, EventArgs e)
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

               await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, "Зняття готівки",
                    user_WithdrawAmount, "UAH", DateTime.Now, "Успішно",user.CardNumber, null, "");
                //тут треба анімація видання коштів 
                MessageBox.Show("Ви успішно зняли готівку");

                WithdrawMoney_Btn.Enabled = false; 
            }
            else
            {
                await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, "Зняття готівки",
                    user_WithdrawAmount, "UAH", DateTime.Now, "Помилка", user.CardNumber, null, "");
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
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Не вдалося оновити баланс користувача.");
                    conn.Close();
                }
            }
        }

        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
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
            this.FormClosed += Close_Form;
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
            AmountToPay_Tbx.BackColor = System.Drawing.Color.FromArgb(191, 191, 191);
            AmountToPay_Tbx.Leave += AmountToPay_Tbx_Leave;

        }
        private async void ExitButton_Click(object sender, EventArgs e)
        {
            AmountToPay_Tbx.Enabled = false;
            await DepositMoneyAnimationAsync();
            Replenish_Btn.Enabled = false;
        }

        private async Task DepositMoneyAnimationAsync()
        {
            int[] amounts = { 500, 200, 100, 50, 20, 10 }; // змінено порядок сум для того, щоб спочатку намагатися підібрати більші суми
            int targetAmount = int.Parse(AmountToPay_Tbx.Text);
            int currentAmount = 0;
            Random rnd = new Random();
            try
            {
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

           
            UpdateUserBalance(user.CardNumber, currentAmount);
                ReloadUserInfo();
            databaseManager.UpdateCardData(user.CardNumber, user.CardType, user.IssueDate, user.ExpirationDate,
                    user.FullName, user.Address, user.CVV_CVC, user.Password, int.Parse(user.Balance), user.CardStatus,
                    user.PaymentSystem, user.SpendingLimit, user.IssuingBank);
            await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, "Внесення готівки",
                    currentAmount, "UAH", DateTime.Now, "Успішно","готівка" , user.CardNumber, "");
           
              
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
        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
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
        private Button payFine_Btn;
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
            this.FormClosed += Close_Form;
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
            payFine_Btn = buttons[0]; 

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
            ReloadFineInfo();
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

                // Позначимо всі оплачені чекбокси як неактивні
                foreach (Control control in panel.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                        checkBox.Checked = false;
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Оплата штрафу: {checkBox.Text}",
    totalAmount, "UAH", DateTime.Now, "Успішно", user.CardNumber, null, "");
                    }
                }
                ReloadFineInfo();
                panel.Controls.Clear();
                AddFlowLayoutPanel();
                panel.Refresh();
       

               

                payFine_Btn.Enabled = false;
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
        private void ReloadFineInfo()
        {
            GlobalFines.ClearFines();
            FinesForm1.ReadFinesData();
            if (GlobalFines.Fines.Count > 0)
            {
                Fine fine = GlobalFines.Fines[0];
            }

        }
        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
            ReloadFineInfo();
        }

    }
    //Ready
    public partial class UtilityBillsForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Button payUtilityBills_Btn;
        private Label[] labels;
        private TextBox[] textBoxes;
        int totalAmount = 0; // Змінна для збереження суми до сплати   
        private Dictionary<int, string> BillsPaymentStatus = new Dictionary<int, string>();
        private Label AmountDue_Lab;
        private Panel panel = new Panel();
       private static User user = GlobalData.Users[0];
        private Utility_Bills utility_Bills;
        public UtilityBillsForm() : base("Комунальні платежі", Resource_Paths.UtilityBillsForm)
        {
            ReadUserBills();
            AddButtons();
            AddLabel();
            AddFlowLayoutPanel();
            this.FormClosing += Close_Form;
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
            payUtilityBills_Btn = buttons[0];

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
            ReloadUtilityBillsInfo();
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
                        
                        UpdateUserBillStatus(billID, "1");
                    }
                    else
                    {
                       
                        UpdateUserBillStatus(billID, "0");
                    }
                }


            ReloadUtilityBillsInfo();

                panel.Controls.Clear();
                AddFlowLayoutPanel();
                panel.Refresh();


                // Позначимо всі оплачені чекбокси як неактивні
                foreach (Control control in panel.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {
                    await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Оплата за послугу {checkBox.Text}",
  totalAmount, "UAH", DateTime.Now, "Успішно", "Готівка", utility_Bills.Company_Name, "");
                    checkBox.Checked = false;
                    
                    }
               
            }
            
                payUtilityBills_Btn.Enabled = false;
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

                string query = @"SELECT ATM_info.Full_name, ATM_info.address, Utility_Сompanies.Id, Utility_Сompanies.Compani_name, Utility_Сompanies.Amount,Utility_Сompanies.tariff,Utility_Сompanies.used,Utility_Сompanies.paid
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
                        string tariff = reader["tariff"].ToString();
                        string used = reader["used"].ToString();
                        string is_paid = reader["paid"].ToString();

                        // Створення об'єкту з отриманих даних і додавання його до списку
                        Utility_Bills userData = new Utility_Bills(id, userFullName,companyName, amountToPay, address, tariff, used, is_paid);
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

        private void ReloadUtilityBillsInfo()
        {
            GlobalUtility_Bills.ClearUtility_Bills();
            ReadUserBills();
            if (GlobalUtility_Bills.utility_Bills.Count > 0)
            {
                utility_Bills = GlobalUtility_Bills.utility_Bills[0];
            }
        }
        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
            ReloadUtilityBillsInfo();

        }
    }
    //Ready
    public partial class InternetForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Button payInternet_Btn;
        private Label[] labels;
        private TextBox[] textBoxes;
        private static TextBox accountNumber_Tbx, address_Tbx, transferAmount_Tbx;
        private Label FundsСontributed_Lab , paidAmount_Label;
        private static Internet internet = GlobalInternetData.internet[0];
        private User user = GlobalData.Users[0];
        private DatabaseManager databaseManager = new DatabaseManager();

        public InternetForm() : base("Інтернет", Resource_Paths.InternetForm)
        {
            AddButtons();
            AddLabel();
            AddTextBox();
            this.FormClosed += Close_Form;
           

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
            payInternet_Btn = buttons[0];

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
                   await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Оплата за інтернет",
   requiredAmount, "UAH", DateTime.Now, "Успішно",user.CardNumber, accountNumber_Tbx.Text, "");
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
                     await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Оплата за інтернет",
      requiredAmount, "UAH", DateTime.Now, "Успішно", "Готівка", accountNumber_Tbx.Text, "");
                        MessageBox.Show($"Ви успішно сплатили {transferAmount}$ за послуги з інтернету. Залишок {remainingAmount}$ був перенесений на вашу картку.");
                }
                    payInternet_Btn.Enabled = false;
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
        public static void GetInternetDataByAddressAndAccountNumber()
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
                        string address = reader["Address"].ToString();
                        int paid = (int)reader["Paid"];
                        int transferAmount = (int)reader["Transfer_Amount"];
                        string tariffPlan = reader["Tariff_Plan"].ToString();
                        string paymentDate = reader["Payment_Date"].ToString();
                        string serviceStatus = reader["Service_Status"].ToString();
                        string dataUsage = reader["Data_Usage"].ToString();
                        string userName = reader["User_Name"].ToString();

                        // Створюємо об'єкт інтернет-послуги та додаємо його до списку
                        Internet internetData = new Internet(id, accNumber, address, paid, transferAmount, tariffPlan, paymentDate, serviceStatus, dataUsage, userName);
                        GlobalInternetData.internet.Add(internetData);
                    }
                }
            }
        }

        public void UpdateInternetData( string paymentDate, int paid)
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

        private void ReloadInternetInfo()
        {

            GetInternetDataByAddressAndAccountNumber();
            if(GlobalInternetData.internet.Count>0)
            internet = GlobalInternetData.internet[0];
        }
        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
            ReloadInternetInfo();
        }
    }
    //Ready
    public partial class TransferToTheCardForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Button TransferToCard_Btn;
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
            TransferToCard_Btn = buttons[0];

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

                        ReloadUserInfo();
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
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ на картку",
 transferAmount, "UAH", DateTime.Now, "Успішно", user.CardNumber, UserCard_Tbx.Text, Designation_Tbx.Text);
                        MessageBox.Show("Ви успішно здійснили переказ коштів на картку", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TransferToCard_Btn.Enabled = false;
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

                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ на картку",
 transferAmount, "UAH", DateTime.Now, "Успішно", "Готівка", UserCard_Tbx.Text, Designation_Tbx.Text);
                        TransferToCard_Btn.Enabled = false;
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

        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
        }
    }
    //Ready
    public partial class TransferByRequisitesForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Button IbanTransfer_Btn;
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
            this.FormClosing += Close_Form;

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
            IbanTransfer_Btn = buttons[0];

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
               
                // Перевірка наявності коштів на рахунку
                if (GlobalCompanyDetails.companies.Count > 0)
                {
                    

                    // Перевірка достатньої суми для переказу
                    int transferAmount = int.Parse(AmountTransfer_Tbx.Text);
                    if (transferAmount <= int.Parse(user.Balance))
                    {
                        await DepositMoneyAnimationAsync();
                        // Виконання переказу коштів

                        // Оновлення балансу користувача
                        TransferToTheCardForm.UpdateUserBalance(user.CardNumber, transferAmount);

                        ReloadUserInfo();

                        // Оновлення балансу картки отримувача
                        company.AccountBalance += transferAmount;

                        databaseManager.UpdateCompanyData(company.IBAN, company.Company_Name, company.Country,
                            company.Address, company.ContactPerson, company.Phone,
                            company.TIN, company.EDRPOU, company.AccountBalance);

                        GlobalCompanyDetails.ClearCompanyDetails();



                        DatabaseManager.LoadCompanyDetailsFromDatabase(IBAN_Tbx.Text);
                        company = GlobalCompanyDetails.companies[0];

                        MessageBox.Show("Ви успішно здійснили переказ коштів на картку", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ за реквізитами",
 transferAmount, "UAH", DateTime.Now, "Успішно", user.CardNumber, IBAN_Tbx.Text, Designation_Tbx.Text);
                        IbanTransfer_Btn.Enabled = false;
                    }
                    else
                    {
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ за реквізитами",
transferAmount, "UAH", DateTime.Now, "Неспішно", user.CardNumber, IBAN_Tbx.Text, Designation_Tbx.Text);
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
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ за реквізитами",
transferAmount, "UAH", DateTime.Now, "Успішно", "Готівка", IBAN_Tbx.Text, Designation_Tbx.Text);
                        IbanTransfer_Btn.Enabled = false;
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

        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
        }
    }
    public partial class CharityForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Button FundTransfer_Btn;
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
            this.FormClosing += Close_Form;
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
            FundTransfer_Btn = buttons[0];

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

                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ в фонд {charityFond.FondName}",
transferAmount, "UAH", DateTime.Now, "Успішно", user.CardNumber, FondName_Tbx.Text, Designation_Tbx.Text);
                        MessageBox.Show("Ви успішно здійснили переказ коштів на картку", "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        FundTransfer_Btn.Enabled = false;
                    }
                    else
                    {
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ в фонд {charityFond.FondName}",
transferAmount, "UAH", DateTime.Now, "Помилка", user.CardNumber, FondName_Tbx.Text, Designation_Tbx.Text);
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
                        await DatabaseManager.InsertTransaction(Resource_Paths.DB_connectionString, user.ID, $"Переказ в фонд {charityFond.FondName}",
transferAmount, "UAH", DateTime.Now, "Успішно", "Готівка", FondName_Tbx.Text, Designation_Tbx.Text);
                        FundTransfer_Btn.Enabled = false;
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

        private void ReloadUserInfo()
        {
            GlobalData.ClearUsers();
            Log_In.GetUserByCardNumberAndPassword();
            user = GlobalData.Users[0];

        }
        private void Close_Form(object sender, EventArgs e)
        {
            ReloadUserInfo();
            

        }
    }

    public partial class PaymentHistoryForm : Base_Form
    {
        Create_Ui_Element create_ui_element = new Create_Ui_Element();
        private Button[] buttons;
        private Label[] labels;
        private TextBox[] textBoxes;
        User user = GlobalData.Users[0];
        public PaymentHistoryForm() : base("Історія платежів", Resource_Paths.PaymentHistoryForm)
        {
            AddLabel();
            //addListView();
            CreateMyDataGridView(user.ID);

        }
        

        private void AddLabel()
        {
            string[] LabelText = { "15:35", "вихід" };

            Point[] Labellocation = { new Point(24, 20), new Point(780, 20) };


            labels = create_ui_element.CreateLabel(2, this, LabelText, Labellocation);
        
        }

       



        public void CreateMyDataGridView(int userId)
        {
            // Створити новий контейнер для DataGridView з прокруткою.
            Panel panel = new Panel();
            panel.Bounds = new Rectangle(new Point(190, 435), new Size(465, 168));

            // Створити новий елемент керування DataGridView.
            DataGridView dataGridView1 = new DataGridView();
            dataGridView1.Dock = DockStyle.Fill; // Заповнити весь доступний простір в контейнері.
            dataGridView1.GridColor = Color.FromArgb(218, 218, 218);
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            // Встановити вигляд для відображення деталей.
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersVisible = false; // Приховати заголовки стовпців

            // Створити стовпці для елементів та піделементів.
            DataGridViewTextBoxColumn column1 = new DataGridViewTextBoxColumn();
            column1.DataPropertyName = "TransactionType";
            column1.Width = 293;
            column1.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Додано перенесення тексту
            column1.DefaultCellStyle.BackColor = Color.FromArgb(218, 218, 218); // Змінено колір фону
            column1.HeaderCell.Style.BackColor = Color.FromArgb(218, 218, 218); // Змінено колір фону заголовка

            DataGridViewTextBoxColumn column2 = new DataGridViewTextBoxColumn();
            column2.DataPropertyName = "Amount";
            column2.Width = 86;
            column2.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Додано перенесення тексту
            column2.DefaultCellStyle.BackColor = Color.FromArgb(218, 218, 218); // Змінено колір фону
            column2.HeaderCell.Style.BackColor = Color.FromArgb(218, 218, 218); // Змінено колір фону заголовка

            DataGridViewTextBoxColumn column3 = new DataGridViewTextBoxColumn();
            column3.DataPropertyName = "Timestamp";
            column3.Width = 83;
            column3.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // Додано перенесення тексту
            column3.DefaultCellStyle.BackColor = Color.FromArgb(218, 218, 218); // Змінено колір фону
            column3.HeaderCell.Style.BackColor = Color.FromArgb(218, 218, 218); // Змінено колір фону заголовка

            dataGridView1.EnableHeadersVisualStyles = false;

            // Додати стовпці до DataGridView.
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { column1, column2, column3 });

            // Отримати дані для користувача
            GlobalTransactionData.ClearTransactions();
            GlobalTransactionData.LoadTransactions(userId);

            // Заповнити DataGridView даними
            foreach (var transaction in GlobalTransactionData.Transactions)
            {
                dataGridView1.Rows.Add(transaction.TransactionType, transaction.Amount, transaction.Timestamp);
            }

            // Приховати бокову панель навігації.
            dataGridView1.RowHeadersVisible = false;

            // Заборонити сортування даних.
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.ReadOnly = true;
            }

            // Заборонити користувачеві редагувати дані в DataGridView.
            dataGridView1.ReadOnly = true;

            // Заборонити зміну розміру комірок.
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            column1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            column3.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Вирівняти дані у центрі для другого стовпчика.
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Вирівняти дані у центрі для третього стовпчика.
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Адаптувати висоту рядка під самий високий елемент.
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Встановити колір виділення для комірок, щоб зробити його непомітним
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(218, 218, 218);
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

            // Заборонити виділення рядків при натисканні
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            // Додати DataGridView до панелі.
            panel.Controls.Add(dataGridView1);

            // Додати панель з прокруткою до форми або іншого контейнера.
            this.Controls.Add(panel);
        }


    }


    public  class ReportManager
    {
        private File_Creator fileCreator;
        private Google_Sheets_Manager googleSheetsManager;
        private Google_Drive googleDrive;

        public ReportManager()
        {
            fileCreator = new File_Creator();
            googleSheetsManager = new Google_Sheets_Manager();
            googleDrive = new Google_Drive();
        }

        public async void GenerateAndUploadReports()
        {
            var driveService = googleDrive.GetDriveService();
            WriteUsersDataToGoogleSheet();
            WriteUtilityBillsToGoogleSheet();
            WriteTransactionsToGoogleSheet();
            WriteFinesToGoogleSheet();
            WriteCompaniesToGoogleSheet();
            WriteCharitiesToGoogleSheet();
            Google_Sheets_Manager.DownloadFile(driveService, Google_Sheets_Manager.SpreadsheetId, Resource_Paths.DataBase_XLSX);
            if (fileCreator.SSD_serialNumber != null && fileCreator.IsSSDSerialNumberValid(fileCreator.SSD_serialNumber))
            {
                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                try
                {
                    // Генерація щоденного звіту
                    fileCreator.DailyReport();
                    DateTime previousMonthDate = DateTime.Now.AddMonths(-1);
                    previousMonthDate.ToString("MMMM");

                    // Отримання об'єкту сервісу Google Drive


                    // Завантаження щоденного звіту на Google Диск
                    string dailyReportPath = Resource_Paths.DailyReport;
                    string folderId = "1THHQ1xNOR7dJ6MFPjL9qtXUYaTz3XLLc"; // Замість 'your-folder-id' вставте ідентифікатор папки на Google Диск
                                                                           // Винесення шляхів до окремих змінних
                    string usersDailyReportPath = Path.Combine(Resource_Paths.Users_DailyReport, $"Users database_{currentDate}.txt");
                    string utilityBillsDailyReportPath = Path.Combine(Resource_Paths.UTilityBils_DailyReport, $"Utility Bills_{currentDate}.txt");
                    string transactionDailyReportPath = Path.Combine(Resource_Paths.Transaction_DailyReport, $"Transactions_{currentDate}.txt");
                    string internetDailyReportPath = Path.Combine(Resource_Paths.Internet_DailyReport, $"Internet Info_{currentDate}.txt");
                    string finesDailyReportPath = Path.Combine(Resource_Paths.Fines_DailyReport, $"Fines_{currentDate}.txt");
                    string companyDetailsDailyReportPath = Path.Combine(Resource_Paths.CompanyDetails_DailyReport, $"Company Details_{currentDate}.txt");
                    string charityFondsDailyReportPath = Path.Combine(Resource_Paths.CharityFonds_DailyReport, $"Charity Fonds_{currentDate}.txt");

                    // Завантаження файлів на Google Диск
                    UploadFileToGoogleDrive(driveService, usersDailyReportPath, "1ktKRRfSc5ZKuegY3IeHqJPJALtYkvBqv");
                    UploadFileToGoogleDrive(driveService, utilityBillsDailyReportPath, "1OfjEMCE0aOnmAM_KfnbV179DbQOlvsRm");
                    UploadFileToGoogleDrive(driveService, transactionDailyReportPath, "1YRPsGD04Zu30J6RcEYJ3W5NQqI5H_0I1");
                    UploadFileToGoogleDrive(driveService, internetDailyReportPath, "1w69mvKMOcrS7epwNNhjKMt6topAedTLi");
                    UploadFileToGoogleDrive(driveService, finesDailyReportPath, "1T5ceGegBC6DRB9buSwCwSM_kbpM--z5F");
                    UploadFileToGoogleDrive(driveService, companyDetailsDailyReportPath, "1mSJ575wyQTB6KBxtKQANtBX3qAdhd1f-");
                    UploadFileToGoogleDrive(driveService, charityFondsDailyReportPath, "1zPDNLvmZRb9XPNBaOfvbWlhYNn4SYiZ0");

                    // Перевірка, чи потрібно генерувати місячний звіт
                    if (DateTime.Now.Day == 1) // Якщо перший день місяця
                    {
                        // Генерація місячного звіту
                        fileCreator.MonthlyReport();
                        string usersMontlyReportPath = Path.Combine(Resource_Paths.Users_MonthlyReport, $"Users database_{previousMonthDate}.txt");
                        string utilityBillsMontlyReportPath = Path.Combine(Resource_Paths.UTilityBils_MonthlyReport, $"Utility Bills_{previousMonthDate}.txt");
                        string transactionMontlyReportPath = Path.Combine(Resource_Paths.Transaction_MonthlyReport, $"Transactions_{previousMonthDate}.txt");
                        string internetMontlyReportPath = Path.Combine(Resource_Paths.Internet_MonthlyReport, $"Internet Info_{previousMonthDate}.txt");
                        string finesMontlyReportPath = Path.Combine(Resource_Paths.Fines_MonthlyReport, $"Fines_{previousMonthDate}.txt");
                        string companyDetailsMontlyReportPath = Path.Combine(Resource_Paths.CompanyDetails_MonthlyReport, $"Company Details_{previousMonthDate}.txt");
                        string charityFondsMontlyReportPath = Path.Combine(Resource_Paths.CharityFonds_MonthlyReport, $"Charity Fonds_{previousMonthDate}.txt");


                     
                        UploadFileToGoogleDrive(driveService, usersMontlyReportPath, folderId);
                        UploadFileToGoogleDrive(driveService, utilityBillsMontlyReportPath, folderId);
                        UploadFileToGoogleDrive(driveService, transactionMontlyReportPath, folderId);
                        UploadFileToGoogleDrive(driveService, internetMontlyReportPath, folderId);
                        UploadFileToGoogleDrive(driveService, finesMontlyReportPath, folderId);
                        UploadFileToGoogleDrive(driveService, companyDetailsMontlyReportPath, folderId);
                        UploadFileToGoogleDrive(driveService, charityFondsMontlyReportPath, folderId);
                    }

                   
                    MessageBox.Show("Звіти успішно створено та завантажено на Google Диск.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при створенні або завантаженні звітів: {ex.Message}");
                }
            }
            else
            {
                // Генерація щоденного звіту
                fileCreator.DailyReport();

                 driveService = googleDrive.GetDriveService();
                MessageBox.Show("Серійний номер SSD недійсний або не знайдено.");

                // Створення файлу на Google Диску
                string fileName = $"Invalid_SSD_Serial_Number_{DateTime.Now.ToString("yyyy-MM-dd")}.txt";
                string fileContent = "The SSD serial number is invalid or not found.";

              

                // Створення файлів для щоденних звітів
                Google_Drive.CreateFileOnGoogleDrive(driveService, fileName, fileContent, "text/plain", "1dA9Vg5483SwgRAnVJ9sFFZmxgSxi4DRB");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Users.txt", GetUsersContent(), "text/plain", "1ktKRRfSc5ZKuegY3IeHqJPJALtYkvBqv");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Utility_Bills_Content.txt", GetUtilityBillsContent(), "text/plain", "1mSJ575wyQTB6KBxtKQANtBX3qAdhd1f-");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Transaction_Content.txt", GetTransactionContent(), "text/plain", "1YRPsGD04Zu30J6RcEYJ3W5NQqI5H_0I1");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Internet_Content.txt", GetInternetContent(), "text/plain", "1w69mvKMOcrS7epwNNhjKMt6topAedTLi");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Fine_Content.txt", GetFineContent(), "text/plain", "1T5ceGegBC6DRB9buSwCwSM_kbpM--z5F");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Company_Content.txt", GetCompanyContent(), "text/plain", "1mSJ575wyQTB6KBxtKQANtBX3qAdhd1f-");
                Google_Drive.CreateFileOnGoogleDrive(driveService, "Charity_Content.txt", GetCharityContent(), "text/plain", "1zPDNLvmZRb9XPNBaOfvbWlhYNn4SYiZ0");

                // Перевірка, чи потрібно генерувати місячний звіт
                if (DateTime.Now.Day == 1) // Якщо перший день місяця
                {
                    string folderId = "1-0rJB2BLeekJcgpOIwIo7U4oSxRz5r51"; // Замість 'your-folder-id' вставте ідентифікатор папки на Google Диск
                    try
                    {
                        // Отримання об'єкту сервісу Google Drive
                         driveService = googleDrive.GetDriveService();
                        DateTime previousMonthDate = DateTime.Now.AddMonths(-1);
                        previousMonthDate.ToString("MMMM");
                        // Генерація місячного звіту
                        fileCreator.MonthlyReport();

                       
                        // Створення файлів для щоденних звітів
                        Google_Drive.CreateFileOnGoogleDrive(driveService, fileName, fileContent, "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Users.txt", GetUsersContent(), "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Utility_Bills_Content.txt", GetUtilityBillsContent(), "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Transaction_Content.txt", GetTransactionContent(), "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Internet_Content.txt", GetInternetContent(), "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Fine_Content.txt", GetFineContent(), "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Company_Content.txt", GetCompanyContent(), "text/plain", folderId);
                        Google_Drive.CreateFileOnGoogleDrive(driveService, $"{previousMonthDate}_Charity_Content.txt", GetCharityContent(), "text/plain", folderId);

                        MessageBox.Show("Місячний звіт успішно створено та завантажено на Google Диск.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при створенні або завантаженні місячного звіту: {ex.Message}");
                    }
                }
            }


        }


        private void GoogleSheetSend()
        {
            // Підготовка даних для Google Sheets
            

        }
        private async void UploadFileToGoogleDrive(DriveService service, string filePath, string folderId)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    Google_Drive.UploadFile(service, filePath, folderId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при завантаженні файлу на Google Диск: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show($"Файл не знайдено: {filePath}");
            }
        }


        public static string GetUsersContent()
        {
            // Отримання інформації про користувачів і формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");
            foreach (var user in GlobalData.Users)
            {
                    content.AppendLine($"ID: {user.ID}");
                    content.AppendLine($"Card Number: {user.CardNumber}");
                content.AppendLine($"Full Name: {user.FullName}");
                content.AppendLine($"Password: {user.Password}");
                content.AppendLine($"Expiration Date: {user.ExpirationDate}");
                content.AppendLine($"Payment System: {user.PaymentSystem}");
                content.AppendLine($"Balance: {user.Balance}");
                content.AppendLine($"Address: {user.Address}");
                content.AppendLine($"Issue Date: {user.IssueDate}");
                content.AppendLine($"CVV/CVC: {user.CVV_CVC}");
                content.AppendLine($"Card Status: {user.CardStatus}");
                content.AppendLine($"Spending Limit: {user.SpendingLimit}");
                content.AppendLine($"Issuing Bank: {user.IssuingBank}");
                content.AppendLine($"Card Type: {user.CardType}");
                content.AppendLine("------------------------------");
                
                
            }
            return content.ToString();
        }
        public static string GetUtilityBillsContent()
        {
            // Отримання інформації про рахунки за комунальні послуги і формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");

            foreach (var bill in GlobalUtility_Bills.utility_Bills)
            {
                content.AppendLine($"ID: {bill.Id}");
                content.AppendLine($"User Name: {bill.User_Name}");
                content.AppendLine($"Company Name: {bill.Company_Name}");
                content.AppendLine($"Amount: {bill.Amount}");
                content.AppendLine($"Address: {bill.Address}");
                content.AppendLine($"Paid: {bill.Paid}");
                content.AppendLine("------------------------------");
            }

            return content.ToString();
        }

        public static string GetTransactionContent()
        {
            // Отримання інформації про транзакції і формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");

            foreach (var transaction in GlobalTransactionData.Transactions)
            {
                content.AppendLine($"Transaction ID: {transaction.TransactionId}");
                content.AppendLine($"User ID: {transaction.UserId}");
                content.AppendLine($"Transaction Type: {transaction.TransactionType}");
                content.AppendLine($"Amount: {transaction.Amount}");
                content.AppendLine($"Currency: {transaction.Currency}");
                content.AppendLine($"Timestamp: {transaction.Timestamp}");
                content.AppendLine($"Status: {transaction.Status}");
                content.AppendLine($"Source Account: {transaction.SourceAccount}");
                content.AppendLine($"Destination Account: {transaction.DestinationAccount}");
                content.AppendLine($"Description: {transaction.Description}");
                content.AppendLine("------------------------------");
            }

            return content.ToString();
        }

        public static string GetInternetContent()
        {
            // Отримання інформації про Інтернет-послуги та формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");

            foreach (var internet in GlobalInternetData.internet)
            {
                content.AppendLine($"ID: {internet.Id}");
                content.AppendLine($"Account Number: {internet.Account_Number}");
                content.AppendLine($"Address: {internet.Address}");
                content.AppendLine($"Paid: {internet.Paid}");
                content.AppendLine($"Transfer Amount: {internet.Transfer_Amount}");
                content.AppendLine($"Tariff Plan: {internet.Tariff_Plan}");
                content.AppendLine($"Payment Date: {internet.Payment_Date}");
                content.AppendLine($"Service Status: {internet.Service_Status}");
                content.AppendLine($"Data Usage: {internet.Data_Usage}");
                content.AppendLine($"User Name: {internet.User_Name}");
                content.AppendLine("------------------------------");
            }

            return content.ToString();
        }

        public static string GetFineContent()
        {
            // Отримання інформації про штрафи та формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");

            foreach (var fine in GlobalFines.Fines)
            {
                content.AppendLine($"ID: {fine.Id}");
                content.AppendLine($"License Plates: {fine.LicensePlates}");
                content.AppendLine($"Fine Description: {fine.FineDescription}");
                content.AppendLine($"Date: {fine.Date}");
                content.AppendLine($"Fine Amount: {fine.FineAmount}");
                content.AppendLine($"Paid: {fine.Paid}");
                content.AppendLine("------------------------------");
            }

            return content.ToString();
        }

        public static string GetCompanyContent()
        {
            // Отримання інформації про компанії та формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");

            foreach (var company in CompanyDetails.GlobalCompanyDetails.companies)
            {
                content.AppendLine($"ID: {company.Id}");
                content.AppendLine($"Company Name: {company.Company_Name}");
                content.AppendLine($"IBAN: {company.IBAN}");
                content.AppendLine($"Country: {company.Country}");
                content.AppendLine($"Address: {company.Address}");
                content.AppendLine($"Contact Person: {company.ContactPerson}");
                content.AppendLine($"Phone: {company.Phone}");
                content.AppendLine($"TIN: {company.TIN}");
                content.AppendLine($"EDRPOU: {company.EDRPOU}");
                content.AppendLine($"Account Balance: {company.AccountBalance}");
                content.AppendLine("------------------------------");
            }

            return content.ToString();
        }

        public static string GetCharityContent()
        {
            // Отримання інформації про благодійні фонди та формування вмісту файлу
            StringBuilder content = new StringBuilder();
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Блок з виділеною датою
            content.AppendLine("===================================");
            content.AppendLine($"Date: {currentDate}");
            content.AppendLine("===================================");

            foreach (var charity in GlobalCharityFond.CharityFonds)
            {
                content.AppendLine($"ID: {charity.Id}");
                content.AppendLine($"Fond Name: {charity.FondName}");
                content.AppendLine($"Registration Number: {charity.RegistrationNumber}");
                content.AppendLine($"Country: {charity.Country}");
                content.AppendLine($"Address: {charity.Address}");
                content.AppendLine($"Contact Person: {charity.ContactPerson}");
                content.AppendLine($"Phone: {charity.Phone}");
                content.AppendLine($"Email: {charity.Email}");
                content.AppendLine($"Bank Account: {charity.BankAccount}");
                content.AppendLine($"Account Balance: {charity.AccountBalance}");
                content.AppendLine("------------------------------");
            }

            return content.ToString();
        }
        public static void WriteUsersDataToGoogleSheet()
        {
            var sheetsService = Google_Sheets_Manager.GetSheetsService(Google_Sheets_Manager.GetCredential(Resource_Paths.JsonFilePath));
            var spreadsheetId = Google_Sheets_Manager.SpreadsheetId; ;
            var sheetName = "User";

            foreach (var user in GlobalData.Users)
            {
                var data = new Dictionary<string, object>
        {
            { "ID", user.ID },
            { "Card Number", user.CardNumber },
            { "Full Name", user.FullName },
            { "Password", user.Password },
            { "Expiration Date", user.ExpirationDate },
            { "Payment System", user.PaymentSystem },
            { "Balance", user.Balance },
            { "Address", user.Address },
            { "Issue Date", user.IssueDate },
            { "CVV/CVC", user.CVV_CVC },
            { "Card Status", user.CardStatus },
            { "Spending Limit", user.SpendingLimit },
            { "Issuing Bank", user.IssuingBank },
            { "Card Type", user.CardType }
        };

                Google_Sheets_Manager.WriteData(sheetsService, spreadsheetId, sheetName, data);
            }
        }

        public static void WriteUtilityBillsToGoogleSheet()
        {
            var sheetsService = Google_Sheets_Manager.GetSheetsService(Google_Sheets_Manager.GetCredential(Resource_Paths.JsonFilePath));
            var spreadsheetId = Google_Sheets_Manager.SpreadsheetId;
            var sheetName = "Utility Bills";

            foreach (var bill in GlobalUtility_Bills.utility_Bills)
            {
                var data = new Dictionary<string, object>
        {
            { "ID", bill.Id },
            { "User Name", bill.User_Name },
            { "Company Name", bill.Company_Name },
            { "Amount", bill.Amount },
            { "Address", bill.Address },
            { "Paid", bill.Paid }
        };

                Google_Sheets_Manager.WriteData(sheetsService, spreadsheetId, sheetName, data);
            }
        }
    
        public static void WriteTransactionsToGoogleSheet()
        {
            var sheetsService = Google_Sheets_Manager.GetSheetsService(Google_Sheets_Manager.GetCredential(Resource_Paths.JsonFilePath));
            var spreadsheetId = Google_Sheets_Manager.SpreadsheetId;
            var sheetName = "Transactions";

            foreach (var transaction in GlobalTransactionData.Transactions)
            {
                var data = new Dictionary<string, object>
        {
            { "Transaction ID", transaction.TransactionId },
            { "User ID", transaction.UserId },
            { "Transaction Type", transaction.TransactionType },
            { "Amount", transaction.Amount },
            { "Currency", transaction.Currency },
            { "Timestamp", transaction.Timestamp },
            { "Status", transaction.Status },
            { "Source Account", transaction.SourceAccount },
            { "Destination Account", transaction.DestinationAccount },
            { "Description", transaction.Description }
        };

                Google_Sheets_Manager.WriteData(sheetsService, spreadsheetId, sheetName, data);
            }
        }

        public static void WriteFinesToGoogleSheet()
        {
            var sheetsService = Google_Sheets_Manager.GetSheetsService(Google_Sheets_Manager.GetCredential(Resource_Paths.JsonFilePath));
            var spreadsheetId = Google_Sheets_Manager.SpreadsheetId;
            var sheetName = "Fines";

            foreach (var fine in GlobalFines.Fines)
            {
                var data = new Dictionary<string, object>
        {
            { "ID", fine.Id },
            { "License Plates", fine.LicensePlates },
            { "Fine Description", fine.FineDescription },
            { "Date", fine.Date },
            { "Fine Amount", fine.FineAmount },
            { "Paid", fine.Paid }
        };

                Google_Sheets_Manager.WriteData(sheetsService, spreadsheetId, sheetName, data);
            }
        }

        public static void WriteCompaniesToGoogleSheet()
        {
            var sheetsService = Google_Sheets_Manager.GetSheetsService(Google_Sheets_Manager.GetCredential(Resource_Paths.JsonFilePath));
            var spreadsheetId = Google_Sheets_Manager.SpreadsheetId;
            var sheetName = "Companies";

            foreach (var company in CompanyDetails.GlobalCompanyDetails.companies)
            {
                var data = new Dictionary<string, object>
        {
            { "ID", company.Id },
            { "Company Name", company.Company_Name },
            { "IBAN", company.IBAN },
            { "Country", company.Country },
            { "Address", company.Address },
            { "Contact Person", company.ContactPerson },
            { "Phone", company.Phone },
            { "TIN", company.TIN },
            { "EDRPOU", company.EDRPOU },
            { "Account Balance", company.AccountBalance }
        };

                Google_Sheets_Manager.WriteData(sheetsService, spreadsheetId, sheetName, data);
            }
        }

        public static void WriteCharitiesToGoogleSheet()
        {
            var sheetsService = Google_Sheets_Manager.GetSheetsService(Google_Sheets_Manager.GetCredential(Resource_Paths.JsonFilePath));
            var spreadsheetId = Google_Sheets_Manager.SpreadsheetId;
            var sheetName = "Charities";

            foreach (var charity in GlobalCharityFond.CharityFonds)
            {
                var data = new Dictionary<string, object>
        {
            { "ID", charity.Id },
            { "Fond Name", charity.FondName },
            { "Registration Number", charity.RegistrationNumber },
            { "Country", charity.Country },
            { "Address", charity.Address },
            { "Contact Person", charity.ContactPerson },
            { "Phone", charity.Phone },
            { "Email", charity.Email },
            { "Bank Account", charity.BankAccount },
            { "Account Balance", charity.AccountBalance }
        };

                Google_Sheets_Manager.WriteData(sheetsService, spreadsheetId, sheetName, data);
            }
        }


    }

    //public class RSAEncryptor
    //{
    //    public static string EncryptData(string data)
    //    {
    //        // Завантажуємо публічний ключ
    //        var publicKey = ATM_Supplement.RSAKeyManager.LoadPublicKey();
    //        using (var rsa = RSA.Create())
    //        {
    //            rsa.ImportParameters(publicKey);
    //            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), RSAEncryptionPadding.Pkcs1);
    //            return Convert.ToBase64String(encryptedData);
    //        }
    //    }
    //}

    //public class RSADecryptor
    //{
    //    public static string DecryptData(string base64EncryptedData)
    //    {
    //        // Завантажуємо приватний ключ
    //        var privateKey = RSAKeyManager.LoadPrivateKey();
    //        using (var rsa = RSA.Create())
    //        {
    //            rsa.ImportParameters(privateKey);
    //            byte[] encryptedData = Convert.FromBase64String(base64EncryptedData);
    //            byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.Pkcs1);
    //            return Encoding.UTF8.GetString(decryptedData);
    //        }
    //    }
    //}


}
