CREATE TABLE [dbo].[CompanyDetails] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [CompanyName]    VARCHAR (MAX) NULL,
    [IBAN]           VARCHAR (MAX) NULL,
    [Country]        VARCHAR (MAX) NULL,
    [Address]        VARCHAR (MAX) NULL,
    [ContactPerson]  VARCHAR (MAX) NULL,
    [Phone]          VARCHAR (MAX) NULL,
    [TIN]            VARCHAR (MAX) NULL,
    [EDRPOU]         VARCHAR (MAX) NULL,
    [AccountBalance] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

