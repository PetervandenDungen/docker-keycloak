CREATE DATABASE keycloak;
GO


USE keycloak;
GO


CREATE LOGIN keycloak WITH PASSWORD = '3@s#21V4pj97YPL';
GO


CREATE USER keycloak FOR LOGIN keycloak;
GO


ALTER ROLE db_owner ADD MEMBER keycloak;
GO
