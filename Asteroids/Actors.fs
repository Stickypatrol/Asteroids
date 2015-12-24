(*
  All actor records and associated functionality
*)

module Actors

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Math
open Input

//Physical body of entities in the game world
type Body =
  {
    Position       : Vector2<p>
    Velocity       : Vector2<p/s>
    Dimensions     : Vector2<p>
    Orientation    : float
  }
  with
  static member Zero =
    {
      Position    = Vector2<p>.Zero
      Velocity    = Vector2<p/s>.Zero
      Dimensions  = Vector2<p>.Zero
      Orientation = 0.0
    }
  static member Move (b : Body) (dt : float<s>) =
    {b with Position = b.Position + b.Velocity * dt}

type Ship =
  {
    Body          : Body
    Health        : int
    InputBehavior : InputBehavior<Ship>
  }
  with
  static member Zero =
    {
      Body          = Body.Zero
      Health        = 1
      InputBehavior = Map.empty
    }
  static member Update (s : Ship) (dt : float<s>) =
    {s with Body = Body.Move s.Body dt}

//The player is a special kind of ship
type Player =
  {
    Ship          : Ship
    Name          : string
    Score         : int
    InputBehavior : InputBehavior<Player>
  }
  with
  //static member Thrust =
    //fun p -> {p with Body = Body.Thrust}
  static member Zero =
    {
      Ship          = Ship.Zero
      Name          = "Player"
      Score         = 0
      InputBehavior =
        [
          KeyboardInput(Keys.W, None), fun s -> s //Player.Thrust
          KeyboardInput(Keys.A, None), fun s -> s
          KeyboardInput(Keys.D, None), fun s -> s
        ] |> Map.ofList
    }

type Asteroid =
  {
    Body          : Body
    Size          : int
    InputBehavior : InputBehavior<Asteroid>
  }
  with
  static member Zero =
    {
      Body          = Body.Zero
      Size          = 3
      InputBehavior = Map.empty
    }

type Projectile =
  {
    Body          : Body
    Owner         : Player
    InputBehavior : InputBehavior<Projectile>
  }
  with
  static member Zero =
    {
      Body          = Body.Zero
      Owner         = Player.Zero
      InputBehavior = Map.empty
    }

//A wrapper that helps express functionality for a cluster of actors of the same type
type ActorWrapper<'a> =
  {
    Actors : 'a List
    Update : 'a -> 'a
    Create : Unit -> 'a List
    Remove : 'a -> bool
  }
  with
  static member Zero =
    {
      Actors = List<'a>.Empty
      Update = fun s -> s
      Create = fun () -> []
      Remove = fun s -> false
    }
