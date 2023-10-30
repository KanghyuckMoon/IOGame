const WebSocket = require('ws')
const wss = new WebSocket.Server({ port: 7777 },()=>{
    console.log('서버시작')
})
wss.on('connection', function connection(ws) {
   ws.on('message', (data) => {
      console.log('데이터 : ' + data)
      ws.send(data);
   })
})
wss.on('listening',()=>{
   console.log('listening on 7777')
})