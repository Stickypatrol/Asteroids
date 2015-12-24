module Input

open System
open Microsoft.Xna.Framework.Input
open Math

type MouseInput =
  | LeftButton   = 0
  | MiddleButton = 1
  | RightButton  = 2

type InputAction =
  | KBinput of Keys
  | Minput of MouseInput

type InputReaction<'a> = 'a -> 'a

//A conversion from an action(input) to an output(reaction)
type InputBehavior<'a> = Map<InputAction, InputReaction<'a>>

(*we get an inputbehavior type with a certain number of entries that map an action to a reaction
so we need to check all the keypresses that occured and return a list of matching reactions
*)
let checkFunction (behavior: Map<InputAction, InputReaction<'a>>) (obj:'a) key =
  match behavior.TryFind key with
  | Some reaction -> reaction obj
  | None -> obj


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

let ProcessInput (behavior:InputBehavior<'a>) (obj:'a) =
  let KBi = List.ofArray <| Keyboard.GetState().GetPressedKeys()
  let Mi = checkLeftButton()
  let KBi' = List.fold (fun state elem -> (KBinput(elem)::state)) List<InputAction>.Empty KBi
  let Mi' = List.fold (fun state elem -> (Minput(elem)::state)) List<InputAction>.Empty Mi
  let obj' = List.fold (checkFunction behavior) obj KBi'
  List.fold (checkFunction behavior) obj Mi'