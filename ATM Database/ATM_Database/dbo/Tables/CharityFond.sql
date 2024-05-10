CREATE TABLE [dbo].[CharityFond] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [FondName]           VARCHAR (MAX)   NULL,
    [RegistrationNumber] VARCHAR (MAX)   NULL,
    [Country]            VARCHAR (MAX)   NULL,
    [Address]            VARCHAR (MAX)   NULL,
    [ContactPerson]      VARCHAR (MAX)   NULL,
    [Phone]              VARCHAR (MAX)   NULL,
    [Email]              VARCHAR (MAX)   NULL,
    [BankAccount]        VARCHAR (MAX)   NULL,
    [AccountBalance]     DECIMAL (18, 2) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

