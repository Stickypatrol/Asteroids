module ClientGame

open System
open ClientInfo

let getTime () : int =
  let time = DateTime.Now
  time.Millisecond +
  (time.Second * 1000) +
  (time.Minute * 60000)
let startGame () : NETStateStack =
  

let stateStack = []