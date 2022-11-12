CREATE PROCEDURE [dbo].[SP_INSERTAR_FACTURA] 
	@nro_factura int, 
	@fecha date,
	@id_forma_pago int,
	@cod_cliente int
AS
BEGIN
	INSERT INTO Facturas(nro_factura, fecha, id_forma_pago , cod_cliente)
    VALUES (@nro_factura, @fecha, @id_forma_pago, @cod_cliente );

END
GO

CREATE PROCEDURE [dbo].[SP_ELIMINAR_FACTURA]
	@nro_factura int
AS
BEGIN
	UPDATE Facturas SET fecha_baja = GETDATE()
	WHERE nro_factura = @nro_factura;
END
GO

CREATE PROCEDURE [dbo].[SP_MODIFICAR_FACTURA] 
	@nro_factura int, 
	@fecha date,
	@id_forma_pago int,
	@cod_cliente int
AS
BEGIN
	UPDATE Facturas SET fecha = @fecha, id_forma_pago = @id_forma_pago, cod_cliente = @cod_cliente
	WHERE nro_factura = @nro_factura;
	
	DELETE Detalles_facturas
	WHERE nro_factura = @nro_factura;
END
GO

CREATE PROCEDURE [dbo].[SP_CONSULTAR_FACTURAS]
	@fecha_desde Datetime,
	@fecha_hasta Datetime

AS
BEGIN
	SELECT * 
	FROM Facturas
	WHERE (@fecha_desde is null OR fecha >= @fecha_desde)
	AND (@fecha_hasta is null OR fecha <= @fecha_hasta)
	AND fecha_baja is null;
END
GO


CREATE PROCEDURE [dbo].[SP_INSERTAR_DETALLES] 
	@id_detalle_factura int,
	@nro_factura int,
	
	@id_vehiculo int,
	@id_autoparte int,

	@precio int,
	@cantidad int
AS
BEGIN
	
	IF @id_vehiculo = (NULL)
	
		INSERT INTO Detalles_facturas(id_detalle_factura, precio, cantidad, id_autoparte, nro_factura)
		VALUES (@id_detalle_factura, @precio, @cantidad, @id_autoparte, @nro_factura );
	
	
	ELSE
		INSERT INTO Detalles_facturas(id_detalle_factura, precio, cantidad, id_vehiculo, nro_factura)
		VALUES (@id_detalle_factura, @precio, @cantidad, @id_vehiculo, @nro_factura );
  
END
GO

CREATE PROCEDURE [dbo].[SP_CONSULTAR_DETALLE_FACTURA] 
	@nro_factura int
AS
BEGIN
	SELECT * FROM Detalles_facturas
END
GO

CREATE PROCEDURE [dbo].[SP_INSERTAR_VEHICULO] 
	@color varchar(20), 
	@año date,
	@precio_act int,
	@id_modelo int,
	@id_vehiculo int OUTPUT
AS
BEGIN
	INSERT INTO Vehiculos(precio_act, año, color, id_modelo)
    VALUES (@precio_act, @año, @color, (@id_modelo+1));
    --Asignamos el valor del último ID autogenerado (obtenido --  
    --mediante la función SCOPE_IDENTITY() de SQLServer)	
    SET @id_vehiculo = SCOPE_IDENTITY();

END
GO

CREATE PROCEDURE [dbo].[SP_MODIFICAR_VEHICULO] 
	@color varchar(20), 
	@año date,
	@precio_act int,
	@id_modelo int,
	@id_vehiculo int 
AS
BEGIN
	UPDATE Vehiculos SET color = @color, año = @año, precio_act = @precio_act, id_modelo = @id_modelo
	WHERE id_vehiculo = @id_vehiculo;
	
END
GO

CREATE PROCEDURE [dbo].[SP_CONSULTAR_VEHICULOS]
AS
BEGIN
	
	SELECT * from Vehiculos ORDER BY 2;
END
GO

CREATE PROCEDURE [dbo].[SP_CONSULTAR_AUTOPARTES]
AS
BEGIN
	
	SELECT * from Autopartes ORDER BY 2;
END
GO

create PROCEDURE [dbo].[SP_PROXIMO_ID]
@next int OUTPUT
AS
BEGIN
	SET @next = (SELECT count(nro_factura)+1  FROM Facturas);
END
GO

create PROCEDURE [dbo].[SP_CONSULTAR_TIPOCLIENTES]
AS
BEGIN
	SELECT id_tipo_cliente, tipo_cliente
	FROM Tipos_Clientes
END
GO

alter PROCEDURE [dbo].[SP_CONSULTAR_CLIENTES]
AS
BEGIN
	SELECT cod_cliente, nombre + ' '+ apellido as Nombre
	FROM Clientes
END
GO

select *
from Clientes