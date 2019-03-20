using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace Modbus_Test
{
    public partial class Form1 : Form
    {
        String[] PortNames;
        //Byte[] buffer = { 0x01, 0x04, 0x00, 0x02, 0x00, 0x01, 0x90, 0x0A};
        Byte[] buffer;
        Boolean Is_Need_Receive = false;
        Int32 Timeout_Count = 0;
        Int32 Temp_Receive_Length = 0, Receive_Length = 0;
        Int32 TimeOut_Error_Times = 0, Error_Times = 0,Total_Times = 0,Correct_Times = 0;
        Byte[] Temp_Receive_Buffer = new Byte[1024];
        delegate void Display(Byte[] buffer);
        Modbus_Rule modbus = new Modbus_Rule();
        Csv_File file = new Csv_File();
        
        Stopwatch s = new Stopwatch();
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PortNames = SerialPort.GetPortNames();
            if (PortNames != null)
            {
                foreach (string portname in PortNames)
                {
                    comboBox1.Items.Add(portname);
                }
            }

            file.dt.Columns.Add(new DataColumn("TX"));
            file.dt.Columns.Add(new DataColumn("RX"));
            file.dt.Columns.Add(new DataColumn("RESULT"));
            
            file.SaveCsv(file.dt, file.GetFileName());
        }

        private void Serial_Button_Click(object sender, EventArgs e)
        {
            if(PortNames != null && comboBox1.Text != "")
            {
                Modbus_Serial = new SerialPort(comboBox1.Text, 9600, Parity.None, 8, StopBits.One);
                Modbus_Serial.ReceivedBytesThreshold = 1;
                Enable_Timer1();
                Modbus_Serial.DataReceived += new SerialDataReceivedEventHandler(Modbus_Serial_DataReceived);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Modbus_Serial.Dispose();
            Modbus_Serial.Open();
            buffer = modbus.GetTransmit();
            Modbus_Serial.Write(buffer, 0, buffer.Length);
            Enable_Timer2();
            Is_Need_Receive = true;
            switch (buffer[modbus.Modbus_FunctionCode_Index])
            {
                    case Modbus_Rule.FuctionCode3:
                    case Modbus_Rule.FuctionCode4:
                        Receive_Length = modbus.GetResponseLength(buffer[modbus.Modbus_FunctionCode_Index], buffer[modbus.Modbus_Quantity_Index]);
                        break;
                    case Modbus_Rule.FuctionCode6:
                        Receive_Length = modbus.GetResponseLength(buffer[modbus.Modbus_FunctionCode_Index], buffer.Length);
                        break;
                    case Modbus_Rule.FuctionCode16:
                        Receive_Length = modbus.GetResponseLength(buffer[modbus.Modbus_FunctionCode_Index], buffer[modbus.Modbus_Modbus_FunctionCode16_Quantity_Index]);
                        break;
            }

            Total_TextBox.AppendText("Tx: ");
            Total_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
            Total_TextBox.AppendText(Environment.NewLine);
            Total_Times++;
            Total_Count_TextBox.Text = Total_Times.ToString();
            //Modbus_Serial.Close();
            //s.Start();//開始計時

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Timeout_Count++;

            if (Timeout_Count == modbus.Modbus_Timeout)
            {
                if (buffer[modbus.Modbus_Id_Index] != 0)
                {
                    TimeOut_Error_Times++;
                    TimeOut_Count_TextBox.Text = TimeOut_Error_Times.ToString();
                    Error_TextBox.AppendText("Tx: ");
                    Error_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
                    Error_TextBox.AppendText(Environment.NewLine);

                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RESULT"] = "FALSE";
                    file.dt.Rows.Add(dr);
                }
                else
                {
                    Correct_Times++;
                    Correct_Count_TextBox.Text = Correct_Times.ToString();

                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RESULT"] = "TRUE";
                    file.dt.Rows.Add(dr);
                }

                file.SaveCsv(file.dt, file.GetFileName());
                Is_Need_Receive = false;
                
                Clear_Timer2_Parameter();
                //s.Stop();//開始計時

                //TimeOut_Count_TextBox.Text = ((s.ElapsedMilliseconds).ToString() + "毫秒");
                //s.Reset();
            }
        }

        private void Modbus_Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Int16 j = 0;
            Int32 length = 0;
            Byte[] Receive_Buffer = new Byte[256];
            /*
            if (Is_Need_Receive == true)
            {*/
            if (Modbus_Serial.IsOpen)
            {
                Thread.Sleep(Convert.ToInt16(Receive_Length * 2.5));///Receive_Length * 2
                try
                {
                    length = Modbus_Serial.Read(Receive_Buffer, 0, Receive_Buffer.Length);
                    j = 0;
                    for (int i = Temp_Receive_Length; i < Temp_Receive_Length + length; i++)// Temp_Receive_Length + length
                    {

                        Temp_Receive_Buffer[i] = Receive_Buffer[j];
                        j++;
                    }

                    Temp_Receive_Length += length;
                    /*
                    if (Temp_Receive_Length == Receive_Length)
                    {
                        if (modbus.GetCrcEqual(ref Temp_Receive_Buffer, Temp_Receive_Buffer.Length - modbus.Temp_Receive_Buffer_Offest,
                                           Temp_Receive_Buffer[Temp_Receive_Buffer.Length - modbus.Temp_Receive_Buffer_Crc_Offest2],
                                           Temp_Receive_Buffer[Temp_Receive_Buffer.Length - modbus.Temp_Receive_Buffer_Crc_Offest1]) == true)
                        {
                                Array.Resize(ref Temp_Receive_Buffer, Temp_Receive_Length);// 傳入矩陣位址
                                Display receive_result = new Display(DisplayText);
                                this.Invoke(receive_result, new Object[] { Temp_Receive_Buffer });
                                Clear_Timer2_Parameter();
                        }
                    }
                   else if (Temp_Receive_Length == modbus.Modbus_Error_Length)
                    {
                       if (modbus.GetCrcEqual(ref Temp_Receive_Buffer, Temp_Receive_Length - modbus.Temp_Receive_Buffer_Offest,
                                          Temp_Receive_Buffer[Temp_Receive_Length - modbus.Temp_Receive_Buffer_Crc_Offest2],
                                          Temp_Receive_Buffer[Temp_Receive_Length - modbus.Temp_Receive_Buffer_Crc_Offest1]) == true)
                       {
                         Array.Resize(ref Temp_Receive_Buffer, Temp_Receive_Length);// 傳入矩陣位址
                         Display receive_result = new Display(DisplayText);
                         this.Invoke(receive_result, new Object[] { Temp_Receive_Buffer });
                         Clear_Timer2_Parameter();
                       }
                   }*/
                    if (Temp_Receive_Length == Receive_Length || Temp_Receive_Length == modbus.Modbus_Error_Length)
                    {

                        Array.Resize(ref Temp_Receive_Buffer, Temp_Receive_Length);// 傳入矩陣位址
                        Display receive_result = new Display(DisplayText);
                        this.Invoke(receive_result, new Object[] { Temp_Receive_Buffer });
                        Clear_Timer2_Parameter();
                    }
                    else
                    {/*
                        Array.Resize(ref Temp_Receive_Buffer, Temp_Receive_Length);// 傳入矩陣位址
                        Display receive_result = new Display(Display_Error_Text);
                        this.Invoke(receive_result, new Object[] { Temp_Receive_Buffer });
                        Clear_Timer2_Parameter();*/
                    }
                }
                catch
                {

                }
                }/*
            }*/
        }

        private void DisplayText(Byte[] Receive_Buffer)
        {/*
            if (Temp_Receive_Buffer.Length != modbus.Modbus_Error_Length)
            {
                if (Temp_Receive_Buffer[modbus.Modbus_Id_Index] == buffer[modbus.Modbus_Id_Index])
                {
                    Display_Correct_Text(Receive_Buffer);
                }
                else
                {
                    Display_Error_Text(Receive_Buffer);
                }
            }
            else
            {
                Display_Error_Text(Receive_Buffer);
            }*/
            if (modbus.GetCrcEqual(ref Receive_Buffer, Temp_Receive_Length - modbus.Temp_Receive_Buffer_Offest,
                                  Receive_Buffer[Temp_Receive_Length - modbus.Temp_Receive_Buffer_Crc_Offest2],
                                  Receive_Buffer[Temp_Receive_Length - modbus.Temp_Receive_Buffer_Crc_Offest1]) == true)
            {
                if (Temp_Receive_Buffer.Length != modbus.Modbus_Error_Length)
                {
                    if (Temp_Receive_Buffer[modbus.Modbus_Id_Index] == buffer[modbus.Modbus_Id_Index])
                    {
                        Display_Correct_Text(Receive_Buffer);
                    }
                    else
                    {
                        Display_Error_Text(Receive_Buffer);
                    }
                }
                else
                {
                    Display_Error_Text(Receive_Buffer);
                }
            }
        }

        private void Display_Correct_Text(Byte[] Receive_Buffer)
        {
            //Total_TextBox.AppendText("Rx: ");
            //Total_TextBox.AppendText(BitConverter.ToString(Receive_Buffer).Replace("-", " "));
            //Total_TextBox.AppendText(Environment.NewLine);
            Correct_TextBox.AppendText("Tx: ");
            Correct_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
            Correct_TextBox.AppendText(Environment.NewLine);
            Correct_TextBox.AppendText("Rx: ");
            Correct_TextBox.AppendText(BitConverter.ToString(Receive_Buffer).Replace("-", " "));
            Correct_TextBox.AppendText(Environment.NewLine);

            DataRow dr = file.dt.NewRow();   //建立新的Rows
            dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
            dr["RX"] = BitConverter.ToString(Receive_Buffer).Replace("-", " ");
            dr["RESULT"] = "TRUE";
            file.dt.Rows.Add(dr);

            file.SaveCsv(file.dt, file.GetFileName());

            Correct_Times++;
            Correct_Count_TextBox.Text = Correct_Times.ToString();
            Temp_Receive_Buffer = new Byte[1024];
        }

        private void Display_Error_Text(Byte[] Receive_Buffer)
        {
            Error_TextBox.AppendText("Tx: ");
            Error_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
            Error_TextBox.AppendText(Environment.NewLine);
            Error_TextBox.AppendText("Rx: ");
            Error_TextBox.AppendText(BitConverter.ToString(Receive_Buffer).Replace("-", " "));
            Error_TextBox.AppendText(Environment.NewLine);
            Error_TextBox.ForeColor = Color.Red;

            DataRow dr = file.dt.NewRow();   //建立新的Rows
            dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
            dr["RX"] = BitConverter.ToString(Receive_Buffer).Replace("-", " ");
            dr["RESULT"] = "FALSE";
            file.dt.Rows.Add(dr);

            file.SaveCsv(file.dt, file.GetFileName());

            Error_Times++;
            Error_Count_TextBox.Text = Error_Times.ToString();
            Temp_Receive_Buffer = new Byte[1024];
        }

        private void Timer_Start_Button_Click(object sender, EventArgs e)
        {
            Enable_Timer1();
        }

        private void Timer_Close_Button_Click(object sender, EventArgs e)
        {
            Disable_Timer1();
        }

        private void Enable_Timer1()
        {
            timer1.Enabled = true;
        }

        private void Disable_Timer1()
        {
            timer1.Enabled = false;
        }

        private void Enable_Timer2()
        {
            timer2.Start();
        }

        private void Disable_Timer2()
        {
            timer2.Stop();
            timer2.Dispose();
        }

        private void Clear_Timer2_Parameter()
        {
            Timeout_Count = 0;
            Temp_Receive_Length = 0;
            Disable_Timer2();
            Is_Need_Receive = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
