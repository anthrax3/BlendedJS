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

 ## HttpClient
 #### Get website
 ```javascript
 var httpClient = new HttpClient();
 var response = httpClient.get('https://www.theguardian.com');
 console.log(response.statusCode);     // 200
 console.log(response.reasonPhrase);   // OK
 console.log(response.body);           // <html>...
 console.log(response.headers);        // { {'Connection':'keep-alive'}...}
```

#### Get rest api
 ```javascript
 var httpClient = new HttpClient();
 var response = httpClient.get({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'}
 });
 var posts = JSON.parse(response.body);
 console.log(posts);   // [ {"userId": 1, ...},...]
```

#### Post rest api
 ```javascript
 var httpClient = new HttpClient();
 var response = httpClient.post({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'},
  body: { userId: 1, title: 'bla', body: 'bla bla bla'}
 });
 var post = JSON.parse(response.body);
 console.log(post);   // {"id": 1}
```

