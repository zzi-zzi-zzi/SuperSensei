using System;
using System.Threading.Tasks;
using static SuperSensei.Utils.Combat;
using static SuperSensei.Utils.Buffs;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;

namespace SuperSensei.CombatClasses
{
	public class SoulFighter : ICombatHandler
	{
		int rotation = 0;
		public async Task Combat()
		{
            var npc = (Npc)GameManager.LocalPlayer.CurrentTarget;
            if (npc == null)
                return;
            if (npc.GetType() == typeof(Npc))
			{
				if (npc.IsCasting && npc.CurrentTarget == GameManager.LocalPlayer)
					foreach (var action in npc.CurrentActions)
					{
						if (action.Target == GameManager.LocalPlayer && action.TimeLeft < TimeSpan.FromMilliseconds(250))
						{
							if (await ExecuteSkillByAlias("SoulFighter_Attack_LeftCounter_Lv1"))
								return;
						}
					}
			}

			if (rotation == 0 && (await ExecuteSkillByAlias("SoulFighter_Attack_JusticePunch_Lv1") ||
			                      await ExecuteSkillByAlias("SoulFighter_Attack_StrongPunch_Lv1") ||
			                      await ExecuteSkillByAlias("SoulFighter_Attack_PowerAttack_Lv1")))
			{
				rotation = 1;
				return;
			}

			if (rotation == 1 && await ExecuteSkillByAlias("SoulFighter_Attack_PowerstepPunch_Lv1"))
			{
				rotation = 0;
				return;
			}
			
		}
	}
}

