module ClientInfo

open Math

type Body =
  {
    Position       : Vector2<p>
    Velocity       : Vector2<p/s>
    Dimensions     : Vector2<p>
    Orientation    : float
  }

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
    Time           : int
  }