module ClientInfo

open Math

type Body =
  {
    Position       : Vector2<p>
    Velocity       : Vector2<p/s>
    Dimensions     : Vector2<p>
    Orientation    : float
  }

type NETPlayer =
  {
    Body          : Body
    Name          : string
  }

type NETAsteroid =
  {
    Body          : Body
  }

type NETProjectile =
  {
    Body          : Body
  }

type NETState =
  {
    Players        : List<NETPlayer>
    Asteroids      : List<NETAsteroid>
    Projectiles    : List<NETProjectile>
  }

type TimeStamp = int

type NETStateStack = List<NETState* TimeStamp>

(*
what we need to do is strip every state after execution to a NETState
then we tack the state onto the head of the NETStateStack, and pop the final one

*)