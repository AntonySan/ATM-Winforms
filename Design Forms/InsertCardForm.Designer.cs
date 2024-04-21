namespace ATM_Winforms.Design_Forms
{
    partial class InsertCardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertCardForm));
            InsertCardBtn = new Button();
            SuspendLayout();
            // 
            // InsertCardBtn
            // 
            InsertCardBtn.BackColor = SystemColors.Control;
            InsertCardBtn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point);
            InsertCardBtn.ForeColor = SystemColors.WindowFrame;
            InsertCardBtn.Location = new Point(256, 503);
            InsertCardBtn.Name = "InsertCardBtn";
            InsertCardBtn.Size = new Size(310, 57);
            InsertCardBtn.TabIndex = 0;
            InsertCardBtn.Text = "вставити картку";
            InsertCardBtn.UseVisualStyleBackColor = false;
            InsertCardBtn.Click += InsertCardBtn_Click;
            // 
            // InsertCardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(826, 600);
            Controls.Add(InsertCardBtn);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "InsertCardForm";
            Text = "AURA BANK";
            Load += InsertCardForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button InsertCardBtn;
    }
}