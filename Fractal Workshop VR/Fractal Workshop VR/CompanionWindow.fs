module CompanionWindow

open System.Drawing
open System.Windows.Forms
open System.Threading

let private window = ref (null:Form)

let Create width height =
    window := new Form()
    let window = !window
    window.ClientSize <- new Size (new Point (width,height))
    let thread = new Thread(fun () -> Application.Run(window))
    thread.Start()
    thread.Join()