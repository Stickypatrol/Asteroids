(*
  Specifies the entry point
*)

module Program

open Game

[<EntryPoint>]
let main argv =
    use g = new AsteroidsGame ()
    g.Run ()
    0