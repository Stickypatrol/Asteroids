(*
  Basic math functionality
*)

module Math

open Microsoft.Xna.Framework

[<Measure>]
type p //Spatial(positional) data

[<Measure>]
type s //Time data, usually the time delta

type Vector2<[<Measure>] 'a> =
  {
    X : float<'a>
    Y : float<'a>
  }
  with
  static member Zero : Vector2<'a> =
      {X = 0.0<_>; Y = 0.0<_>}
  static member (+)
      (v1 : Vector2<'a>, v2 : Vector2<'a>) : Vector2<'a> =
      {X = v1.X + v2.X; Y = v1.Y + v2.Y}
  static member (+)
      (v : Vector2<'a>, k:float<'a>) : Vector2<'a> =
      {X = v.X + k; Y = v.Y + k}
  static member (+)
      (k : float<'a>, v:Vector2<'a>) : Vector2<'a> = v + k
  static member (~-)
      (v : Vector2<'a>) : Vector2<'a> =
      {X = -v.X; Y = -v.Y}
  static member (-)
      (v1 : Vector2<'a>, v2 : Vector2<'a>) : Vector2<'a> =
      v1 + (-v2)
  static member (-)
      (v : Vector2<'a>, k : float<'a>) : Vector2<'a> = v + (-k)
  static member (-)
      (k : float<'a>, v : Vector2<'a>) : Vector2<'a> = k + (-v)
  static member (*)
      (v1 : Vector2<'a>, v2 : Vector2<'b>) : Vector2<'a * 'b> =
      {X = v1.X * v2.X; Y = v1.Y * v2.Y}
  static member (*)
      (v : Vector2<'a>, f : float<'b>) : Vector2<'a * 'b> =
      {X = v.X * f; Y = v.Y * f}
  static member (*)
      (f:float<'b>, v:Vector2<'a>) : Vector2<'b * 'a> =
      {X = f * v.X; Y = f * v.Y}
  static member (/)
      (v : Vector2<'a>, f : float<'b>) : Vector2<'a / 'b> =
      v * (1.0 / f)
  member this.Length : float<'a> =
      sqrt((this.X * this.X + this.Y * this.Y))
  static member Distance(v1 : Vector2<'a>, v2 : Vector2<'a>) =
      (v1 - v2).Length
  static member Normalize(v : Vector2<'a>) : Vector2<1> =
      v / v.Length
  member this.ToXNA : Vector2 =
      Vector2(this.X |> float32, this.Y |> float32)

let toGameTime ms =
  ms |> float |> (/) 1000.0<s>

let Limit op (l:'a) =
  fun (x:'a) ->
    match x with
    | x when op x l -> l
    | _ -> x

let LimitAbsint op (l:int) =
  fun (x:int) ->
    match x with
    | x when op x l -> l
    | x when op -x l -> -l
    | _ -> x

let LimitAbs op (l:float) =
  fun (x:float) ->
    match x with
    | x when op x l -> l
    | x when op -x l -> -l
    | _ -> x

let LimitVecAbs op (l:Vector2<'a>) =
  fun (z:Vector2<'a>) ->
    match z with
    | z when op z.X l.X && op z.Y l.Y -> {X = l.X; Y = l.Y}
    | z when op z.X l.X && not (op z.Y -l.Y) -> {X = l.X; Y = -l.Y}
    | z when not (op z.X -l.X) && not (op z.Y -l.Y) -> {X = -l.X; Y = -l.Y}
    | z when not (op z.X -l.X) && op z.Y l.Y -> {X = -l.X; Y = l.Y}
    | z when op z.X l.X -> {X = l.X; Y = z.Y}
    | z when op z.Y l.Y -> {X = z.X; Y = l.Y}
    | z when not (op z.X -l.X) -> {X = -l.X; Y = z.Y}
    | z when not (op z.Y -l.Y) -> {X = z.X; Y = -l.Y}
    | _ -> z

let Middelize (func:Vector2<'a> -> Vector2<'a>) (offset:Vector2<'a>) (vec:Vector2<'a>) =
  (func (vec - offset)) + offset

type Rectangle<[<Measure>] 'a > =
  {
    Height : float<'a>
    Width  : float<'a>
    X      : float<'a>
    Y      : float<'a>
  }
  with
  member this.ToXNA : Rectangle =
    Rectangle(this.Height |> int, this.Width |> int, this.X |> int, this.Y |> int)