(*
  MonoGame base game class overrides
*)

module Game

open System
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics
open Media
open Actors
open CstateMonad
open GameState
open Math

//The base game class provided by MonoGame
type AsteroidsGame () as context =
    inherit Game ()
  
    let mutable graphics = new GraphicsDeviceManager (context)
    let mutable spriteBatch = null
    let mutable gameState = GameState.Zero

    override context.Initialize () =
        base.Content.RootDirectory <- "Content"
        graphics.GraphicsProfile <- GraphicsProfile.HiDef
        spriteBatch <- new SpriteBatch (context.GraphicsDevice)
        gameState <- GameState.LoadTextures gameState context.Content
        base.Initialize ()
        ()

    override context.LoadContent () =
        base.LoadContent ()
        ()

    override context.Update gameTime =
        base.Update gameTime
        //here we run the mainUpdate function and return the 2nd(the state) item in the tuple it returns
        gameState <- snd(GameState.GameUpdate (gameTime.ElapsedGameTime.Milliseconds |> toGameTime) gameState)
        ()

    override context.Draw gameTime =
        context.GraphicsDevice.Clear Color.CornflowerBlue
        spriteBatch.Begin ()
        GameState.GameDraw gameState spriteBatch
        spriteBatch.End ()
        base.Draw gameTime
        ()