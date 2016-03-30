using System;
using System.Collections.Generic;
using System.IO;

namespace ValidateSPHashes
{
    internal interface IHasProgress
    {
        string ProgressText(bool force);
        void UpdateProgress();
        void ClearProgress();
    }

    internal class ProgressStream : Stream, IDisposable, IHasProgress
    {
        private string _lastProg = null;
        private static string fmtProg(long pos, long len)
        {
            if (pos >= len)
                return "100%";
            if (pos == 0)
                return "0%";
            try
            {
                return $"{(pos * 100.0) / len:0.0}%";
            }
            catch { }
            return "";
        }

        public string ProgressText(bool force = false)
        {
            string right = string.Empty;
            IHasProgress inner = _stream as IHasProgress;
            if (inner != null)
                right = ", " + inner.ProgressText(force);

            return ((force || _lastProg == null) ? fmtProg(Position, Length) : _lastProg) + right;
        }

        public void UpdateProgress()
        {
            string currProg = fmtProg(Position, Length);
            if (_lastProg == currProg)
                return;
            if (_lastProg != null)
                Console.Write("{0}{1}{0}", new string('\b', _lastProg.Length), new string(' ', _lastProg.Length));
            var clr = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(currProg);
            Console.ForegroundColor = clr;
            _lastProg = currProg;
         }

        public void ClearProgress()
        {
            if (_lastProg != null)
            {
                Console.Write("{0}{1}{0}", new string('\b', _lastProg.Length), new string(' ', _lastProg.Length));
                _lastProg = null;
            }
        }

        public ProgressStream(Stream stream)
        {
            _stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ClearProgress();
            _stream?.Dispose();
            _stream = null;
        }


        private Stream _stream;

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override long Position
        {
            get { return Program.exit_loops ? _stream.Length : _stream.Position; }
            set { _stream.Position = value; UpdateProgress(); }
        }

        public override void Flush() => _stream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Program.exit_loops)
                return 0;
            var res = _stream.Read(buffer, offset, count);
            UpdateProgress();
            return res;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long res = _stream.Seek(offset, origin);
            UpdateProgress();
            return res;
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
            UpdateProgress();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
            UpdateProgress();
        }
    }
}