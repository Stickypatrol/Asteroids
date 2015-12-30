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
  let pressedKeys = Keyboard.GetState().GetPressedKeys() |> List.ofArray
  List.fold (fun elem key ->  match (beh.TryFind <| key) with
                              | Some func -> func elem
                              | None -> elem) elem pressedKeys