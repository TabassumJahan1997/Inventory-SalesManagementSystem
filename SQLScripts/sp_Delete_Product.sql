USE [Inventory&SalesDb]
GO

---------/* SP for DELETE */----------

CREATE PROC sp_Delete_Product @productId INT
AS
BEGIN
	DELETE FROM tblProduct
	WHERE ProductId = @productId
END
GO

EXEC sp_Delete_Product @productId = 4
GO