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
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  var rows = sqlClient.query('select * from employees');
```

 #### Run SQL with parameter
 ```javascript
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  sqlClient.query({
    sql:'select * from employees where EmployeeId=@EmployeeId', 
    parameters:{EmployeeId:1}
  });
```

 #### Run SQL with parameter (other version)
 ```javascript
  var sqlClient = new  SqlClient({provider:'Sqlite',connectionString:'Data Source = chinook.db;'});
  sqlClient.query('select * from employees where EmployeeId=@EmployeeId', {EmployeeId:1});
```

<table>
<tr>
<th>Member</th>
<th>Description</th>
</tr>
<tr>
 <td><code>constructor(provider:String, connectionString:String)</code></td>
 <td>constructor takes provider and connection string; provider:Sqlite|SqlServer|MySql|PostgreSQL/td>
</tr>
<tr>
 <td>query(sqlOrOptions:Object) :Object</td>
 <td>run SQL</td>
</tr>
<tr>
 <td>query(sql:String, parameters:Object)</td>
 <td>run SQL with parameters</td>
</tr>
</table>

