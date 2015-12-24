(*
  A record to keep track of the game state
  This is what's being propagated by the state monad
*)

module GameState

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Actors

type GameState =
  {
    Players        : ActorWrapper<Player>
    Asteroids      : ActorWrapper<Asteroid>
    Projectiles    : ActorWrapper<Projectile>
    Textures       : Map<string, Texture2D Option>
  }
  with
  static member Zero =
    {
      Players        = ActorWrapper<Player>.Zero
      Asteroids      = ActorWrapper<Asteroid>.Zero
      Projectiles    = ActorWrapper<Projectile>.Zero
      Textures       = Map.empty
    }
