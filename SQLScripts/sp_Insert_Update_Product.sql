USE [Inventory&SalesDb]
GO

---------/* SP for INSERT and UPDATE */----------

CREATE PROC sp_Insert_Update_Product 
	@productId INT = NULL,
	@productName NVARCHAR(50) = NULL,
	@description NVARCHAR(200) = 'No Details Available',
	@price MONEY = NULL,
	@quantity INT = NULL,
	@isAvailable BIT = NULL,
	@isDeleted BIT = 0

AS
BEGIN
	-- INSERT
	IF(@productId IS NULL OR @productId = 0)
	BEGIN
		INSERT INTO tblProduct (ProductName, Description, Price, Quantity, IsAvailable)
		VALUES (@productName, @description, @price, @quantity, @isAvailable)
	END

	--UPDATE
	ELSE IF(@productId IS NOT NULL OR @productId > 0)
	BEGIN
		UPDATE tblProduct 
		SET 
			ProductName = ISNULL(@productName, ProductName),
			Description = ISNULL(@description, Description),
			Price = ISNULL(@price, Price),
			Quantity = ISNULL(@quantity, Quantity),
			IsAvailable = ISNULL(@isAvailable, IsAvailable),
			isDeleted = ISNULL(@isDeleted, isDeleted)
		WHERE 
			ProductId = @productId
	END

END
GO

--INSERT
EXEC sp_Insert_Update_Product @productName = 'Product05',
							  @description = '',
							  @price = 2500,
							  @quantity = 20,
							  @isAvailable = 1
GO

--UPDATE
EXEC sp_Insert_Update_Product @productId = 5,
							  @productName = 'Product 05',
							  @description = null,
							  @price = 35500,
							  @quantity = 35,
							  @isAvailable = 1
GO

--DELETE 
EXEC sp_Insert_Update_Product @productId = 16, @isDeleted = 1
