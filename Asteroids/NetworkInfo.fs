module NetworkInfo

open Math
open Actors

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