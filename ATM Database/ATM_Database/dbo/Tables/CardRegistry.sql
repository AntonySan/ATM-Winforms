CREATE TABLE [dbo].[CardRegistry] (
    [Id]                    INT           IDENTITY (1, 1) NOT NULL,
    [Card_Number]           VARCHAR (MAX) NULL,
    [Card_Type]             VARCHAR (MAX) NULL,
    [Issue_Date]            VARCHAR (MAX) NULL,
    [Expiration_Date]       VARCHAR (MAX) NULL,
    [Cardholder_Name]       VARCHAR (MAX) NULL,
    [Cardholder's_Address:] VARCHAR (MAX) NULL,
    [CVV/CVC ]              VARCHAR (MAX) NULL,
    [PIN]                   VARCHAR (MAX) NULL,
    [Account_Balance]       INT           NULL,
    [Card_Status]           VARCHAR (MAX) NULL,
    [Payment_System]        VARCHAR (MAX) NULL,
    [Spending_Limit]        VARCHAR (MAX) NULL,
    [Issuing_bank]          VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

