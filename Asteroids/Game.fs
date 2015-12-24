(*
  MonoGame base game class overrides
*)

module Game

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Media
open Actors
open StateMonad
open GameState

//The base game class provided by MonoGame
type AsteroidsGame () as context =
    inherit Game ()
  
    let mutable graphics = new GraphicsDeviceManager (context)
    let mutable spriteBatch = null
    let mutable gameState = GameState.Zero

    override context.Initialize () =
        graphics.GraphicsProfile <- GraphicsProfile.HiDef
        context.Content.RootDirectory <- "Content"
        spriteBatch <- new SpriteBatch (context.GraphicsDevice)

        gameState <-
            {
                Players        = ActorWrapper<Player>.Zero
                Asteroids      = ActorWrapper<Asteroid>.Zero
                Projectiles    = ActorWrapper<Projectile>.Zero
                Textures       = Map.empty
            }

        base.Initialize ()
        ()

    override context.LoadContent () =
        base.LoadContent ()
        ()

    override context.Update gameTime =
        base.Update gameTime
        ()

    override context.Draw gameTime =
        context.GraphicsDevice.Clear Color.CornflowerBlue
        spriteBatch.Begin ()

        spriteBatch.End ()
        base.Draw gameTime
        ()
