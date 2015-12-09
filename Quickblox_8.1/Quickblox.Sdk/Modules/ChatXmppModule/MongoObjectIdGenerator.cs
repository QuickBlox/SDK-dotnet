using System;
using System.Text;
using System.Threading;
using Quickblox.Sdk.Builder;

namespace Quickblox.Sdk.Modules.ChatXmppModule
{
    public static class MongoObjectIdGenerator
    {
        #region Fields

        private static readonly int _machine;
        private static readonly short _pid;
        private static int _increment;

        #endregion

        #region Static ctor

        static MongoObjectIdGenerator()
        {
            var random = new Random();
            _machine = random.Next(0, 16777215);
            _pid = (short) random.Next(0, 32768);
            _increment = random.Next(0, 32768);
        }

        #endregion

        #region Public methods

        public static MongoObjectId GenerateNewObjectId()
        {
            var timestamp = GetTimestampFromDateTime(DateTime.UtcNow);
            Interlocked.Increment(ref _increment);
            return new MongoObjectId(_machine, _pid, _increment, timestamp);
        }

        public static string GetNewObjectIdString()
        {
            return GenerateNewObjectId().ToHexString();
        }

        #endregion

        #region Private methods

        private static int GetTimestampFromDateTime(DateTime timestamp)
        {
            var secondsSinceEpoch = timestamp.ToUnixEpoch();
            if (secondsSinceEpoch < int.MinValue || secondsSinceEpoch > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException("timestamp");
            }
            return (int) secondsSinceEpoch;
        }

        #endregion

    }

    public struct MongoObjectId
    {
        #region Fields

        private readonly int _machine;
        private readonly short _pid;
        private readonly int _increment;
        private readonly int _timestamp;

        #endregion

        #region Ctor

        public MongoObjectId(int machine, short pid, int increment, int timestamp)
        {
            _machine = machine;
            _pid = pid;
            _increment = increment;
            _timestamp = timestamp;
        }

        #endregion

        #region Public methods

        public string ToHexString()
        {
            var bytes = ToByteArray();

            var sb = new StringBuilder(bytes.Length*2);
            foreach (var b in bytes)
            {
                sb.AppendFormat("{0:x2}", b);
            }
            return sb.ToString();
        }

        public byte[] ToByteArray()
        {
            if ((_machine & 0xff000000) != 0)
            {
                throw new Exception("The machine value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }
            if ((_increment & 0xff000000) != 0)
            {
                throw new Exception("The increment value must be between 0 and 16777215 (it must fit in 3 bytes).");
            }

            byte[] bytes = new byte[12];
            bytes[0] = (byte) (_timestamp >> 24);
            bytes[1] = (byte) (_timestamp >> 16);
            bytes[2] = (byte) (_timestamp >> 8);
            bytes[3] = (byte) (_timestamp);
            bytes[4] = (byte) (_machine >> 16);
            bytes[5] = (byte) (_machine >> 8);
            bytes[6] = (byte) (_machine);
            bytes[7] = (byte) (_pid >> 8);
            bytes[8] = (byte) (_pid);
            bytes[9] = (byte) (_increment >> 16);
            bytes[10] = (byte) (_increment >> 8);
            bytes[11] = (byte) (_increment);
            return bytes;
        }

        #endregion

    }
}
