/* Esquema base inventario multi-organización */

CREATE TABLE Categorias_Productos (
    catpro_id INT IDENTITY(1,1) PRIMARY KEY,
    catpro_org_id INT NOT NULL,
    catpro_nombre VARCHAR(120) NOT NULL,
    catpro_descripcion VARCHAR(255) NULL,
    catpro_activo BIT NOT NULL CONSTRAINT DF_catpro_activo DEFAULT(1),
    catpro_fec_alta DATETIME NOT NULL CONSTRAINT DF_catpro_fec_alta DEFAULT(GETDATE()),
    catpro_fec_mod DATETIME NULL,
    catpro_fec_baja DATETIME NULL,
    catpro_usu_id_alta INT NULL,
    catpro_usu_id_mod INT NULL,
    catpro_usu_id_baja INT NULL,
    CONSTRAINT FK_catpro_org FOREIGN KEY (catpro_org_id) REFERENCES SIS_Organizaciones(org_id)
);
CREATE UNIQUE INDEX UX_catpro_org_nombre ON Categorias_Productos(catpro_org_id, catpro_nombre);

CREATE TABLE Unidades_Medidas (
    unimed_id INT IDENTITY(1,1) PRIMARY KEY,
    unimed_org_id INT NOT NULL,
    unimed_nombre VARCHAR(80) NOT NULL,
    unimed_codigo VARCHAR(20) NOT NULL,
    unimed_activo BIT NOT NULL CONSTRAINT DF_unimed_activo DEFAULT(1),
    unimed_fec_alta DATETIME NOT NULL CONSTRAINT DF_unimed_fec_alta DEFAULT(GETDATE()),
    unimed_fec_mod DATETIME NULL,
    unimed_fec_baja DATETIME NULL,
    unimed_usu_id_alta INT NULL,
    unimed_usu_id_mod INT NULL,
    unimed_usu_id_baja INT NULL,
    CONSTRAINT FK_unimed_org FOREIGN KEY (unimed_org_id) REFERENCES SIS_Organizaciones(org_id)
);
CREATE UNIQUE INDEX UX_unimed_org_codigo ON Unidades_Medidas(unimed_org_id, unimed_codigo);

CREATE TABLE Productos (
    pro_id INT IDENTITY(1,1) PRIMARY KEY,
    pro_org_id INT NOT NULL,
    pro_catpro_id INT NOT NULL,
    pro_unimed_id INT NOT NULL,
    pro_codigo_interno VARCHAR(60) NOT NULL,
    pro_codigo_barras VARCHAR(60) NULL,
    pro_descripcion_corta VARCHAR(180) NOT NULL,
    pro_descripcion_larga VARCHAR(500) NULL,
    pro_requiere_lote BIT NOT NULL CONSTRAINT DF_pro_lote DEFAULT(0),
    pro_requiere_vencimiento BIT NOT NULL CONSTRAINT DF_pro_venc DEFAULT(0),
    pro_requiere_serie BIT NOT NULL CONSTRAINT DF_pro_serie DEFAULT(0),
    pro_requiere_trazabilidad BIT NOT NULL CONSTRAINT DF_pro_traza DEFAULT(0),
    pro_controlado BIT NOT NULL CONSTRAINT DF_pro_controlado DEFAULT(0),
    pro_stock_minimo DECIMAL(18,4) NOT NULL CONSTRAINT DF_pro_stockmin DEFAULT(0),
    pro_stock_maximo DECIMAL(18,4) NOT NULL CONSTRAINT DF_pro_stockmax DEFAULT(0),
    pro_punto_reposicion DECIMAL(18,4) NOT NULL CONSTRAINT DF_pro_repos DEFAULT(0),
    pro_activo BIT NOT NULL CONSTRAINT DF_pro_activo DEFAULT(1),
    pro_fec_alta DATETIME NOT NULL CONSTRAINT DF_pro_fec_alta DEFAULT(GETDATE()),
    pro_fec_mod DATETIME NULL,
    pro_fec_baja DATETIME NULL,
    pro_usu_id_alta INT NULL,
    pro_usu_id_mod INT NULL,
    pro_usu_id_baja INT NULL,
    CONSTRAINT FK_pro_org FOREIGN KEY (pro_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_pro_catpro FOREIGN KEY (pro_catpro_id) REFERENCES Categorias_Productos(catpro_id),
    CONSTRAINT FK_pro_unimed FOREIGN KEY (pro_unimed_id) REFERENCES Unidades_Medidas(unimed_id)
);
CREATE UNIQUE INDEX UX_pro_org_codigo ON Productos(pro_org_id, pro_codigo_interno);

CREATE TABLE Tipos_Movimientos_Inventario (
    timi_id INT IDENTITY(1,1) PRIMARY KEY,
    timi_org_id INT NOT NULL,
    timi_codigo VARCHAR(30) NOT NULL,
    timi_nombre VARCHAR(120) NOT NULL,
    timi_signo CHAR(1) NOT NULL,
    timi_activo BIT NOT NULL CONSTRAINT DF_timi_activo DEFAULT(1),
    timi_fec_alta DATETIME NOT NULL CONSTRAINT DF_timi_fec_alta DEFAULT(GETDATE()),
    timi_fec_mod DATETIME NULL,
    timi_fec_baja DATETIME NULL,
    timi_usu_id_alta INT NULL,
    timi_usu_id_mod INT NULL,
    timi_usu_id_baja INT NULL,
    CONSTRAINT FK_timi_org FOREIGN KEY (timi_org_id) REFERENCES SIS_Organizaciones(org_id)
);
CREATE UNIQUE INDEX UX_timi_org_codigo ON Tipos_Movimientos_Inventario(timi_org_id, timi_codigo);

CREATE TABLE Stocks (
    stock_id INT IDENTITY(1,1) PRIMARY KEY,
    stock_org_id INT NOT NULL,
    stock_depo_id INT NOT NULL,
    stock_pro_id INT NOT NULL,
    stock_actual DECIMAL(18,4) NOT NULL CONSTRAINT DF_stock_actual DEFAULT(0),
    stock_reservado DECIMAL(18,4) NOT NULL CONSTRAINT DF_stock_res DEFAULT(0),
    stock_disponible AS (stock_actual - stock_reservado),
    stock_activo BIT NOT NULL CONSTRAINT DF_stock_activo DEFAULT(1),
    stock_fec_alta DATETIME NOT NULL CONSTRAINT DF_stock_fec_alta DEFAULT(GETDATE()),
    stock_fec_mod DATETIME NULL,
    stock_fec_baja DATETIME NULL,
    stock_usu_id_alta INT NULL,
    stock_usu_id_mod INT NULL,
    stock_usu_id_baja INT NULL,
    CONSTRAINT FK_stock_org FOREIGN KEY (stock_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_stock_depo FOREIGN KEY (stock_depo_id) REFERENCES Depositos(depo_id),
    CONSTRAINT FK_stock_pro FOREIGN KEY (stock_pro_id) REFERENCES Productos(pro_id)
);
CREATE UNIQUE INDEX UX_stock_org_depo_pro ON Stocks(stock_org_id, stock_depo_id, stock_pro_id);

CREATE TABLE Movimientos_Inventario (
    movinv_id INT IDENTITY(1,1) PRIMARY KEY,
    movinv_org_id INT NOT NULL,
    movinv_timi_id INT NOT NULL,
    movinv_depo_origen_id INT NOT NULL,
    movinv_depo_destino_id INT NULL,
    movinv_pro_id INT NOT NULL,
    movinv_fecha DATETIME NOT NULL,
    movinv_cantidad DECIMAL(18,4) NOT NULL,
    movinv_lote VARCHAR(80) NULL,
    movinv_vencimiento DATETIME NULL,
    movinv_serie VARCHAR(80) NULL,
    movinv_motivo VARCHAR(255) NULL,
    movinv_observaciones VARCHAR(500) NULL,
    movinv_estado VARCHAR(30) NOT NULL,
    movinv_activo BIT NOT NULL CONSTRAINT DF_movinv_activo DEFAULT(1),
    movinv_fec_alta DATETIME NOT NULL CONSTRAINT DF_movinv_fec_alta DEFAULT(GETDATE()),
    movinv_fec_mod DATETIME NULL,
    movinv_fec_baja DATETIME NULL,
    movinv_usu_id_alta INT NULL,
    movinv_usu_id_mod INT NULL,
    movinv_usu_id_baja INT NULL,
    CONSTRAINT FK_movinv_org FOREIGN KEY (movinv_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_movinv_timi FOREIGN KEY (movinv_timi_id) REFERENCES Tipos_Movimientos_Inventario(timi_id),
    CONSTRAINT FK_movinv_depo_o FOREIGN KEY (movinv_depo_origen_id) REFERENCES Depositos(depo_id),
    CONSTRAINT FK_movinv_depo_d FOREIGN KEY (movinv_depo_destino_id) REFERENCES Depositos(depo_id),
    CONSTRAINT FK_movinv_pro FOREIGN KEY (movinv_pro_id) REFERENCES Productos(pro_id)
);
CREATE INDEX IX_movinv_org_fecha ON Movimientos_Inventario(movinv_org_id, movinv_fecha DESC);

CREATE TABLE Solicitudes_Suministros (
    solsumi_id INT IDENTITY(1,1) PRIMARY KEY,
    solsumi_org_id INT NOT NULL,
    solsumi_depo_id INT NOT NULL,
    solsumi_sector_solicitante VARCHAR(180) NOT NULL,
    solsumi_estado VARCHAR(30) NOT NULL,
    solsumi_fecha DATETIME NOT NULL,
    solsumi_observaciones VARCHAR(500) NULL,
    solsumi_activo BIT NOT NULL CONSTRAINT DF_solsumi_activo DEFAULT(1),
    solsumi_fec_alta DATETIME NOT NULL CONSTRAINT DF_solsumi_fec_alta DEFAULT(GETDATE()),
    solsumi_fec_mod DATETIME NULL,
    solsumi_fec_baja DATETIME NULL,
    solsumi_usu_id_alta INT NULL,
    solsumi_usu_id_mod INT NULL,
    solsumi_usu_id_baja INT NULL,
    CONSTRAINT FK_solsumi_org FOREIGN KEY (solsumi_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_solsumi_depo FOREIGN KEY (solsumi_depo_id) REFERENCES Depositos(depo_id)
);

CREATE TABLE Solicitudes_Suministros_Detalles (
    solsumi_det_id INT IDENTITY(1,1) PRIMARY KEY,
    solsumi_det_org_id INT NOT NULL,
    solsumi_det_solsumi_id INT NOT NULL,
    solsumi_det_pro_id INT NOT NULL,
    solsumi_det_cantidad_solicitada DECIMAL(18,4) NOT NULL,
    solsumi_det_cantidad_entregada DECIMAL(18,4) NOT NULL CONSTRAINT DF_solsumi_det_ent DEFAULT(0),
    solsumi_det_estado VARCHAR(30) NOT NULL,
    CONSTRAINT FK_solsumi_det_org FOREIGN KEY (solsumi_det_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_solsumi_det_cab FOREIGN KEY (solsumi_det_solsumi_id) REFERENCES Solicitudes_Suministros(solsumi_id),
    CONSTRAINT FK_solsumi_det_pro FOREIGN KEY (solsumi_det_pro_id) REFERENCES Productos(pro_id)
);

CREATE TABLE Inventarios_Conteos (
    invcon_id INT IDENTITY(1,1) PRIMARY KEY,
    invcon_org_id INT NOT NULL,
    invcon_depo_id INT NOT NULL,
    invcon_fecha DATETIME NOT NULL,
    invcon_estado VARCHAR(30) NOT NULL,
    invcon_observaciones VARCHAR(500) NULL,
    invcon_fec_alta DATETIME NOT NULL CONSTRAINT DF_invcon_fec_alta DEFAULT(GETDATE()),
    invcon_usu_id_alta INT NULL,
    CONSTRAINT FK_invcon_org FOREIGN KEY (invcon_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_invcon_depo FOREIGN KEY (invcon_depo_id) REFERENCES Depositos(depo_id)
);

CREATE TABLE Alertas_Inventario (
    alertinv_id INT IDENTITY(1,1) PRIMARY KEY,
    alertinv_org_id INT NOT NULL,
    alertinv_pro_id INT NOT NULL,
    alertinv_depo_id INT NULL,
    alertinv_tipo VARCHAR(50) NOT NULL,
    alertinv_mensaje VARCHAR(500) NOT NULL,
    alertinv_nivel VARCHAR(20) NOT NULL,
    alertinv_resuelta BIT NOT NULL CONSTRAINT DF_alertinv_res DEFAULT(0),
    alertinv_fecha DATETIME NOT NULL CONSTRAINT DF_alertinv_fecha DEFAULT(GETDATE()),
    CONSTRAINT FK_alertinv_org FOREIGN KEY (alertinv_org_id) REFERENCES SIS_Organizaciones(org_id),
    CONSTRAINT FK_alertinv_pro FOREIGN KEY (alertinv_pro_id) REFERENCES Productos(pro_id),
    CONSTRAINT FK_alertinv_depo FOREIGN KEY (alertinv_depo_id) REFERENCES Depositos(depo_id)
);

CREATE TABLE Auditoria_Inventario (
    audinv_id INT IDENTITY(1,1) PRIMARY KEY,
    audinv_org_id INT NOT NULL,
    audinv_entidad VARCHAR(80) NOT NULL,
    audinv_entidad_id INT NOT NULL,
    audinv_accion VARCHAR(30) NOT NULL,
    audinv_datos_antes VARCHAR(MAX) NULL,
    audinv_datos_despues VARCHAR(MAX) NULL,
    audinv_contexto VARCHAR(250) NULL,
    audinv_usuario_id INT NOT NULL,
    audinv_fecha DATETIME NOT NULL CONSTRAINT DF_audinv_fecha DEFAULT(GETDATE()),
    CONSTRAINT FK_audinv_org FOREIGN KEY (audinv_org_id) REFERENCES SIS_Organizaciones(org_id)
);
