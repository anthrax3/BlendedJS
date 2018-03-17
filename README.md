# BlendedJS

[Jint](https://github.com/sebastienros/jint) wrapper, adding extra classes to JavaScript.

 ```cs
 BlendedJSEngine engine = new BlendedJSEngine();
 var result = engine.ExecuteScript(
  @"
    var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
    sqlClient.query('select * from employees');
  ");
  Assert.IsNotNull(result);
  Assert.AreEqual(8, ((object[])result.Value).Length);
```

                
 ## SqlClient
 #### Run SQL
 ```javascript
  //provider:Sqlite | MySql | SqlServer | PostgreSQL
  //connectionString: Server=serverAddress;Database=dbName;User Id=user; Password=pass;
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var rows = sqlClient.query('select * from employees');
```

 #### Run SQL with parameter
 ```javascript
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var rows = sqlClient.query({
    sql:'select * from employees where EmployeeId=@EmployeeId', 
    parameters:{EmployeeId:1}
  });
```

 #### Run SQL with parameter (other version)
 ```javascript
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var rows = sqlClient.query('select * from employees where EmployeeId=@EmployeeId', {EmployeeId:1});
```


