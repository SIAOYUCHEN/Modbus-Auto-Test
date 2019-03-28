using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modbus_Test
{
    class Csv_File
    {
        private DateTime LastTime = DateTime.Now;
        public DataTable dt = new DataTable();

        public void SaveCsv(DataTable dt, string filepath)
        {
            try
            {
                string data = "";
                FileInfo fi = new FileInfo(filepath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                StreamWriter wr = new StreamWriter(filepath, false, System.Text.Encoding.Default);
                foreach (DataColumn column in dt.Columns)
                {
                    data += column.ColumnName + ",";
                }
                data += "\n";
                wr.Write(data);
                data = "";

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        data += row[column].ToString().Trim() + ",";
                    }
                    data += "\n";
                    wr.Write(data);
                    data = "";
                }
                data += "\n";

                wr.Dispose();
                wr.Close();
            }
            catch
            {

            }
        }

        private string GetYear()
        {
            return LastTime.Year.ToString();
        }

        private string GetWeek()
        {
            return LastTime.Date.Month.ToString();
        }

        private string GetDay()
        {
            return LastTime.Date.Day.ToString();
        }

        public string GetFileName()
        {
            TimeSpan span = DateTime.Now - LastTime;
            if (span.TotalHours > 6)
            {
                LastTime = DateTime.Now;
                dt.Clear();
                return "D:/data/" + DateTime.Now.ToString("yyyy_MM_dd_HH") + ".csv";
            }
            else
            {
                return "D:/data/" + LastTime.ToString("yyyy_MM_dd_HH") + ".csv";
            }
        }
    }
}
