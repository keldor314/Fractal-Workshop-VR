using System;
using Valve.VR;

namespace Fractal_Workshop_VR_CS.VR
{
    public class Control
    {
        public static bool isVRPresent()
        {
            return OpenVR.IsHmdPresent();
        }

        public Control()
        {
            var error = EVRInitError.None;
            OpenVR.Init(ref error,EVRApplicationType.VRApplication_Other);
            if (error != EVRInitError.None) throw new Exception(string.Format("Error in OpenVR init: {0}", error));
        }

        public int GetDXGIOutputDevice()
        {
            var index = 0;
            OpenVR.System.GetDXGIOutputInfo(ref index);
            return index;
        }

        ~Control()
        {
            if (OpenVR.System != null) OpenVR.Shutdown();
        }
    }
}
