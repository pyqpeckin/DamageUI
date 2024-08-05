using DamageUI.Models;
using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamageUI
{
    public class Config : IRocketPluginConfiguration
    {
        public ushort EffectID = 30000;
        public TColor color = new TColor()
        {
            Structure = "<color=yellow>{0}",
            Animal = "<color=yellow>{0}",
            Barricade = "<color=yellow>{0}",
            Resource = "<color=yellow>{0}",
            Zombie = "<color=yellow>{0}",
            Player_Body = "<color=white>{0}",
            Player_Skulll = "<color=red>{0}"
        };
        public void LoadDefaults()
        {
        }
    }
}
