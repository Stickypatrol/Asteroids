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

let Second c =
  fun s ->
    match c s with
    | Done(a,s) -> s
    | Yield(a,s) -> failwith "something went wrong"

type CstateBuilder () =
  member this.Return (a : 'a) = ret a
  member this.ReturnFrom (c : Cstate<'a, 's>) : Cstate<'a, 's> = c
  member this.Snd c = Second c
  member this.Zero () = fun s -> Done((), s)
  member this.Bind (p : Cstate<'a, 's>, k : 'a -> Cstate<'b, 's>) = p >>= k
let cs = CstateBuilder()