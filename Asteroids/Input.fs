module Input

open System
open Microsoft.Xna.Framework.Input
open Math

type MouseInput =
  | LeftButton   = 0
  | MiddleButton = 1
  | RightButton  = 2

//A conversion from an action(key) to an output(transformation action)
type InputBehavior<'a> = Map<Keys, 'a -> 'a>

//we need to deal with the mouse input differently than
//the keyboard input so we can combine them if needed
let checkRightButton mouselist =
  if Mouse.GetState().LeftButton = ButtonState.Pressed then
    MouseInput.RightButton::mouselist
  else
    mouselist
    
let checkMiddleButton mouselist =
  if Mouse.GetState().LeftButton = ButtonState.Pressed then
    checkRightButton (MouseInput.MiddleButton::mouselist)
  else
    checkRightButton mouselist

let checkLeftButton () =
  if Mouse.GetState().LeftButton = ButtonState.Pressed then
    checkMiddleButton [MouseInput.LeftButton]
  else
    checkMiddleButton []

let ProcessInput (beh : InputBehavior<'a>) =
  fun elem ->
    let KBinput = List.ofArray <| Keyboard.GetState().GetPressedKeys()
    List.fold (fun elem key ->  match (beh.TryFind <| key) with
                                | Some func -> func elem
                                | None -> elem) elem KBinput