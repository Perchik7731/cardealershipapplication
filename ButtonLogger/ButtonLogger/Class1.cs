using System;
using System.IO;

namespace ButtonLogger
{
    public class LoggerForButton
    {
        private string _logFilePath;

        public LoggerForButton(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void LogButtonClick()
        {
            string logMessage = $"{DateTime.Now}: Кнопка нажата.";
            WriteToLogFile(logMessage);
        }

        private void WriteToLogFile(string logMessage)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
            }
        }
    }
}
