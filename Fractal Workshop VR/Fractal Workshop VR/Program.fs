open System
open Valve.VR

[<EntryPoint>] [<STAThread>]
let main argv = 
    printfn "%A" argv

    let width, height = 
        let width = ref 0u
        let height = ref 0u
        ExecutionProc.HMD.GetRecommendedRenderTargetSize(width,height)
        (int !width,int !height)

    CompanionWindow.Create width height


    OpenVR.Shutdown()
    0
