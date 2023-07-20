USE [Inventory&SalesDb]
GO

---------/* SP for GETALL and GETBYID */----------

ALTER PROC sp_GetAll_GetById_Product 
	@productId INT = NULL
AS
BEGIN
	IF(@productId IS NOT NULL OR @productId > 0)
		SELECT * FROM tblProduct WHERE ProductId = @productId
	ELSE
		SELECT * FROM tblProduct
END
GO

-- GETALL
EXEC sp_GetAll_GetById_Product
GO

-- GETBYID
EXEC sp_GetAll_GetById_Product @productId = 3
GO
