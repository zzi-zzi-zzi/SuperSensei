using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SuperSensei.Utils.Combat;
using static SuperSensei.Utils.Buffs;

namespace SuperSensei.CombatClasses
{
    partial class Assassin : ICombatHandler
    {
        private static ILog Log = LogManager.GetLogger("[Super Saiyan][Assassin]");
        Func<Task> _combat;
        Func<object, Task> _pull;

        public void UpdateRoutine()
        {
            if (hasStealthPull)
            {
                _pull = StealthPull;
            }
            if (hasStealthBuild)
            {
                if (_combat != Stealth)
                    Log.Info("Lightning Rend detected. Using Infinite Backstab Build");

                _combat = Stealth;
                return;
            }
            Log.Info("Lightning Rend was not found. Using default build");
        }

        public Assassin()
        {
            UpdateRoutine();
        }

        public async Task Combat()
        {
            await _combat();
        }

        public async Task Pull(object Target)
        {
            await _pull(Target);
        }


    }
}
