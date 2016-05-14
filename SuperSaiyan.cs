﻿using Buddy.BladeAndSoul.Game;
using Buddy.BladeAndSoul.Game.Objects;
using Buddy.BladeAndSoul.Infrastructure;
using Buddy.BotCommon;
using Buddy.Engine;
using log4net;
using SuperSaiyan.CombatClasses;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Markup;
using UserControl = System.Windows.Controls.UserControl;
using Application = System.Windows.Application;
using Buddy.BladeAndSoul.ViewModels;
using SuperSaiyan.Settings;
using SuperSaiyan.GUI.Components;

namespace SuperSaiyan
{
    public class SuperSaiyan : CombatRoutineBase, IUIButtonProvider
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

        public string ButtonText
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        public SuperSaiyan()
        {
        }

        private static ILog Log = LogManager.GetLogger("[Super Saiyan]");
        private ICombatHandler _combatMachine;
        private Window _gui;
        private UserControl _windowContent;

        private static object ContentLock = new object();

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
        /// </summary>
        /// <returns></returns>
        public override async Task Combat()
        {
            if(_combatMachine != null)
                await _combatMachine.Combat();
            return;
        }

        public void OnButtonClicked(object sender)
        {
            try
            {
                if (_gui == null)
                {
                    _gui = new Window
                    {
                        DataContext = new SuperSettings(),
                        Content = WPFUtils.LoadWindowContent(Path.Combine(AppSettings.Instance.FullRoutinesPath, "SuperSaiyan", "GUI")),
                        MinHeight = 400,
                        MinWidth = 200,
                        Title = "Super Saiyan Settings",
                        ResizeMode = ResizeMode.CanResizeWithGrip,

                        //SizeToContent = SizeToContent.WidthAndHeight,
                        SnapsToDevicePixels = true,
                        Topmost = false,
                        WindowStartupLocation = WindowStartupLocation.Manual,
                        WindowStyle = WindowStyle.SingleBorderWindow,
                        Owner = null,
                        Width = 550,
                        Height = 650,
                    };
                    _gui.Closed += WindowClosed;

                }
            }
            catch { }
            
            _gui.Show();
        }

        /// <summary>Call when Config Window is closed.</summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        void WindowClosed(object sender, EventArgs e)
        {
            var context = _gui.DataContext as SuperSettings;
            if(context != null)
            {
                Log.Info("Save settings!");
                context.Save();
            } else
            {
                Log.InfoFormat("context == null");
            }
            _gui = null;
        }
    }
}