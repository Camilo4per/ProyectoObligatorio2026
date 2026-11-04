/*
	ScriptObligatorio.sql
	Datos de prueba para ProyectoObligatorio.
	SQL Server / .NET 10 - EF Core.

	Ejecutar únicamente en una base de datos de pruebas: el script elimina
	las tablas del modelo antes de crearlas nuevamente.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

USE [ObligatorioDB];
GO

SELECT
	@@SERVERNAME AS Servidor,
	DB_NAME() AS BaseDeDatos;
GO

BEGIN TRANSACTION;

DECLARE @sql nvarchar(max) = N'';
SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + N'.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
			  N' DROP CONSTRAINT ' + QUOTENAME(name) + N';' + CHAR(13)
FROM sys.foreign_keys
WHERE parent_object_id IN
(
	OBJECT_ID(N'dbo.Auditorias'), OBJECT_ID(N'dbo.Lecturas'),
	OBJECT_ID(N'dbo.Opcion'), OBJECT_ID(N'dbo.CategoriaHistoria'),
	OBJECT_ID(N'dbo.Categorias'), OBJECT_ID(N'dbo.Historias'),
	OBJECT_ID(N'dbo.Capitulos')
);
IF @sql <> N'' EXEC sp_executesql @sql;

DROP TABLE IF EXISTS [Auditorias];
DROP TABLE IF EXISTS [Lecturas];
DROP TABLE IF EXISTS [Opcion];
DROP TABLE IF EXISTS [CategoriaHistoria];
DROP TABLE IF EXISTS [Categorias];
DROP TABLE IF EXISTS [Historias];
DROP TABLE IF EXISTS [Capitulos];
DROP TABLE IF EXISTS [Usuarios];

CREATE TABLE [Usuarios]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[NombreUsuario] nvarchar(max) NOT NULL,
	[Rol] int NOT NULL,
	[NombreCompleto_Nombre] nvarchar(max) NOT NULL,
	[NombreCompleto_Apellido] nvarchar(max) NOT NULL,
	[Email_Value] nvarchar(max) NOT NULL,
	[Password_Value] nvarchar(max) NOT NULL,
	CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
);

CREATE TABLE [Categorias]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Nombre_Valor] nvarchar(max) NOT NULL,
	CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);

CREATE TABLE [Historias]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[idCapituloInicial] int NULL,
	[Estado] int NOT NULL,
	[Titulo_Valor] nvarchar(max) NOT NULL,
	[Sinopsis_Valor] nvarchar(max) NOT NULL,
	CONSTRAINT [PK_Historias] PRIMARY KEY ([Id])
);

CREATE TABLE [Capitulos]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Titulo_Valor] nvarchar(max) NOT NULL,
	[Texto_Valor] nvarchar(max) NOT NULL,
	[idHistoria] int NULL,
	[Discriminator] nvarchar(13) NOT NULL,
	[CantLlegaron_Valor] int NULL,
	CONSTRAINT [PK_Capitulos] PRIMARY KEY ([Id]),
	CONSTRAINT [CK_Capitulos_FinalCantidad] CHECK
		([Discriminator] <> N'Final' OR [CantLlegaron_Valor] IS NOT NULL)
);

CREATE TABLE [Opcion]
(
	[idCapitulo] int NOT NULL,
	[Id] int IDENTITY(1,1) NOT NULL,
	[idCapDestino] int NULL,
	[Texto] nvarchar(max) NOT NULL,
	CONSTRAINT [PK_Opcion] PRIMARY KEY ([idCapitulo], [Id])
);

CREATE TABLE [CategoriaHistoria]
(
	[CategoriasId] int NOT NULL,
	[HistoriasId] int NOT NULL,
	CONSTRAINT [PK_CategoriaHistoria] PRIMARY KEY ([CategoriasId], [HistoriasId])
);

CREATE TABLE [Lecturas]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[idUsuario] int NULL,
	[idHistoria] int NULL,
	[FechaInicio] datetime2 NOT NULL,
	[FechaFin] datetime2 NULL,
	[idFinal] int NULL,
	CONSTRAINT [PK_Lecturas] PRIMARY KEY ([Id])
);

CREATE TABLE [Auditorias]
(
	[Id] int IDENTITY(1,1) NOT NULL,
	[Fecha] datetime2 NOT NULL,
	[Accion] nvarchar(max) NOT NULL,
	[idUsuario] int NULL,
	[idHistoria] int NULL,
	CONSTRAINT [PK_Auditorias] PRIMARY KEY ([Id])
);

ALTER TABLE [Historias] ADD CONSTRAINT [FK_Historias_Capitulos_idCapituloInicial]
	FOREIGN KEY ([idCapituloInicial]) REFERENCES [Capitulos] ([Id]);
ALTER TABLE [Capitulos] ADD CONSTRAINT [FK_Capitulos_Historias_idHistoria]
	FOREIGN KEY ([idHistoria]) REFERENCES [Historias] ([Id]) ON DELETE CASCADE;
ALTER TABLE [Opcion] ADD CONSTRAINT [FK_Opcion_Capitulos_idCapitulo]
	FOREIGN KEY ([idCapitulo]) REFERENCES [Capitulos] ([Id]) ON DELETE CASCADE;
ALTER TABLE [Opcion] ADD CONSTRAINT [FK_Opcion_Capitulos_idCapDestino]
	FOREIGN KEY ([idCapDestino]) REFERENCES [Capitulos] ([Id]);
ALTER TABLE [CategoriaHistoria] ADD CONSTRAINT [FK_CategoriaHistoria_Categorias_CategoriasId]
	FOREIGN KEY ([CategoriasId]) REFERENCES [Categorias] ([Id]) ON DELETE CASCADE;
ALTER TABLE [CategoriaHistoria] ADD CONSTRAINT [FK_CategoriaHistoria_Historias_HistoriasId]
	FOREIGN KEY ([HistoriasId]) REFERENCES [Historias] ([Id]) ON DELETE CASCADE;
ALTER TABLE [Lecturas] ADD CONSTRAINT [FK_Lecturas_Usuarios_idUsuario]
	FOREIGN KEY ([idUsuario]) REFERENCES [Usuarios] ([Id]);
ALTER TABLE [Lecturas] ADD CONSTRAINT [FK_Lecturas_Historias_idHistoria]
	FOREIGN KEY ([idHistoria]) REFERENCES [Historias] ([Id]);
ALTER TABLE [Lecturas] ADD CONSTRAINT [FK_Lecturas_Capitulos_idFinal]
	FOREIGN KEY ([idFinal]) REFERENCES [Capitulos] ([Id]);
ALTER TABLE [Auditorias] ADD CONSTRAINT [FK_Auditorias_Usuarios_idUsuario]
	FOREIGN KEY ([idUsuario]) REFERENCES [Usuarios] ([Id]);
ALTER TABLE [Auditorias] ADD CONSTRAINT [FK_Auditorias_Historias_idHistoria]
	FOREIGN KEY ([idHistoria]) REFERENCES [Historias] ([Id]);

SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] ([Id], [NombreUsuario], [Rol], [NombreCompleto_Nombre], [NombreCompleto_Apellido], [Email_Value], [Password_Value]) VALUES
(1, N'admin01', 0, N'Ana', N'Administrador', N'ana.admin@obligatorio.test', N'HashDemo_Admin01'),
(2, N'lector01', 1, N'Bruno', N'Silva', N'bruno.silva@obligatorio.test', N'HashDemo_lector01'),
(3, N'lector02', 1, N'Carla', N'Pereira', N'carla.pereira@obligatorio.test', N'HashDemo_lector02'),
(4, N'lector03', 1, N'Diego', N'Rodriguez', N'diego.rodriguez@obligatorio.test', N'HashDemo_lector03'),
(5, N'lector04', 1, N'Elena', N'Costa', N'elena.costa@obligatorio.test', N'HashDemo_lector04'),
(6, N'lector05', 1, N'Fabian', N'Vera', N'fabian.vera@obligatorio.test', N'HashDemo_lector05'),
(7, N'lector06', 1, N'Gabriela', N'Nuñez', N'gabriela.nunez@obligatorio.test', N'HashDemo_lector06'),
(8, N'lector07', 1, N'Hugo', N'Acosta', N'hugo.acosta@obligatorio.test', N'HashDemo_lector07'),
(9, N'lector08', 1, N'Irene', N'Mendez', N'irene.mendez@obligatorio.test', N'HashDemo_lector08'),
(10, N'lector09', 1, N'Julian', N'Perez', N'julian.perez@obligatorio.test', N'HashDemo_lector09');
SET IDENTITY_INSERT [Usuarios] OFF;

SET IDENTITY_INSERT [Categorias] ON;
INSERT INTO [Categorias] ([Id], [Nombre_Valor]) VALUES
(1, N'Aventura'), (2, N'Ciencia ficción'), (3, N'Misterio'), (4, N'Fantasía'),
(5, N'Terror'), (6, N'Romance'), (7, N'Historia'), (8, N'Comedia'),
(9, N'Acción'), (10, N'Drama');
SET IDENTITY_INSERT [Categorias] OFF;

SET IDENTITY_INSERT [Historias] ON;
INSERT INTO [Historias] ([Id], [idCapituloInicial], [Estado], [Titulo_Valor], [Sinopsis_Valor]) VALUES
(1, NULL, 1, N'La brújula de las mareas', N'Una tripulación busca una isla que aparece una vez cada cien años.'),
(2, NULL, 1, N'El último faro', N'Una guardiana debe decidir qué sacrificar para salvar la costa.'),
(3, NULL, 1, N'Código Aurora', N'Una programadora descubre una señal inteligente bajo el hielo.'),
(4, NULL, 1, N'El bosque de cristal', N'Dos hermanos atraviesan un bosque que cambia según sus decisiones.'),
(5, NULL, 1, N'La estación vacía', N'Un detective investiga un tren que llega sin pasajeros.'),
(6, NULL, 1, N'Cartas del tiempo', N'Una carta del futuro altera la vida de una historiadora.'),
(7, NULL, 1, N'El pacto del dragón', N'Una aprendiz negocia el destino de dos reinos enfrentados.'),
(8, NULL, 1, N'La ciudad sumergida', N'Un equipo explora las ruinas de una ciudad bajo el océano.'),
(9, NULL, 1, N'La máscara de cobre', N'Un músico debe descubrir quién controla el teatro de la ciudad.'),
(10, NULL, 1, N'El jardín de las sombras', N'Una botánica encuentra una planta que conserva recuerdos.');
SET IDENTITY_INSERT [Historias] OFF;

SET IDENTITY_INSERT [Capitulos] ON;
INSERT INTO [Capitulos] ([Id], [Titulo_Valor], [Texto_Valor], [idHistoria], [Discriminator], [CantLlegaron_Valor]) VALUES
(1, N'El mapa roto', N'El mapa señala dos rutas hacia la isla.', 1, N'Intermedio', NULL),
(2, N'La ruta del arrecife', N'El barco atraviesa el arrecife y encuentra la isla.', 1, N'Intermedio', NULL),
(3, N'Final: tesoro compartido', N'La tripulación comparte el tesoro con su pueblo.', 1, N'Final', 7),
(4, N'Final: tormenta eterna', N'La tormenta impide el regreso, pero la tripulación sobrevive.', 1, N'Final', 3),
(5, N'La luz del faro', N'La guardiana escucha dos pedidos de auxilio.', 2, N'Intermedio', NULL),
(6, N'El puerto seguro', N'La señal conduce a los barcos hacia un puerto protegido.', 2, N'Intermedio', NULL),
(7, N'Final: noche salvada', N'El faro vuelve a iluminar toda la costa.', 2, N'Final', 11),
(8, N'Final: promesa de vigilia', N'La guardiana mantiene encendida una pequeña luz.', 2, N'Final', 5),
(9, N'La señal bajo el hielo', N'El equipo debe elegir entre responder o investigar.', 3, N'Intermedio', NULL),
(10, N'La cámara azul', N'Una cámara contiene la fuente de la señal.', 3, N'Intermedio', NULL),
(11, N'Final: primer contacto', N'La humanidad responde a la inteligencia de Aurora.', 3, N'Final', 13),
(12, N'Final: silencio seguro', N'La señal queda aislada para evitar una guerra.', 3, N'Final', 6),
(13, N'La entrada de vidrio', N'El bosque ofrece un sendero luminoso y otro oscuro.', 4, N'Intermedio', NULL),
(14, N'La copa del árbol', N'Desde lo alto se ve el destino de ambos hermanos.', 4, N'Intermedio', NULL),
(15, N'Final: guardianes del bosque', N'Los hermanos aceptan proteger el bosque.', 4, N'Final', 8),
(16, N'Final: regreso a casa', N'Los hermanos rompen el hechizo y regresan.', 4, N'Final', 4),
(17, N'El andén sin reloj', N'El detective encuentra dos boletos con destinos opuestos.', 5, N'Intermedio', NULL),
(18, N'El vagón siete', N'El tren guarda una pista sobre su origen.', 5, N'Intermedio', NULL),
(19, N'Final: caso resuelto', N'El misterio se revela antes del amanecer.', 5, N'Final', 9),
(20, N'Final: viaje sin retorno', N'El detective aborda el tren y desaparece.', 5, N'Final', 2),
(21, N'La primera carta', N'La historiadora recibe una advertencia fechada mañana.', 6, N'Intermedio', NULL),
(22, N'El archivo sellado', N'El archivo contiene dos versiones de la historia.', 6, N'Intermedio', NULL),
(23, N'Final: historia corregida', N'La carta evita una injusticia histórica.', 6, N'Final', 10),
(24, N'Final: ciclo cerrado', N'La historiadora envía la carta a su propio pasado.', 6, N'Final', 7),
(25, N'La negociación', N'La aprendiz recibe una oferta del dragón.', 7, N'Intermedio', NULL),
(26, N'El puente de ceniza', N'El reino exige una prueba antes de firmar la paz.', 7, N'Intermedio', NULL),
(27, N'Final: paz de escamas', N'Los reinos firman un tratado duradero.', 7, N'Final', 12),
(28, N'Final: corona de fuego', N'La aprendiz gobierna el reino del dragón.', 7, N'Final', 3),
(29, N'El domo hundido', N'La expedición encuentra dos entradas a la ciudad.', 8, N'Intermedio', NULL),
(30, N'La biblioteca de coral', N'Los libros revelan por qué la ciudad desapareció.', 8, N'Intermedio', NULL),
(31, N'Final: ciudad despierta', N'La expedición activa las antiguas máquinas.', 8, N'Final', 6),
(32, N'Final: secreto preservado', N'El equipo protege la ciudad del mundo exterior.', 8, N'Final', 9),
(33, N'El telón cerrado', N'El músico encuentra una máscara detrás del escenario.', 9, N'Intermedio', NULL),
(34, N'El palco vacío', N'La máscara muestra dos posibles culpables.', 9, N'Intermedio', NULL),
(35, N'Final: verdad en escena', N'El público descubre la conspiración.', 9, N'Final', 5),
(36, N'Final: nueva función', N'El músico toma el control del teatro.', 9, N'Final', 8),
(37, N'La semilla negra', N'La botánica escucha recuerdos en las hojas.', 10, N'Intermedio', NULL),
(38, N'El invernadero oculto', N'El jardín ofrece conservar o liberar los recuerdos.', 10, N'Intermedio', NULL),
(39, N'Final: memoria compartida', N'La ciudad recupera su pasado.', 10, N'Final', 14),
(40, N'Final: jardín en paz', N'La botánica deja el jardín protegido.', 10, N'Final', 6);
SET IDENTITY_INSERT [Capitulos] OFF;

UPDATE [Historias] SET [idCapituloInicial] = CASE [Id]
	WHEN 1 THEN 1 WHEN 2 THEN 5 WHEN 3 THEN 9 WHEN 4 THEN 13 WHEN 5 THEN 17
	WHEN 6 THEN 21 WHEN 7 THEN 25 WHEN 8 THEN 29 WHEN 9 THEN 33 WHEN 10 THEN 37
END;

SET IDENTITY_INSERT [Opcion] ON;
INSERT INTO [Opcion] ([idCapitulo], [Id], [idCapDestino], [Texto]) VALUES
(1,1,2,N'Seguir el arrecife'), (1,2,3,N'Confiar en la brújula'),
(2,3,3,N'Compartir el hallazgo'), (2,4,4,N'Buscar refugio'),
(5,5,6,N'Seguir la señal del puerto'), (5,6,7,N'Encender el faro antiguo'),
(6,7,7,N'Guiar a todos los barcos'), (6,8,8,N'Guardar una última luz'),
(9,9,10,N'Investigar la cámara'), (9,10,11,N'Responder a la señal'),
(10,11,11,N'Abrir el núcleo'), (10,12,12,N'Cerrar el núcleo'),
(13,13,14,N'Tomar el sendero luminoso'), (13,14,15,N'Entrar en la oscuridad'),
(14,15,15,N'Proteger el bosque'), (14,16,16,N'Romper el hechizo'),
(17,17,18,N'Subir al vagón siete'), (17,18,19,N'Esperar el siguiente tren'),
(18,19,19,N'Ordenar las pistas'), (18,20,20,N'Seguir el silbato'),
(21,21,22,N'Leer el archivo'), (21,22,23,N'Cambiar la advertencia'),
(22,23,23,N'Corregir el documento'), (22,24,24,N'Cerrar el ciclo'),
(25,25,26,N'Aceptar la prueba'), (25,26,27,N'Proponer una alianza'),
(26,27,27,N'Cruzar el puente'), (26,28,28,N'Usar la corona'),
(29,29,30,N'Entrar por la biblioteca'), (29,30,31,N'Activar el domo'),
(30,31,31,N'Leer el registro'), (30,32,32,N'Proteger el secreto'),
(33,33,34,N'Investigar el palco'), (33,34,35,N'Ver la función'),
(34,35,35,N'Revelar la máscara'), (34,36,36,N'Aceptar el papel'),
(37,37,38,N'Seguir los recuerdos'), (37,38,39,N'Podar la planta'),
(38,39,39,N'Liberar la memoria'), (38,40,40,N'Proteger el jardín');
SET IDENTITY_INSERT [Opcion] OFF;

INSERT INTO [CategoriaHistoria] ([CategoriasId], [HistoriasId]) VALUES
(1,1),(2,1),(3,2),(4,2),(2,3),(9,3),(4,4),(1,4),(3,5),(5,5),
(7,6),(10,6),(4,7),(9,7),(2,8),(3,8),(8,9),(10,9),(5,10),(4,10);

SET IDENTITY_INSERT [Lecturas] ON;
INSERT INTO [Lecturas] ([Id], [idUsuario], [idHistoria], [FechaInicio], [FechaFin], [idFinal]) VALUES
(1,2,1,'2026-10-01T09:00:00','2026-10-01T09:35:00',3),
(2,3,1,'2026-10-01T10:00:00','2026-10-01T10:40:00',4),
(3,4,2,'2026-10-02T11:00:00','2026-10-02T11:30:00',7),
(4,5,2,'2026-10-02T12:00:00','2026-10-02T12:25:00',8),
(5,6,3,'2026-10-03T13:00:00','2026-10-03T13:45:00',11),
(6,7,4,'2026-10-03T14:00:00','2026-10-03T14:30:00',15),
(7,8,5,'2026-10-04T15:00:00','2026-10-04T15:20:00',19),
(8,9,6,'2026-10-04T16:00:00',NULL,NULL),
(9,10,7,'2026-10-05T17:00:00','2026-10-05T17:50:00',27),
(10,2,8,'2026-10-05T18:00:00',NULL,NULL),
(11,3,9,'2026-10-06T09:00:00','2026-10-06T09:25:00',35),
(12,4,10,'2026-10-06T10:00:00','2026-10-06T10:35:00',39);
SET IDENTITY_INSERT [Lecturas] OFF;

SET IDENTITY_INSERT [Auditorias] ON;
INSERT INTO [Auditorias] ([Id], [Fecha], [Accion], [idUsuario], [idHistoria]) VALUES
(1,'2026-09-20T08:00:00',N'Historia creada',1,1),
(2,'2026-09-20T08:05:00',N'Historia publicada',1,1),
(3,'2026-09-21T09:00:00',N'Historia creada',1,2),
(4,'2026-09-21T09:05:00',N'Historia publicada',1,2),
(5,'2026-09-22T10:00:00',N'Historia creada',1,3),
(6,'2026-09-22T10:05:00',N'Historia publicada',1,3),
(7,'2026-09-23T11:00:00',N'Historia modificada',1,4),
(8,'2026-09-24T12:00:00',N'Historia publicada',1,5),
(9,'2026-10-01T09:35:00',N'Lectura finalizada',2,1),
(10,'2026-10-03T13:45:00',N'Lectura finalizada',6,3),
(11,'2026-10-05T17:50:00',N'Lectura finalizada',10,7),
(12,'2026-10-06T10:35:00',N'Lectura finalizada',4,10);
SET IDENTITY_INSERT [Auditorias] OFF;

COMMIT TRANSACTION;

-- Verificación del requisito: mínimo 10 registros por tabla.
SELECT N'Usuarios' AS Tabla, COUNT(*) AS Registros FROM [Usuarios]
UNION ALL SELECT N'Categorias', COUNT(*) FROM [Categorias]
UNION ALL SELECT N'Historias', COUNT(*) FROM [Historias]
UNION ALL SELECT N'Capitulos', COUNT(*) FROM [Capitulos]
UNION ALL SELECT N'Opcion', COUNT(*) FROM [Opcion]
UNION ALL SELECT N'CategoriaHistoria', COUNT(*) FROM [CategoriaHistoria]
UNION ALL SELECT N'Lecturas', COUNT(*) FROM [Lecturas]
UNION ALL SELECT N'Auditorias', COUNT(*) FROM [Auditorias];

SELECT h.Id, h.Titulo_Valor, COUNT(DISTINCT o.Id) AS Opciones,
	   COUNT(DISTINCT CASE WHEN c.Discriminator = N'Final' THEN c.Id END) AS Finales
FROM [Historias] h
JOIN [Capitulos] c ON c.idHistoria = h.Id
LEFT JOIN [Opcion] o ON o.idCapitulo = c.Id
GROUP BY h.Id, h.Titulo_Valor
ORDER BY h.Id;
