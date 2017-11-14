namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.trackBarAngleX = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarAngleY = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBarAngleZ = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.trackBarMoveX = new System.Windows.Forms.TrackBar();
            this.trackBarMoveZ = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarMoveY = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngleZ)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMoveX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMoveZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMoveY)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(560, 561);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // trackBarAngleX
            // 
            this.trackBarAngleX.Location = new System.Drawing.Point(3, 33);
            this.trackBarAngleX.Maximum = 360;
            this.trackBarAngleX.Name = "trackBarAngleX";
            this.trackBarAngleX.Size = new System.Drawing.Size(231, 45);
            this.trackBarAngleX.TabIndex = 1;
            this.trackBarAngleX.ValueChanged += new System.EventHandler(this.trackBarRotate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Поворот по оси OX";
            // 
            // trackBarAngleY
            // 
            this.trackBarAngleY.Location = new System.Drawing.Point(3, 97);
            this.trackBarAngleY.Maximum = 360;
            this.trackBarAngleY.Name = "trackBarAngleY";
            this.trackBarAngleY.Size = new System.Drawing.Size(231, 45);
            this.trackBarAngleY.TabIndex = 3;
            this.trackBarAngleY.ValueChanged += new System.EventHandler(this.trackBarRotate_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Поворот по оси OY";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(62, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Поворот по оси OZ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(3, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Проекция";
            // 
            // trackBarAngleZ
            // 
            this.trackBarAngleZ.Location = new System.Drawing.Point(3, 161);
            this.trackBarAngleZ.Maximum = 360;
            this.trackBarAngleZ.Name = "trackBarAngleZ";
            this.trackBarAngleZ.Size = new System.Drawing.Size(231, 45);
            this.trackBarAngleZ.TabIndex = 7;
            this.trackBarAngleZ.ValueChanged += new System.EventHandler(this.trackBarRotate_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.trackBarAngleX);
            this.panel1.Controls.Add(this.trackBarAngleZ);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.trackBarAngleY);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(563, 72);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(240, 234);
            this.panel1.TabIndex = 8;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 212);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(192, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Относительно центра координат";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(563, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(240, 69);
            this.panel2.TabIndex = 9;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Тетраэдр",
            "Гексаэдр (Куб)",
            "Октаэдр",
            "Икосаэдр",
            "Додекаэдр"});
            this.comboBox2.Location = new System.Drawing.Point(119, 39);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 9;
            this.comboBox2.Text = "Икосаэдр";
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Тип многогранника";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Центральная",
            "Параллельная"});
            this.comboBox1.Location = new System.Drawing.Point(119, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = "Центральная";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.trackBarMoveX);
            this.panel3.Controls.Add(this.trackBarMoveZ);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.trackBarMoveY);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Location = new System.Drawing.Point(563, 306);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(240, 199);
            this.panel3.TabIndex = 10;
            // 
            // trackBarMoveX
            // 
            this.trackBarMoveX.Location = new System.Drawing.Point(6, 19);
            this.trackBarMoveX.Maximum = 1000;
            this.trackBarMoveX.Minimum = -1000;
            this.trackBarMoveX.Name = "trackBarMoveX";
            this.trackBarMoveX.Size = new System.Drawing.Size(231, 45);
            this.trackBarMoveX.TabIndex = 1;
            this.trackBarMoveX.ValueChanged += new System.EventHandler(this.trackBarTransfer_ValueChanged);
            // 
            // trackBarMoveZ
            // 
            this.trackBarMoveZ.Location = new System.Drawing.Point(6, 146);
            this.trackBarMoveZ.Maximum = 300;
            this.trackBarMoveZ.Minimum = -300;
            this.trackBarMoveZ.Name = "trackBarMoveZ";
            this.trackBarMoveZ.Size = new System.Drawing.Size(231, 45);
            this.trackBarMoveZ.TabIndex = 7;
            this.trackBarMoveZ.ValueChanged += new System.EventHandler(this.trackBarTransfer_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(62, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Сдвиг по оси OX";
            // 
            // trackBarMoveY
            // 
            this.trackBarMoveY.Location = new System.Drawing.Point(6, 83);
            this.trackBarMoveY.Maximum = 300;
            this.trackBarMoveY.Minimum = -300;
            this.trackBarMoveY.Name = "trackBarMoveY";
            this.trackBarMoveY.Size = new System.Drawing.Size(231, 45);
            this.trackBarMoveY.TabIndex = 3;
            this.trackBarMoveY.ValueChanged += new System.EventHandler(this.trackBarTransfer_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(62, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Сдвиг по оси OZ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(62, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Сдвиг по оси OY";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 561);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Программа - убийца памяти";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAngleZ)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMoveX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMoveZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMoveY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TrackBar trackBarAngleX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarAngleY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBarAngleZ;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TrackBar trackBarMoveX;
        private System.Windows.Forms.TrackBar trackBarMoveZ;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TrackBar trackBarMoveY;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

