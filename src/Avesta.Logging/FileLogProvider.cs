﻿using System;
using System.IO;
using System.Text;


namespace Avesta.Logging
{
    public class FileLogProvider : LogProvider
    {

        #region Fields

        private string path;

        private Stream Stream = null;

        public static StreamWriter WriterContext;

        #endregion

        #region Constructors
        public FileLogProvider(string path)
        {
            this.path = path;
        }
        #endregion

        #region Destructor
        public override void Dispose()
        {
            base.Dispose();
            WriterContext?.Flush();
            WriterContext?.Dispose();   
            Stream?.Dispose();
            WriterContext = null;
            Stream = null;
        }
        #endregion

        #region Internal Methods
        private void Open()
        {
            lock (this)
            {
                if (!File.Exists(path))
                    File.Open(path, FileMode.Create).Dispose();
                Stream = File.Open(path, FileMode.Open);
                Stream.Seek(0, SeekOrigin.End);
                WriterContext = new StreamWriter(Stream, Encoding.UTF8, 1024, true);
            }
        }
        #endregion


        #region Methods
        public override void Log(object log, LogSeverity severity)
        {
            if (WriterContext == null)
                Open();
            WriterContext?.WriteLine($"({DateTime.UtcNow}) [{severity,-7}]\t{log}");
            WriterContext.Flush();
        }
        #endregion

    }
}

