using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;
using Buddy.BotCommon;
using Buddy.Common.Math;
using Buddy.Coroutines;
using log4net;
using System;

using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperSensei.Utils
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
		/// Executes the skill by alias.
		/// </summary>
		/// <returns>The skill by alias.</returns>
		/// <param name="alias">Alias.</param>
		internal static async Task<bool> ExecuteSkillByAlias(string alias)
		{
			var skill = GameManager.LocalPlayer.GetSkillByAlias(alias);
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
            await Coroutine.Sleep(150);
            return true;
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

        internal static bool IsSkillOnCooldown(string alias)
        {
            return IsSkillOnCooldown(GameManager.LocalPlayer.GetSkillByAlias(alias));
        }
        
        /// <summary>
        /// taken from honorbuddy. Moves the character behind the mob. 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static async Task GetBehindUnit(Npc target)
        {
            if (target == null)
                return;

            var posa = CalculatePointBehind(target.Position, target.Facing, 3f);
            var posb = CalculatePointBehind(target.Position, target.Facing, 2.5f);


            await CommonBehaviors.MoveTo( GetNearestPointOnSegment(posa, posb) );
            target.Face();
        }
        
        /// <summary>
        /// taken from honorbuddy. This will get the closest point between two points that make a line.
        /// </summary>
        /// <param name="segmentStart"></param>
        /// <param name="segmentEnd"></param>
        /// <returns></returns>
        internal static Vector3 GetNearestPointOnSegment(Vector3 segmentStart, Vector3 segmentEnd)
        {
            var point = GameManager.LocalPlayer.Position;

            float num = segmentStart.DistanceSqr(segmentEnd);
            if ((double)num == 0.0)
            {
                return segmentStart;
            }
            var point1 = point - segmentStart;
            var point2 = segmentEnd - segmentStart;
            float num2 = Dot(point1, point2) / num;

            if (num2 < 0f)
            {
                return segmentStart;
            }
            if (num2 > 1f)
            {
                return segmentEnd;
            }
            return new Vector3(segmentStart.X + point2.X * num2, segmentStart.Y + point2.Y * num2, segmentStart.Z + point2.Z * num2);
        }

        internal static float Dot(Vector3 a, Vector3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        internal static Vector3 CalculatePointBehind(Vector3 target, float facingRads, float distanceToTarget)
        {
            var targetFacingRadians = NormalizeRadian(facingRads);
            var pnt = new Vector3((float)Math.Cos(targetFacingRadians), (float)Math.Sin(targetFacingRadians), 0f);
            return target - pnt * (distanceToTarget * GameConsts.WorldScale);
        }

        public const float TWO_PI = 6.2831853071795862f;

        // Styx.Helpers.WoWMathHelper
        public static float NormalizeRadian(float radian)
        {
            if (radian < 0f)
            {
                return (-((-radian) % TWO_PI) + TWO_PI);
            }
            return radian % TWO_PI;
        }

    }
}
