using System.Threading;
using System.Windows.Forms;

namespace SimpleCL.Util.Extension
{
    public static class UiExt
    {
        /// <summary>
        /// Equal to Java's SwingUtilities#invokeLater
        /// </summary>
        /// <param name="control"></param>
        /// <param name="doRun"></param>
        public static void InvokeLater(this Control control, MethodInvoker doRun)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(doRun);
            }
            else
            {
                doRun();
            }
        }
    }
}