﻿// TimeoutInformation.cs
// Contains information about a spam timeout.

using System;

namespace FluffyEars.Spam
{
    public struct TimeoutInformation
    {
        /// <summary>When, in unix epoch, the timeout ends.</summary>
        public long TimeoutEndMilliseconds;
        public ulong UserId;

        public override bool Equals(object obj)
        {
            return obj is TimeoutInformation information &&
                   UserId == information.UserId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TimeoutEndMilliseconds, UserId);
        }
    }
}
