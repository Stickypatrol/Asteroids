(*
    Basic media loading functionality
*)

module Media

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Content
open Microsoft.Xna.Framework.Graphics

//Load a single texture
let loadTexture (content:ContentManager) (textureName:string) =
  if not (System.String.IsNullOrEmpty textureName) then
    Some (content.Load textureName)
  else
    None
