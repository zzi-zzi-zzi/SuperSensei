using Buddy.BladeAndSoul.Game;
using Buddy.Coroutines;
using log4net;
using System;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSaiyan.Utils
{
    class Combat
    {
        private static ILog Log = LogManager.GetLogger("[Super Saiyan - Combat]");

        /// <summary>
        /// attempts to run a skill
        /// </summary>
        /// <param name="skillName"></param>
        /// <returns></returns>
        internal static async Task<bool> ExecuteSkill(string skillName)
        {
            var skill = GameManager.LocalPlayer.GetSkillByName(skillName);
            if (skill == null)
            {
                return false;
            }

            return await ExecuteSkill(skill);
            
        }

        /// <summary>
        /// handles the actual exxectuion of a skill.
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
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

        /// <summary>
        /// do a cooldown check using the skill.
        /// </summary>
        /// <param name="skill"></param>
        /// <returns></returns>
        internal static bool IsSkillOnCooldown(Skill skill)
        {
            RecycleEntry recycle = skill.GetLocalPlayerRecycleEntry();
            return ((recycle != null) ? new double?(recycle.TimeLeft.TotalMilliseconds) : null) > 0.0;
        }
    }
}
