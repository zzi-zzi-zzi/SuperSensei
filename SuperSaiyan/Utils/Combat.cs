using Buddy.Auth.Math;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.DataTables;
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

        internal static async Task<bool> ExecuteSkill(string skillName, Keys hotkey)
        {
            var skill = GameManager.LocalPlayer.GetSkillByName(skillName);
            if (skill == null)
            {
                return false;
            }

            return await ExecuteSkill(skill, hotkey);
            
        }

        internal static async Task<bool> ExecuteSkill(Skill skill, Keys hotkey)
        {
            if (skill == null)
                return false;

            var castResult = skill.ActorCanCastResult(GameManager.LocalPlayer);
            Log.DebugFormat("[{0}] {1} CanCast result: {2}", skill.Id, skill.Name, castResult);

            if (castResult > SkillUseError.None)
                return false;

            Log.InfoFormat("Casting {0} on key {1}", skill.Name, hotkey);
            InputManager.PressKey(hotkey);
            await Coroutine.Sleep(100);
            return true;
        }
    }
}
