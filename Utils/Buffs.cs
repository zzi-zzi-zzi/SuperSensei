using Buddy.BladeAndSoul.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSaiyan.Utils
{
    class Buffs
    {
        /// <summary>
        /// Check to see if we are activly under the effects of the debufs
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal static bool HasDebuf(List<string> options)
        {
            return GameManager.LocalPlayer.Effects.Where(e => { return options.Contains(e.Name); }).Count() > 0;
        }
    }
}
