USE ClinicaDB;
GO
IF OBJECT_ID('dbo.Usuarios') IS NOT NULL DROP TABLE dbo.Usuarios;
GO
CREATE TABLE dbo.Usuarios (
    Id        INT IDENTITY(1,1) PRIMARY KEY,
    Correo    NVARCHAR(120) NOT NULL,
    ClaveHash NVARCHAR(100) NOT NULL,
    Rol       NVARCHAR(30)  NOT NULL,
    CONSTRAINT UQ_Usuarios_Correo UNIQUE (Correo),
    CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN ('Recepcionista','Director'))
);
GO
INSERT INTO dbo.Usuarios (Correo, ClaveHash, Rol) VALUES
('director@clinica.pe',  '$2b$11$uE9vQqcj6/4qJqa1z3LXc.5mKU0IPQ3iSc5w/9ON337549eTIHPpO', 'Director'),
('recepcion@clinica.pe', '$2b$11$uE9vQqcj6/4qJqa1z3LXc.5mKU0IPQ3iSc5w/9ON337549eTIHPpO', 'Recepcionista');
GO