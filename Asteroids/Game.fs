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

  //let mutable gameState = GameState.Zero

  override context.Initialize () =
    context.Content.RootDirectory <- "Content"
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
    //spriteBatch.Begin ()

    //spriteBatch.End ()
    base.Draw gameTime
    ()
