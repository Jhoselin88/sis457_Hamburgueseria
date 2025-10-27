-- Usar LabHamburgueseria como la base de datos principal.
-- Si prefieres LabBenditaBurger, cambia el nombre en CREATE DATABASE y en USE.
IF DB_ID('LabHamburgueseria') IS NULL
BEGIN
    CREATE DATABASE LabHamburgueseria;
END
GO

USE [LabHamburgueseria];
GO

drop database LabHamburgueseria;

-- Crear login y usuario sólo si no existen
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'usrhambu')
BEGIN
    CREATE LOGIN [usrhambu] WITH PASSWORD = N'123456',
        DEFAULT_DATABASE = [LabHamburgueseria],
        CHECK_EXPIRATION = OFF,
        CHECK_POLICY = ON;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'usrhambu')
BEGIN
    CREATE USER [usrhambu] FOR LOGIN [usrhambu];
    ALTER ROLE [db_owner] ADD MEMBER [usrhambu];
END
GO

-- Eliminar tablas en orden correcto (dependencias FK)
DROP TABLE IF EXISTS VentaDetalle;
DROP TABLE IF EXISTS Venta;
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS Categoria;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Cliente;
GO

-- Crear tablas
CREATE TABLE Cliente (
    id INT IDENTITY(1,1) PRIMARY KEY,
    documento VARCHAR(20) NOT NULL,
    nombreCompleto VARCHAR(100) NOT NULL,
    correo VARCHAR(100) NULL,
    telefono VARCHAR(15) NULL
);

CREATE TABLE Empleado (
    idEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    cedulaIdentidad VARCHAR(12) NOT NULL,
    nombres VARCHAR(30) NOT NULL,
    primerApellido VARCHAR(30) NULL,
    segundoApellido VARCHAR(30) NULL,
    direccion VARCHAR(250) NOT NULL,
    celular BIGINT NOT NULL,
    cargo VARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    usuario VARCHAR(50) NOT NULL,
    clave VARCHAR(250) NOT NULL
);

CREATE TABLE Categoria (
    IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
    descripcion VARCHAR(100) NOT NULL
);

CREATE TABLE Producto (
    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(20) NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    IdCategoria INT NOT NULL,
    Stock DECIMAL(10,2) NOT NULL DEFAULT 0,
    PrecioVenta DECIMAL(10,2) NOT NULL CHECK (PrecioVenta > 0),
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (IdCategoria) REFERENCES Categoria(IdCategoria),
    CONSTRAINT UQ_Producto_Codigo UNIQUE (Codigo)
);

-- Tabla Venta: agregué IdEmpleado por si quieres rastrear qué empleado atendió.
CREATE TABLE Venta (
    IdVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    IdEmpleado INT NULL, -- NULL si no aplica
    TipoDocumento VARCHAR(20) NOT NULL,
    DocumentoCliente VARCHAR(20) NOT NULL,
    NombreCliente VARCHAR(100) NOT NULL,
    MontoPago DECIMAL(18,2) NOT NULL CHECK (MontoPago >= 0),
    MontoCambio DECIMAL(18,2) NOT NULL CHECK (MontoCambio >= 0),
    MontoTotal DECIMAL(18,2) NOT NULL CHECK (MontoTotal > 0),
    CONSTRAINT FK_Venta_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario),
    CONSTRAINT FK_Venta_Empleado FOREIGN KEY (IdEmpleado) REFERENCES Empleado(idEmpleado)
);

CREATE TABLE VentaDetalle (
    IdDetalleVenta INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT NOT NULL,
    IdProducto INT NOT NULL,
    PrecioVenta DECIMAL(18,2) NOT NULL CHECK (PrecioVenta > 0),
    Cantidad DECIMAL(10,2) NOT NULL CHECK (Cantidad > 0),
    SubTotal DECIMAL(18,2) NOT NULL CHECK (SubTotal > 0),
    CONSTRAINT FK_VentaDetalle_Venta FOREIGN KEY (IdVenta) REFERENCES Venta(IdVenta),
    CONSTRAINT FK_VentaDetalle_Producto FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto)
);

-- Campos de auditoría y estado (valores por defecto)
ALTER TABLE Cliente ADD UsuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE Cliente ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Cliente ADD estado SMALLINT NOT NULL DEFAULT 1;

ALTER TABLE Usuario ADD UsuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE Usuario ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Usuario ADD estado SMALLINT NOT NULL DEFAULT 1;

ALTER TABLE Empleado ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE Empleado ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Empleado ADD estado SMALLINT NOT NULL DEFAULT 1;

ALTER TABLE Categoria ADD UsuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE Categoria ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Categoria ADD estado SMALLINT NOT NULL DEFAULT 1;

ALTER TABLE Producto ADD UsuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE Producto ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Producto ADD estado SMALLINT NOT NULL DEFAULT 1;

ALTER TABLE Venta ADD UsuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE Venta ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Venta ADD estado SMALLINT NOT NULL DEFAULT 1;

ALTER TABLE VentaDetalle ADD UsuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_SNAME();
ALTER TABLE VentaDetalle ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE VentaDetalle ADD estado SMALLINT NOT NULL DEFAULT 1;
GO

-- PROCEDIMIENTOS

-- paCategoriaListar
CREATE OR ALTER PROC paCategoriaListar @parametro VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * 
    FROM Categoria
    WHERE estado <> -1 
      AND descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%';
END
GO

-- paProductoListar (soporta búsqueda por Código exacto o por Nombre)
CREATE OR ALTER PROCEDURE paProductoListar
    @parametro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1 
        FROM Producto
        WHERE estado != -1 AND Codigo = @parametro
    )
    BEGIN
        SELECT * 
        FROM Producto
        WHERE estado != -1 AND Codigo = @parametro;
    END
    ELSE
    BEGIN
        SELECT * 
        FROM Producto
        WHERE estado != -1 AND Nombre LIKE '%' + @parametro + '%';
    END
END
GO

-- paClienteListar
CREATE OR ALTER PROC paClienteListar
    @parametro VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT * 
    FROM Cliente
    WHERE estado <> -1 
      AND (documento LIKE '%' + REPLACE(@parametro, ' ', '%') + '%' 
           OR nombreCompleto LIKE '%' + REPLACE(@parametro, ' ', '%') + '%');
END
GO

-- paEmpleadoListar
CREATE OR ALTER PROC paEmpleadoListar
    @parametro VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Empleado
    WHERE cedulaIdentidad LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
       OR nombres LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
       OR primerApellido LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
       OR segundoApellido LIKE '%' + REPLACE(@parametro, ' ', '%') + '%';
END
GO

-- paVentaListar: ya no referencia IdEmpleado si es NULL; mostramos IdEmpleado si existe.
CREATE OR ALTER PROC paVentaListar
    @parametro NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT v.IdVenta, v.IdUsuario, v.IdEmpleado, v.TipoDocumento, v.DocumentoCliente, 
           v.NombreCliente, v.MontoPago, v.MontoCambio, v.MontoTotal, 
           v.fechaRegistro, v.estado
    FROM Venta v
    WHERE v.estado != -1
      AND (v.DocumentoCliente LIKE '%' + @parametro + '%'
           OR v.NombreCliente LIKE '%' + @parametro + '%'
           OR v.TipoDocumento LIKE '%' + @parametro + '%');
END
GO

-- Datos de prueba
INSERT INTO Categoria (Descripcion) VALUES
('Bebidas'),
('Hamburguesas'),
('Postres'),
('Snacks');

-- Empleados
INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, cargo) VALUES
('1234567890', 'Pedro', 'Gómez', 'Martínez', 'Calle del Sabor 10, Ciudad', 9876543210, 'Cocinero'),
('0987654321', 'Lucía', 'Fernández', 'Pérez', 'Avenida del Hambre 20, Ciudad', 9123456789, 'Atención al Cliente');

-- Clientes
INSERT INTO Cliente (Documento, NombreCompleto, Correo, Telefono) VALUES
('20001', 'Juan Perez', 'juan.perez@example.com', '71234567'),
('20002', 'Maria Lopez', 'maria.lopez@example.com', '72345678'),
('20003', 'Carlos Gomez', 'carlos.gomez@example.com', '73456789');

-- Usuario (clave ya encriptada o hash según tu lógica)
INSERT INTO Usuario (usuario, clave)
VALUES ('jhoselin', 'S9I36R5QgtGwtxnpM0iiV8pkjX30McRyxpPOCEyDUQk=');
select * from Usuario;

-- Productos (evité duplicar Código P001)
INSERT INTO Producto (Codigo, Nombre, Descripcion, IdCategoria, Stock, PrecioVenta) VALUES
('P001', 'Coca-Cola 500ml', 'Bebida gaseosa', 1, 50, 7.50),
('P002', 'Hamburguesa Clásica', 'Hamburguesa con queso y vegetales', 2, 30, 25.00),
('P003', 'Brownie de Chocolate', 'Postre de chocolate', 3, 20, 12.00),
('P004', 'Papas Fritas', 'Papas fritas crocantes', 4, 40, 5.00),
('P005', 'Hamburguesa Simple', 'Hamburguesa de res con pan', 2, 50, 5.50);

-- Venta ejemplo (IdUsuario = 1 existe)
INSERT INTO Venta (IdUsuario, IdEmpleado, TipoDocumento, DocumentoCliente, NombreCliente, MontoPago, MontoCambio, MontoTotal)
VALUES (1, 1, 'Boleta', '101010', 'Juan Perez', 100.00, 10.00, 90.00);
GO

-- Consultas de verificación
SELECT * FROM Categoria;
SELECT * FROM Cliente;
SELECT * FROM Empleado;
SELECT * FROM Usuario;
SELECT * FROM Producto;
SELECT * FROM Venta;
SELECT * FROM VentaDetalle;

-- Procedimientos de prueba
EXEC paCategoriaListar 'Bebidas';
EXEC paVentaListar 'juan';
EXEC paProductoListar 'P002';
GO

-- Vista ejemplo
CREATE OR ALTER VIEW VistaEmpleado AS
SELECT idEmpleado, nombres, cedulaIdentidad FROM Empleado;
GO

-- Chequeos: filas huérfanas en VentaDetalle
SELECT * 
FROM VentaDetalle vd
LEFT JOIN Venta v ON vd.IdVenta = v.IdVenta
WHERE v.IdVenta IS NULL;

SELECT * 
FROM VentaDetalle vd
LEFT JOIN Producto p ON vd.IdProducto = p.IdProducto
WHERE p.IdProducto IS NULL;

-- Buscar producto por código
SELECT * FROM Producto WHERE Codigo = 'P001';
GO