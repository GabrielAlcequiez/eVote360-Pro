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
- Validación: un candidato no puede ocupar más de un puesto.

---

### Persona B

#### Asignación de Dirigentes
- Módulo administrativo para vincular un usuario con rol `Dirigente` a un partido político.

#### Alianzas Políticas y Candidatos Aliados
- Sistema de solicitudes de alianza con tres estados: `En espera`, `Aceptada`, `Rechazada`.
- Lógica para permitir que un partido asigne a un candidato de un partido aliado.

---

## Fase 4: Core del Sistema
> **Modalidad: Full-Stack por módulo**

> ⚠️ **Advertencia de carga:** La tarea de Persona B en esta fase (OCR + OTP + correo + boleta inmutable) es significativamente más compleja que la de Persona A. Se recomienda revisar el balance y considerar mover alguna sub-tarea antes de iniciar.

---

### Persona A — Motor de Elecciones

#### Ciclo de Vida de la Elección
- Gestión de estados: `Pendiente` → `Activa` → `Finalizada`.
- Restricción global: una vez que la elección pasa a `Activa`, se bloquea la edición de candidatos, partidos y puestos.

#### Pantalla de Resultados
- Cálculo de porcentajes por candidato.
- Ordenamiento de resultados de mayor a menor.
- Detección y manejo de empates.
- Determinación de ganadores por puesto.

---

### Persona B — Flujo del Elector y Seguridad

#### Validación de Identidad
- Validación inicial del documento de identidad del ciudadano.
- Integración del motor OCR **Tesseract** para leer y verificar la foto de la cédula.

#### Autenticación por Código OTP
- Generación de código de 6 dígitos (capa `Shared`).
- Envío del código por correo electrónico.
- Validación con expiración de **5 minutos**.

#### Boleta y Voto
- Diseño de la boleta interactiva.
- Persistencia del voto como registro **inmutable**.

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
| Boleta y Voto | | ✅ |
