using System.Runtime.InteropServices;

namespace Bank.API
{
    [ComVisible(true)]
    public interface IDisposable
    {
        //
        // Summary:
        //     Performs application-defined tasks associated with freeing, releasing, or resetting
        //     unmanaged resources.
        void Dispose();
    }
}
