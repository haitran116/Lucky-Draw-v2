namespace luckydraw_v2
{
    partial class Form2
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            label_show_ketqua_form2 = new Label();
            button1 = new Button();
            comboBox_show_ketqua = new ComboBox();
            label_chucmung = new Label();
            timer_nhapnhay = new System.Windows.Forms.Timer(components);
            label_quatang_form2 = new Label();
            pictureBox_giaithuong = new PictureBox();
            pictureBox_luckydraw = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox_giaithuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_luckydraw).BeginInit();
            SuspendLayout();
            // 
            // label_show_ketqua_form2
            // 
            label_show_ketqua_form2.AutoSize = true;
            label_show_ketqua_form2.BackColor = Color.Transparent;
            label_show_ketqua_form2.Font = new Font("Segoe UI", 12.75F, FontStyle.Bold, GraphicsUnit.Point);
            label_show_ketqua_form2.ForeColor = Color.AliceBlue;
            label_show_ketqua_form2.Location = new Point(199, 370);
            label_show_ketqua_form2.Name = "label_show_ketqua_form2";
            label_show_ketqua_form2.Size = new Size(1079, 92);
            label_show_ketqua_form2.TabIndex = 0;
            label_show_ketqua_form2.Text = resources.GetString("label_show_ketqua_form2.Text");
            label_show_ketqua_form2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.BackColor = Color.Red;
            button1.ForeColor = SystemColors.ButtonHighlight;
            button1.Location = new Point(1332, 733);
            button1.Name = "button1";
            button1.Size = new Size(22, 23);
            button1.TabIndex = 1;
            button1.Text = "X";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // comboBox_show_ketqua
            // 
            comboBox_show_ketqua.BackColor = Color.OrangeRed;
            comboBox_show_ketqua.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox_show_ketqua.ForeColor = SystemColors.MenuBar;
            comboBox_show_ketqua.FormattingEnabled = true;
            comboBox_show_ketqua.Location = new Point(12, 732);
            comboBox_show_ketqua.Name = "comboBox_show_ketqua";
            comboBox_show_ketqua.Size = new Size(191, 24);
            comboBox_show_ketqua.TabIndex = 50;
            comboBox_show_ketqua.SelectedIndexChanged += comboBox_show_ketqua_SelectedIndexChanged;
            // 
            // label_chucmung
            // 
            label_chucmung.AutoSize = true;
            label_chucmung.BackColor = Color.Transparent;
            label_chucmung.Font = new Font("Segoe UI", 21.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label_chucmung.ForeColor = Color.Indigo;
            label_chucmung.Location = new Point(611, 314);
            label_chucmung.Name = "label_chucmung";
            label_chucmung.Size = new Size(168, 40);
            label_chucmung.TabIndex = 52;
            label_chucmung.Text = "Chúc mừng";
            // 
            // timer_nhapnhay
            // 
            timer_nhapnhay.Tick += timer_nhapnhay_Tick;
            // 
            // label_quatang_form2
            // 
            label_quatang_form2.AutoSize = true;
            label_quatang_form2.BackColor = Color.Transparent;
            label_quatang_form2.Font = new Font("Tahoma", 12F, FontStyle.Bold, GraphicsUnit.Point);
            label_quatang_form2.ForeColor = Color.Indigo;
            label_quatang_form2.Location = new Point(389, 562);
            label_quatang_form2.Name = "label_quatang_form2";
            label_quatang_form2.Size = new Size(614, 76);
            label_quatang_form2.TabIndex = 54;
            label_quatang_form2.Text = resources.GetString("label_quatang_form2.Text");
            label_quatang_form2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox_giaithuong
            // 
            pictureBox_giaithuong.BackColor = Color.Transparent;
            pictureBox_giaithuong.Image = (Image)resources.GetObject("pictureBox_giaithuong.Image");
            pictureBox_giaithuong.Location = new Point(597, 520);
            pictureBox_giaithuong.Name = "pictureBox_giaithuong";
            pictureBox_giaithuong.Size = new Size(192, 30);
            pictureBox_giaithuong.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_giaithuong.TabIndex = 53;
            pictureBox_giaithuong.TabStop = false;
            // 
            // pictureBox_luckydraw
            // 
            pictureBox_luckydraw.BackColor = Color.Transparent;
            pictureBox_luckydraw.Image = (Image)resources.GetObject("pictureBox_luckydraw.Image");
            pictureBox_luckydraw.Location = new Point(389, 94);
            pictureBox_luckydraw.Name = "pictureBox_luckydraw";
            pictureBox_luckydraw.Size = new Size(617, 104);
            pictureBox_luckydraw.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox_luckydraw.TabIndex = 55;
            pictureBox_luckydraw.TabStop = false;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1366, 768);
            Controls.Add(pictureBox_luckydraw);
            Controls.Add(label_quatang_form2);
            Controls.Add(pictureBox_giaithuong);
            Controls.Add(label_chucmung);
            Controls.Add(comboBox_show_ketqua);
            Controls.Add(button1);
            Controls.Add(label_show_ketqua_form2);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form2";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox_giaithuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox_luckydraw).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_show_ketqua_form2;
        private Button button1;
        private ComboBox comboBox_show_ketqua;
        private Label label_chucmung;
        private System.Windows.Forms.Timer timer_nhapnhay;
        private Label label_quatang_form2;
        private PictureBox pictureBox_giaithuong;
        private PictureBox pictureBox_luckydraw;
    }
}