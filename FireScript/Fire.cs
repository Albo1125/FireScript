using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireScript
{
    internal class Fire : BaseScript
    {

        public int ManageFireTimer = 0;
        public Vector3 Position = Vector3.Zero;

        public bool Explosion = false;
        public int MaxFlames = 20;
        public float MaxSpreadDistance = 15;
        public List<Flame> Flames = new List<Flame>();
        public CoordinateParticleEffect SmokePTFX;
        public CoordinateParticleEffect InteriorSmokePTFX;
        public bool Active { get; private set; }

        public Fire() { }

        public Fire(Vector3 Position, int MaxFlames, bool Explosion, int MaxSpreadDistance, bool explosion)
        {
            this.Position = Position;
            this.MaxFlames = MaxFlames;
            this.MaxSpreadDistance = MaxSpreadDistance;
            this.Explosion = explosion;
        }

        public async Task Start()
        {
            Active = true;
            for (int i = 0; i < MaxFlames; i++)
            {
                Flames.Add(new Flame(Position.Around(0, MaxSpreadDistance)));
            }
            ParticleEffectsAsset asset = new ParticleEffectsAsset("scr_agencyheistb");
            await asset.Request(1000);
            SmokePTFX = asset.CreateEffectAtCoord("scr_env_agency3b_smoke", Position, scale:MaxSpreadDistance*0.5f, startNow:true);
            //SmokePTFX = Function.Call<uint>(Hash.START_PARTICLE_FX_LOOPED_AT_COORD, "scr_env_agency3b_smoke", Position.X, Position.Y, Position.Z + 0.87, 0, 0, 0, 5, 0, 0, 0, 0);
            //Function.Call(Hash.SET_PARTICLE_FX_LOOPED_ALPHA, SmokePTFX, 1);
            if (this.Explosion)
            {
                World.AddExplosion(Position, ExplosionType.Propane, 1, 1, aubidble: true);
                ParticleEffectsAsset expl_asset = new ParticleEffectsAsset("scr_trevor3");
                await expl_asset.Request(1000);
                Vector3 pos = Position;
                pos.Z += 1;
                expl_asset.StartNonLoopedAtCoord("scr_trev3_trailer_expolsion", pos);
            }
            if (API.GetInteriorAtCoords(Position.X, Position.Y, Position.Z) == 0)
            {
                InteriorSmokePTFX = asset.CreateEffectAtCoord("scr_env_agency3b_smoke", Position, scale: 5, startNow: true);
            }            
            //FireScript.WriteDebug("Started fire");
        }

        public void Remove(bool TriggerFireOutEvent)
        {

            foreach (Flame f in this.Flames)
            {
                f.Remove();
            }
            this.Flames.Clear();
            SmokePTFX.RemovePTFX();
            InteriorSmokePTFX.RemovePTFX();
            if (TriggerFireOutEvent)
            {
                TriggerServerEvent("FireScript:FirePutOut", Position.X, Position.Y, Position.Z);
            }
            Active = false;

            //FireScript.WriteDebug("Removed fire!");
        }

        public void Manage()
        {
            if (Active)
            {
                //int NumberOfFlames = Function.Call<int>(Hash.GET_NUMBER_OF_FIRES_IN_RANGE, Position.X, Position.Y, Position.Z, 50);
                //Screen.ShowNotification("Managing");
                foreach (Flame f in this.Flames)
                {
                    f.Manage();
                }
                foreach (Flame f in this.Flames.ToArray())
                {
                    if (!f.Active)
                    {
                        this.Flames.Remove(f);
                        //Debug.WriteLine("Removed an inactive flame");
                    }
                }
                if (this.Flames.Count < 8)
                {
                    this.Remove(true);
                    //Debug.WriteLine("Removed fire completely - flame count under 8! " + this.Flames.Count);
                }

            }
        }                    
    }
}
