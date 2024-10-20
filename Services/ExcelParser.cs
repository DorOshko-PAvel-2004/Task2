using NPOI.HSSF.UserModel; // Для .xls файлов
using NPOI.SS.UserModel;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NPOI.XSSF.UserModel;

namespace Task2.Services
{
    class ExcelParser
    {
        public Dictionary<string, List<string[]>> ReadRowsExcel(string filePath)
        {
            // Словарь для хранения данных по классам
            Dictionary<string, List<string[]>> classData;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Открываем файл .xls
                var workbook = WorkbookFactory.Create(fileStream);
                var sheet = workbook.GetSheetAt(0); // Получаем первый лист
                                                    //ReadHead(sheet);
                classData = ReadTurnovers(sheet);
            }
            return classData;
            /*// Пример вывода данных
            foreach (var classEntry in classData)
            {
                Console.WriteLine($"Класс: {classEntry.Key}");
                foreach (var row in classEntry.Value)
                {
                    Console.WriteLine($"Счёт банка: {row[0]}, Входящее сальдо (Актив): {row[1]}, Входящее сальдо (Пассив): {row[2]}, " +
                                      $"Обороты (Дебет): {row[3]}, Обороты (Кредит): {row[4]}, Исходящее сальдо (Актив): {row[5]}, Исходящее сальдо (Пассив): {row[6]}");
                }
                Console.WriteLine("------------------------------------------------");
            }*/
        }
        public Dictionary<string, string> ReadHeadExcel(string filePath)
        {

            Dictionary<string, string> headData;
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Открываем файл .xls
                var workbook = WorkbookFactory.Create(fileStream); 
                var sheet = workbook.GetSheetAt(0); // Получаем первый лист
                headData = ReadHead(sheet);
            }
            return headData;
        }

        Dictionary<string, List<string[]>> ReadTurnovers(ISheet sheet)
        {
            string currentClass = null;
            Dictionary<string, List<string[]>> classData = new Dictionary<string, List<string[]>>();
            // Начало данных
            int startRow = 7; // Строка начала данных
            int endRow = sheet.LastRowNum - 1; // Последняя строка, без итогового баланса

            for (int row = startRow; row < endRow; row++)
            {
                var currentRow = sheet.GetRow(row);

                if (currentRow != null)
                {
                    string firstCell = currentRow.GetCell(0)?.ToString();

                    // Проверка, является ли первая ячейка названием класса
                    if (!string.IsNullOrWhiteSpace(firstCell) && !IsNumeric(firstCell) && !IsGeneral(firstCell))
                    {
                        currentClass = firstCell;
                        if (!classData.ContainsKey(currentClass))
                        {
                            classData[currentClass] = new List<string[]>();
                        }
                    }
                    else if (currentClass != null)
                    {
                        // Читаем данные столбцов A-G
                        if (currentRow.GetCell(0)?.ToString().Count() == 4)
                        {
                            string[] rowData = new string[7];
                            for (int col = 0; col < 7; col++)
                            {
                                rowData[col] = currentRow.GetCell(col)?.ToString() ?? "";
                            }
                            // Добавляем данные в список текущего класса
                            classData[currentClass].Add(rowData);
                        }
                    }
                }
            }

            return classData;
        }

        Dictionary<string, string> ReadHead(ISheet sheet)
        {
            Dictionary<string, string> head = new Dictionary<string, string>();
            var firstRow = sheet.GetRow(0);
            string bankName = firstRow.GetCell(0)?.ToString();
            var dateRow = sheet.GetRow(5);
            DateTime date = Convert.ToDateTime(dateRow.GetCell(0).ToString());
            string inCurrency = dateRow.GetCell(6)?.ToString();

            head.Add("BankName", bankName);
            head.Add("CreationDate", date.ToString());
            head.Add("CurrencyType", inCurrency);
            string name = "";
            for (int i = 1; i < 5; i++)
            {
                var row = sheet.GetRow(i);
                name += row.GetCell(0)?.ToString() + " ";
            }
            head.Add("StatementName", name);
            return head;
        }

        // Метод для проверки, является ли строка числом
        bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }

        bool IsGeneral(string value)
        {
            return value.Contains("ПО КЛАССУ");
        }
    }
}