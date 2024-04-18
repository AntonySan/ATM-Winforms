CREATE TABLE [dbo].[ATM_info] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Card_number] VARCHAR (MAX) NOT NULL,
    [PIN]         VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

