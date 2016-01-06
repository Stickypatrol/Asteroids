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

type CstateBuilder () =
  member this.Return (a : 'a) = ret a
  member this.ReturnFrom (c : Cstate<'a, 's>) : Cstate<'a, 's> = c
  member this.Bind (p : Cstate<'a, 's>, k : 'a -> Cstate<'b, 's>) = p >>= k
  member this.For (c : 'a -> Cstate<Unit, 's>, sl : seq<'a>) =
    if (Seq.isEmpty sl) then
      this.Return ()
    else
      (c (Seq.head sl)) >>= fun () -> this.For(c, (Seq.tail sl))
let cs = CstateBuilder()
