#nullable disable
using System.Text;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using System.Collections;
using System.Configuration;
using XgenBase;
using XauroCommon.Interface;

namespace XgenMySQL.Connect;

public partial class Database : IDatabase
{
    int defaultPort = 3306;
    string _port = null;
    string _database = "mysql";
    string passPhrase = "Pa55pr@se";  // can be any string
    string saltValue = "s@1tV@lue";  // can be any string
    string hashAlgorithm = "SHA1";       // can be "MD5"
    int passwordIterations = 2;            // can be any number
    string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
    int keySize = 256; // can be 192 or 128

    public string DataBaseType => "MySQL Server";
    public string ListDatabaseCommand => "SHOW DATABASES;";
    public string ListTablesCommand(string DatabaseName) => $"SELECT table_name FROM information_schema.tables WHERE table_schema = '{DatabaseName}';";

    #region Propiedades publicas
    public string Server { get; set; }
    public string Port
    {
        get
        {
            return (string.IsNullOrWhiteSpace(_port) ? defaultPort.ToString() : _port);
        }
        set
        {
            _port = value;
        }
    }
    public string User { get; set; }
    public string Password { get; set; }
    public string DatabaseName
    {
        get
        {
            _database = string.IsNullOrWhiteSpace(_database) ? "mysql" : _database;
            return _database;
        }
        set
        {
            _database = value;
        }
    }
    public string Provider => "MySql.Data.MySqlClient";
    #endregion

    private DbConnection conexion = null;
    private DbCommand comando = null;
    private DbTransaction transaccion = null;
    private string ConnectionString;
    private int _TIMEOUT = -1;

    //private static DbProviderFactory factory = null;

    /// <summary>
    /// Crea una instancia del acceso a la base de datos.
    /// </summary>
    public Database()
    {

    }


    /// <summary>
    /// Crea una instancia del acceso a la base de datos MySQL
    /// </summary>
    public Database(string servidor, string port, string userName, string password, string proveedor, string dataBase)
    {

        Server = servidor;
        User = userName;
        Password = password;
        Port = port;
        DatabaseName = dataBase;
        //Provider = proveedor;

        LoadParameters();
        Config();

        this.Server = servidor;
        this.Port = port; // EncriptacionPlus.Decrypt(port, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        this.User = userName; // EncriptacionPlus.Decrypt(userName, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        this.Password = password; // EncriptacionPlus.Decrypt(password, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);
        this.DatabaseName = dataBase; // EncriptacionPlus.Decrypt(dataBase, passPhrase, saltValue, hashAlgorithm, passwordIterations, initVector, keySize);

    }

    public void LoadParameters()
    {

        // Mapeo personalizado
        //
        string rutaConfig = "Xgen.dll.config";

        ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap
        {
            ExeConfigFilename = rutaConfig
        };

        // Cargar la configuración desde ese archivo
        Configuration appConfig = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

        passPhrase = appConfig.AppSettings.Settings["Frase"].Value;
        saltValue = appConfig.AppSettings.Settings["Valor"].Value;
        hashAlgorithm = appConfig.AppSettings.Settings["Seguridad"].Value;
        passwordIterations = int.Parse(appConfig.AppSettings.Settings["Iteracion"].Value);
        initVector = appConfig.AppSettings.Settings["Vector"].Value;
        keySize = int.Parse(appConfig.AppSettings.Settings["Tamaño"].Value);

    }

    public string ListFieldPKCommand(string TableName)
    {
        if (DatabaseName == null)
        {
            throw new DatabaseException("No se ha definido la base de datos.");
        }

        return @$"SELECT tc.CONSTRAINT_NAME
                       , tc.CONSTRAINT_TYPE
                       , kcu.TABLE_NAME
                       , kcu.COLUMN_NAME
                   FROM information_schema.TABLE_CONSTRAINTS tc
                   JOIN information_schema.KEY_COLUMN_USAGE kcu ON tc.CONSTRAINT_NAME = kcu.CONSTRAINT_NAME
                   AND tc.TABLE_NAME = kcu.TABLE_NAME
                   AND tc.CONSTRAINT_SCHEMA = kcu.CONSTRAINT_SCHEMA
                   WHERE tc.TABLE_SCHEMA = '{DatabaseName}'
                   AND tc.TABLE_NAME = '{TableName}';";
    }

    public int TimeOut
    {
        get { return _TIMEOUT == -1 ? 120 : _TIMEOUT; }
        set { _TIMEOUT = value; }
    }

    /// <summary>
    /// Configura el acceso a la base de datos para su utilización.
    /// </summary>
    /// <exception cref="BaseDatosException">Si existe un error al cargar la configuración.</exception>
    private void Config()
    {
        try
        {
            // Crea el stringconnection para MySQL
            var _server = this.Server + (!string.IsNullOrWhiteSpace(this.Port) ? $"{(this.Port.Equals(defaultPort.ToString()) ? "" : "," + this.Port.ToString())}" : "");
            this.ConnectionString = $"Server={_server}; Port={Port}; Database={DatabaseName}; Uid={User}; Pwd={Password};";
        }
        catch (ConfigurationException ex)
        {
            throw new DatabaseException("Error al cargar la configuración del acceso a datos.", ex);
        }
    }

    /// <summary>
    /// Configura el acceso a la base de datos para su utilización.
    /// </summary>
    /// <exception cref="BaseDatosException">Si existe un error al cargar la configuración.</exception>
    public void Config(
              string servidor
            , string port
            , string userName
            , string password
            , string proveedor
            , string dataBase
        )
    {

        try
        {

            Server = servidor;
            Port = port;
            User = userName;
            Password = password;
            DatabaseName = dataBase;

            LoadParameters();
            Config();

        }
        catch (ConfigurationException ex)
        {
            throw new DatabaseException("Error al cargar la configuración del acceso a datos.", ex);
        }

    }


    /// <summary>
    /// Permite desconectarse de la base de datos.
    /// </summary>
    public void Desconectar()
    {
        if (this.conexion.State.Equals(ConnectionState.Open))
        {

            this.conexion.Close();

        }
    }

    /// <summary>
    /// Se concecta con la base de datos.
    /// </summary>
    /// <exception cref="BaseDatosException">Si existe un error al conectarse.</exception>
    public bool Conectar()
    {
        bool lret = false;
        Config();
        if (this.conexion != null && !this.conexion.State.Equals(ConnectionState.Closed))
        {
            return true;
        }
        try
        {
            if (this.conexion == null)
            {

                this.conexion = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
                this.conexion.ConnectionString = ConnectionString;


            }
            this.conexion.Open();
            lret = true;
        }
        catch (DataException ex)
        {
            lret = false;
            throw new DatabaseException("Error al conectarse a la base de datos.", ex);
        }
        catch (Exception e)
        {
            lret = false;
            throw new DatabaseException("Error al conectarse a la base de datos.", e);
        }
        return lret;
    }

    /// <summary>
    /// Crea un comando en base a una sentencia SQL.
    /// Ejemplo:
    /// <code>SELECT * FROM Tabla WHERE campo1=@campo1, campo2=@campo2</code>
    /// Guarda el comando para el seteo de parámetros y la posterior ejecución.
    /// </summary>
    /// <param name="sentenciaSQL">La sentencia SQL con el formato: SENTENCIA [param = @param,]</param>
    public void CrearComando(string sentenciaSQL)
    {

        this.comando = conexion.CreateCommand();
        this.comando.CommandTimeout = this.TimeOut;
        this.comando.Connection = this.conexion;
        this.comando.CommandType = CommandType.Text;
        this.comando.CommandText = sentenciaSQL;
        if (this.transaccion != null)
        {
            this.comando.Transaction = this.transaccion;
        }
    }

    /// <summary>
    /// Setea un parámetro como nulo del comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro cuyo valor será nulo.</param>
    public void AsignarParametroNulo(string nombre)
    {
        DbParameter param = comando.CreateParameter(); ;
        param.DbType = System.Data.DbType.Object;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Value = DBNull.Value;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "", "NULL");
    }

    /// <summary>
    /// Asigna un parámetro de tipo cadena al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroCadena(string nombre, string valor)
    {
        DbParameter param = comando.CreateParameter();
        param.DbType = System.Data.DbType.String;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Size = valor.Length;
        param.Value = valor;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "'", valor);
    }

    /// <summary>
    /// Asigna un parámetro de tipo entero al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroEntero(string nombre, int valor)
    {
        DbParameter param = comando.CreateParameter(); ;
        param.DbType = System.Data.DbType.Int32;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Value = valor;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "", valor.ToString());
    }

    /// <summary>
    /// Asigna un parámetro de tipo Boolean al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroBoolean(string nombre, Boolean valor)
    {
        DbParameter param = comando.CreateParameter(); ;
        param.DbType = System.Data.DbType.Boolean;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Value = valor;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "", valor.ToString());
    }

    /// <summary>
    /// Asigna un parámetro de tipo double al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroDouble(string nombre, double valor)
    {

        DbParameter param = comando.CreateParameter(); ;
        param.DbType = System.Data.DbType.Double;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Value = valor;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "", valor.ToString("#.#"));
    }

    /// <summary>
    /// Asigna un parámetro de tipo double al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroFloat(string nombre, float valor)
    {

        DbParameter param = comando.CreateParameter(); ;
        param.DbType = System.Data.DbType.Double;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Value = valor;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "", valor.ToString("#.#"));
    }

    //public void AsignarParametroImage(string nombre, System.IO.MemoryStream valor = null )
    public void AsignarParametroImage(string nombre, byte[] valor = null)
    {
        DbParameter param = comando.CreateParameter();
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        // param.Value = valor.GetBuffer();
        param.Value = valor;

        comando.Parameters.Add(param);

        // this.comando.Parameters.Add(SqlDbType.Image);
        //this.comando.Parameters[nombre].Value = valor.GetBuffer();
    }

    /// <summary>
    /// Asigna un parámetro de tipo fecha al comando creado.
    /// </summary>
    /// <param name="nombre">El nombre del parámetro.</param>
    /// <param name="valor">El valor del parámetro.</param>
    public void AsignarParametroFecha(string nombre, DateTime valor)
    {
        DbParameter param = comando.CreateParameter(); ;
        param.DbType = System.Data.DbType.DateTime;
        param.Direction = ParameterDirection.Input;
        param.ParameterName = nombre;
        param.Value = valor;

        comando.Parameters.Add(param);

        // AsignarParametro(nombre, "'", valor.ToString());
    }

    /// <summary>
    /// Ejecuta el comando creado y retorna el resultado de la consulta.
    /// </summary>
    /// <returns>El resultado de la consulta.</returns>
    /// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
    public DbDataReader EjecutarConsulta()
    {
        return this.comando.ExecuteReader();
    }

    /// <summary>
    /// Ejecuta el comando creado y retorna un escalar.
    /// </summary>
    /// <returns>El escalar que es el resultado del comando.</returns>
    /// <exception cref="BaseDatosException">Si ocurre un error al ejecutar el comando.</exception>
    public int EjecutarEscalar()
    {
        int escalar = 0;
        try
        {
            // this.comando.CommandType = CommandType.StoredProcedure;
            escalar = int.Parse(this.comando.ExecuteScalar().ToString());
        }
        catch (InvalidCastException ex)
        {
            throw new DatabaseException("Error al ejecutar un escalar.", ex);
        }
        finally
        {

        }
        return escalar;
    }

    /// <summary>
    /// Ejecuta el comando creado.
    /// </summary>
    public void EjecutarComando()
    {
        this.comando.ExecuteNonQuery();
    }

    /// <summary>
    /// Comienza una transacción en base a la conexion abierta.
    /// Todo lo que se ejecute luego de esta ionvocación estará 
    /// dentro de una tranasacción.
    /// </summary>
    public void ComenzarTransaccion()
    {
        if (this.transaccion == null)
        {
            this.transaccion = this.conexion.BeginTransaction();
        }
    }

    /// <summary>
    /// Cancela la ejecución de una transacción.
    /// Todo lo ejecutado entre ésta invocación y su 
    /// correspondiente <c>ComenzarTransaccion</c> será perdido.
    /// </summary>
    public void CancelarTransaccion()
    {
        if (this.transaccion != null)
        {
            this.transaccion.Rollback();
        }
    }

    /// <summary>
    /// Confirma todo los comandos ejecutados entre el <c>ComanzarTransaccion</c>
    /// y ésta invocación.
    /// </summary>
    public void ConfirmarTransaccion()
    {
        if (this.transaccion != null)
        {
            this.transaccion.Commit();
        }
    }

    public string DecryptString(string inputString, int dwKeySize, string xmlString)
    {
        RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
        rsaCryptoServiceProvider.FromXmlString(xmlString);
        int base64BlockSize = ((dwKeySize / 8) % 3 != 0) ? (((dwKeySize / 8) / 3) * 4) + 4 : ((dwKeySize / 8) / 3) * 4;
        int iterations = inputString.Length / base64BlockSize;
        ArrayList arrayList = new ArrayList();
        for (int i = 0; i < iterations; i++)
        {
            byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
            Array.Reverse(encryptedBytes);
            arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
        }
        return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
    }


}
