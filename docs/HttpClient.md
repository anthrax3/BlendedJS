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
response.bodyJson = JSON.parse(response.body);                        
console.log(response);
//{
//  "statusCode": 200,
//  "reasonPhrase": "OK",
//  "body": "[\n  {\n    \"userId\": 1,\n    \"id\": 1,\n    \"title\": \"sunt aut facere repellat provident occaecati ...",
//  "headers": {
//    "Connection": "keep-alive",
//    "Date": "Sun, 15 Apr 2018 12:41:51 GMT",
//    "Pragma": "no-cache"
//  },
//  "bodyJson": [
//    {
//      "userId": 1,
//      "id": 1,
//      "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
//      ...
//    },
//    ...
//  ]
//}
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

