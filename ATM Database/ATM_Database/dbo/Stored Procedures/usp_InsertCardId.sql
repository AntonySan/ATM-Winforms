CREATE PROCEDURE usp_InsertCardId
AS
BEGIN
    DECLARE @CardId INT;
    SELECT @CardId = id FROM Сard;

    INSERT INTO Transaction1 (Card_id) VALUES (@CardId);
END;