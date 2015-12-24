module Input

open System
open Microsoft.Xna.Framework.Input

type InputState =
  | KeyboardState of KeyboardState
  | MouseState    of MouseState

type MouseButtons =
  | LeftButton   = 0
  | RightButton  = 1
  | MiddleButton = 2

type KeyCombo = Keys * Option<Keys>
type MouseCombo = MouseButtons * MouseButtons Option

type ButtonStates =
  | KeyboardInput of KeyCombo
  | MouseInput    of MouseCombo

//A conversion from an action(input) to an output(reaction)
type InputBehavior<'a> = Map<ButtonStates, 'a -> 'a>
