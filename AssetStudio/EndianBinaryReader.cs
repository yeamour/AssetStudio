using System;
using System.Buffers.Binary;
using System.IO;

namespace AssetStudio
{
    public class EndianBinaryReader : BinaryReader
    {
        private readonly byte[] buffer;

        public EndianType Endian;

        private bool isRecord = false;
        private long recordIndex;
        private long recordSize;

        public EndianBinaryReader(Stream stream, EndianType endian = EndianType.BigEndian) : base(stream)
        {
            Endian = endian;
            buffer = new byte[8];
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public void BeginRecord()
        {
            isRecord = true;
            recordIndex = Position;
            recordSize = 0;
        }
        public void EndRecord()
        {
            isRecord = false;
        }
        public bool IsRecord()
        {
            return isRecord;
        }
        public void AddRecord(int size)
        {
            if (!isRecord) return;

            recordSize += size;
        }
        public byte[] GetRecord()
        {
            long oldPosition = Position;
            Position = recordIndex;
            byte[] result = ReadBytes((int)recordSize);
            Position = oldPosition;
            return result;
        }

        public override bool ReadBoolean()
        {
            AddRecord(1);
            return base.ReadBoolean();
        }

        public override byte[] ReadBytes(int count)
        {
            AddRecord(count);
            return base.ReadBytes(count);
        }

        public override short ReadInt16()
        {
            AddRecord(2);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 2);
                return BinaryPrimitives.ReadInt16BigEndian(buffer);
            }
            return base.ReadInt16();
        }

        public override int ReadInt32()
        {
            AddRecord(4);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 4);
                return BinaryPrimitives.ReadInt32BigEndian(buffer);
            }
            return base.ReadInt32();
        }

        public override long ReadInt64()
        {
            AddRecord(8);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 8);
                return BinaryPrimitives.ReadInt64BigEndian(buffer);
            }
            return base.ReadInt64();
        }

        public override ushort ReadUInt16()
        {
            AddRecord(2);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 2);
                return BinaryPrimitives.ReadUInt16BigEndian(buffer);
            }
            return base.ReadUInt16();
        }

        public override uint ReadUInt32()
        {
            AddRecord(4);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 4);
                return BinaryPrimitives.ReadUInt32BigEndian(buffer);
            }
            return base.ReadUInt32();
        }

        public override ulong ReadUInt64()
        {
            AddRecord(8);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 8);
                return BinaryPrimitives.ReadUInt64BigEndian(buffer);
            }
            return base.ReadUInt64();
        }

        public override float ReadSingle()
        {
            AddRecord(4);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 4);
                Array.Reverse(buffer, 0, 4);
                return BitConverter.ToSingle(buffer, 0);
            }
            return base.ReadSingle();
        }

        public override double ReadDouble()
        {
            AddRecord(8);
            if (Endian == EndianType.BigEndian)
            {
                Read(buffer, 0, 8);
                Array.Reverse(buffer);
                return BitConverter.ToDouble(buffer, 0);
            }
            return base.ReadDouble();
        }
    }
}
