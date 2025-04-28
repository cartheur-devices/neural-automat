using System;
using System.Text;
using System.IO;

namespace Automat
{
    /// <summary>
    /// The class providing the logging function.
    /// </summary>
    internal class SimpleLog
    {
        /// <summary>
        /// Stream writer
        /// </summary>
        private readonly StreamWriter _mStreamWriter;
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleLog"/> class.
        /// </summary>
        /// <param name="sFilePath">The file path of the log file.</param>
        public SimpleLog(string sFilePath)
        {
            _mStreamWriter = new StreamWriter(sFilePath, false, Encoding.ASCII) { AutoFlush = true };
        }
        /// <summary>
        /// Writes the log message to a file.
        /// </summary>
        /// <param name="sLogMessage">The log message.</param>
        /// <param name="args">The args.</param>
        public void Log(string sLogMessage, params object[] args)
        {
            var time = DateTime.Now;
            Console.WriteLine(sLogMessage, args);
            if (_mStreamWriter != null)
                _mStreamWriter.WriteLine(time + " | " + sLogMessage, args);
        }
    }
}
