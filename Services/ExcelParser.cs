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
    //Класс по работе с excel файлами
    class ExcelParser
    {
        //Возврат строк оборотов из отчёта
        public Dictionary<string, List<string[]>> ReadRowsExcel(string filePath)
        {
            // Словарь для хранения данных по классам
            Dictionary<string, List<string[]>> classData;
            //Создание потока чтения из файла
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Открываем excel файл
                var workbook = WorkbookFactory.Create(fileStream);
                var sheet = workbook.GetSheetAt(0); // Получаем первый лист
                //Чтение из отчёта строк оборотов на листе excel 
                classData = ReadTurnovers(sheet);
            }
            return classData;
        }
        //Возврат головной части отчёта из excel файла
        public Dictionary<string, string> ReadHeadExcel(string filePath)
        {

            Dictionary<string, string> headData;
            //Открытие потока чтения из файла
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                // Открываем excel файл 
                var workbook = WorkbookFactory.Create(fileStream); 
                var sheet = workbook.GetSheetAt(0); // Получаем первый лист
                //чтение головной части из отчёта
                headData = ReadHead(sheet);
            }
            return headData;
        }
        //Чтение строк оборотв из отчёта
        Dictionary<string, List<string[]>> ReadTurnovers(ISheet sheet)
        {
            string currentClass = null;
            //Журнал значений оборотов по классам
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

                    // Проверка, не является ли первая ячейка б/сч
                    if (!string.IsNullOrWhiteSpace(firstCell) && !IsNumeric(firstCell) && !IsGeneral(firstCell))
                    {
                        currentClass = firstCell;
                        //Если такой класс не попадался, добавляем в список
                        if (!classData.ContainsKey(currentClass))
                        {
                            classData[currentClass] = new List<string[]>();
                        }
                    }
                    else if (currentClass != null)
                    {
                        // Чтение данных столбцов A-G 
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

        //чтение головной части из отчёта
        Dictionary<string, string> ReadHead(ISheet sheet)
        {
            //Словарь головой части. Ключ - суть записываемой информации
            Dictionary<string, string> head = new Dictionary<string, string>();
            //получение названия банка с первой строки
            var firstRow = sheet.GetRow(0);
            string bankName = firstRow.GetCell(0)?.ToString();
            //получение даты создания отчёта и валюты оборотов
            var dateRow = sheet.GetRow(5);
            DateTime date = Convert.ToDateTime(dateRow.GetCell(0).ToString());
            string inCurrency = dateRow.GetCell(6)?.ToString();

            head.Add("BankName", bankName);
            head.Add("CreationDate", date.ToString());
            head.Add("CurrencyType", inCurrency);
            //запись в слловарь полного названия отчёта
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
        //Проверка при чтении строк для пропуска итоговых значений по классу
        bool IsGeneral(string value)
        {
            return value.Contains("ПО КЛАССУ");
        }
    }
}