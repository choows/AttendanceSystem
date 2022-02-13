using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Functions
{
    public class Logging
    {
        public void WriteLog(string LoggingMessage , [CallerMemberName] string MethodName = "")
        {
            try
            {
                DateTime currentdatetime = DateTime.Now;
                string today_date = currentdatetime.Year.ToString() + "-" + currentdatetime.Month.ToString() + "-" + currentdatetime.Day.ToString();
                string filename = today_date + ".txt";
                string folder_name = "\\App_Data\\Logs";
                string direc = Environment.CurrentDirectory + folder_name +"\\" +filename;
                System.IO.Directory.CreateDirectory(Environment.CurrentDirectory + folder_name);
                using (StreamWriter writer = System.IO.File.AppendText(direc))
                {
                    string content = today_date + ":" + currentdatetime.Hour.ToString() + ":" + currentdatetime.Minute.ToString() + ":" + currentdatetime.Second.ToString() + "." + currentdatetime.Millisecond.ToString() + "\t"+ MethodName + "\t" + LoggingMessage;
                    writer.WriteLine(content);
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine("Error writing log file : " + exp);
            }
        }
    }
}
