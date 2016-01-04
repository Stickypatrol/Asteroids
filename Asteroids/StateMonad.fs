(*
  A simple state monad implementation
*)

module CstateMonad

type Cstate<'a, 's> = 's -> CstateStep<'a, 's>
and CstateStep<'a, 's> =
  | Done of 'a* 's
  | Yield of Cstate<'a, 's>*'s

let rec (>>=) (p:Cstate<'a, 's>) (k:'a -> Cstate<'b, 's>) : Cstate<'b, 's> =
  fun s ->
    match p s with
    | Done(a, s) -> k a s
    | Yield(p', s') -> Yield((>>=) p' k, s')

let ret a = fun s -> Done(a, s)

let rec unpack (c : Cstate<'a, 's>) =
  fun s ->
    match c s with
    | Done (a, s) -> s
    | Yield (c', s) -> unpack c' s

type CstateBuilder () =
  member this.Return (a : 'a) = ret a
  member this.ReturnFrom (c : Cstate<'a, 's>) : Cstate<'a, 's> = c
  member this.Bind (p : Cstate<'a, 's>, k : 'a -> Cstate<'b, 's>) = p >>= k
  member this.Run (c : Cstate<'a, 's>) = unpack c
  member this.Separate (sep : 's -> 's1* 's2* 's3) = fun s -> sep s
  member this.Join (join : 's1* 's2* 's3 -> 's) = fun 's1, 's2, 's3 -> join xs
let cs = CstateBuilder()
