using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.DataTables;
using Buddy.Coroutines;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CombatUitils = SuperSaiyan.Utils.Combat;

namespace SuperSaiyan.CombatClasses
{
    class Warlock : ICombatHandler
    {
        private static ILog Log = LogManager.GetLogger("[Super Saiyan][Warlock]");

        #region misc Dragon Helix debug stuff
        //base.OnRegistered();
        //var skillName = "Dragon Helix";
        //var skill = GameManager.LocalPlayer.Skills28.Find((S) => { return S.Id == 28092; });// GetSkillById(28091);
        //if (skill == null)
        //{
        //    Log.InfoFormat("Skill not found for {0}", skillName);
        //    return;
        //}
        ////Log.InfoFormat("Test: {0}", JsonConvert.SerializeObject());
        ////Log.InfoFormat("Dragon Helix Debug: {0}",
        ////            JsonConvert.SerializeObject(GameManager.LocalPlayer.Skills28, Formatting.None,
        ////                new JsonSerializerSettings()
        ////                {
        ////                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ////                    MaxDepth = 1,
        ////                }));
        //    try
        //    {
        //    var z = 28453;
        //        var s = DataTables.Skill3.GetRecord(z, 1);
        //        var obj = JsonConvert.SerializeObject(DataTables.Skillcastcondition3.GetRecord(s.CastConditionRecordId));
        //        Log.InfoFormat("Found skill with id: {0}\n{1}", z, obj);
        //    }
        //    catch(Exception e)
        //    {
        //    Log.Info(e.StackTrace);
        //    }
        ////var skill3 = DataTables.Skill3.GetRecord(28090, 1);
        ////Log.InfoFormat("Stance: {0}", JsonConvert.SerializeObject(DataTables.Stance.GetRecord(skill3.UiStance)));
        ////foreach (var x in GameManager.LocalPlayer.CurrentSkills)
        ////{
        ////    Log.InfoFormat("Found skill: {0} - {1}", x.Id, x.Name);
        ////}
        #endregion

        public async Task Combat()
        {
            //todo: is this channeled? & are we still hitting the target.
            if (await CombatUitils.ExecuteSkill("Dragon Helix", Keys.D4) || await CombatUitils.ExecuteSkill("Dragoncall", Keys.D4))
            {
                //dragon call is a casted ability. wait for it to end.
                while (GameManager.LocalPlayer.IsCasting)
                    await Coroutine.Sleep(100);
                return;
            } else { //debug Dragon Helix
                try
                {
                    var skill = GameManager.LocalPlayer.GetSkillByName("Dragon Helix");
                    var castResult = skill.ActorCanCastResult(GameManager.LocalPlayer);
                    if (castResult == SkillUseError.WrongStance)
                    {
                        Log.InfoFormat("Listing Current Player Effects");
                        foreach (var effect in GameManager.LocalPlayer.Effects)
                        {
                            Log.InfoFormat("Effect id: {0} Effect Name: {1}", effect.RecordId, effect.Name);
                        }

                        Log.InfoFormat("[{2}] Player Stance: {0} Skill Stance: {1}",
                            GameManager.LocalPlayer.Stance,
                            DataTables.Skillcastcondition3.GetRecord(skill.Record.CastConditionRecordId).Stance,
                            skill.Id
                            );
                        var skill2 = GameManager.LocalPlayer.Skills28.Find((S) => { return S.Id == 28091; });// GetSkillById(28091);
                        Log.InfoFormat("[{2}] Player Stance: {0} Skill Stance: {1}",
                            GameManager.LocalPlayer.Stance,
                            DataTables.Skillcastcondition3.GetRecord(skill2.Record.CastConditionRecordId).Stance,
                            skill2.Id
                            );
                        skill2 = GameManager.LocalPlayer.Skills28.Find((S) => { return S.Id == 28092; });// GetSkillById(28091);
                        Log.InfoFormat("[{2}] Player Stance: {0} Skill Stance: {1}",
                            GameManager.LocalPlayer.Stance,
                            DataTables.Skillcastcondition3.GetRecord(skill2.Record.CastConditionRecordId).Stance,
                            skill2.Id
                            );
                    }
                }
                catch { }
            }

            if (await CombatUitils.ExecuteSkill("Wingstorm", Keys.V))
            {
                return;
            }
            
            if (await CombatUitils.ExecuteSkill("Awakened Rupture", Keys.F) || await CombatUitils.ExecuteSkill("Rupture", Keys.F))
            {
                return;
            }
            //if we have leech make sure it's not on cooldown before casting soulshackle 
            var leech = GameManager.LocalPlayer.GetSkillByName("Leech");
            if (
                (leech != null && !GameManager.LocalPlayer.IsSkillOnCooldown("Leech") && await CombatUitils.ExecuteSkill("Soul Shackle", Keys.D2))
                || await CombatUitils.ExecuteSkill("Soul Shackle", Keys.D2)
               )
            {
                return;
            }

            if (await CombatUitils.ExecuteSkill(leech, Keys.F))
            {
                return;
            }
            var center = GameManager.LocalPlayer.CurrentTarget.Position; //should be close within 100ms
            var imprison = GameManager.LocalPlayer.GetSkillByName("Imprison");
            if (await CombatUitils.ExecuteSkill(imprison, Keys.D3))
            {
                while (GameManager.LocalPlayer.IsCasting)
                {
                    //Imprison has the raidus of 3 so make sure the target is inside 1.5
                    //need to figure out how position distance compares to spell distance.
                    if (!(center.Distance2D(GameManager.LocalPlayer.CurrentTarget.Position) <= 1.5) )
                        break; //stop casting if our target has moved outside of our circle.
                    await Coroutine.Sleep(100);
                }
                return;
            }
            if (await CombatUitils.ExecuteSkill("Dimensional Volley", Keys.T) || await CombatUitils.ExecuteSkill("Bombardment", Keys.T))
            {
                return;
            }
            await DefaultSkill();
        }

        async Task DefaultSkill()
        {
            if (await CombatUitils.ExecuteSkill("Incantation", Keys.R) ||
                await CombatUitils.ExecuteSkill("Burst", Keys.R) ||
                await CombatUitils.ExecuteSkill("Mantra", Keys.R)
                )
                return;
        }
    }
}
