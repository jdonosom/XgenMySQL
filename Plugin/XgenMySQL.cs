#nullable disable

using System.Data.Common;
using System.Data;
using System.Reflection;
using System.Text;
using Xgen.Plugin;
using System.Drawing;
using XgenMySQL.Connect;
using XauroCommon.Interface;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Zlib;
using System.Transactions;
using System.Configuration;
using System.Security.Policy;

namespace SauroGenerador.Plugin;

public class XgenMySQL : ISqlGeneratorPlugin
{

    #region Variables  para colores del Plugins
    #endregion

    private Colors _colors;
    private IDatabase _database;
    public IDatabase Database
    {
        get => _database;
        set => _database = value;
    }

    public string Tabs => new string(' ', 4);
    public Image Icon { get; set; }
    public Dictionary<string, string> DataTypes { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> DataTypesDb { get; set; } = new Dictionary<string, string>();
    public Dictionary<string, string> DataTypesParam { get; set; } = new Dictionary<string, string>();
    public List<Table> Tables { get; set; } = new List<Table>(); // Changed from private set to public set to implement the interface
    public string SQLType => "MySQL Server";
    public string DatabaseNameDefault => "mysql";
    public Colors Colors => _colors;
    public XgenMySQL() { this.Inicialize(); }

    public XgenMySQL(
            string tables
        )
    {

        this.Inicialize();
        LoadTables(tables);

    }

    public void Inicialize()
    {

        #region Leer configuracion de colores para el Plugin
        // Mapeo personalizado
        //
        string rutaConfig = AppDomain.CurrentDomain.BaseDirectory + @"\Components\XgenMySQL.dll.config";

        ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = rutaConfig
        };

        _colors = new Colors();
        // Cargar la configuración desde ese archivo
        //
        Configuration appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

        _colors.cmdSQLComments = "";
        if (appConfig.AppSettings.Settings["cmdSQLComments"] != null)
            _colors.cmdSQLComments = appConfig.AppSettings.Settings["cmdSQLComments"].Value ?? "";

        _colors.cmdSQLCommentsColor = "";
        if (appConfig.AppSettings.Settings["cmdSQLCommentsColor"] != null)
            _colors.cmdSQLCommentsColor = appConfig.AppSettings.Settings["cmdSQLCommentsColor"].Value ?? "";

        _colors.cmdSQLCommentsBloq = "";
        if (appConfig.AppSettings.Settings["cmdSQLCommentsBloq"] != null)
            _colors.cmdSQLCommentsBloq = appConfig.AppSettings.Settings["cmdSQLCommentsBloq"].Value ?? "";

        _colors.cmdSQLCommentsBloqColor = "";
        if (appConfig.AppSettings.Settings["cmdSQLCommentsBloqColor"] != null)
            _colors.cmdSQLCommentsBloqColor = appConfig.AppSettings.Settings["cmdSQLCommentsBloqColor"].Value ?? "";

        _colors.cmdSQLString = "";
        if (appConfig.AppSettings.Settings["cmdSQLString"] != null)
            _colors.cmdSQLString = appConfig.AppSettings.Settings["cmdSQLString"].Value ?? "";

        _colors.cmdSQLStringColor = "";
        if (appConfig.AppSettings.Settings["cmdSQLStringColor"] != null)
            _colors.cmdSQLStringColor = appConfig.AppSettings.Settings["cmdSQLStringColor"].Value ?? "";

        _colors.cmdSQLKey1 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey1"] != null)
            _colors.cmdSQLKey1 = appConfig.AppSettings.Settings["cmdSQLKey1"].Value ?? "";

        _colors.cmdSQLKey1Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey1Color"] != null)
            _colors.cmdSQLKey1Color = appConfig.AppSettings.Settings["cmdSQLKey1Color"].Value ?? "";

        _colors.cmdSQLKey2 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey2"] != null)
            _colors.cmdSQLKey2 = appConfig.AppSettings.Settings["cmdSQLKey2"].Value ?? "";

        _colors.cmdSQLKey2Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey2Color"] != null)
            _colors.cmdSQLKey2Color = appConfig.AppSettings.Settings["cmdSQLKey2Color"].Value ?? "";

        _colors.cmdSQLKey3 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey3"] != null)
            _colors.cmdSQLKey3 = appConfig.AppSettings.Settings["cmdSQLKey3"].Value ?? "";

        _colors.cmdSQLKey3Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey3Color"] != null)
            _colors.cmdSQLKey3Color = appConfig.AppSettings.Settings["cmdSQLKey3Color"].Value ?? "";

        _colors.cmdSQLKey4 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey4"] != null)
            _colors.cmdSQLKey4 = appConfig.AppSettings.Settings["cmdSQLKey4"].Value ?? "";

        _colors.cmdSQLKey4Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey4Color"] != null)
            _colors.cmdSQLKey4Color = appConfig.AppSettings.Settings["cmdSQLKey4Color"].Value ?? "";

        _colors.cmdSQLKey5 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey5"] != null)
            _colors.cmdSQLKey5 = appConfig.AppSettings.Settings["cmdSQLKey5"].Value ?? "";

        _colors.cmdSQLKey5Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey5Color"] != null)
            _colors.cmdSQLKey5Color = appConfig.AppSettings.Settings["cmdSQLKey5Color"].Value ?? "";

        _colors.cmdSQLKey6 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey6"] != null)
            _colors.cmdSQLKey6 = appConfig.AppSettings.Settings["cmdSQLKey6"].Value ?? "";

        _colors.cmdSQLKey6Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey6Color"] != null)
            _colors.cmdSQLKey6Color = appConfig.AppSettings.Settings["cmdSQLKey6Color"].Value ?? "";

        _colors.cmdSQLKey7 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey7"] != null)
            _colors.cmdSQLKey7 = appConfig.AppSettings.Settings["cmdSQLKey7"].Value ?? "";

        _colors.cmdSQLKey7Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey7Color"] != null)
            _colors.cmdSQLKey7Color = appConfig.AppSettings.Settings["cmdSQLKey7Color"].Value ?? "";

        _colors.cmdSQLKey8 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey8"] != null)
            _colors.cmdSQLKey8 = appConfig.AppSettings.Settings["cmdSQLKey8"].Value ?? "";

        _colors.cmdSQLKey8Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey8Color"] != null)
            _colors.cmdSQLKey8Color = appConfig.AppSettings.Settings["cmdSQLKey8Color"].Value ?? "";

        _colors.cmdSQLKey9 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey9"] != null)
            _colors.cmdSQLKey9 = appConfig.AppSettings.Settings["cmdSQLKey9"].Value ?? "";

        _colors.cmdSQLKey9Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey9Color"] != null)
            _colors.cmdSQLKey9Color = appConfig.AppSettings.Settings["cmdSQLKey9Color"].Value ?? "";

        _colors.cmdSQLKey10 = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey10"] != null)
            _colors.cmdSQLKey10 = appConfig.AppSettings.Settings["cmdSQLKey10"].Value ?? "";

        _colors.cmdSQLKey10Color = "";
        if (appConfig.AppSettings.Settings["cmdSQLKey10Color"] != null)
            _colors.cmdSQLKey10Color = appConfig.AppSettings.Settings["cmdSQLKey10Color"].Value ?? "";
        #endregion

    }

    public List<string> GetListTables()
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<string> tables = new List<string>();
        try
        {
            if (!Database.Conectar())
                return null;

            string sSql = Database.ListTablesCommand(Database.DatabaseName);
            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            if (dr == null)
                return null;

            while (dr.Read())
            {
                string name = dr.IsDBNull(dr.GetOrdinal("TABLE_NAME")) ? "" : dr.GetString(dr.GetOrdinal("TABLE_NAME"));
                tables.Add(name);
            }
            Database.Desconectar();
        }
        catch (Exception)
        {

            return tables;
        }
        return tables;
    }


    /// <summary>
    /// Setea los parametros indices para pasar a las funciones
    /// </summary>
    /// <param name="fields">Lista de campos</param>
    /// <returns>Retorna una cadena con los parametros de entrada</returns>
    public string SetParamProcIn(
            List<FieldList> fields
        )
    {
        // <Type0> <nameField0>, <Type1> <nameField1>, ...
        string asignacionParametrosSP = string.Empty;
        fields.ForEach(x =>
        {
            if (x.esClavePrimaria)
                asignacionParametrosSP += $"{x.nombreCampo.ToLower()}, ";
        });
        // Param0, Param1, ...
        return asignacionParametrosSP.Substring(0, asignacionParametrosSP.Length - 2);
    }


    /// <summary>
    /// Carga las tablas y sus campos en la lista de tablas.
    /// </summary>
    /// <param name="tables">Lista de tablas separados por comas.</param>
    /// <exception cref="Exception">Genera excepción de tipo Exception</exception>
    public void LoadTables(
            string tables
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<Entity2> entity = new List<Entity2>();
        string index_keys = string.Empty;

        var _tables = new List<Table>();
        string[] tablasArray = tables.Split(',');

        foreach (string tabla in tablasArray)
        {
            Table t = new Table();
            t.nombreTabla = tabla;
            t.Campos = new List<FieldList>();

            try
            {
                index_keys = GetConstraintKeys(tabla);
                entity = GetEntity(tabla);
                entity.ForEach(x =>
                {
                    t.Campos.Add(new FieldList()
                    {
                        nombreCampo = x.Field.ToString(),
                        tipoCampo = ConvertType(x.Type.ToString()),
                        esNulo = ((string)x.IsNullable) == "yes",
                        esIdentidad = (bool)x.IsIdentity,
                        esClavePrimaria = ((bool)x.PrimaryKey),
                        esClaveForanea = false,
                        tablaRelacion = "",
                        campoRelacion = "",
                        defaultValue = SetDefaultValue(x.Type.ToString())
                    });
                });
                _tables.Add(t);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        Tables = _tables;

    }


    public string ConvertType(
            string type
        )
    {
        FillTypesDb();

        string _tipo = type.ToUpper();
        string tipoConvertido = string.Empty;

        if (DataTypes.ContainsKey(_tipo))
        {
            tipoConvertido = DataTypes[_tipo];
        }
        return tipoConvertido;
    }


    public string FooterSpMethod(
            string nsp
        )
    {
        StringBuilder sBuffer = new StringBuilder();

        sBuffer.AppendLine("SELECT ");
        sBuffer.AppendLine("    IF(");
        sBuffer.AppendLine("        EXISTS(");
        sBuffer.AppendLine("            SELECT 1 ");
        sBuffer.AppendLine("            FROM information_schema.ROUTINES ");
        sBuffer.AppendLine("            WHERE ROUTINE_SCHEMA = DATABASE() ");
        sBuffer.AppendLine("            AND ROUTINE_NAME = 'LogsSelProc'");
        sBuffer.AppendLine("            ),");
        sBuffer.AppendLine("        '<<< PROCEDIMIENTO LogsSelProc CREADO >>>',");
        sBuffer.AppendLine("        '<<< HA FALLADO LA CREACIÓN DEL PROCEDIMIENTO LogsSelProc >>>'");
        sBuffer.AppendLine("    ) AS Mensaje;");
        sBuffer.AppendLine("");
        sBuffer.AppendLine("DELIMITER ;");

        return sBuffer.ToString();
    }


    public void GenerarProcedimientos(
            List<Table> tables,
            string nsp,
            string pk,
            string pathOut,
            string projectName,
            string SQLType,
            string dataBaseName
        )
    {

        string pathDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string pathProyecto = GnrUtil.AddSlash($"{pathOut}");

        tables.ForEach(x =>
        {
            x.BaseDatos = dataBaseName;
            var selProc = GenerateSelectProc(x, SQLType);
            var updProc = GenerateUpdateProc(x, SQLType);
            var delProc = GenerateDeleteProc(x, SQLType);

            GnrUtil.SaveFile(pathProyecto, $"{x.nombreTabla}SelProc.sql", selProc);
            GnrUtil.SaveFile(pathProyecto, $"{x.nombreTabla}UpdProc.sql", updProc);
            GnrUtil.SaveFile(pathProyecto, $"{x.nombreTabla}DelProc.sql", delProc);

        });

    }

    public string GenerateDeleteProc(
            Table table,
            string version
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamWhereSp = null;

        // Obtener los campos de la tabla
        List<Entity2> camposDB = GetEntity(table.nombreTabla);

        // Convertir el nombre de la tabla a UpperCamelCase
        var model = GnrUtil.ToUpperCamelCase(table.nombreTabla);

        // Generar los parámetros de entrada y la cláusula WHERE
        sParamSpPK = GetInputParamSpPK(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);

        // Nombre del procedimiento almacenado
        SP = $"{model}_DelProc";

        // Construir el procedimiento almacenado
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"DELIMITER $$");
        sb.AppendLine($"CREATE PROCEDURE {SP} (");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine($")");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}START TRANSACTION;");
        sb.AppendLine($"{Tabs}DELETE FROM {table.nombreTabla}");

        // Agregar la cláusula WHERE si es necesaria
        if (!string.IsNullOrEmpty(sParamWhereSp))
        {
            sb.AppendLine($"{Tabs}WHERE {sParamWhereSp};");
        }

        sb.AppendLine($"{Tabs}IF ROW_COUNT() = 0 THEN");
        sb.AppendLine($"{Tabs}{Tabs}ROLLBACK;");
        sb.AppendLine($"{Tabs}{Tabs}SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No rows were deleted.';");
        sb.AppendLine($"{Tabs}ELSE");
        sb.AppendLine($"{Tabs}{Tabs}COMMIT;");
        sb.AppendLine($"{Tabs}END IF;");
        sb.AppendLine("END $$");
        sb.AppendLine($"DELIMITER ;");

        return sb.ToString();
    }


    public string GenerateSelectProc(
            Table table,
            string version
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamWhereSp = null;

        List<Entity2> camposDB = GetEntity(table.nombreTabla);

        var model = GnrUtil.ToUpperCamelCase(table.nombreTabla);

        sParamSpPK = GetInputParamSpPK(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);

        SP = $"{model}_SelProc";
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"DELIMITER $$");
        sb.AppendLine($"CREATE PROCEDURE {SP} (");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine($")");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}SELECT");

        // Generar lista de campos para el SELECT
        for (int i = 0; i < camposDB.Count; i++)
        {
            var campo = camposDB[i];
            sb.Append($"{Tabs}{campo.Field}");
            if (i < camposDB.Count - 1)
                sb.Append(",");
            sb.AppendLine();
        }

        sb.AppendLine($"{Tabs}FROM {table.nombreTabla}");

        // Agregar cláusula WHERE si es necesario
        if (!string.IsNullOrEmpty(sParamWhereSp))
        {
            sb.AppendLine($"{Tabs}WHERE {sParamWhereSp}");
        }

        sb.AppendLine("END $$");
        sb.AppendLine($"DELIMITER ;");

        return sb.ToString();
    }


    public string GenerateTableCreateScript(
            string tableName
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        var connection = Database.Conectar();

        //using var connection = new MySqlConnection(connectionString);
        //connection.Open();

        var createScript = new StringBuilder();
        createScript.AppendLine($"CREATE TABLE `{tableName}` (");

        var columns = GetColumnsDefinition(tableName);
        var primaryKey = GetPrimaryKeyDefinition(tableName);

        if (!string.IsNullOrWhiteSpace(primaryKey))
        {
            columns.Add(primaryKey);
        }

        createScript.AppendLine(string.Join(",\n", columns));
        createScript.AppendLine(");");

        return createScript.ToString();
    }


    /// <summary>
    /// Genera el script SQL para crear tablas a partir de una lista de nombres de tablas y los almacena en la ruta projectPath
    /// </summary>
    /// <param name="tables"></param>
    /// <param name="namespaceDB"></param>
    /// <param name="classModifiers"></param>
    /// <param name="projectPath"></param>
    /// <param name="projectName"></param>
    /// <param name="classType"></param>
    public void GenerateTables(
            List<string> tables,
            string namespaceDB,
            string classModifiers,
            string projectPath,
            string projectName,
            ClassType classType
        )
    {
        string pathDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string pathProyecto = GnrUtil.AddSlash($"{projectPath}");
        if (!Directory.Exists(pathProyecto))
        {
            Directory.CreateDirectory(pathProyecto);
        }

        foreach (var table in tables)
        {
            // Obtener la estructura de la tabla (esto debe ser implementado según tu base de datos)
            var columnas = GetTableStructure(table);

            // Generar el código SQL CREATE TABLE
            string createTableSQL = GenerateTableCreateScript(table);

            /***
            * 
            */
            if (!Directory.Exists(pathProyecto))
            {
                Directory.CreateDirectory(pathProyecto);
            }
            var pathOut = GnrUtil.AddSlash(pathProyecto);
            //File.WriteAllText($"{pathOut}{GnrUtil.GetSingular(table)}.cs", createTableSQL);
            File.WriteAllText($"{pathOut}{table}.cs", createTableSQL);

        }

    }

    public string GenerateUpdateProc(
            Table table,
            string version
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamSpSet = null;
        string sParamWhereSp = null;

        // Obtener los campos de la tabla
        List<Entity2> camposDB = GetEntity(table.nombreTabla);

        // Convertir el nombre de la tabla a UpperCamelCase
        var model = GnrUtil.ToUpperCamelCase(table.nombreTabla);

        // Generar los parámetros de entrada y las cláusulas SET y WHERE
        sParamSpPK = GetInputParamSp(camposDB);
        sParamSpSet = GetInputParamSpSet(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);

        // Nombre del procedimiento almacenado
        SP = $"{model}_UpdProc";

        // Construir el procedimiento almacenado
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"DELIMITER $$");
        sb.AppendLine($"CREATE PROCEDURE {SP} (");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine($")");
        sb.AppendLine("BEGIN");
        sb.AppendLine($"{Tabs}START TRANSACTION;");

        // Actualizar el registro
        sb.AppendLine($"{Tabs}UPDATE {table.nombreTabla}");
        sb.AppendLine($"{Tabs}SET {sParamSpSet}");
        if (!string.IsNullOrEmpty(sParamWhereSp))
        {
            sb.AppendLine($"{Tabs}WHERE {sParamWhereSp};");
        }

        // Manejo de errores y confirmación de la transacción
        sb.AppendLine($"{Tabs}IF ROW_COUNT() = 0 THEN");
        sb.AppendLine($"{Tabs}{Tabs}ROLLBACK;");
        sb.AppendLine($"{Tabs}{Tabs}SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'No rows were updated.';");
        sb.AppendLine($"{Tabs}ELSE");
        sb.AppendLine($"{Tabs}{Tabs}COMMIT;");
        sb.AppendLine($"{Tabs}END IF;");
        sb.AppendLine("END $$");
        sb.AppendLine($"DELIMITER ;");

        return sb.ToString();
    }


    public string GetHeadSp(
            string sp,
            string version
        )
    {
        StringBuilder sb = new StringBuilder();

        // Longitud fija para justificar las líneas
        int lineLength = 80;

        // Función auxiliar para justificar una línea
        string JustifyLine(string content)
        {
            int padding = lineLength - content.Length - 4; // 4 para "/***" y "***/"
            return $"/*** {content}{new string(' ', padding)}***/";
        }
        //                 1         2         3         4         5         6         7         8         9         1
        //        12345678901234567890123456789012345678901234567890123456789012345678901234567890
        ///****** xauro Gen: Procedimiento Almacenado dbo.BoletasDelProc ******/
        ///****** Fecha: {DateTime.Now.ToString("dd/MM/yyyy").PadRight(nLongitud - 17)} ******/
        // Encabezado del procedimiento almacenado
        sb.AppendLine(JustifyLine($"MySQL Stored Procedure: {sp}"));
        sb.AppendLine(JustifyLine($"Fecha: {DateTime.Now:dd/MM/yyyy}"));
        sb.AppendLine(JustifyLine($"Autor: {Environment.UserName}"));
        sb.AppendLine(JustifyLine($"Versión: {version}"));

        return sb.ToString();
    }


    /// <summary>
    /// Obtiene todos los parámetros de un procedimiento almacenado.
    /// </summary>
    /// <param name="fields">Lista de campos</param>
    /// <param name="tabs">Largo del tab</param>
    /// <returns>Retorna una lista de campos de tipo. int Param0, string Param1, ...</returns>
    public string GetAllParameters(
            List<FieldList> fields,
            int tabs = 0
        )
    {

        string Param = string.Empty;
        int extraTabs = 0;
        // Buscar el tipo de campo en la lista de campos
        fields.ForEach(x =>
        {

            // BaseProceso.DataTypesParam.TryGetValue(x.tipoCampo, out string type);
            Param += $"{new string(' ', tabs + extraTabs)}{x.tipoCampo} {GnrUtil.ToUpperCamelCase(x.nombreCampo)}, \r\n";
            if (extraTabs == 0 && fields.Count > 4)
                extraTabs = 20; // x.tipoCampo.Length;
        });
        // int Param0, string Param1, ...
        return Param.Substring(0, Param.Length - 4);

    }


    /// <summary>
    /// Obtener los parametros para la llamada a la funcion de eliminacion
    /// </summary>
    /// <param name="index_keys">La PK de la tabla</param>
    /// <returns>Retorna una cadena con parametros de llammadas desde DataLayer</returns>
    /// <exception cref="NotImplementedException"></exception>
    public string SetParamCallProcDeletePKSp(
            string index_keys
        )
    {
        // <Type0> <nameField0>, <Type1> <nameField1>, ...

        string[] keys = index_keys.Split(',');
        string asignacionParametrosSP = string.Empty;
        string type = string.Empty;
        foreach (string key in keys)
        {
            DataTypesDb.TryGetValue(key, out type);
            asignacionParametrosSP += $"@{GnrUtil.ToUpperCamelCase(key)}, ";
        }
        return asignacionParametrosSP.Substring(0, asignacionParametrosSP.Length - 2);

    }


    /// <summary>
    /// Obtiene la lista de parametros para llamadas desde DataLayer
    /// </summary>
    /// <param name="index_keys">Cadena con lista de PK separadas por comas</param>
    /// <returns>Retorna cadena con parametros PK para llamada desde DataLayer</returns>
    public string GetParametersCallPKSp(
            string index_keys
        )
    {
        // @<Type0> <nameField0>, @<Type1> <nameField1>, ...

        string[] keys = index_keys.Split(',');
        string asignacionParametrosSP = string.Empty;
        string type = string.Empty;
        string typeDb = string.Empty;

        foreach (string key in keys)
        {
            asignacionParametrosSP += $"@{GnrUtil.ToUpperCamelCase(key.Trim())}, ";
        }
        // @Param0, @Param1, ...
        return asignacionParametrosSP.Substring(0, asignacionParametrosSP.Length - 2);
    }


    public string GetInputParamSp(
            List<Entity2> entities
        )
    {
        List<Entity2> ents = new List<Entity2>();
        int maxLen = 0;
        //
        foreach (Entity2 o in entities)
        {
            maxLen = ((int)o.LenField > maxLen ? (int)o.LenField : maxLen);
            ents.Add(o);
        }
        //
        string sFieldFirst = ents[0].Field.ToString();
        string sFieldLast = ents[ents.Count - 1].Field.ToString();

        string sParametrosSP = null;
        foreach (Entity2 o in ents)
        {
            // string sTab = TabInsert(o.LenField, NumMax);
            sParametrosSP += $"{new string(' ', 4)}IN p" + o.Field.ToString().PadRight(maxLen + 1) +
                             GetSpDataType(o).ToUpper() +
                             (sFieldLast == o.Field.ToString() ? "" : ", " + Environment.NewLine);
        }
        return sParametrosSP;
    }


    public string GetInputParamSpPK(
            List<Entity2> entities,
            string tableName
        )
    {
        StringBuilder sb = new StringBuilder();
        var primaryKeys = entities.Where(e => (bool)e.PrimaryKey).ToList();

        for (int i = 0; i < primaryKeys.Count; i++)
        {
            var key = primaryKeys[i];
            sb.Append($"{Tabs}IN p{key.Field} {GetSpDataType(key)}");
            if (i < primaryKeys.Count - 1)
                sb.Append(", ");
        }
        return sb.ToString();
    }


    public string GetInputParamSpSet(
            List<Entity2> entities,
            string table
        )
    {
        List<Entity2> ents = new List<Entity2>();
        double maxLen = 0;
        //
        foreach (Entity2 o in entities)
        {
            maxLen = ((int)o.LenField > maxLen ? (int)o.LenField : maxLen);
            ents.Add(o);
        }
        string sParametrosSP = null;
        string sTabOrNull = string.Empty;

        string sFirstField = ents[0].Field.ToString();
        string sLastField = ents[ents.Count - 1].Field.ToString();

        foreach (Entity2 o in entities)
        {
            if (!(bool)o.IsIdentity)
            {
                // sParametrosSP += sTabOrNull + o.Field.ToString().PadRight((int)maxLen) + " = @" + o.Field + Environment.NewLine;

                sParametrosSP += $"{sTabOrNull}{o.Field.ToString().PadRight((int)maxLen)}= p{o.Field}{(o.Field.ToString() == sLastField ? "" : "\r\n")}";
                sTabOrNull = $"{string.Concat(Enumerable.Repeat(Tabs, 3))},";
            }
        }
        return sParametrosSP;
    }


    public string GetInputParamSpWhere(
        List<Entity2> entities,
        string tableName,
        bool oneLine = false,
        int align = 0
    )
    {
        StringBuilder sb = new StringBuilder();
        var primaryKeys = entities.Where(e => (bool)e.PrimaryKey).ToList();

        for (int i = 0; i < primaryKeys.Count; i++)
        {
            var key = primaryKeys[i];
            sb.Append($"{key.Field} = p{key.Field}");
            if (i < primaryKeys.Count - 1)
                sb.Append(oneLine ? " AND " : $"{Environment.NewLine}{new string(' ', align)}AND ");
        }

        return sb.ToString();
    }


    public string GetKeysTable(
            string tableName
        )
    {
        Database = this.Database;
        string indexKeys = string.Empty;

        if (!Database.Conectar())
        {
            return null;
        }

        try
        {
            // Consulta para obtener las claves primarias de la tabla
            string sql = $@"
                      SELECT COLUMN_NAME
                        FROM information_schema.KEY_COLUMN_USAGE
                        WHERE TABLE_SCHEMA = DATABASE()
                          AND TABLE_NAME = '{tableName}'
                          AND CONSTRAINT_NAME = 'PRIMARY'";

            Database.CrearComando(sql);
            DbDataReader dr = Database.EjecutarConsulta();

            List<string> keys = new List<string>();

            while (dr.Read())
            {
                keys.Add(dr.GetString(0)); // Obtener el nombre de la columna
            }

            dr.Close();
            Database.Desconectar();

            // Unir las claves primarias en una cadena separada por comas
            indexKeys = string.Join(",", keys);
            return indexKeys;
        }
        catch (Exception ex)
        {
            Database.Desconectar();
            throw new Exception($"Error al obtener las claves primarias de la tabla '{tableName}': {ex.Message}");
        }
    }


    public string GetListFieldSP(
            List<Entity2> entities,
            string tableName
        )
    {
        string sParList = null;
        List<Entity2> ents = new List<Entity2>();

        try
        {
            var index_keys = GetKeysTable(tableName);
            //
            foreach (Entity2 o in entities)
            {

                bool isKey = index_keys.Contains((string)o.Field);

                if (isKey)
                {
                    ents.Add(o);
                }

            }

            if (ents.Count > 0)
            {
                string sFieldFirst = ents[0].Field.ToString();
                string sFieldLast = ents[ents.Count - 1].Field.ToString();
                foreach (Entity2 o in ents)
                {
                    sParList += $"{(o.Field.ToString() == sFieldFirst ? "" : Tabs)}{o.Field}{GetSpDataType(o)}{(o.Field.ToString() != sFieldLast ? "," : "")}\r\n";
                }

            }
        }
        catch (Exception)
        {

            return null;

        }
        return sParList;
    }


    private string HandlerError(
            string errorMessage
          , string sqlState
          , int errorCode
          , string SP
        )
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"{Tabs}-- Declaraci�n de variables para el manejo de errores");
        sb.AppendLine($"{Tabs}DECLARE v_error_message VARCHAR(1000);");
        sb.AppendLine($"{Tabs}DECLARE v_error_occurred BOOLEAN DEFAULT FALSE;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}-- Manejador para SQLEXCEPTION (errores de SQL)");
        sb.AppendLine($"{Tabs}DECLARE EXIT HANDLER FOR SQLEXCEPTION");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}-- Guarda el mensaje de error MySQL");
        sb.AppendLine($"{Tabs}{Tabs}GET DIAGNOSTICS CONDITION 1 v_error_message = MESSAGE_TEXT;");
        sb.AppendLine($"{Tabs}{Tabs}");
        sb.AppendLine($"{Tabs}{Tabs}-- Deshace la transacción de error MySQL");
        sb.AppendLine($"{Tabs}{Tabs}IF (SELECT @transaction_READ_ONLY = 0) THEN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK;");
        sb.AppendLine($"{Tabs}{Tabs}END IF;");
        sb.AppendLine($"{Tabs}{Tabs}");
        sb.AppendLine($"{Tabs}{Tabs}-- Lanza un error personalizado con detalles");
        sb.AppendLine($"{Tabs}{Tabs}SET v_error_message=CONCAT('Error en {SP}: ', v_error_message);");
        sb.AppendLine($"{Tabs}{Tabs}SIGNAL SQLSTATE '{sqlState}' SET MESSAGE_TEXT = v_error_message;");
        sb.AppendLine($"{Tabs}END;");
        sb.AppendLine($"{Tabs}");
        return sb.ToString();
    }


    public string GetRaisError(
              string SQLVersion
            , int Severity
            , int State
            , string model
            , string spName
        )
    {

        StringBuilder sb = new StringBuilder();

        // Construir el mensaje de error
        //
        string errorMessage = $"{spName}: No se pudo actualizar la tabla {model} ({SQLVersion}).";

        // Generar la instrucción SIGNAL para MySQL
        //
        sb.AppendLine($"{Tabs}SIGNAL SQLSTATE '{State}'");
        sb.AppendLine($"{Tabs}SET MESSAGE_TEXT = '{errorMessage}',");
        sb.AppendLine($"{Tabs}ERROR_CODE = {Severity};");
        //
        //
        return sb.ToString();

    }


    public string GetSpDataType(
            Entity2 entity
        )
    {
        switch (entity.Type.ToString().ToLower())
        {
            case "varchar":
            case "char":
                return $"{entity.Type}({entity.MaxLength})";
            case "decimal":
            case "numeric":
                return $"{entity.Type}({entity.Precision},{entity.Scale})";
            default:
                return entity.Type.ToString();
        }
    }


    /// <summary>
    /// Obtiene el script SQL para crear una tabla específica.
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Retorna un script con comando create de la tabla</returns>
    public string GetTableScript(
            string tableName
        )
    {
        if (!Database.Conectar())
        {
            return null;
        }

        try
        {
            // Consulta para obtener las columnas de la tabla
            string sqlColumns = $@"
                SELECT COLUMN_NAME, 
                       DATA_TYPE, 
                       CHARACTER_MAXIMUM_LENGTH, 
                       IS_NULLABLE, 
                       COLUMN_KEY, 
                       EXTRA
                FROM information_schema.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = '{tableName}'
                ORDER BY ORDINAL_POSITION;";

            Database.CrearComando(sqlColumns);
            DbDataReader dr = Database.EjecutarConsulta();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"CREATE TABLE `{tableName}` (");

            List<string> primaryKeys = new List<string>();
            while (dr.Read())
            {
                string columnName = dr["COLUMN_NAME"].ToString();
                string dataType = dr["DATA_TYPE"].ToString();
                string maxLength = dr["CHARACTER_MAXIMUM_LENGTH"] != DBNull.Value
                    ? $"({dr["CHARACTER_MAXIMUM_LENGTH"]})"
                    : string.Empty;
                string isNullable = dr["IS_NULLABLE"].ToString() == "YES" ? "NULL" : "NOT NULL";
                string extra = dr["EXTRA"].ToString();

                sb.AppendLine($"    `{columnName}` {dataType}{maxLength} {isNullable} {extra},");

                // Si la columna es clave primaria, agregarla a la lista
                if (dr["COLUMN_KEY"].ToString() == "PRI")
                {
                    primaryKeys.Add($"`{columnName}`");
                }
            }

            dr.Close();

            // Agregar la clave primaria si existe
            if (primaryKeys.Count > 0)
            {
                sb.AppendLine($"    PRIMARY KEY ({string.Join(", ", primaryKeys)})");
            }

            // Eliminar la última coma si no hay clave primaria
            if (sb[sb.Length - 3] == ',')
            {
                sb.Remove(sb.Length - 3, 1);
            }

            sb.AppendLine(") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");

            Database.Desconectar();
            return sb.ToString();
        }
        catch (Exception ex)
        {
            Database.Desconectar();
            throw new Exception($"Error al generar el script de la tabla '{tableName}': {ex.Message}");
        }
    }


    /// <summary>
    /// Obtiene la estructura de una tabla SQL, incluyendo nombre de columna, tipo, longitud máxima, precisión, escala, si es nulo y si es clave primaria.
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Retorna un List con los datos (string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)</returns>
    public List<(string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)> GetTableStructure(
        string tableName
        )
    {
        // Simulación de datos obtenidos de la base de datos
        // En un caso real, esto debería obtenerse de la base de datos o de un esquema definido
        List<Entity2> campos = GetSqlTableInfo(tableName);

        var columnas = new List<(string Nombre, string Tipo, bool EsNulo, bool EsClavePrimaria)>();

        foreach (var campo in campos)
        {
            string tipo = campo.Type.ToString();
            if (campo.MaxLength != null && int.TryParse(campo.MaxLength.ToString(), out int maxLength) && maxLength > 0)
            {
                tipo += $"({maxLength})";
            }

            columnas.Add((
                Nombre: campo.Field.ToString(),
                Tipo: tipo,
                EsNulo: campo.IsNullable != null && (bool)campo.IsNullable.Equals("yes"),
                EsClavePrimaria: campo.PrimaryKey != null && (bool)campo.PrimaryKey
            ));
        }

        return columnas;
    }


    /// <summary>
    /// Genera el encabezado para un procedimiento almacenado en MySQL.
    /// </summary>
    /// <param name="spName">Nombre del SP</param>
    /// <returns>Retorna encabezado para el procedimiento</returns>
    public string HeaderSpMethod(
            string spName
        )
    {
        StringBuilder sb = new StringBuilder();

        // Generar el encabezado para MySQL
        sb.AppendLine($"DELIMITER $$\n\r");
        sb.AppendLine($"DROP PROCEDURE IF EXISTS `{spName}` $$\n\r");
        return sb.ToString();
    }


    /// <summary>
    /// Obtiene información de la tabla SQL, incluyendo nombre de columna, tipo, longitud máxima, precisión, escala, si es nulo, si es identidad y si es clave primaria.
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Retorn una lista de Entity2</returns>
    /// <exception cref="Exception"></exception>
    public List<Entity2> GetSqlTableInfo(
            string tableName
        )
    {
        List<Entity2> entityList = new List<Entity2>();

        try
        {
            var connection = Database.Conectar();

            // Consulta para obtener información de las columnas de la tabla en MySQL
            string sSql = $@"SELECT 
                                COLUMN_NAME AS Field,
                                DATA_TYPE AS Type,
                                CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                                NUMERIC_PRECISION AS `Precision`,
                                NUMERIC_SCALE AS Scale,
                                IS_NULLABLE AS IsNullable,
                                EXTRA AS IsIdentity,
                                COLUMN_KEY AS ColumnKey
                            FROM INFORMATION_SCHEMA.COLUMNS
                            WHERE TABLE_SCHEMA = '{Database.DatabaseName}'
                              AND TABLE_NAME = '{tableName}'
                            ORDER BY ORDINAL_POSITION;";

            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            while (dr.Read())
            {
                Entity2 entity = new Entity2
                {
                    Field = dr["Field"].ToString(),
                    Type = dr["Type"].ToString(),
                    MaxLength = dr["MaxLength"] != DBNull.Value ? dr["MaxLength"] : null,
                    Precision = dr["Precision"] != DBNull.Value ? dr["Precision"] : null,
                    Scale = dr["Scale"] != DBNull.Value ? dr["Scale"] : null,
                    IsNullable = dr["IsNullable"].ToString().ToLower() == "yes",
                    IsIdentity = dr["IsIdentity"].ToString().ToLower().Contains("auto_increment"),
                    PrimaryKey = dr["ColumnKey"].ToString().ToLower() == "pri"
                };

                entityList.Add(entity);
            }

            dr.Close();
            return entityList;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener información de la tabla '{tableName}': {ex.Message}");
        }
        finally
        {
            Database.Desconectar();
        }
    }


    /// <summary>
    /// Obtiene la lista de bases de datos
    /// </summary>
    /// <returns>Retorna una lista de tipo string con los nombres de las bases de datos</returns>
    public List<string> GetDataBases()
    {

        string sSql = "SHOW DATABASES;";
        var oObject = new List<string>();

        if (!Database.Conectar())
        {
            return null;
        }

        try
        {
            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            DataTable dt = new DataTable();

            // Convertimos el DataRead a DataTable
            //
            dt.TableName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            dt.Load(dr);

            DataTableReader reader = new DataTableReader(dt);
            //
            if (reader == null)
                return null;

            while (reader.Read())
            {
                var Databasename = reader.IsDBNull(reader.GetOrdinal("Database")) ? "" : reader.GetString(reader.GetOrdinal("name"));
                oObject.Add(Databasename);
            }
            return oObject;
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            Database.Desconectar();
        }
    }


    /// <summary>
    /// Obtiene la lista de tablas de la base datos donde esta conectado
    /// </summary>
    /// <returns>Retorna una lista de tipo string con nombre de tablas</returns>
    public List<string> GetListTables(
            string DatabaseName
        )
    {
        string sSql = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{DatabaseName}';";
        List<string> sLista = new List<string>();

        try
        {
            if (!Database.Conectar())
                return null;

            Database.CrearComando(sSql);
            DbDataReader dr = Database.EjecutarConsulta();

            DataTable dt = new DataTable();

            // Convertimos el DataRead a DataTable
            //
            dt.TableName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            dt.Load(dr);

            DataTableReader reader = new DataTableReader(dt);
            //
            if (reader == null)
                return null;

            while (reader.Read())
            {
                string name = reader.IsDBNull(reader.GetOrdinal("TABLE_NAME")) ? "" : reader.GetString(reader.GetOrdinal("TABLE_NAME"));
                sLista.Add(name);
            }
            Database.Desconectar();
        }
        catch (Exception)
        {
            return sLista;
        }
        return sLista;
    }

    /// <summary>
    /// Obtiene la estructura de una tabla específica en la base de datos.
    /// </summary>
    /// <param name="tableName">Nombre de la tabla</param>
    /// <returns>Retorna un List con la estructura de la tabla</returns>
    public List<Entity2> GetEntity(
            string tabla
        )
    {

        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<Entity2> entityList = new List<Entity2>();

        // Consulta para obtener información de las columnas de la tabla
        string sSql = @$"SELECT COLUMN_NAME AS Field,
                               DATA_TYPE AS Type,
                               CHARACTER_MAXIMUM_LENGTH AS MaxLength,
                               NUMERIC_PRECISION AS `Precision`,
                               NUMERIC_SCALE AS Scale,
                               IS_NULLABLE AS IsNullable,
                               EXTRA AS Extra,
                               CASE WHEN COLUMN_KEY = 'PRI' THEN 1 ELSE 0 END AS ColumnKey
                           FROM INFORMATION_SCHEMA.COLUMNS
                          WHERE TABLE_SCHEMA = '{this.Database.DatabaseName}'
                            AND TABLE_NAME = '{tabla}'
                          ORDER BY ORDINAL_POSITION;";

        try
        {

            // Conectar a la base de datos
            var connection = Database.Conectar();
            if (!connection)
            {
                throw new Exception("No se pudo conectar a la base de datos");
            }

            // Crear el comando y asignar el parámetro
            Database.CrearComando(sSql);
            //Database.AsignarParametroCadena("@table", tabla);

            // Ejecutar la consulta
            DbDataReader dr = Database.EjecutarConsulta();

            while (dr.Read())
            {
                string fieldName = dr.IsDBNull(dr.GetOrdinal("Field")) ? "" : dr.GetString(dr.GetOrdinal("Field"));
                string dataType = dr.IsDBNull(dr.GetOrdinal("Type")) ? "" : dr.GetString(dr.GetOrdinal("Type"));
                object maxLength = dr.IsDBNull(dr.GetOrdinal("MaxLength")) ? null : dr.GetValue(dr.GetOrdinal("MaxLength"));
                object precision = dr.IsDBNull(dr.GetOrdinal("Precision")) ? null : dr.GetValue(dr.GetOrdinal("Precision"));
                object scale = dr.IsDBNull(dr.GetOrdinal("Scale")) ? null : dr.GetValue(dr.GetOrdinal("Scale"));
                bool isNullable = dr.IsDBNull(dr.GetOrdinal("IsNullable")) ? false : dr.GetString(dr.GetOrdinal("IsNullable")).Equals("YES", StringComparison.OrdinalIgnoreCase);
                bool isIdentity = dr.IsDBNull(dr.GetOrdinal("Extra")) ? false : dr.GetString(dr.GetOrdinal("Extra")).Contains("auto_increment");
                bool isPrimaryKey = dr.IsDBNull(dr.GetOrdinal("ColumnKey")) ? false : (dr.GetInt32(dr.GetOrdinal("ColumnKey")) == 1);

                // Crear la entidad
                Entity2 entity = new Entity2()
                {
                    Field = fieldName,
                    Type = dataType,
                    MaxLength = maxLength,
                    Precision = precision,
                    Scale = scale,
                    IsNullable = isNullable,
                    IsIdentity = isIdentity,
                    PrimaryKey = isPrimaryKey,
                    ParamPrg = "\"@" + GnrUtil.ToUpperCamelCase(fieldName) + "\", " + GnrUtil.ToUpperCamelCase(fieldName),
                    LenField = fieldName.Length
                };

                entityList.Add(entity);
            }

            dr.Close();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las columnas de la tabla '{tabla}': {ex.Message}");
        }
        finally
        {
            // Desconectar de la base de datos
            Database.Desconectar();
        }
        return entityList;
    }

    /// <summary>
    /// Obtener los campos PK de la tabla
    /// </summary>
    /// <param name="table">Nombre de la tabla</param>
    /// <returns>Cadena separada por comas</returns>
    /// <exception cref="Exception"></exception>
    public string GetConstraintKeys(
            string table
        )
    {
        string indexKeys = string.Empty;

        try
        {
            // Conectar a la base de datos
            //
            var conection = Database.Conectar();
            if (!conection)
            {
                throw new Exception("No se pudo conectar a la base de datos");
            }


            // Consulta para obtener las claves primarias de la tabla
            string query = @$"SELECT GROUP_CONCAT(COLUMN_NAME) AS constraint_keys
                               FROM information_schema.KEY_COLUMN_USAGE
                              WHERE TABLE_SCHEMA = '{this.Database.DatabaseName}'
                                AND TABLE_NAME = '{table}'
                                AND CONSTRAINT_NAME = 'PRIMARY'";

            // Crear el comando y asignar el parámetro
            Database.CrearComando(query);
            // Database.AsignarParametroCadena("@table", table);

            // Ejecutar la consulta
            DbDataReader dr = Database.EjecutarConsulta();
            if (dr.Read())
            {
                indexKeys = dr.IsDBNull(dr.GetOrdinal("constraint_keys"))
                    ? string.Empty
                    : dr.GetString(dr.GetOrdinal("constraint_keys"));
            }

            dr.Close();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al obtener las claves primarias de la tabla '{table}': {ex.Message}");
        }
        finally
        {
            // Desconectar de la base de datos
            Database.Desconectar();
        }

        // Retornar las claves primarias separadas por comas, sin espacios
        return indexKeys.Replace(" ", "");
    }

    /// Setea el valor por defecto de un tipo de dato en MySQL
    /// </summary>
    /// <param name="type">Tipo de datos</param>
    /// <returns>Retorna cadena de caracteres con el valor por defecto para un tipo de datos</returns>
    public string SetDefaultValue(
            string type
        )
    {
        switch (type.ToLower())
        {
            case "int":
            case "integer":
            case "smallint":
            case "mediumint":
            case "bigint":
            case "tinyint":
                return "0";

            case "float":
            case "double":
            case "real":
                return "0.0";

            case "decimal":
            case "numeric":
                return "0.0m";

            case "char":
            case "varchar":
            case "text":
            case "tinytext":
            case "mediumtext":
            case "longtext":
                return "\"\"";

            case "binary":
            case "varbinary":
            case "blob":
            case "tinyblob":
            case "mediumblob":
            case "longblob":
                return "new byte[0]";

            case "date":
            case "datetime":
            case "timestamp":
                return "DateTime.Parse(\"0001-01-01\")";

            case "time":
                return "TimeSpan.Zero.ToString()";

            case "year":
                return "DateTime.Now.Year.ToString()";

            case "boolean":
            case "bool":
                return "false";

            case "json":
                return "\"{}\"";

            case "enum":
            case "set":
                return "\"\"";

            default:
                return "null";
        }
    }

    /// <summary>
    /// Llena la base de datos de tipos de datos
    /// </summary>
    public void FillTypesDb()
    {
        DataTypes.Clear();

        // Tipos numéricos
        DataTypes.Add("TINYINT", "byte");
        DataTypes.Add("SMALLINT", "short");
        DataTypes.Add("MEDIUMINT", "int");
        DataTypes.Add("INT", "int");
        DataTypes.Add("INTEGER", "int");
        DataTypes.Add("BIGINT", "long");
        DataTypes.Add("DECIMAL", "decimal");
        DataTypes.Add("NUMERIC", "decimal");
        DataTypes.Add("FLOAT", "float");
        DataTypes.Add("DOUBLE", "double");
        DataTypes.Add("REAL", "double");

        // Tipos de texto
        DataTypes.Add("CHAR", "string");
        DataTypes.Add("VARCHAR", "string");
        DataTypes.Add("TEXT", "string");
        DataTypes.Add("TINYTEXT", "string");
        DataTypes.Add("MEDIUMTEXT", "string");
        DataTypes.Add("LONGTEXT", "string");

        // Tipos binarios
        DataTypes.Add("BINARY", "byte[]");
        DataTypes.Add("VARBINARY", "byte[]");
        DataTypes.Add("BLOB", "byte[]");
        DataTypes.Add("TINYBLOB", "byte[]");
        DataTypes.Add("MEDIUMBLOB", "byte[]");
        DataTypes.Add("LONGBLOB", "byte[]");

        // Tipos de fecha y hora
        DataTypes.Add("DATE", "DateTime");
        DataTypes.Add("DATETIME", "DateTime");
        DataTypes.Add("TIMESTAMP", "DateTime");
        DataTypes.Add("TIME", "TimeSpan");
        DataTypes.Add("YEAR", "int");

        // Tipos booleanos
        DataTypes.Add("BOOLEAN", "bool");
        DataTypes.Add("BOOL", "bool");

        // Tipos JSON
        DataTypes.Add("JSON", "string");

        // Tipos especiales
        DataTypes.Add("ENUM", "string");
        DataTypes.Add("SET", "string");
    }

    private List<string> GetColumnsDefinition(
            string tableName
        )
    {
        var columns = new List<string>();
        var Sql = @$"
            SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, CHARACTER_MAXIMUM_LENGTH,
                   NUMERIC_PRECISION, NUMERIC_SCALE, EXTRA 
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = '{tableName}' AND TABLE_SCHEMA = '{Database.DatabaseName}'
            ORDER BY ORDINAL_POSITION";

        Database.CrearComando(Sql);
        var dr = Database.EjecutarConsulta();
        while (dr.Read())
        {
            string colName = dr.GetString("COLUMN_NAME");
            string dataType = dr.GetString("DATA_TYPE");
            string isNullable = dr.GetString("IS_NULLABLE");
            object charLength = dr["CHARACTER_MAXIMUM_LENGTH"];
            object numPrecision = dr["NUMERIC_PRECISION"];
            object numScale = dr["NUMERIC_SCALE"];
            string extra = dr.GetString("EXTRA");

            var colDef = new StringBuilder();
            colDef.Append($"  `{colName}` {dataType}");

            if (dataType is "varchar" or "char" or "binary" or "varbinary")
            {
                if (charLength != DBNull.Value)
                    colDef.Append($"({charLength})");
            }
            else if (dataType is "decimal" or "float" or "double")
            {
                if (numPrecision != DBNull.Value)
                {
                    colDef.Append($"({numPrecision}");
                    if (numScale != DBNull.Value && Convert.ToInt32(numScale) > 0)
                        colDef.Append($",{numScale}");
                    colDef.Append(")");
                }
            }

            colDef.Append(isNullable == "NO" ? " NOT NULL" : " NULL");

            if (extra.Contains("auto_increment", StringComparison.OrdinalIgnoreCase))
                colDef.Append(" AUTO_INCREMENT");

            columns.Add(colDef.ToString());
        }
        Database.Desconectar();
        return columns;
    }

    private string GetPrimaryKeyDefinition(
            string tableName
        )
    {
        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }
        var connection = Database.Conectar();


        var pkCols = new List<string>();
        var Sql = @$"
            SELECT COLUMN_NAME
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
            WHERE TABLE_NAME = '{tableName}' AND CONSTRAINT_NAME = 'PRIMARY' AND TABLE_SCHEMA = '{Database.DatabaseName}'
            ORDER BY ORDINAL_POSITION";

        Database.CrearComando(Sql);
        var dr = Database.EjecutarConsulta();
        while (dr.Read())
        {
            pkCols.Add($"`{dr.GetString("COLUMN_NAME")}`");
        }
        Database.Desconectar();

        if (pkCols.Count > 0)
            return $"  PRIMARY KEY ({string.Join(", ", pkCols)})";
        else
            return null;
    }

    public string ProcSelProc(
            Table tabla
        )
    {

        if (Database == null)
        {
            throw new InvalidOperationException("La instancia de Database no está configurada.");
        }

        List<Entity2> camposDB = GetEntity(tabla.nombreTabla);

        var model = GnrUtil.ToUpperCamelCase(tabla.nombreTabla);
        string sParamSpPK = GetInputParamSpPK(camposDB, model);
        string sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);
        string SP = SP = $"{model}SelProc";
        string sParamSpSet = null;
        string spParamListField = null;

        StringBuilder sb = new StringBuilder();

        if (string.IsNullOrWhiteSpace(sParamSpPK))
        {
            return "";
        }

        sParamSpSet = GetInputParamSpSet(camposDB, model);
        spParamListField = GetListFieldSP(camposDB, model);

        sb.Append(GetHeadSp(SP, SQLType));
        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE PROCEDURE {SP}");
        sb.AppendLine("(");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine(")");
        sb.AppendLine("BEGIN");
        sb.Append($"{Tabs}SELECT ");

        // Leer datos SELECT
        // Estilo:
        //     SELECT Campo1
        //           ,Campo2
        //
        int nUltimoElemento = camposDB.Count - 1;
        string sFieldFirst = camposDB[0].Field.ToString();
        string sFieldLast = camposDB[nUltimoElemento].Field.ToString();
        string sTabOrNull = "";

        foreach (Entity2 o in camposDB)
        {
            sb.AppendLine(sTabOrNull + o.Field + ", ");
            sTabOrNull = new string(' ', 11);
        }
        sb = sb.Remove(sb.Length - 4, 2);

        sb.AppendLine($"{new string(' ', 6)}FROM {model}");
        if (sParamWhereSp != null)
        {
            sb.AppendLine($"{new string(' ', 5)}WHERE {sParamWhereSp};");
        }
        sb.AppendLine("");
        sb.AppendLine("END");
        sb.AppendLine("$$");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));

        return sb.ToString();

    }

    public string ProcDelProc(
            Table tabla
        )
    {
        string SP = null;
        string sParamSpPK = null;
        string sParamSpSet = null;
        string sParamWhereSp = null;
        string spParamListField = null;

        List<Entity2> camposDB = GetEntity(tabla.nombreTabla);
        var model = GnrUtil.ToUpperCamelCase(tabla.nombreTabla);

        sParamSpPK = GetInputParamSpPK(camposDB, model);
        if (string.IsNullOrWhiteSpace(sParamSpPK))
        {
            return "";
        }

        sParamSpSet = GetInputParamSpSet(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 10);
        spParamListField = GetListFieldSP(camposDB, model);
        SP = $"{model}DelProc";

        StringBuilder sb = new StringBuilder();
        sb.Append(GetHeadSp(SP, SQLType));
        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE DEFINER=`{Database.User}`@`%` PROCEDURE `{SP}`");
        sb.AppendLine("(");
        sb.AppendLine(sParamSpPK);
        sb.AppendLine(")");
        sb.AppendLine("BEGIN");
        sb.AppendLine(
                HandlerError($"{SP}: Error al ejecutar la operación de eliminación", "45000", 1640, SP)
            );
        sb.AppendLine($"{Tabs}START TRANSACTION;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}DELETE FROM {model}");

        // Leer datos SELECT
        // Estilo:
        //     DELETE FROM {model}
        //           
        //
        int nUltimoElemento = camposDB.Count - 1;
        string sFieldFirst = camposDB[0].Field.ToString();
        string sFieldLast = camposDB[nUltimoElemento].Field.ToString();
        if (sParamWhereSp != null)
        {
            sb.AppendLine($"{new string(' ', 5)}WHERE {sParamWhereSp};");
        }
        sb.AppendLine($"{Tabs}IF ROW_COUNT() = 0 THEN");
        sb.AppendLine($"{Tabs}{Tabs}ROLLBACK;");
        sb.AppendLine($"{Tabs}{Tabs}SELECT 'Observacion : No puedo eliminar el registro porque existe una referencia externa a la tabla {model}' AS error_message;");
        sb.AppendLine($"{Tabs}ELSE");
        sb.AppendLine($"{Tabs}{Tabs}COMMIT;");
        sb.AppendLine($"{Tabs}{Tabs}SELECT '' AS error_message;");
        sb.AppendLine($"{Tabs}END IF;");
        sb.AppendLine("END");
        sb.AppendLine("$$");
        sb.AppendLine("");

        sb.Append(FooterSpMethod(SP));
        return sb.ToString();
    }

    public string ProcUpdProc(
            Table tabla
        )
    {
        StringBuilder sb = new StringBuilder();

        string SP = null;
        string sParamSpPK = null;
        string sParamSpSet = null;
        string sParamWhereSp = null;
        string sParamWhereSp13 = null;
        string spParamListField = null;

        List<Entity2> camposDB = GetEntity(tabla.nombreTabla);
        List<Entity2> camposDBPK = GetEntity(tabla.nombreTabla)
            .Where(z => ((bool)z.PrimaryKey))
            .ToList();

        // Crea cadena con los campos PrimaryKey este en true con linq

        if (camposDBPK is null || camposDBPK.Count == 0)
        {
            return "";
        }

        string strPK = "";
        string strFirstField = camposDBPK[0].Field.ToString();
        string strLastField = camposDBPK[camposDBPK.Count - 1].Field.ToString();
        camposDBPK.ForEach(x =>
        {
            if ((bool)x.PrimaryKey)
            {
                strPK += $"{x.Field}{(x.Field.Equals(strLastField) ? "" : ";")}";
            }
        });
        var model = GnrUtil.ToUpperCamelCase(tabla.nombreTabla);
        var sParamSp = GetInputParamSp(camposDB);

        sParamSpPK = GetInputParamSpPK(camposDB, model);
        sParamSpSet = GetInputParamSpSet(camposDB, model);
        sParamWhereSp = GetInputParamSpWhere(camposDB, model, false, 21);
        sParamWhereSp13 = GetInputParamSpWhere(camposDB, model, false, 13);
        spParamListField = GetListFieldSP(camposDB, model);

        SP = $"{model}UpdProc";

        sb.Append(GetHeadSp(SP, SQLType));
        sb.Append(HeaderSpMethod(SP));
        sb.AppendLine("");
        sb.AppendLine($"CREATE DEFINER=`{Database.User}`@`%` PROCEDURE `{SP}`");
        sb.AppendLine("(");
        sb.AppendLine(sParamSp);
        sb.AppendLine(")");
        sb.AppendLine("BEGIN");

        // Error management
        //
        sb.AppendLine($"{Tabs}-- Declaración de variables para el manejo de errores");
        sb.AppendLine($"{Tabs}DECLARE v_error_message VARCHAR(1000);");
        sb.AppendLine($"{Tabs}DECLARE v_error_occurred BOOLEAN DEFAULT FALSE;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}-- Manejador para SQLEXCEPTION (errores de SQL)");
        sb.AppendLine($"{Tabs}DECLARE EXIT HANDLER FOR SQLEXCEPTION");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}-- Guarda el mensaje de error MySQL");
        sb.AppendLine($"{Tabs}{Tabs}GET DIAGNOSTICS CONDITION 1 v_error_message = MESSAGE_TEXT;");
        sb.AppendLine($"{Tabs}{Tabs}");
        sb.AppendLine($"{Tabs}{Tabs}-- Deshace la transacción en caso de error");
        sb.AppendLine($"{Tabs}{Tabs}IF (SELECT @@transaction_READ_ONLY = 0) THEN");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}ROLLBACK;");
        sb.AppendLine($"{Tabs}{Tabs}END IF;");
        sb.AppendLine($"{Tabs}{Tabs}");
        sb.AppendLine($"{Tabs}{Tabs}-- Lanza un error personalizado con detalles");
        sb.AppendLine($"{Tabs}{Tabs}SET v_error_message =  CONCAT('Error en {SP}: ', v_error_message);");
        sb.AppendLine($"{Tabs}{Tabs}SIGNAL SQLSTATE '45000'");
        sb.AppendLine($"{Tabs}{Tabs}SET MESSAGE_TEXT = v_error_message;");
        sb.AppendLine($"{Tabs}END;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}-- Manejador para advertencias (opcional)");
        sb.AppendLine($"{Tabs}DECLARE CONTINUE HANDLER FOR SQLWARNING");
        sb.AppendLine($"{Tabs}BEGIN");
        sb.AppendLine($"{Tabs}{Tabs}SET v_error_occurred = TRUE;");
        sb.AppendLine($"{Tabs}{Tabs}-- Podríamos registrar advertencias en una tabla de logs");
        sb.AppendLine($"{Tabs}END;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}-- Inicia la transacción");
        sb.AppendLine($"{Tabs}START TRANSACTION;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}IF EXISTS (SELECT 1 FROM {model} WHERE {sParamWhereSp}) THEN");

        sb.AppendLine($"{Tabs}{Tabs}-- Actualiza el/la {model} existente");
        sb.AppendLine($"{Tabs}{Tabs}UPDATE {model}");
        sb.AppendLine($"{Tabs}{Tabs}SET {sParamSpSet}");
        if (sParamWhereSp != null)
        {
            sb.AppendLine($"{Tabs}{Tabs}WHERE {sParamWhereSp13};");
        }

        sb.AppendLine($"{Tabs}ELSE");
        sb.AppendLine($"{Tabs}{Tabs}-- Inserta una nueva {model}");
        sb.AppendLine($"{Tabs}{Tabs}INSERT INTO {model}(");
        //
        // Insertar parametros INSERT 
        // Estilo:
        //     INSERT INTO Tabla ( Campo1
        //                        ,Campo2 )
        //
        int nUltimoElemento = camposDB.Count - 1;
        string sFieldFirst = camposDB[0].Field.ToString();
        string sFieldLast = camposDB[nUltimoElemento].Field.ToString();
        string sTabOrNull = string.Concat(Enumerable.Repeat(Tabs, 4));
        foreach (Entity2 o in camposDB)
        {
            sb.Append($"{sTabOrNull}{o.Field}{(o.Field.ToString() == sFieldLast ? " )" : "\r\n")}");
            sTabOrNull = string.Concat(Enumerable.Repeat(Tabs, 4)) + ",";
        }
        //
        // Insertar parametros VALUES
        // Estilo:
        //               VALUES ( Campo1
        //                       ,Campo2 )
        //
        sb.Append($"\r\n{Tabs}{Tabs} VALUES (");
        sTabOrNull = "";
        foreach (Entity2 o in camposDB)
        {
            sb.Append($"{sTabOrNull}p{o.Field}{(o.Field.ToString() == sFieldLast ? ");" : "\r\n")}");
            sTabOrNull = string.Concat(Enumerable.Repeat(Tabs, 4)) + ",";
        }
        sb.AppendLine("");
        sb.AppendLine($"{Tabs}END IF;");
        sb.AppendLine($"{Tabs}-- Si llegó aquí sin errores, confirma la transacción");
        sb.AppendLine($"{Tabs}COMMIT;");
        sb.AppendLine($"{Tabs}");
        sb.AppendLine($"{Tabs}-- Mensaje de éxito (opcional)");
        sb.AppendLine($"{Tabs}SELECT CONCAT('Operación exitosa en Bodega ID: ',");
        sb.AppendLine($"{Tabs}{Tabs}{Tabs}IFNULL({strPK}, LAST_INSERT_ID())) AS Resultado;");
        sb.AppendLine("END;");
        sb.AppendLine("$$");
        sb.AppendLine("");
        sb.Append(FooterSpMethod(SP));
        return sb.ToString();

    }


}
