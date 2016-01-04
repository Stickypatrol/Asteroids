module NetworkInfo

open Math
open Actors

type NetPlayer =
  {
    Body          : Body
    Name          : string
  }

type NetAsteroid =
  {
    Body          : Body
    Name          : string
  }

type NetProjectile =
  {
    Body          : Body
    Name          : string
  }

type NetState =
  {
    Players        : List<NetPlayer>
    Asteroids      : List<NetAsteroid>
    Projectiles    : List<NetProjectile>
  }

type TimeStamp = int

type NetStateStack = List<NetState* TimeStamp>

(*
what we need to do is strip every state after execution to a NETState
then we tack the state onto the head of the NETStateStack, and pop the final one

*)