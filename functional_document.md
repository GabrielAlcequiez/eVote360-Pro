Mini proyecto: eVote360 Pro

[2026] Documento Funcional

Versión 1.0

25 de Mayo, 2026

© Itla 2026

Autopista Las Américas, Km. 27, PCSD,

La Caleta, Boca Chica 11606.

Tel. 809-738-4852

Índice

Índice

Objetivo general

Funcionalidades

Funcionalidades del elector

Funcionalidades del Administrador

Mantenimiento de Puesto electivo

Mantenimiento de Ciudadano

Mantenimiento de Partidos políticos

Mantenimiento de Usuarios
Asignación de dirigente políticos

Elecciones

Funcionalidades del Dirigente

Mantenimiento de candidatos
Alianzas políticas
Asignar candidato a puesto

Consideraciones generales

Requerimientos técnicos


Objetivo general

Desarrollar una aplicación web de votación electrónica que permita gestionar de

manera integral el ciclo completo de un proceso electoral, desde el registro y

validación de los ciudadanos habilitados para votar, hasta la conﬁguración de

elecciones, partidos políticos, puestos electivos, candidatos, alianzas políticas y

emisión de votos.

El sistema debe permitir que los electores validen su identidad mediante el número

de documento y la carga de una imagen de su cédula, la cual será procesada

mediante OCR para veriﬁcar que los datos extraídos coincidan con la información

registrada en el sistema.

Además, la plataforma debe garantizar que cada ciudadano pueda votar una sola

vez por elección activa, seleccionar candidatos por cada puesto electivo disponible y

ﬁnalizar su proceso de votación de forma segura y conﬁdencial. Al ﬁnalizar, el

sistema debe enviar al elector un resumen de las selecciones realizadas.

La aplicación también debe permitir a los administradores gestionar la conﬁguración

general del proceso electoral, incluyendo ciudadanos, usuarios, partidos políticos,

puestos electivos y elecciones. De igual forma, debe permitir a los dirigentes

políticos administrar sus candidatos, solicitar o responder alianzas políticas y

asignar candidatos a los puestos electivos correspondientes.

El sistema debe preservar la conﬁdencialidad del voto, controlar el acceso según el

rol del usuario y mantener la integridad de los resultados electorales mediante

restricciones que impidan modiﬁcar los datos principales de ciudadanos, partidos,

candidatos y puestos electivos que ya hayan participado en una elección.

Funcionalidades

Funcionalidades del elector

El sistema debe permitir que un ciudadano habilitado pueda participar en una

elección activa mediante un proceso de votación seguro, controlado y conﬁdencial.

El ﬂujo del elector inicia en la pantalla principal del sistema, donde el ciudadano

debe ingresar su número de documento de identidad. A partir de este dato, el

sistema debe validar si existe una elección activa, si el ciudadano está registrado y

activo, si ya ejerció su derecho al voto y si puede continuar con el proceso de

validación de identidad.

Para reforzar la seguridad del proceso, antes de permitir que el ciudadano acceda a

la boleta electoral, el sistema debe validar su identidad mediante OCR de su cédula

y posteriormente enviar un código de veriﬁcación al correo electrónico registrado del

ciudadano. Solo si el código ingresado es correcto y se encuentra vigente, el

ciudadano podrá continuar con la selección de candidatos.

El proceso de votación debe permitir que el elector seleccione un candidato por

cada puesto electivo disponible en la elección activa. Una vez completada la

selección de todos los puestos, el sistema debe permitir ﬁnalizar el proceso,

registrar formalmente los votos emitidos y enviar al ciudadano un resumen por

correo electrónico.

Pantalla inicial del elector

Al ingresar al sistema sin haber iniciado sesión, el usuario debe visualizar la

pantalla inicial destinada al proceso de votación del elector.

Esta pantalla debe contener un formulario para que el ciudadano pueda ingresar su

número de documento de identidad.

El formulario debe contener los siguientes campos:

Campo

Número de

documento de
identidad

Tipo de
dato

Texto /
string

Requerido

Descripción

Sí

Número de documento del

ciudadano que desea iniciar
el proceso de votación.

Debajo del campo debe existir un botón con el texto Votar.

Además, en la parte superior de esta pantalla debe existir un menú con una opción

llamada Acceder, la cual debe enviar al usuario a la pantalla de inicio de sesión para

usuarios administrativos o dirigentes políticos.

Descripción del campo

Número de documento de identidad

Representa el número de identiﬁcación del ciudadano dentro del sistema.

Este campo debe utilizarse para buscar al ciudadano registrado y validar si puede

iniciar el proceso de votación.

Ejemplo:

●  00112345678
●  40212345678

Este valor debe guardarse y compararse como texto, no como número, porque

puede contener ceros al inicio.

Validaciones al presionar el botón Votar

Al presionar el botón Votar, el sistema debe realizar las siguientes validaciones:

●  El número de documento de identidad es requerido.
●  El número de documento ingresado debe existir en el mantenimiento de

ciudadanos.

●  Debe existir una elección activa en el sistema.
●  El ciudadano debe estar activo.
●  El ciudadano no debe haber ﬁnalizado su voto en la elección activa.
●  Si el ciudadano ya votó en la elección activa, no debe permitirse continuar.

●  Si el ciudadano cumple todas las condiciones, el sistema debe enviarlo a la

pantalla de validación de identidad.

Si no existe ninguna elección activa, el sistema debe mostrar el siguiente mensaje:

“No hay ningún proceso electoral en estos momentos.”

Si el ciudadano ya ejerció su voto en la elección activa, el sistema debe mostrar el

siguiente mensaje:

“Ya ha ejercido su derecho al voto.”

Si el ciudadano está inactivo, el sistema debe mostrar el siguiente mensaje:

“Este ciudadano se encuentra inactivo y no puede participar en el

proceso de votación.”

Si el número de documento no corresponde a ningún ciudadano registrado, el

sistema debe mostrar un mensaje como:

“No existe un ciudadano registrado con este número de documento.”

Validación de identidad mediante OCR

Luego de validar el número de documento del ciudadano, el sistema debe enviar al

usuario a una pantalla de validación de identidad.

Esta pantalla permitirá que el ciudadano suba una imagen de la parte frontal de su

cédula. La imagen será procesada mediante OCR para extraer el texto del

documento y validar que el número de cédula extraído coincida con el número de

documento ingresado previamente.

El sistema debe utilizar el motor OCR Tesseract para procesar la imagen cargada y

extraer los datos relevantes del documento de identidad.

Pantalla de validación de identidad

La pantalla de validación de identidad debe mostrar un formulario con un campo

para cargar la imagen de la cédula.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Imagen de
la cédula

File / imagen  Sí

Imagen frontal de la cédula del

ciudadano. Debe ser una foto clara,

bien iluminada y donde se visualice

correctamente el número de
documento.

Debajo del campo debe existir un botón con el texto Validar identidad.

Descripción del campo

Imagen de la cédula

Representa la imagen del documento de identidad que será utilizada para validar

que el ciudadano que intenta votar corresponde con el número de documento

ingresado inicialmente.

La imagen debe cumplir las siguientes condiciones:

●  Debe mostrar únicamente la parte frontal de la cédula.
●  Debe verse con buena iluminación.
●  Debe permitir leer claramente el número de documento.
●  Debe tener un formato de imagen válido.
●  No debe estar borrosa, cortada o demasiado oscura.

Formatos recomendados:

●

●

●

.jpg

.jpeg

.png

Validaciones de la imagen de cédula

El formulario de validación de identidad debe cumplir las siguientes validaciones:

●  La imagen de la cédula es requerida.
●  El archivo cargado debe ser una imagen.
●  El archivo debe tener un formato permitido.

●  El sistema debe procesar la imagen mediante OCR.
●  El sistema debe intentar extraer el número de documento desde el texto

detectado.

●  El número de documento extraído mediante OCR debe coincidir con el

número de documento ingresado inicialmente por el ciudadano.

Si el ciudadano intenta continuar sin subir una imagen, el sistema debe mostrar un

mensaje como:

“Debe subir una imagen de su cédula para validar su identidad.”

Si el archivo cargado no es una imagen válida, el sistema debe mostrar un mensaje

como:

“El archivo seleccionado no tiene un formato de imagen válido.”

Si el OCR no puede extraer el número de documento desde la imagen, el sistema

debe mostrar un mensaje como:

“No fue posible leer correctamente el número de documento en la

imagen cargada. Por favor, suba una imagen más clara.”

Si el número de documento extraído de la imagen no coincide con el número

ingresado inicialmente, el sistema debe redirigir al ciudadano nuevamente a la

pantalla de validación de identidad y mostrar el siguiente mensaje:

“Los datos extraídos de la foto no coinciden con los datos previamente

ingresados por el elector.”

Si el número de documento coincide, el sistema debe continuar con el proceso de

veriﬁcación por correo electrónico.

Veriﬁcación por código de correo electrónico

Después de que el sistema valide correctamente la identidad del ciudadano

mediante OCR, debe generar un código de veriﬁcación temporal y enviarlo al correo

electrónico registrado del ciudadano.

El ciudadano no podrá acceder a la boleta electoral hasta que introduzca

correctamente el código recibido por correo electrónico.

Esta validación adicional permite reforzar la seguridad del proceso de votación y

asegurar que el ciudadano tenga acceso al correo registrado en el sistema.

Flujo de veriﬁcación por correo

El ﬂujo debe funcionar de la siguiente manera:

1.  El ciudadano ingresa su número de documento de identidad.

2.  El sistema valida que exista una elección activa y que el ciudadano pueda

votar.

3.  El ciudadano sube una imagen frontal de su cédula.

4.  El sistema procesa la imagen mediante OCR.

5.  El sistema valida que el número extraído por OCR coincida con el número

ingresado.

6.  El sistema genera un código de veriﬁcación temporal.

7.  El sistema envía el código al correo electrónico registrado del ciudadano.

8.  El ciudadano ingresa el código recibido.

9.  El sistema valida que el código sea correcto y que no haya expirado.

10. Si el código es válido, el sistema permite acceder a la pantalla de puestos

electivos disponibles.

Generación del código de veriﬁcación

Cuando la validación OCR sea exitosa, el sistema debe generar un código de

veriﬁcación.

El código debe cumplir las siguientes condiciones:

Elemento

Descripción

Longitud

Debe tener 6 dígitos.

Formato

Debe ser numérico.

Vigencia

El código de veriﬁcación debe tener una vigencia
de 5 minutos.

Uso

Debe poder utilizarse una sola vez.

Elemento

Descripción

Relación

Debe estar asociado al ciudadano y a la elección
activa.

Ejemplo de código:

482913

El sistema debe guardar internamente la información necesaria para validar el

código.

El sistema debe almacenar como mínimo los siguientes datos :

Campo

Descripción

CiudadanoId

Ciudadano al que pertenece el código.

EleccionId

Elección activa asociada al proceso.

Código

Código generado por el sistema.

FechaGeneracion

Fecha y hora en que se generó el código.

FechaExpiracion

Fecha y hora en que el código deja de ser
válido.

Usado

Indica si el código ya fue utilizado.

Envío del código por correo

El sistema debe enviar un correo electrónico al ciudadano con el código de

veriﬁcación generado.

El correo debe enviarse al email registrado en el mantenimiento de ciudadanos.

El mensaje del correo puede tener un contenido como el siguiente:

Asunto: Código de veriﬁcación para votar

Hola [Nombre del ciudadano],

Su código de veriﬁcación para continuar con el proceso de votación es:

[CÓDIGO]

Este código tendrá una vigencia de 5 minutos.

Si usted no inició este proceso, ignore este mensaje.

Si el ciudadano no tiene correo electrónico registrado, el sistema no debe permitir

continuar y debe mostrar un mensaje como:

“Este ciudadano no tiene un correo electrónico registrado. No es posible

continuar con la veriﬁcación de identidad.”

Si ocurre un error al enviar el correo, el sistema debe mostrar un mensaje como:

“No fue posible enviar el código de veriﬁcación. Intente nuevamente más

tarde.”

Pantalla para ingresar código de veriﬁcación

Luego de enviar el código por correo, el sistema debe redirigir al ciudadano a una

pantalla donde pueda introducir el código recibido.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Código de
veriﬁcación

Texto / string

Sí

Código temporal

enviado al correo

electrónico

registrado del
ciudadano.

Debajo del campo debe existir un botón con el texto Validar código.

Validaciones del código de veriﬁcación

El formulario de validación del código debe cumplir las siguientes validaciones:

●  El código de veriﬁcación es requerido.
●  El código debe pertenecer al ciudadano que inició el proceso.
●  El código debe pertenecer a la elección activa.
●  El código debe coincidir con el último código vigente generado.
●  El código no debe estar expirado.
●  El código no debe haber sido utilizado anteriormente.
●  Si el código es válido, debe marcarse como usado.
●  Si el código es válido, el sistema debe permitir acceder a la pantalla de

puestos electivos disponibles.

Si el ciudadano intenta validar sin escribir el código, el sistema debe mostrar un

mensaje como:

“Debe ingresar el código de veriﬁcación enviado a su correo electrónico.”

Si el código ingresado es incorrecto, el sistema debe mostrar un mensaje como:

“El código de veriﬁcación ingresado no es válido.”

Si el código expiró, el sistema debe mostrar un mensaje como:

“El código de veriﬁcación ha expirado. Solicite un nuevo código para

continuar.”

Si el código ya fue utilizado, el sistema debe mostrar un mensaje como:

“Este código de veriﬁcación ya fue utilizado.”

Pantalla de puestos electivos disponibles

Una vez validado correctamente el código enviado por correo electrónico, el sistema

debe enviar al ciudadano a la pantalla donde se muestran los puestos electivos

disponibles para votar en la elección activa.

En esta pantalla se debe mostrar un listado con todos los puestos electivos activos

que forman parte de la elección.

De cada puesto electivo se debe mostrar la siguiente información:

Campo

Descripción

Nombre del puesto

Nombre del cargo o posición disponible para votar.
Ejemplo: Diputado, Senador, Alcalde.

Cantidad de

partidos
participantes

Cantidad de partidos políticos que presentan candidatura
para ese puesto.

Cantidad de
candidatos reales

Cantidad de candidatos diferentes que aspiran a ese

puesto, sin duplicar candidatos que aparezcan por alianzas
políticas.

Estado de selección

Indica si el ciudadano ya seleccionó un candidato para ese
puesto.

Cada puesto listado debe tener una acción para seleccionar o modiﬁcar el voto

correspondiente a ese puesto, mientras el ciudadano no haya ﬁnalizado el proceso

completo de votación.

Consideración sobre partidos y candidatos

El sistema debe diferenciar entre la cantidad de partidos políticos representados y

la cantidad de candidatos reales.

Por ejemplo, un mismo candidato puede participar por varios partidos políticos

debido a alianzas. En ese caso, si el candidato “A” participa por 5 partidos

diferentes, debe contarse como 5 representaciones partidarias, pero como un solo

candidato real.

Ejemplo:

Puesto

Senador

Diputado

Partidos
participantes

Candidatos reales

20

15

5

6

Puesto

Partidos
participantes

Candidatos reales

Alcalde

10

4

Selección de candidato por puesto electivo

Cuando el ciudadano haga clic sobre un puesto electivo, el sistema debe enviarlo a

una pantalla donde podrá seleccionar el candidato de su preferencia para ese

puesto.

La pantalla debe mostrar todos los candidatos activos asociados al puesto

seleccionado dentro de la elección activa.

De cada candidato se debe mostrar la siguiente información:

Campo

Descripción

Foto del candidato

Imagen registrada del candidato.

Nombre del
candidato

Nombre y apellido del candidato.

Partido político

Partido por el cual el candidato participa para ese
puesto.

Logo del partido

Logo del partido político correspondiente.

Además, el sistema debe incluir una opción adicional llamada Ninguno, para

permitir que el ciudadano no seleccione ningún candidato especíﬁco para ese

puesto.

Formulario de votación por puesto

El formulario de votación por puesto debe contener las opciones de candidatos

disponibles para el puesto seleccionado.

El ciudadano sólo podrá seleccionar una opción.

El formulario debe contener:

Campo

Tipo de dato

Requerido

Descripción

Candidato
seleccionado

Radio button /
entero

Sí

Representa al candidato elegido

por el ciudadano para el puesto

electivo seleccionado. También

debe permitir seleccionar la
opción “Ninguno”.

Al ﬁnal del formulario debe existir un botón con el texto Guardar voto del puesto.

Validaciones al votar por un puesto

El formulario de votación por puesto debe cumplir las siguientes validaciones:

●  El ciudadano debe haber validado previamente su identidad mediante OCR.
●  El ciudadano debe haber validado correctamente el código enviado por

correo.

●  Debe existir una elección activa.
●  El puesto seleccionado debe pertenecer a la elección activa.
●  El puesto debe estar disponible para votar.
●  El ciudadano no debe haber ﬁnalizado su proceso de votación.
●  El ciudadano debe seleccionar un candidato o la opción “Ninguno”.
●  El ciudadano solo puede registrar una selección por cada puesto electivo.
●  Si el ciudadano vuelve a entrar al mismo puesto antes de ﬁnalizar la votación,

el sistema debe permitir modiﬁcar la selección anterior.

Si el ciudadano intenta enviar el formulario sin seleccionar una opción, el sistema

debe mantenerlo en la misma pantalla y mostrar el siguiente mensaje:

“Debe seleccionar un candidato antes de votar.”

Al guardar correctamente la selección de un puesto, el sistema debe redirigir al

ciudadano a la pantalla de puestos electivos disponibles.

Finalización del proceso de votación

Cuando el ciudadano haya seleccionado una opción para todos los puestos electivos

disponibles, el sistema debe permitirle ﬁnalizar el proceso de votación.

En la pantalla de puestos electivos debe existir un botón con el texto Finalizar

votación.

Al presionar este botón, el sistema debe validar que el ciudadano haya seleccionado

una opción para todos los puestos disponibles.

Validaciones para ﬁnalizar votación

Antes de ﬁnalizar el proceso, el sistema debe validar:

●  Debe existir una elección activa.
●  El ciudadano debe estar activo.
●  El ciudadano debe haber validado su identidad mediante OCR.
●  El ciudadano debe haber validado correctamente el código enviado por

correo.

●  El ciudadano no debe haber ﬁnalizado previamente su voto en la elección

activa.

●  El ciudadano debe haber seleccionado una opción para cada puesto electivo

disponible.

●  Cada selección debe corresponder a un puesto perteneciente a la elección

activa.

Si el ciudadano intenta ﬁnalizar sin haber seleccionado una opción para todos los

puestos, el sistema debe redirigirlo a la pantalla de puestos disponibles y mostrar

un mensaje indicando especíﬁcamente cuáles puestos faltan.

Ejemplo:

“Debe completar su selección para los siguientes puestos electivos:

Senador, Diputado.”

Si todas las validaciones se cumplen, el sistema debe registrar formalmente el voto

del ciudadano para la elección activa y marcar al ciudadano como que ya votó en

dicha elección.

Una vez ﬁnalizado el proceso, el ciudadano no podrá modiﬁcar sus votos.

Correo de resumen de votación

Al ﬁnalizar correctamente el proceso de votación, el sistema debe enviar un correo

electrónico al ciudadano con un resumen de las selecciones realizadas.

El resumen debe incluir:

Campo

Descripción

Nombre de la elección

Nombre del proceso electoral en el que
participó.

Fecha de la elección

Fecha registrada para la elección.

Puestos votados

Listado de puestos electivos en los que
participó.

Selección realizada

Nombre del candidato seleccionado o la opción
“Ninguno”.

Partido político

Partido correspondiente al candidato
seleccionado, si aplica.

Ejemplo de correo:

Asunto: Resumen de su participación electoral

Hola [Nombre del ciudadano],

Su proceso de votación ha sido completado correctamente.

Resumen de selección:

Elección: Elecciones Municipales 2026

Puesto: Alcalde

Selección: Juan Pérez

Partido: Partido Ejemplo

Puesto: Regidor

Selección: Ninguno

Gracias por ejercer su derecho al voto.

El correo de resumen debe enviarse únicamente después de ﬁnalizar correctamente

el proceso de votación.

Inicio de sesión desde la pantalla inicial

En la pantalla inicial del sistema, donde el elector ingresa su número de documento

de identidad, debe existir una opción de menú llamada Acceder.

Al hacer clic sobre esta opción, el sistema debe redirigir al usuario a la pantalla de

inicio de sesión.

Esta pantalla será utilizada por usuarios con roles administrativos o políticos, no por

los electores comunes.

Pantalla de inicio de sesión

La pantalla de inicio de sesión debe mostrar un formulario para que el usuario

pueda ingresar sus credenciales.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre de
usuario

Texto / string

Contraseña

Password / string

Sí

Sí

Nombre de

usuario registrado
en el sistema.

Contraseña

asociada al
usuario.

Debajo del formulario debe existir un botón con el texto Iniciar sesión.

Validaciones de inicio de sesión

El formulario de inicio de sesión debe cumplir las siguientes validaciones:

●  El nombre de usuario es requerido.
●  La contraseña es requerida.
●  Las credenciales deben coincidir con un usuario registrado.
●  El usuario debe estar activo.
●  El usuario debe tener un rol válido dentro del sistema.
●  Si el usuario tiene rol de dirigente político, debe tener un partido político

asignado.

Si el nombre de usuario o la contraseña son incorrectos, el sistema debe mostrar el

siguiente mensaje:

“Los datos de acceso son inválidos.”

Si el usuario está inactivo, el sistema debe mostrar el siguiente mensaje:

“El usuario está inactivo.”

Si el usuario tiene rol de administrador, el sistema debe redirigirlo al Home del

administrador.

Si el usuario tiene rol de dirigente político, tiene un partido político asignado y dicho

partido se encuentra activo, el sistema debe redirigirlo al Home del dirigente.

Si el usuario tiene rol de dirigente político, pero no tiene un partido político

asignado, el sistema debe redirigirlo nuevamente a la pantalla de inicio de sesión y

mostrar el siguiente mensaje:

“No tiene un partido político asignado, por lo tanto no puede iniciar

sesión. Por favor, póngase en contacto con un administrador.”

Reglas adicionales del módulo

●  El elector no necesita iniciar sesión con usuario y contraseña para votar.
●  El proceso de votación inicia con el número de documento de identidad.
●  Debe existir una elección activa para permitir el voto.
●  Un ciudadano inactivo no puede votar.
●  Un ciudadano solo puede votar una vez por elección activa.
●  La validación OCR es obligatoria antes de permitir el acceso a la boleta

electoral.

●  La veriﬁcación por código de correo es obligatoria después de validar

correctamente la cédula mediante OCR.

●  El código de veriﬁcación debe tener expiración.
●  El código de veriﬁcación debe poder utilizarse una sola vez.
●  El código de veriﬁcación debe estar asociado al ciudadano y a la elección

activa.

●  El ciudadano debe seleccionar una opción para cada puesto electivo antes de

ﬁnalizar.

●  La opción Ninguno debe estar disponible en cada puesto electivo.
●  Una vez ﬁnalizada la votación, el ciudadano no puede modiﬁcar sus

selecciones.

●  El sistema debe enviar un correo de resumen al ﬁnalizar correctamente la

votación.

●  El resumen enviado por correo no debe permitir modiﬁcar el voto ni acceder

nuevamente a la boleta.

●  El sistema debe preservar la conﬁdencialidad del voto.
●  Los usuarios administradores y dirigentes políticos deben acceder al sistema

mediante la opción Acceder.

●  El acceso a funcionalidades administrativas o políticas debe controlarse

según el rol del usuario.

Funcionalidades del Administrador

Si el usuario que inicia sesión tiene el rol Administrador, el sistema debe redirigir

automáticamente al Home del administrador.

Esta pantalla funcionará como el panel principal de administración del sistema.

Desde ella, el administrador podrá acceder a los principales módulos de

conﬁguración electoral y consultar un resumen general de las elecciones

registradas.

El Home del administrador debe permitir visualizar información resumida sobre los

procesos electorales realizados, incluyendo la cantidad de partidos participantes, la

cantidad de candidatos reales que participaron y la cantidad de ciudadanos que

completaron su proceso de votación.

Menú principal del administrador

En el Home del administrador, el sistema debe mostrar un menú de navegación con

las opciones disponibles para este rol.

El menú debe contener las siguientes opciones:

Opción del menú

Descripción

Puesto Electivo

Envía al mantenimiento de puestos electivos.

Ciudadanos

Envía al mantenimiento de ciudadanos registrados en
el sistema.

Partidos políticos

Envía al mantenimiento de partidos políticos.

Usuarios

Envía al mantenimiento de usuarios del sistema.

Asignación de
dirigentes políticos

Envía a la pantalla donde se relacionan usuarios
dirigentes con partidos políticos.

Elecciones

Envía al módulo de gestión de elecciones.

Cada opción del menú debe estar disponible únicamente para usuarios con rol

Administrador.

Si un usuario que no tiene el rol Administrador intenta acceder directamente al

Home del administrador mediante la URL, el sistema debe impedir el acceso y

redirigirlo a una pantalla de acceso denegado o a su Home correspondiente, según

aplique.

Resumen electoral por año

Además del menú principal, el Home del administrador debe mostrar una sección

para consultar el resumen de las elecciones registradas en el sistema.

Esta sección debe permitir que el administrador seleccione un año y obtenga un

listado de las elecciones realizadas durante ese período.

Formulario de consulta

El formulario de consulta debe contener los siguientes elementos:

Campo

Tipo de
dato

Requerido

Descripción

Año electoral

Select /
entero

Sí

Lista de años en los que existen
elecciones registradas en el sistema.

Obtener

Resumen
Electoral

Botón

Sí

Permite consultar las elecciones

correspondientes al año
seleccionado.

Descripción del campo

Año electoral

Representa el año por el cual el administrador desea consultar el resumen de

elecciones registradas.

El sistema debe cargar este select tomando como base los años de las elecciones

registradas en la base de datos.

Ejemplo:

●  2022
●  2023
●  2024
●  2025

Por defecto, el sistema debe seleccionar automáticamente el año más reciente

disponible.

Ejemplo:

Si existen elecciones registradas en los años:

●  2022
●  2023
●  2024

El valor seleccionado por defecto debe ser:

2024

Si no existen elecciones registradas en el sistema, el select debe mostrarse vacío o

deshabilitado, y el sistema debe mostrar un mensaje informativo como:

“No existen elecciones registradas en el sistema.”

Botón Obtener Resumen Electoral

Junto al select de año electoral, debe existir un botón con el texto Obtener

Resumen Electoral.

Al hacer clic sobre este botón, el sistema debe consultar todas las elecciones

registradas para el año seleccionado y mostrar un listado con el resumen

correspondiente.

Listado de resumen electoral

Luego de presionar el botón Obtener Resumen Electoral, el sistema debe mostrar

un listado con todas las elecciones registradas en el año seleccionado.

De cada elección se debe mostrar la siguiente información:

Campo

Nombre de la
elección

Descripción

Nombre con el que fue registrado el proceso electoral.

Fecha de realización

Fecha asignada a la elección.

Cantidad de partidos
participantes

Total de partidos políticos que participaron en la elección.

Campo

Descripción

Cantidad de

candidatos
participantes

Cantidad de

ciudadanos que
votaron

Total de candidatos reales que participaron en la elección,

sin duplicar candidatos que aparezcan por alianzas
políticas.

Total de ciudadanos que completaron correctamente su
proceso de votación en esa elección.

Consideración sobre partidos y candidatos participantes

El sistema debe diferenciar entre la cantidad de partidos políticos participantes y la

cantidad de candidatos reales que participaron en la elección.

Esto es importante porque un mismo candidato puede participar representado por

varios partidos políticos debido a alianzas políticas.

Ejemplo:

Un candidato llamado Juan Pérez puede participar como candidato a senador por

cinco partidos políticos diferentes.

En este caso:

Partidos participantes: 5

Candidatos reales: 1

Por esta razón, el sistema no debe calcular la cantidad de candidatos únicamente

contando las candidaturas por partido. Debe evitar duplicar un mismo candidato

cuando aparezca asociado a varios partidos para el mismo puesto electivo.

Ejemplo de resumen electoral

El listado puede mostrarse de la siguiente forma:

Nombre de la
elección

Fecha de
realización

Partidos
participantes

Candidatos
participantes

Ciudadanos
que votaron

Elecciones

Municipales
2024

Elecciones

Congresuales
2024

Elecciones

Presidenciales
2024

18/02/2024

12

25

10,450

19/05/2024

15

40

12,300

19/05/2024

8

8

13,100

Validaciones del resumen electoral

Al consultar el resumen electoral, el sistema debe cumplir las siguientes

validaciones:

●  El año electoral es requerido.
●  El año seleccionado debe existir dentro de los años con elecciones

registradas.

●  Solo los usuarios con rol Administrador pueden acceder a esta pantalla.
●  Si no existen elecciones registradas para el año seleccionado, el sistema

debe mostrar un mensaje informativo.

●  Si no existen elecciones registradas en el sistema, el formulario debe

indicarlo claramente.

●  El resumen debe calcularse utilizando los datos actuales del sistema,

tomando en cuenta que los campos principales de partidos, candidatos y

puestos que ya participaron en elecciones no pueden ser modiﬁcados.

Si el usuario presiona el botón sin seleccionar un año, el sistema debe mostrar un

mensaje como:

“Debe seleccionar un año para consultar el resumen electoral.”

Si no existen elecciones registradas para el año seleccionado, el sistema debe

mostrar un mensaje como:

“No existen elecciones registradas para el año seleccionado.”

Reglas de negocio del Home del administrador

●  El Home del administrador solo debe estar disponible para usuarios con rol

Administrador.

●  El menú principal debe mostrar únicamente las opciones permitidas para el

administrador.

●  El campo Año electoral debe cargarse con los años en los que existan

elecciones registradas.

●  El año seleccionado por defecto debe ser el año más reciente con elecciones

registradas.

●  El resumen electoral debe mostrar únicamente elecciones correspondientes

al año seleccionado.

●  La cantidad de partidos participantes debe representar los partidos políticos

que participaron en la elección.

●  La cantidad de candidatos participantes debe representar candidatos reales,
evitando duplicar candidatos que participen por más de un partido debido a

alianzas políticas.

●  La cantidad de ciudadanos que votaron debe representar la cantidad de

ciudadanos que ﬁnalizaron correctamente su proceso de votación.

●  La cantidad de ciudadanos que votaron no debe confundirse con la cantidad
total de votos por candidatos, ya que un ciudadano puede votar por varios

puestos electivos.

●  El sistema debe usar el mismo layout general de la aplicación para mantener

consistencia visual con los demás módulos.

Mantenimiento de Puesto electivo

Al ingresar a la opción Puesto Electivo desde el menú principal del administrador,

el sistema debe enviar al usuario al mantenimiento de puestos electivos.

Este mantenimiento permitirá registrar, consultar, editar, activar y desactivar los

puestos electivos que podrán ser disputados dentro de un proceso electoral.

Un puesto electivo representa un cargo o posición por la cual los ciudadanos podrán

votar durante una elección. Por ejemplo: Presidente, Senador, Diputado, Alcalde,

Regidor, entre otros.

Los puestos electivos activos serán utilizados al momento de crear una nueva

elección y determinarán los cargos disponibles para votación.

Pantalla inicial del mantenimiento

En la pantalla inicial del mantenimiento de puestos electivos, el sistema debe

mostrar un listado con todos los puestos electivos creados en el sistema.

De cada puesto electivo se debe mostrar la siguiente información:

Campo

Descripción

Nombre del puesto

Nombre del cargo o posición electoral. Ejemplo: Presidente,
Senador, Diputado, Alcalde.

Descripción

Explicación breve del puesto electivo.

Estado

Indica si el puesto electivo está activo o inactivo dentro del
sistema.

Cada puesto electivo listado debe tener las siguientes acciones:

Acción

Editar

Activar

Descripción

Permite modiﬁcar los datos del puesto electivo.

Permite cambiar el estado del puesto de inactivo a activo.
Solo debe mostrarse si el puesto está inactivo.

Desactivar

Permite cambiar el estado del puesto de activo a inactivo.
Solo debe mostrarse si el puesto está activo.

Arriba del listado debe existir un botón con el texto Crear puesto electivo.

Si existe una elección activa, el sistema no debe permitir crear, editar, activar ni

desactivar puestos electivos.

En ese caso, los botones de acción deben mostrarse deshabilitados y debe indicarse

visualmente que la acción no está disponible mientras exista una elección activa.

Mensaje sugerido:

“No se pueden modiﬁcar puestos electivos mientras exista una elección

activa.”

Crear puesto electivo

Al hacer clic sobre el botón Crear puesto electivo, el sistema debe enviar al usuario

a una pantalla con un formulario para registrar un nuevo puesto.

Este botón solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre del
puesto

Texto / string

Sí

Descripción

Texto / string

Sí

Representa el nombre del

cargo o posición que será
disputada en una elección.

Permite describir brevemente

el alcance o naturaleza del
puesto electivo.

Estado

Booleano /
checkbox

Indica si el puesto estará

activo o inactivo dentro del

Sí

sistema. Por defecto, al crear

un puesto debe venir
marcado como activo.

Descripción de campos

Nombre del puesto

Es el nombre con el que se identiﬁcará el cargo dentro del sistema.

Ejemplo:

●  Presidente
●  Senador

●  Diputado
●  Alcalde
●  Regidor

Este valor será mostrado en los formularios donde se asignen candidatos a puestos

electivos y también en la boleta electoral del ciudadano.

Descripción

Permite registrar una breve explicación del puesto electivo.

Ejemplo:

Cargo municipal encargado de la administración del ayuntamiento.

Cargo legislativo que representa una provincia en el Senado.

Cargo legislativo que representa una demarcación territorial en la Cámara de

Diputados.

Estado

Indica si el puesto electivo está activo o inactivo.

●  Si el puesto está activo, puede ser utilizado al crear una nueva elección.
●  Si el puesto está inactivo, no debe aparecer como opción para nuevas

elecciones ni para nuevas asignaciones de candidatos.

●  Si un puesto inactivo fue utilizado en una elección activa o ﬁnalizada, debe

seguir mostrándose en los resultados históricos de esa elección.

Validaciones para crear puesto electivo

El formulario de creación debe cumplir las siguientes validaciones:

●  El nombre del puesto es requerido.
●  La descripción es requerida.
●  El nombre del puesto no debe repetirse.
●  La validación del nombre debe ignorar espacios al inicio y al ﬁnal.
●  El estado debe manejarse como booleano.
●  Al crear un puesto electivo, el estado debe venir marcado como activo por

defecto.

●  No debe permitirse crear un puesto electivo si existe una elección activa.

Si el usuario intenta registrar un puesto con un nombre ya existente, el sistema

debe mostrar un mensaje como:

“Ya existe un puesto electivo registrado con este nombre.”

Si el usuario intenta crear un puesto mientras existe una elección activa, el sistema

debe mostrar un mensaje como:

“No se puede crear un puesto electivo mientras exista una elección

activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de puestos electivos sin guardar

cambios.

●  Crear puesto electivo: guarda el nuevo puesto en el sistema.

Al hacer clic en Crear puesto electivo, si los datos son válidos, el sistema debe

registrar el puesto en estado activo y redirigir al usuario a la pantalla inicial del

mantenimiento de puestos electivos.

Editar puesto electivo

En el listado de puestos electivos, al presionar el botón Editar, el sistema debe

enviar al usuario a una pantalla con un formulario para modiﬁcar el puesto

seleccionado.

Esta acción sólo debe estar disponible si no existe una elección activa.

El formulario debe mostrar los datos actuales del puesto electivo.

El formulario de edición debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre del
puesto

Texto / string

Sí

Nombre del cargo

o posición
electoral.

Campo

Tipo de dato

Requerido

Descripción

Descripción

Texto / string

Estado

Booleano /
checkbox

Sí

Sí

Explicación breve

del puesto
electivo.

Indica si el puesto

está activo o
inactivo.

Como es una edición, los campos deben venir cargados con los valores guardados

para el puesto seleccionado.

Validaciones para editar puesto electivo

El formulario de edición debe cumplir las siguientes validaciones:

●  El nombre del puesto es requerido.
●  La descripción es requerida.
●  El nombre del puesto no debe repetirse.
●  La validación del nombre debe ignorar espacios al inicio y al ﬁnal.
●  El estado debe manejarse como booleano.
●  No debe permitirse editar un puesto electivo si existe una elección activa.

En el caso del nombre, el sistema debe validar que no exista otro puesto electivo

diferente con el mismo nombre.

Ejemplo:

Puesto editado: Senador

Nombre actual: Senador

Si el usuario guarda el mismo nombre Senador, debe permitirse, porque pertenece

al mismo puesto que se está editando.

Pero si existe otro puesto con el nombre de Senador, no debe permitirse guardar.

Si el usuario intenta editar un puesto mientras existe una elección activa, el sistema

debe mostrar un mensaje como:

“No se puede editar un puesto electivo mientras exista una elección

activa.”

Restricciones importantes al editar

Si el puesto electivo ya fue utilizado en una elección activa o ﬁnalizada, el sistema

debe tener cuidado con los cambios que puedan afectar la interpretación histórica

de los resultados.

Para esto solo se puede editar la descripción, pero no el nombre del puesto si ya fue

utilizado en una elección.

Mensaje sugerido:

“No se puede modiﬁcar el nombre de este puesto electivo porque ya fue

utilizado en una elección.”

●  Si el puesto ya fue utilizado en una elección activa o ﬁnalizada, no se debe

permitir modiﬁcar su nombre.

●  Sí se debe permitir modiﬁcar la descripción y el estado, siempre que no exista

una elección activa.

Al ﬁnal del formulario de edición debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de puestos electivos sin guardar

cambios.

●  Guardar cambios: actualiza la información del puesto.

Al hacer clic en Guardar cambios, si los datos son válidos, el sistema debe

actualizar el puesto y redirigir al usuario a la pantalla inicial del mantenimiento.

Activar puesto electivo

Si el puesto electivo está inactivo, en el listado debe mostrarse un botón con el

texto Activar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea activar este puesto electivo?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de puestos

electivos sin realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado del puesto a activo y

redirigir al usuario nuevamente al listado de puestos electivos.

Validaciones para activar puesto electivo

Antes de activar un puesto electivo, el sistema debe validar:

●  El puesto electivo debe existir.
●  El puesto electivo debe estar actualmente inactivo.
●  No debe existir una elección activa.
●  No debe existir otro puesto activo con el mismo nombre.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede activar un puesto electivo mientras exista una elección

activa.”

Si el puesto ya está activo, el sistema debe mostrar un mensaje como:

“Este puesto electivo ya se encuentra activo.”

Desactivar puesto electivo

Si el puesto electivo está activo, en el listado debe mostrarse un botón con el texto

Desactivar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desactivar este puesto electivo?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de puestos

electivos sin realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe intentar cambiar el estado del puesto a

inactivo.

Validaciones para desactivar puesto electivo

Antes de desactivar un puesto electivo, el sistema debe validar:

●  El puesto electivo debe existir.
●  El puesto electivo debe estar actualmente activo.
●  No debe existir una elección activa.
●  El puesto no debe tener candidatos activos asignados para futuras

elecciones.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede desactivar un puesto electivo mientras exista una elección

activa.”

Si el puesto tiene candidatos activos asignados, el sistema no debe permitir

desactivarlo y debe mostrar un mensaje como:

“No se puede desactivar este puesto electivo porque tiene candidatos

activos asignados.”

Si el puesto ya está inactivo, el sistema debe mostrar un mensaje como:

“Este puesto electivo ya se encuentra inactivo.”

Si todas las validaciones se cumplen, el sistema debe cambiar el estado del puesto

a inactivo y redirigir al usuario nuevamente al listado de puestos electivos.

Reglas adicionales del mantenimiento

●  Solo los usuarios con rol Administrador pueden acceder al mantenimiento

de puestos electivos.

●  No se debe permitir crear, editar, activar ni desactivar puestos electivos

mientras exista una elección activa.

●  Los puestos electivos activos pueden ser utilizados al crear una nueva

elección.

●  Los puestos electivos inactivos no deben aparecer como opción para nuevas

elecciones ni nuevas asignaciones de candidatos.

●  Los puestos electivos inactivos sí deben mostrarse en registros históricos

donde ya fueron utilizados.

●  No debe permitirse crear dos puestos electivos con el mismo nombre.
●  El nombre del puesto debe compararse ignorando espacios al inicio y al ﬁnal.
●  Al crear un puesto electivo, debe guardarse en estado activo por defecto.
●  No se debe permitir desactivar un puesto electivo si tiene candidatos activos

asignados.

●  Si un puesto electivo ya fue utilizado en una elección activa o ﬁnalizada, no se

debe permitir modiﬁcar su nombre

●  Los resultados de elecciones ﬁnalizadas deben seguir mostrando los puestos

utilizados, aunque posteriormente hayan sido inactivados. Para evitar

inconsistencias, no se permite modiﬁcar el nombre de un puesto electivo que

ya haya sido utilizado en una elección.

●  El mantenimiento debe usar el mismo layout general de la aplicación.

Mantenimiento de Ciudadano

Al ingresar a la opción Ciudadanos desde el menú principal del administrador, el

sistema debe enviar al usuario al mantenimiento de ciudadanos.

Este mantenimiento permitirá registrar, consultar, editar, activar y desactivar los

ciudadanos que podrán participar como electores en los procesos electorales del

sistema.

Un ciudadano representa una persona habilitada para votar. Para poder participar

en una elección activa, el ciudadano debe estar registrado en el sistema,

encontrarse activo y no haber ejercido previamente su derecho al voto en la elección

activa.

El número de documento de identidad será utilizado como identiﬁcador principal del

ciudadano durante el proceso de votación y también será validado contra los datos

extraídos de la cédula mediante OCR.

Pantalla inicial del mantenimiento

En la pantalla inicial del mantenimiento de ciudadanos, el sistema debe mostrar un

listado con todos los ciudadanos creados en el sistema.

De cada ciudadano se debe mostrar la siguiente información:

Campo

Descripción

Nombre

Apellido

Nombre del ciudadano registrado.

Apellido del ciudadano registrado.

Correo electrónico

Correo electrónico asociado al ciudadano. Este correo será

utilizado para enviar el código de veriﬁcación y el resumen
de votación.

Número de

documento de
identidad

Número único que identiﬁca al ciudadano dentro del
sistema.

Estado

Indica si el ciudadano está activo o inactivo.

Cada ciudadano listado debe tener las siguientes acciones:

Acción

Descripción

Editar

Permite modiﬁcar los datos del ciudadano.

Activar

Desactivar

Permite cambiar el estado del ciudadano de inactivo a
activo. Solo debe mostrarse si el ciudadano está inactivo.

Permite cambiar el estado del ciudadano de activo a
inactivo. Solo debe mostrarse si el ciudadano está activo.

Arriba del listado debe existir un botón con el texto Crear ciudadano.

Si existe una elección activa, el sistema no debe permitir crear, editar, activar ni

desactivar ciudadanos. En ese caso, los botones de acción deben mostrarse

deshabilitados y debe indicarse visualmente que la acción no está disponible

mientras exista una elección activa.

Mensaje sugerido:

“No se pueden modiﬁcar ciudadanos mientras exista una elección

activa.”

Crear ciudadano

Al hacer clic sobre el botón Crear ciudadano, el sistema debe enviar al usuario a

una pantalla con un formulario para registrar un nuevo ciudadano.

Este botón solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre

Texto / string

Apellido

Texto / string

Sí

Sí

Representa el nombre del
ciudadano.

Representa el apellido del
ciudadano.

Correo electrónico del

ciudadano. Será utilizado

Correo electrónico  Texto / string

Sí

para enviar códigos de

veriﬁcación y resumen de
votación.

Número de

documento de
identidad

Texto / string

Sí

Número único del documento
de identidad del ciudadano.

Estado

Booleano /
checkbox

Sí

Indica si el ciudadano estará

activo o inactivo. Por defecto,

Campo

Tipo de dato

Requerido

Descripción

al crear un ciudadano debe
venir marcado como activo.

Descripción de campos

Nombre

Representa el nombre del ciudadano que será registrado en el sistema.

Ejemplo:

●  Juan
●  María
●  Carlos
●  Ana

Apellido

Representa el apellido del ciudadano.

Ejemplo:

●  Pérez
●  Rodríguez
●  Gómez
●  Martínez

Correo electrónico

Representa el correo electrónico asociado al ciudadano.

Este correo será utilizado para:

●  Enviar el código de veriﬁcación después de validar la cédula mediante OCR.
●  Enviar el resumen del proceso de votación al ﬁnalizar correctamente.
●
Identiﬁcar un canal de contacto del ciudadano dentro del sistema.

Ejemplo:

●

juan.perez@email.com

●  maria.rodriguez@email.com

Número de documento de identidad

Representa el número de identiﬁcación del ciudadano dentro del sistema.

Este campo debe guardarse como texto, no como número, porque puede contener

ceros al inicio o formatos especiales.

Ejemplo:

●  00112345678
●  40212345678

El número de documento será utilizado en la pantalla inicial del elector y deberá

coincidir con el número extraído de la imagen de la cédula mediante OCR.

Estado

Indica si el ciudadano está activo o inactivo.

●  Si el ciudadano está activo, puede participar en una elección activa, siempre

que cumpla las demás validaciones del proceso de votación.

●  Si el ciudadano está inactivo, no puede iniciar ni completar el proceso de

votación.

●  Si un ciudadano inactivo participó en elecciones anteriores, su participación

histórica debe conservarse.

Validaciones para crear ciudadano

El formulario de creación debe cumplir las siguientes validaciones:

●  El nombre es requerido.
●  El apellido es requerido.
●  El correo electrónico es requerido.
●  El correo electrónico debe tener un formato válido.
●  El correo electrónico del ciudadano debe ser único en el sistema.
●  El número de documento de identidad es requerido.
●  El número de documento de identidad no puede repetirse.
●  La validación del número de documento debe ignorar espacios al inicio y al

ﬁnal.

●  El número de documento debe guardarse como texto.
●  El estado debe manejarse como booleano.
●  Al crear un ciudadano, el estado debe venir marcado como activo por defecto.
●  No debe permitirse crear un ciudadano si existe una elección activa.

Si el usuario intenta registrar un ciudadano con un número de documento ya

existente, el sistema debe mostrar un mensaje como:

“Ya existe un ciudadano registrado con este número de documento de

identidad.”

Si el usuario intenta registrar un correo electrónico con formato inválido, el sistema

debe mostrar un mensaje como:

“Debe ingresar un correo electrónico válido.”

Si el usuario intenta registrar un correo electrónico ya existente, el sistema debe

mostrar un mensaje como:

“Ya existe un ciudadano registrado con este correo electrónico. ”

Si el usuario intenta crear un ciudadano mientras existe una elección activa, el

sistema debe mostrar un mensaje como:

“No se puede crear un ciudadano mientras exista una elección activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de ciudadanos sin guardar

cambios.

●  Crear ciudadano: guarda el ciudadano en el sistema.

Al hacer clic en Crear ciudadano, si los datos son válidos, el sistema debe registrar

el ciudadano en estado activo y redirigir al usuario a la pantalla inicial del

mantenimiento de ciudadanos.

Editar ciudadano

En el listado de ciudadanos, al presionar el botón Editar, el sistema debe enviar al

usuario a una pantalla con un formulario para modiﬁcar el ciudadano seleccionado.

Esta acción sólo debe estar disponible si no existe una elección activa.

El formulario debe mostrar los datos actuales del ciudadano.

El formulario de edición debe contener los siguientes campos:

Campo

Tipo de dato

Requerid
o

Descripción

Nombre

Apellido

Texto / string

Texto / string

Correo electrónico

Texto / string

Sí

Sí

Sí

Nombre del ciudadano.

Apellido del ciudadano.

Correo electrónico asociado al
ciudadano.

Número de

documento de
identidad

Estado

Texto / string

Sí

Número único del documento
de identidad del ciudadano.

Booleano /
checkbox

Sí

Indica si el ciudadano está
activo o inactivo.

Como es una edición, los campos deben venir cargados con los valores guardados

para el ciudadano seleccionado.

Validaciones para editar ciudadano

El formulario de edición debe cumplir las siguientes validaciones:

●  El nombre es requerido.
●  El apellido es requerido.
●  El correo electrónico es requerido.
●  El correo electrónico del ciudadano debe ser único en el sistema.
●  El correo electrónico debe tener formato válido.
●  El número de documento de identidad es requerido.
●  El número de documento de identidad no puede repetirse.
●  La validación del número de documento debe ignorar espacios al inicio y al

ﬁnal.

●  El estado debe manejarse como booleano.
●  No debe permitirse editar un ciudadano si existe una elección activa.

En el caso del número de documento de identidad, el sistema debe validar que no

exista otro ciudadano diferente con el mismo número.

Ejemplo:

Ciudadano editado: Juan Pérez

Documento actual: 00112345678

Si el usuario guarda el mismo documento 00112345678, debe permitirse, porque

pertenece al mismo ciudadano que se está editando.

Pero si existe otro ciudadano con el documento 00112345678, no debe permitirse

guardar.

Si el usuario intenta editar un ciudadano mientras existe una elección activa, el

sistema debe mostrar un mensaje como:

“No se puede editar un ciudadano mientras exista una elección activa.”

Restricciones importantes al editar

Si el ciudadano ya participó en una elección activa o ﬁnalizada, el sistema no debe

permitir modiﬁcar su número de documento de identidad.

Esto evita inconsistencias históricas y problemas con la trazabilidad del proceso

electoral, ya que el número de documento fue utilizado para validar su identidad y

registrar su participación.

Mensaje sugerido:

“No se puede modiﬁcar el número de documento de identidad de este

ciudadano porque ya participó en una elección.”

Sí se debe permitir modiﬁcar datos como:

●  Nombre.
●  Apellido.
●  Correo electrónico.
●  Estado.

Siempre que no exista una elección activa.

Si se permite modiﬁcar nombre o apellido de un ciudadano que ya participó en una

elección activa o ﬁnalizada, los resultados históricos no deben verse afectados. La

participación histórica debe conservarse correctamente.

En el caso del correo electrónico, el sistema debe validar que no exista otro

ciudadano diferente con el mismo correo.

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de ciudadanos sin guardar

cambios.

●  Guardar cambios: actualiza la información del ciudadano.

Al hacer clic en Guardar cambios, si los datos son válidos, el sistema debe

actualizar el ciudadano y redirigir al usuario a la pantalla inicial del mantenimiento.

Activar ciudadano

Si el ciudadano está inactivo, en el listado debe mostrarse un botón con el texto

Activar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea activar este ciudadano?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de ciudadanos sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado del ciudadano a

activo y redirigir al usuario nuevamente al listado de ciudadanos.

Validaciones para activar ciudadano

Antes de activar un ciudadano, el sistema debe validar:

●  El ciudadano debe existir.
●  El ciudadano debe estar actualmente inactivo.
●  No debe existir una elección activa.
●  No debe existir otro ciudadano activo o inactivo con el mismo número de

documento.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede activar un ciudadano mientras exista una elección activa.”

Si el ciudadano ya está activo, el sistema debe mostrar un mensaje como:

“Este ciudadano ya se encuentra activo.”

Desactivar ciudadano

Si el ciudadano está activo, en el listado debe mostrarse un botón con el texto

Desactivar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desactivar este ciudadano?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de ciudadanos sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe intentar cambiar el estado del

ciudadano a inactivo.

Validaciones para desactivar ciudadano

Antes de desactivar un ciudadano, el sistema debe validar:

●  El ciudadano debe existir.
●  El ciudadano debe estar actualmente activo.
●  No debe existir una elección activa.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede desactivar un ciudadano mientras exista una elección

activa.”

Si el ciudadano ya está inactivo, el sistema debe mostrar un mensaje como:

“Este ciudadano ya se encuentra inactivo.”

Si todas las validaciones se cumplen, el sistema debe cambiar el estado del

ciudadano a inactivo y redirigir al usuario nuevamente al listado de ciudadanos.

Reglas adicionales del mantenimiento

●  Solo los usuarios con rol Administrador pueden acceder al mantenimiento

de ciudadanos.

●  No se debe permitir crear, editar, activar ni desactivar ciudadanos mientras

exista una elección activa.

●  El número de documento de identidad debe ser único en todo el sistema.
●  El número de documento debe manejarse como texto, no como número.
●  La validación del número de documento debe ignorar espacios al inicio y al

ﬁnal.

●  El correo electrónico debe ser obligatorio, porque será utilizado para enviar el

código de veriﬁcación y el resumen de votación.

●  Al crear un ciudadano, debe guardarse en estado activo por defecto.
●  Los ciudadanos activos pueden participar en una elección activa, siempre que

no hayan votado previamente en dicha elección.

●  Los ciudadanos inactivos no pueden participar en procesos de votación.
●  Si un ciudadano ya participó en una elección, no se debe permitir modiﬁcar su

número de documento de identidad.

●  La inactivación de un ciudadano no debe eliminar ni alterar sus

participaciones históricas en elecciones ﬁnalizadas.

●  Los votos emitidos por ciudadanos que posteriormente sean inactivados

deben seguir contándose en los resultados históricos.

●  El mantenimiento debe usar el mismo layout general de la aplicación.

Mantenimiento de Partidos políticos

Al ingresar a la opción Partidos políticos desde el menú principal del

administrador, el sistema debe enviar al usuario al mantenimiento de partidos

políticos.

Este mantenimiento permitirá registrar, consultar, editar, activar y desactivar los

partidos políticos que podrán participar en los procesos electorales del sistema.

Un partido político representa una organización que puede registrar candidatos,

participar en elecciones, solicitar alianzas políticas y presentar candidatos propios o

aliados para los diferentes puestos electivos.

Los partidos políticos activos serán utilizados en los módulos de asignación de

dirigentes políticos, mantenimiento de candidatos, alianzas políticas, asignación de

candidatos a puestos electivos y creación de elecciones.

Pantalla inicial del mantenimiento

En la pantalla inicial del mantenimiento de partidos políticos, el sistema debe

mostrar un listado con todos los partidos creados en el sistema.

De cada partido político se debe mostrar la siguiente información:

Campo

Descripción

Nombre del partido

Nombre completo del partido político.

Descripción

Información descriptiva o breve reseña del partido político.

Siglas

Código corto que identiﬁca al partido político. Ejemplo:
PRM, PLD, FP.

Logo del partido

Imagen o emblema visual del partido político.

Estado

Indica si el partido político está activo o inactivo dentro del
sistema.

Cada partido político listado debe tener las siguientes acciones:

Acción

Editar

Activar

Descripción

Permite modiﬁcar los datos del partido político.

Permite cambiar el estado del partido de inactivo a activo.
Solo debe mostrarse si el partido está inactivo.

Desactivar

Permite cambiar el estado del partido de activo a inactivo.
Solo debe mostrarse si el partido está activo.

Arriba del listado debe existir un botón con el texto Crear partido político.

Si existe una elección activa, el sistema no debe permitir crear, editar, activar ni

desactivar partidos políticos. En ese caso, los botones de acción deben mostrarse

deshabilitados y debe indicarse visualmente que la acción no está disponible

mientras exista una elección activa.

Mensaje sugerido:

“No se pueden modiﬁcar partidos políticos mientras exista una elección

activa.”

Crear partido político

Al hacer clic sobre el botón Crear partido político, el sistema debe enviar al usuario

a una pantalla con un formulario para registrar un nuevo partido.

Este botón solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato  Requerido

Descripción

Nombre del
partido

Texto / string

Sí

Representa el nombre completo
del partido político.

Descripción

Texto / string

No

Permite registrar una breve
descripción del partido político.

Campo

Tipo de dato  Requerido

Descripción

Siglas

Texto / string

Sí

Logo del partido  File / imagen

Sí

Estado

Booleano /
checkbox

Sí

Código corto que identiﬁca al
partido político dentro del sistema.

Imagen que representa
visualmente al partido político.

Indica si el partido estará activo o

inactivo. Por defecto, al crear un

partido debe venir marcado como
activo.

Descripción de campos

Nombre del partido

Representa el nombre completo del partido político.

Ejemplo:

●  Partido Nacional Democrático
●  Movimiento de Unidad Ciudadana
●  Partido Popular Reformista

Este valor será mostrado en los mantenimientos, reportes, alianzas políticas,

asignaciones de candidatos y resultados electorales.

Descripción

Permite registrar información adicional o una breve reseña del partido político.

Este campo es opcional.

Ejemplo:

Organización política orientada a la participación democrática nacional.

Partido político con representación municipal y congresual.

Siglas

Representan el código corto o abreviatura que identiﬁca al partido político.

Ejemplo:

●  PND
●  MUC
●  PPR

Las siglas deben ser únicas en todo el sistema.

Se recomienda guardar las siglas en mayúscula para mantener la consistencia

visual.

Ejemplo:

Si el usuario escribe:

pnd

El sistema puede guardarlo como:

PND

Logo del partido

Representa la imagen o emblema visual del partido político.

El logo será utilizado en:

●  Listado de partidos políticos.
●  Pantalla de candidatos disponibles para el elector.
●  Resultados electorales.
●  Visualización de alianzas o candidaturas, si aplica.

Formatos recomendados:

●

●

●

.jpg

.jpeg

.png

El archivo debe ser una imagen válida y tener un tamaño razonable para evitar

problemas de carga o almacenamiento.

Estado

Indica si el partido político está activo o inactivo.

●  Si el partido está activo, puede participar en nuevas conﬁguraciones

electorales.

●  Si el partido está inactivo, no debe aparecer en nuevos formularios de

asignación, alianzas o candidaturas.

●  Si un partido inactivo participó en elecciones anteriores, debe seguir
mostrándose en los resultados históricos donde haya participado.

Validaciones para crear partido político

El formulario de creación debe cumplir las siguientes validaciones:

●  El nombre del partido es requerido.
●  Las siglas son requeridas.
●  Las siglas no pueden repetirse.
●  La validación de las siglas debe ignorar espacios al inicio y al ﬁnal.
●  Las siglas deben guardarse preferiblemente en mayúscula.
●  El logo del partido es requerido.
●  El archivo cargado como logo debe ser una imagen válida.
●  La descripción es opcional.
●  El estado debe manejarse como booleano.
●  Al crear un partido político, el estado debe venir marcado como activo por

defecto.

●  No debe permitirse crear un partido político si existe una elección activa.

Si el usuario intenta registrar un partido con siglas ya existentes, el sistema debe

mostrar un mensaje como:

“Ya existe un partido político registrado con estas siglas.”

Si el usuario intenta subir un archivo que no es una imagen válida, el sistema debe

mostrar un mensaje como:

“El logo del partido debe ser una imagen válida.”

Si el usuario intenta crear un partido mientras existe una elección activa, el sistema

debe mostrar un mensaje como:

“No se puede crear un partido político mientras exista una elección

activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de partidos políticos sin guardar

cambios.

●  Crear partido político: guarda el nuevo partido en el sistema.

Al hacer clic en Crear partido político, si los datos son válidos, el sistema debe

registrar el partido en estado activo y redirigir al usuario a la pantalla inicial del

mantenimiento de partidos políticos.

Editar partido político

En el listado de partidos políticos, al presionar el botón Editar, el sistema debe

enviar al usuario a una pantalla con un formulario para modiﬁcar el partido

seleccionado.

Esta acción sólo debe estar disponible si no existe una elección activa.

El formulario debe mostrar los datos actuales del partido político.

El formulario de edición debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre del
partido

Texto / string

Sí

Descripción

Texto / string

No

Siglas

Texto / string

Sí

Logo del partido

File / imagen

No

Nombre completo del partido
político.

Breve descripción del partido
político.

Código corto que identiﬁca al
partido político.

Nuevo logo del partido. En
edición debe ser opcional.

Campo

Tipo de dato

Requerido

Descripción

Estado

Booleano /
checkbox

Sí

Indica si el partido está activo
o inactivo.

Como es una edición, los campos deben venir cargados con los valores guardados

para el partido seleccionado.

En el caso del logo, si el usuario no carga una nueva imagen, el sistema debe

conservar el logo actual.

Validaciones para editar partido político

El formulario de edición debe cumplir las siguientes validaciones:

●  El nombre del partido es requerido.
●  Las siglas son requeridas.
●  Las siglas no pueden repetirse.
●  La validación de las siglas debe ignorar espacios al inicio y al ﬁnal.
●  Las siglas deben guardarse preferiblemente en mayúscula.
●  La descripción es opcional.
●  El logo es opcional en edición.
●  Si se carga un nuevo logo, el archivo debe ser una imagen válida.
●  El estado debe manejarse como booleano.
●  No debe permitirse editar un partido político si existe una elección activa.

En el caso de las siglas, el sistema debe validar que no exista otro partido político

diferente con las mismas siglas.

Ejemplo:

Partido editado: Partido Nacional Democrático

Siglas actuales: PND

Si el usuario guarda las mismas siglas PND, debe permitirse, porque pertenecen al

mismo partido que se está editando.

Pero si existe otro partido con las siglas PND, no debe permitirse guardar.

Si el usuario intenta editar un partido mientras existe una elección activa, el sistema

debe mostrar un mensaje como:

“No se puede editar un partido político mientras exista una elección

activa.”

Restricciones importantes al editar

Si el partido político ya participó en una elección activa o ﬁnalizada, el sistema no

debe permitir modiﬁcar sus siglas.

Esto evita inconsistencias históricas, porque las siglas identiﬁcan al partido en

resultados electorales, candidaturas, alianzas políticas y reportes.

Mensaje sugerido:

“No se pueden modiﬁcar las siglas de este partido político porque ya

participó en una elección.”

También se recomienda no permitir modiﬁcar el nombre del partido si ya participó

en una elección activa o ﬁnalizada, salvo que el sistema guarde una copia histórica

de los datos del partido al momento de iniciar cada elección.

Regla recomendada para este proyecto:

●  Si el partido ya participó en una elección activa o ﬁnalizada, no se debe

permitir modiﬁcar sus siglas.

●  Si el sistema no maneja datos históricos congelados por elección, tampoco se

debe permitir modiﬁcar el nombre.

●  Sí se debe permitir modiﬁcar la descripción, el logo y el estado, siempre que
no exista una elección activa y que el cambio no afecte reglas activas del

sistema.

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de partidos políticos sin guardar

cambios.

●  Guardar cambios: actualiza la información del partido.

Al hacer clic en Guardar cambios, si los datos son válidos, el sistema debe

actualizar el partido y redirigir al usuario a la pantalla inicial del mantenimiento.

Activar partido político

Si el partido político está inactivo, en el listado debe mostrarse un botón con el

texto Activar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea activar este partido político?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de partidos

políticos sin realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado del partido a activo y

redirigir al usuario nuevamente al listado de partidos políticos.

Validaciones para activar partido político

Antes de activar un partido político, el sistema debe validar:

●  El partido político debe existir.
●  El partido debe estar actualmente inactivo.
●  No debe existir una elección activa.
●  No debe existir otro partido político activo o inactivo con las mismas siglas.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede activar un partido político mientras exista una elección

activa.”

Si el partido ya está activo, el sistema debe mostrar un mensaje como:

“Este partido político ya se encuentra activo.”

Desactivar partido político

Si el partido político está activo, en el listado debe mostrarse un botón con el texto

Desactivar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desactivar este partido político?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de partidos

políticos sin realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe intentar cambiar el estado del partido a

inactivo.

Validaciones para desactivar partido político

Antes de desactivar un partido político, el sistema debe validar:

●  El partido político debe existir.
●  El partido debe estar actualmente activo.
●  No debe existir una elección activa.
●  El partido no debe tener candidatos activos registrados.
●  El partido no debe tener un dirigente político activo asignado, si esa

asignación se mantiene vigente.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede desactivar un partido político mientras exista una elección

activa.”

Si el partido tiene candidatos activos registrados, el sistema no debe permitir

desactivarlo y debe mostrar un mensaje como:

“No se puede desactivar este partido político porque tiene candidatos

activos registrados.”

Si el partido tiene un dirigente político activo asignado, el sistema no debe permitir

desactivarlo y debe mostrar un mensaje como:

“No se puede desactivar este partido político porque tiene un dirigente

político asignado.”

Si el partido ya está inactivo, el sistema debe mostrar un mensaje como:

“Este partido político ya se encuentra inactivo.”

Si todas las validaciones se cumplen, el sistema debe cambiar el estado del partido

a inactivo y redirigir al usuario nuevamente al listado de partidos políticos.

Reglas adicionales del mantenimiento

●  Solo los usuarios con rol Administrador pueden acceder al mantenimiento

de partidos políticos.

●  No se debe permitir crear, editar, activar ni desactivar partidos políticos

mientras exista una elección activa.

●  Las siglas del partido deben ser únicas en todo el sistema.
●  Las siglas deben guardarse preferiblemente en mayúscula.
●  La validación de siglas debe ignorar espacios al inicio y al ﬁnal.
●  El nombre del partido es requerido.
●  La descripción del partido es opcional.
●  El logo del partido es requerido al crear.
●  El logo del partido es opcional al editar.
●  Si en la edición no se carga un nuevo logo, debe conservarse el logo actual.
●  Al crear un partido político, debe guardarse en estado activo por defecto.
●  Los partidos activos pueden participar en nuevas conﬁguraciones electorales,

alianzas y asignaciones de candidatos.

●  Los partidos inactivos no deben aparecer como opción en nuevos formularios

de alianzas, asignación de dirigentes o asignación de candidatos.

●  Los partidos inactivos sí deben mostrarse en registros históricos donde ya

participaron.

●  No se debe permitir desactivar un partido político si tiene candidatos activos

registrados.

●  No se debe permitir desactivar un partido político si tiene un dirigente

político activo asignado.

●  Si un partido ya participó en una elección activa o ﬁnalizada, no se deben
permitir cambios que alteren la interpretación histórica de sus resultados.
●  Los resultados de elecciones ﬁnalizadas deben seguir mostrando los partidos

utilizados, aunque posteriormente hayan sido inactivados. Para evitar

inconsistencias, no se permite modiﬁcar el nombre, las siglas ni el logo de un

partido político que ya haya participado en una elección activa o ﬁnalizada.

●  El mantenimiento debe usar el mismo layout general de la aplicación.

Mantenimiento de Usuarios

Al ingresar a la opción Usuarios desde el menú principal del administrador, el

sistema debe enviar al usuario al mantenimiento de usuarios.

Este mantenimiento permitirá registrar, consultar, editar, activar y desactivar los

usuarios que tendrán acceso administrativo o político al sistema.

Los usuarios del sistema no representan electores comunes. Los electores

participan en el proceso de votación mediante su número de documento de

identidad. En cambio, los usuarios registrados en este mantenimiento son aquellos

que podrán iniciar sesión para acceder a funcionalidades internas del sistema.

El sistema maneja dos roles principales:

●  Administrador
●  Dirigente político

El usuario con rol Administrador podrá acceder a los módulos administrativos del

sistema. El usuario con rol Dirigente político podrá acceder a las funcionalidades

relacionadas con su partido político, siempre que tenga un partido asignado.

Pantalla inicial del mantenimiento

En la pantalla inicial del mantenimiento de usuarios, el sistema debe mostrar un

listado con todos los usuarios creados en el sistema.

De cada usuario se debe mostrar la siguiente información:

Campo

Nombre

Descripción

Nombre del usuario registrado.

Campo

Apellido

Descripción

Apellido del usuario registrado.

Correo electrónico

Correo electrónico asociado al usuario.

Nombre de usuario

Identiﬁcador utilizado para iniciar sesión.

Rol

Estado

Rol asignado al usuario dentro del sistema.

Indica si el usuario está activo o inactivo.

Cada usuario listado debe tener las siguientes acciones:

Acción

Editar

Activar

Descripción

Permite modiﬁcar los datos del usuario.

Permite cambiar el estado del usuario de inactivo a activo.
Solo debe mostrarse si el usuario está inactivo.

Desactivar

Permite cambiar el estado del usuario de activo a inactivo.
Solo debe mostrarse si el usuario está activo.

Arriba del listado debe existir un botón con el texto Crear usuario.

Si existe una elección activa, el sistema no debe permitir crear, editar, activar ni

desactivar usuarios. En ese caso,los botones de acción deben mostrarse

deshabilitados y debe indicarse visualmente que la acción no está disponible

mientras exista una elección activa.

Mensaje sugerido:

“No se pueden modiﬁcar usuarios mientras exista una elección activa.”

Crear usuario

Al hacer clic sobre el botón Crear usuario, el sistema debe enviar al usuario a una

pantalla con un formulario para registrar un nuevo usuario.

Este botón solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato  Requerido

Descripción

Nombre

Texto / string

Sí

Apellido

Texto / string

Sí

Correo
electrónico

Nombre de
usuario

Contraseña

Texto / string

Sí

Texto / string

Sí

Password /
string

Sí

Sí

Conﬁrmar
contraseña

Password /
string

Rol

Select / string

Sí

Representa el nombre del
usuario.

Representa el apellido del
usuario.

Correo electrónico asociado al
usuario.

Nombre que será utilizado para
iniciar sesión en el sistema.

Contraseña inicial del usuario.

Conﬁrmación de la contraseña
ingresada.

Rol que tendrá el usuario dentro
del sistema.

Estado

Booleano /
checkbox

Sí

Indica si el usuario estará activo
o inactivo.

Descripción de campos

Nombre

Representa el nombre de la persona que utilizará el usuario.

Ejemplo:

●  Carlos
●  María
●  José
●  Laura

Apellido

Representa el apellido de la persona que utilizará el usuario.

Ejemplo:

●  Pérez
●  Rodríguez
●  Gómez
●  Martínez

Correo electrónico

Representa el correo electrónico del usuario.

Este correo puede utilizarse para ﬁnes de identiﬁcación, recuperación de acceso o

notiﬁcaciones internas del sistema.

Ejemplo:

admin@email.com

dirigente@email.com

El correo electrónico debe tener un formato válido y no debe repetirse entre

usuarios.

Nombre de usuario

Representa el identiﬁcador que el usuario utilizará para iniciar sesión.

Ejemplo:

●  admin01
●  mrodriguez
●  dirigente_prm

El nombre de usuario debe ser único en todo el sistema.

Se recomienda guardar este valor sin espacios al inicio ni al ﬁnal.

Contraseña

Representa la clave de acceso inicial del usuario.

Por seguridad, la contraseña no debe guardarse en texto plano. El sistema debe

almacenarla utilizando un mecanismo seguro de hash.

Reglas mínimas recomendadas:

●  Debe tener al menos 8 caracteres.
●  Debe contener al menos una letra.
●  Debe contener al menos un número.
●  Debe coincidir con el campo Conﬁrmar contraseña.

Conﬁrmar contraseña

Permite validar que el usuario administrador escribió correctamente la contraseña

deseada.

Este campo debe coincidir exactamente con el valor colocado en el campo

Contraseña.

Rol

Representa el nivel de acceso que tendrá el usuario dentro del sistema.

Valores permitidos:

Administrador

Dirigente político

El sistema no debe permitir registrar usuarios con roles diferentes a los deﬁnidos

para el proyecto.

Estado

Indica si el usuario está activo o inactivo.

●  Si el usuario está activo, puede iniciar sesión, siempre que sus credenciales

sean correctas.

●  Si el usuario está inactivo, no puede iniciar sesión.
●  Si el usuario es dirigente político, además de estar activo, debe tener un
partido político asignado para poder acceder al Home del dirigente.

Validaciones para crear usuario

El formulario de creación debe cumplir las siguientes validaciones:

●  El nombre es requerido.
●  El apellido es requerido.
●  El correo electrónico es requerido.
●  El correo electrónico debe tener formato válido.
●  El correo electrónico no puede repetirse.
●  El nombre de usuario es requerido.
●  El nombre de usuario no puede repetirse.
●  La validación del nombre de usuario debe ignorar espacios al inicio y al ﬁnal.
●  La contraseña es requerida.
●  La conﬁrmación de contraseña es requerida.
●  La contraseña y la conﬁrmación de contraseña deben coincidir.
●  La contraseña debe cumplir la política mínima deﬁnida por el sistema.
●  El rol es requerido.
●  El rol seleccionado debe ser Administrador o Dirigente político.
●  El estado debe manejarse como booleano.
●  Al crear un usuario, el estado debe venir marcado como activo por defecto.
●  No debe permitirse crear un usuario si existe una elección activa.

Si el usuario intenta registrar un nombre de usuario ya existente, el sistema debe

mostrar un mensaje como:

“Ya existe un usuario registrado con este nombre de usuario.”

Si el usuario intenta registrar un correo electrónico ya existente, el sistema debe

mostrar un mensaje como:

“Ya existe un usuario registrado con este correo electrónico.”

Si la contraseña y la conﬁrmación no coinciden, el sistema debe mostrar un mensaje

como:

“La contraseña y la conﬁrmación de contraseña no coinciden.”

Si el rol seleccionado no es válido, el sistema debe mostrar un mensaje como:

“Debe seleccionar un rol válido para el usuario.”

Si el usuario intenta crear un usuario mientras existe una elección activa, el sistema

debe mostrar un mensaje como:

“No se puede crear un usuario mientras exista una elección activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de usuarios sin guardar cambios.
●  Crear usuario: guarda el nuevo usuario en el sistema.

Al hacer clic en Crear usuario, si los datos son válidos, el sistema debe registrar el

usuario en estado activo y redirigir al usuario a la pantalla inicial del mantenimiento

de usuarios.

Editar usuario

En el listado de usuarios, al presionar el botón Editar, el sistema debe enviar al

usuario a una pantalla con un formulario para modiﬁcar el usuario seleccionado.

Esta acción sólo debe estar disponible si no existe una elección activa.

El formulario debe mostrar los datos actuales del usuario.

El formulario de edición debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre

Texto / string

Apellido

Texto / string

Correo electrónico  Texto / string

Nombre de
usuario

Contraseña

Texto / string

Password /
string

Sí

Sí

Sí

Sí

No

Nombre del usuario.

Apellido del usuario.

Correo electrónico asociado
al usuario.

Nombre utilizado para iniciar
sesión.

Nueva contraseña del

usuario. Si se deja vacío, se

Campo

Tipo de dato

Requerido

Descripción

Conﬁrmar
contraseña

Password /
string

Rol

Select / string

Estado

Booleano /
checkbox

No

Sí

Sí

conserva la contraseña
actual.

Conﬁrmación de la nueva

contraseña. Solo se valida si

se escribió una nueva
contraseña.

Rol asignado al usuario.

Indica si el usuario está activo
o inactivo.

Como es una edición, los campos deben venir cargados con los valores guardados

para el usuario seleccionado.

El campo contraseña no debe venir precargado.

Validaciones para editar usuario

El formulario de edición debe cumplir las siguientes validaciones:

●  El nombre es requerido.
●  El apellido es requerido.
●  El correo electrónico es requerido.
●  El correo electrónico debe tener formato válido.
●  El correo electrónico no puede repetirse en otro usuario.
●  El nombre de usuario es requerido.
●  El nombre de usuario no puede repetirse en otro usuario.
●  El rol es requerido.
●  El rol seleccionado debe ser Administrador o Dirigente político.
●  El estado debe manejarse como booleano.
●  La contraseña es opcional.
●  Si se escribe una nueva contraseña, debe completarse también el campo

Conﬁrmar contraseña.

●  Si se escribe una nueva contraseña, la contraseña y la conﬁrmación deben

coincidir.

●  Si se escribe una nueva contraseña, debe cumplir la política mínima deﬁnida

por el sistema.

●  No debe permitirse editar un usuario si existe una elección activa.

En el caso del nombre de usuario, el sistema debe validar que no exista otro usuario

diferente con el mismo valor.

Ejemplo:

Usuario editado: María Rodríguez

Nombre de usuario actual: mrodriguez

Si el usuario guarda el mismo nombre de usuario mrodriguez, debe permitirse,

porque pertenece al mismo usuario que se está editando.

Pero si existe otro usuario con el nombre de usuario mrodriguez, no debe permitirse

guardar.

Si el usuario intenta editar un usuario mientras existe una elección activa, el sistema

debe mostrar un mensaje como:

“No se puede editar un usuario mientras exista una elección activa.”

Restricciones importantes al editar

Si el usuario tiene rol Dirigente político y tiene un partido político asignado, no se

debe permitir cambiar su rol a Administrador mientras mantenga esa asignación.

Esto evita que exista una relación de dirigente político asociada a un usuario que ya

no tiene rol de dirigente.

Mensaje sugerido:

“No se puede cambiar el rol de este usuario porque tiene un partido

político asignado como dirigente.”

En ese caso, primero debe eliminarse la relación en el módulo de Asignación de

dirigentes políticos.

Si el usuario es el único administrador activo del sistema, no se debe permitir

cambiar su rol a Dirigente político ni desactivarlo.

Mensaje sugerido:

“No se puede modiﬁcar este usuario porque es el único administrador

activo del sistema.”

Si el administrador está editando su propio usuario, se recomienda no permitir que

cambie su propio rol ni se desactive a sí mismo.

Mensaje sugerido:

“No puede cambiar su propio rol ni desactivar su propio usuario mientras

está autenticado.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de usuarios sin guardar cambios.
●  Guardar cambios: actualiza la información del usuario.

Al hacer clic en Guardar cambios, si los datos son válidos, el sistema debe

actualizar el usuario y redirigir al usuario a la pantalla inicial del mantenimiento.

Activar usuario

Si el usuario está inactivo, en el listado debe mostrarse un botón con el texto

Activar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea activar este usuario?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de usuarios sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado del usuario a activo y

redirigir al usuario nuevamente al listado de usuarios.

Validaciones para activar usuario

Antes de activar un usuario, el sistema debe validar:

●  El usuario debe existir.
●  El usuario debe estar actualmente inactivo.
●  No debe existir una elección activa.
●  El nombre de usuario no debe estar repetido.
●  El correo electrónico no debe estar repetido.
●  El usuario debe tener un rol válido.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede activar un usuario mientras exista una elección activa.”

Si el usuario ya está activo, el sistema debe mostrar un mensaje como:

“Este usuario ya se encuentra activo.”

Desactivar usuario

Si el usuario está activo, en el listado debe mostrarse un botón con el texto

Desactivar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desactivar este usuario?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de usuarios sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe intentar cambiar el estado del usuario a

inactivo.

Validaciones para desactivar usuario

Antes de desactivar un usuario, el sistema debe validar:

●  El usuario debe existir.
●  El usuario debe estar actualmente activo.
●  No debe existir una elección activa.
●  No debe ser el único administrador activo del sistema.
●  El usuario autenticado no debe estar intentando desactivarse a sí mismo.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede desactivar un usuario mientras exista una elección activa.”

Si el usuario es el único administrador activo, el sistema debe mostrar un mensaje

como:

“No se puede desactivar este usuario porque es el único administrador

activo del sistema.”

Si el usuario autenticado intenta desactivar su propio usuario, el sistema debe

mostrar un mensaje como:

“No puede desactivar su propio usuario mientras está autenticado.”

Si el usuario ya está inactivo, el sistema debe mostrar un mensaje como:

“Este usuario ya se encuentra inactivo.”

Si todas las validaciones se cumplen, el sistema debe cambiar el estado del usuario

a inactivo y redirigir al usuario nuevamente al listado de usuarios.

Reglas adicionales del mantenimiento

●  Solo los usuarios con rol Administrador pueden acceder al mantenimiento

de usuarios.

●  No se debe permitir crear, editar, activar ni desactivar usuarios mientras

exista una elección activa.

●  El nombre de usuario debe ser único en todo el sistema.
●  El correo electrónico debe ser único en todo el sistema.
●  El nombre de usuario debe guardarse sin espacios al inicio ni al ﬁnal.

●  El correo electrónico debe tener formato válido.
●  El sistema solo debe permitir los roles Administrador y Dirigente político.
●  Al crear un usuario, debe guardarse en estado activo por defecto.
●  La contraseña no debe almacenarse en texto plano.
●  En creación, la contraseña y la conﬁrmación de contraseña son obligatorias.
●  En edición, la contraseña es opcional.
●  En edición, si no se escribe una nueva contraseña, debe conservarse la

contraseña actual.

●  Si un usuario tiene rol Dirigente político, sólo podrá iniciar sesión

correctamente si tiene un partido político asignado y dicho partido se

encuentra activo.

●  No se debe permitir cambiar el rol de un dirigente político que tenga un

partido asignado.

●  No se debe permitir desactivar el único administrador activo del sistema.
●  No se debe permitir que un administrador se desactive a sí mismo mientras

está autenticado.

●  El mantenimiento debe usar el mismo layout general de la aplicación.

Asignación de dirigente políticos

Al ingresar a la opción Asignación de dirigentes políticos desde el menú principal

del administrador, el sistema debe enviar al usuario a la pantalla de asignación de

dirigentes políticos.

Este módulo permitirá relacionar un usuario con rol Dirigente político con un

partido político activo del sistema.

La relación entre dirigentes políticos y partidos políticos será de tipo uno a uno.

Esto signiﬁca que un dirigente político solo puede estar asignado a un partido

político y, al mismo tiempo, un partido político solo puede tener un dirigente político

asignado.

Esta asignación es necesaria para que un usuario con rol Dirigente político pueda

iniciar sesión correctamente y acceder al Home del dirigente. Si un usuario tiene rol

de dirigente político, pero no tiene un partido asignado, no podrá acceder a las

funcionalidades del dirigente político.

Pantalla inicial del módulo

En la pantalla inicial de asignación de dirigentes políticos, el sistema debe mostrar

un listado con todas las relaciones existentes entre dirigentes políticos y partidos

políticos.

De cada relación se debe mostrar la siguiente información:

Campo

Descripción

Nombre del dirigente
político

Nombre y apellido del usuario con rol Dirigente
político.

Nombre de usuario

Nombre de usuario utilizado por el dirigente para
iniciar sesión.

Partido político asignado

Nombre del partido político relacionado con el
dirigente.

Siglas del partido

Siglas del partido político asignado.

Estado del dirigente

Indica si el usuario dirigente está activo o inactivo.

Estado del partido

Indica si el partido político asignado está activo o
inactivo.

Cada relación listada debe tener la siguiente acción:

Acción

Descripción

Eliminar relación

Permite desvincular el dirigente político del partido político
asignado.

Arriba del listado debe existir un botón con el texto Agregar asignación.

Si existe una elección activa, el sistema no debe permitir crear ni eliminar

asignaciones entre dirigentes políticos y partidos políticos. En ese caso, el botón

Agregar asignación y los botones de eliminación deben ocultarse o mostrarse

deshabilitados.

Mensaje sugerido:

“No se pueden modiﬁcar asignaciones de dirigentes políticos mientras

exista una elección activa.”

Agregar asignación

Al hacer clic sobre el botón Agregar asignación, el sistema debe enviar al usuario a

una pantalla con un formulario para crear una nueva relación entre un dirigente

político y un partido político.

Esta acción solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato  Requerido

Descripción

Dirigente
político

Partido político

Select /
entero

Select /
entero

Sí

Sí

Usuario activo con rol Dirigente

político que aún no tenga partido
político asignado.

Partido político activo que aún no
tenga dirigente político asignado.

Descripción de campos

Dirigente político

Representa el usuario que será asignado como dirigente político de un partido.

Este campo debe cargarse desde el mantenimiento de usuarios.

El select debe mostrar únicamente usuarios que cumplan todas las siguientes

condiciones:

●  El usuario debe estar activo.
●  El usuario debe tener el rol Dirigente político.
●  El usuario no debe estar asignado actualmente a ningún partido político.

El texto mostrado en el select puede incluir el nombre completo y el nombre de

usuario.

Ejemplo:

María Rodríguez - mrodriguez

Carlos Pérez - cperez

No deben aparecer en este select:

●  Usuarios inactivos.
●  Usuarios con rol Administrador.
●  Usuarios con rol Dirigente político que ya tengan un partido asignado.

Partido político

Representa el partido político que será asignado al dirigente seleccionado.

Este campo debe cargarse desde el mantenimiento de partidos políticos.

El select debe mostrar únicamente partidos políticos que cumplan todas las

siguientes condiciones:

●  El partido debe estar activo.
●  El partido no debe tener un dirigente político asignado.

El texto mostrado en el select debe incluir el nombre del partido y sus siglas.

Ejemplo:

Partido Nacional Democrático - PND

Movimiento de Unidad Ciudadana - MUC

No deben aparecer en este select:

●  Partidos políticos inactivos.
●  Partidos políticos que ya tengan un dirigente asignado.

Validaciones para agregar asignación

El formulario de creación debe cumplir las siguientes validaciones:

●  El dirigente político es requerido.

●  El partido político es requerido.
●  El usuario seleccionado debe existir.
●  El usuario seleccionado debe estar activo.
●  El usuario seleccionado debe tener el rol Dirigente político.
●  El usuario seleccionado no debe estar asignado a otro partido político.
●  El partido político seleccionado debe existir.
●  El partido político seleccionado debe estar activo.
●  El partido político seleccionado no debe tener otro dirigente político

asignado.

●  No debe permitirse crear una asignación si existe una elección activa.

Si el usuario seleccionado no tiene rol Dirigente político, el sistema debe mostrar

un mensaje como:

“El usuario seleccionado no tiene el rol de dirigente político.”

Si el dirigente seleccionado ya está relacionado con otro partido político, el sistema

debe mostrar el siguiente mensaje:

“Este dirigente ya está relacionado con otro partido político.”

Si el partido seleccionado ya tiene un dirigente político asignado, el sistema debe

mostrar el siguiente mensaje:

“Este partido político ya tiene un dirigente asignado.”

Si el usuario intenta crear una asignación mientras existe una elección activa, el

sistema debe mostrar un mensaje como:

“No se puede crear una asignación de dirigente político mientras exista

una elección activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de asignaciones sin guardar

cambios.

●  Crear asignación: guarda la relación entre el dirigente político y el partido

político.

Al hacer clic en Crear asignación, si los datos son válidos, el sistema debe registrar

la relación y redirigir al usuario a la pantalla inicial del módulo.

Eliminar asignación

En el listado de asignaciones, cada relación debe tener un botón con el texto

Eliminar relación.

Esta acción solo debe estar disponible si no existe una elección activa.

Al presionar el botón Eliminar relación, el sistema debe enviar al usuario a una

pantalla de conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desvincular este dirigente político de este

partido?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de asignaciones sin

eliminar la relación.

Si el usuario pulsa Aceptar, el sistema debe eliminar la relación entre el dirigente

político y el partido político, y luego debe redirigir al usuario nuevamente al listado

de asignaciones.

Validaciones para eliminar asignación

Antes de eliminar una asignación, el sistema debe validar:

●  La relación debe existir.
●  No debe existir una elección activa.
●  El dirigente político no debe estar participando en una operación activa

dependiente de esa relación.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede eliminar una asignación de dirigente político mientras

exista una elección activa.”

Si la relación no existe, el sistema debe mostrar un mensaje como:

“La asignación seleccionada no existe o ya fue eliminada.”

Efecto de eliminar una asignación

Cuando se elimina la relación entre un dirigente político y un partido político, el

usuario dirigente queda sin partido asignado.

A partir de ese momento, si ese usuario intenta iniciar sesión, el sistema debe

impedir el acceso al Home del dirigente y mostrar el mensaje deﬁnido en el módulo

de inicio de sesión:

“No tiene un partido político asignado, por lo tanto no puede iniciar

sesión. Por favor, póngase en contacto con un administrador.”

La eliminación de la asignación no debe eliminar el usuario ni el partido político.

Solo debe eliminar la relación entre ambos.

Reglas adicionales del módulo

●  Solo los usuarios con rol Administrador pueden acceder al módulo de

asignación de dirigentes políticos.

●  No se debe permitir crear ni eliminar asignaciones mientras exista una

elección activa.

●  Un dirigente político solo puede estar asignado a un partido político.
●  Un partido político solo puede tener un dirigente político asignado.
●  Solo pueden asignarse usuarios activos con rol Dirigente político.
●  No pueden asignarse usuarios con rol Administrador.
●  Solo pueden asignarse partidos políticos activos.
●  Un dirigente político sin partido asignado no puede acceder al Home del

dirigente.

●  Un partido político inactivo no debe aparecer en el select para nuevas

asignaciones.

●  Un usuario dirigente inactivo no debe aparecer en el select para nuevas

asignaciones.

●  La eliminación de una asignación no debe eliminar el usuario ni el partido

político.

●  Si un dirigente político fue desvinculado de un partido, podrá ser asignado
posteriormente a otro partido, siempre que no exista una elección activa.

●  Si un partido político fue desvinculado de un dirigente, podrá recibir otro
dirigente posteriormente, siempre que no exista una elección activa.

●  El módulo debe usar el mismo layout general de la aplicación.

Elecciones

Al ingresar a la opción Elecciones desde el menú principal del administrador, el

sistema debe enviar al usuario al módulo de gestión de elecciones.

Este módulo permitirá crear procesos electorales, consultar elecciones registradas,

activar una elección pendiente, ﬁnalizar una elección activa y visualizar los

resultados de elecciones ﬁnalizadas.

Una elección representa un proceso electoral formal dentro del sistema. Para

preservar la consistencia de los resultados, el sistema no debe crear copias

históricas de partidos políticos, candidatos ni puestos electivos al momento de crear

una elección. Los resultados se calcularán utilizando los datos actuales registrados

en el sistema.

Debido a esto, una vez que un puesto electivo, partido político o candidato haya

participado en una elección, el sistema no debe permitir modiﬁcar los campos

principales que afectan la boleta electoral o los resultados.

Para ﬁnes de bloqueo de campos críticos, se considera que un puesto electivo,

partido político o candidato participó en una elección desde el momento en que una

elección donde aparece pasa al estado Activa.

Estados de una elección

Toda elección debe manejar uno de los siguientes estados:

Estado

Descripción

Pendiente

La elección fue creada, pero todavía no está disponible

para votación.

Activa

La elección está disponible para que los ciudadanos

puedan votar.

Estado

Descripción

Finalizada

La elección terminó y sus resultados pueden ser

consultados. No se permite votar ni modiﬁcar votos.

Pueden existir múltiples elecciones en estado pendiente.

Solo puede existir una elección en estado Activa a la vez.

Una elección en estado pendiente no permite votación.

Una elección en estado activo permite la votación.

Una elección en estado Finalizada no permite nuevos votos.

Pantalla inicial del módulo

En la pantalla inicial del módulo de elecciones, el sistema debe mostrar un listado

con todas las elecciones registradas, organizadas desde la más reciente hasta la

más antigua.

Si existe una elección en estado Activa, esta debe aparecer de primera en el listado

y debe contar con una etiqueta o color visual que indique claramente que se

encuentra activa.

De cada elección se debe mostrar la siguiente información:

Campo

Descripción

Nombre de la elección

Nombre con el que fue registrado el proceso
electoral.

Fecha de realización

Fecha informativa asignada a la elección.

Estado

Estado actual de la elección: Pendiente, Activa o
Finalizada.

Cantidad de partidos
participantes

Cantidad de partidos políticos que participan en la
elección.

Campo

Descripción

Cantidad de puestos
disputados

Cantidad de puestos electivos incluidos en la
elección.

Cantidad de ciudadanos
que votaron

Total de ciudadanos que ﬁnalizaron correctamente
su proceso de votación.

Cada elección listada debe tener acciones según su estado.

Estado de la elección

Acción
disponible

Descripción

Pendiente

Activar

Permite iniciar formalmente el
proceso de votación.

Activa

Finalizar

Permite cerrar la elección activa.

Finalizada

Ver resultados  Permite consultar los resultados de

la elección.

Arriba del listado debe existir un botón con el texto Crear elección.

El botón Crear elección sólo debe estar disponible si no existe una elección activa.

Como pueden existir múltiples elecciones pendientes, el sistema sí puede permitir

crear nuevas elecciones en estado Pendiente, siempre que no exista una elección

Activa y que se cumplan las validaciones requeridas.

Crear elección

Al hacer clic sobre el botón Crear elección, el sistema debe enviar al usuario a una

pantalla con un formulario para registrar un nuevo proceso electoral.

Una elección nueva debe crearse inicialmente en estado Pendiente. Esto signiﬁca

que, luego de crearla, todavía no estará disponible para votación hasta que el

administrador la active desde el listado de elecciones.

El formulario debe contener los siguientes campos:

Campo

Tipo de
dato

Requerid
o

Descripción

Nombre de la
elección

Fecha de
realización

Texto /
string

Date

Sí

Sí

Nombre que identiﬁcará el proceso
electoral.

Fecha informativa asociada al
proceso electoral.

Descripción de campos

Nombre de la elección

Representa el nombre con el que se identiﬁcará el proceso electoral dentro del

sistema.

Ejemplo:

●  Elecciones Municipales 2026
●  Elecciones Congresuales 2026
●  Elecciones Presidenciales 2028

Este nombre será mostrado en el listado de elecciones, en la pantalla de resultados

y en el resumen enviado al ciudadano luego de ﬁnalizar su votación.

Fecha de realización

Representa la fecha oﬁcial o referencial asociada al proceso electoral.

Ejemplo:

15/05/2026

20/02/2028

La fecha de realización es informativa y se utiliza para organización, consulta por

año y reportes.

La fecha de realización no activa automáticamente la elección.

La activación de la elección será manual y deberá realizarla el administrador

mediante el botón Activar.

El sistema no debe impedir activar una elección por estar antes o después de la

fecha de realización, siempre que se cumplan las validaciones de activación.

Validaciones para crear elección

El formulario de creación debe cumplir las siguientes validaciones:

●  El nombre de la elección es requerido.
●  La fecha de realización es requerida.
●  No debe existir una elección activa.
●  Debe existir al menos un puesto electivo activo.
●  Deben existir al menos dos partidos políticos activos.
●  Cada partido político activo debe tener un candidato activo asignado para

cada puesto electivo activo.

●  No debe permitirse crear una elección si no existen puestos electivos activos.
●  No debe permitirse crear una elección si no existen suﬁcientes partidos

políticos activos.

●  No debe permitirse crear una elección si algún partido político activo no tiene

candidatos activos asignados para todos los puestos electivos activos.

Si no existen puestos electivos activos, el sistema debe mostrar el siguiente

mensaje:

“No hay puestos electivos activos para realizar una elección.”

Si no existen al menos dos partidos políticos activos, el sistema debe mostrar el

siguiente mensaje:

“No hay suﬁcientes partidos políticos para realizar una elección.”

Si existen partidos políticos activos, pero alguno de ellos no tiene candidatos

activos asignados para todos los puestos electivos activos, el sistema debe mostrar

un mensaje por cada partido con el siguiente formato:

“El partido político [nombre del partido] ([siglas del partido]) no tiene

candidatos activos asignados para los siguientes puestos electivos:

[listado de puestos].”

Si el usuario intenta crear una elección mientras existe una elección activa, el

sistema debe mostrar el siguiente mensaje:

“No se puede crear una nueva elección mientras exista una elección

activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de elecciones sin guardar

cambios.

●  Crear elección: guarda el proceso electoral en el sistema.

Al hacer clic en Crear elección, si los datos son válidos, el sistema debe crear la

elección en estado Pendiente y redirigir al usuario al listado de elecciones.

Conﬁguración dinámica de elecciones pendientes

Mientras una elección esté en estado Pendiente, su conﬁguración se considera

dinámica.

Esto signiﬁca que la elección pendiente no congela partidos políticos, candidatos ni

puestos electivos.

La conﬁguración válida para activar una elección será la conﬁguración existente en

el sistema al momento de presionar el botón Activar.

Por esta razón, aunque una elección haya sido creada correctamente en estado

Pendiente, el sistema debe volver a validar toda la conﬁguración electoral antes de

activarla.

El sistema debe validar nuevamente:

●  Que no exista otra elección activa.
●  Que existan puestos electivos activos.
●  Que existan al menos dos partidos políticos activos.
●  Que cada partido político activo tenga candidatos activos asignados para

todos los puestos electivos activos.

●  Que no existan inconsistencias en las asignaciones de candidatos a puestos.
●  Que los candidatos asignados se encuentren activos.
●  Que los partidos de los candidatos asignados se encuentren activos.

●  Que los puestos electivos asignados se encuentren activos.

Relación de la elección con los datos actuales del sistema

Al crear una elección, el sistema no debe crear copias históricas de puestos

electivos, partidos políticos ni candidatos.

La elección debe trabajar con los registros actuales existentes en los módulos de:

●  Puestos electivos.
●  Partidos políticos.
●  Candidatos.
●  Asignación de candidatos a puestos.

Esto signiﬁca que la boleta electoral y los resultados utilizarán los datos actuales

registrados en el sistema.

Para evitar inconsistencias, desde el momento en que una elección pasa a estado

Activa, el sistema debe bloquear la modiﬁcación de los campos principales de los

puestos electivos, partidos políticos y candidatos que forman parte de esa elección.

Activar elección

Una elección en estado Pendiente debe mostrar un botón con el texto Activar.

Al hacer clic sobre este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea activar esta elección?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de elecciones sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe intentar cambiar el estado de la

elección de Pendiente a Activa.

La activación de una elección es manual. La fecha de realización no activa la

elección automáticamente.

Validaciones para activar elección

Antes de activar una elección, el sistema debe validar:

●  La elección debe existir.
●  La elección debe estar en estado Pendiente.
●  No debe existir otra elección activa.
●  Debe existir al menos un puesto electivo activo.
●  Deben existir al menos dos partidos políticos activos.
●  Cada partido político activo debe tener candidatos activos asignados para

todos los puestos electivos activos.

●  La conﬁguración electoral actual debe seguir cumpliendo las condiciones

necesarias para iniciar el proceso de votación.

Si ya existe una elección activa, el sistema debe mostrar el siguiente mensaje:

“No se puede activar esta elección porque ya existe una elección activa.”

Si la elección no está en estado Pendiente, el sistema debe mostrar un mensaje

como:

“Solo se pueden activar elecciones en estado pendiente.”

Si no existen puestos electivos activos, el sistema debe mostrar el siguiente

mensaje:

“No hay puestos electivos activos para activar esta elección.”

Si no existen al menos dos partidos políticos activos, el sistema debe mostrar el

siguiente mensaje:

“No hay suﬁcientes partidos políticos para activar esta elección.”

Si existen partidos políticos activos, pero alguno de ellos no tiene candidatos

activos asignados para todos los puestos electivos activos, el sistema debe mostrar

un mensaje por cada partido con el siguiente formato:

“El partido político [nombre del partido] ([siglas del partido]) no tiene

candidatos activos asignados para los siguientes puestos electivos:

[listado de puestos].”

Si la conﬁguración electoral actual no cumple con las condiciones necesarias para

activar la elección, el sistema debe mostrar un mensaje como:

“No se puede activar esta elección porque la conﬁguración electoral

actual no está completa.”

Si todas las validaciones se cumplen, el sistema debe activar la elección y redirigir al

usuario al listado de elecciones.

A partir de ese momento, los ciudadanos habilitados podrán iniciar el proceso de

votación.

Desde ese momento, los puestos, partidos y candidatos que forman parte de la

elección se consideran participantes, por lo que deben aplicarse las restricciones de

modiﬁcación de campos críticos.

Finalizar elección

Una elección en estado Activa debe mostrarse de primera en el listado y debe tener

una etiqueta visual que indique que se encuentra activa.

Para la elección activa no debe mostrarse el botón Ver resultados. En su lugar,

debe mostrarse un botón con el texto Finalizar.

Al hacer clic sobre el botón Finalizar, el sistema debe enviar al usuario a una

pantalla de conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea ﬁnalizar esta elección?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de elecciones sin

ﬁnalizar la elección.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado de la elección de

Activa a Finalizada y redirigir al usuario nuevamente al listado de elecciones.

Validaciones para ﬁnalizar elección

Antes de ﬁnalizar una elección, el sistema debe validar:

●  La elección debe existir.
●  La elección debe estar en estado Activa.
●  La elección no debe estar ya ﬁnalizada.

Si la elección ya está ﬁnalizada, el sistema debe mostrar un mensaje como:

“Esta elección ya se encuentra ﬁnalizada.”

Si la elección no está activa, el sistema debe mostrar un mensaje como:

“Solo se pueden ﬁnalizar elecciones activas.”

Al ﬁnalizar una elección:

●  No se deben permitir nuevos votos.
●  No se deben permitir modiﬁcaciones a votos ya emitidos.
●  Se debe habilitar la consulta de resultados.
●  El botón Finalizar debe dejar de mostrarse.
●  Debe mostrarse el botón Ver resultados.

Ver resultados

Una elección en estado Finalizada debe mostrar un botón con el texto Ver

resultados.

Al hacer clic sobre este botón, el sistema debe enviar al usuario a una pantalla

donde pueda visualizar los resultados de la elección seleccionada.

La pantalla de resultados debe mostrar los puestos disputados en esa elección y,

dentro de cada puesto, los candidatos que participaron, incluyendo la opción

Ninguno.

Los resultados deben calcularse usando los registros actuales del sistema. Por esta

razón, el sistema debe impedir que se modiﬁquen los datos principales de puestos,

partidos y candidatos que ya hayan participado en una elección.

Información a mostrar en resultados

Para cada puesto electivo disputado en la elección, el sistema debe mostrar un

listado de resultados ordenado desde la opción con mayor cantidad de votos hasta

la de menor cantidad de votos.

De cada opción votable se debe mostrar la siguiente información:

Campo

Descripción

Puesto electivo

Nombre actual del puesto disputado.

Candidato / opción

Nombre actual del candidato participante o la opción
Ninguno.

Partido político

Partido por el cual participó el candidato. Para la
opción Ninguno, debe mostrarse “No aplica”.

Cantidad de votos

Total de votos recibidos por el candidato u opción.

Porcentaje de votos

Porcentaje obtenido sobre el total de votos emitidos
para ese puesto.

Resultado

Indica si es la opción ganadora del puesto o si existe
empate.

Cálculo del porcentaje de votos

El porcentaje de votos debe calcularse tomando como base la cantidad total de

votos emitidos para el puesto electivo consultado.

Fórmula:

Porcentaje = (Cantidad de votos de la opción / Total de votos del puesto) *

100

Ejemplo:

Si para el puesto Senador votaron 100 ciudadanos y el candidato Juan Pérez

obtuvo 30 votos:

Porcentaje = (30 / 100) * 100 = 30%

Por lo tanto, el sistema debe mostrar:

Juan Pérez - 30 votos - 30%

La opción Ninguno debe incluirse en el cálculo de porcentaje.

Ejemplo:

Opción

Votos

Porcentaje

Juan Pérez

María Gómez

Ninguno

45

35

20

45%

35%

20%

Determinación del ganador

Para cada puesto electivo, el sistema debe identiﬁcar como ganador al candidato u

opción con mayor cantidad de votos.

Ninguna opción Ninguno participa en el resultado. Si ninguno obtiene la mayor

cantidad de votos en un puesto, debe mostrarse como la opción más votada.

El sistema debe mostrar los resultados ordenados de mayor a menor cantidad de

votos.

Empates

Si dos o más candidatos u opciones obtienen la misma cantidad de votos en el

primer lugar, el sistema debe mostrar que existe un empate.

Mensaje sugerido:

“Existe un empate en el primer lugar para este puesto electivo.”

En este caso, el sistema no debe marcar un único ganador.

Botón Volver atrás

En la pantalla de resultados debe existir un botón con el texto Volver atrás.

Al hacer clic en este botón, el sistema debe regresar al listado de elecciones.

Reglas adicionales del módulo

●  Solo los usuarios con rol Administrador pueden acceder al módulo de

elecciones.

●  Una elección sólo puede tener uno de estos estados: Pendiente, Activa o

Finalizada.

●  Pueden existir múltiples elecciones en estado pendiente.
●  Solo puede existir una elección activa a la vez.
●  Al crear una elección, esta debe quedar en estado pendiente.
●  Una elección pendiente no permite votar.
●  Mientras una elección esté en estado pendiente, su conﬁguración se

considera dinámica.

●  Antes de activar una elección pendiente, el sistema debe validar nuevamente

la conﬁguración electoral actual.
●  La fecha de realización es informativa.
●  La fecha de realización no activa automáticamente la elección.
●  La activación de la elección es manual mediante el botón Activar.
●  Para permitir la votación, el administrador debe activar manualmente la

elección.

●  Al activar una elección, los ciudadanos habilitados podrán iniciar el proceso

de votación.

●  Al ﬁnalizar una elección, no se deben permitir nuevos votos.
●  El botón Crear elección sólo debe estar disponible si no existe una elección

activa.

●  El botón Activar solo debe mostrarse para elecciones en estado pendiente.
●  El botón Finalizar solo debe mostrarse para elecciones en estado Activa.

●  El botón Ver resultados solo debe mostrarse para elecciones en estado

Finalizada.

●  El sistema no debe crear copias históricas de partidos, candidatos ni puestos

al crear una elección.

●  Los resultados deben calcularse usando los datos actuales del sistema.
●  Para ﬁnes de bloqueo de campos críticos, un puesto electivo, partido político
o candidato se considera participante desde el momento en que una elección

donde aparece pasa a estado Activa.

●  No se deben permitir modiﬁcaciones a los campos principales de partidos,
candidatos o puestos que ya hayan participado en una elección activa o

ﬁnalizada.

●  La opción Ninguno debe aparecer en los resultados como una opción más

votable.

●  El porcentaje de votos debe calcularse sobre el total de votos emitidos para

cada puesto.

●  La cantidad de ciudadanos que votaron no debe confundirse con la cantidad

total de votos por puesto.

●  El listado de elecciones debe estar organizado desde la más reciente hasta la

más antigua.

●  Si existe una elección activa, debe aparecer de primera en el listado.
●  El módulo debe usar el mismo layout general de la aplicación.

Funcionalidades del Dirigente

Si el usuario que inicia sesión tiene el rol Dirigente político, el sistema debe redirigir

automáticamente al Home del dirigente.

El Home del dirigente será la pantalla principal desde donde el usuario podrá

acceder a las funcionalidades relacionadas con el partido político que tiene

asignado.

Un usuario con rol Dirigente político solo puede acceder a esta pantalla si cumple

las siguientes condiciones:

●  El usuario está activo.
●  El usuario tiene el rol Dirigente político.
●  El usuario tiene un partido político asignado.
●  El partido político asignado está activo.

Si el usuario dirigente no tiene un partido político asignado, el sistema no debe

permitirle acceder al Home del dirigente.

Mensaje sugerido:

“No tiene un partido político asignado. Por favor, póngase en contacto

con un administrador.”

Si el partido político asignado se encuentra inactivo, el sistema no debe permitirle

acceder al Home del dirigente.

Mensaje sugerido:

“El partido político asignado a este usuario se encuentra inactivo.”

Menú principal del dirigente

En el Home del dirigente se debe mostrar un menú con las opciones disponibles

para el usuario autenticado.

El menú debe contener las siguientes opciones:

Opción

Descripción

Candidatos

Permite al dirigente político gestionar los candidatos
pertenecientes a su partido político.

Asignar candidato a
puesto

Permite asignar candidatos del partido a los puestos
electivos disponibles.

Alianzas políticas

Permite gestionar solicitudes de alianzas políticas con
otros partidos.

El usuario dirigente solo podrá visualizar y administrar información relacionada con

el partido político que tiene asignado.

No se puede consultar, crear, modiﬁcar ni eliminar información perteneciente a otros

partidos políticos.

Información del partido político asignado

Además del menú, el Home del dirigente debe mostrar la información principal del

partido político al que pertenece el usuario autenticado.

La información a mostrar será la siguiente:

Campo

Descripción

Nombre del partido
político

Nombre completo del partido asignado al dirigente.

Siglas del partido político  Siglas registradas para identiﬁcar el partido.

Logo del partido político

Imagen o emblema visual del partido.

Ejemplo de visualización:

Partido Nacional Democrático (PND)

[Logo del partido]

Esta información debe obtenerse a partir de la relación existente entre el usuario

dirigente y el partido político registrada en el módulo de Asignación de dirigentes

políticos.

Indicadores del Home del dirigente

En el Home del dirigente también se deben mostrar indicadores relacionados

únicamente con el partido político asignado al usuario autenticado.

Los indicadores a mostrar son los siguientes:

Indicador

Descripción

Cantidad de candidatos
activos

Total de candidatos activos registrados para el partido
político del dirigente.

Cantidad de candidatos
inactivos

Total de candidatos inactivos registrados para el
partido político del dirigente.

Cantidad de alianzas
políticas

Total de alianzas políticas aprobadas en las que
participa el partido político del dirigente.

Indicador

Descripción

Cantidad de solicitudes

Total de solicitudes de alianza recibidas por el partido

de alianzas políticas
pendientes de responder

político del dirigente que aún no han sido aceptadas
ni rechazadas.

Cantidad de candidatos

asignados a puestos
electivos

Total de candidatos del partido que se encuentran
asignados a un puesto electivo.

Reglas para el cálculo de indicadores

Cantidad de candidatos activos

Debe contar únicamente los candidatos que pertenecen al partido político asignado

al dirigente y cuyo estado sea Activo.

No deben incluirse candidatos de otros partidos.

Cantidad de candidatos inactivos

Debe contar únicamente los candidatos que pertenecen al partido político asignado

al dirigente y cuyo estado sea Inactivo.

No deben incluirse candidatos de otros partidos.

Cantidad de alianzas políticas

Debe contar las alianzas políticas aprobadas donde participe el partido político del

dirigente, ya sea como partido solicitante o como partido receptor de la solicitud.

Solo deben contarse alianzas en estado Aceptada.

No deben contarse solicitudes pendientes ni rechazadas.

Cantidad de solicitudes de alianzas políticas pendientes de responder

Debe contar las solicitudes de alianza política que cumplan las siguientes

condiciones:

●  Fueron enviadas por otro partido político.

●  Están dirigidas al partido político del dirigente autenticado.
●  Se encuentran en estado en espera de respuesta.

No deben contarse solicitudes creadas por el propio partido del dirigente, porque

esas solicitudes están pendientes de respuesta por parte del otro partido.

Cantidad de candidatos asignados a puestos electivos

Debe contar los candidatos del partido político del dirigente que se encuentren

asignados a un puesto electivo.

La cantidad debe calcularse tomando en cuenta únicamente asignaciones vigentes

del partido político del dirigente.

No deben contarse asignaciones históricas eliminadas ni candidatos de otros

partidos.

Restricciones de acceso

El sistema debe validar que el usuario autenticado tenga rol Dirigente político

antes de permitir el acceso a este Home.

Si un usuario con rol Administrador intenta acceder directamente por URL al Home

del dirigente, el sistema debe denegar el acceso o redirigirlo al Home del

administrador.

Si un usuario no autenticado intenta acceder directamente por URL al Home del

dirigente, el sistema debe redirigirlo a la pantalla de inicio de sesión.

Si un usuario dirigente intenta consultar información de un partido político diferente

al asignado, el sistema debe impedirlo.

Mensaje sugerido:

“No tiene permisos para consultar información de este partido político.”

Reglas adicionales del módulo

●  El Home del dirigente solo debe estar disponible para usuarios con rol

Dirigente político.

●  El dirigente debe estar activo para poder acceder.
●  El dirigente debe tener un partido político asignado.
●  El partido político asignado debe estar activo.
●  La información mostrada debe pertenecer exclusivamente al partido político

asignado al dirigente autenticado.

●  El menú del dirigente debe contener las opciones Candidatos, Asignar

candidato a puesto y Alianzas políticas.

●  Los indicadores deben calcularse únicamente con datos del partido político

asignado.

●  No se deben mostrar datos de otros partidos políticos.
●  Si no existen candidatos registrados para el partido, los indicadores de

candidatos deben mostrarse en cero.

●  Si no existen alianzas políticas aprobadas, el indicador de alianzas debe

mostrarse en cero.

●  Si no existen solicitudes pendientes de responder, el indicador

correspondiente debe mostrarse en cero.

●  Si no existen candidatos asignados a puestos electivos, el indicador debe

mostrarse en cero.

●  El módulo debe usar el mismo layout general de la aplicación.

Mantenimiento de candidatos

Al ingresar a la opción Candidatos desde el menú principal del dirigente político, el

sistema debe enviar al usuario al mantenimiento de candidatos.

Este mantenimiento permitirá registrar, consultar, editar, activar y desactivar

candidatos pertenecientes únicamente al partido político asignado al dirigente

autenticado.

Los candidatos siempre deben pertenecer a un partido político. Por lo tanto, cuando

un dirigente político crea un candidato, el sistema debe asociarlo automáticamente

al partido político del dirigente que inició sesión.

El dirigente no debe poder visualizar, crear, editar, activar ni desactivar candidatos

pertenecientes a otros partidos políticos.

Pantalla inicial del mantenimiento

En la pantalla inicial del mantenimiento de candidatos, el sistema debe mostrar un

listado con todos los candidatos creados en el sistema que pertenezcan al partido

político del dirigente autenticado.

De cada candidato se debe mostrar la siguiente información:

Campo

Nombre

Apellido

Descripción

Nombre del candidato.

Apellido del candidato.

Foto del candidato

Imagen o fotografía del candidato.

Puesto electivo
asociado

Nombre del puesto electivo al que está asignado el

candidato. Si no tiene puesto asociado, debe mostrar el
texto “Sin puesto asociado”.

Estado

Indica si el candidato está activo o inactivo.

Cada candidato listado debe tener las siguientes acciones:

Acción

Descripción

Editar

Permite modiﬁcar los datos del candidato.

Activar

Permite cambiar el estado del candidato de inactivo a
activo. Solo debe mostrarse si el candidato está inactivo.

Desactivar

Permite cambiar el estado del candidato de activo a
inactivo. Solo debe mostrarse si el candidato está activo.

Arriba del listado debe existir un botón con el texto Crear candidato.

Si existe una elección activa, el sistema no debe permitir crear, editar, activar ni

desactivar candidatos. En ese caso, los botones de acción deben mostrarse

deshabilitados y debe indicarse visualmente que la acción no está disponible

mientras exista una elección activa.

Mensaje sugerido:

“No se pueden modiﬁcar candidatos mientras exista una elección activa.”

Crear candidato

Al hacer clic sobre el botón Crear candidato, el sistema debe enviar al usuario a una

pantalla con un formulario para registrar un nuevo candidato.

Esta acción solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre del
candidato

Apellido del
candidato

Texto / string

Sí

Texto / string

Sí

Foto del candidato  File / imagen

Sí

Estado

Booleano /
checkbox

Sí

Nombre del candidato que

pertenece al partido político
del dirigente.

Apellido del candidato que

pertenece al partido político
del dirigente.

Fotografía que identiﬁcará
visualmente al candidato.

Indica si el candidato estará

activo o inactivo. Por defecto,

al crear un candidato debe
venir marcado como activo.

El formulario no debe permitir seleccionar el partido político, porque este dato debe

tomarse automáticamente del partido político asignado al dirigente autenticado.

Descripción de campos

Nombre del candidato

Representa el nombre del candidato.

Ejemplo:

●  Juan
●  María
●  Carlos

●  Ana

Apellido del candidato

Representa el apellido del candidato.

Ejemplo:

●  Pérez
●  Rodríguez
●  Gómez
●  Martínez

Foto del candidato

Representa la imagen que identiﬁcará visualmente al candidato en el sistema.

La foto del candidato será utilizada en:

●  Listado de candidatos.
●  Asignación de candidatos a puestos electivos.
●  Boleta electoral mostrada al ciudadano.
●  Resultados electorales.

Formatos permitidos:

●

●

●

.jpg

.jpeg

.png

El archivo cargado debe ser una imagen válida.

Estado

Indica si el candidato está activo o inactivo.

●  Si el candidato está activo, puede ser asignado a un puesto electivo.
●  Si el candidato está inactivo, no puede ser asignado a nuevos puestos

electivos.

●  Si el candidato ya participó en una elección, debe seguir apareciendo en los

resultados correspondientes, aunque posteriormente sea inactivado.

Validaciones para crear candidato

El formulario de creación debe cumplir las siguientes validaciones:

●  El nombre del candidato es requerido.
●  El apellido del candidato es requerido.
●  La foto del candidato es requerida.
●  El archivo cargado como foto debe ser una imagen válida.
●  El estado debe manejarse como booleano.
●  Al crear un candidato, el estado debe venir marcado como activo por defecto.
●  No debe permitirse crear candidatos si existe una elección activa.
●  El usuario autenticado debe tener rol Dirigente político.
●  El usuario autenticado debe tener un partido político asignado.
●  El partido político asignado al dirigente debe estar activo.

Si el usuario intenta crear un candidato sin tener partido político asignado, el

sistema debe mostrar un mensaje como:

“No puede crear candidatos porque no tiene un partido político

asignado.”

Si el partido político asignado al dirigente está inactivo, el sistema debe mostrar un

mensaje como:

“No puede crear candidatos porque el partido político asignado se

encuentra inactivo.”

Si el usuario intenta subir un archivo que no es una imagen válida, el sistema debe

mostrar un mensaje como:

“La foto del candidato debe ser una imagen válida.”

Si el usuario intenta crear un candidato mientras existe una elección activa, el

sistema debe mostrar un mensaje como:

“No se puede crear un candidato mientras exista una elección activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de candidatos sin guardar

cambios.

●  Crear candidato: guarda el nuevo candidato en el sistema.

Al hacer clic en Crear candidato, si los datos son válidos, el sistema debe registrar

el candidato en estado activo, asociarlo automáticamente al partido político del

dirigente autenticado y redirigir al usuario a la pantalla inicial del mantenimiento.

Editar candidato

En el listado de candidatos, al presionar el botón Editar, el sistema debe enviar al

usuario a una pantalla con un formulario para modiﬁcar el candidato seleccionado.

Esta acción solo debe estar disponible si no existe una elección activa.

El sistema solo debe permitir editar candidatos que pertenezcan al partido político

asignado al dirigente autenticado.

El formulario debe mostrar los datos actuales del candidato.

El formulario de edición debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Nombre del
candidato

Apellido del
candidato

Foto del
candidato

Estado

Texto / string

Texto / string

Sí

Sí

Nombre del candidato.

Apellido del candidato.

File / imagen

No

Nueva foto del candidato. En
edición debe ser opcional.

Booleano /
checkbox

Sí

Indica si el candidato está activo o
inactivo.

Como es una edición, los campos deben venir cargados con los valores guardados

para el candidato seleccionado.

En el caso de la foto, si el usuario no carga una nueva imagen, el sistema debe

conservar la foto actual.

Validaciones para editar candidato

El formulario de edición debe cumplir las siguientes validaciones:

●  El candidato debe existir.
●  El candidato debe pertenecer al partido político del dirigente autenticado.
●  El nombre del candidato es requerido.
●  El apellido del candidato es requerido.
●  La foto del candidato es opcional en edición.
●  Si se carga una nueva foto, debe ser una imagen válida.
●  El estado debe manejarse como booleano.
●  No debe permitirse editar candidatos si existe una elección activa.
●  El usuario autenticado debe tener rol Dirigente político.
●  El usuario autenticado debe tener un partido político asignado.
●  El partido político asignado al dirigente debe estar activo.

Si el candidato no pertenece al partido político del dirigente autenticado, el sistema

debe impedir la operación y mostrar un mensaje como:

“No tiene permisos para modiﬁcar este candidato.”

Si el usuario intenta editar un candidato mientras existe una elección activa, el

sistema debe mostrar un mensaje como:

“No se puede editar un candidato mientras exista una elección activa.”

Si el usuario carga una nueva foto y el archivo no es válido, el sistema debe mostrar

un mensaje como:

“La foto del candidato debe ser una imagen válida.”

Restricciones importantes al editar

Si el candidato ya participó en una elección, el sistema no debe permitir modiﬁcar

los campos principales que se muestran en la boleta electoral y en los resultados.

Por lo tanto, si un candidato ya participó en una elección, no debe permitirse

modiﬁcar:

●  Nombre del candidato.
●  Apellido del candidato.

●  Foto del candidato.

Mensaje sugerido:

“No se pueden modiﬁcar los datos principales de este candidato porque

ya participó en una elección.”

En ese caso, el sistema solo podrá permitir modiﬁcar el estado del candidato,

siempre que no exista una elección activa y que el candidato no tenga una

asignación vigente a un puesto electivo.

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de candidatos sin guardar

cambios.

●  Guardar cambios: actualiza la información del candidato.

Al hacer clic en Guardar cambios, si los datos son válidos, el sistema debe

actualizar el candidato y redirigir al usuario a la pantalla inicial del mantenimiento.

Activar candidato

Si el candidato está inactivo, en el listado debe mostrarse un botón con el texto

Activar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea activar este candidato?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de candidatos sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado del candidato a

activo y redirigir al usuario nuevamente al listado de candidatos.

Validaciones para activar candidato

Antes de activar un candidato, el sistema debe validar:

●  El candidato debe existir.
●  El candidato debe pertenecer al partido político del dirigente autenticado.
●  El candidato debe estar actualmente inactivo.
●  No debe existir una elección activa.
●  El partido político del candidato debe estar activo.
●  El usuario autenticado debe tener rol Dirigente político.
●  El usuario autenticado debe tener asignado el mismo partido político al que

pertenece el candidato.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede activar un candidato mientras exista una elección activa.”

Si el candidato ya está activo, el sistema debe mostrar un mensaje como:

“Este candidato ya se encuentra activo.”

Si el partido político del candidato está inactivo, el sistema debe mostrar un

mensaje como:

“No se puede activar este candidato porque su partido político se

encuentra inactivo.”

Si el candidato no pertenece al partido político del dirigente autenticado, el sistema

debe mostrar un mensaje como:

“No tiene permisos para activar este candidato.”

Si todas las validaciones se cumplen, el sistema debe cambiar el estado del

candidato a activo y redirigir al usuario nuevamente al listado.

Desactivar candidato

Si el candidato está activo, en el listado debe mostrarse un botón con el texto

Desactivar.

Al presionar este botón, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desactivar este candidato?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de candidatos sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe intentar cambiar el estado del

candidato a inactivo.

Validaciones para desactivar candidato

Antes de desactivar un candidato, el sistema debe validar:

●  El candidato debe existir.
●  El candidato debe pertenecer al partido político del dirigente autenticado.
●  El candidato debe estar actualmente activo.
●  No debe existir una elección activa.
●  El candidato no debe estar asignado a ningún puesto electivo vigente.
●  El usuario autenticado debe tener rol Dirigente político.
●  El usuario autenticado debe tener asignado el mismo partido político al que

pertenece el candidato.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede desactivar un candidato mientras exista una elección

activa.”

Si el candidato está asignado a un puesto electivo vigente, el sistema no debe

permitir desactivarlo y debe mostrar el siguiente mensaje:

“No se puede desactivar este candidato porque está asignado a un

puesto electivo.”

Si el candidato ya está inactivo, el sistema debe mostrar un mensaje como:

“Este candidato ya se encuentra inactivo.”

Si el candidato no pertenece al partido político del dirigente autenticado, el sistema

debe mostrar un mensaje como:

“No tiene permisos para desactivar este candidato.”

Si todas las validaciones se cumplen, el sistema debe cambiar el estado del

candidato a inactivo y redirigir al usuario nuevamente al listado.

Reglas adicionales del mantenimiento

●  Solo los usuarios con rol Dirigente político pueden acceder al mantenimiento

de candidatos.

●  El dirigente debe tener un partido político asignado.
●  El partido político asignado al dirigente debe estar activo.
●  El dirigente solo puede gestionar candidatos pertenecientes a su partido

político.

●  El dirigente no puede ver candidatos de otros partidos políticos.
●  No se debe permitir crear, editar, activar ni desactivar candidatos mientras

exista una elección activa.

●  Todo candidato debe pertenecer a un partido político.
●  Al crear un candidato, el sistema debe asociarlo automáticamente al partido

político del dirigente autenticado.

●  El formulario de creación no debe permitir seleccionar un partido político.
●  El nombre del candidato es requerido.
●  El apellido del candidato es requerido.
●  La foto del candidato es requerida al crear.
●  La foto del candidato es opcional al editar.
●  Si en edición no se carga una nueva foto, debe conservarse la foto actual.
●  El candidato debe crearse en estado activo por defecto.
●  Si un candidato está activo, puede ser asignado a un puesto electivo.
●  Si un candidato está inactivo, no puede ser asignado a nuevos puestos

electivos.

●  Si un candidato ya participó en una elección, no se deben modiﬁcar su

nombre, apellido ni foto.

●  Si un candidato está asignado a un puesto electivo vigente, no se debe

permitir desactivarlo.

●  Para desactivar un candidato asignado, primero debe eliminarse su

asignación desde el módulo Asignar candidato a puesto.

●  Si un candidato no tiene puesto asociado, en el listado debe mostrarse el

texto “Sin puesto asociado”.

●  El módulo debe usar el mismo layout general de la aplicación.

Alianzas políticas

Al ingresar a la opción Alianzas políticas desde el menú principal del dirigente

político, el sistema debe enviar al usuario al módulo de alianzas políticas.

Este módulo permitirá al dirigente político gestionar las solicitudes de alianza entre

su partido político y otros partidos activos del sistema.

Una alianza política representa un acuerdo entre dos partidos políticos. Cuando una

solicitud de alianza es aceptada, ambos partidos quedan formalmente aliados y

podrán asignar candidatos del otro partido a puestos electivos, según las reglas

deﬁnidas en el módulo Asignar candidato a puesto.

El dirigente político sólo podrá visualizar y gestionar solicitudes o alianzas donde

participe el partido político que tiene asignado.

Pantalla inicial del módulo

La pantalla inicial del módulo de alianzas políticas debe mostrar tres secciones
principales:

Sección

Descripción

Solicitudes pendientes
de responder

Solicitudes de alianza enviadas por otros partidos al
partido del dirigente autenticado.

Solicitudes realizadas

Solicitudes de alianza enviadas por el partido del
dirigente autenticado a otros partidos.

Alianzas vigentes

Alianzas aceptadas donde participa el partido del
dirigente autenticado.

Si existe una elección activa, el sistema no debe permitir crear solicitudes, aceptar

solicitudes, rechazar solicitudes, eliminar solicitudes ni eliminar alianzas vigentes.

Mensaje sugerido:

“No se pueden modiﬁcar alianzas políticas mientras exista una elección

activa.”

Solicitudes pendientes de responder

En esta sección se deben mostrar todas las solicitudes de alianza política que otros

partidos han enviado al partido político del dirigente autenticado y que aún se

encuentren en estado En espera de respuesta.

De cada solicitud se debe mostrar la siguiente información:

Campo

Descripción

Partido solicitante

Nombre y siglas del partido político que envió la
solicitud.

Fecha de solicitud

Fecha en que se realizó la solicitud de alianza.

Estado

Debe mostrarse como “En espera de respuesta”.

Cada solicitud pendiente debe tener las siguientes acciones:

Acción

Aceptar

Descripción

Permite aceptar la solicitud de alianza política.

Rechazar

Permite rechazar la solicitud de alianza política.

Estas acciones solo deben estar disponibles si no existe una elección activa.

Aceptar solicitud de alianza

Al presionar el botón Aceptar, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea aceptar la alianza con el partido [nombre del

partido] ([siglas del partido])?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar a la pantalla inicial del

módulo sin realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado de la solicitud a

Aceptada y crear formalmente la alianza política entre ambos partidos.

Luego, el sistema debe redirigir al usuario nuevamente a la pantalla inicial del

módulo.

Validaciones para aceptar solicitud

Antes de aceptar una solicitud de alianza, el sistema debe validar:

●  La solicitud debe existir.
●  La solicitud debe estar en estado en espera de respuesta.
●  La solicitud debe estar dirigida al partido político del dirigente autenticado.
●  El partido solicitante debe estar activo.
●  El partido receptor debe estar activo.
●  No debe existir una elección activa.
●  No debe existir ya una alianza vigente entre ambos partidos.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede aceptar una solicitud de alianza mientras exista una

elección activa.”

Si la solicitud no pertenece al partido del dirigente autenticado, el sistema debe

mostrar un mensaje como:

“No tiene permisos para responder esta solicitud de alianza.”

Si la solicitud ya fue respondida, el sistema debe mostrar un mensaje como:

“Esta solicitud de alianza ya fue respondida.”

Si ya existe una alianza vigente entre ambos partidos, el sistema debe mostrar un

mensaje como:

“Ya existe una alianza vigente con este partido político.”

Rechazar solicitud de alianza

Al presionar el botón Rechazar, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea rechazar la alianza con el partido [nombre del

partido] ([siglas del partido])?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar a la pantalla inicial del

módulo sin realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe cambiar el estado de la solicitud es

rechazada.

Luego, la solicitud no debe aparecer en el listado de solicitudes pendientes de

responder y el sistema debe redirigir al usuario nuevamente a la pantalla inicial del

módulo.

Validaciones para rechazar solicitud

Antes de rechazar una solicitud de alianza, el sistema debe validar:

●  La solicitud debe existir.
●  La solicitud debe estar en estado en espera de respuesta.
●  La solicitud debe estar dirigida al partido político del dirigente autenticado.
●  No debe existir una elección activa.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede rechazar una solicitud de alianza mientras exista una

elección activa.”

Si la solicitud no pertenece al partido del dirigente autenticado, el sistema debe

mostrar un mensaje como:

“No tiene permisos para responder esta solicitud de alianza.”

Si la solicitud ya fue respondida, el sistema debe mostrar un mensaje como:

“Esta solicitud de alianza ya fue respondida.”

Solicitudes realizadas

En esta sección se deben mostrar todas las solicitudes de alianza que ha enviado el

partido político del dirigente autenticado a otros partidos políticos.

De cada solicitud enviada se debe mostrar la siguiente información:

Campo

Descripción

Partido destino

Nombre y siglas del partido político al que se envió
la solicitud.

Fecha de solicitud

Fecha en que se realizó la solicitud de alianza.

Estado

Estado actual de la solicitud.

Los estados posibles de una solicitud enviada son:

Estado

Descripción

En espera de respuesta

La solicitud fue enviada y todavía no ha sido aceptada
ni rechazada por el partido receptor.

Aceptada

El partido receptor aceptó la solicitud y se creó la
alianza política.

Rechazada

El partido receptor rechazó la solicitud.

Arriba de este listado debe colocarse un botón con el texto Crear solicitud de

alianza.

Este botón solo debe estar disponible si no existe una elección activa.

Crear solicitud de alianza

Al hacer clic sobre el botón Crear solicitud de alianza, el sistema debe enviar al

usuario a una pantalla con un formulario para registrar una nueva solicitud de

alianza política.

El formulario debe contener el siguiente campo:

Campo

Tipo de dato  Requerido

Descripción

Partido político

Select /
entero

Sí

Partido político activo al cual se
enviará la solicitud de alianza.

Descripción del campo Partido político

Este select debe mostrar únicamente partidos políticos que cumplan todas las

siguientes condiciones:

●  El partido político debe estar activo.
●  El partido político debe ser diferente al partido del dirigente autenticado.
●  No debe existir una alianza vigente entre ambos partidos.
●  No debe existir una solicitud pendiente enviada por el partido del dirigente

autenticado hacia ese partido.

●  No debe existir una solicitud pendiente enviada por ese partido hacia el

partido del dirigente autenticado.

El select debe mostrar el nombre y las siglas del partido político.

Ejemplo:

Partido Nacional Democrático (PND)

Movimiento de Unidad Ciudadana (MUC)

No deben aparecer en el select:

●  Partidos políticos inactivos.

●  El mismo partido del dirigente autenticado.
●  Partidos con los que ya exista una alianza vigente.
●  Partidos a los que ya se les envió una solicitud pendiente.
●  Partidos que ya enviaron una solicitud pendiente al partido del dirigente

autenticado.

Validaciones para crear solicitud de alianza

El formulario de creación debe cumplir las siguientes validaciones:

●  El partido político es requerido.
●  El partido seleccionado debe existir.
●  El partido seleccionado debe estar activo.
●  El partido seleccionado debe ser diferente al partido del dirigente

autenticado.

●  El partido del dirigente autenticado debe estar activo.
●  No debe existir una elección activa.
●  No debe existir una alianza vigente entre ambos partidos.
●  No debe existir una solicitud pendiente entre ambos partidos en ninguna

dirección.

Si el usuario intenta crear una solicitud hacia su mismo partido, el sistema debe

mostrar un mensaje como:

“No puede crear una solicitud de alianza hacia su propio partido político.”

Si el partido seleccionado está inactivo, el sistema debe mostrar un mensaje como:

“No puede crear una solicitud de alianza con un partido político inactivo.”

Si ya existe una alianza vigente con ese partido, el sistema debe mostrar un

mensaje como:

“Ya existe una alianza vigente con este partido político.”

Si ya existe una solicitud pendiente enviada por el partido del dirigente autenticado

hacia ese partido, el sistema debe mostrar un mensaje como:

“Ya existe una solicitud de alianza pendiente enviada a este partido

político.”

Si ya existe una solicitud pendiente enviada por ese partido hacia el partido del

dirigente autenticado, el sistema debe mostrar un mensaje como:

“Ya existe una solicitud de alianza pendiente enviada por este partido

político.”

Si el usuario intenta crear una solicitud mientras existe una elección activa, el

sistema debe mostrar un mensaje como:

“No se puede crear una solicitud de alianza mientras exista una elección

activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario a la pantalla inicial del módulo sin guardar

cambios.

●  Crear solicitud: guarda la solicitud de alianza política.

Al hacer clic en Crear solicitud, si los datos son válidos, el sistema debe registrar la

solicitud con estado En espera de respuesta y redirigir al usuario a la pantalla

inicial del módulo.

Eliminar solicitud de alianza

En el listado de solicitudes realizadas, cada solicitud debe tener un botón con el

texto Eliminar únicamente cuando la solicitud esté en estado En espera de

respuesta o Rechazada.

Las solicitudes en estado Aceptada no deben mostrar el botón Eliminar, porque ya

generaron una alianza política vigente. Para terminar una alianza vigente se debe

utilizar la acción Eliminar alianza desde el listado de alianzas vigentes.

Al presionar el botón Eliminar, el sistema debe enviar al usuario a una pantalla de

conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea eliminar la solicitud de alianza con el partido

[nombre del partido] ([siglas del partido])?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar a la pantalla inicial del

módulo sin eliminar la solicitud.

Si el usuario pulsa Aceptar, el sistema debe eliminar la solicitud de alianza y

redirigir al usuario nuevamente a la pantalla inicial del módulo.

Validaciones para eliminar solicitud

Antes de eliminar una solicitud de alianza, el sistema debe validar:

●  La solicitud debe existir.
●  La solicitud debe haber sido enviada por el partido político del dirigente

autenticado.

●  La solicitud debe estar en estado En espera de respuesta o Rechazada.
●  No debe existir una elección activa.
●  La solicitud no debe estar en estado Aceptada.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede eliminar una solicitud de alianza mientras exista una

elección activa.”

Si la solicitud no pertenece al partido del dirigente autenticado, el sistema debe

mostrar un mensaje como:

“No tiene permisos para eliminar esta solicitud de alianza.”

Si la solicitud está en estado Aceptada, el sistema debe mostrar un mensaje como:

“No se puede eliminar una solicitud aceptada porque ya generó una

alianza vigente. Para terminarla debe eliminar la alianza desde el listado

de alianzas vigentes.”

Si la solicitud no existe o ya fue eliminada, el sistema debe mostrar un mensaje

como:

“La solicitud de alianza seleccionada no existe o ya fue eliminada.”

Alianzas vigentes

En esta sección se deben mostrar todas las alianzas políticas vigentes donde

participe el partido político del dirigente autenticado.

Una alianza se considera vigente cuando existe una solicitud de alianza en estado

aceptada entre dos partidos políticos activos.

De cada alianza vigente se debe mostrar la siguiente información:

Campo

Descripción

Partido aliado

Nombre y siglas del partido aliado.

Fecha de aceptación

Fecha en que fue aceptada la solicitud de alianza.

Cada alianza vigente debe tener la siguiente acción:

Acción

Eliminar alianza

Descripción

Permite terminar la alianza vigente entre el partido
del dirigente autenticado y el partido aliado.

La acción Eliminar alianza solo debe estar disponible si no existe una elección

activa.

Eliminar alianza vigente

Al presionar el botón Eliminar alianza, el sistema debe enviar al usuario a una

pantalla de conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea eliminar la alianza política con el partido

[nombre del partido] ([siglas del partido])?”

Debajo del mensaje debe haber dos botones:

●  Cancelar

●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar a la pantalla inicial del

módulo sin eliminar la alianza.

Si el usuario pulsa Aceptar, el sistema debe intentar eliminar la alianza vigente

entre ambos partidos.

Eliminar una alianza vigente signiﬁca que ambos partidos dejarán de poder utilizar

candidatos del otro partido para nuevas asignaciones.

La eliminación de la alianza no debe eliminar partidos políticos, candidatos ni votos

registrados.

Validaciones para eliminar alianza vigente

Antes de eliminar una alianza vigente, el sistema debe validar:

●  La alianza debe existir.
●  La alianza debe estar vigente.
●  La alianza debe involucrar al partido político del dirigente autenticado.
●  No debe existir una elección activa.
●  No deben existir candidatos aliados asignados entre los partidos

relacionados.

Si existe una elección activa, el sistema debe mostrar un mensaje como:

“No se puede eliminar una alianza política mientras exista una elección

activa.”

Si la alianza no pertenece al partido del dirigente autenticado, el sistema debe

mostrar un mensaje como:

“No tiene permisos para eliminar esta alianza política.”

Si existen candidatos aliados asignados entre los partidos relacionados, el sistema

no debe permitir eliminar la alianza y debe mostrar el siguiente mensaje:

“No se puede eliminar esta alianza porque existen candidatos aliados

asignados entre estos partidos. Primero deben eliminarse las

asignaciones correspondientes desde el módulo Asignar candidato a

puesto.”

Si la alianza no existe o ya fue eliminada, el sistema debe mostrar un mensaje como:

“La alianza política seleccionada no existe o ya fue eliminada.”

Si todas las validaciones se cumplen, el sistema debe eliminar la alianza vigente y

redirigir al usuario nuevamente a la pantalla inicial del módulo.

Efecto de eliminar una alianza vigente

Cuando se elimina una alianza vigente:

●  Los partidos dejan de estar aliados.
●  Ninguno de los dos partidos podrá asignar nuevos candidatos del otro

partido.

●  Los candidatos propios de cada partido no se eliminan.
●  Los partidos políticos no se eliminan.
●  Las solicitudes históricas pueden conservarse para ﬁnes de consulta o

auditoría.

●  Los votos ya emitidos no deben alterarse.
●  Los resultados de elecciones ﬁnalizadas no deben modiﬁcarse.
●  Las asignaciones propias de cada partido no se ven afectadas.

Si antes de eliminar la alianza existían candidatos aliados asignados entre ambos

partidos, el sistema debe impedir la eliminación hasta que dichas asignaciones sean

eliminadas desde el módulo Asignar candidato a puesto.

Restricciones de acceso

El sistema debe validar que el usuario autenticado tenga rol Dirigente político

antes de permitir el acceso al módulo de alianzas políticas.

Además, el usuario debe tener un partido político asignado y dicho partido debe

estar activo.

Si el usuario no tiene partido político asignado, el sistema debe impedir el acceso al

módulo.

Mensaje sugerido:

“No tiene un partido político asignado. Por favor, póngase en contacto

con un administrador.”

Si el partido político asignado está inactivo, el sistema debe impedir el acceso al

módulo.

Mensaje sugerido:

“El partido político asignado a este usuario se encuentra inactivo.”

Reglas adicionales del módulo

●  Solo los usuarios con rol Dirigente político pueden acceder al módulo de

alianzas políticas.

●  El dirigente debe tener un partido político asignado.
●  El partido político asignado al dirigente debe estar activo.
●  El dirigente solo puede gestionar solicitudes y alianzas donde participe su

partido político.

●  No se deben mostrar solicitudes ni alianzas de otros partidos.
●  No se debe permitir crear, aceptar, rechazar ni eliminar solicitudes mientras

exista una elección activa.

●  No se debe permitir eliminar alianzas vigentes mientras exista una elección

activa.

●  Una solicitud de alianza solo puede tener uno de estos estados: En espera de

respuesta, Aceptada o Rechazada.

●  No se puede crear una solicitud de alianza hacia el mismo partido del

dirigente autenticado.

●  No se puede crear una solicitud si ya existe una alianza vigente entre ambos

partidos.

●  No se puede crear una solicitud si ya existe una solicitud pendiente entre

ambos partidos en cualquier dirección.

●  Aceptar una solicitud cambia su estado a Aceptada y crea una alianza

vigente entre ambos partidos.

●  Rechazar una solicitud cambia su estado a Rechazada.
●  Las solicitudes rechazadas no deben aparecer en el listado de pendientes de

responder.

●  Las solicitudes aceptadas no deben aparecer en el listado de pendientes de

responder.

●  Las solicitudes aceptadas sí deben aparecer en el listado de solicitudes

realizadas con estado Aceptada, cuando fueron enviadas por el partido del

dirigente.

●  Solo se pueden eliminar solicitudes realizadas en estado En espera de

respuesta o Rechazada.

●  No se pueden eliminar solicitudes aceptadas desde el listado de solicitudes

realizadas.

●  Las solicitudes aceptadas generan alianzas vigentes.
●  Las alianzas vigentes deben mostrarse en el tercer listado.
●  Una alianza vigente permite que ambos partidos puedan asignar candidatos
del otro partido a puestos electivos, según las reglas del módulo Asignar

candidato a puesto.

●  Una alianza vigente puede eliminarse desde el listado de alianzas vigentes.
●  No se puede eliminar una alianza vigente si existen candidatos aliados

asignados entre los partidos relacionados.

●  Para eliminar una alianza con candidatos aliados asignados, primero deben
eliminarse las asignaciones correspondientes desde el módulo Asignar

candidato a puesto.

●  Eliminar una alianza no elimina los partidos políticos relacionados.
●  Eliminar una alianza no elimina candidatos.
●  Eliminar una alianza no afecta votos emitidos ni resultados de elecciones

ﬁnalizadas.

●  El módulo debe usar el mismo layout general de la aplicación.

Asignar candidato a puesto

Al ingresar a la opción Asignar candidato a puesto desde el menú principal del

dirigente político, el sistema debe enviar al usuario a la pantalla de asignación de

candidatos a puestos electivos.

La ﬁnalidad de este módulo es permitir que el dirigente político asigne candidatos a

los puestos electivos disponibles para su partido.

El dirigente sólo podrá administrar asignaciones correspondientes al partido político

que tiene asignado. No debe poder visualizar, crear ni eliminar asignaciones de otros

partidos políticos.

Este módulo también debe permitir asignar candidatos de partidos aliados, siempre

que exista una alianza política vigente y se cumplan las reglas deﬁnidas para

candidaturas aliadas.

Pantalla inicial del módulo

En la pantalla inicial del módulo se debe mostrar una tabla con las asignaciones de

candidatos a puestos electivos correspondientes al partido político del dirigente

autenticado.

De cada asignación se debe mostrar la siguiente información:

Campo

Descripción

Nombre del candidato

Nombre del candidato asignado.

Apellido del candidato

Apellido del candidato asignado.

Partido de origen del

Partido político al que pertenece originalmente el

candidato

candidato.

Puesto electivo asociado

Nombre del puesto electivo al que está asignado el

candidato dentro del partido del dirigente.

Tipo de candidatura

Indica si el candidato es propio o aliado.

Cada asignación debe tener la siguiente acción:

Acción

Descripción

Eliminar relación

Permite desvincular el candidato del puesto electivo
dentro del partido del dirigente autenticado.

Arriba de la tabla debe existir un botón con el texto Agregar asignación.

Si existe una elección activa, el sistema no debe permitir crear ni eliminar

asignaciones de candidatos a puestos electivos. En ese caso, el botón Agregar

asignación y los botones de eliminación deben ocultarse o mostrarse

deshabilitados.

Mensaje sugerido:

“No se pueden modiﬁcar asignaciones de candidatos a puestos mientras

exista una elección activa.”

Agregar asignación

Al hacer clic sobre el botón Agregar asignación, el sistema debe enviar al usuario a

una pantalla con un formulario para crear una nueva relación entre un candidato y

un puesto electivo.

Esta acción solo debe estar disponible si no existe una elección activa.

El formulario debe contener los siguientes campos:

Campo

Tipo de dato

Requerido

Descripción

Candidato político

Puesto electivo

Select /
entero

Select /
entero

Sí

Sí

Candidato activo disponible

para ser asignado al partido
del dirigente.

Puesto electivo activo

disponible para el partido del
dirigente.

Descripción del campo Candidato político

El campo Candidato político debe ser un select que muestre los candidatos

disponibles para ser asignados por el partido político del dirigente autenticado.

El select puede incluir dos tipos de candidatos:

Tipo de candidato

Descripción

Candidato propio

Candidato aliado

Candidato activo perteneciente al partido político del

dirigente autenticado.

Candidato activo perteneciente a un partido político

aliado vigente.

Candidatos propios disponibles

Un candidato propio es aquel que pertenece directamente al partido político del

dirigente autenticado.

El sistema debe mostrar en el select únicamente candidatos propios que cumplan

todas las condiciones siguientes:

●  El candidato debe estar activo.
●  El candidato debe pertenecer al partido político del dirigente autenticado.
●  El candidato no debe estar asignado actualmente a ningún puesto electivo

dentro del partido del dirigente.
●  No debe existir una elección activa.

Ejemplo:

Si el partido A tiene al candidato Juan Pérez asignado al puesto Diputado, ese

candidato no debe aparecer nuevamente en el select para el partido A, porque

dentro de un mismo partido un candidato no puede aspirar a más de un puesto

electivo.

Mensaje sugerido si se intenta seleccionar manualmente un candidato propio ya

asignado:

“Este candidato ya está asignado a un puesto dentro del partido.”

Candidatos aliados disponibles

Un candidato aliado es aquel que pertenece a un partido político aliado al partido

del dirigente autenticado.

El sistema debe mostrar candidatos aliados únicamente cuando exista una alianza

política vigente entre ambos partidos.

Un candidato aliado solo podrá ser asignado por el partido del dirigente si cumple

todas las condiciones siguientes:

●  El candidato debe estar activo.
●  El partido de origen del candidato debe estar activo.
●  Debe existir una alianza política vigente entre el partido del dirigente y el

partido de origen del candidato.

●  El candidato debe estar previamente asignado a un puesto electivo en su

partido de origen.

●  El puesto electivo seleccionado por el dirigente debe ser exactamente el

mismo puesto al que el candidato aspira en su partido de origen.

●  El candidato no debe estar ya asignado a otro puesto dentro del partido del

dirigente autenticado.

●  El puesto seleccionado no debe tener ya otro candidato asignado dentro del

partido del dirigente autenticado.
●  No debe existir una elección activa.

Ejemplo válido:

El Partido B tiene al candidato Juan Pérez asignado al puesto de Diputado.

El Partido A tiene una alianza vigente con el Partido B.

El Partido A puede asignar a Juan Pérez al puesto de Diputado.

Ejemplo no válido:

El Partido B tiene al candidato Juan Pérez asignado al puesto de Diputado.

El Partido A tiene una alianza vigente con el Partido B.

El Partido A intenta asignar a Juan Pérez al puesto de Senador.

En este caso, el sistema debe impedir la asignación y mostrar el mensaje:

“Este candidato en su partido de origen aspira a un puesto diferente al

seleccionado.”

Ejemplo no válido:

El Partido B tiene al candidato Carlos Gómez creado, pero no está asignado a

ningún puesto en su partido de origen.

El Partido A tiene una alianza vigente con el Partido B.

El Partido A intenta asignar a Carlos Gómez al puesto de Diputado.

En este caso, el sistema debe impedir la asignación y mostrar el mensaje:

“Este candidato aliado no tiene un puesto asignado en su partido de

origen.”

Descripción del campo Puesto electivo

El campo Puesto electivo debe ser un select que muestre los puestos electivos

activos disponibles para el partido político del dirigente autenticado.

El sistema debe mostrar únicamente puestos que cumplan las siguientes

condiciones:

●  El puesto electivo debe estar activo.
●  El puesto electivo no debe tener ya un candidato asignado dentro del partido

del dirigente autenticado.

●  No debe existir una elección activa.

Ejemplo:

Si el partido del dirigente ya tiene un candidato asignado al puesto Diputado, el

puesto Diputado no debe aparecer nuevamente en el select.

Esto evita que un mismo partido tenga más de un candidato para el mismo puesto

electivo.

Mensaje sugerido si se intenta seleccionar manualmente un puesto ya ocupado

dentro del partido:

“Este puesto electivo ya tiene un candidato asignado dentro del partido.”

Validaciones para agregar asignación

El formulario de creación debe cumplir las siguientes validaciones:

●  El candidato político es requerido.
●  El puesto electivo es requerido.
●  El candidato debe existir.
●  El candidato debe estar activo.
●  El puesto electivo debe existir.
●  El puesto electivo debe estar activo.
●  El usuario autenticado debe tener rol Dirigente político.
●  El usuario autenticado debe tener un partido político asignado.
●  El partido político del dirigente debe estar activo.
●  No debe existir una elección activa.
●  El puesto seleccionado no debe tener ya un candidato asignado dentro del

partido del dirigente.

●  El candidato seleccionado no debe estar asignado a otro puesto dentro del

partido del dirigente.

●  Si el candidato pertenece al partido del dirigente, se considera candidato

propio.

●  Si el candidato no pertenece al partido del dirigente, debe existir una alianza

política vigente entre ambos partidos.

●  Si el candidato es aliado, debe tener una asignación previa en su partido de

origen.

●  Si el candidato es aliado, el puesto seleccionado debe ser el mismo puesto al

que aspira en su partido de origen.

Si el candidato propio ya está asignado a un puesto dentro del partido, el sistema

debe mostrar el siguiente mensaje:

“Este candidato ya está asignado a un puesto dentro del partido.”

Si el puesto ya tiene un candidato asignado dentro del partido del dirigente, el

sistema debe mostrar el siguiente mensaje:

“Este puesto electivo ya tiene un candidato asignado dentro del partido.”

Si el candidato pertenece a un partido que no tiene alianza vigente con el partido

del dirigente, el sistema debe mostrar un mensaje como:

“No existe una alianza vigente con el partido de este candidato.”

Si el candidato aliado no tiene puesto asignado en su partido de origen, el sistema

debe mostrar el siguiente mensaje:

“Este candidato aliado no tiene un puesto asignado en su partido de

origen.”

Si el candidato aliado está asignado en su partido de origen a un puesto diferente al

seleccionado, el sistema debe mostrar el siguiente mensaje:

“Este candidato en su partido de origen aspira a un puesto diferente al

seleccionado.”

Si el usuario intenta crear una asignación mientras existe una elección activa, el

sistema debe mostrar el siguiente mensaje:

“No se puede asignar candidatos a puestos mientras exista una elección

activa.”

Al ﬁnal del formulario debe haber dos botones:

●  Volver atrás: devuelve al usuario al listado de asignaciones sin guardar

cambios.

●  Crear asignación: guarda la relación entre candidato y puesto electivo.

Al hacer clic en Crear asignación, si los datos son válidos, el sistema debe registrar

la asignación entre el candidato seleccionado y el puesto electivo seleccionado para

el partido político del dirigente autenticado.

Luego, el sistema debe redirigir al usuario a la pantalla inicial del módulo.

Eliminar asignación

En el listado de relaciones entre candidatos y puestos electivos, cada ﬁla debe tener

un botón con el texto Eliminar relación.

Esta acción sólo debe estar disponible si no existe una elección activa.

Al presionar el botón Eliminar relación, el sistema debe enviar al usuario a una

pantalla de conﬁrmación.

La pantalla debe mostrar el siguiente mensaje:

“¿Está seguro que desea desvincular este candidato de este puesto

electivo?”

Debajo del mensaje debe haber dos botones:

●  Cancelar
●  Aceptar

Si el usuario pulsa Cancelar, el sistema debe regresar al listado de asignaciones sin

realizar cambios.

Si el usuario pulsa Aceptar, el sistema debe eliminar la relación entre el candidato y

el puesto electivo dentro del partido político del dirigente autenticado.

Luego, el sistema debe redirigir al usuario nuevamente a la pantalla inicial del

módulo.

Validaciones para eliminar asignación

Antes de eliminar una asignación, el sistema debe validar:

●  La asignación debe existir.
●  La asignación debe pertenecer al partido político del dirigente autenticado.
●  El usuario autenticado debe tener rol Dirigente político.
●  El usuario autenticado debe tener un partido político asignado.
●  No debe existir una elección activa.

Si la asignación no pertenece al partido político del dirigente autenticado, el sistema

debe impedir la operación y mostrar un mensaje como:

“No tiene permisos para eliminar esta asignación.”

Si existe una elección activa, el sistema debe mostrar el siguiente mensaje:

“No se puede eliminar una asignación mientras exista una elección

activa.”

Si la asignación no existe o ya fue eliminada, el sistema debe mostrar un mensaje

como:

“La asignación seleccionada no existe o ya fue eliminada.”

Reglas para candidatos propios y aliados

Candidato propio

Un candidato propio es aquel que pertenece al mismo partido político del dirigente

autenticado.

Reglas:

●  Puede ser asignado a un puesto electivo activo.
●  Solo puede tener un puesto electivo asignado dentro de su partido.
●  No puede aspirar a más de un puesto dentro del mismo partido.
●  No puede ser asignado si está inactivo.

●  No puede ser asignado si ya tiene un puesto dentro del partido.

Candidato aliado

Un candidato aliado es aquel que pertenece a un partido político aliado vigente.

Reglas:

●  Solo puede ser utilizado si existe una alianza vigente entre ambos partidos.
●  Solo puede ser asignado si está activo.
●  Solo puede ser asignado si su partido de origen está activo.
●  Solo puede ser asignado si ya tiene un puesto asignado en su partido de

origen.

●  Solo puede ser asignado al mismo puesto electivo que ocupa en su partido

de origen.

●  No puede ser asignado a un puesto diferente.
●  No puede ser usado para ocupar un puesto que ya tenga candidato dentro

del partido del dirigente.

●  No puede ocupar más de un puesto dentro del partido del dirigente.

Reglas adicionales del módulo

●  Solo los usuarios con rol Dirigente político pueden acceder al módulo

Asignar candidato a puesto.

●  El dirigente debe tener un partido político asignado.
●  El partido político asignado al dirigente debe estar activo.
●  El dirigente solo puede ver asignaciones de su partido político.
●  No se debe permitir agregar ni eliminar asignaciones mientras exista una

elección activa.

●  Solo pueden asignarse candidatos activos.
●  Solo pueden asignarse puestos electivos activos.
●  Un partido político no puede tener más de un candidato asignado al mismo

puesto electivo.

●  Un candidato no puede aspirar a más de un puesto dentro del mismo partido.
●  Un candidato propio no puede aparecer en el select si ya está asignado a un

puesto dentro del partido.

●  Un puesto electivo no puede aparecer en el select si ya tiene candidato

asignado dentro del partido.

●  Los candidatos aliados solo deben aparecer si existe una alianza política

vigente.

●  Un candidato aliado solo puede ser asignado si ya tiene un puesto asignado

en su partido de origen.

●  Un candidato aliado solo puede ser asignado al mismo puesto que ocupa en

su partido de origen.

●  Si se elimina una asignación, solo se elimina la relación entre el candidato y
el puesto dentro del partido del dirigente; no se elimina el candidato ni el

puesto electivo.

●  El módulo debe usar el mismo layout general de la aplicación.
●  Si una asignación corresponde a un candidato aliado, dicha asignación impide
eliminar la alianza política entre el partido del dirigente y el partido de origen

del candidato.

Consideraciones generales

Las siguientes consideraciones se aplican de manera transversal a todos los

módulos del sistema eVote360. Estas reglas deben ser respetadas durante el

desarrollo para garantizar consistencia funcional, integridad de la información,

seguridad y transparencia en el proceso electoral.

Reglas generales de ciudadanos

El número de documento de identidad debe ser único por ciudadano.

No puede existir más de un ciudadano registrado con el mismo número de

documento de identidad.

El número de documento debe almacenarse como texto, no como número, para

evitar pérdida de ceros iniciales o problemas de formato.

Si un ciudadano ya participó en una elección, el sistema no debe permitir modiﬁcar

su número de documento de identidad.

Mensaje sugerido:

“No se puede modiﬁcar el número de documento de identidad de este

ciudadano porque ya participó en una elección.”

Si un ciudadano se encuentra inactivo, no podrá participar en ningún proceso de

votación.

Reglas generales de candidatos

Todo candidato debe estar asociado obligatoriamente a un partido político.

No debe existir un candidato sin partido político.

Cuando un dirigente político crea un candidato, el sistema debe asociarlo

automáticamente al partido político asignado al dirigente autenticado.

Un candidato no puede aspirar a más de un puesto electivo dentro del mismo

partido político.

Ejemplo:

Si el partido “A” tiene al candidato “Juan Pérez” postulado como Diputado,

Ese mismo candidato no puede ser postulado como Senador dentro del partido

“A”.

Si un candidato ya participó en una elección, el sistema no debe permitir modiﬁcar

los datos principales que se muestran en la boleta electoral o en los resultados.

Los campos que no deben modiﬁcarse son:

●  Nombre del candidato.
●  Apellido del candidato.
●  Foto del candidato.

Mensaje sugerido:

“No se pueden modiﬁcar los datos principales de este candidato porque

ya participó en una elección.”

Si un candidato está asignado a un puesto electivo vigente, no se debe permitir

desactivarlo.

Primero debe eliminarse la asignación desde el módulo Asignar candidato a

puesto.

Mensaje sugerido:

“No se puede desactivar este candidato porque está asignado a un

puesto electivo.”

Reglas generales de puestos electivos

Debe existir al menos un puesto electivo activo para poder crear una elección.

Si un puesto electivo ya fue incluido en una elección, el sistema no debe permitir

modiﬁcar su nombre.

Mensaje sugerido:

“No se puede modiﬁcar el nombre de este puesto electivo porque ya fue

utilizado en una elección.”

Si un puesto electivo tiene candidatos asignados, no debe permitirse desactivarlo.

Primero deben eliminarse las asignaciones de candidatos asociadas a ese puesto.

Mensaje sugerido:

“No se puede desactivar este puesto electivo porque tiene candidatos

asignados.”

Un puesto electivo inactivo no debe aparecer en nuevas asignaciones ni en nuevas

conﬁguraciones electorales.

Reglas generales de partidos políticos

Deben existir al menos dos partidos políticos activos para poder crear una elección.

Todo partido político debe tener siglas únicas dentro del sistema.

Si un partido político ya participó en una elección, el sistema no debe permitir

modiﬁcar los datos principales que se muestran en la boleta electoral o en los

resultados.

Los campos que no deben modiﬁcarse son:

●  Nombre del partido.
●  Siglas del partido.

●  Logo del partido.

Mensaje sugerido:

“No se pueden modiﬁcar los datos principales de este partido político

porque ya participó en una elección.”

Si un partido político tiene candidatos activos registrados, no debe permitirse

desactivarlo.

Primero deben desactivarse o eliminarse las relaciones correspondientes de esos

candidatos, según las reglas del módulo de candidatos y asignaciones.

Mensaje sugerido:

“No se puede desactivar este partido político porque tiene candidatos

activos registrados.”

Un partido político inactivo no debe aparecer en nuevos formularios de asignación,

alianzas, candidaturas o creación de elecciones.

Opción “Ninguno” en la votación

En el listado de candidatos que se muestra al elector durante el proceso de

votación, el sistema debe incluir una opción adicional llamada Ninguno.

Esta opción permite que el elector decida no votar por ningún candidato para un

puesto electivo especíﬁco.

La opción Ninguno debe mostrarse para cada puesto electivo disponible en la

elección activa.

La opción Ninguno debe contar como una opción votable válida.

Por lo tanto:

●  Debe poder seleccionarse durante el proceso de votación.
●  Debe guardarse como parte del voto emitido.
●  Debe aparecer en los resultados de la elección.
●  Debe incluirse en el cálculo de porcentajes.
●  Puede quedar en primer lugar si obtiene más votos que los candidatos.

Ejemplo:

Opción

Votos

Porcentaje

Juan Pérez

María Gómez

Ninguno

45

35

20

45%

35%

20%

Requisitos para crear una nueva elección

Para poder crear una nueva elección deben cumplirse todas las siguientes

condiciones:

●  No debe existir ninguna elección activa en el sistema.
●  Debe existir al menos un puesto electivo creado y activo.
●  Deben existir al menos dos partidos políticos creados y activos.
●  Cada partido político activo debe tener candidatos activos asignados para

todos los puestos electivos activos.

●  Cada puesto electivo activo debe tener un candidato asignado por cada

partido político participante.

●  No deben existir inconsistencias en las asignaciones de candidatos a puestos.
●  La elección nueva debe crearse inicialmente en estado pendiente.

Si no existe al menos un puesto electivo activo, el sistema debe mostrar el siguiente

mensaje:

“No hay puestos electivos activos para realizar una elección.”

Si no existen al menos dos partidos políticos activos, el sistema debe mostrar el

siguiente mensaje:

“No hay suﬁcientes partidos políticos para realizar una elección.”

Si un partido político activo no tiene candidatos registrados para todos los puestos

electivos activos, el sistema debe mostrar un mensaje con el siguiente formato:

“El partido político [nombre del partido] ([siglas del partido]) no tiene

candidatos registrados para los siguientes puestos electivos: [listado de

puestos].”

Estados de una elección

Toda elección debe manejar uno de los siguientes estados:

Estado

Pendiente

Activa

Finalizada

Descripción

La elección fue creada, pero todavía no está disponible

para votación.

La elección está disponible para que los ciudadanos

puedan votar. Solo puede existir una elección activa
a la vez.

La elección terminó y sus resultados pueden ser

consultados.

Solo puede existir una elección activa a la vez.

Una elección pendiente no permite votación.

Una elección activa permite la votación.

Una elección ﬁnalizada no permite nuevos votos.

Reglas sobre resultados electorales

El sistema no debe crear copias históricas de partidos políticos, candidatos ni

puestos electivos al momento de crear una elección.

Los resultados de elecciones ﬁnalizadas deben calcularse utilizando los datos

actuales registrados en el sistema.

Para evitar inconsistencias históricas, una vez que un puesto electivo, partido

político o candidato haya participado en una elección, el sistema debe bloquear la

modiﬁcación de los campos principales que se muestran en la boleta electoral y en

los resultados.

Esto signiﬁca que:

●  Si un puesto participó en una elección, no puede modiﬁcarse su nombre.
●  Si un partido participó en una elección, no puede modiﬁcarse su nombre,

siglas ni logo.

●  Si un candidato participó en una elección, no puede modiﬁcarse su nombre,

apellido ni foto.

El estado actual de una entidad no debe impedir que sus votos anteriores sean

contados.

Por ejemplo:

●  Si un ciudadano votó en una elección y luego fue inactivado, sus votos deben

seguir contándose.

●  Si un candidato participó en una elección y luego fue inactivado, sus votos

deben seguir apareciendo en los resultados.

●  Si un partido participó en una elección y luego fue inactivado, sus resultados

deben seguir visibles.

●  Si un puesto electivo fue utilizado en una elección y luego fue inactivado, sus

resultados deben seguir visibles.

La inactivación de una entidad solo afecta operaciones futuras, no los resultados ya

generados.

Reglas de acceso al proceso de votación

La pantalla inicial destinada al proceso de votación del elector no debe ser utilizada

por usuarios administrativos o dirigentes políticos.

Si un usuario con rol Administrador intenta acceder a la pantalla de votación del

elector, el sistema debe redirigir automáticamente al Home del administrador.

Si un usuario con rol Dirigente político intenta acceder a la pantalla de votación del

elector, el sistema debe redirigir automáticamente al Home del dirigente.

Los electores no deben iniciar sesión con usuario y contraseña. Su acceso al proceso

de votación se realiza mediante:

●  Número de documento de identidad.

●  Validación de cédula mediante OCR.
●  Código de veriﬁcación enviado al correo electrónico.

Reglas de seguridad y autorización

El sistema debe garantizar que ningún usuario no autorizado pueda acceder a

funcionalidades restringidas.

Esto aplica incluso si el usuario conoce directamente la URL de una pantalla interna.

Las funcionalidades administrativas solo deben estar disponibles para usuarios con

rol Administrador.

Las funcionalidades del dirigente político solo deben estar disponibles para

usuarios con rol Dirigente político.

El sistema debe impedir que un administrador acceda a funcionalidades exclusivas

de dirigentes políticos.

El sistema debe impedir que un dirigente político acceda a funcionalidades

exclusivas de administradores.

Si un usuario intenta acceder a una sección para la cual no tiene permisos, el

sistema debe redirigirlo a una pantalla de acceso denegado.

La pantalla de acceso denegado debe mostrar un mensaje como:

“No posee permisos para acceder a esta sección.”

Además, debe incluir un enlace que lleve al usuario a su pantalla de inicio

correspondiente.

Ejemplos:

●  Si el usuario es administrador, el enlace debe llevar al Home del

administrador.

●  Si el usuario es dirigente político, el enlace debe llevar al Home del

dirigente.

●  Si el usuario no está autenticado, debe ser redirigido al inicio de sesión.

Reglas de eliminación lógica

Todos los procesos de eliminación en el sistema deben realizarse de manera lógica.

Esto signiﬁca que las entidades no deben eliminarse físicamente de la base de

datos, sino que deben manejar un campo de estado que indique si se encuentran

activas o inactivas.

Estados generales:

Estado

Descripción

Activo

La entidad puede utilizarse en nuevos procesos del sistema.

Inactivo

La entidad no puede utilizarse en nuevos procesos, pero se
conserva para ﬁnes históricos o de auditoría.

Una entidad inactiva no debe aparecer en nuevos formularios de selección ni en

nuevas conﬁguraciones.

Sin embargo, una entidad inactiva sí puede aparecer en resultados históricos si

participó en una elección anterior.

Reglas de inactivación

La inactivación de una entidad no debe provocar eliminación física de datos.

Tampoco debe generar cambios automáticos que alteren resultados electorales o

relaciones históricas.

Las reglas generales son las siguientes:

Ciudadanos

Si un ciudadano se inactiva:

●  No podrá participar en nuevas votaciones.
●  Sus votos emitidos anteriormente deben seguir contándose.
●  No debe eliminarse su historial de participación.

Candidatos

Si un candidato se inactiva:

●  No podrá ser asignado a nuevos puestos electivos.
●  No aparecerá en nuevas boletas electorales.
●  Sus votos anteriores deben seguir contándose.
●  No debe permitirse inactivarlo si está asignado a un puesto electivo vigente.

Puestos electivos

Si un puesto electivo se inactiva:

●  No podrá utilizarse en nuevas elecciones.
●  No aparecerá en nuevas asignaciones.
●  Sus resultados anteriores deben seguir mostrándose.
●  No debe permitirse inactivarlo si tiene candidatos asignados.

Partidos políticos

Si un partido político se inactiva:

●  No podrá participar en nuevas elecciones.
●  No podrá crear candidatos.
●  No podrá solicitar ni recibir nuevas alianzas.
●  No aparecerá en nuevos formularios de asignación.
●  Sus resultados anteriores deben seguir mostrándose.
●  No debe permitirse inactivarlo si tiene candidatos activos registrados.

Reglas de alianzas políticas

Una alianza política representa una relación aceptada entre dos partidos políticos

activos.

Si dos partidos tienen una alianza vigente, ambos partidos podrán utilizar

candidatos del otro partido, cumpliendo las reglas de asignación de candidatos

aliados.

Un partido no puede enviarse una solicitud de alianza a sí mismo.

No puede existir más de una solicitud pendiente entre dos partidos políticos en

ninguna dirección.

Si el partido A recibe una solicitud de alianza del partido B y aún no la ha

respondido, el partido A no podrá crear una nueva solicitud hacia el partido B.

De igual forma, si el partido A ya envió una solicitud de alianza al partido B y esta

no ha sido respondida, no podrá generar una nueva solicitud hacia el partido B

hasta que la anterior haya sido aceptada o rechazada.

Una solicitud de alianza puede tener los siguientes estados:

Estado

Descripción

En espera de respuesta

La solicitud fue enviada y todavía no ha sido
respondida.

Aceptada

El partido receptor aceptó la solicitud y se generó una
alianza vigente.

Rechazada

El partido receptor rechazó la solicitud.

No se pueden crear, aceptar, rechazar ni eliminar solicitudes de alianzas mientras

exista una elección activa.

Reglas de candidatos aliados

Un candidato proveniente de un partido aliado puede ser postulado por otro partido

con el que exista una alianza vigente, siempre que aspire exactamente al mismo

puesto electivo que tiene en su partido de origen.

Un candidato aliado solo puede ser asignado por otro partido si ya tiene un puesto

asignado en su partido de origen.

Ejemplo válido:

El partido “B” tiene a “Juan Pérez” asignado como Diputado.

El partido “A” tiene una alianza vigente con el partido “B”.

El partido “A” puede asignar a “Juan Pérez” como Diputado.

Ejemplo no válido:

El partido “B” tiene a “Juan Pérez” asignado como Diputado.

El partido “A” intenta asignarlo como Senador.

En este caso, el sistema debe mostrar el siguiente mensaje:

“Este candidato en su partido de origen aspira a un puesto diferente al

seleccionado.”

Ejemplo no válido:

El partido “B” tiene a “Carlos Gómez” como candidato, pero no está asignado a

ningún puesto.

El partido “A” intenta asignarlo como Diputado.

En este caso, el sistema debe mostrar el siguiente mensaje:

“Este candidato aliado no tiene un puesto asignado en su partido de

origen.”

Regla de imparcialidad del sistema

Bajo ninguna circunstancia debe implementarse un algoritmo, condición, ﬁltro,

ordenamiento o regla de negocio que beneﬁcie directa o indirectamente a un

candidato o partido político en particular.

El sistema debe operar de forma imparcial, transparente y consistente.

Esto implica que:

●  El ordenamiento de candidatos en la boleta no debe favorecer a un partido

especíﬁco.

●  Los cálculos de resultados deben basarse únicamente en votos registrados.
●  Los porcentajes deben calcularse de forma uniforme para todos los

candidatos y para la opción Ninguno.

●  Las validaciones deben aplicarse por igual a todos los partidos, candidatos,

ciudadanos y usuarios.

●  Ninguna regla debe depender del nombre, siglas, logo, color o identidad

política de un partido.

El sistema debe preservar la transparencia del proceso electoral y garantizar que
todos los participantes sean tratados bajo las mismas reglas funcionales.

Requerimientos técnicos

●  Deben crear un proyecto ASP.NET Core MVC utilizando .NET 9.

●  Deben utilizar ViewModels para manejar la información que se envía desde
las vistas hacia los controladores. Las validaciones de los formularios deben
realizarse desde los mismos ViewModels.

●  Deben utilizar DTOs para la transferencia de información desde los servicios

hacia las demás capas de la aplicación.

●  Se debe utilizar Entity Framework Core con el enfoque Code First para la

persistencia de los datos. El proyecto debe incluir sus migraciones
correspondientes.

●  El proyecto debe ser visualmente entendible para el usuario. Para esto,

deben utilizar Bootstrap u otro framework de CSS que permita construir una
interfaz clara, organizada y fácil de utilizar.

●  El proyecto debe implementar correctamente la arquitectura Onion. Esta
arquitectura debe aplicarse correctamente en toda la solución. En caso de
que la arquitectura esté mal aplicada o no se respeten las responsabilidades
de las capas, se considerará incorrecta.

●  Se deben utilizar repositorios genéricos aplicando el patrón Repository
Pattern para centralizar las operaciones comunes de acceso a datos.

●  Los controladores no deben contener lógica de negocio compleja. Su

responsabilidad debe ser recibir las solicitudes, validar los modelos, llamar a
los servicios correspondientes y retornar las vistas adecuadas.

●  Las vistas no deben acceder directamente a la base de datos ni trabajar
directamente con entidades de persistencia cuando corresponda utilizar
ViewModels.

●  Se debe usar la capa Shared para el servicio de correo. Este servicio será

utilizado para el envío del código de veriﬁcación del elector y para el envío
del resumen de votación al ﬁnalizar el proceso.

●  El sistema debe implementar autenticación y autorización por roles,

garantizando que los usuarios con rol Administrador solo accedan a las
funcionalidades administrativas y que los usuarios con rol Dirigente político
sólo accedan a las funcionalidades correspondientes a su partido político.

●  El sistema debe impedir el acceso directo por URL a funcionalidades
restringidas cuando el usuario no esté autenticado o no tenga el rol
requerido.


