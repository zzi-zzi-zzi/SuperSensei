using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperSensei.Utils.Combat;
using static SuperSensei.Utils.Buffs;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;

namespace SuperSensei.CombatClasses
{
    class Summoner : ICombatHandler
    {
        private static ILog Log = LogManager.GetLogger("[Super Saiyan][Summoner]");

        /// <summary>
        /// summoner rotation.
        /// currently just does Super Sunflower
        /// </summary>
        /// <returns></returns>
        public async Task Combat()
        {
            var SuperSunflower = GameManager.LocalPlayer.GetSkillByName("Super Sunflower");
            //TODO: need to test to see if super sunflower *actually* works since it's the same kind of buff as Dragon Helix (warlock)
            if (HasDebuf("Overflow") || (SuperSunflower != null && SuperSunflower.CanCast()))
            {
                if (await ExecuteSkill("Super Sunflower"))
                    return;
            }

            #region Cat Logic
            if (await ExecuteSkill("Lunge"))
                return;
            
            //TODO distance check
            if (await ExecuteSkill("Crouching Tiger"))
                return;
            #endregion

            //TODO: don't like casting multiple things in a single combat tick. need to find a better way of doing this.
            //TODO: is this really Ivy Poison
            if (!HasDebuf((Npc)GameManager.LocalPlayer.CurrentTarget, "Ivy Poison") &&
                 (await ExecuteSkill("Weed Whack") || await ExecuteSkill("Grasping Roots"))
               )
            {
                await ExecuteSkill("Rosethorn"); //get some chi back
                await ExecuteSkill("Flying Nettles"); //allows us to build up photosyntesis stacks
                return;
            }

            //todo: if we can't cast doom n bloom. have the cat 'graple' the target so we can keep building photo stacks
            if (!HasDebuf((Npc)GameManager.LocalPlayer.CurrentTarget, "Flying Nettles") && 
                !HasDebuf((Npc)GameManager.LocalPlayer.CurrentTarget, "Doom 'n' Bloom") &&
                 await ExecuteSkill("Doom 'n' Bloom"))
            {
                return;
            }
            
            if(GameManager.LocalPlayer.Focus < 5)
            {
                await ExecuteSkill("Rosethorn"); //should always be available
                return;
            }
            if (await ExecuteSkill("Sunflower"))
                return;

            return;
        }
        
    }
}
