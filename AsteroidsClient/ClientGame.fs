module ClientGame

open System
open ClientInfo

let rec runGame (curState : NetState) : NetState =
  {Players = []; Asteroids = []; Projectiles = []; Time = 0}
  (*we need to check if the new state has been 
  *)