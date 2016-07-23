using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSensei
{
    interface ICombatHandler
    {
        void UpdateRoutine();
        Task Combat();
        Task Pull(object Target);
    }
}
