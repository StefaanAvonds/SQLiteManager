# SQLiteManager
An install-and-go package for SQLite. You will only need to install this package, add some models and DataAccess-classes and your SQLite-database is created!

**Be advised: this currently only works for Android!!**

## How to use?
It's really easy to use this package; as the description dictates: just install the package and you are ready to go with the SQLite-database. In NuGet search for **SQLiteDatabase** by Stefaan Avonds or use the Command Line *[SQLiteDatabase] (https://www.nuget.org/packages/SQLiteDatabase/1.0.0)*.

### DataModel
Every database consists of data; that is the whole purpose of a database. The records itself can be defined by using **DataModels**.

In your solution create a new class and let it derive from BaseDataModel. For instance:
```C#
public class User : SQLiteManager.DataModels.BaseDataModel
{
    private string _username;
    private string _password;

    public String Username
    {
        get { return _username; }
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public String Password
    {
        get { return _password; }
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public User()
        : base()
    {
        Username = String.Empty;
        Password = String.Empty;
    }

    public User(string username, string password)
        : this()
    {
        Username = username;
        Password = password;
    }
}
```

This BaseDataModel already has a few properties that will automatically be initialized upon creating a new object that derives from BaseDataModel:
- Guid ID;
- Boolean IsDeleted;
- DateTime InsertTimestamp;
- DateTime UpdateTimestamp;
- DateTime DeletedTimestamp.

Later more about these properties.

### DataAccess-class
Ofcourse a DataAccess-class is needed since this will be the class that query's the SQLite-table for that model.

In your solution create a new class that derives from **BaseDataAccess**. For instance:

```C#

public class DAUser : BaseDataAccess<User>
{
    public DAUser()
        : base()
    {

    }

    public User SelectByUsernameAndPassword(string username, string password)
    {
        return PerformQuery(() =>
        {
            return AsyncConnection.Table<User>()
                .Where(x => x.Username.ToLower().Equals(username.ToLower()) && x.Password.Equals(password))
                .FirstOrDefaultAsync();
        });
    }
}

```

**BaseDataAccess** expects a generic type of **BaseDataModel** so it is perfectly possible to use the newly created model from above.
By deriving from BaseDataAccess we no longer need to define the connection to SQLite, since this class does that for us. Also the default query's like "SelectAll" or "Insert" are already ready to use for any new DataAccess-class:

```C#
    public class BaseDataAccess<TDataModel> where TDataModel : BaseDataModel, new()
    {
        public BaseDataAccess();

        protected SQLiteAsyncConnection AsyncConnection { get; }

        public virtual bool CreateTable();
        public virtual int Delete(TDataModel item);
        public virtual int DeleteBatch(IEnumerable<TDataModel> items);
        public virtual bool DropTable();
        public virtual int Insert(TDataModel item);
        public virtual int InsertBatch(IEnumerable<TDataModel> items);
        public virtual List<TDataModel> SelectAll();
        public virtual TDataModel SelectById(Guid idPrimaryKey);
        public virtual int Update(TDataModel item);
        public virtual int UpdateBatch(IEnumerable<TDataModel> items);
        protected TResult PerformQuery<TResult>(Func<Task<TResult>> query);
        protected TResult PerformQuery<TResult>(Func<TResult> query);
    }
```

All these basic query's can be overridden for a specific implementation, but that is not necessary. Other query's that need to be used for this DataAccess-class specifically can be implemented pretty easily.

FYI: the default methods contain some logic for the default properties:
- The method **SelectAll** will only return the records with the flag *IsDeleted* set to FALSE and orders the result based on the *InsertTimestamp*.
- The method **SelectById** will search for a record by its Primary Key. This does not take into account the flag *IsDeleted*.
- The insert-methods **Insert** and **InsertBatch** will ofcourse insert one or multiple records. The properties *InsertTimestamp* and *UpdateTimestamp* are initialized to the current timestamp.
- The update-methods **Update** and **UpdateBatch** will update the given record. Next to that the property *UpdateTimestamp* is set to the current timestamp.
- The delete-methods **Delete** and **DeleteBatch** update the flag *IsDeleted* to TRUE. They will also update the property *DeletedTimestamp* to the current timestamp.


Another nice feature is the protected method **PerformQuery** that can be used for your own personal query's. This is actually just the equivalent of a try-catch block; the entire code inside "PerformQuery" is placed in a try-catch block but will return the type that you provide. If for some reason an exception has occured during execution of your query, the default value of that type will be returned.

```C#

protected TResult PerformQuery<TResult>(Func<Task<TResult>> query)
{
    try
    {
        return query.Invoke().Result;
    }
    catch (Exception ex)
    {
        return default(TResult);
    }
}
```

The protected method "PerformQuery" does not have to be used if no try-catch block is needed/wanted.

## Connection and ConnectionString
Every (SQLite-)database needs a Connection and a ConnectionString, else it will not know where the database is located. With the current package this step is no longer needed! It will automatically generate the database-file at the platform-specific location with a default name: "my_database.db3".
The filename (not the path) of the SQLite-file can easily be changed by 1 single statement - the best place is inside the App-class of your solution:

```C#

public App()
{
    SQLiteManager.Database.DatabaseFilename = "SQLiteSample.db3";

    MainPage = new NavigationPage(new LoginPage());
}

```

That's it! From now on your database uses a different filename!

The Connection itself will never need to be set manually; this package does that entirely for you:

```C#

public static SQLiteAsyncConnection AsyncConnection
{
    get
    {
        if (_asyncConnection == null) _asyncConnection = DependencyService.Get<ISQLite>().GetAsyncConnection();
        return _asyncConnection;
    }
}

```

## Export database
An extra feature is the export of your SQLite-database file. You can export the file with every table etc. to your SD-card just by implementing the method **TryExportDatabase**.
Multiple overloads are available, but every single one of them work the same: the SQLite-file is copied to a certain path and that very path will be returned with the out-parameter.

The exported file can be read by other applications like [SQLiteStudio] (http://sqlitestudio.pl/).

Just like the filename of the SQLite-database file, the filename of the exported file can be changed. By default this will be "Database.db3".

# Summary
To sum it all up, there are a few things you need to do to let this package work:
* Install this package;
* Create a DataModel that derives from BaseDataModel;
* Create a DataAccess-class that derives from BaseDataAccess;
* Maybe change the name of the database-file in your App-class;
* Use your query's.

That's it! Now every table looks and feels the same and new tables are easily added!
