open System
open Valve.VR

open CompanionWindow
[<EntryPoint>] [<STAThread>]
let main argv = 
    printfn "%A" argv

    let models = RenderContext.RenderModels


    RenderContext.Shutdown()
    0
