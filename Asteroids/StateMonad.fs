(*
  A simple state monad implementation
*)

module StateMonad

type State<'a, 's> = 's -> 'a * 's

let (>>=) p k : State<'b, 's> =
  fun s ->
    let a, s' = p s
    k a s'

type StateBuilder () =
  member this.Return (a : 'a) = fun s -> (a, s)
  member this.ReturnFrom (s : State<'a, 's>) : State<'a, 's> = s
  member this.Bind (p : State<'a, 's>, k : 'a -> State<'b, 's>) = p >>= k

let st = StateBuilder()
