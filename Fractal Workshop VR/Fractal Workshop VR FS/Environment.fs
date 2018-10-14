module Environment

let dxDebug = 
#if DEBUG
    true
#else
    false
#endif