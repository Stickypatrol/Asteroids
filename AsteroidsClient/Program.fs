open System
open System.Net
open System.Net.Sockets
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open ClientInfo
open ClientGame

//CONNECTION FUNCTIONS
type Settings =
  {
    LocalIP : IPAddress
    LocalPort : int
  }

let localSettings = {LocalIP = IPAddress.Parse "192.168.178.117"; LocalPort = 8888}

let BootClient (sett : Settings) =
  printfn "initializing socket..."
  let clientSocket = new Socket(SocketType.Dgram, ProtocolType.Udp)
  printfn "correctly initialized..."
  printfn "initializing binding..."
  let endpoint = IPEndPoint(sett.LocalIP, sett.LocalPort)
  clientSocket.Bind(endpoint)
  printfn "correctly bound to port, waiting for traffic."
  endpoint, clientSocket

let TestConnection (endpoint : IPEndPoint) (socket : Socket) =
  ignore <| socket.SendTo(Text.Encoding.ASCII.GetBytes("REQ"), endpoint)
  let recbuffer = Array.create 10 (new Byte())
  ignore <| socket.Receive(recbuffer)
  match recbuffer with
  | data when Text.Encoding.ASCII.GetString(data) = "ACK" ->  printfn "ACK received!"
                                                              true
  | _ ->  printfn "no ACK received"
          false

//RECEIVE, SEND AND PARSE FUNCTIONS
let ParseStateBytes bytes : NetState =
  {Players = []; Asteroids = []; Projectiles = []; Time = 0}
  //parse the bytes here, this will be a moderately complicated operation

let ReceiveStateBytes (endpoint : IPEndPoint) (socket : Socket) =
  let byteData = Array.create 1000 (new Byte())
  ignore <| socket.Receive(byteData)
  byteData

let ByterizeState (state:NetState) =
  Array.create 1000 (new Byte())

let SendCurrentState (endpoint : IPEndPoint) (socket : Socket) curState =
  ignore <| socket.SendTo(curState, endpoint)

//ACTUAL PROGRAM
let endpoint, socket = BootClient localSettings
let SendState = SendCurrentState endpoint socket
while not (TestConnection endpoint socket) do
  printfn "couldn't connect to or find server, you dun goofed"
printfn "we found a server, let's see if we can talk to it!"
//put the actual logic of interacting with the game here
while true do
  //do check for a received state
  let parsedState = ParseStateBytes (ReceiveStateBytes endpoint, socket)
  let clientState = runGame parsedState
  do SendState (ByterizeState clientState)