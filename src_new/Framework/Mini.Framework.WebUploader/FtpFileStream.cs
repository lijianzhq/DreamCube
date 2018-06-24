using System;
using System.IO;

using FluentFTP;

namespace Mini.Framework.WebUploader
{
    public class FtpFileStream : Stream, IDisposable
    {
        private Stream _stream = null;
        private FtpClient _ftpClient = null;

        public FtpFileStream(FtpClient client, Stream stream)
        {
            this._stream = stream;
            this._ftpClient = client;
        }

        public override bool CanRead => _stream.CanRead;

        public override bool CanSeek => _stream.CanSeek;

        public override bool CanWrite => _stream.CanWrite;

        public override long Length => _stream.Length;

        public override long Position { get => _stream.Position; set => _stream.Position = value; }

        public override void Flush()
        {
            _stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            _stream.Close();
            _ftpClient.Dispose();
        }
    }
}
