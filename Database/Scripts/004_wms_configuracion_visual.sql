/* =====================================================================
   Configuración visual de depósitos (WMS)
   - Zonas / Racks / Pasillos sobre lienzo + ubicaciones
   - Catálogos de estados (ubicación, stock, ingreso, pedido de salida)
   Script idempotente: puede ejecutarse varias veces sin error.
   ===================================================================== */

SET NOCOUNT ON;

/* ---------------------------------------------------------------------
   1. Zonas_Logisticas: geometría sobre el lienzo y relación con depósito
   --------------------------------------------------------------------- */
IF COL_LENGTH('Zonas_Logisticas', 'zonlog_depo_id') IS NULL
    ALTER TABLE Zonas_Logisticas ADD zonlog_depo_id INT NULL;
IF COL_LENGTH('Zonas_Logisticas', 'zonlog_x') IS NULL
    ALTER TABLE Zonas_Logisticas ADD zonlog_x DECIMAL(18,2) NULL;
IF COL_LENGTH('Zonas_Logisticas', 'zonlog_y') IS NULL
    ALTER TABLE Zonas_Logisticas ADD zonlog_y DECIMAL(18,2) NULL;
IF COL_LENGTH('Zonas_Logisticas', 'zonlog_largo') IS NULL
    ALTER TABLE Zonas_Logisticas ADD zonlog_largo DECIMAL(18,2) NULL;
IF COL_LENGTH('Zonas_Logisticas', 'zonlog_ancho') IS NULL
    ALTER TABLE Zonas_Logisticas ADD zonlog_ancho DECIMAL(18,2) NULL;
IF COL_LENGTH('Zonas_Logisticas', 'zonlog_color') IS NULL
    ALTER TABLE Zonas_Logisticas ADD zonlog_color NVARCHAR(20) NULL;
GO

/* ---------------------------------------------------------------------
   2. Depositos_Pasillos: relación con zona y marca de tránsito
   --------------------------------------------------------------------- */
IF COL_LENGTH('Depositos_Pasillos', 'depopas_zonlog_id') IS NULL
    ALTER TABLE Depositos_Pasillos ADD depopas_zonlog_id INT NULL;
IF COL_LENGTH('Depositos_Pasillos', 'depopas_es_transito') IS NULL
    ALTER TABLE Depositos_Pasillos ADD depopas_es_transito BIT NULL;
GO

/* ---------------------------------------------------------------------
   3. Depositos_Racks
   --------------------------------------------------------------------- */
IF OBJECT_ID('Depositos_Racks', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Depositos_Racks]
    (
        [deprack_id]                 INT             IDENTITY (1, 1) NOT NULL,
        [deprack_depo_id]            INT             NULL,
        [deprack_zonlog_id]          INT             NULL,
        [deprack_pasillo_id]         INT             NULL,
        [deprack_org_id]             INT             NULL,
        [deprack_codigo]             NVARCHAR (30)   NULL,
        [deprack_nombre]             NVARCHAR (100)  NULL,
        [deprack_descripcion]        NVARCHAR (MAX)  NULL,
        [deprack_x]                  DECIMAL (18, 2) NULL,
        [deprack_y]                  DECIMAL (18, 2) NULL,
        [deprack_largo]              DECIMAL (18, 2) NULL,
        [deprack_ancho]              DECIMAL (18, 2) NULL,
        [deprack_orientacion]        NVARCHAR (1)    NULL,
        [deprack_cantidad_columnas]  INT             NULL,
        [deprack_cantidad_niveles]   INT             NULL,
        [deprack_altura_nivel]       DECIMAL (18, 2) NULL,
        [deprack_peso_maximo]        DECIMAL (18, 2) NULL,
        [deprack_color]              NVARCHAR (20)   NULL,
        [deprack_usu_id_alta]        INT             NULL,
        [deprack_fec_alta]           DATETIME        NULL,
        [deprack_usu_id_mod]         INT             NULL,
        [deprack_fec_mod]            DATETIME        NULL,
        [deprack_fec_baja]           DATETIME        NULL,
        [deprack_usu_id_baja]        INT             NULL,
        [deprack_activo]             BIT             NULL,
        CONSTRAINT [PK_Depositos_Racks] PRIMARY KEY CLUSTERED ([deprack_id] ASC),
        CONSTRAINT [FK_Depositos_Racks_Depositos] FOREIGN KEY ([deprack_depo_id]) REFERENCES [dbo].[Depositos] ([depo_id])
    );

    CREATE INDEX [IX_Depositos_Racks_Deposito] ON [dbo].[Depositos_Racks] ([deprack_depo_id], [deprack_activo]);
END
GO

/* ---------------------------------------------------------------------
   4a. Ubicaciones_Logisticas: asegurar IDENTITY en la PK
       (la generación de ubicaciones depende de @@IDENTITY).
       Sólo se reconstruye si la tabla está vacía.
   --------------------------------------------------------------------- */
IF OBJECT_ID('Ubicaciones_Logisticas', 'U') IS NOT NULL
   AND COLUMNPROPERTY(OBJECT_ID('Ubicaciones_Logisticas'), 'ubilog_id', 'IsIdentity') = 0
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Ubicaciones_Logisticas)
    BEGIN
        IF OBJECT_ID('FK_Ubicaciones_Logisticas_Depositos', 'F') IS NOT NULL
            ALTER TABLE Ubicaciones_Logisticas DROP CONSTRAINT FK_Ubicaciones_Logisticas_Depositos;
        IF OBJECT_ID('FK_Ubicaciones_Logisticas_Depositos_Pasillos', 'F') IS NOT NULL
            ALTER TABLE Ubicaciones_Logisticas DROP CONSTRAINT FK_Ubicaciones_Logisticas_Depositos_Pasillos;

        DROP TABLE Ubicaciones_Logisticas;

        CREATE TABLE [dbo].[Ubicaciones_Logisticas] (
            [ubilog_id]                  INT             IDENTITY (1, 1) NOT NULL,
            [ubilog_codigo]              NVARCHAR (30)   NULL,
            [ubilog_tubilog_id]          INT             NULL,
            [ubilog_tmanilog_id]         INT             NULL,
            [ubilog_trotlog_id]          INT             NULL,
            [ubilog_teubilog_id]         INT             NULL,
            [ubilog_zonlog_id]           INT             NULL,
            [ubilog_secuencia_ruta]      NVARCHAR (50)   NULL,
            [ubilog_planta_id]           INT             NULL,
            [ubilog_depo_id]             INT             NULL,
            [ubilog_pasillo_id]          INT             NULL,
            [ubilog_deprack_id]          INT             NULL,
            [ubilog_posicion]            INT             NULL,
            [ubilog_columna]             INT             NULL,
            [ubilog_nivel]               NVARCHAR (10)   NULL,
            [ubilog_coord_x]             DECIMAL (18, 2) NULL,
            [ubilog_coord_y]             DECIMAL (18, 2) NULL,
            [ubilog_coord_z]             DECIMAL (18, 2) NULL,
            [ubilog_altura]              DECIMAL (18, 2) NULL,
            [ubilog_longitud]            DECIMAL (18, 2) NULL,
            [ubilog_anchura]             DECIMAL (18, 2) NULL,
            [ubilog_capacidad_cubica]    DECIMAL (18, 2) NULL,
            [ubilog_capacidad_maxima]    DECIMAL (18, 2) NULL,
            [ubilog_volumen_maximo]      DECIMAL (18, 2) NULL,
            [ubilog_peso_maximo]         DECIMAL (18, 2) NULL,
            [ubilog_tipo_producto_permitido] NVARCHAR (100) NULL,
            [ubilog_multiples_articulos] BIT             NULL,
            [ubilog_multiples_lotes]     BIT             NULL,
            [ubilog_usu_id_alta]         INT             NULL,
            [ubilog_fec_alta]            DATETIME        NULL,
            [ubilog_usu_id_mod]          INT             NULL,
            [ubilog_fec_mod]             DATETIME        NULL,
            [ubilog_activo]              BIT             NULL,
            CONSTRAINT [PK_Ubicaciones_Logisticas] PRIMARY KEY CLUSTERED ([ubilog_id] ASC),
            CONSTRAINT [FK_Ubicaciones_Logisticas_Depositos] FOREIGN KEY ([ubilog_depo_id]) REFERENCES [dbo].[Depositos] ([depo_id]),
            CONSTRAINT [FK_Ubicaciones_Logisticas_Depositos_Pasillos] FOREIGN KEY ([ubilog_pasillo_id]) REFERENCES [dbo].[Depositos_Pasillos] ([depopas_id])
        );

        PRINT 'Ubicaciones_Logisticas reconstruida con IDENTITY.';
    END
    ELSE
        PRINT 'ATENCION: Ubicaciones_Logisticas no tiene IDENTITY y contiene datos; revisar manualmente.';
END
GO

/* ---------------------------------------------------------------------
   4b. Ubicaciones_Logisticas: atributos extendidos
   --------------------------------------------------------------------- */
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_deprack_id') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_deprack_id INT NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_columna') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_columna INT NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_coord_x') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_coord_x DECIMAL(18,2) NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_coord_y') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_coord_y DECIMAL(18,2) NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_coord_z') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_coord_z DECIMAL(18,2) NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_capacidad_maxima') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_capacidad_maxima DECIMAL(18,2) NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_volumen_maximo') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_volumen_maximo DECIMAL(18,2) NULL;
IF COL_LENGTH('Ubicaciones_Logisticas', 'ubilog_tipo_producto_permitido') IS NULL
    ALTER TABLE Ubicaciones_Logisticas ADD ubilog_tipo_producto_permitido NVARCHAR(100) NULL;
GO

/* ---------------------------------------------------------------------
   5. Catálogos de estados
   --------------------------------------------------------------------- */
IF OBJECT_ID('Tipo_Estado_Stock', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Tipo_Estado_Stock] (
        [testk_id]          INT           IDENTITY (1, 1) NOT NULL,
        [testk_nombre]      NVARCHAR (50) NULL,
        [testk_org_id]      INT           NULL,
        [testk_usu_id_alta] INT           NULL,
        [testk_fec_alta]    DATETIME      NULL,
        [testk_usu_id_mod]  INT           NULL,
        [testk_fec_mod]     DATETIME      NULL,
        [testk_activo]      BIT           NULL,
        [testk_usu_id_baja] INT           NULL,
        [testk_fec_baja]    DATETIME      NULL,
        CONSTRAINT [PK_Tipo_Estado_Stock] PRIMARY KEY CLUSTERED ([testk_id] ASC)
    );
END
GO

IF OBJECT_ID('Tipo_Estado_Ingreso', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Tipo_Estado_Ingreso] (
        [teing_id]          INT           IDENTITY (1, 1) NOT NULL,
        [teing_nombre]      NVARCHAR (50) NULL,
        [teing_org_id]      INT           NULL,
        [teing_usu_id_alta] INT           NULL,
        [teing_fec_alta]    DATETIME      NULL,
        [teing_usu_id_mod]  INT           NULL,
        [teing_fec_mod]     DATETIME      NULL,
        [teing_activo]      BIT           NULL,
        [teing_usu_id_baja] INT           NULL,
        [teing_fec_baja]    DATETIME      NULL,
        CONSTRAINT [PK_Tipo_Estado_Ingreso] PRIMARY KEY CLUSTERED ([teing_id] ASC)
    );
END
GO

IF OBJECT_ID('Tipo_Estado_Pedido_Salida', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Tipo_Estado_Pedido_Salida] (
        [tepsa_id]          INT           IDENTITY (1, 1) NOT NULL,
        [tepsa_nombre]      NVARCHAR (50) NULL,
        [tepsa_org_id]      INT           NULL,
        [tepsa_usu_id_alta] INT           NULL,
        [tepsa_fec_alta]    DATETIME      NULL,
        [tepsa_usu_id_mod]  INT           NULL,
        [tepsa_fec_mod]     DATETIME      NULL,
        [tepsa_activo]      BIT           NULL,
        [tepsa_usu_id_baja] INT           NULL,
        [tepsa_fec_baja]    DATETIME      NULL,
        CONSTRAINT [PK_Tipo_Estado_Pedido_Salida] PRIMARY KEY CLUSTERED ([tepsa_id] ASC)
    );
END
GO

/* ---------------------------------------------------------------------
   6. Carga de catálogos (estados base)
   --------------------------------------------------------------------- */

-- Estados de ubicación
INSERT INTO Tipo_Estado_Ubicacion_Logistica (teubilog_nombre, teubilog_activo, teubilog_fec_alta)
SELECT v.nombre, 1, GETDATE()
FROM (VALUES
    (N'Libre'), (N'Ocupada'), (N'Parcialmente ocupada'),
    (N'Reservada'), (N'Bloqueada'), (N'En mantenimiento'), (N'Inactiva')
) AS v(nombre)
WHERE NOT EXISTS (SELECT 1 FROM Tipo_Estado_Ubicacion_Logistica e WHERE e.teubilog_nombre = v.nombre);

-- Estados de stock
INSERT INTO Tipo_Estado_Stock (testk_nombre, testk_activo, testk_fec_alta)
SELECT v.nombre, 1, GETDATE()
FROM (VALUES
    (N'Disponible'), (N'Reservado'), (N'Bloqueado'), (N'En recepción'),
    (N'Pendiente de ubicación'), (N'En picking'), (N'Preparado'),
    (N'Despachado'), (N'Dañado'), (N'Vencido')
) AS v(nombre)
WHERE NOT EXISTS (SELECT 1 FROM Tipo_Estado_Stock e WHERE e.testk_nombre = v.nombre);

-- Estados de ingreso
INSERT INTO Tipo_Estado_Ingreso (teing_nombre, teing_activo, teing_fec_alta)
SELECT v.nombre, 1, GETDATE()
FROM (VALUES
    (N'Borrador'), (N'Pendiente de recepción'), (N'Recibido parcial'),
    (N'Recibido completo'), (N'Con diferencias'), (N'Cerrado'), (N'Cancelado')
) AS v(nombre)
WHERE NOT EXISTS (SELECT 1 FROM Tipo_Estado_Ingreso e WHERE e.teing_nombre = v.nombre);

-- Estados de pedido de salida
INSERT INTO Tipo_Estado_Pedido_Salida (tepsa_nombre, tepsa_activo, tepsa_fec_alta)
SELECT v.nombre, 1, GETDATE()
FROM (VALUES
    (N'Borrador'), (N'Pendiente de reserva'), (N'Reservado'), (N'En picking'),
    (N'Picking parcial'), (N'Preparado'), (N'En packing'),
    (N'Listo para despacho'), (N'Despachado'), (N'Cancelado')
) AS v(nombre)
WHERE NOT EXISTS (SELECT 1 FROM Tipo_Estado_Pedido_Salida e WHERE e.tepsa_nombre = v.nombre);
GO

/* ---------------------------------------------------------------------
   7. Menú (SIS_Acciones)
   --------------------------------------------------------------------- */
-- Configuración visual del depósito (bajo "Gestión Deposito" = id_padre 29)
INSERT INTO SIS_Acciones(acc_nombre, acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_activo)
SELECT N'Configuración visual', N'Submenú - Configuración visual del depósito', N'Depositos', N'ConfiguracionVisual', 29, N'fa fa-th', 60, CAST(1 AS BIT), CAST(1 AS BIT)
WHERE NOT EXISTS (SELECT 1 FROM SIS_Acciones WHERE acc_controller='Depositos' AND acc_accion='ConfiguracionVisual');

-- Catálogos de estados (bajo "Ubicaciones Logísticas" = id_padre 27)
INSERT INTO SIS_Acciones(acc_nombre, acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_activo)
SELECT N'Estados de stock', N'Submenú - Tipos de estados de stock', N'UbicacionesLogisticas', N'EstadosStock', 27, N'sin', 110, CAST(1 AS BIT), CAST(1 AS BIT)
WHERE NOT EXISTS (SELECT 1 FROM SIS_Acciones WHERE acc_controller='UbicacionesLogisticas' AND acc_accion='EstadosStock');

INSERT INTO SIS_Acciones(acc_nombre, acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_activo)
SELECT N'Estados de ingreso', N'Submenú - Tipos de estados de ingreso', N'UbicacionesLogisticas', N'EstadosIngreso', 27, N'sin', 120, CAST(1 AS BIT), CAST(1 AS BIT)
WHERE NOT EXISTS (SELECT 1 FROM SIS_Acciones WHERE acc_controller='UbicacionesLogisticas' AND acc_accion='EstadosIngreso');

INSERT INTO SIS_Acciones(acc_nombre, acc_descripcion, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_activo)
SELECT N'Estados de pedido de salida', N'Submenú - Tipos de estados de pedido de salida', N'UbicacionesLogisticas', N'EstadosPedidoSalida', 27, N'sin', 130, CAST(1 AS BIT), CAST(1 AS BIT)
WHERE NOT EXISTS (SELECT 1 FROM SIS_Acciones WHERE acc_controller='UbicacionesLogisticas' AND acc_accion='EstadosPedidoSalida');

-- Asociar las nuevas acciones a todos los perfiles que ya ven Depósitos / Ubicaciones
INSERT INTO SIS_Perfiles_Acciones(pac_prf_id, pac_acc_id)
SELECT DISTINCT pa.pac_prf_id, a.acc_id
FROM SIS_Acciones a
CROSS JOIN SIS_Perfiles_Acciones pa
INNER JOIN SIS_Acciones ref ON ref.acc_id = pa.pac_acc_id
WHERE a.acc_controller IN ('Depositos','UbicacionesLogisticas')
  AND a.acc_accion IN ('ConfiguracionVisual','EstadosStock','EstadosIngreso','EstadosPedidoSalida')
  AND ref.acc_controller IN ('Depositos','UbicacionesLogisticas')
  AND NOT EXISTS (SELECT 1 FROM SIS_Perfiles_Acciones x WHERE x.pac_prf_id=pa.pac_prf_id AND x.pac_acc_id=a.acc_id);
GO

PRINT 'Configuración visual de depósitos: migración aplicada.';
