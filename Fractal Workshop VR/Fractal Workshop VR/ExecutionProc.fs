module ExecutionProc

open Valve.VR

let HMD = 
    let error = ref EVRInitError.None
    let hmd = OpenVR.Init(error)
    match !error with
    | EVRInitError.None -> hmd
    | error -> failwithf "Error in VR Init: %A" error