(*
  the declaration of the typeclasses of the actual actors of the game
*)

module Actors

open Microsoft.Xna.Framework.Graphics
open Microsoft.Xna.Framework.Input
open Math
open Input
open CstateMonad


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
    {b with Velocity = b.Velocity + vel}//this function is for transforming the velocity, we need a calculation here that takes the rotation into account
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
  static member Update dt =
    fun player ->
      let player' = ProcessInput player.InputBehavior player
      let limitedPlayer =
        {player' with Body =
                      {player'.Body with  Velocity = LimitVecAbs (>) {X = 0.05<p/s>; Y = 0.05<p/s>} player'.Body.Velocity
                                          Position = Middelize (LimitVecAbs (>) {X = 400.0<p>; Y = 240.0<p>})
                                                                      {X = 400.0<p>; Y = 240.0<p>} player'.Body.Position}} //then clamps the speed to the limit for the ship
      Done({limitedPlayer with
              Body = Body.Move limitedPlayer.Body dt}, player)
  static member Create () =
    fun s ->
      Done(List<Player>.Empty, s)//for now there is no create function, it just returns an empty list
  static member Remove player world : bool =
    //check here for collision and health depletion
    false//for now it just always returns false
  static member Zero =
    {
      Body          = Body.Zero
      Name          = "player"
      Score         = 0
      InputBehavior =
        [//here is an example of how imho it makes it a little more readable, we can use it but need to be consistent
          Keys.W, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = 0.0<p/s>; Y = -0.003<p/s>}}
          Keys.S, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = 0.0<p/s>; Y = 0.003<p/s>}}
          Keys.A, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = -0.003<p/s>; Y = 0.0<p/s>}}
          Keys.D, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = 0.003<p/s>; Y = 0.0<p/s>}}
        ] |> Map.ofList
    }
  static member Start =
    {
      Body          = { Position = {X = 400.0<p>;Y = 240.0<p>};
                        Velocity = {X = 0.0<p/s>;Y = 0.0<p/s>};
                        Dimensions = {X = 0.0<p>;Y = 0.0<p>};
                        Orientation = 0.0}
      Name          = "player"
      Score         = 0
      InputBehavior =
        [//here is an example of how imho it makes it a little more readable, we can use it but need to be consistent
          Keys.W, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = 0.0<p/s>; Y = -0.003<p/s>}}
          Keys.S, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = 0.0<p/s>; Y = 0.003<p/s>}}
          Keys.A, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = -0.003<p/s>; Y = 0.0<p/s>}}
          Keys.D, fun pl -> {pl with Body = Body.TransformVel pl.Body {X = 0.003<p/s>; Y = 0.0<p/s>}}
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
    Update : 'a -> float<s> -> Cstate<'a, 's>
    Create : Unit -> 'a List
    Remove : 'a -> 's -> bool
  }
  with
  static member UpdateAll dt (wrpr : ActorWrapper<'a, 's>)=
    cs{
      let! created = wrpr.Create()
      
    }
    
    (*fun gs ->
      let Actors' = (wrpr.Create()) @ [for e in wrpr.Actors do
                                        if not (wrpr.Remove e gs) then
                                          let e' = wrpr.Update e dt
                                          yield e']
      Done({wrpr with Actors = Actors'}, gs)
  (*we need to update each entity
  remove the ones that need to be removed

  *)