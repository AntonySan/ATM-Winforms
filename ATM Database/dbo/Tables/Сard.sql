CREATE TABLE [dbo].[Сard] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [User_id]     NCHAR (10) NOT NULL,
    [Card_number] NCHAR (10) NOT NULL,
    [Pin_code]    NCHAR (10) NOT NULL,
    [Balance]     NCHAR (10) NOT NULL,
    [Card_id]     NCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

