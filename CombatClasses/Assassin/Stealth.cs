using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperSensei.Utils.Combat;
using static SuperSensei.Utils.Buffs;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;
using Buddy.Coroutines;

namespace SuperSensei.CombatClasses
{
    partial class Assassin
    {
        public bool hasStealthPull => GameManager.LocalPlayer.GetSkillByName("Blink Step") != null;


        public bool hasStealthBuild => GameManager.LocalPlayer.GetSkillByName("Lightning Rend") != null;

        public async Task Stealth()
        {
            var stealthTimer = GameManager.LocalPlayer.Effects.FirstOrDefault(i => i.RecordId == 0x18CB6D1);
            if(stealthTimer == null)
            {
                //get back into stealth - Decoy
                if (GameManager.LocalPlayer.CurrentTarget.GetType() == typeof(Npc))
                {
                    var npc = GameManager.LocalPlayer.CurrentTarget as Npc;
                    if (npc != null && npc.IsCasting && npc.CurrentTarget == GameManager.LocalPlayer)
                        foreach (var action in npc.CurrentActions)
                        {
                            if (action.Target == GameManager.LocalPlayer && action.TimeLeft < TimeSpan.FromMilliseconds(250))
                            {
                                if (await ExecuteSkill("Decoy"))
                                    return;
                            }
                        }
                }
            }

            //currently bugged https://github.com/BosslandGmbH/SenseibuddyBugs/issues/12
            if (stealthTimer.TimeLeft < TimeSpan.FromSeconds(1))
            {
                //try and prolong stealth.
                var sd = GameManager.LocalPlayer.GetSkillByName("Shadow Drain");
                if (sd != null && !IsSkillOnCooldown(sd))
                {
                    if (await ExecuteSkill("Throwing Dagger"))
                        await Coroutine.Yield();

                    if (await ExecuteSkill(sd))
                        return;
                }
                else
                {
                    if (await ExecuteSkill("Bolt Strike"))
                        await Coroutine.Yield();
                    await GetBehindUnit(GameManager.LocalPlayer.CurrentTarget as Npc);
                    return;
                }

            }
            if (await ExecuteSkill("Lightning Pierce") || await ExecuteSkill("Lightning Crash"))
                return;

            if (GameManager.LocalPlayer.Focus < 6 && await ExecuteSkill("Lightning Rend"))
                return;

            if (await ExecuteSkill("Heartstab"))
                return;
        }

        public async Task StealthPull(object Target)
        {
            var real = Target as Npc;
            if (real == null)
                return;
            real.Face();
            InputManager.PressKeybind(ShortcutKey.DashForward);
            await ExecuteSkill("Blink Step");
        }
    }
}
