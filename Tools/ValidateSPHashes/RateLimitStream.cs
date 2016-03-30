using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateSPHashes
{
    public class RateLimitStream : Stream, IDisposable
    {
        public RateLimitStream(Stream stream)
        {
            _stream = stream;
        }

        ~RateLimitStream()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            var sd = _stream as IDisposable;
            _stream = null;
            if (sd != null)
                sd.Dispose();
        }

        private Stream _stream;

        public override bool CanRead => _stream.CanRead;
        public override bool CanSeek => _stream.CanSeek;
        public override bool CanWrite => _stream.CanWrite;
        public override long Length => _stream.Length;
        public override void Flush() => _stream.Flush();
        public override void SetLength(long value) => _stream.SetLength(value);

        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = Position; ; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #region Rate limiting
        private long _rateBytesPerSecond = 1000000;

        private class ratemeasure
        {
            public DateTime time;
            public long position;
        }
        private ratemeasure[] _rateHistory = new ratemeasure[64];
        private int _rateIndex = 0;

        private void ResetRateMeasures()
        {
            for (int i = 0; i < _rateHistory.Length; i++)
                _rateHistory[i] = null;
            _rateHistory[0] = new ratemeasure { time = DateTime.Now, position = Position };
            _rateIndex = 0;
        }

        /// <summary>
        /// get current measured rate in bytes per second
        /// </summary>
        /// <param name="nSamples">Optional maximum number of history samples to count, in the range 2..64</param>
        /// <returns>0 if less than two read points have been recorded, else total bytes over total time for recorded reads</returns>
        public float CurrentRate(int nSamples = 64)
        {
            // enforce nSamples range
            nSamples = Math.Max(2, Math.Min(nSamples, 64));
            var datapoints = Enumerable.Range(0, nSamples).Select(i => _rateHistory[(_rateIndex - i) & 0x3F]).TakeWhile(v => v != null).ToArray();
            if (datapoints.Length < 1)
                return 0;
            var left = datapoints.First();
            var right = datapoints.Length > 1 ? datapoints.Last() : new ratemeasure { time = DateTime.Now, position = Position };

            TimeSpan time = right.time - left.time;
            long bytes = right.position - left.position;

            return Math.Max(0, (float)(bytes / time.TotalSeconds));
        }
        #endregion
    }
}
