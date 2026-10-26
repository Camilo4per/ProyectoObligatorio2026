-- ==========================================================================================
-- PROYECTO OBLIGATORIO
-- ENTREGA: E. Scripts SQL con los juegos de datos de prueba
-- Contenido:
--   1. Esquema DDL (Tablas, Claves Primarias, Claves Foráneas e Índices)
--   2. Datos de Precarga (Configuración inicial, Administradores, Categorías base, Historias iniciales)
--   3. Datos transaccionales derivados del uso del sistema (Usuarios lectores, Capítulos, Opciones,
--      Lecturas y Auditoría)
--   * Mínimo de 10 registros por cada una de las tablas del modelo.
-- ==========================================================================================

USE [master];
GO

-- Crear base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ObligatorioDB')
BEGIN
    CREATE DATABASE [ObligatorioDB];
END
GO

USE [ObligatorioDB];
GO

-- ==========================================================================================
-- 1. ESQUEMA DDL (CREACIÓN DE TABLAS)
-- ==========================================================================================

-- Eliminar tablas en orden inverso de dependencias si ya existen
IF OBJECT_ID(N'dbo.Auditorias', N'U') IS NOT NULL DROP TABLE dbo.Auditorias;
IF OBJECT_ID(N'dbo.Lecturas', N'U') IS NOT NULL DROP TABLE dbo.Lecturas;
IF OBJECT_ID(N'dbo.Opcion', N'U') IS NOT NULL DROP TABLE dbo.Opcion;
IF OBJECT_ID(N'dbo.CategoriaHistoria', N'U') IS NOT NULL DROP TABLE dbo.CategoriaHistoria;
IF OBJECT_ID(N'dbo.Categorias', N'U') IS NOT NULL DROP TABLE dbo.Categorias;

-- Romper dependencia circular entre Historias y Capitulos antes de dropear
IF OBJECT_ID(N'dbo.Historias', N'U') IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Historias_Capitulos_idCapituloInicial')
        ALTER TABLE dbo.Historias DROP CONSTRAINT FK_Historias_Capitulos_idCapituloInicial;
END

IF OBJECT_ID(N'dbo.Capitulos', N'U') IS NOT NULL DROP TABLE dbo.Capitulos;
IF OBJECT_ID(N'dbo.Historias', N'U') IS NOT NULL DROP TABLE dbo.Historias;
IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NOT NULL DROP TABLE dbo.Usuarios;
GO

-- Tabla: Usuarios
-- Roles: 0 = Administrador, 1 = Usuario
CREATE TABLE dbo.Usuarios (
    Id INT IDENTITY(1,1) NOT NULL,
    NombreUsuario NVARCHAR(MAX) NOT NULL,
    Rol INT NOT NULL,
    NombreCompleto_Nombre NVARCHAR(MAX) NOT NULL,
    NombreCompleto_Apellido NVARCHAR(MAX) NOT NULL,
    Email_Value NVARCHAR(MAX) NOT NULL,
    Password_Value NVARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_Usuarios PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: Categorias
CREATE TABLE dbo.Categorias (
    Id INT IDENTITY(1,1) NOT NULL,
    Nombre_Valor NVARCHAR(MAX) NOT NULL,
    CONSTRAINT PK_Categorias PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: Historias
-- Estados: 0 = Borrador, 1 = Publicada
CREATE TABLE dbo.Historias (
    Id INT IDENTITY(1,1) NOT NULL,
    Titulo_Valor NVARCHAR(MAX) NOT NULL,
    Sinopsis_Valor NVARCHAR(MAX) NOT NULL,
    Estado INT NOT NULL,
    idCapituloInicial INT NULL,
    CONSTRAINT PK_Historias PRIMARY KEY CLUSTERED (Id ASC)
);
GO

-- Tabla: Capitulos (Estrategia TPH: Discriminator = 'Intermedio' o 'Final')
CREATE TABLE dbo.Capitulos (
    Id INT IDENTITY(1,1) NOT NULL,
    Titulo_Valor NVARCHAR(MAX) NOT NULL,
    Texto_Valor NVARCHAR(MAX) NOT NULL,
    idHistoria INT NULL,
    Discriminator NVARCHAR(13) NOT NULL,
    CantLlegaron_Valor INT NULL,
    CONSTRAINT PK_Capitulos PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Capitulos_Historias_idHistoria FOREIGN KEY (idHistoria) 
        REFERENCES dbo.Historias (Id) ON DELETE CASCADE
);
GO

-- Relación circular diferida: Historia -> CapituloInicial
ALTER TABLE dbo.Historias
ADD CONSTRAINT FK_Historias_Capitulos_idCapituloInicial FOREIGN KEY (idCapituloInicial)
    REFERENCES dbo.Capitulos (Id);
GO

-- Tabla: CategoriaHistoria (Relación Muchos a Muchos)
CREATE TABLE dbo.CategoriaHistoria (
    CategoriasId INT NOT NULL,
    HistoriasId INT NOT NULL,
    CONSTRAINT PK_CategoriaHistoria PRIMARY KEY CLUSTERED (CategoriasId ASC, HistoriasId ASC),
    CONSTRAINT FK_CategoriaHistoria_Categorias_CategoriasId FOREIGN KEY (CategoriasId)
        REFERENCES dbo.Categorias (Id) ON DELETE CASCADE,
    CONSTRAINT FK_CategoriaHistoria_Historias_HistoriasId FOREIGN KEY (HistoriasId)
        REFERENCES dbo.Historias (Id) ON DELETE CASCADE
);
GO

-- Tabla: Opcion (Entidad dependiente de Capitulo Intermedio)
CREATE TABLE dbo.Opcion (
    idCapitulo INT NOT NULL,
    Id INT IDENTITY(1,1) NOT NULL,
    Texto NVARCHAR(MAX) NOT NULL,
    idCapDestino INT NULL,
    CONSTRAINT PK_Opcion PRIMARY KEY CLUSTERED (idCapitulo ASC, Id ASC),
    CONSTRAINT FK_Opcion_Capitulos_idCapitulo FOREIGN KEY (idCapitulo)
        REFERENCES dbo.Capitulos (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Opcion_Capitulos_idCapDestino FOREIGN KEY (idCapDestino)
        REFERENCES dbo.Capitulos (Id)
);
GO

-- Tabla: Lecturas
CREATE TABLE dbo.Lecturas (
    Id INT IDENTITY(1,1) NOT NULL,
    idUsuario INT NULL,
    idHistoria INT NULL,
    FechaInicio DATETIME2 NOT NULL,
    FechaFin DATETIME2 NULL,
    idFinal INT NULL,
    CONSTRAINT PK_Lecturas PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Lecturas_Usuarios_idUsuario FOREIGN KEY (idUsuario)
        REFERENCES dbo.Usuarios (Id),
    CONSTRAINT FK_Lecturas_Historias_idHistoria FOREIGN KEY (idHistoria)
        REFERENCES dbo.Historias (Id),
    CONSTRAINT FK_Lecturas_Capitulos_idFinal FOREIGN KEY (idFinal)
        REFERENCES dbo.Capitulos (Id)
);
GO

-- Tabla: Auditorias
CREATE TABLE dbo.Auditorias (
    Id INT IDENTITY(1,1) NOT NULL,
    Fecha DATETIME2 NOT NULL,
    Accion NVARCHAR(MAX) NOT NULL,
    idUsuario INT NULL,
    idHistoria INT NULL,
    CONSTRAINT PK_Auditorias PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Auditorias_Usuarios_idUsuario FOREIGN KEY (idUsuario)
        REFERENCES dbo.Usuarios (Id),
    CONSTRAINT FK_Auditorias_Historias_idHistoria FOREIGN KEY (idHistoria)
        REFERENCES dbo.Historias (Id)
);
GO

-- Índices para optimizar búsquedas y claves foráneas
CREATE NONCLUSTERED INDEX IX_Capitulos_idHistoria ON dbo.Capitulos (idHistoria ASC);
CREATE NONCLUSTERED INDEX IX_Historias_idCapituloInicial ON dbo.Historias (idCapituloInicial ASC);
CREATE NONCLUSTERED INDEX IX_CategoriaHistoria_HistoriasId ON dbo.CategoriaHistoria (HistoriasId ASC);
CREATE NONCLUSTERED INDEX IX_Opcion_idCapDestino ON dbo.Opcion (idCapDestino ASC);
CREATE NONCLUSTERED INDEX IX_Lecturas_idUsuario ON dbo.Lecturas (idUsuario ASC);
CREATE NONCLUSTERED INDEX IX_Lecturas_idHistoria ON dbo.Lecturas (idHistoria ASC);
CREATE NONCLUSTERED INDEX IX_Lecturas_idFinal ON dbo.Lecturas (idFinal ASC);
CREATE NONCLUSTERED INDEX IX_Auditorias_idUsuario ON dbo.Auditorias (idUsuario ASC);
CREATE NONCLUSTERED INDEX IX_Auditorias_idHistoria ON dbo.Auditorias (idHistoria ASC);
GO


-- ==========================================================================================
-- 2. INSERCIÓN DE JUEGOS DE DATOS DE PRUEBA (MÍNIMO 10 REGISTROS POR TABLA)
-- ==========================================================================================

-- ------------------------------------------------------------------------------------------
-- 2.1 TABLA: Usuarios (Total: 12 registros)
--     * Precargas del sistema: Administradores base
--     * Datos del uso del sistema: Lectores registrados
--     * Reglas VO: Contraseñas con minúscula, mayúscula, dígito, carácter especial y >= 8 chars
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Usuarios ON;

-- Precarga: Administradores del sistema
INSERT INTO dbo.Usuarios (Id, NombreUsuario, Rol, NombreCompleto_Nombre, NombreCompleto_Apellido, Email_Value, Password_Value) VALUES
(1, N'admin', 0, N'Administrador', N'General', N'admin@historias.com', N'Admin2026!'),
(2, N'carlos_admin', 0, N'Carlos', N'Gómez', N'carlos.admin@historias.com', N'Carlos#2026'),
(3, N'lucia_coord', 0, N'Lucía', N'Méndez', N'lucia.coord@historias.com', N'Lucia$9876');

-- Datos transaccionales: Usuarios lectores que utilizan la plataforma
INSERT INTO dbo.Usuarios (Id, NombreUsuario, Rol, NombreCompleto_Nombre, NombreCompleto_Apellido, Email_Value, Password_Value) VALUES
(4, N'juan_lector', 1, N'Juan', N'Pérez', N'juan.perez@gmail.com', N'Lector#01'),
(5, N'maria_rodriguez', 1, N'María', N'Rodríguez', N'maria.rod@outlook.com', N'Lector#02'),
(6, N'sofia_silva', 1, N'Sofía', N'Silva', N'sofia.silva@hotmail.com', N'Lector#03'),
(7, N'matias_torres', 1, N'Matías', N'Torres', N'matias.torres@yahoo.com', N'Lector#04'),
(8, N'valeria_suarez', 1, N'Valeria', N'Suárez', N'valeria.suarez@gmail.com', N'Lector#05'),
(9, N'diego_fernandez', 1, N'Diego', N'Fernández', N'diego.f@gmail.com', N'Lector#06'),
(10, N'camila_benitez', 1, N'Camila', N'Benítez', N'camila.b@gmail.com', N'Lector#07'),
(11, N'agustin_lopez', 1, N'Agustín', N'López', N'agustin.lopez@gmail.com', N'Lector#08'),
(12, N'florencia_diaz', 1, N'Florencia', N'Díaz', N'florencia.diaz@gmail.com', N'Lector#09');

SET IDENTITY_INSERT dbo.Usuarios OFF;
GO

-- ------------------------------------------------------------------------------------------
-- 2.2 TABLA: Categorias (Total: 10 registros)
--     * Precargas del sistema: Categorías oficiales del catálogo
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Categorias ON;

INSERT INTO dbo.Categorias (Id, Nombre_Valor) VALUES
(1, N'Aventura'),
(2, N'Terror'),
(3, N'Ciencia Ficción'),
(4, N'Misterio'),
(5, N'Fantasía'),
(6, N'Suspenso'),
(7, N'Distopía'),
(8, N'Histórica'),
(9, N'Policial'),
(10, N'Drama');

SET IDENTITY_INSERT dbo.Categorias OFF;
GO

-- ------------------------------------------------------------------------------------------
-- 2.3 TABLA: Historias (Total: 10 registros)
--     * Estados: 0 = Borrador, 1 = Publicada
--     * Se insertan inicialmente con idCapituloInicial = NULL para resolver dependencia circular
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Historias ON;

-- Precarga: Obras clásicas e interactivas precargadas por la administración
INSERT INTO dbo.Historias (Id, Titulo_Valor, Sinopsis_Valor, Estado, idCapituloInicial) VALUES
(1, N'El Laberinto del Minotauro', N'Adéntrate en los oscuros túneles de Creta desafiando a la mítica bestia en una aventura llena de acertijos.', 1, NULL),
(2, N'La Sombra de Blackwood Hall', N'Una herencia maldita te conduce a una mansión gótica donde cada puerta oculta secretos sobrenaturales.', 1, NULL),
(3, N'Odisea Estelar 2180', N'La nave Discovery pierde comunicación en el cinturón de asteroides y debes elegir cómo salvar a la tripulación.', 1, NULL),
(4, N'El Enigma del Reloj de Arena', N'Un detective victoriano investiga la desaparición de un renombrado alquimista londinense.', 1, NULL),
(5, N'Crónicas de Eldoria', N'El reino ha caído en desdicha; un joven explorador debe hallar los fragmentos del cristal sagrado.', 1, NULL);

-- Datos del uso del sistema: Historias creadas y administradas durante la vida del sistema
INSERT INTO dbo.Historias (Id, Titulo_Valor, Sinopsis_Valor, Estado, idCapituloInicial) VALUES
(6, N'Bajo la Niebla de Chernóbil', N'Un equipo de respuesta rápida entra a la zona de exclusión tras una anomalía electromagnética inexplicable.', 1, NULL),
(7, N'El Manuscrito de Alejandría', N'Un misterioso papiro descubierto en el desierto despierta conspiraciones y persecuciones internacionales.', 1, NULL),
(8, N'Rebelión en Neo-Tokio', N'En una megalópolis cyberpunk dominada por megacorporaciones, un hacker decide cambiar las reglas del juego.', 0, NULL),
(9, N'El Vuelo del Halcón Perdido', N'Naufragio en una isla no cartografiada durante la era dorada de la piratería en el Caribe.', 1, NULL),
(10, N'Ecos del Pasado', N'Un psicólogo forense se enfrenta a las memorias fragmentadas de un paciente sospechoso de un crimen perfecto.', 0, NULL);

SET IDENTITY_INSERT dbo.Historias OFF;
GO

-- ------------------------------------------------------------------------------------------
-- 2.4 TABLA: Capitulos (Total: 25 registros)
--     * TPH: Discriminator = 'Intermedio' (CantLlegaron = NULL) o 'Final' (CantLlegaron >= 0)
--     * Precarga y generación progresiva de caminos de las historias
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Capitulos ON;

-- Historia 1: El Laberinto del Minotauro
INSERT INTO dbo.Capitulos (Id, Titulo_Valor, Texto_Valor, idHistoria, Discriminator, CantLlegaron_Valor) VALUES
(1, N'La Entrada a Creta', N'Te encuentras ante el enorme portal de piedra del laberinto. A tu izquierda hay una antorcha encendida y a la derecha un hilo dorado.', 1, N'Intermedio', NULL),
(2, N'La Encrucijada de Huesos', N'Sigues el corredor y llegas a una bifurcación repleta de restos óseos antiguos.', 1, N'Intermedio', NULL),
(3, N'La Guarida de la Bestia', N'El rugido resuena en la oscuridad. El Minotauro aparece con su imponente hacha de doble filo.', 1, N'Intermedio', NULL),
(4, N'Victoria Heroica', N'Usando tu agilidad logras desarmar al monstruo y encuentras la salida guiado por la luz del día.', 1, N'Final', 5),
(5, N'Perdido en las Tinieblas', N'El laberinto se cierra tras de ti. Tus pasos se pierden en la eternidad subterránea.', 1, N'Final', 3);

-- Historia 2: La Sombra de Blackwood Hall
INSERT INTO dbo.Capitulos (Id, Titulo_Valor, Texto_Valor, idHistoria, Discriminator, CantLlegaron_Valor) VALUES
(6, N'Llegada a la Mansión', N'La lluvia golpea con furia los ventanales góticos de la residencia Blackwood. La puerta principal está entreabierta.', 2, N'Intermedio', NULL),
(7, N'La Biblioteca Oculta', N'Entre estanterías polvorientas hallas un diario que advierte sobre la maldición de la medianoche.', 2, N'Intermedio', NULL),
(8, N'Escape en la Tormenta', N'Huyes a través del jardín con las pruebas de lo sucedido mientras la mansión se consume en llamas.', 2, N'Final', 4),
(9, N'Atrapado en el Espejo', N'Ignoras las advertencias y tu reflejo toma el control de tu cuerpo para siempre.', 2, N'Final', 2);

-- Historia 3: Odisea Estelar 2180
INSERT INTO dbo.Capitulos (Id, Titulo_Valor, Texto_Valor, idHistoria, Discriminator, CantLlegaron_Valor) VALUES
(10, N'Alarma de Despresurización', N'Las sirenas rojas parpadean en el puente de mando. El casco ha sufrido una perforación crítica.', 3, N'Intermedio', NULL),
(11, N'Módulo de Escape', N'Debes decidir si evacuar de inmediato o intentar reiniciar el reactor de fusión.', 3, N'Intermedio', NULL),
(12, N'Aterrizaje en Titán', N'La cápsula desciende con éxito sobre la luna helada a la espera del equipo de rescate.', 3, N'Final', 3),
(13, N'Deriva Espacial', N'El reactor falla y quedas incomunicado flotando en el silencioso vacío cósmico.', 3, N'Final', 1);

-- Historia 4: El Enigma del Reloj de Arena
INSERT INTO dbo.Capitulos (Id, Titulo_Valor, Texto_Valor, idHistoria, Discriminator, CantLlegaron_Valor) VALUES
(14, N'El Taller del Alquimista', N'Frascos humeantes y un reloj de péndulo roto marcan la escena del crimen.', 4, N'Intermedio', NULL),
(15, N'La Cripta Subterránea', N'Sigues un pasadizo secreto bajo las alcantarillas de Londres hasta un laboratorio clandestino.', 4, N'Intermedio', NULL),
(16, N'Caso Resuelto', N'Desvelas el complot y recuperas la fórmula antes de que caiga en manos de la hermandad.', 4, N'Final', 2);

-- Historia 5: Crónicas de Eldoria
INSERT INTO dbo.Capitulos (Id, Titulo_Valor, Texto_Valor, idHistoria, Discriminator, CantLlegaron_Valor) VALUES
(17, N'El Bosque de los Susurros', N'Los árboles parecen comunicarse mientras buscas la reliquia perdida de Eldoria.', 5, N'Intermedio', NULL),
(18, N'El Guardián de Piedra', N'Un gólem ancestral despierta para juzgar la pureza de tu corazón.', 5, N'Final', 1);

-- Historias 6 a 10 (Capítulos iniciales e intermedios/finales)
INSERT INTO dbo.Capitulos (Id, Titulo_Valor, Texto_Valor, idHistoria, Discriminator, CantLlegaron_Valor) VALUES
(19, N'El Puesto de Avanzada', N'Los contadores Geiger aumentan su chasquido al cruzar el perímetro de exclusión.', 6, N'Intermedio', NULL),
(20, N'Refugio Seguro', N'Alcanzas el búnker subterráneo con muestras intactas de la radiación anómala.', 6, N'Final', 2),
(21, N'La Excavación', N'El viento del desierto descubre una tumba sellada desde la época de los Ptolomeos.', 7, N'Intermedio', NULL),
(22, N'El Velo Levantado', N'El contenido del papiro cambia la historia conocida de la humanidad.', 7, N'Final', 1),
(23, N'Conexión Ilegal', N'Te conectas a la red neuronal corporativa esquivando los firewalls de defensa.', 8, N'Intermedio', NULL),
(24, N'Encallados en el Arrecife', N'El barco encalla de madrugada y una playa salvaje se abre ante los supervivientes.', 9, N'Intermedio', NULL),
(25, N'La Primera Sesión', N'El paciente se sienta en silencio, sosteniendo una cinta de casete grabada en 1994.', 10, N'Intermedio', NULL);

SET IDENTITY_INSERT dbo.Capitulos OFF;
GO

-- ------------------------------------------------------------------------------------------
-- 2.5 ACTUALIZACIÓN: Vincular idCapituloInicial en Historias
-- ------------------------------------------------------------------------------------------
UPDATE dbo.Historias SET idCapituloInicial = 1 WHERE Id = 1;
UPDATE dbo.Historias SET idCapituloInicial = 6 WHERE Id = 2;
UPDATE dbo.Historias SET idCapituloInicial = 10 WHERE Id = 3;
UPDATE dbo.Historias SET idCapituloInicial = 14 WHERE Id = 4;
UPDATE dbo.Historias SET idCapituloInicial = 17 WHERE Id = 5;
UPDATE dbo.Historias SET idCapituloInicial = 19 WHERE Id = 6;
UPDATE dbo.Historias SET idCapituloInicial = 21 WHERE Id = 7;
UPDATE dbo.Historias SET idCapituloInicial = 23 WHERE Id = 8;
UPDATE dbo.Historias SET idCapituloInicial = 24 WHERE Id = 9;
UPDATE dbo.Historias SET idCapituloInicial = 25 WHERE Id = 10;
GO

-- ------------------------------------------------------------------------------------------
-- 2.6 TABLA: CategoriaHistoria (Total: 16 registros)
--     * Relación Muchos a Muchos entre Historias y Categorías
-- ------------------------------------------------------------------------------------------
INSERT INTO dbo.CategoriaHistoria (CategoriasId, HistoriasId) VALUES
(1, 1),  -- Aventura -> El Laberinto del Minotauro
(5, 1),  -- Fantasía -> El Laberinto del Minotauro
(2, 2),  -- Terror -> La Sombra de Blackwood Hall
(6, 2),  -- Suspenso -> La Sombra de Blackwood Hall
(3, 3),  -- Ciencia Ficción -> Odisea Estelar 2180
(6, 3),  -- Suspenso -> Odisea Estelar 2180
(4, 4),  -- Misterio -> El Enigma del Reloj de Arena
(8, 4),  -- Histórica -> El Enigma del Reloj de Arena
(9, 4),  -- Policial -> El Enigma del Reloj de Arena
(1, 5),  -- Aventura -> Crónicas de Eldoria
(5, 5),  -- Fantasía -> Crónicas de Eldoria
(3, 6),  -- Ciencia Ficción -> Bajo la Niebla de Chernóbil
(6, 6),  -- Suspenso -> Bajo la Niebla de Chernóbil
(4, 7),  -- Misterio -> El Manuscrito de Alejandría
(7, 8),  -- Distopía -> Rebelión en Neo-Tokio
(1, 9);  -- Aventura -> El Vuelo del Halcón Perdido
GO

-- ------------------------------------------------------------------------------------------
-- 2.7 TABLA: Opcion (Total: 18 registros)
--     * Opciones de bifurcación de los capítulos intermedios hacia otros capítulos
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Opcion ON;

-- Opciones del Capítulo 1 (Entrada de Creta)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(1, 1, N'Tomar la antorcha y avanzar por el túnel izquierdo.', 2),
(1, 2, N'Seguir el rastro del hilo dorado hacia la derecha.', 3);

-- Opciones del Capítulo 2 (Encrucijada)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(2, 3, N'Investigar los restos en busca de armas.', 3),
(2, 4, N'Correr en silencio evitando tocar las piedras sueltas.', 5);

-- Opciones del Capítulo 3 (Guarida del Minotauro)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(3, 5, N'Enfrentar a la bestia con tu escudo.', 4),
(3, 6, N'Intentar huir por la grieta estrecha.', 5);

-- Opciones del Capítulo 6 (Llegada a Blackwood)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(6, 7, N'Subir las escaleras hacia la biblioteca polvorienta.', 7),
(6, 8, N'Descender al sótano oscuro en busca de la llave.', 9);

-- Opciones del Capítulo 7 (Biblioteca)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(7, 9, N'Descifrar el conjuro protector del diario.', 8),
(7, 10, N'Tocar el gran espejo victoriano del salón.', 9);

-- Opciones del Capítulo 10 (Alarma de Despresurización)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(10, 11, N'Correr a la esclusa del módulo de evacuación.', 11),
(10, 12, N'Intentar aislar manualmente la fuga en el reactor.', 13);

-- Opciones del Capítulo 11 (Módulo de Escape)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(11, 13, N'Calibrar la trayectoria hacia la colonia de Titán.', 12),
(11, 14, N'Expulsar la cápsula sin fijar coordenadas.', 13);

-- Opciones del Capítulo 14 (Taller del Alquimista)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(14, 15, N'Examinar las manchas químicas en el suelo.', 15),
(14, 16, N'Interrogar al mayordomo en la sala contigua.', 16);

-- Opciones del Capítulo 17 (Bosque de los Susurros)
INSERT INTO dbo.Opcion (idCapitulo, Id, Texto, idCapDestino) VALUES
(17, 17, N'Seguir la melodía hasta el claro del claro de piedra.', 18),
(17, 18, N'Cruzar el río subterráneo remando con calma.', 18);

SET IDENTITY_INSERT dbo.Opcion OFF;
GO

-- ------------------------------------------------------------------------------------------
-- 2.8 TABLA: Lecturas (Total: 12 registros)
--     * Datos generados por el uso real de los usuarios en la plataforma
--     * Lecturas activas (FechaFin = NULL, idFinal = NULL) y completadas (con idFinal)
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Lecturas ON;

-- Lecturas finalizadas (con fecha de fin y capítulo final alcanzado)
INSERT INTO dbo.Lecturas (Id, idUsuario, idHistoria, FechaInicio, FechaFin, idFinal) VALUES
(1, 4, 1, '2026-10-01 10:30:00', '2026-10-01 11:15:20', 4),
(2, 5, 1, '2026-10-02 14:00:00', '2026-10-02 14:40:10', 5),
(3, 6, 2, '2026-10-03 18:20:00', '2026-10-03 19:05:00', 8),
(4, 7, 2, '2026-10-04 21:10:00', '2026-10-04 21:55:40', 9),
(5, 8, 3, '2026-10-05 09:15:00', '2026-10-05 10:00:15', 12),
(6, 9, 3, '2026-10-06 16:45:00', '2026-10-06 17:30:00', 13),
(7, 10, 4, '2026-10-07 12:00:00', '2026-10-07 12:45:30', 16),
(8, 11, 5, '2026-10-08 20:30:00', '2026-10-08 21:10:00', 18);

-- Lecturas en curso (iniciadas recientemente por usuarios, aún no han alcanzado un final)
INSERT INTO dbo.Lecturas (Id, idUsuario, idHistoria, FechaInicio, FechaFin, idFinal) VALUES
(9, 4, 2, '2026-10-14 11:00:00', NULL, NULL),
(10, 5, 3, '2026-10-14 15:20:00', NULL, NULL),
(11, 12, 1, '2026-10-15 08:45:00', NULL, NULL),
(12, 6, 4, '2026-10-15 16:10:00', NULL, NULL);

SET IDENTITY_INSERT dbo.Lecturas OFF;
GO

-- ------------------------------------------------------------------------------------------
-- 2.9 TABLA: Auditorias (Total: 12 registros)
--     * Registros de auditoría generados por acciones administrativas en el sistema
-- ------------------------------------------------------------------------------------------
SET IDENTITY_INSERT dbo.Auditorias ON;

INSERT INTO dbo.Auditorias (Id, Fecha, Accion, idUsuario, idHistoria) VALUES
(1, '2026-09-16 09:00:00', N'Creación de la historia inicial', 1, 1),
(2, '2026-09-16 09:30:00', N'Publicación de la historia tras revisión de capítulos', 1, 1),
(3, '2026-09-17 11:15:00', N'Creación de la historia y asignación de categorías', 2, 2),
(4, '2026-09-17 12:00:00', N'Publicación oficial en el catálogo interactivo', 2, 2),
(5, '2026-09-18 14:20:00', N'Creación de la historia de ciencia ficción', 3, 3),
(6, '2026-09-18 15:00:00', N'Publicación de la historia', 3, 3),
(7, '2026-09-20 10:00:00', N'Modificación de sinopsis y título de historia', 1, 4),
(8, '2026-09-20 10:45:00', N'Publicación de la historia', 1, 4),
(9, '2026-09-22 16:30:00', N'Creación de historia en modo borrador', 2, 8),
(10, '2026-09-25 18:00:00', N'Asociación de nuevas categorías temáticas', 3, 5),
(11, '2026-10-01 10:00:00', N'Creación de historia policial en borrador', 1, 10),
(12, '2026-10-10 17:30:00', N'Publicación de la historia y bifurcaciones finales', 2, 7);

SET IDENTITY_INSERT dbo.Auditorias OFF;
GO

-- ==========================================================================================
-- 3. VERIFICACIÓN DEL VOLUMEN DE DATOS POR TABLA (MÍNIMO 10 REQUERIDOS)
-- ==========================================================================================
SELECT 'Usuarios' AS Tabla, COUNT(*) AS CantidadRegistros FROM dbo.Usuarios
UNION ALL
SELECT 'Categorias', COUNT(*) FROM dbo.Categorias
UNION ALL
SELECT 'Historias', COUNT(*) FROM dbo.Historias
UNION ALL
SELECT 'Capitulos', COUNT(*) FROM dbo.Capitulos
UNION ALL
SELECT 'CategoriaHistoria', COUNT(*) FROM dbo.CategoriaHistoria
UNION ALL
SELECT 'Opcion', COUNT(*) FROM dbo.Opcion
UNION ALL
SELECT 'Lecturas', COUNT(*) FROM dbo.Lecturas
UNION ALL
SELECT 'Auditorias', COUNT(*) FROM dbo.Auditorias;
GO
