﻿module RenderContext

open Valve.VR
open SharpDX
open SharpDX.Direct3D12
open SharpDX.DXGI

open Common

let HMD = 
    let error = ref EVRInitError.None
    let hmd = OpenVR.Init(error, EVRApplicationType.VRApplication_Scene)
    match !error with
    | EVRInitError.None -> hmd
    | error -> failwithf "Error in VR Init: %A" error

let private companionWindowWidth, private companionWindowHeight =
    let width = ref 0u
    let height = ref 0u
    HMD.GetRecommendedRenderTargetSize(width,height)
    CompanionWindow.Create (int !width) (int !height)
    (int !width, int !height)

let RenderModels = 
    let error = ref EVRInitError.None
    let models = OpenVR.GetGenericInterface (OpenVR.IVRRenderModels_Version, error)
    match !error with
    | EVRInitError.None -> models
    | error -> failwithf "Error getting render models: %A" error

let private factory = 
    if Environment.dxDebug then
        DebugInterface.Get().EnableDebugLayer()
    new DXGI.Factory2(Environment.dxDebug)

let DebugLayer = DXGIDebug1.TryCreate()

///Devices.[0] will always contain the device the headset is plugged into.
let Devices =
    let hmdAdapterIndex = ref 0
    HMD.GetDXGIOutputInfo(hmdAdapterIndex)
    let adapters = factory.Adapters1
    let devices = 
        adapters
        |> Array.mapi (fun i adapter     ->  (adapter,i))
        |> Array.filter (fun (adapter,i) ->  adapter.Description.Description <> "Microsoft Basic Render Driver")
        |> Array.partition (fun (_,i)    ->  i = !hmdAdapterIndex)
        |> (fun (primary,secondary)      ->  Array.concat [primary; secondary])
        |> Array.map (fun (adapter,i)    ->  new Direct3D12.Device(adapter, Direct3D.FeatureLevel.Level_12_0))
    adapters |> Array.iter (fun adapter -> ~~adapter)
    devices

let GraphicsQueue =
    let mutable queueDesc = new CommandQueueDescription(CommandListType.Direct)
    queueDesc.Priority <- int CommandQueuePriority.High
    Devices.[0].CreateCommandQueue queueDesc

let ComputeQueues =
    Devices
    |> Array.map (fun device ->
        let mutable queueDesc = new CommandQueueDescription(CommandListType.Compute)
        queueDesc.Priority <- int CommandQueuePriority.Normal
        device.CreateCommandQueue queueDesc )

let CopyQueues =
    Devices
    |> Array.map (fun device ->
        let mutable queueDesc = new CommandQueueDescription(CommandListType.Copy)
        queueDesc.Priority <- int CommandQueuePriority.High
        device.CreateCommandQueue queueDesc )

let SwapChain =
    let desc = 
        new SwapChainDescription (
            ModeDescription = 
                new ModeDescription(
                    Width = int companionWindowWidth,
                    Height = int companionWindowHeight,
                    Format = Format.R16G16B16A16_Float),
                SampleDescription = new SampleDescription(
                    Count = 1,
                    Quality = 0),
                Usage = Usage.BackBuffer,
                BufferCount = 2,
                SwapEffect = SwapEffect.FlipDiscard,
                OutputHandle = CompanionWindow.GetHandle(),
                IsWindowed = Mathematics.Interop.RawBool true )
    new SwapChain (factory, GraphicsQueue, desc)



let Shutdown () =
    for queue in CopyQueues do ~~queue
    for queue in ComputeQueues do ~~queue
    ~~GraphicsQueue
    for device in Devices do ~~device
    ~~factory
    ~~SwapChain
    OpenVR.Shutdown()
    if DebugLayer <> null then DebugLayer.ReportLiveObjects(DebugId.All, DebugRloFlags.Summary ||| DebugRloFlags.IgnoreInternal)
    