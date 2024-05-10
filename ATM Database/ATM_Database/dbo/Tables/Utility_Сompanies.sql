CREATE TABLE [dbo].[Utility_Сompanies] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Compani_name] VARCHAR (MAX) NULL,
    [Bills]        VARCHAR (MAX) NULL,
    [Amount]       INT           NULL,
    [Address]      VARCHAR (MAX) NULL,
    [tariff]       VARCHAR (MAX) NULL,
    [used]         VARCHAR (MAX) NULL,
    [paid]         VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

