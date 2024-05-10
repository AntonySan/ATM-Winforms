CREATE TABLE [dbo].[Internet] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [Account_number]  VARCHAR (MAX) NULL,
    [Address]         VARCHAR (MAX) NULL,
    [Transfer_Amount] INT           NULL,
    [Paid]            INT           NULL,
    [Tariff_Plan]     VARCHAR (MAX) NULL,
    [Payment_Date]    VARCHAR (MAX) NULL,
    [Service_Status]  VARCHAR (MAX) NULL,
    [Data_Usage]      VARCHAR (MAX) NULL,
    [User_Name]       VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

