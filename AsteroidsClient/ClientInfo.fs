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
    Time           : int
  }