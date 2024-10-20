
CREATE TABLE Bank (
    BankID INT PRIMARY KEY IDENTITY(1,1),
    BankName NVARCHAR(100) NOT NULL
);

create table AccountClass(
AccountClassID int primary key identity(1,1),
AccountClassName varchar(100)
);

create table [Statement](
StatementID int Primary key identity(1,1),
StatementName NVARCHAR(150),
CreationDate datetime not null,
InCurrency NVARCHAR(20) not null,
BankID int not null,
FOREIGN KEY (BankID) REFERENCES Bank(BankID)
);

-- Таблица банковских счетов с составным ключом
CREATE TABLE BankAccount (
	BankAccountID int Primary key identity(1,1),
    AccountNumber NVARCHAR(4) not null,
	BankID int not null,
	AccountClassID int not null,
	FOREIGN KEY (BankID) REFERENCES Bank(BankID),
	constraint BancAccountBank unique (AccountNumber,BankID),
	Foreign key (AccountClassID) references AccountClass(AccountClassID)
);

-- Таблица оборотов по счетам с внешними ключами на банковский счет
CREATE TABLE Turnover (
    TurnoverID INT PRIMARY KEY IDENTITY(1,1),
    BankAccountID int NOT NULL,
    StatementID INT NOT NULL,
    OpeningBalanceDebit DECIMAL(18, 2),
    OpeningBalanceCredit DECIMAL(18, 2),
    TurnoverDebit DECIMAL(18, 2),
    TurnoverCredit DECIMAL(18, 2),
    ClosingBalanceDebit AS (OpeningBalanceDebit + TurnoverDebit - TurnoverCredit) PERSISTED,
    ClosingBalanceCredit AS (OpeningBalanceCredit + TurnoverCredit - TurnoverDebit) PERSISTED,
    FOREIGN KEY (BankAccountID) REFERENCES BankAccount(BankAccountID),
    FOREIGN KEY (StatementID) REFERENCES [Statement](StatementID)
);
-- Пересмотреть про подсчёт итогового сальдо, пока какой-то бред