# HttpClient

## Get

 ```javascript
  var httpClient = new HttpClient();
  var response = httpClient.get('https://www.theguardian.com');
  console.log(response);
  //{
  //"statusCode": 200,
  //"reasonPhrase": "OK",
  //"body": "\n<!DOCTYPE html>\n<html ...",
  //"headers": {
  //  "Accept-Ranges": "bytes",
  //  "Date": "Thu, 12 Apr 2018 20:27:26 GMT",
  //  "Age": "0",
  //  "Connection": "keep-alive"
  //}
}
```

 ```javascript
var httpClient = new HttpClient();
var response = httpClient.get({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'}
 });
var body = JSON.parse(response.body);
```
