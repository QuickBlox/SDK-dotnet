using System;

namespace Quickblox.Sdk.Builder
{
    /// <summary>
    /// DateTimeBuilder class.
    /// </summary>
    public static class DateTimeBuilder
    {
        #region Fields

        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddHours(4);

        #endregion

        #region Public Members

        public static Int64 ToUnixEpoch(this DateTime dateTime)
        {
            var timeSpan = dateTime.Subtract(UnixEpoch);
            return (Int64)timeSpan.TotalMilliseconds;
        }

        #endregion
    }
}
