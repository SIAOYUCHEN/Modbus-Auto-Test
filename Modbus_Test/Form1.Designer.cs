namespace Modbus_Test
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Serial_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Timer_Start_Button = new System.Windows.Forms.Button();
            this.Timer_Close_Button = new System.Windows.Forms.Button();
            this.Modbus_Serial = new System.IO.Ports.SerialPort(this.components);
            this.Total_TextBox = new System.Windows.Forms.TextBox();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.Correct_TextBox = new System.Windows.Forms.TextBox();
            this.Error_TextBox = new System.Windows.Forms.TextBox();
            this.TimeOut_Count_TextBox = new System.Windows.Forms.TextBox();
            this.Error_Count_TextBox = new System.Windows.Forms.TextBox();
            this.Total_Count_TextBox = new System.Windows.Forms.TextBox();
            this.Correct_Count_TextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(44, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "請選擇串列埠";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(180, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Serial_Button
            // 
            this.Serial_Button.Location = new System.Drawing.Point(319, 32);
            this.Serial_Button.Name = "Serial_Button";
            this.Serial_Button.Size = new System.Drawing.Size(75, 23);
            this.Serial_Button.TabIndex = 2;
            this.Serial_Button.Text = "open";
            this.Serial_Button.UseVisualStyleBackColor = true;
            this.Serial_Button.Click += new System.EventHandler(this.Serial_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(409, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "定時器關閉";
            // 
            // Timer_Start_Button
            // 
            this.Timer_Start_Button.Location = new System.Drawing.Point(524, 33);
            this.Timer_Start_Button.Name = "Timer_Start_Button";
            this.Timer_Start_Button.Size = new System.Drawing.Size(75, 23);
            this.Timer_Start_Button.TabIndex = 4;
            this.Timer_Start_Button.Text = "start";
            this.Timer_Start_Button.UseVisualStyleBackColor = true;
            this.Timer_Start_Button.Click += new System.EventHandler(this.Timer_Start_Button_Click);
            // 
            // Timer_Close_Button
            // 
            this.Timer_Close_Button.Location = new System.Drawing.Point(616, 33);
            this.Timer_Close_Button.Name = "Timer_Close_Button";
            this.Timer_Close_Button.Size = new System.Drawing.Size(75, 23);
            this.Timer_Close_Button.TabIndex = 5;
            this.Timer_Close_Button.Text = "close";
            this.Timer_Close_Button.UseVisualStyleBackColor = true;
            this.Timer_Close_Button.Click += new System.EventHandler(this.Timer_Close_Button_Click);
            // 
            // Modbus_Serial
            // 
            this.Modbus_Serial.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.Modbus_Serial_DataReceived);
            // 
            // Total_TextBox
            // 
            this.Total_TextBox.Location = new System.Drawing.Point(3, 93);
            this.Total_TextBox.Multiline = true;
            this.Total_TextBox.Name = "Total_TextBox";
            this.Total_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Total_TextBox.Size = new System.Drawing.Size(261, 142);
            this.Total_TextBox.TabIndex = 6;
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // Correct_TextBox
            // 
            this.Correct_TextBox.Location = new System.Drawing.Point(270, 93);
            this.Correct_TextBox.Multiline = true;
            this.Correct_TextBox.Name = "Correct_TextBox";
            this.Correct_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Correct_TextBox.Size = new System.Drawing.Size(261, 142);
            this.Correct_TextBox.TabIndex = 7;
            // 
            // Error_TextBox
            // 
            this.Error_TextBox.Location = new System.Drawing.Point(551, 93);
            this.Error_TextBox.Multiline = true;
            this.Error_TextBox.Name = "Error_TextBox";
            this.Error_TextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Error_TextBox.Size = new System.Drawing.Size(261, 142);
            this.Error_TextBox.TabIndex = 8;
            // 
            // TimeOut_Count_TextBox
            // 
            this.TimeOut_Count_TextBox.Location = new System.Drawing.Point(712, 32);
            this.TimeOut_Count_TextBox.Name = "TimeOut_Count_TextBox";
            this.TimeOut_Count_TextBox.Size = new System.Drawing.Size(100, 22);
            this.TimeOut_Count_TextBox.TabIndex = 9;
            // 
            // Error_Count_TextBox
            // 
            this.Error_Count_TextBox.Location = new System.Drawing.Point(712, 65);
            this.Error_Count_TextBox.Name = "Error_Count_TextBox";
            this.Error_Count_TextBox.Size = new System.Drawing.Size(100, 22);
            this.Error_Count_TextBox.TabIndex = 10;
            // 
            // Total_Count_TextBox
            // 
            this.Total_Count_TextBox.Location = new System.Drawing.Point(59, 65);
            this.Total_Count_TextBox.Name = "Total_Count_TextBox";
            this.Total_Count_TextBox.Size = new System.Drawing.Size(100, 22);
            this.Total_Count_TextBox.TabIndex = 11;
            // 
            // Correct_Count_TextBox
            // 
            this.Correct_Count_TextBox.Location = new System.Drawing.Point(343, 65);
            this.Correct_Count_TextBox.Name = "Correct_Count_TextBox";
            this.Correct_Count_TextBox.Size = new System.Drawing.Size(100, 22);
            this.Correct_Count_TextBox.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 273);
            this.Controls.Add(this.Correct_Count_TextBox);
            this.Controls.Add(this.Total_Count_TextBox);
            this.Controls.Add(this.Error_Count_TextBox);
            this.Controls.Add(this.TimeOut_Count_TextBox);
            this.Controls.Add(this.Error_TextBox);
            this.Controls.Add(this.Correct_TextBox);
            this.Controls.Add(this.Total_TextBox);
            this.Controls.Add(this.Timer_Close_Button);
            this.Controls.Add(this.Timer_Start_Button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Serial_Button);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button Serial_Button;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Timer_Start_Button;
        private System.Windows.Forms.Button Timer_Close_Button;
        private System.IO.Ports.SerialPort Modbus_Serial;
        private System.Windows.Forms.TextBox Total_TextBox;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.TextBox Correct_TextBox;
        private System.Windows.Forms.TextBox Error_TextBox;
        private System.Windows.Forms.TextBox TimeOut_Count_TextBox;
        private System.Windows.Forms.TextBox Error_Count_TextBox;
        private System.Windows.Forms.TextBox Total_Count_TextBox;
        private System.Windows.Forms.TextBox Correct_Count_TextBox;
    }
}

