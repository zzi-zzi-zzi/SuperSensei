using System;
using System.Threading.Tasks;
using static SuperSensei.Utils.Combat;
using static SuperSensei.Utils.Buffs;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;
using Buddy.Coroutines;

namespace SuperSensei.CombatClasses
{
	public class SoulFighter : ICombatHandler
	{
        public void UpdateRoutine() { }

        public async Task Combat()
		{
            var npc = (Npc)GameManager.LocalPlayer.CurrentTarget;
            if (npc == null)
                return;
            if (npc.GetType() == typeof(Npc))
			{
                //todo: debuf check for frozen.
				if (npc.IsCasting && npc.CurrentTarget == GameManager.LocalPlayer && !IsSkillOnCooldown("SoulFighter_Attack_LeftCounter_Lv1"))

                    foreach (var action in npc.CurrentActions)
					{
						if (action.Target == GameManager.LocalPlayer && action.TimeLeft < TimeSpan.FromMilliseconds(250))
						{
							if (await ExecuteSkillByAlias("SoulFighter_Attack_LeftCounter_Lv1"))
								return;
						}
					}
			}

			if (await ExecuteSkillByAlias("SoulFighter_Attack_JusticePunch_Lv1") ||
			                      await ExecuteSkillByAlias("SoulFighter_Attack_StrongPunch_Lv1") ||
			                      await ExecuteSkillByAlias("SoulFighter_Attack_PowerAttack_Lv1"))
			{
                await Coroutine.Yield();
			}

			if (await ExecuteSkillByAlias("SoulFighter_Attack_PowerstepPunch_Lv1"))
			{
				return;
			}
			
		}
        public async Task Pull(object Target)
        {

        }
    }
}

