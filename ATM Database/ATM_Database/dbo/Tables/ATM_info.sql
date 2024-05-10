CREATE TABLE [dbo].[ATM_info] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Card_number]     VARCHAR (MAX) NOT NULL,
    [PIN]             VARCHAR (MAX) NOT NULL,
    [Full_name]       VARCHAR (MAX) NULL,
    [Expiration_date] VARCHAR (MAX) NULL,
    [Payment_system]  VARCHAR (MAX) NULL,
    [balance]         INT           NULL,
    [address]         VARCHAR (MAX) NULL,
    [Issue_Date]      VARCHAR (MAX) NULL,
    [CVV/CVC ]        VARCHAR (MAX) NULL,
    [Card_Status]     VARCHAR (MAX) NULL,
    [Spending_Limit]  VARCHAR (MAX) NULL,
    [Issuing_bank]    VARCHAR (MAX) NULL,
    [Card_Type]       VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

