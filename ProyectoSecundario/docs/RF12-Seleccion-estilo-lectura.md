# Prompt
Quiero un plan para implementar dos estilos visuales, quiero que uses variables de session, que no se borren al logout, que haya un boton en el menu del layout para cambiar entre un estilo claro/oscuro y que documentes el paso a paso de su creacion: RF12 – Selección de estilo de lectura.

# RF12 – Selección de estilo de lectura

## 1. Objetivo

RF12 permite que el usuario seleccione entre dos estilos visuales de lectura:

- Estilo claro.
- Estilo oscuro.

La preferencia se guarda en la sesión HTTP y se aplica desde el layout compartido de `ProyectoSecundario`.

La implementación utiliza el patrón actual del proyecto, basado en controladores MVC y vistas Razor. No se agregó una entidad de base de datos ni un sistema de persistencia adicional.

## 2. Flujo funcional

### 2.1 Carga inicial

1. El usuario solicita cualquier vista que utilice `Views/Shared/_Layout.cshtml`.
2. El layout consulta la variable de sesión `EstiloLectura`.
3. Si el valor es `Oscuro`, se asigna la clase CSS `tema-oscuro` al elemento `body`.
4. Para cualquier otro valor, incluido un valor inexistente, se asigna `tema-claro`.
5. El menú muestra la acción opuesta al estilo actual:
   - En estilo claro: `Activar modo oscuro`.
   - En estilo oscuro: `Activar modo claro`.

### 2.2 Cambio de estilo

1. El usuario presiona el botón del menú.
2. El formulario envía una petición `POST` a `Auth/CambiarEstilo`.
3. El formulario incluye el token antiforgery.
4. Se envían dos valores:
   - `estilo`: el estilo al que se desea cambiar.
   - `returnUrl`: la ruta actual para volver a la página donde se realizó la acción.
5. `AuthController.CambiarEstilo` valida el estilo recibido.
6. Si el valor es `Oscuro`, se guarda `Oscuro`; cualquier otro valor se normaliza a `Claro`.
7. La preferencia se guarda en la sesión mediante `SetString`.
8. Si `returnUrl` es una URL local válida, se redirige allí. En caso contrario, se redirige a la pantalla de login.
9. El layout se vuelve a renderizar con la nueva clase CSS.

### 2.3 Persistencia durante la navegación

La variable permanece disponible mientras la sesión HTTP esté vigente. Por eso el usuario puede navegar entre historias, rankings, lecturas y el historial sin perder el estilo seleccionado.

### 2.4 Comportamiento durante logout

El logout de `AuthController` elimina únicamente las variables relacionadas con la autenticación:

- `token`.
- `Rol`.
- `NombreUsuario`.
- `IdUsuario`.

No se ejecuta `HttpContext.Session.Clear()`, ya que esa operación también eliminaría `EstiloLectura`.

Como resultado:

1. El usuario deja de estar autenticado.
2. Las opciones protegidas desaparecen del menú.
3. El estilo visual seleccionado permanece en la sesión.
4. Al volver a iniciar sesión, el estilo continúa aplicado.

La preferencia se conserva durante el logout, pero sigue dependiendo del vencimiento normal de la sesión configurado en `Program.cs`.

## 3. Implementación técnica

### 3.1 Configuración de sesión

`Program.cs` registra una caché distribuida en memoria y la sesión:

```csharp
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
```

El middleware se ejecuta después de routing y antes de autorización:

```csharp
app.UseRouting();
app.UseSession();
app.UseAuthorization();
```

### 3.2 Variables y validación

`AuthController` define una clave y los valores válidos:

```csharp
private const string ClaveEstiloLectura = "EstiloLectura";
private const string EstiloClaro = "Claro";
private const string EstiloOscuro = "Oscuro";
```

La acción de cambio solo acepta explícitamente `Oscuro`. Cualquier otro valor se convierte en `Claro`:

```csharp
string estiloNormalizado = estilo == EstiloOscuro
	? EstiloOscuro
	: EstiloClaro;
```

Esto evita que un valor recibido desde el cliente se convierta directamente en una clase CSS arbitraria.

### 3.3 Acción `CambiarEstilo`

La acción se implementa como `POST` porque modifica el estado de la sesión:

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult CambiarEstilo(string estilo, string? returnUrl)
```

Después de guardar la preferencia, `Url.IsLocalUrl` valida que el retorno no redirija a un sitio externo.

### 3.4 Aplicación en el layout

El layout interpreta la sesión y genera una clase controlada:

```razor
string estiloLectura = Context.Session.GetString("EstiloLectura") == "Oscuro"
	? "Oscuro"
	: "Claro";

string claseEstilo = estiloLectura == "Oscuro"
	? "tema-oscuro"
	: "tema-claro";
```

La clase se aplica al `body`:

```razor
<body class="@claseEstilo">
```

El botón se encuentra dentro de un formulario Razor que apunta a `Auth/CambiarEstilo` y contiene el token antiforgery.

### 3.5 Estilos CSS

`wwwroot/css/site.css` define variables para cada tema:

- Color de fondo.
- Color de superficie del navbar.
- Color del texto.
- Color de bordes.
- Color de enlaces.
- Color de controles de formulario.

Las reglas se activan mediante:

```css
body.tema-claro { }
body.tema-oscuro { }
```

También se adaptan el navbar, los enlaces, los formularios, las tablas, el footer y el botón responsive de navegación.

## 4. Archivos de la implementación

- `ProyectoSecundario/ProyectoSecundario/Program.cs`
  - Configuración y middleware de sesión existente.
- `ProyectoSecundario/ProyectoSecundario/Controllers/AuthController.cs`
  - Cambio de estilo y logout selectivo.
- `ProyectoSecundario/ProyectoSecundario/Views/Shared/_Layout.cshtml`
  - Lectura de la sesión, botón y aplicación del tema.
- `ProyectoSecundario/ProyectoSecundario/wwwroot/css/site.css`
  - Variables y reglas de los estilos claro y oscuro.
- `ProyectoSecundario/docs/RF12-Seleccion-estilo-lectura.md`
  - Documentación funcional y técnica del requisito.

## 5. Seguridad

La implementación contempla las siguientes medidas:

- El cambio de estilo utiliza `POST`.
- El formulario incluye `@Html.AntiForgeryToken()`.
- El estilo recibido se normaliza contra una lista limitada de valores.
- La URL de retorno se valida con `Url.IsLocalUrl`.
- El logout elimina los datos de autenticación sin eliminar la preferencia visual.

## 6. Criterios de aceptación

- [ ] El estilo claro es el predeterminado.
- [ ] El menú permite activar el estilo oscuro.
- [ ] El menú permite volver al estilo claro.
- [ ] El estilo se guarda en `EstiloLectura`.
- [ ] La selección se mantiene al navegar.
- [ ] El logout elimina el token y los datos del usuario.
- [ ] El logout no elimina `EstiloLectura`.
- [ ] La selección se conserva al volver a iniciar sesión.
- [ ] Los valores inválidos se convierten en estilo claro.
- [ ] El cambio utiliza antiforgery.

## 7. Pruebas manuales

### Prueba 1: Estilo predeterminado

1. Abrir la aplicación con una sesión nueva.
2. Comprobar que se visualiza el estilo claro.
3. Comprobar que el botón indica `Activar modo oscuro`.

### Prueba 2: Activación del estilo oscuro

1. Presionar `Activar modo oscuro`.
2. Comprobar el cambio de fondo, navbar, texto y controles.
3. Navegar a otra sección.
4. Comprobar que el estilo oscuro se mantiene.

### Prueba 3: Persistencia después del logout

1. Activar el estilo oscuro.
2. Presionar `Logout`.
3. Comprobar que desaparecen las opciones autenticadas.
4. Comprobar que el estilo oscuro sigue aplicado.
5. Volver a iniciar sesión.
6. Comprobar que el estilo oscuro se conserva.

### Prueba 4: Regreso al estilo claro

1. Presionar `Activar modo claro`.
2. Comprobar que la interfaz vuelve al estilo claro.
3. Navegar a otra sección.
4. Comprobar que el estilo claro se mantiene.
