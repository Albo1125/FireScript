using CitizenFX.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireScript
{
    public class FireScript : BaseScript
    {
        private const int ManageFireTimeout = 50;
        private List<Fire> ActiveFires = new List<Fire>();
        private List<Tuple<CoordinateParticleEffect, Vector3>> SmokeWithoutFire = new List<Tuple<CoordinateParticleEffect, Vector3>>();
        public FireScript()
        {
            TriggerEvent("chat:addSuggestion", "/startfire", "Starts a fire at your location", new[]
            {
                new { name = "NumFlames", help = "The number of flames (up to 100 depending on size)" },
                new { name="Radius", help="The radius in metres (up to 30 depending on size)" },
                new { name="Explosion", help="True to create an explosion, false to not create an explosion." },
            });
            TriggerEvent("chat:addSuggestion", "/startsmoke", "Starts smoke without fire at your location", new[]
            {
                new { name = "Scale", help = "Magnitude of the smoke (recommended 0.5-5.0)" }
            });
            EventHandlers["FireScript:StartFireAtPlayer"] += new Action<int,int, int, bool>((int source, int maxFlames, int maxRange, bool explosion) =>
            {               
                startFire(source, maxFlames, maxRange, explosion);
            });
            EventHandlers["FireScript:StopFiresAtPlayer"] += new Action<int>((int source) =>
            {
                stopFires(true, Players[source].Character.Position);
            });
            EventHandlers["FireScript:StopAllFires"] += new Action<dynamic>((dynamic res) =>
            {
                stopFires(false, Vector3.Zero);
            });
            EventHandlers["FireScript:StopFireAtPosition"] += new Action<float, float, float>((float x, float y, float z) =>
            {
                stopFires(true, new Vector3(x, y, z), 3);
                //Screen.ShowNotification("Fire put out at " + x + y + z);
            });

            EventHandlers["FireScript:StartSmokeAtPlayer"] += new Action<int, float>((int source, float scale) =>
            {
                startSmoke(Players[source].Character.Position, scale);
            });
            EventHandlers["FireScript:StopSmokeAtPlayer"] += new Action<int>((int source) =>
            {
                stopSmoke(true, Players[source].Character.Position);
            });

            EventHandlers["FireScript:StopAllSmoke"] += new Action<dynamic>((dynamic res) =>
            {
                stopSmoke(false, Vector3.Zero);
            });
            Main();
        }

        private async void Main()
        {
            DateTime timekeeper = DateTime.Now;
            while (true)
            {
                await Delay(10);
                if ((System.DateTime.Now - timekeeper).TotalMilliseconds > ManageFireTimeout)
                {
                    timekeeper = DateTime.Now;
                    foreach (Fire f in ActiveFires.ToArray())
                    {
                        if (f.Active)
                        {
                            f.Manage();
                        }
                        else
                        {
                            ActiveFires.Remove(f);
                        }
                    }
                }
            }
        }
        private void stopFires(bool onlyNearbyFires, Vector3 pos, float distance = 35)
        {
            foreach (Fire f in ActiveFires.ToArray())
            {
                if (!onlyNearbyFires || Vector3.Distance(f.Position, pos) < distance)
                {
                    f.Remove(false);
                    ActiveFires.Remove(f);
                }
            }
        }

        private void startFire(int source, int maxFlames, int maxRange, bool explosion)
        {
            Vector3 Pos = Players[source].Character.Position;
            Pos.Z -= 0.87f;
            if (maxRange > 30) { maxRange = 30; }
            if (maxFlames > 100) { maxRange = 100; }
            Fire f = new Fire(Pos, maxFlames, false, maxRange, explosion);
            ActiveFires.Add(f);
            f.Start();
        }

        private async Task startSmoke(Vector3 pos, float scale)
        {
            ParticleEffectsAsset asset = new ParticleEffectsAsset("scr_agencyheistb");
            await asset.Request(1000);
            SmokeWithoutFire.Add(Tuple.Create(asset.CreateEffectAtCoord("scr_env_agency3b_smoke", pos, scale: scale, startNow: true), pos));
        }

        private void stopSmoke(bool allSmoke, Vector3 position)
        {
            foreach (Tuple<CoordinateParticleEffect, Vector3> f in SmokeWithoutFire.ToArray())
            {
                if (!allSmoke || Vector3.Distance(f.Item2, position) < 30f)
                {
                    f.Item1.RemovePTFX();
                    SmokeWithoutFire.Remove(f);
                }
            }
        }
    }
}
