﻿namespace SentryExample.Core.Helpers
{
    public static class RandomHelper
    {
        public static int RandomLong()
        {
            Random random = new Random();
            byte[] bytes = new byte[4];
            random.NextBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
