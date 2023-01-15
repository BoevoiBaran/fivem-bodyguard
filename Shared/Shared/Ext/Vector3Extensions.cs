using System;
using CitizenFX.Core;

// ReSharper disable once CheckNamespace
namespace Shared
{
    public static class Vector3Extensions
    {
        public static float DistanceTo(this Vector3 from, Vector3 to)
        {
            var deltaX = from.X - to.X;
            var deltaY = from.Y - to.Y;
            var deltaZ = from.Z - to.Z;

            return (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
        }
    }    
}

