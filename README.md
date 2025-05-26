# XgenMySQL
Plugin MySQL para Xgen Framework

![image](Resources/icono.png)


# Funciones públicas de la clase XgenMySQL

## ConvertType(string type)

Convierte un tipo de dato de SQL Server a su equivalente en C# utilizando un diccionario de mapeo.

>### Parámetros:
>   
>	- type: Tipo de dato en SQL.
>   

>### Retorna: 
>   - string - El tipo de dato convertido de SQL a C#.

### Uso: 
```CSharp
string TypeCSharp = ConvertType("VARCHAR");
```

## FillTypesDb()
Llena el diccionario de tipos de datos con los mapeos entre los tipos de SQL Server y sus equivalentes en C#.

> ### Parámetros:
>
>   - Ninguno.
>

>### Retorna: void.

### Uso: 
```CSharp
FillTypesDb();
```

## FooterSpMethod(string namesp)
Genera un mensaje de confirmación para indicar si un procedimiento almacenado fue creado exitosamente o falló.

> ### Parámetros:
>
>	- namesp: Nombre del procedimiento almacenado.
>

> ### Retorna: 
>   - string - El pie de un procedimiento almacenado.

### Uso:
```CSharp
var spName = "ClientSelProc";
string footer = FooterSpMethod(spName);
```
## GenerateDeleteProc(Table table, string version)
Genera el script SQL para un procedimiento almacenado que elimina registros de una tabla específica. Se utiliza para generar el script SQL de un procedimiento almacenado que elimina registros de una tabla.

> ### Parámetros:
>
>   - table: Objeto Table con información de la tabla.
>   - version: Versión del procedimiento.
>

> ### Retorna: 
>   - string - El script SQL para eliminar datos en una tabla.

### Uso: 

```CSharp
var delProc = Plugin.GenerateDeleteProc(tabla, "1.0");
File.WriteAllText(@"C:\ProtectSales\ClientsDelProc.sql", delProc);
```

## GenerateSelectProc(Table table, string version)
Genera el script SQL para un procedimiento almacenado que selecciona registros de una tabla específica.

> ### Parámetros:
>
>	- table: Objeto Table con información de la tabla.
>	- version: Versión del procedimiento.
>

> ### Retorna: 
>   - string - El script SQL para seleccionar datos en una tabla.

### Uso:
```CSharp
Table table = new Table 
{
      BaseDatos = "Sales"
    , Tabla = "Clients"
    , Fields = GetField() // Get list of fields
};
var SQLVersion = "MSSQL2012";
string sSelProc = GenerateSelectProc(table, SQLVersion);
```

## GenerateTableCreateScript(string tableName)
Genera el script SQL para crear una tabla específica en la base de datos.

> ### Parámetros:
>
>	- tableName: Nombre de la tabla.
>

> ### Retorna: 
>   - string - El script SQL para crear una tabla.

### Uso:
```CSharp
var sTableName = "Clients";
string script = GenerateTableCreateScript(sTableName);
```

## GenerateTables(List<string> tables, string namespaceDB, string classModifiers, string projectPath, string projectName, ClassType classType)
Genera scripts SQL para crear múltiples tablas y los guarda en la ruta especificada.

> ### Parámetros:
>
>	- tables: Lista de nombres de tablas.
>	- namespaceDB: Espacio de nombres para la base de datos.
>	- classModifiers: Modificadores de clase.
>	- projectPath: Ruta del proyecto.
>	- projectName: Nombre del proyecto.
>	- classType: Tipo de clase.
>

> ### Retorna: void.

### Uso:
```CSharp
var listTables = GetTables("MyDB");
var namespace = "Model";
var classModifiers = "";
var projectPath = @"C:\ProjectDB\";
var projectName = "SalesProject";
var classType = "public";
GenerateTables(listTables, namespaceDB, classModifiers, projectPath, projectName, classType);
```
## GenerateUpdateProc(Table table, string version)
Genera el script SQL para un procedimiento almacenado que actualiza registros de una tabla específica.

> ### Parámetros:
>
>	- table: Objeto Table con información de la tabla.
>	- version: Versión del procedimiento.
>

> ### Retorna: 
>   - string - El script SQL para actualizar datos en una tabla.

### Uso:
```CSharp
Table table = new Table 
{
      BaseDatos = "Sales"
    , Tabla = "Clients"
    , Fields = GetField() // Get list of fields
};
var SQLVersion = "MSSQL2012";
string sUpdProc = GenerateUpdateProc(table, version);
```

## GetAllParameters(List<FieldList> fields, int tabs = 0)
Genera una lista de parámetros de entrada para un procedimiento almacenado a partir de una lista de campos.

> ### Parámetros:
>
>	- fields: Lista de campos.
>	- tabs: Número de tabulaciones.
>

> ### Retorna:
>   - string - Los parámetros de entrada de un procedimiento almacenado.

### Uso:
```CSharp
Table table = new Table 
{
      BaseDatos = "Sales"
    , Tabla = "Clients"
    , Fields = GetField() // Get list of fields
};
var fields = table.Fields;
var tabs = 0;
string paramInputForSP = GetAllParameters(fields, tabs);
```

## GetConstraintKeys(string tableName)
Obtiene las claves primarias de una tabla específica en formato de cadena separada por comas.

> ### Parámetros:
>
>   - tableName: Nombre de la tabla.
>

> ### Retorna:
>   - string - Las claves primarias de una tabla.

### Uso:
```CSharp
var tableName = "Clients";
string keys = GetConstraintKeys(tableName);
```

## GetDataBases()
Obtiene una lista de nombres de bases de datos disponibles en el servidor.

> ### Parámetros: Ninguno.
>

> ### Retorna:
>   - List<string> - Los nombres de las bases de datos.

### Uso:
```CSharp
List<string> listDBs = GetDataBases();
```

## GetEntity(string tableName)
Obtiene la estructura de una tabla, incluyendo columnas, tipos de datos, claves primarias y otras propiedades.

> ### Parámetros:
>
>   - tableName: Nombre de la tabla.
>

> ### Retorna: 
>   - List<Entity2> - La estructura de una tabla.

### Uso:
```CSharp
tableName = "Clients";
List<Entity2> strTable = GetEntity(tableName);
```

## GetHeadSp(string spName, string version)
Genera un encabezado para un procedimiento almacenado, incluyendo información como nombre, fecha, autor y versión.

> ### Parámetros:
>
>	- spName : Nombre del procedimiento.
>	- version: Versión del procedimiento.
>

> ### Retorna:
>   - string - El encabezado de un procedimiento almacenado.

### Uso:
```CSharp
var spName = "ClientsSelProc";
var version = "MSSQL2008";
string sHeadSP = GetHeadSp(spName, version);
```

## GetInputParamSp(List<Entity2> entities)
Genera una lista de parámetros de entrada para un procedimiento almacenado a partir de una lista de entidades.

> ### Parámetros:
>
>   - entities: Lista de entidades.
>

> ### Retorna:
>   - string - Los parámetros de entrada de un procedimiento almacenado.

### Uso:
```CSharp
List<Entity2> tables = GetTables("SalesDB");
string paramInputSP = GetInputParamSp(tables);
```
## GetInputParamSpPK(List<Entity2> entities, string tableName)
Genera una lista de parámetros de entrada para las claves primarias de una tabla.

> ### Parámetros:
>
>   - entities: Lista de entidades.
>   - tableName: Nombre de la tabla.
>

> ### Retorna:
>   - string - Los parámetros de entrada para claves primarias.

### Uso:
```CSharp
List<Entity2> tables = GetTables("SalesDB");
var tableName = "Clients";
string paramInputSPPK = GetInputParamSpPK(tables, tableName);
```

## GetInputParamSpSet(List<Entity2> entities, string tableName)
Genera una lista de asignaciones de parámetros para un procedimiento almacenado de actualización.

> ### Parámetros:
>
>   - entities: Lista de entidades.
>   - table: Nombre de la tabla.
>

> ### Retorna:
>   - string - Los parámetros de entrada para un procedimiento almacenado.

### Uso:
```CSharp
List<Entity2> tables = GetTables("SalesDB");
string paramInputSpUpd = GetInputParamSpSet(tables, tableName);
```

## GetInputParamSpWhere(List<Entity2> entities, string tableName, bool oneLine, int align)
Genera una cláusula WHERE para un procedimiento almacenado basada en las claves primarias de una tabla.

> ### Parámetros:
>
>   - entities: Lista de entidades.
>   - tableName: Nombre de la tabla.
>   - oneLine: Indica si la salida debe estar en una línea. Por defecto usa false.
>   - align: Número de espacios para alinear. Por defecto usa 0 (cero).

> ### Retorna:
>   - string - Los parámetros de entrada para la cláusula WHERE.

### Uso:
```CSharp
List<Entity2> tables = GetTablas("Clients");
bool oneLine = false;
aling = 0;
string tableName = GetInputParamSpWhere(tables, tableName, oneLine, align);
```

## GetKeysTable(string tableName)
Obtiene las claves primarias de una tabla específica en formato de cadena separada por comas.

> ### Parámetros:
>
>   - tableName: Nombre de la tabla.
>

> ### Retorna:
>   - string - Las claves primarias de una tabla.

### Uso:
```CSharp
var tableName = "Clients";
string sPrimaryKeys = GetKeysTable(tableName);
```

## GetListFieldSP(List<Entity2> entities, string tableName)
Genera una lista de campos para un procedimiento almacenado basada en las claves primarias de una tabla.

> ### Parámetros:
>
>   - entities: Lista de entidades.
>	- tableName: Nombre de la tabla.
>
	
> ### Retorna:
>   - string - Los campos de una tabla.

### Uso:

```CSharp
var tableName = "Clients";
List<Entity2> fields = GetFields(tableName);

string paramFieldSPPK = GetListFieldSP(fields, tableName)
```

## GetListTables()
Obtiene una lista de nombres de tablas disponibles en la base de datos configurada.

> ### Parámetros: Ninguno.

> ### Retorna:
>   - List<string> - Los nombres de las tablas.

### Uso:
```CSharp
List<string> tables = GetListTables();
```
 
## GetParametersCallPKSp(string index_keys)
Genera una lista de parámetros para llamar a un procedimiento almacenado basado en las claves primarias.

> ### Parámetros:
>
>   - index_keys: Claves primarias.
>

> ### Retorna:
>   - string - Los parámetros para llamar a un procedimiento almacenado.

### Uso:
```CSharp
var index_keys = "IdClient";
string paramInputSP = GetParametersCallPKSp(index_keys);
```

## GetRaisError(string SQLVersion, int Severity, int State, string model, string spName)
Genera un mensaje de error personalizado para procedimientos almacenados en SQL Server.

> ### Parámetros:
>
>   - SQLVersion: Nombre del servidor SQL.
>   - Severity: Código de error.
>   - State: Estado del error.
>   - model: Modelo afectado.
>   - spName: Nombre del procedimiento almacenado.
>

> ### Retorna: 
>   - string - El manejo de errores en SQL Server.

### Uso:
```CSharp
var sRaiceError = GetRaisError("MySQL", 500, "Client", "ClientsSelProc");
```


## GetSpDataType(Entity2 entitie)
Obtiene el tipo de dato SQL Server de una entidad (campo), incluyendo longitud, precisión y escala si aplica.
> ### Parámetros:
>
>   - entitie: Objeto Entity2.
>

> ### Retorna:
>   - string - El tipo de dato en SQL Server.

### Uso:
```CSharp
Entity2 entity = new Entity2
{
    Field = "IdClient",
    Type = "INT",
    MaxLength = null,
    Precision = null,
    Scale = null,
    IsNullable = false,
    IsIdentity = true,
    PrimaryKey = true,
    ParamPrg = "@IdClient"
};
string dataType = GetSpDataType(entity);
```

## GetTableScript(string tableName)
Genera el script SQL para crear una tabla específica, incluyendo columnas y claves primarias.
> ### Parámetros:
>
>   - tableName: Nombre de la tabla.
>

> ### Retorna:
>   - string - El script SQL para crear una tabla.

### Uso:
```CSharp
string tableName = "Clients";
string script = GetTableScript(tableName);
```
 
## GetTableStructure(string tableName)
Obtiene la estructura de una tabla, incluyendo nombre de columna, tipo, nulabilidad y si es clave primaria.
> ### Parámetros:
>
>   - tableName: Nombre de la tabla.

> ### Retorna:
>   - List<(string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)> - La estructura de una tabla.

### Uso:
```CSharp
string tableName = "Clients";
List<(string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)> structure = GetTableStructure(tableName);
```

## HeaderSpMethod(string spName)
Genera un encabezado para un procedimiento almacenado, incluyendo instrucciones para eliminarlo si ya existe.
> ### Parámetros:
>
>   - spName: Nombre del procedimiento.
>

> ### Retorna:
>   - string - El encabezado de un procedimiento almacenado.

### Uso:
```CSharp
var spName = "ClientsSelProc";
string header = HeaderSpMethod(spName);
```

## Inicialize()
Inicializa la configuración del plugin, incluyendo la carga de colores y otros parámetros desde un archivo de configuración.
> ### Parámetros: Ninguno.
>

### Retorna: void.

### Uso:
```CSharp
Inicialize();
```


> ### Retorna: void.
	
## LoadTables(string tables)
Carga la estructura de una lista de tablas en la Propiedad Tablas, incluyendo columnas, tipos de datos y claves primarias.
> ### Parámetros:
>
>   - tables: Lista de nombres de tablas.
>

> ### Retorna: void.

### Uso:
```CSharp
LoadTables("Clients, Orders, Products");
```
	
## ProcDelProc(Table tabla)
Genera el script SQL para un procedimiento almacenado que elimina registros de una tabla específica.
> ### Parámetros:
>
>   - tabla: Objeto Table.
>

> ### Retorna: 
>   - string - El script SQL para eliminar datos en una tabla.

### Uso:
```CSharp
var tabla = "Clients";
string sProcDelProc = ProcDelProc(tabla);
```

## ProcSelProc(Table tabla)
Genera el script SQL para un procedimiento almacenado que selecciona registros de una tabla específica.
> ### Parámetros:
>
>   - tabla: Objeto Table.
>

> ### Retorna:
>   - string - El script SQL para seleccionar datos en una tabla.

### Uso:
```CSharp
var tabla = "Clients";
string sProcSelProc = ProcSelProc(tabla);
```

## ProcUpdProc(Table tabla)
Genera el script SQL para un procedimiento almacenado que actualiza registros de una tabla específica.
> ### Parámetros:
>
>   - tabla: Objeto Table.
>

> ### Retorna:
>   - string - El script SQL para actualizar datos en una tabla.

### Uso:
```CSharp
var tabla = "Clients";
string sProcUpdProc = ProcUpdProc(tabla);
```

## SetDefaultValue(string type)
Devuelve el valor por defecto para un tipo de dato específico.
> ### Parámetros:
>
>   - type: Tipo de dato.
>

> ### Retorna:
>   string - El valor por defecto para un tipo de dato.

### Uso:
```CSharp
string defaultValue = SetDefaultValue("VARCHAR");
```

## SetParamCallProcDeletePKSp(string index_keys)
Genera una lista de parámetros para llamar a un procedimiento almacenado de eliminación basado en las claves primarias.
> ### Parámetros:
>
>   - index_keys: Claves primarias.
>
Retorna: string - Los parámetros para llamar a un procedimiento almacenado.

### Uso:

```CSharp
var index_keys = "IdClient";
string paramCallDelete = SetParamCallProcDeletePKSp(index_keys);
```

## SetParamProcIn(List<FieldList> fields)
Genera una lista de parámetros de entrada para un procedimiento almacenado basado en una lista de campos.
> ### Parámetros:
>
>   - fields: Lista de campos.
>
> ### Retorna:
>   - string - Los parámetros de entrada para un procedimiento almacenado.

### Uso:
```CSharp
List<FieldList> fields = GetFields("Clients");
string paramInput = SetParamProcIn(fields);
```
 
# Funciones privadas en la clase XgenSQL2008

## HandlerError(string errorMessage, string sqlState, int errorCode, string spName)
Genera un bloque de manejo de errores para procedimientos almacenados en SQL Server.
> ### Parámetros:
>
>   - errorMessage: Mensaje de error.
>   - sqlState: Código de estado SQL.
>   - errorCode: Código de error.
>   - spName: Nombre del procedimiento almacenado.
>

> ### Retorna:
>   - string - Código SQL para manejar errores en procedimientos almacenados.
	
### Uso:
```CSharp
string errorMessage = "Error al ejecutar el procedimiento almacenado.";
string sqlState = "42000";
int errorCode = 500;
string spName = "ClientsSelProc";
string errorHandler = HandlerError(errorMessage, sqlState, errorCode, spName);
```


## GetColumnsDefinition(string tableName)
Obtiene las definiciones de las columnas de una tabla, incluyendo tipo de dato, longitud y nulabilidad.
> ### Parámetros:
>
>   - tableName: Nombre de la tabla.
>
> ### Retorna:
>   - List<string> - Lista de definiciones de columnas de una tabla.

### Uso:
```CSharp
List<string> columns = GetColumnsDefinition("Clients");
```

## GetPrimaryKeyDefinition(string tableName)
Obtiene la definición de la clave primaria de una tabla específica.
> ### Parámetros:
>
>   - tableName: Nombre de la tabla.
>

> ### Retorna:
>   - string - Definición de la clave primaria de una tabla.

### Uso:
```CSharp
var tableName = "Clients";
string primaryKey = GetPrimaryKeyDefinition(tableName);
```
    
# Propiedades
### Database (IDatabase)
Instancia para gestionar conexiones y operaciones con la base de datos.
````
Tipo: IDatabase
Acceso: Público
````

### DataTypes (Dictionary<string, string>)
Mapeo de tipos de datos SQL a tipos de lenguaje (ej: "VARCHAR" => "string").
````
Tipo: Dictionary<string, string>
Acceso: Público
````
### SQLType (string)
Indica el motor de base de datos (ej: SQLServer, MySQL, Sybase, etc).
````
Tipo: string
Acceso: Público
````
### cmdSQLComments
````
Contiene la cadena de caracteres asociada a los comentarios simples de SQL.
Tipo: string
````
### cmdSQLCommentsColor
````
Define el color para resaltar los comentarios simples en SQL.
Tipo: string
````
### cmdSQLCommentsBloq
````
Contiene la cadena para identificar comentarios en bloque dentro del código SQL.
Tipo: string
````
### cmdSQLCommentsBloqColor
Define el color utilizado para resaltar los comentarios en bloque del SQL.
````
Tipo: string
````
### cmdSQLString
Contiene la cadena utilizada para identificar textos o literales dentro del código SQL.
````
Tipo: string
````
### cmdSQLStringColor
Define el color aplicado a las cadenas de texto SQL.
````
Tipo: string
````
### cmdSQLKey1
Palabra clave SQL personalizada n.º 1.`
````
Tipo: string
````
### cmdSQLKey1Color
Color asignado a la palabra clave SQL n.º 1.
````
Tipo: string
````
### cmdSQLKey2
Palabra clave SQL personalizada n.º 2.
````
Tipo: string
````
### cmdSQLKey2Color
Color asignado a la palabra clave SQL n.º 2.
````
Tipo: string
````
### cmdSQLKey3
Palabra clave SQL personalizada n.º 3.
````
Tipo: string
````
### cmdSQLKey3Color
Color asignado a la palabra clave SQL n.º 3.
````
Tipo: string
````
### cmdSQLKey4
Palabra clave SQL personalizada n.º 4.
````
Tipo: string
````
### cmdSQLKey4Color
Color asignado a la palabra clave SQL n.º 4.
````
Tipo: string
````
### cmdSQLKey5
Palabra clave SQL personalizada n.º 5.
````
Tipo: string
````
### cmdSQLKey5Color
Color asignado a la palabra clave SQL n.º 5.
````
Tipo: string
````
### cmdSQLKey6
Palabra clave SQL personalizada n.º 6.
````
Tipo: string
````
### cmdSQLKey6Color
Color asignado a la palabra clave SQL n.º 6.
````
Tipo: string
````
### cmdSQLKey7
Palabra clave SQL personalizada n.º 7.
````
Tipo: string
````
### cmdSQLKey7Color
Color asignado a la palabra clave SQL n.º 7.
````
Tipo: string
````
### cmdSQLKey8
Palabra clave SQL personalizada n.º 8.
````
Tipo: string
````
### cmdSQLKey8Color
Color asignado a la palabra clave SQL n.º 8.
````
Tipo: string
````
### cmdSQLKey9
Palabra clave SQL personalizada n.º 9.
````
Tipo: string
````
### cmdSQLKey9Color
Color asignado a la palabra clave SQL n.º 9.
````
Tipo: string
````
### cmdSQLKey10
Palabra clave SQL personalizada n.º 10.
````
Tipo: string
````
### cmdSQLKey10Color
Color asignado a la palabra clave SQL n.º 10.
````
Tipo: string
````
### Tabs
Cadena que representa una tabulación de 4 espacios.`
````
Tipo: string
````
### Icon
Imagen asociada al plugin del generador SQL.
````
Tipo: Image
````
### Tables
Lista de tablas procesadas por el generador SQL.
````
Tipo: List<Table>
````
### SQLType
Indica el tipo de base de datos SQL manejada por el Plugin.
````
Valor fijo: "MySQL"
Tipo: string
````
### DatabaseNameDefault
Nombre de la base de datos predeterminada.
````
Valor fijo: "master"
Tipo: string
````
### Colors
Accede a la configuración de colores del Plugin.
````
Tipo: Colors
````

### Clases Base 
### Entity 
Clase que representa la estructura simplificada de una entidad.
```CSharp
public class Entity
{
    public object Field { get; set; }
    public object Type { get; set; }
    public object Primary { get; set; }
    public object ParamPrg { get; set; }
}
```

### Entity2 
Clase que representa la estructura de una entidad.
```CSharp
public class Entity2
{
    public object Field { get; set; }
    public object Type { get; set; }
    public object MaxLength { get; set; }
    public object Precision { get; set; }
    public object Scale { get; set; }
    public object IsNullable { get; set; }
    public object IsIdentity { get; set; }
    public object PrimaryKey { get; set; }
    public object ParamPrg { get; set; }
    public int LenField { get; set; }
}
```

### FieldList 
Clase que representa la lista de campos de una tabla.
```C#
public class FieldList
{
    public string nombreCampo;
    public string tipoCampo;
    public bool esNulo;
    public bool esIdentidad;
    public bool esClavePrimaria;
    public bool esClaveForanea;
    public string tablaRelacion;
    public string campoRelacion;
    public string defaultValue;
}
```
 
### Table 
Clase que representa la estructura de una tabla.
```CSharp
public class Table
{
    public string BaseDatos;
    public string nombreTabla;
    public List<FieldList> Campos;
}
```

## Xauro Dev
José Patricio Donoso Moscoso (Xcode)
