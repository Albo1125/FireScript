using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace FireScript
{
    internal static class Extensions
    {
        public static void RemovePTFX(this CoordinateParticleEffect fx)
        {
            if (fx != null && API.DoesParticleFxLoopedExist(fx.Handle))
            {
                API.StopParticleFxLooped(fx.Handle, true);
                API.RemoveParticleFx(fx.Handle, true);
            }
        }
    }
}
