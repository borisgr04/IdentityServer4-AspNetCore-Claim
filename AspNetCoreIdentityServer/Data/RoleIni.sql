INSERT INTO [AspNetCoreIdentityServer].[dbo].[AspNetRoles]
           (
           [Id]
           ,[ConcurrencyStamp]
           ,[Name]
           ,[NormalizedName])
     VALUES
           (
           'Prueba'
           ,'2003-02-11 00:00:00.000'
           ,'Prueba'
           ,'Prueba')
GO

commit;

INSERT INTO [AspNetCoreIdentityServer].[dbo].[AspNetUserRoles]
           ([UserId]
           ,[RoleId])
     VALUES
           ('4690c17a-9a54-461f-aa5a-1aacebc63c74'
           ,'Prueba')
GO
commit;



