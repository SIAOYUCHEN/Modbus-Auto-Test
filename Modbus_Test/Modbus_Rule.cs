using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modbus_Test
{

    class Modbus_Rule
    {
        public int Modbus_Id_Index = 0;
        public int Modbus_FunctionCode_Index = 1;
        public int Modbus_Quantity_Index = 5;
        public int Modbus_Modbus_FunctionCode16_Quantity_Index = 6;
        public int Modbus_BroadC0ast = 0;
        public int Modbus_FuctionCode3 = 3;
        public int Modbus_FuctionCode4 = 4;
        public int Modbus_FuctionCode6 = 6;
        public int Modbus_FuctionCode16 = 16;
        public int Modbus_Error_Length = 5;
        public int Modbus_Timeout = 34;// 100ms 34

        public int Temp_Receive_Buffer_Crc_Offest1 = 1;
        public int Temp_Receive_Buffer_Crc_Offest2 = 2;
        public int Temp_Receive_Buffer_Offest = 2;

        private const int FuctionCode3 = 3;
        private const int FuctionCode4 = 4;
        private const int FuctionCode6 = 6;
        private const int FuctionCode16 = 16;

        private int crc_high = 0, crc_low = 0;

        private byte[] Modbus_Reference_FunctionCode = {0x03, 0x04, 0x06, 0x10 };
        private byte[] Modbus_Reference_FunctionCode4_Address = { 0x00, 0x01, 0x02, 0x03, 0x0A,
                                                                  0x0C, 0x0D, 0x0E, 0x0F, 0x10,
                                                                  0x11, 0x21, 0x02
                                                                };
        private byte[] Modbus_Reference_FunctionCode3_Address = { 0x01, 0x02, 0x1E, 0x23, 0x28,
                                                                  0x29, 0x2A, 0x2D, 0x2E, 0x42
                                                                };
        private byte[] Modbus_Reference_FunctionCode6_Address = { 0x2A, 0x42 };

        public Byte[] GetTransmit()
        {
            Byte[] context = new Byte[1024];
            Random data1 = new Random();
            context[0] = Convert.ToByte(data1.Next(0,1));
            if (context[0] != Modbus_BroadC0ast)
            {
                context[1] = Modbus_Reference_FunctionCode[data1.Next(0, Modbus_Reference_FunctionCode.Length)];
                if (context[Modbus_FunctionCode_Index] == FuctionCode3)
                {
                    context[2] = 0x00;
                    context[3] = Modbus_Reference_FunctionCode3_Address[data1.Next(0, Modbus_Reference_FunctionCode3_Address.Length)];
                    context[4] = 0x00;
                    context[5] = Convert.ToByte(data1.Next(0, 6));
                    SetCrc(ref context, 6);
                    context[6] = Convert.ToByte(crc_high);
                    context[7] = Convert.ToByte(crc_low);
                    Array.Resize(ref context, 8);// 傳入矩陣位址
                }
                else if (context[Modbus_FunctionCode_Index] == FuctionCode4)
                {
                    context[2] = 0x00;
                    context[3] = Modbus_Reference_FunctionCode4_Address[data1.Next(0, Modbus_Reference_FunctionCode4_Address.Length)];
                    context[4] = 0x00;
                    context[5] = Convert.ToByte(data1.Next(0, 6));
                    SetCrc(ref context, 6);
                    context[6] = Convert.ToByte(crc_high);
                    context[7] = Convert.ToByte(crc_low);
                    Array.Resize(ref context, 8);// 傳入矩陣位址
                }
                else if (context[Modbus_FunctionCode_Index] == FuctionCode6)
                {
                    context[2] = 0x00;
                    context[3] = Modbus_Reference_FunctionCode6_Address[data1.Next(0, Modbus_Reference_FunctionCode6_Address.Length)];
                    context[4] = 0x00;
                    if (context[3] == Modbus_Reference_FunctionCode6_Address[0])
                    {
                        context[5] = Convert.ToByte(data1.Next(0, 1));
                    }
                    else if (context[3] == Modbus_Reference_FunctionCode6_Address[1])
                    {
                        context[5] = Convert.ToByte(data1.Next(100, 255));
                    }
                    SetCrc(ref context, 6);
                    context[6] = Convert.ToByte(crc_high);
                    context[7] = Convert.ToByte(crc_low);
                    Array.Resize(ref context, 8);// 傳入矩陣位址
                }
                else if (context[Modbus_FunctionCode_Index] == FuctionCode16)
                {
                    int temp = data1.Next(0, 100);
                    int temp_length = data1.Next(2, 5);
                    if (temp_length == 2)
                    {
                        context[2] = 0x00;
                        context[3] = Convert.ToByte(temp);
                        context[4] = 0x00;
                        context[5] = Convert.ToByte(temp_length);
                        context[6] = Convert.ToByte(temp_length * 2);
                        context[7] = 0x00;
                        context[8] = Convert.ToByte(data1.Next(100, 255));
                        context[9] = 0x00;
                        context[10] = Convert.ToByte(data1.Next(100, 255));
                        SetCrc(ref context, 11);
                        context[11] = Convert.ToByte(crc_high);
                        context[12] = Convert.ToByte(crc_low);
                        Array.Resize(ref context, 13);// 傳入矩陣位址
                    }
                    else if (temp_length == 3)
                    {
                        context[2] = 0x00;
                        context[3] = Convert.ToByte(temp);
                        context[4] = 0x00;
                        context[5] = Convert.ToByte(temp_length);
                        context[6] = Convert.ToByte(temp_length * 2);
                        context[7] = 0x00;
                        context[8] = Convert.ToByte(data1.Next(100, 255));
                        context[9] = 0x00;
                        context[10] = Convert.ToByte(data1.Next(100, 255));
                        context[11] = 0x00;
                        context[12] = Convert.ToByte(data1.Next(100, 255));
                        SetCrc(ref context, 13);
                        context[13] = Convert.ToByte(crc_high);
                        context[14] = Convert.ToByte(crc_low);
                        Array.Resize(ref context, 15);// 傳入矩陣位址
                    }
                    else if (temp_length == 4)
                    {
                        context[2] = 0x00;
                        context[3] = Convert.ToByte(temp);
                        context[4] = 0x00;
                        context[5] = Convert.ToByte(temp_length);
                        context[6] = Convert.ToByte(temp_length * 2);
                        context[7] = 0x00;
                        context[8] = Convert.ToByte(data1.Next(100, 255));
                        context[9] = 0x00;
                        context[10] = Convert.ToByte(data1.Next(100, 255));
                        context[11] = 0x00;
                        context[12] = Convert.ToByte(data1.Next(100, 255));
                        context[13] = 0x00;
                        context[14] = Convert.ToByte(data1.Next(100, 255));
                        SetCrc(ref context, 15);
                        context[15] = Convert.ToByte(crc_high);
                        context[16] = Convert.ToByte(crc_low);
                        Array.Resize(ref context, 17);// 傳入矩陣位址
                    }
                }
            }
            else
            {
                context[1] = Modbus_Reference_FunctionCode[data1.Next(2, 4)];
                if (context[Modbus_FunctionCode_Index] == FuctionCode6)
                {
                    context[2] = 0x00;
                    context[3] = Modbus_Reference_FunctionCode6_Address[data1.Next(0, Modbus_Reference_FunctionCode6_Address.Length)];
                    context[4] = 0x00;
                    if (context[3] == Modbus_Reference_FunctionCode6_Address[0])
                    {
                        context[5] = Convert.ToByte(data1.Next(0, 1));
                    }
                    else if (context[3] == Modbus_Reference_FunctionCode6_Address[1])
                    {
                        context[5] = Convert.ToByte(data1.Next(100, 255));
                    }
                    SetCrc(ref context, 6);
                    context[6] = Convert.ToByte(crc_high);
                    context[7] = Convert.ToByte(crc_low);
                    Array.Resize(ref context, 8);// 傳入矩陣位址
                }
                else if (context[Modbus_FunctionCode_Index] == FuctionCode16)
                {
                    int temp = data1.Next(0, 100);
                    int temp_length = data1.Next(2, 5);
                    if (temp_length == 2)
                    {
                        context[2] = 0x00;
                        context[3] = Convert.ToByte(temp);
                        context[4] = 0x00;
                        context[5] = Convert.ToByte(temp_length);
                        context[6] = Convert.ToByte(temp_length * 2);
                        context[7] = 0x00;
                        context[8] = Convert.ToByte(data1.Next(100, 255));
                        context[9] = 0x00;
                        context[10] = Convert.ToByte(data1.Next(100, 255));
                        SetCrc(ref context, 11);
                        context[11] = Convert.ToByte(crc_high);
                        context[12] = Convert.ToByte(crc_low);
                        Array.Resize(ref context, 13);// 傳入矩陣位址
                    }
                    else if (temp_length == 3)
                    {
                        context[2] = 0x00;
                        context[3] = Convert.ToByte(temp);
                        context[4] = 0x00;
                        context[5] = Convert.ToByte(temp_length);
                        context[6] = Convert.ToByte(temp_length * 2);
                        context[7] = 0x00;
                        context[8] = Convert.ToByte(data1.Next(100, 255));
                        context[9] = 0x00;
                        context[10] = Convert.ToByte(data1.Next(100, 255));
                        context[11] = 0x00;
                        context[12] = Convert.ToByte(data1.Next(100, 255));
                        SetCrc(ref context, 13);
                        context[13] = Convert.ToByte(crc_high);
                        context[14] = Convert.ToByte(crc_low);
                        Array.Resize(ref context, 15);// 傳入矩陣位址
                    }
                    else if (temp_length == 4)
                    {
                        context[2] = 0x00;
                        context[3] = Convert.ToByte(temp);
                        context[4] = 0x00;
                        context[5] = Convert.ToByte(temp_length);
                        context[6] = Convert.ToByte(temp_length * 2);
                        context[7] = 0x00;
                        context[8] = Convert.ToByte(data1.Next(100, 255));
                        context[9] = 0x00;
                        context[10] = Convert.ToByte(data1.Next(100, 255));
                        context[11] = 0x00;
                        context[12] = Convert.ToByte(data1.Next(100, 255));
                        context[13] = 0x00;
                        context[14] = Convert.ToByte(data1.Next(100, 255));
                        SetCrc(ref context, 15);
                        context[15] = Convert.ToByte(crc_high);
                        context[16] = Convert.ToByte(crc_low);
                        Array.Resize(ref context, 17);// 傳入矩陣位址
                    }
                }
            }
            return context;
        }

        public int GetResponseLength(int functioncode, int quantity)
        {
            int response_length = 0;
            switch(functioncode)
            {
                case FuctionCode3:
                case FuctionCode4:
                    response_length = quantity * 2 + 5;
                break;
                case FuctionCode6:
                    response_length = quantity;
                break;
                case FuctionCode16:
                    response_length = quantity + 4;
                break;
            }
            return response_length;
        }

        public bool GetCrcEqual(ref Byte[] receive_num, int length, Byte num_high, Byte num_low)
        {
            int crc = 0xFFFF;
            //int crc_high = 0, crc_low = 0;
            int lsb;
            for(int i = 0; i<length; i++)
            {
                crc ^= receive_num[i];
                for (int j = 0; j<8; j++)
                {
                    lsb = crc & 0x0001;
                    crc = crc >> 1;
                    if (lsb != 0)
                    {
                        crc ^= 0xA001;
                    }
                }   
            }
            crc_low = (crc & 0xFF00) >> 8;
            crc_high = crc & 0x00FF;
            if (num_high == crc_high && num_low == crc_low)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetCrc(ref Byte[] receive_num, int length)
        {
            int crc = 0xFFFF;
            //int crc_high = 0, crc_low = 0;
            int lsb;
            for (int i = 0; i < length; i++)
            {
                crc ^= receive_num[i];
                for (int j = 0; j < 8; j++)
                {
                    lsb = crc & 0x0001;
                    crc = crc >> 1;
                    if (lsb != 0)
                    {
                        crc ^= 0xA001;
                    }
                }
            }
            crc_low = (crc & 0xFF00) >> 8;
            crc_high = crc & 0x00FF;
        }
    }
}
