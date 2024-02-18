//////////////////////////////////////////////////////////////////////////////////////////////////
// IMAGE EDITOR
//////////////////////////////////////////////////////////////////////////////////////////////////

using System.Runtime.InteropServices;

namespace ImageEditor.ProgramLogic
{
    /**
     * DllLoader class is responsible for loading dll files and getting function pointers from them.
     * It is used to load ImageProcessorASM.dll and ImageProcessorCpp.dll - dll files with functions written in C++ and ASM.
     */
    public class DllLoader
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        private static extern bool FreeLibrary(IntPtr hModule);

        private readonly IntPtr dllPointer;

        public DllLoader(string dllPath)
        {
            dllPointer = LoadLibrary(dllPath);
        }

        public IntPtr GetFunctionPointer(string function)
        {
            return GetProcAddress(dllPointer, function);
        }

        public void FreeDll()
        {
            FreeLibrary(dllPointer);
        }

        ~DllLoader()
        {
            FreeDll();
        }
    }
}
