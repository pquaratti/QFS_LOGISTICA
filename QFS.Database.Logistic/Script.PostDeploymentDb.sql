/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

PRINT 'Iniciando Post-Deployment...'

IF '$(IsFirstDeploy)' = 'true'
BEGIN
    PRINT 'Insertando datos maestros...'
    
    :r .\dbo\Catalogs\DataModulos.sql
    :r .\dbo\Catalogs\DataPerfiles.sql
    :r .\dbo\Catalogs\DataAcciones.sql
    :r .\dbo\Catalogs\DataPerfilesAcciones.sql
    :r .\dbo\Catalogs\DataOrganizaciones.sql
    :r .\dbo\Catalogs\DataUsuarios.sql
    :r .\dbo\Catalogs\DataUsuariosModulos.sql
    :r .\dbo\Catalogs\DataProvincias.sql
    :r .\dbo\Catalogs\DataAreas.sql

    PRINT 'Datos maestros completados.'
END