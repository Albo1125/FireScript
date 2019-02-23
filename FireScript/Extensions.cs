using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FireScript
{
    internal static class Extensions
    {
        public static void RemovePTFX(this CoordinateParticleEffect fx)
        {
            if (fx != null && Function.Call<bool>(Hash.DOES_PARTICLE_FX_LOOPED_EXIST, fx.Handle))
            {
                Function.Call(Hash.STOP_PARTICLE_FX_LOOPED, fx.Handle, 1);
                Function.Call(Hash.REMOVE_PARTICLE_FX, fx.Handle, 1);
            }
        }
    }
}
