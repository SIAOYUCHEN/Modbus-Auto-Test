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
        #region Initialize
        System.Threading.Timer SendData_Timer = null;
        System.Threading.Timer Timeout_Timer = null;
        String[] PortNames;
        //Byte[] buffer = { 0x01, 0x04, 0x00, 0x02, 0x00, 0x01, 0x90, 0x0A};
        Byte[] buffer;
        Boolean Timeout_Flag = true, Mutex_Flag = false;
        Int32 Temp_Receive_Length = 0, Receive_Length = 0;
        Int32 TimeOut_Error_Times = 0, Error_Times = 0, Total_Times = 0, Correct_Times = 0;
        Byte[] Temp_Receive_Buffer = new Byte[1024];
        delegate void Display(Byte[] buffer);
        delegate void Display_SendData();
        delegate void Display_Timeout_Correct_Data();
        delegate void Display_Timeout_Error_Data();
        Modbus_Rule modbus = new Modbus_Rule();
        Csv_File file = new Csv_File();
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        #region Form_Load
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
        #endregion

        #region Serial_Button_Click
        private void Serial_Button_Click(object sender, EventArgs e)
        {
            if (PortNames != null && comboBox1.Text != "")
            {
                Modbus_Serial = new SerialPort(comboBox1.Text, 9600, Parity.None, 8, StopBits.One);
                Modbus_Serial.ReceivedBytesThreshold = 1;
                Modbus_Serial.DataReceived += new SerialDataReceivedEventHandler(Modbus_Serial_DataReceived);
                SendData_Timer = new System.Threading.Timer(this.SendData_Timer_Tick, null, 0, 1000);
            }
        }
        #endregion

        #region Send_Data
        public void SendData_Timer_Tick(object info)
        {
            Modbus_Serial.Dispose();
            try
            {
                Modbus_Serial.Open();

                buffer = modbus.GetTransmit();
                Modbus_Serial.Write(buffer, 0, buffer.Length);

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

                Display_SendData receive_result = new Display_SendData(Display_Total_Text);
                this.Invoke(receive_result);
                Timeout_Timer = new System.Threading.Timer(this.Timeout_Timer_Tick, null, 100, 100);

            }
            catch
            {

            }
        }
        #endregion

        #region Timeout
        public void Timeout_Timer_Tick(object info)
        {
            if (Timeout_Flag == false)
            {
                Timeout_Flag = true;
                //Disable_Timeout_Timer();
            }
            else
            {
                Mutex_Flag = true;
                if (buffer[modbus.Modbus_Id_Index] != 0)
                {
                    Display_SendData receive_result = new Display_SendData(Display_Timeout_Error_Text);
                    this.Invoke(receive_result);
                    /*
                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RESULT"] = "FALSE";
                    file.dt.Rows.Add(dr);
                    file.SaveCsv(file.dt, file.GetFileName());*/
                    Save_Data(true, false, (Byte[])null);// broacst result
                    //Disable_Timeout_Timer();
                }
                else if (buffer[modbus.Modbus_Id_Index] == 0)
                {
                    Display_SendData receive_result = new Display_SendData(Display_Timeout_Correct_Text);
                    this.Invoke(receive_result);
                    /*
                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RESULT"] = "TRUE";
                    file.dt.Rows.Add(dr);
                    file.SaveCsv(file.dt, file.GetFileName());*/
                    Save_Data(true, true, (Byte[])null);// broacst result
                    //Disable_Timeout_Timer();
                }
                //file.SaveCsv(file.dt, file.GetFileName());
            }
            Disable_Timeout_Timer();
        }
        #endregion

        #region Receive_Data
        private void Modbus_Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Int16 j = 0;
            Int32 length = 0;
            Byte[] Receive_Buffer = new Byte[256];

            if (Modbus_Serial.IsOpen)
            {
                Thread.Sleep(Convert.ToInt16(Receive_Length * 2));
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

                    if (Temp_Receive_Length == Receive_Length || Temp_Receive_Length == modbus.Modbus_Error_Length)
                    {
                        Timeout_Flag = false;
                        if (Mutex_Flag != true)
                        {
                            Array.Resize(ref Temp_Receive_Buffer, Temp_Receive_Length);// 傳入矩陣位址
                            Display receive_result = new Display(DisplayText);
                            this.Invoke(receive_result, new Object[] { Temp_Receive_Buffer });

                        }
                        else
                        {
                            Mutex_Flag = false;
                            Temp_Receive_Length = 0;
                            Temp_Receive_Buffer = new Byte[1024];
                        }
                    }
                    else
                    {
                        Timeout_Flag = false;
                        Mutex_Flag = false;
                        Temp_Receive_Length = 0;
                    }
                }
                catch
                {

                }
            }
        }
        #endregion

        #region Display_Text
        private void DisplayText(Byte[] Receive_Buffer)
        {
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
                Temp_Receive_Length = 0;
            }
        }

        private void Display_Correct_Text(Byte[] Receive_Buffer)
        {
            Correct_TextBox.AppendText("Tx: ");
            Correct_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
            Correct_TextBox.AppendText(Environment.NewLine);
            Correct_TextBox.AppendText("Rx: ");
            Correct_TextBox.AppendText(BitConverter.ToString(Receive_Buffer).Replace("-", " "));
            Correct_TextBox.AppendText(Environment.NewLine);
            /*
            DataRow dr = file.dt.NewRow();   //建立新的Rows
            dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
            dr["RX"] = BitConverter.ToString(Receive_Buffer).Replace("-", " ");
            dr["RESULT"] = "TRUE";
            file.dt.Rows.Add(dr);

            file.SaveCsv(file.dt, file.GetFileName());*/
            Save_Data(false,true, Receive_Buffer);// broacst result

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
            /*
            DataRow dr = file.dt.NewRow();   //建立新的Rows
            dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
            dr["RX"] = BitConverter.ToString(Receive_Buffer).Replace("-", " ");
            dr["RESULT"] = "FALSE";
            file.dt.Rows.Add(dr);

            file.SaveCsv(file.dt, file.GetFileName());*/
            Save_Data(false, false, Receive_Buffer);// broacst result

            Error_Times++;
            Error_Count_TextBox.Text = Error_Times.ToString();
            Temp_Receive_Buffer = new Byte[1024];
        }

        private void Display_Total_Text()
        {
            Total_Times++;

            Total_TextBox.AppendText("Tx: ");
            Total_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
            Total_TextBox.AppendText(Environment.NewLine);
            Total_Count_TextBox.Text = Total_Times.ToString();
        }

        private void Display_Timeout_Correct_Text()
        {
            Correct_Times++;
            Correct_Count_TextBox.Text = Correct_Times.ToString();
        }

        private void Display_Timeout_Error_Text()
        {
            TimeOut_Error_Times++;
            TimeOut_Count_TextBox.Text = TimeOut_Error_Times.ToString();
            Error_TextBox.AppendText("Tx: ");
            Error_TextBox.AppendText(BitConverter.ToString(buffer).Replace("-", " "));
            Error_TextBox.AppendText(Environment.NewLine);
        }
        #endregion

        #region Save_Data
        private void Save_Data(bool IsBroadCast, bool flag, Byte[] temp)
        {
            if (IsBroadCast == true)
            {
                if (flag == true)
                {
                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RESULT"] = "TRUE";
                    file.dt.Rows.Add(dr);
                    file.SaveCsv(file.dt, file.GetFileName());
                }
                else
                {
                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RESULT"] = "FALSE";
                    file.dt.Rows.Add(dr);
                    file.SaveCsv(file.dt, file.GetFileName());
                }
            }
            else
            {
                if (flag == true)
                {
                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RX"] = BitConverter.ToString(temp).Replace("-", " ");
                    dr["RESULT"] = "TRUE";
                    file.dt.Rows.Add(dr);

                    file.SaveCsv(file.dt, file.GetFileName());
                }
                else
                {
                    DataRow dr = file.dt.NewRow();   //建立新的Rows
                    dr["TX"] = BitConverter.ToString(buffer).Replace("-", " ");
                    dr["RX"] = BitConverter.ToString(temp).Replace("-", " ");
                    dr["RESULT"] = "FALSE";
                    file.dt.Rows.Add(dr);

                    file.SaveCsv(file.dt, file.GetFileName());
                }
            }
        }
        #endregion

        #region Timer_Start_Button_Click
        private void Timer_Start_Button_Click(object sender, EventArgs e)
        {
            SendData_Timer = new System.Threading.Timer(this.SendData_Timer_Tick, null, 0, 1000);
        }
        #endregion

        #region Timer_Close_Button_Click
        private void Timer_Close_Button_Click(object sender, EventArgs e)
        {
            if (SendData_Timer != null)
            {
                SendData_Timer.Dispose();
                SendData_Timer = null;
            }
            if (Timeout_Timer != null)
            {
                Timeout_Timer.Dispose();
                Timeout_Timer = null;
            }
        }
        #endregion

        #region Disable_Timeout_Timer
        private void Disable_Timeout_Timer()
        {
            if (Timeout_Timer != null)
            {
                Timeout_Timer.Dispose();
                Timeout_Timer = null;
            }
        }
        #endregion

        #region Form1_FormClosed
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (SendData_Timer != null)
            {
                SendData_Timer.Dispose();
                SendData_Timer = null;
            }
            if (Timeout_Timer != null)
            {
                Timeout_Timer.Dispose();
                Timeout_Timer = null;
            }
            System.Environment.Exit(0);
        }
        #endregion
    }
}
