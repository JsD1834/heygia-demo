# 🚀 ¡Hola grupo HeyGIA!

## Demo

La idea de este ejercicio, fue simular una prueba tecnica en el tiempo que se tenia predestinado para la prueba virtual(2 horas hasta donde me dejo ver la plataforma Evalart). Como requerimiento quise replicar de manera basica pero funcional su modelo de negocios de acuerdo a lo que encontre en la web y a la breve descripcion que se me dio.

## Pruebas en REMOTO

### Tuneles de Microsoft
El siguiente link apunta a puerto expuesto mediante un túnel seguro desde un servidor de Microsoft donde estara corriendo la solución y se podra probar de forma remota: https://wp7w00tp-7224.use2.devtunnels.ms/swagger/index.html

> [!WARNING]
> - Al entrar al tunel se solicita confiar en el autor, si no se desea(lo cual seria muy normal) probar con los siguientes metodos.

### Codespaces
Si por precaución o protocolo empresarial no desean abrir el link como medida de seguridad, pueden cambiarse a la rama _"sqlite"_ y crear un codespaces para ejecutar la solución con los siguientes pasos:

- Crear el Codespaces con el boton verde "Code"-> pestaña "Codespaces" -> botón verde "Crear Codespaces en sqlite".
- Esto abrira una nueva pestaña en donde cargara la instancia de un entorno de desarrollo aislado que trabaja bajo un contenedor y maneja VS Code. Al terminar la configuración del mismo, se podra escribir los siguientes comandos en la consola:

```bash
cd HeyGiaDemo
dotnet run
```
> [!IMPORTANT]
> - Debido a que el entorno no esta completamente configurado al desplegarse y abrir la solucion en el navegador este no redireccionara correctamente a la url, se debe agregarle _/swagger/_ 
<br>  Cuando se abre el navegador: https://mi-codespaces.app.github.dev/
<br>  URL completa: https://mi-codespaces.app.github.dev/swagger/ -> Asi ya por defecto buscara el index.
> - El contenedor corre sobre linux por lo cual es sensible a las mayusculas.
> - Al cambiar a SQLite algunas funcionalidades especificas pudieron verse afectadas.

> [!NOTE]
> Tuve que manejarlo con SQLite ya que configurar el codespaces con SQL Server se me complico y no lo logre, se puede lograr pero es tiempo con el que no cuento.

 ## Pruebas en LOCAL
Clonar el repositorio de cualquiera de las dos ramas
- **main**: Tener en cuenta que este codigo lo desarrolle con una conexion con SQL Server, por lo cual se tendrá que tener instalado el correspondiente motor y la base de datos creada. Ademas de tener que aplicar las migraciones en la DB que se cree:

> En la terminal del proyecto
 ```bash
dotnet ef database update
```

> O por la consola de paquetes Nuget
 ```bash
Update-Database
```

---
- **sqlite**: Con este codigo funcionaria igual que como codespaces.


## Observaciones
Debido a que el desarrollo conto con un tiempo limite, añado los siguientes puntos a mejorar que se podrian implementar

- Continuar puliendo la arquitectura ( clean architecture)
- Cambiar por una inyección de configuración y servicios (IoC) de forma externa para tener el contenedor mas limpio y legible.
- Terminar la conexion con un LLM local o externo(via API o se podria ver alguna integracion con MCP) -> no me dio tiempo.
- Mi enfoque con el ORM fue code first, pero dependiendo el caso puede ser al contrario o de forma mixta.

## 😊¡Gracias por su atención! 🎉

Agradezco mucho su tiempo y disposición para revisar esta solución.  
Si surge cualquier duda o comentario, estaré atento para dar respuesta.  


