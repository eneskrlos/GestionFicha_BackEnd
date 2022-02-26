
/****** Table [dbo].[PERSONAL]     ******/
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PERSONAL](
	[N_INTERNO] [numeric](5, 0) NOT NULL,
	[NOMBRE] [varchar](60) NULL,
	[APELLIDOS] [varchar](60) NULL,
	[USUARIO_RED] [varchar](15) NULL,
	[N_INTERNO_RESP] [numeric](5, 0) NULL,
 CONSTRAINT [PK_dbo.PERSONAL] PRIMARY KEY CLUSTERED 
(
	[N_INTERNO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PERSONAL]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PERSONAL_dbo.PERSONAL_N_INTERNO_RESP] FOREIGN KEY([N_INTERNO_RESP])
REFERENCES [dbo].[PERSONAL] ([N_INTERNO])
GO

ALTER TABLE [dbo].[PERSONAL] CHECK CONSTRAINT [FK_dbo.PERSONAL_dbo.PERSONAL_N_INTERNO_RESP]
GO
 --- Seed de PErsonal ------------------------------------------------------------------------------------
 SET IDENTITY_INSERT [dbo].[PERSONAL] ON
GO
INSERT INTO [dbo].[PERSONAL] ([N_INTERNO], [NOMBRE], [APELLIDOS], [USUARIO_RED], [N_INTERNO_RESP]) 
VALUES (N'146', N'RMtest_R1', N'RMtest_R1', N'RMtest_R1', NULL)
GO
GO
INSERT INTO [dbo].[PERSONAL] ([N_INTERNO], [NOMBRE], [APELLIDOS], [USUARIO_RED], [N_INTERNO_RESP]) 
VALUES (N'147', N'RMtest_R11', N'RMtest_R11', N'RMtest_R11', NULL)
GO
GO
INSERT INTO [dbo].[PERSONAL] ([N_INTERNO], [NOMBRE], [APELLIDOS], [USUARIO_RED], [N_INTERNO_RESP]) 
VALUES (N'149', N'RMtest_R12', N'RMtest_R12', N'RMtest_R12', NULL)
GO
GO
INSERT INTO [dbo].[PERSONAL] ([N_INTERNO], [NOMBRE], [APELLIDOS], [USUARIO_RED], [N_INTERNO_RESP]) 
VALUES (N'150', N'RMtest_Dtor', N'RMtest_Dtor', N'RMtest_Dtor', NULL)
GO
GO
INSERT INTO [dbo].[PERSONAL] ([N_INTERNO], [NOMBRE], [APELLIDOS], [USUARIO_RED], [N_INTERNO_RESP]) 
VALUES (N'148', N'RMtest_NAD1', N'RMtest_NAD1', N'RMtest_NAD1', NULL)
GO
GO
INSERT INTO [dbo].[PERSONAL] ([N_INTERNO], [NOMBRE], [APELLIDOS], [USUARIO_RED], [N_INTERNO_RESP]) 
VALUES (N'15', N'RMtest_Jorge', N'RMtest_Jorges', N'RMtest_Jorge', NULL)
GO
GO
SET IDENTITY_INSERT [dbo].[PERSONAL] OFF
GO

 ----------------------------------------------------------------------------------------------------------


/****** Table [dbo].[ADMINISTRADORES]     ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ADMINISTRADORES](
	[ID_PERSONA] [numeric](5, 0) NOT NULL,
 CONSTRAINT [PK_dbo.ADMINISTRADORES] PRIMARY KEY CLUSTERED 
(
	[ID_PERSONA] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ADMINISTRADORES]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ADMINISTRADORES_dbo.PERSONAL_ID_PERSONA] FOREIGN KEY([ID_PERSONA])
REFERENCES [dbo].[PERSONAL] ([N_INTERNO])
GO

ALTER TABLE [dbo].[ADMINISTRADORES] CHECK CONSTRAINT [FK_dbo.ADMINISTRADORES_dbo.PERSONAL_ID_PERSONA]
GO

----Seed de Administradores ---------------------------------------------------------------------------------------
INSERT INTO [dbo].[ADMINISTRADORES] ([ID_PERSONA]) VALUES (N'146')
GO
INSERT INTO [dbo].[ADMINISTRADORES] ([ID_PERSONA]) VALUES (N'147')
GO
INSERT INTO [dbo].[ADMINISTRADORES] ([ID_PERSONA]) VALUES (N'150')
GO
-------------------------------------------------------------------------------------------------------------------

/******  Table [dbo].[GESTORES]  ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GESTORES](
	[ID_PERSONA] [numeric](5, 0) NOT NULL,
 CONSTRAINT [PK_dbo.GESTORES] PRIMARY KEY CLUSTERED 
(
	[ID_PERSONA] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GESTORES]  WITH CHECK ADD  CONSTRAINT [FK_dbo.GESTORES_dbo.PERSONAL_ID_PERSONA] FOREIGN KEY([ID_PERSONA])
REFERENCES [dbo].[PERSONAL] ([N_INTERNO])
GO

ALTER TABLE [dbo].[GESTORES] CHECK CONSTRAINT [FK_dbo.GESTORES_dbo.PERSONAL_ID_PERSONA]
GO

----------Seed de Gestores---------------------------------------------------------------------------------------------
INSERT INTO [dbo].[GESTORES] ([ID_PERSONA]) VALUES (N'148')
GO
INSERT INTO [dbo].[GESTORES] ([ID_PERSONA]) VALUES (N'150')
GO
-----------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[PRODUCTO]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PRODUCTO](
	[ID_PRODUCTO] [int] IDENTITY(1,1) NOT NULL,
	[NOMBRE] [nvarchar](150) NOT NULL,
	[DESCRIPCION] [nvarchar](60) NULL,
	[PRECIO] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_dbo.PRODUCTO] PRIMARY KEY CLUSTERED 
(
	[ID_PRODUCTO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
--------------------Seed de Productos ---------------------------------------------------------------------------------
INSERT INTO [GestionFichaBD].[dbo].[PRODUCTO]
           ([NOMBRE]
           ,[DESCRIPCION]
           ,[PRECIO])
     VALUES
           ('Producto 1'
           ,'Producto 1 de muy buena calidad'
           ,8.75)
GO

INSERT INTO [GestionFichaBD].[dbo].[PRODUCTO]
           ([NOMBRE]
           ,[DESCRIPCION]
           ,[PRECIO])
     VALUES
           ('Producto 2'
           ,'Producto 2 de muy buena calidad'
           ,30.20)
GO

INSERT INTO [GestionFichaBD].[dbo].[PRODUCTO]
           ([NOMBRE]
           ,[DESCRIPCION]
           ,[PRECIO])
     VALUES
           ('Producto 3'
           ,'Producto 3 de muy buena calidad'
           ,58.06)
GO
-----------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[ORDEN]   ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ORDEN](
	[ID_ORDEN] [int] IDENTITY(1,1) NOT NULL,
	[FECHA] [datetime] NOT NULL,
	[DIRECCION] [nvarchar](max) NOT NULL,
	[CANTIDAD] [int] NOT NULL,
	[ES_GESTOR] [bit] NOT NULL,
	[ID_PRODUCTO] [int] NOT NULL,
	[N_INTERNO] [numeric](5, 0) NOT NULL,
 CONSTRAINT [PK_dbo.ORDEN] PRIMARY KEY CLUSTERED 
(
	[ID_ORDEN] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[ORDEN]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ORDEN_dbo.PERSONAL_N_INTERNO] FOREIGN KEY([N_INTERNO])
REFERENCES [dbo].[PERSONAL] ([N_INTERNO])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ORDEN] CHECK CONSTRAINT [FK_dbo.ORDEN_dbo.PERSONAL_N_INTERNO]
GO

ALTER TABLE [dbo].[ORDEN]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ORDEN_dbo.PRODUCTO_ID_PRODUCTO] FOREIGN KEY([ID_PRODUCTO])
REFERENCES [dbo].[PRODUCTO] ([ID_PRODUCTO])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ORDEN] CHECK CONSTRAINT [FK_dbo.ORDEN_dbo.PRODUCTO_ID_PRODUCTO]
GO

--------------Procedimiento  --------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[verificarOrdenUsuario] @id_orden int
AS
--  verificará si el usuario de dicha orden posee el role de gestor, 
--de ser asi actualizará el campo ES_GESTOR de la misma. Este procedimiento almacenado se ejecutará 
--desde el api una ves se realice la orden
BEGIN
  SET NOCOUNT ON
  declare 
  @id_rol int,
  @id_per int
  --busco el id del usuario atraves del id de la orden
 SET @id_per = (SELECT ORD.N_INTERNO  FROM [ORDEN] ORD where ORD.ID_ORDEN = @id_orden); 
 --pregunto si ese usuario es gestor
 IF (exists(SELECT G.ID_PERSONA FROM [GESTORES] G WHERE G.ID_PERSONA = @id_per))
 BEGIN
 --actualizo la orden
	UPDATE ORDEN SET ES_GESTOR = 1 WHERE ID_ORDEN = @id_orden
	
 END
END
------------------------------------------------------------------------------------------------------------------------