using System;

namespace Fractal_Workshop_VR_CS.Renderer
{
    public partial class Engine : IDisposable
    {
        public Engine()
        {
        }



        private bool isDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {

                isDisposed = true;
            }
        }

        ~Engine() { Dispose(false); }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
