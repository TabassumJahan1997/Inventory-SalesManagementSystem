CREATE FUNCTION func_Calculate_TotalProductOrderPrice(@unitPrice MONEY ,
													  @quantity INT)
RETURNS MONEY
BEGIN
	DECLARE @totalPrice MONEY

	SET @totalPrice = @unitPrice*@quantity

	RETURN @totalPrice
END
GO

DECLARE @price MONEY
SET @price = dbo.func_Calculate_TotalProductOrderPrice(253.23, 5)
PRINT 'Total Price : '+ CAST(@price AS NVARCHAR(10))
GO