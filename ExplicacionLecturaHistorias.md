# Justificación del Diseño de DTOs en el Requerimiento RF07 (Lectura de Historias)

Este documento detalla los fundamentos arquitectónicos, de seguridad, rendimiento y buenas prácticas de diseño (DDD y REST) que justifican la utilización de **DTOs especializados y granulares** para el flujo interactivo de **Lectura de Historias (RF07)**, en lugar de utilizar un único DTO genérico (`LecturaDTO`).

---

## 1. Contexto del Problema

El sistema implementa una mecánica interactiva del tipo *"Elige tu propia aventura"*. A diferencia de un CRUD tradicional (donde una pantalla crea o edita un registro en un único paso), la lectura es un **proceso conversacional de múltiples pasos** que involucra:
1. Una solicitud inicial para comenzar la lectura de una historia publicada.
2. La entrega progresiva de capítulos (texto y opciones) a medida que el lector toma decisiones.
3. El avance nodo a nodo verificando que cada camino elegido sea válido dentro de la historia.
4. El cierre inmutable de la lectura una vez alcanzado un desenlace (`Final`), actualizando estadísticas y fechas.

Si se hubiera forzado el uso de un solo `LecturaDTO` para todas estas operaciones, se habrían originado graves inconvenientes de seguridad, sobrecarga de red y acoplamiento.

---

## 2. Razones Fundamentales del Diseño

```mermaid
flowchart TD
    subgraph Cliente["Frontend / Lector (ProyectoSecundario o API)"]
        UI1["1. Inicio de Lectura"]
        UI2["2. Pantalla de Lectura / Decisiones"]
        UI3["3. Pantalla de Desenlace"]
    end

    subgraph DTOs["Capa de Contratos (DTOs Especializados)"]
        D_Init["IniciarLecturaDTO\n(idHistoria, idUsuario)"]
        D_InitRes["LecturaIniciadaDTO\n(IdLectura, IdCapituloInicial)"]
        D_Avance["AvanzarLecturaDTO\n(idUsuario, idOrigen, idDestino)"]
        D_Paso["PasoLecturaDTO\n(CapituloLecturaDTO, LecturaFinalizada)"]
        D_Entidad["LecturaDTO\n(Consulta Administrativa Completa)"]
    end

    subgraph Backend["Lógica de Aplicación y Dominio"]
        CU_Add["AgregarLecturaCU"]
        CU_Step["ObtenerPasoLecturaCU"]
        CU_Next["AvanzarLecturaCU"]
        Dominio["Lectura / Historia / Final"]
    end

    UI1 -->|POST /api/Lectura| D_Init --> CU_Add
    CU_Add --> Dominio
    CU_Add --> D_InitRes -->|201 Created| UI1

    UI2 -->|POST /api/Lectura/{id}/Avanzar| D_Avance --> CU_Next
    CU_Next --> Dominio
    CU_Next --> D_Paso -->|200 OK| UI2

    Dominio --> D_Entidad -->|GET /api/Lectura (Admin)| UI3
```

---

### 2.1. Seguridad y Prevención de "Spoilers" (Principio de Menor Exposición)

En una historia interactiva, el misterio y la progresión dependen de que el lector **desconozca los caminos futuros y los posibles finales**:

* **Si se usara `LecturaDTO` o `HistoriaDTO`:** La serialización enviaría en un solo payload JSON todo el grafo de la historia: todos los capítulos intermedios, todos los textos y todos los desenlaces. Cualquier usuario podría abrir la consola de desarrollador del navegador (`F12` $\rightarrow$ pestaña *Network*) e inspeccionar la respuesta, arruinando la experiencia narrativa.
* **Con `CapituloLecturaDTO` y `OpcionLecturaDTO`:** El backend actúa como un guardián narrativo (*narrative gateway*). Solo entrega el texto del capítulo en el que el lector se encuentra actualmente y los textos de las opciones inmediatas, **manteniendo en total reserva el contenido de los capítulos subsiguientes y de los finales**.

---

### 2.2. Optimización de Ancho de Banda y Eliminación de Ciclos JSON

El grafo de navegación de una historia interactiva es recursivo y complejo:
* Una entidad `Historia` contiene múltiples `Capitulos`.
* Un `Intermedio` contiene múltiples `Opcion`.
* Cada `Opcion` referencia a un `CapDestino` (que es otro `Capitulo`).
* A su vez, `Lectura` referencia a `Usuario`, `Historia` y `FinalAlcanzado`.

Intentar transportar este modelo relacional completo en cada avance de página ocasionaría:
1. **Payloads inflados e ineficientes:** Transferir decenas de kilobytes innecesarios en cada clic del usuario.
2. **Riesgo de recursión infinita:** Errores de serialización JSON causados por referencias circulares (`Capitulo` $\rightarrow$ `Opcion` $\rightarrow$ `Capitulo`).
3. **DTOs planos:** [`PasoLecturaDTO`](file:///C:/Users/camil/Documents/Carpeta%20Principal/Dev/Repos/ObligatorioDW/ProyectoObligatorio/DTOs/DTOs/PasoLecturaDTO.cs) y [`CapituloLecturaDTO`](file:///C:/Users/camil/Documents/Carpeta%20Principal/Dev/Repos/ObligatorioDW/ProyectoObligatorio/DTOs/DTOs/CapituloLecturaDTO.cs) transmiten estrictamente lo que la vista necesita renderizar (título, texto del nodo actual, opciones con sus identificadores de destino y un booleano de finalización).

---

### 2.3. Principio de Responsabilidad Única (SRP) y Contratos Granulares

Cada acción del flujo de lectura tiene intenciones, validaciones y datos completamente distintos:

| DTO | Dirección | Propósito Exclusivo | Justificación de su existencia aislada |
| :--- | :---: | :--- | :--- |
| **`IniciarLecturaDTO`** | Entrada (`POST`) | Solicitar el inicio de una lectura para una historia. | Solo requiere `{ idHistoria, idUsuario }`. No tiene sentido exigirle al cliente que envíe fechas, estados o capítulos que el backend debe calcular y validar por reglas de negocio. |
| **`LecturaIniciadaDTO`** | Salida (`201 Created`) | Confirmar la creación de la lectura y proveer el punto de partida. | Devuelve `{ IdLectura, IdHistoria, TituloHistoria, IdCapituloInicial }` para que el frontend pueda orquestar inmediatamente la redirección hacia la pantalla de lectura del capítulo inicial. |
| **`AvanzarLecturaDTO`** | Entrada (`POST`) | Comunicar la decisión tomada por el lector. | Transporta la terna mínima requerida para verificar y validar la transición de estado: `{ idUsuario, idCapituloOrigen, idCapDestino }`. |
| **`PasoLecturaDTO`** | Salida (`200 OK`) | Proveer el contenido del paso actual y el estado del juego. | Envuelve el `CapituloLecturaDTO` y el indicador `LecturaFinalizada`. Permite al frontend saber si debe mostrar botones de decisión o el mensaje de desenlace. |
| **`CapituloLecturaDTO`** | Componente | Representación segura del capítulo actual. | Expone `Id`, `Tipo`, `Titulo`, `Texto` y una lista opcional de `OpcionLecturaDTO`. No expone métricas internas. |
| **`OpcionLecturaDTO`** | Componente | Representación de una alternativa de camino. | Contiene únicamente el `Texto` de la opción y el `idCapDestino`. Oculta navegaciones complejas del ORM. |
| **`LecturaDTO`** | Consulta | Mapeo tradicional de la entidad completa. | Se reserva exclusivamente para auditoría, reportes o consultas administrativas (`GET /api/Lectura`), donde sí se requiere la historia completa, usuario, `FechaInicio`, `FechaFin` y `FinalAlcanzado`. |

---

### 2.4. Encapsulamiento del Dominio y Ocultamiento de Métricas Internas

El modelo de dominio contiene propiedades e invariantes que no pertenecen al ámbito público del lector:
* **`CantLlegaron` en la entidad `Final`:** Es un contador de uso del sistema. El lector común no necesita recibir este campo mientras navega su historia; exponerlo violaría el principio de menor privilegio.
* **Claves de auditoría y relaciones EF Core:** Los DTOs evitan que el cliente web conozca la estructura interna de persistencia (`DominioContext`, claves compuestas o estrategias TPH).

---

## 3. Beneficios Obtenidos en la Implementación

1. **Desacoplamiento Front-Back:** Los modelos del frontend (`ProyectoSecundario.Models`) mapean 1:1 con estos DTOs, simplificando los controladores y vistas Razor.
2. **Claridad en la Documentación Swagger/OpenAPI:** Los endpoints de `LecturaController` exponen esquemas claros y auto-descriptivos, sin campos opcionales ambiguos ni propiedades en `null`.
3. **Mantenibilidad:** Modificar la estructura de un capítulo o agregar un metadato en la lectura no rompe los contratos de avance de lectura existentes.
