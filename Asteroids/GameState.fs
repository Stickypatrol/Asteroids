(*
  the declaration of the gamestate and all members associated with it
*)

module GameState

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Actors
open Math
open Media

type GameState =
  {
    Players        : ActorWrapper<Player, GameState>
    Asteroids      : ActorWrapper<Asteroid, GameState>
    Projectiles    : ActorWrapper<Projectile, GameState>
    Textures       : Map<string, Texture2D Option>
  }
  with
  static member GameUpdate (gs : GameState) (dt : float<s>) =
    {(gs:GameState) with
      Players = ActorWrapper<Player, GameState>.UpdateAll gs.Players gs dt
      Asteroids = ActorWrapper<Asteroid, GameState>.UpdateAll gs.Asteroids gs dt
      Projectiles = ActorWrapper<Projectile, GameState>.UpdateAll gs.Projectiles gs dt}
  static member GameDraw (gs : GameState) (sb : SpriteBatch) =
    let Draw =
      fun (b : Body) (name : string) ->
        let tex = match (gs.Textures.TryFind name) with
                  | Some tex -> tex
                  | None -> failwith "no appropriate texture found"
        sb.Draw(tex.Value, (b.Position.ToXNA), Color.White)
    printfn "draw"//I know this is ugly we should fix this somehow, throwing an error is not very pretty
    List.iter (fun (pl : Player) -> Draw pl.Body pl.Name) gs.Players.Actors
    List.iter (fun (ast : Asteroid) -> Draw ast.Body ast.Name) gs.Asteroids.Actors
    List.iter (fun (pro : Projectile) -> Draw pro.Body pro.Name) gs.Projectiles.Actors
  static member Zero =
    {//we have to declare the actorwrappers here because we have to give specific implementations
    //for each of their functions somewhere, and this is the most centralized location
      Players        = {
                        Actors = [Player.Zero]
                        Update = Player.Update;
                        Create = Player.Create;
                        Remove = Player.Remove;
                        }
      Asteroids      = {
                        Actors = List<Asteroid>.Empty;
                        Update = Asteroid.Update;
                        Create = Asteroid.Create;
                        Remove = Asteroid.Remove;
                        }
      Projectiles    = {
                        Actors = List<Projectile>.Empty;
                        Update = Projectile.Update;
                        Create = Projectile.Create;
                        Remove = Projectile.Remove;
                        }
      Textures       = Map.empty<string, Option<Texture2D>>
    }
  static member LoadTextures gs content =
    { gs with
        Textures = ["player", (loadTexture content "player")
                    "asteroid", (loadTexture content "asteroid")
                    "projectile", (loadTexture content "projectile")
                    ] |> Map.ofList
      
    }