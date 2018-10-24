using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fractal_Workshop_VR_CS.Common
{
    public class Environment
    {
        public const bool DxDebug =
#if DEBUG
            true;
#else
            false;
#endif
        public static bool VREnabled = VR.Control.isVRPresent();
    }
}
