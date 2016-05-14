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
        /// attempt to push and hold a button for a set amount of time.
        /// </summary>
        /// <param name="skillName"></param>
        /// <param name="timeSpan"></param>
        internal static async Task<bool> ExecuteSkill(string skillName, TimeSpan duration)
        {
            var skill = GameManager.LocalPlayer.GetSkillByName(skillName);
            if (skill == null)
            {
                return false;
            }

            var castResult = skill.ActorCanCastResult(GameManager.LocalPlayer);
            Log.DebugFormat("[{0}] {1} CanCast result: {2}", skill.Id, skill.Name, castResult);

            if (castResult > SkillUseError.None)
                return false;

            Log.InfoFormat("Casting {0}", skill.Name);

            GameOptions.GameOption gameOption;
            if (GameOptions.ControlMode.Int32 == 1)
            {
                gameOption = GameOptions.FindOptionByShortcut(skill.ShortcutKeyClassic, GameOptions.GameOptionType.KeybindData);
            }
            else
            {
                gameOption = GameOptions.FindOptionByShortcut(skill.ShortcutKey, GameOptions.GameOptionType.KeybindData);
            }
            await PressKeybind(gameOption.Keybind, duration);
            await Coroutine.Sleep(100);
            return true;
        }

        /// <summary>
        /// push the button for a duration.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="duration"></param>
        private async static Task PressKeybind(GameOptions.Keybind keybind, TimeSpan duration)
        {
            GameOptions.KeybindVKEntry keybindVKEntry = default(GameOptions.KeybindVKEntry);
            if (keybind.Primary.Key != Keys.None)
            {
                keybindVKEntry = keybind.Primary;
            }
            else if (keybind.Secondary.Key != Keys.None)
            {
                keybindVKEntry = keybind.Secondary;
            }
            if (keybindVKEntry.Key == Keys.None)
            {
                return;
            }
            Keys[] mods = new Keys[]
            {
                keybindVKEntry.Modifier0,
                keybindVKEntry.Modifier1,
                keybindVKEntry.Modifier2
            };
            foreach(var mod in mods)
            {
                if(mod != Keys.None)
                    InputManager.KeyDown(mod);
            }
            switch (keybindVKEntry.Key)
            {
                case Keys.LButton:
                case Keys.RButton:
                case Keys.MButton:
                case Keys.XButton1:
                case Keys.XButton2:
                    InputManager.PressMouseButton(keybindVKEntry.Key, 100, 100);
                    break;
                default:
                    InputManager.PressKey(keybindVKEntry.Key);
                    break;
            }

            await Coroutine.Sleep(duration);
            foreach (var mod in mods)
            {
                if (mod != Keys.None)
                    InputManager.KeyUp(mod);
            }
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
