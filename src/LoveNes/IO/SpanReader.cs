using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LoveNes.IO
{
    public ref struct SpanReader
    {
        private ReadOnlySpan<byte> _span;

        public bool IsCosumed => _span.IsEmpty;

        public SpanReader ReadAsSubReader(int length)
        {
            var reader = new SpanReader(_span.Slice(0, length));
            Advance(length);
            return reader;
        }

        public SpanReader(ReadOnlySpan<byte> span)
        {
            _span = span;
        }

        public uint ReadAsUnsignedInt()
        {
            // var value = _span.ReadBigEndian<uint>();
            uint value = (uint)_span[0]*256*256*256 + (uint)_span[1]*256*256 + (uint)_span[2]*256 + (uint)_span[3];
            Advance(sizeof(uint));
            return value;
        }

        public byte ReadAsByte()
        {
            // var value = _span.ReadBigEndian<byte>();
            byte value = _span[0];
            Advance(sizeof(byte));
            return value;
        }

        public byte[] ReadAsByteArray(int length)
        {
            var value = ReadBytes(length);
            return value.ToArray();
        }

        public ReadOnlySpan<byte> ReadAsSpan(int length)
        {
            return ReadBytes(length);
        }

        public byte[] ReadAsByteArray()
        {
            var bytes = _span.ToArray();
            _span = ReadOnlySpan<byte>.Empty;
            return bytes;
        }

        private ReadOnlySpan<byte> ReadBytes(int length)
        {
            var bytes = _span.Slice(0, length);
            Advance(length);
            return bytes;
        }

        public void Advance(int count)
        {
            _span = _span.Slice(count);
        }
    }
}
