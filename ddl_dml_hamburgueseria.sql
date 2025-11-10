CREATE DATABASE LabHamburgueseria;
GO

USE [master];
GO
drop login usrhambu;
drop user usrhambu;
CREATE LOGIN [usrhambu] WITH PASSWORD = N'123456',
    DEFAULT_DATABASE = [LabHamburgueseria],
    CHECK_EXPIRATION = OFF,
    CHECK_POLICY = ON;
GO

USE [LabHamburgueseria];
GO

drop login usrhambu;

CREATE USER [usrhambu] FOR LOGIN [usrhambu];
GO
ALTER ROLE [db_owner] ADD MEMBER [usrhambu];
GO


-- ============================
-- ELIMINAR OBJETOS EXISTENTES
-- ============================
DROP TABLE IF EXISTS DetalleVentas;
DROP TABLE IF EXISTS Ventas;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS Categoria;
DROP PROC IF EXISTS paVentaListar;
DROP PROC IF EXISTS paClienteListar;
DROP PROC IF EXISTS paProductoListar;
DROP PROC IF EXISTS paEmpleadoListar;
GO

-- ============================
-- TABLAS PRINCIPALES
-- ============================

CREATE TABLE Categoria (
    id INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(50) NOT NULL UNIQUE,
    estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE Producto (
    id INT PRIMARY KEY IDENTITY(1,1),
    idCategoria INT NOT NULL,
    codigo VARCHAR(20) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(250),
    saldo DECIMAL NOT NULL DEFAULT 0,
    precioVenta DECIMAL NOT NULL CHECK (precioVenta > 0),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (idCategoria) REFERENCES Categoria(id)
);

CREATE TABLE Cliente (
    id INT PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad VARCHAR(12) NOT NULL,
    nombres VARCHAR(100) NOT NULL,
    apellidos VARCHAR(100) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE Empleado (
    id INT PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad VARCHAR(12) NOT NULL,
    nombres VARCHAR(30) NOT NULL,
    primerApellido VARCHAR(30) NULL,
    segundoApellido VARCHAR(30) NULL,
    direccion VARCHAR(250) NOT NULL,
    celular BIGINT NOT NULL,
    cargo VARCHAR(50) NOT NULL,  
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1
);

CREATE TABLE Usuario (
    id INT PRIMARY KEY IDENTITY(1,1),
    idEmpleado INT NOT NULL, 
    usuario VARCHAR(50) UNIQUE NOT NULL,
    clave VARCHAR(255) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Usuario_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);

-- ============================
-- TABLAS DE VENTAS
-- ============================

CREATE TABLE Ventas (
    id INT PRIMARY KEY IDENTITY(1,1),
    idUsuario INT NOT NULL,
    idCliente INT NOT NULL,
    numeroTransaccion AS ('VEN-' + CAST(id AS VARCHAR(10))),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_Venta_Usuario FOREIGN KEY(idUsuario) REFERENCES Usuario(id),
    CONSTRAINT fk_Venta_Cliente FOREIGN KEY(idCliente) REFERENCES Cliente(id)
);

CREATE TABLE DetalleVentas (
    id INT PRIMARY KEY IDENTITY(1,1),
    idVenta INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad INT NOT NULL,
    precioUnitario DECIMAL NOT NULL,
    total AS (cantidad * precioUnitario) PERSISTED,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1,
    CONSTRAINT fk_DetalleVenta_Venta FOREIGN KEY(idVenta) REFERENCES Ventas(id),
    CONSTRAINT fk_DetalleVenta_Producto FOREIGN KEY(idProducto) REFERENCES Producto(id)
);
GO

-- ============================
-- PROCEDIMIENTOS ALMACENADOS
-- ============================

CREATE PROC paVentaListar @parametro VARCHAR(100)
AS
  SELECT v.id, v.numeroTransaccion, 
         c.nombres + ' ' + c.apellidos AS Cliente, 
         u.usuario AS Usuario, 
         v.fechaRegistro, v.estado
  FROM Ventas v
  INNER JOIN Cliente c ON c.id = v.idCliente
  INNER JOIN Usuario u ON u.id = v.idUsuario
  WHERE v.estado<>-1 
    AND (c.nombres + c.apellidos + u.usuario + v.numeroTransaccion) LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY v.fechaRegistro DESC;
GO

CREATE PROC paClienteListar @parametro VARCHAR(100)
AS
  SELECT c.id, c.cedulaIdentidad, c.nombres, c.apellidos, 
         c.usuarioRegistro, c.fechaRegistro, c.estado
  FROM Cliente c
  WHERE c.estado<>-1 
    AND (c.cedulaIdentidad + c.nombres + c.apellidos) LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
  ORDER BY c.nombres, c.apellidos;
GO

CREATE PROC paProductoListar @parametro VARCHAR(100)
AS
  SELECT p.id, p.codigo,p.nombre, p.descripcion, ca.nombre AS Categoria, 
         p.saldo, p.precioVenta, p.usuarioRegistro, p.fechaRegistro, 
         p.estado, p.idCategoria
  FROM Producto p
  INNER JOIN Categoria ca ON ca.id = p.idCategoria
  WHERE p.estado<>-1 
    AND (p.nombre+p.codigo+p.descripcion+ca.nombre) LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY p.estado DESC, p.nombre ASC;
GO

CREATE PROC paEmpleadoListar @parametro VARCHAR(50)
AS
  SELECT e.id, e.cedulaIdentidad, e.nombres, ISNULL(e.primerApellido,'') AS primerApellido, 
		 ISNULL(e.segundoApellido, '') AS segundoApellido, e.direccion, e.celular, e.cargo,
		 ISNULL(e.usuarioRegistro, '') AS usuarioRegistro, ISNULL(e.fechaRegistro,GETDATE()) AS fechaRegistro, 
		 ISNULL(u.id,0) as idUsuario, ISNULL(u.usuario, '') as usuario,
         e.estado
  FROM Empleado e
  LEFT JOIN Usuario u ON e.id = u.idEmpleado
  WHERE e.estado<>-1 
		AND (e.cedulaIdentidad+e.nombres+e.primerApellido+e.segundoApellido) LIKE '%'+REPLACE(@parametro, ' ', '%')+'%'
  ORDER BY e.nombres,e.primerApellido;
GO

-- ============================
-- DATOS DE PRUEBA
-- ============================

-- Categorías
INSERT INTO Categoria(nombre) VALUES ('Hamburguesas');
INSERT INTO Categoria(nombre) VALUES ('Bebidas');
INSERT INTO Categoria(nombre) VALUES ('Acompañamientos');
INSERT INTO Categoria(nombre) VALUES ('Postres');

select * from Categoria;

-- Productos
INSERT INTO Producto(idCategoria,codigo,nombre,descripcion,saldo,precioVenta)
VALUES (1,'HB-CL', 'Hamburguesa Clásica', 'Carne, queso, tomate, lechuga y salsa especial', 30, 18);

INSERT INTO Producto(idCategoria,codigo,nombre,descripcion,saldo,precioVenta)
VALUES (1,'HB-DLX', 'Hamburguesa Deluxe', 'Doble carne con queso cheddar, tocino y cebolla caramelizada', 20, 25);

INSERT INTO Producto(idCategoria,codigo,nombre,descripcion,saldo,precioVenta)
VALUES (2,'BB-CO', 'Bebida Cola', 'Bebida gaseosa 500ml', 50, 5);

INSERT INTO Producto(idCategoria,codigo,nombre,descripcion,saldo,precioVenta)
VALUES (3,'AC-PF', 'Papas Fritas', 'Porción mediana de papas fritas', 40, 8);

INSERT INTO Producto(idCategoria,codigo,nombre,descripcion,saldo,precioVenta)
VALUES (4,'PS-CH', 'Cheesecake', 'Porción de cheesecake casero', 15, 10);

select * from Producto;

-- Empleados
INSERT INTO Empleado(cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, cargo)
VALUES ('1234567', 'Lucas', 'Rojas', 'Fernández', 'Av. Principal 123', 71717171, 'Cajero');

-- Clientes
INSERT INTO Cliente(cedulaIdentidad,nombres,apellidos)
VALUES ('765421', 'Andrea', 'López');

-- Usuarios
INSERT INTO Usuario(idEmpleado,usuario,clave)
VALUES (1, 'jairox', 'I0HCOO/NSSY6WOS9POP5XW==');

select * from Usuario;
-- Venta de ejemplo
INSERT INTO Ventas (idUsuario, idCliente)
VALUES (1, 1);

-- ============================
-- CONSULTAS DE PRUEBA
-- ============================
SELECT * FROM Categoria;
SELECT * FROM Producto;
SELECT * FROM Empleado;
SELECT * FROM Cliente;
SELECT * FROM Usuario;
SELECT * FROM Ventas;

EXEC paProductoListar '';