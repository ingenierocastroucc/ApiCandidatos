# API Web para Gestión de Cuestionarios

Este proyecto es una API Web en .NET 8 diseñada para gestionar preguntas de cuestionarios. Incluye endpoints para recuperar, agregar y eliminar preguntas de cuestionarios. La API está construida con ASP.NET Core y utiliza Entity Framework Core para las interacciones con la base de datos en SQL Server.

## Características

**Operaciones CRUD:** Recuperar, agregar y eliminar preguntas de cuestionarios.
**Validación:** Asegura que los datos de entrada cumplan con los requisitos establecidos.
**Soporte para CORS:** Configurado para permitir solicitudes de orígenes cruzados.
**Integración con Swagger:** Proporciona documentación interactiva de la API en modo desarrollo

## Configuración Inicial

## 1. Clonar el Repositorio

```
git clone https://github.com/ingenierocastroucc/ApiCandidatos.git

```
## 2. Instalar Dependencias

Ejecuta dotnet restore para instalar las dependencias requeridas.

```
dotnet restore

```

## 3. Actualizar la Cadena de Conexión

Actualiza la cadena de conexión DefaultConnection en el archivo appsettings.json con tu cadena de conexión a SQL Server:

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tu_servidor;Database=tu_base_de_datos;User Id=tu_usuario;Password=tu_contraseña;"
  }
}

```

# Endpoints

## GET /api/servicesquestion/{theme}

**Descripción:** Recupera una lista de preguntas de cuestionarios basadas en el tema especificado.

**Parámetros:**

**theme (string):** El tema para filtrar las preguntas.

**Respuestas:**

**200 OK:** Devuelve una lista de preguntas que coinciden con el tema.
**400 Bad Request:** Si el parámetro de tema está ausente o es inválido.
**500 Internal Server Error:** Si ocurre un error inesperado.

## Ejemplo de Solicitud:

```
GET https://localhost:(puerto)/api/servicesquestion/Art

```

## Ejemplo de Respuesta:

```
[
    {
        "id": "d97c9709-1b2e-4084-aee3-17094f61bf74",
        "question": "Which of the following is the name of a Leonardo da Vinci's masterpiece?",
        "answerIndex": 2,
        "score": 3,
        "theme": "Art",
        "choicesJson": "[\"Sunflowers\",\"Mona Lisa\",\"The Kiss\"]",
        "choices": [
            "Sunflowers",
            "Mona Lisa",
            "The Kiss"
        ]
    }
]

```

## POST /api/quiz

**Descripción:** Agrega una nueva pregunta de cuestionario.

## Parámetros:

**QuizItemModel (cuerpo):** Contiene la pregunta y la respuesta que se va a agregar.

**Esquema del Cuerpo:**

```
{
  "Id": "d97c9709-1b2e-4084-aee3-17094f61bf58",
  "Question": "What is the capital of Colombia?",
  "AnswerIndex" : 5,
  "Score" : 3,
  "Theme" : "Global",
  "Choices": [
    "Paris",
    "London",
    "Berlin",
    "Bogota"
  ]
}
```

## Respuestas:

**201 Created:** Si la pregunta se agrega correctamente. La respuesta incluye la ubicación del nuevo recurso.
**400 Bad Request:** Si el payload de la solicitud es inválido.
**500 Internal Server Error:** Si ocurre un error inesperado.

## Ejemplo de Solicitud:

```
POST https://localhost:(puerto)/api/quiz
Content-Type: application/json

{
  "Id": "d97c9709-1b2e-4084-aee3-17094f61bf58",
  "Question": "What is the capital of Colombia?",
  "AnswerIndex" : 5,
  "Score" : 3,
  "Theme" : "Global",
  "Choices": [
    "Paris",
    "London",
    "Berlin",
    "Bogota"
  ]
}

```
## Ejemplo de Respuesta:

```
{
    "id": "d97c9709-1b2e-4084-aee3-17094f61bf58",
    "question": "What is the capital of Colombia?",
    "answerIndex": 5,
    "score": 3,
    "theme": "Global",
    "choicesJson": "[\"Paris\",\"London\",\"Berlin\",\"Bogota\"]",
    "choices": [
        "Paris",
        "London",
        "Berlin",
        "Bogota"
    ]
}
```

## DELETE /api/quiz/{id}

**Descripción:** Elimina una pregunta de cuestionario por su ID.

**Parámetros:**

**id (Guid):** El identificador único de la pregunta a eliminar.

## Respuestas:

**200 OK:** Si la pregunta se elimina correctamente.
**400 Bad Request:** Si el ID es inválido.
**404 Not Found:** Si la pregunta con el ID especificado no existe.
**500 Internal Server Error:** Si ocurre un error inesperado.

## Ejemplo de Solicitud:

```
DELETE https://localhost:(puerto)/api/quiz/D97C9709-1B2E-4084-AEE3-17094F61BF58
```

## Configuración de CORS
CORS (Cross-Origin Resource Sharing) está configurado para permitir solicitudes desde cualquier origen durante el desarrollo.

## Configuración
En el archivo Program.cs, CORS está configurado de la siguiente manera:

```
    services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", builder =>
            {
                builder.WithOrigins("https://localhost:7241/") // Reemplaza con los orígenes permitidos
                        .WithMethods("GET", "POST", "DELETE") // Métodos HTTP permitidos
                        .WithHeaders("Content-Type", "Authorization"); // Encabezados permitidos
            });
        });
```

## Aplicación de la Política de CORS

La política de CORS se aplica en el pipeline de solicitudes:

```
app.UseCors("MyPolicy");
```

## Validación

La API utiliza validación personalizada para los datos de entrada en los endpoint. La clase QuizItemModel incluye anotaciones de datos para imponer restricciones en los datos de la solicitud.

Este README proporciona una visión general completa de tu proyecto, cubriendo la configuración, los endpoints, la configuración de CORS y la validación del modelo. Está diseñado para ayudar a otros desarrolladores a entender y usar tu API de manera efectiva.