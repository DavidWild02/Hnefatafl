using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hnefatafl
{
    // https://stackoverflow.com/questions/57615/how-to-add-a-timeout-to-console-readline/18342182#18342182
    // I was digging for copper... and I found gold!
    // 
    class HiddenReader
    {
        private static Thread inputThread;
        private static AutoResetEvent askForInput, deliverInput;
        private static ConsoleKeyInfo input;

        static HiddenReader()
        {
            askForInput = new AutoResetEvent(false);
            deliverInput = new AutoResetEvent(false);
            inputThread = new Thread(reader);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        private static void reader()
        {
            while (true)
            {
                askForInput.WaitOne();
                input = Console.ReadKey(true);
                deliverInput.Set();
            }
        }

        public static ConsoleKeyInfo readKey()
        {
            askForInput.Set();
            deliverInput.WaitOne();
            return input;
        }
    }
}
