using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509.Qualified;
using System.Diagnostics;
using Task2.Models;
using Task2.Models.Entities;
using Task2.Models.ViewModels;
using Task2.Services;

namespace Task2.Controllers
{
    //[Route("{controller}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        
        private readonly ExcelService service;

        private ExcelParser parser = new ExcelParser();

        public HomeController(ILogger<HomeController> logger, ExcelService service)
        {
            _logger = logger;
            //��������� ������� ExcelService ��� ������������� �����������
            this.service = service;
        }

        //�������� ��������� ��������
        public IActionResult Index()
        {
            return View();
        }

        //����� �������� ��������� ������
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //�������� ����� Excel � ���� ������. IFormFile excelFile - ������ ������ � ����������� �����
        [HttpPost]
        public IActionResult UploadExcel(IFormFile excelFile)
        {
            IActionResult view;
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.Message = "Please choose a valid Excel file.";
                //������� �� �������� ������ �������
                view = GetStatements();
                return view;
            }
            // ��������� ���� � ���������� ����� �� ������� 
            var filePath = Path.GetTempFileName(); 

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                excelFile.CopyTo(stream); // ����������� ���������� ������������ �����
            }
            try
            {
                LoadInDatabase(filePath);
                ViewBag.Message = $"File uploaded successfully.";
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Unable to load file:\n{ex.Message}";
            }
            view = GetStatements();
            //������� �� �������� ������ �������
            return view;
        }
        //�������� ����� � ��, ��������� ���� � ����� filePath
        public void LoadInDatabase(string filePath)
        {
            //������ excel �����, ��������� ������������ ����� ������
            var head = parser.ReadHeadExcel(filePath);
            Bank bank = new Bank();
            bank.BankName = head["BankName"];
            Statement statement = new Statement();
            statement.StatementName = head["StatementName"];
            statement.CreationDate = Convert.ToDateTime(head["CreationDate"]);
            statement.InCurrency = head["CurrencyType"];

            //������ excel �����, ��������� ����� �������� ������
            var rows = parser.ReadRowsExcel(filePath);
            List<AccountClass> accountClasses = new List<AccountClass>();
            AccountClass accClass;
            foreach (var accountClass in rows.Keys)
            {
                accClass = new AccountClass();
                accClass.AccountClassName = accountClass;
                accountClasses.Add(accClass);
            }
            List<BankAccount> bankAccounts = new List<BankAccount>();
            BankAccount bankAccount;
            List<Turnover> turnovers = new List<Turnover>();
            Turnover turnover;
            foreach (var accountRows in rows.Values)
            {
                foreach (var accountRow in accountRows)
                {
                    bankAccount = new BankAccount();
                    bankAccount.AccountNumber = accountRow[0];
                    bankAccounts.Add(bankAccount);

                    turnover = new Turnover();
                    turnover.OpeningBalanceDebit = decimal.Parse(accountRow[1]);
                    turnover.OpeningBalanceCredit = decimal.Parse(accountRow[2]);
                    turnover.TurnoverDebit = decimal.Parse(accountRow[3]);
                    turnover.TurnoverCredit = decimal.Parse(accountRow[4]);
                    turnover.ClosingBalanceDebit = decimal.Parse(accountRow[5]);
                    turnover.ClosingBalanceCredit = decimal.Parse(accountRow[6]);
                    turnovers.Add(turnover);
                }
            }
            //�������� ������ ������������������ �������� ������ � ��
            service.LoadInDatabase(bank, statement, accountClasses, bankAccounts, turnovers);

        }
        //��������� ������ �� �� ��� ��������� ����������� ������. statementId - �������� ���������� ����� ������
        public IActionResult ReadFromDatabase(int statementId)
        {
            //��������� ���� ������ �� ������ � ������ statementId
            var data = service.ReturnFromDatabase(statementId);
            //���������� ������: �� �������� ����� (statement) � ������ (turnovers)
            Statement statement = data.First().Key;
            List<Turnover> turnovers= data.First().Value;
            //����������� ���������� ������� �� ������
            //� ��������� (�� ������ 2 ������ ������ �/��)
            var groupedTurnovers = turnovers.GroupBy(t => new
            {
                ClassName = t.BankAccount?.AccountClass?.AccountClassName ?? "Unknown",
                SubGroup = t.BankAccount?.AccountNumber?.Substring(0, 2) ?? "00"
            }).ToList();

            // �������� �������������� �������� �������� �� ������� � ����������, ������� ��� ������ �� ��������
            var turnoverGroups = groupedTurnovers
        .GroupBy(g => g.Key.ClassName) // ����������� �� ������
        .Select(classGroup => new TurnoverGroupViewModel
        {
            AccountClassName = classGroup.Key, // �������� ������
            SubGroups = classGroup
                .Select(subGroup => new TurnoverSubGroupViewModel
                {
                    SubGroup = subGroup.Key.SubGroup, // �������� ���������
                    Turnovers = subGroup.Select(t => new TurnoverViewModel
                    {
                        AccountNumber = t.BankAccount?.AccountNumber ?? "0000",
                        OpeningBalanceDebit = (decimal)t.OpeningBalanceDebit,
                        OpeningBalanceCredit = (decimal)t.OpeningBalanceCredit,
                        TurnoverDebit = (decimal)t.TurnoverDebit,
                        TurnoverCredit = (decimal)t.TurnoverCredit,
                        ClosingBalanceDebit = (decimal)t.ClosingBalanceDebit,
                        ClosingBalanceCredit = (decimal)t.ClosingBalanceCredit
                    }).ToList()
                }).ToList()
        })
        .ToList();

            // ������������ StatementViewModel, ������� ������ � Statement � TurnoverGroups
            var viewModel = new StatementViewModel
            {
                StatementID = statement.StatementId,
                StatementName = statement.StatementName,
                CreationDate = statement.CreationDate,
                InCurrency = statement.InCurrency,
                BankName = statement.Bank?.BankName ?? "Unknown",
                TurnoverGroups = turnoverGroups 
            };
            //����������� �������� ��������� ������� ������
            return View(viewModel);
        }
        //��������� ������ �������, ���������� � ��
        public IActionResult GetStatements()
        {
            List<Statement> statements = service.GetStatements();
            return View("Files",statements);
        }
    }
}
