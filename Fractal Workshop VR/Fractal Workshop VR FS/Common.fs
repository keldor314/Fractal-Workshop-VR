module Common

open SharpDX

let (~~) (item:DisposeBase) = if item <> null then item.Dispose()