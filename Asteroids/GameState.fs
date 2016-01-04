(*
  the declaration of the gamestate and all members associated with it
*)

module GameState

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Actors
open Math
open Media
open CstateMonad
open NetworkInfo

type GameState =
  {
    Players        : ActorWrapper<Player, GameState>
    Asteroids      : ActorWrapper<Asteroid, GameState>
    Projectiles    : ActorWrapper<Projectile, GameState>
    Textures       : Map<string, Texture2D Option>
  }
  with
  static member Disassemble gs =
    gs.Players, gs.Asteroids, gs.Projectiles, gs.Textures
  static member GameUpdate (dt : float<s>) =
    fun gs ->
      let play, ast, proj, tex = GameState.Disassemble gs
      let players' = ActorWrapper<Player, GameState>.UpdateAll dt play gs
      let asteroids' = ActorWrapper<Asteroid, GameState>.UpdateAll dt ast gs
      let projectiles' = ActorWrapper<Projectile, GameState>.UpdateAll dt proj gs
      ((), {gs with Players = players'
                    Asteroids = asteroids'
                    Projectiles = projectiles'})
  static member GameDraw (gs : GameState) (sb : SpriteBatch) =
    let Draw =
      fun (b : Body) (name : string) ->
        let tex = match (gs.Textures.TryFind name) with
                  | Some tex -> tex
                  | None -> failwith "no appropriate texture found"
        let rect = System.Nullable(Rectangle(0, 0, tex.Value.Width, tex.Value.Height))
        sb.Draw(tex.Value, (b.Position.ToXNA), rect, Color.White, float32<| b.Orientation,
                Vector2(float32 <| tex.Value.Width/2, float32 <| tex.Value.Height/2), 1.0f, SpriteEffects.None, 0.0f)
    List.iter (fun (pl : Player) -> Draw pl.Body pl.Name) gs.Players.Actors
    List.iter (fun (ast : Asteroid) -> Draw ast.Body ast.Name) gs.Asteroids.Actors
    List.iter (fun (pro : Projectile) -> Draw pro.Body pro.Name) gs.Projectiles.Actors
  static member Zero =
    {//we have to declare the actorwrappers here because we have to give specific implementations
    //for each of their functions somewhere, and this is the most centralized location
      Players        = {
                        Actors = [Player.Start]
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