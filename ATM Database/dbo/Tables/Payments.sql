CREATE TABLE [dbo].[Payments] (
    [Id]                INT        IDENTITY (1, 1) NOT NULL,
    [Payment_id ]       NCHAR (10) NOT NULL,
    [User_id]           NCHAR (10) NOT NULL,
    [Payment_type]      NCHAR (10) NOT NULL,
    [Amount]            NCHAR (10) NOT NULL,
    [Recipient_account] NCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

