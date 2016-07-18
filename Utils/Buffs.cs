using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSensei.Utils
{
    class Buffs
    {
        
        /// <summary>
        /// Check to see if we are activly under the effects of the debufs
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static bool HasDebuf(params string[] options)
        {
            return HasDebuf(GameManager.LocalPlayer, options);
        }


        internal static bool HasDebuf(Npc currentTarget, params string[] options)
        {
            return currentTarget.Effects.Where(e => { return options.Contains(e.Name); }).Count() > 0;
        }
    }
}
