open System
open System.Text
open System.Net
open System.Net.Sockets
open System.Threading
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open ClientInfo

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
  printfn "currectly initialized..."
  printfn "initializing binding..."
  let endpoint = IPEndPoint(sett.LocalIP, sett.LocalPort)
  clientSocket.Bind(endpoint)
  endpoint, clientSocket

let TestConnection (endpoint : IPEndPoint) (socket : Socket) =
  ignore <| socket.SendTo(Encoding.ASCII.GetBytes("REQ"), endpoint)
  let recbuffer = Array.create 10 (new Byte())
  ignore <| socket.Receive(recbuffer)
  match recbuffer with
  | data when Encoding.ASCII.GetString(data) = "ACK" -> printfn "ACK received!"
                                                        true
  | _ ->  printfn "no ACK received"
          false
//data receive, send and parse functions
let ParseStateBytes bytes : NETStateStack =
  [({Players = []; Asteroids = []; Projectiles = []}, 0)]
  //parse the bytes here, this will be a moderately complicated operation

let ReceiveStateBytes (socket : Socket) (endpoint : IPEndPoint) =
  let byteData = Array.create 1000 (new Byte())
  ignore <| socket.Receive(byteData)



let endpoint, socket = BootClient localSettings
while not (TestConnection endpoint socket) do
  printfn "couldn't connect to or find server, you dun goofed"
printfn "we found a server, let's see if we can talk to it!"
//put the actual logic of interacting with the game here
while true do
  let receivedState = ParseStateBytes (ReceiveStateBytes endpoint, socket)
  do 