using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireScript
{
    internal static class Vector3Extension
    {
        private static Random random = new Random(Environment.TickCount);
        public static Vector3 Around(this Vector3 start, float radius)
        {
            // Random direction.
            Vector3 direction = Vector3Extension.RandomXY();
            Vector3 around = start + (direction * radius);
            return around;
        }

        public static Vector3 Around(this Vector3 start, float MinDistance, float MaxDistance)
        {
            return start.Around(GetRandomFloat(MinDistance, MaxDistance));
        }
        public static float GetRandomFloat(float minimum, float maximum)
        {

            return (float)random.NextDouble() * (maximum - minimum) + minimum;
        }
        public static float DistanceTo(this Vector3 start, Vector3 end)
        {
            return (end - start).Length();
        }

        public static Vector3 RandomXY()
        {            
            Vector3 vector3 = new Vector3();
            vector3.X = (float)(random.NextDouble() - 0.5);
            vector3.Y = (float)(random.NextDouble() - 0.5);
            vector3.Z = 0.0f;
            vector3.Normalize();
            return vector3;
        }
    }
}
