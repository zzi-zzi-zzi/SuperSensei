using Buddy.Common;

namespace SuperSaiyan
{
    internal class SuperSettings : JsonSettings
    {
        public SuperSettings() : base(GetSettingsFilePath(SettingsPath, "SuperSaiyan.json"))
        {
        }
    }
}