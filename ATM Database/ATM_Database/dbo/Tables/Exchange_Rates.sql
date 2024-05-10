CREATE TABLE [dbo].[Exchange_Rates] (
    [Id]              INT        IDENTITY (1, 1) NOT NULL,
    [Date]            NCHAR (10) NULL,
    [Bank]            NCHAR (10) NULL,
    [baseCurrency]    NCHAR (10) NULL,
    [baseCurrencyLit] NCHAR (10) NULL,
    [exchangeRate]    NCHAR (10) NULL,
    [currency]        NCHAR (10) NULL,
    [SaleRateNB]      NCHAR (10) NULL,
    [PurchaseRateNB]  NCHAR (10) NULL,
    [SaleRate]        NCHAR (10) NULL,
    [PurchaseRate]    NCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

