using Buddy.Auth.Math;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.DataTables;
using Buddy.Coroutines;
using log4net;
using System;
using System.Threading.Tasks;

namespace SuperSaiyan.Utils
{
    class Combat
    {
        private static ILog Log = LogManager.GetLogger("[Super Saiyan - Combat]");

        internal static async Task<bool> ExecuteSkill(string skillName)
        {
            var skill = GameManager.LocalPlayer.GetSkillByName(skillName);
            if (skill == null)
            {
                return false;
            }

            return await ExecuteSkill(skill);
            
        }

        internal static async Task<bool> ExecuteSkill(Skill skill)
        {
            if (skill == null)
                return false;

            var castResult = skill.ActorCanCastResult(GameManager.LocalPlayer);
            Log.DebugFormat("[{0}] {1} CanCast result: {2}", skill.Id, skill.Name, castResult);

            if (castResult > SkillUseError.None)
                return false;

            Log.InfoFormat("Casting {0}", skill.Name);
            skill.Cast();
            await Coroutine.Sleep(100);
            return true;
        }
    }
}
