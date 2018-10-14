module CompanionWindow

open System
open System.Drawing
open System.Windows.Forms
open System.Threading


let mutable companionWindow = null:Form

type private getHandleDelegate = delegate of unit -> IntPtr
let GetHandle () = 
    (companionWindow.Invoke <| new getHandleDelegate(fun () -> companionWindow.Handle)) :?> IntPtr

type private closeWindowDelegate = delegate of unit -> unit
let SignalClose () =
    companionWindow.Invoke <| new closeWindowDelegate (fun () -> companionWindow.Close ()) |> ignore

let Create width height =
    let initWaitHandle = new EventWaitHandle (false, EventResetMode.AutoReset)
    let windowThread = new Thread(fun () ->         
        companionWindow <- new Form ()
        companionWindow.Text <- "Fractal Workshop VR"
        companionWindow.ClientSize <- new Size (new Point (width,height))
        companionWindow.Shown.AddHandler <| new EventHandler (fun _ _ -> initWaitHandle.Set() |> ignore)
        companionWindow.FormClosed.AddHandler <| new FormClosedEventHandler (fun _ _ -> ())
        companionWindow.Show ()
        Application.Run companionWindow)
    windowThread.Start()
    initWaitHandle.WaitOne () |> ignore