CREATE TABLE [dbo].[Transaction] (
    [Id]                 INT        IDENTITY (1, 1) NOT NULL,
    [Transaction_id]     NCHAR (10) NOT NULL,
    [Card_id]            NCHAR (10) NOT NULL,
    [Transaction_type]   NCHAR (10) NOT NULL,
    [Recipient_account ] NCHAR (10) NOT NULL,
    [Amount]             NCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

