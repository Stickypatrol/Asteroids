module ClientGame

open System
open ClientInfo

let rec runGame (curState : NETState) : NETState =
  {Players = []; Asteroids = []; Projectiles = []; Time = 0}
//simulate the game with the received state here until we get a new update from the server