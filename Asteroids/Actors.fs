(*
  the declaration of the typeclasses of the actual actors of the game
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
    {b with Position = b.Position + b.Velocity * dt}//this function applies the velocity to the position
  static member TransformVel (b : Body) (vel : Vector2<p/s>) =
    {b with Velocity = b.Velocity + vel}//this function is for transforming the velocity
  static member TransformOri (b : Body) (orient : float) =
    {b with Orientation = b.Orientation + orient}//this function is for transforming the orientations
//removed the ship typeclass and gave the player a body instead of a ship to keep consistency across typeclasses
type Player =
  {
    Body          : Body
    Name          : string
    Score         : int
    InputBehavior : InputBehavior<Player>
  }
  with
  static member Update player dt =
    let player' = ProcessInput player.InputBehavior player
    {player' with//this function first updates the body of the object
      Body = Body.Move player'.Body dt}//and then applies the appropriate velocity
  static member Create () =
    List<Player>.Empty//for now there is no create function, it just returns an empty list
  static member Remove player world : bool =
    //check here for collision and health depletion
    false//for now it just always returns false
  static member Zero =
    {
      Body          = Body.Zero
      Name          = "Player"
      Score         = 0
      InputBehavior =
        [//here is an example of how imho it makes it a little more readable, we can use it but need to be consistent
          Keys.W, fun pl -> {pl with Body = Body.TransformVel pl.Body (Vec2 -5.0<p/s> 0.0<p/s>)}
          Keys.S, fun pl -> {pl with Body = Body.TransformVel pl.Body (Vec2 5.0<p/s> 0.0<p/s>)}
          Keys.A, fun pl -> {pl with Body = Body.TransformOri pl.Body 0.05}
          Keys.D, fun pl -> {pl with Body = Body.TransformOri pl.Body -0.05}
        ] |> Map.ofList
    }

type Asteroid =
  {
    Body          : Body
    Name          : string
    Size          : int
    InputBehavior : InputBehavior<Asteroid>
  }
  with
  static member Update asteroid dt =
    let asteroid' = ProcessInput asteroid.InputBehavior asteroid
    {asteroid' with//this function first updates the body of the object
      Body = Body.Move asteroid'.Body dt}//and then applies the appropriate velocity
  static member Create () =
    List<Asteroid>.Empty//for now there is no create function, it just returns an empty list
  static member Remove asteroid world : bool =
    //check here for collision and health depletion
    false//for now it just always returns false
  static member Zero =
    {
      Body          = Body.Zero
      Name          = "asteroid"
      Size          = 3
      InputBehavior = Map.empty
    }

type Projectile =
  {
    Body          : Body
    Name          : string
    Owner         : Player
    InputBehavior : InputBehavior<Projectile>
  }
  with
  static member Update projectile dt =
    let projectile' = ProcessInput projectile.InputBehavior projectile
    {projectile' with//this function first updates the body of the object
      Body = Body.Move projectile'.Body dt}//and then applies the appropriate velocity
  static member Create () =
    List<Projectile>.Empty//for now there is no create function, it just returns an empty list
  static member Remove projectile world : bool =
    //check here for collision and health depletion
    false//for now it just always returns false
  static member Zero =
    {
      Body          = Body.Zero
      Name          = "projectile"
      Owner         = Player.Zero
      InputBehavior = Map.empty
    }

//A wrapper that helps express functionality for a cluster of actors of the same type
type ActorWrapper<'a, 's> =
  {
    Actors : 'a List
    Update : 'a -> float<s> ->'a
    Create : Unit -> 'a List
    Remove : 'a -> 's -> bool
  }
  with
  static member UpdateAll (wrpr : ActorWrapper<'a, 's>) state dt =
    {wrpr with Actors =
                (wrpr.Create()) @ [for e in wrpr.Actors do
                                    if not (wrpr.Remove e state) then
                                      let e' = wrpr.Update e dt
                                      yield e']}
//changed the static functions to take the additional necessary parameters