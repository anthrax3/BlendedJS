# HttpClient

## Get
- Get website content
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

- Get json
 ```javascript
var httpClient = new HttpClient();
var response = httpClient.get({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'}
 });
var posts = JSON.parse(response.body);
console.log(posts);
```

## Post
- Post json
```javascript
var httpClient = new HttpClient();
var response = httpClient.post({
  url:'https://jsonplaceholder.typicode.com/posts',
  headers: {'Content-Type':'application/json'},
  body: { userId: 1, title: 'bla', body: 'bla bla bla'}
  });
var responseStatus = JSON.parse(response.body);
console.log(responseStatus);
```

## Put
- Put json
```javascript
var httpClient = new HttpClient();
var response = httpClient.put({
  url:'https://jsonplaceholder.typicode.com/posts/1',
  headers: {'Content-Type':'application/json'},
  body: { userId: 1, title: 'bla', body: 'bla bla bla'}
  });
var responseStatus = JSON.parse(response.body);
console.log(responseStatus);
```

## Delete
- Delete json
```javascript
var httpClient = new HttpClient();
var response = httpClient.delete({
 url:'https://jsonplaceholder.typicode.com/posts/1'
 });
var responseStatus = JSON.parse(response.body);
console.log(responseStatus);
```

## Head
- Head json
```javascript
var httpClient = new HttpClient();
var response = httpClient.head({
 url:'https://jsonplaceholder.typicode.com/posts/1'
 });
var responseStatus = JSON.parse(response.body);
console.log(responseStatus);
```

## Send
- Send head
```javascript
var httpClient = new HttpClient();
var response = httpClient.send({
 method:'HEAD'
 url:'https://jsonplaceholder.typicode.com/posts/1'
 });
var responseStatus = JSON.parse(response.body);
console.log(responseStatus);
```

