# Plan de División del Proyecto — Enfoque Vertical Slicing

> **Metodología:** Cada integrante es dueño de su módulo de punta a punta:
> Vista (Bootstrap) → Controlador → Servicio + DTOs → Repository / Unit of Work.

---

## Fase 1: Arquitectura y Configuración
> **Modalidad: Trabajo en pareja (100% conjunto)**

Antes de dividirse, ambos integrantes configuran las bases del proyecto para evitar conflictos de integración posteriores.

| Tarea | Descripción |
|---|---|
| Crear solución .NET 9 | Inicializar la solución con la estructura Onion Architecture |
| Entity Framework Core | Configurar Code First y el contexto de base de datos |
| Repository genérico | Implementar el patrón Repository y Unit of Work base |
| Inyección de dependencias | Registrar servicios y repositorios en el contenedor de DI |

---

## Fase 2: Mantenimientos Core
> **Modalidad: Full-Stack por módulo**

### Persona A

#### Mantenimiento de Puestos Electivos
- CRUD completo de puestos electivos.
- Restricción: no se pueden modificar ni eliminar si existe una elección activa.

#### Mantenimiento de Ciudadanos
- CRUD de ciudadanos.
- El número de documento se almacena como `string` y debe ser **único**.
- Validaciones de integridad en capa de servicio.

---

### Persona B

#### Mantenimiento de Usuarios y Autenticación
- Configuración del flujo de login.
- Hash seguro de contraseñas.
- CRUD de usuarios con validación de roles: `Administrador` y `Dirigente Político`.

#### Mantenimiento de Partidos Políticos
- CRUD de partidos políticos.
- Subida, almacenamiento y visualización de imágenes (logos).

---

## Fase 3: Lógica Partidaria
> **Modalidad: Full-Stack por módulo**

### Persona A

#### Candidatos
- CRUD de candidatos.
- Asociación automática al partido del dirigente autenticado.
- Subida y gestión de fotos de candidatos.

#### Asignación de Candidato a Puesto
- Lógica para asignar candidatos propios a posiciones disponibles.
- Lógica para permitir que un partido asigne a un candidato de un partido aliado (alianza activa).
- Validación: un candidato no puede ocupar más de un puesto.

---

### Persona B

#### Asignación de Dirigentes
- Módulo administrativo para vincular un usuario con rol `Dirigente` a un partido político.

#### Alianzas Políticas
- Sistema de solicitudes de alianza con tres estados: `En espera`, `Aceptada`, `Rechazada`.
- Nota: La postulación y asignación de candidatos de partidos aliados es responsabilidad de la Persona A.

---

## Fase 4: Core del Sistema
> **Modalidad: Full-Stack por módulo**

> **Ajuste de balance:** La boleta electoral y la persistencia del voto se asignan a Persona A porque dependen directamente del motor de elecciones, la disponibilidad de puestos/candidatos y el cálculo posterior de resultados. Persona B se enfoca en el flujo de seguridad previo al voto: validación del ciudadano, OCR, OTP y correo.

---

### Persona A — Motor de Elecciones

#### Ciclo de Vida de la Elección
- Gestión de estados: `Pendiente` → `Activa` → `Finalizada`.
- Restricción global: una vez que la elección pasa a `Activa`, se bloquea la edición de candidatos, partidos y puestos.

#### Boleta y Registro de Votos
- Pantalla de puestos electivos disponibles para la elección activa.
- Pantalla de selección de candidato por puesto.
- Inclusión de la opción `Ninguno` como selección válida.
- Validación de que el ciudadano haya completado OCR y OTP antes de votar.
- Persistencia del voto como registro **inmutable**.
- Validación de una sola selección por ciudadano, elección y puesto.
- Finalización del proceso de votación cuando todos los puestos tengan selección.
- Envío del resumen de votación usando el servicio de correo provisto por Persona B.

#### Pantalla de Resultados
- Cálculo de porcentajes por candidato.
- Conteo de votos para la opción `Ninguno`.
- Ordenamiento de resultados de mayor a menor.
- Detección y manejo de empates.
- Determinación de ganadores por puesto.

---

### Persona B — Flujo del Elector y Seguridad

#### Validación de Identidad
- Validación inicial del documento de identidad del ciudadano.
- Integración del motor OCR **Tesseract** para leer y verificar la foto de la cédula.
- Marcado temporal de identidad validada para que la boleta pueda autorizar el acceso.

#### Autenticación por Código OTP
- Generación de código de 6 dígitos (capa `Shared`).
- Envío del código por correo electrónico.
- Validación con expiración de **5 minutos**.
- Marcado temporal de OTP validado para que la boleta pueda autorizar el acceso.

#### Servicio de Correo y Seguridad del Flujo
- Configuración de servicio de correo reutilizable.
- Correo de código de verificación.
- Plantilla base para correo de resumen de votación.
- Validación de acceso para impedir votar sin OCR y OTP.

---

## Contrato de Integración Fase 4

| Punto de integración | Responsable principal | Consume |
|---|---|---|
| Elección activa y estados | Persona A | Persona B |
| Validación de ciudadano/documento | Persona B | Persona A |
| OCR validado | Persona B | Persona A |
| OTP validado | Persona B | Persona A |
| Servicio de correo | Persona B | Persona A |
| Boleta, votos y finalización | Persona A | Persona B |
| Resultados electorales | Persona A | |

---

## Resumen de Responsabilidades

| Módulo | Persona A | Persona B |
|---|:---:|:---:|
| Puestos Electivos | ✅ | |
| Ciudadanos | ✅ | |
| Usuarios y Autenticación | | ✅ |
| Partidos Políticos | | ✅ |
| Candidatos | ✅ | |
| Asignación Candidato-Puesto | ✅ | |
| Asignación de Dirigentes | | ✅ |
| Alianzas Políticas | | ✅ |
| Motor de Elecciones | ✅ | |
| Resultados | ✅ | |
| Validación OCR / OTP | | ✅ |
| Servicio de Correo | | ✅ |
| Boleta y Voto | ✅ | |
