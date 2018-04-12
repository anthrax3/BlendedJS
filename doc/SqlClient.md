# SqlClient

#### Initialization

- Sqlite
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = database.db;'});
  
  //using properties
  var sqlClient = new  SqlClient({provider:'Sqlite', dataSource:'database.db'});
```

- MySql
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'MySql',connectionString:'SERVER=ServerUrl;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'MySql',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'mysql://UserId:Password@ServerUrl/DatabaseName'});
```

- MariaDb
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'MariaDb',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'MariaDb',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'mariadb://UserId:Password@ServerUrl/DatabaseName'});
```

- Postgres
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Postgres',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'Postgres',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'postgres://UserId:Password@ServerUrl/DatabaseName'});
```

- Oracle (not tested)
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Oracle',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'Oracle',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'oracle://UserId:Password@ServerUrl/DatabaseName'});
```

- SqlServer (not tested)
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'SqlServer',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'SqlServer',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'sqlserver://UserId:Password@ServerUrl/DatabaseName'});
```

- DB2 (not tested)
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'DB2',connectionString:'SERVER=ServerId;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'DB2',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Passowrd'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'db2://UserId:Password@ServerUrl/DatabaseName'});
```

- Odbc
 ```javascript
  //using connectionString
  var sqlClient = new  SqlClient({provider:'Odbc',connectionString:'Driver={MySQL ODBC 5.1 Driver};SERVER=ServerUrl;DATABASE=DatabaseName;UID=UserId;PASSWORD=Password;'});
  
  //using properties
  var sqlClient = new  SqlClient({
                        provider:'Odbc',
                        driver:'{MySQL ODBC 5.1 Driver}',
                        server:'ServerUrl',
                        database:'DatabaseName',
                        user:'UserId',
                        password:'Password'});
                        
  //using connectionUrl
  var sqlClient = new  SqlClient({connectionUrl:'mysql://UserId:Password@ServerUrl/DatabaseName'});
```


 #### Run SQL
 ```javascript
  //provider:Sqlite | MySql | SqlServer | PostgreSQL
  //connectionString: Server=serverAddress;Database=dbName;User Id=user; Password=pass;
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var rows = sqlClient.query('select * from employees');
  console.log(rows);   // [{EmployeeId:1, ...},...]
```

 #### Run SQL with parameter
 ```javascript
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var rows = sqlClient.query({
    sql:'select * from employees where EmployeeId=@EmployeeId', 
    parameters:{EmployeeId:1}
  });
  console.log(rows);   // [{EmployeeId:1, ...}]
```

 #### Run SQL with parameter (other version)
 ```javascript
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var row = sqlClient.query('select * from employees where EmployeeId=@EmployeeId', {EmployeeId:1});
  console.log(row);   // [{EmployeeId:1, ...}]
```
