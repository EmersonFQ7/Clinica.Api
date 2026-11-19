IF DB_ID('ClinicaDB') IS NULL
    CREATE DATABASE ClinicaDB;
GO
USE ClinicaDB;
GO

IF OBJECT_ID('dbo.Citas') IS NOT NULL DROP TABLE dbo.Citas;
IF OBJECT_ID('dbo.Medicos') IS NOT NULL DROP TABLE dbo.Medicos;
GO

CREATE TABLE dbo.Medicos (
    Id           INT IDENTITY(1,1) PRIMARY KEY,
    Nombre       NVARCHAR(80)  NOT NULL,
    Apellido     NVARCHAR(80)  NOT NULL,
    Especialidad NVARCHAR(80)  NOT NULL,
    Email        NVARCHAR(120) NOT NULL,
    Activo       BIT           NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Medicos_Email UNIQUE (Email)
);

CREATE TABLE dbo.Citas (
    Id             INT IDENTITY(1,1) PRIMARY KEY,
    MedicoId       INT           NOT NULL,
    NombrePaciente NVARCHAR(120) NOT NULL,
    DniPaciente    NVARCHAR(15)  NOT NULL,
    FechaHora      DATETIME2     NOT NULL,
    Motivo         NVARCHAR(250) NULL,
    Estado         NVARCHAR(20)  NOT NULL DEFAULT 'Programada',
    CONSTRAINT FK_Citas_Medicos FOREIGN KEY (MedicoId) REFERENCES dbo.Medicos(Id),
    CONSTRAINT CK_Citas_Estado CHECK (Estado IN ('Programada','Atendida','Cancelada'))
);
GO

INSERT INTO dbo.Medicos (Nombre, Apellido, Especialidad, Email, Activo) VALUES
('Carlos',  'Ramírez', 'Cardiología', 'cramirez@clinica.pe', 1),
('Lucía',   'Torres',  'Pediatría',   'ltorres@clinica.pe',  1),
('Miguel',  'Vargas',  'Dermatología','mvargas@clinica.pe',  1),
('Elena',   'Soto',    'Cardiología', 'esoto@clinica.pe',    0);

INSERT INTO dbo.Citas (MedicoId, NombrePaciente, DniPaciente, FechaHora, Motivo, Estado) VALUES
(1, 'Juan Pérez',   '45678912', DATEADD(HOUR,   9, CAST(CAST(DATEADD(DAY,  2, GETDATE()) AS DATE) AS DATETIME2)), 'Control de presión', 'Programada'),
(1, 'María Gómez',  '41236547', DATEADD(HOUR,  10, CAST(CAST(DATEADD(DAY,  2, GETDATE()) AS DATE) AS DATETIME2)), 'Dolor de pecho',     'Programada'),
(2, 'Pedro Quispe', '70123456', DATEADD(HOUR,  11, CAST(CAST(DATEADD(DAY,  3, GETDATE()) AS DATE) AS DATETIME2)), 'Vacunación',         'Programada'),
(3, 'Ana Flores',   '48965231', DATEADD(HOUR,  15, CAST(CAST(DATEADD(DAY, -5, GETDATE()) AS DATE) AS DATETIME2)), 'Revisión de lunar',  'Atendida');
GO