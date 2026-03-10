/* Bootstrap inicial desde cero */

DECLARE @OrgID INT;
DECLARE @AdminPerfilID INT;
DECLARE @AdminUserID INT;

IF NOT EXISTS(SELECT 1 FROM SIS_Organizaciones WHERE org_nombre = 'Organización Demo')
BEGIN
    INSERT INTO SIS_Organizaciones(org_nombre, org_abreviatura, org_mail, org_activo, org_fec_alta)
    VALUES('Organización Demo', 'DEMO', 'admin@demo.local', 1, GETDATE());
END

SELECT @OrgID = org_id FROM SIS_Organizaciones WHERE org_nombre = 'Organización Demo';

SELECT @AdminPerfilID = prf_id FROM SIS_Perfiles WHERE prf_nombre IN ('Administrador', 'ADMINISTRADOR') ORDER BY prf_id;
IF @AdminPerfilID IS NULL
BEGIN
    INSERT INTO SIS_Perfiles(prf_nombre, prf_descripcion, prf_activo, prf_org_id, prf_fec_alta)
    VALUES('Administrador', 'Perfil administrador de inventario', 1, @OrgID, GETDATE());
    SET @AdminPerfilID = SCOPE_IDENTITY();
END

IF NOT EXISTS(SELECT 1 FROM SIS_Usuarios WHERE usu_login = 'admin.inventory')
BEGIN
    INSERT INTO SIS_Usuarios(usu_nombre, usu_apellido, usu_mail, usu_login, usu_password, usu_org_id, usu_prf_id, usu_activo, usu_fec_alta)
    VALUES('Admin', 'Inventario', 'admin@demo.local', 'admin.inventory', 'admin123*', @OrgID, @AdminPerfilID, 1, GETDATE());
END

SELECT @AdminUserID = usu_id FROM SIS_Usuarios WHERE usu_login = 'admin.inventory';

INSERT INTO SIS_Acciones(acc_nombre, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_descripcion)
SELECT 'Inventario - Productos', 'InventarioProductos', 'Index', 0, 'fa fa-cubes', 500, 1, 'ABM de productos de inventario'
WHERE NOT EXISTS(SELECT 1 FROM SIS_Acciones WHERE acc_controller='InventarioProductos' AND acc_accion='Index');

INSERT INTO SIS_Acciones(acc_nombre, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_descripcion)
SELECT 'Inventario - Movimientos', 'InventarioMovimientos', 'Index', 0, 'fa fa-exchange', 510, 1, 'Gestión de movimientos y kardex'
WHERE NOT EXISTS(SELECT 1 FROM SIS_Acciones WHERE acc_controller='InventarioMovimientos' AND acc_accion='Index');


INSERT INTO SIS_Acciones(acc_nombre, acc_controller, acc_accion, acc_id_padre, acc_icono, acc_orden, acc_menu, acc_descripcion)
SELECT 'Inventario - Unidades de medida', 'InventarioUnidadesMedidas', 'Index', 0, 'fa fa-balance-scale', 520, 1, 'ABM de unidades de medida'
WHERE NOT EXISTS(SELECT 1 FROM SIS_Acciones WHERE acc_controller='InventarioUnidadesMedidas' AND acc_accion='Index');

INSERT INTO SIS_Perfiles_Acciones(pac_prf_id, pac_acc_id)
SELECT @AdminPerfilID, a.acc_id
FROM SIS_Acciones a
WHERE a.acc_controller IN ('InventarioProductos', 'InventarioMovimientos', 'InventarioUnidadesMedidas')
  AND NOT EXISTS(SELECT 1 FROM SIS_Perfiles_Acciones pa WHERE pa.pac_prf_id=@AdminPerfilID AND pa.pac_acc_id=a.acc_id);

INSERT INTO Unidades_Medidas(unimed_org_id, unimed_nombre, unimed_codigo, unimed_activo, unimed_fec_alta, unimed_usu_id_alta)
SELECT @OrgID, 'Unidad', 'UN', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Unidades_Medidas WHERE unimed_org_id=@OrgID AND unimed_codigo='UN');

INSERT INTO Unidades_Medidas(unimed_org_id, unimed_nombre, unimed_codigo, unimed_activo, unimed_fec_alta, unimed_usu_id_alta)
SELECT @OrgID, 'Caja', 'CJ', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Unidades_Medidas WHERE unimed_org_id=@OrgID AND unimed_codigo='CJ');

INSERT INTO Categorias_Productos(catpro_org_id, catpro_nombre, catpro_descripcion, catpro_activo, catpro_fec_alta, catpro_usu_id_alta)
SELECT @OrgID, 'Medicamentos', 'Catálogo sanitario base', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Categorias_Productos WHERE catpro_org_id=@OrgID AND catpro_nombre='Medicamentos');

INSERT INTO Categorias_Productos(catpro_org_id, catpro_nombre, catpro_descripcion, catpro_activo, catpro_fec_alta, catpro_usu_id_alta)
SELECT @OrgID, 'Descartables', 'Insumos de uso frecuente', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Categorias_Productos WHERE catpro_org_id=@OrgID AND catpro_nombre='Descartables');

INSERT INTO Tipos_Movimientos_Inventario(timi_org_id, timi_codigo, timi_nombre, timi_signo, timi_activo, timi_fec_alta, timi_usu_id_alta)
SELECT @OrgID, 'ING_INI', 'Ingreso inicial', '+', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Tipos_Movimientos_Inventario WHERE timi_org_id=@OrgID AND timi_codigo='ING_INI');

INSERT INTO Tipos_Movimientos_Inventario(timi_org_id, timi_codigo, timi_nombre, timi_signo, timi_activo, timi_fec_alta, timi_usu_id_alta)
SELECT @OrgID, 'AJ_POS', 'Ajuste positivo', '+', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Tipos_Movimientos_Inventario WHERE timi_org_id=@OrgID AND timi_codigo='AJ_POS');

INSERT INTO Tipos_Movimientos_Inventario(timi_org_id, timi_codigo, timi_nombre, timi_signo, timi_activo, timi_fec_alta, timi_usu_id_alta)
SELECT @OrgID, 'AJ_NEG', 'Ajuste negativo', '-', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Tipos_Movimientos_Inventario WHERE timi_org_id=@OrgID AND timi_codigo='AJ_NEG');

INSERT INTO Tipos_Movimientos_Inventario(timi_org_id, timi_codigo, timi_nombre, timi_signo, timi_activo, timi_fec_alta, timi_usu_id_alta)
SELECT @OrgID, 'TRANSF', 'Transferencia entre depósitos', '-', 1, GETDATE(), @AdminUserID
WHERE NOT EXISTS(SELECT 1 FROM Tipos_Movimientos_Inventario WHERE timi_org_id=@OrgID AND timi_codigo='TRANSF');
