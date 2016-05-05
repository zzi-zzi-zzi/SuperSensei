using System;
using System.Threading.Tasks;
using System.Windows.Forms;

using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.DataTables;
using Buddy.BotCommon;
using Buddy.Coroutines;

using log4net;
using Buddy.BladeAndSoul.Game.Objects;
using SuperSaiyan.CombatClasses;

namespace SuperSaiyan
{
    public class SuperSaiyan : CombatRoutineBase
    {
        #region IAuthored
        /// <summary>
        ///     The name of this authored object.
        /// </summary>
        public override string Name { get { return "Super Saiyan By ZZI"; } }

        /// <summary>
        ///     The author of this object.
        /// </summary>
        public override string Author { get { return "zzi"; } }

        /// <summary>
        ///     The version of this object implementation.
        /// </summary>
        public override Version Version { get { return new Version(0, 0, 1); } }
        #endregion


        private static ILog Log = LogManager.GetLogger("[Super Saiyan]");
        private ICombatHandler _combatMachine;

        public override void OnRegistered()
        {
            
            switch(GameManager.LocalPlayer.Class)
            {
                case PlayerClass.Warlock:
                    _combatMachine = new Warlock();
                    break;
                default:
                    Log.InfoFormat("[Super Saiyan] cannot handle class: {0} (YET!)", GameManager.LocalPlayer.Class);
                    _combatMachine = null;
                    break;
            }
        }

        /// <summary>
        ///     Utility & Thrall are up to the user.
        /// </summary>
        /// <returns></returns>
        public override async Task Combat()
        {
            if(_combatMachine != null)
                await _combatMachine.Combat();
            return;
        }

        
    }
}