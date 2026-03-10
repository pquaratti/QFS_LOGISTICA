SET IDENTITY_INSERT [dbo].[SIS_Acciones] ON;

MERGE INTO [dbo].[SIS_Acciones] AS Target
USING (
    VALUES
        (1,  N'Dashboard',                N'Dashboard General',                                   N'Dashboard',              N'Home',                    0,  N'fa fa-home',     1,   CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (2,  N'Sistema',                  N'Menú - Sistema',                                      NULL,                      NULL,                       0,  N'fa fa-home',     1,   CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (3,  N'Usuario',                  N'Submenú - Listado de usuarios del sistema',           N'Usuario',                N'Index',                   2,  N'sin',            1,   CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (4,  N'Acciones',                 N'Submenú - Listado de acciones del sistema',           N'Accion',                 N'Index',                   2,  N'sin',            2,   CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (5,  N'Modulos',                  N'Submenú - Listado de módulos del sistema',            N'Modulo',                 N'Index',                   2,  N'sin',            2,   CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (6,  N'Configuración',            N'Menú - Configuración',                                NULL,                      NULL,                       0,  N'fa fa-home',     10,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (7,  N'Preguntas Frecuentes',     N'Submenú - Gestión de Preguntas Frecuentes',           N'PreguntasFrecuentes',    N'PreguntasFrecuentesABM',  6,  N'fa fa-home',     10,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (8,  N'Mesa de Ayuda',            N'Submenú - Gestión de Mesa de Ayuda',                  N'MesaAyuda',              N'Administrador',           6,  N'fa fa-home',     20,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (9,  N'Tutoriales',               N'Submenú - Gestión de Tutoriales',                     N'Tutoriales',             N'Tutoriales',              6,  N'fa fa-home',     30,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (10, N'Organizacion',             N'Menú - Organización',                                 NULL,                      NULL,                       0,  N'fa fa-sitemap',  0,   CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (11, N'Areas',                    N'Submenú - Areas de la organización',                  N'Areas',                  N'Index',                   10, N'sin',            10,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, NULL),
        (16, N'Gestión de Personal',      N'Submenú - Gestión de Colaboradores',                  N'Colaboradores',          N'administracion',          18, N'sin',            100, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (17, N'Organizaciones',           N'Submenú - Gestión de Organizaciones',                 N'Organizaciones',         N'Index',                   2,  N'sin',            100, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (18, N'Personal',                 N'Menú - Personal',                                     NULL,                      NULL,                       0,  N'fa fa-users',    30,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (19, N'Categorías',               N'Submenú - Gestión de Categorías de Colaboradores',   N'Categorias',             N'Colaborador',             18, N'sin',            10,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (20, N'Depósitos',                N'Submenú - Depósitos',                                 N'Depositos',              N'Index',                   6,  N'sin',            100, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (21, N'Plantas',                  N'Submenú - Plantas',                                   N'Plantas',                N'Index',                   6,  N'sin',            50,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (22, N'Clientes',                 N'Submenú - Gestión de Clientes',                       N'Clientes',               N'Index',                   6,  N'sin',            200, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (23, N'Zonas',                    N'Submenú - Zonas Logísticas',                          N'ZonasLogisticas',        N'Index',                   6,  N'sin',            200, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (24, N'Tipos de Manipulaciones',  N'Submenú - Tipos de manipulaciones logisticas',        N'ManipulacionesLogisticas', N'Tipos',                 6,  N'sin',            210, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (25, N'Tipos de Rotaciones',      N'Submenú - Tipos de Rotaciones Logísticas',            N'RotacionesLogisticas',   N'Tipos',                   6,  N'sin',            220, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (26, N'Tipos',                    N'Submenú - Tipos de Ubicaciones Logísticas',           N'UbicacionesLogisticas',  N'Tipos',                   27, N'sin',            230, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (27, N'Ubicaciones Logísticas',   N'Menú - Ubicaciones Logísticas',                       NULL,                      NULL,                       0,  N'fa fa-boxes',    50,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (28, N'Estados',                  N'Submenú - Tipo de Estados de Ubicaciones',            N'UbicacionesLogisticas',  N'Estados',                 27, N'sin',            100, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (29, N'Gestión Deposito',         N'Menú de Gestión de Depósitos',                        NULL,                      NULL,                       0,  N'fa fa-home',     50,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (30, N'Inventario depósito',      N'Submenú - Inventario Depósito',                       N'Depositos',              N'Inventario',              29, N'sin',            50,  CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (31, N'Marcas',                   N'Submenú - Gestión de Marcas ',                        N'Marcas',                 N'Index',                   6,  N'fa fa-list',     100, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (32, N'Modelos',                  N'Submenú - Gestión de Modelos',                        N'Modelos',                N'Index',                   6,  N'fa fa-list',     200, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (33, N'Inventario - Productos',   N'Submenú - Maestro de productos / insumos',            N'InventarioProductos',    N'Index',                   29, N'fa fa-cubes',    300, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (34, N'Inventario - Rubros',      N'Submenú - ABM de rubros de productos',                N'InventarioRubros',       N'Index',                   29, N'fa fa-tags',     310, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (35, N'Inventario - Categorías',  N'Submenú - ABM de categorías de productos',            N'InventarioCategorias',   N'Index',                   29, N'fa fa-folder',   320, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT)),
        (36, N'Inventario - Subcategorías',N'Submenú - ABM de subcategorías de productos',        N'InventarioSubcategorias',N'Index',                   29, N'fa fa-sitemap',  330, CAST(1 AS BIT), NULL, NULL, NULL, NULL, NULL, NULL, CAST(1 AS BIT))
) AS Source (
    NewAccId,
    NewAccNombre,
    NewAccDescripcion,
    NewAccController,
    NewAccAccion,
    NewAccIdPadre,
    NewAccIcono,
    NewAccOrden,
    NewAccMenu,
    NewAccFecBaja,
    NewAccFecAlta,
    NewAccFecMod,
    NewAccUsuIdAlta,
    NewAccUsuIdMod,
    NewAccUsuIdBaja,
    NewAccActivo
)
ON Target.[acc_id] = Source.NewAccId

WHEN MATCHED THEN
    UPDATE SET
        Target.[acc_nombre]      = Source.NewAccNombre,
        Target.[acc_descripcion] = Source.NewAccDescripcion,
        Target.[acc_controller]  = Source.NewAccController,
        Target.[acc_accion]      = Source.NewAccAccion,
        Target.[acc_id_padre]    = Source.NewAccIdPadre,
        Target.[acc_icono]       = Source.NewAccIcono,
        Target.[acc_orden]       = Source.NewAccOrden,
        Target.[acc_menu]        = Source.NewAccMenu,
        Target.[acc_fec_baja]    = Source.NewAccFecBaja,
        Target.[acc_fec_alta]    = Source.NewAccFecAlta,
        Target.[acc_fec_mod]     = Source.NewAccFecMod,
        Target.[acc_usu_id_alta] = Source.NewAccUsuIdAlta,
        Target.[acc_usu_id_mod]  = Source.NewAccUsuIdMod,
        Target.[acc_usu_id_baja] = Source.NewAccUsuIdBaja,
        Target.[acc_activo]      = Source.NewAccActivo

WHEN NOT MATCHED BY TARGET THEN
    INSERT (
        [acc_id],
        [acc_nombre],
        [acc_descripcion],
        [acc_controller],
        [acc_accion],
        [acc_id_padre],
        [acc_icono],
        [acc_orden],
        [acc_menu],
        [acc_fec_baja],
        [acc_fec_alta],
        [acc_fec_mod],
        [acc_usu_id_alta],
        [acc_usu_id_mod],
        [acc_usu_id_baja],
        [acc_activo]
    )
    VALUES (
        Source.NewAccId,
        Source.NewAccNombre,
        Source.NewAccDescripcion,
        Source.NewAccController,
        Source.NewAccAccion,
        Source.NewAccIdPadre,
        Source.NewAccIcono,
        Source.NewAccOrden,
        Source.NewAccMenu,
        Source.NewAccFecBaja,
        Source.NewAccFecAlta,
        Source.NewAccFecMod,
        Source.NewAccUsuIdAlta,
        Source.NewAccUsuIdMod,
        Source.NewAccUsuIdBaja,
        Source.NewAccActivo
    );

SET IDENTITY_INSERT [dbo].[SIS_Acciones] OFF;