CREATE TABLE [dbo].[Users] (
    [Id]            INT           NOT NULL,
    [Full_name]     VARCHAR (MAX) NOT NULL,
    [Password_hash] VARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

