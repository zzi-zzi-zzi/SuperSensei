using System;
using System.Linq;
using System.Threading.Tasks;
using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;
using Buddy.BotCommon;
using Buddy.Coroutines;
using log4net;

namespace SuperSensei.Utils
{
	public class Looting
	{
		private static readonly ILog Log = LogManager.GetLogger("[Super Saiyan]");	
		/// <summary>
		/// Looting. thanks Theonn for this!
		/// </summary>
		public static async Task Loot()
		{
			//Log.Info("Looting");

			//Log.InfoFormat("Player HP {0} MAX HP {1} PCTHP {2}", GameManager.LocalPlayer.Health, GameManager.LocalPlayer.MaxHealth, GameManager.LocalPlayer.HealthPercent);
			//Log.Info("Heal Called===============");

			if (GameManager.LocalPlayer.IsCasting)
			{
				return;
			}
            
			try
			{
				foreach (var a in GameManager.Actors.OfType<ILootable>())
				{
	        		LootInfo lootInfo = (a as ILootable)?.LootInfo;

					if (lootInfo != null && lootInfo.IsValid && lootInfo.AvailableLoot.Count > 0)
					{
						Log.Info(a.Name + " [" + a.Id.ToString("X16") + "] " + a.Alias + " HasLoot " + lootInfo.HasLoot);

						foreach (var available in lootInfo.AvailableLoot)
						{
							Log.InfoFormat("Item Record {0} Stack Count {1} ItemId {2}", available.ItemRecord.Alias, available.StackCount, available.ItemId);
						}

						Log.InfoFormat("Lootable {0}  distance {1}", a.Name, a.Distance);
						if (a is EnvironmentObject)
							await CommonBehaviors.LootActor(a as EnvironmentObject, 2);
						else
							await CommonBehaviors.LootActor(a as DeadBody, 2);

						await Coroutine.Yield();
					}
				}
			}
			finally
			{
			}
		}
	}
}

