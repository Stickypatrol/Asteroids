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
open StateMonad
open GameState
open Math

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
        gameState <- gameState.LoadTextures gameState context.Content
        base.Initialize ()
        ()

    override context.LoadContent () =
        base.LoadContent ()
        ()

    override context.Update gameTime =
        base.Update gameTime
        gameState <- GameState.GameUpdate gameState (toGameTime <| gameTime.ElapsedGameTime.Milliseconds)
        ()

    override context.Draw gameTime =
        context.GraphicsDevice.Clear Color.CornflowerBlue
        spriteBatch.Begin ()
        GameState.GameDraw gameState spriteBatch
        spriteBatch.End ()
        base.Draw gameTime
        ()