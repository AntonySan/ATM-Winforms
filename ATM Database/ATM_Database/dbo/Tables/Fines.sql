CREATE TABLE [dbo].[Fines] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [license_plates] VARCHAR (MAX) NULL,
    [fine]           VARCHAR (MAX) NULL,
    [date]           VARCHAR (MAX) NULL,
    [fine_amount]    INT           NULL,
    [paid]           VARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

