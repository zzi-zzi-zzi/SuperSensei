using Buddy.BladeAndSoul.Game;
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
                        Content = LoadWindowContent(Path.Combine(AppSettings.Instance.FullRoutinesPath, "SuperSaiyan", "SuperSaiyan", "GUI")),
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


        internal UserControl LoadWindowContent(string uiPath)
        {
            try
            {
                lock (ContentLock)
                {
                    _windowContent = LoadAndTransformXamlFile<UserControl>(Path.Combine(uiPath, "MainView.xaml"));
                    LoadChild(_windowContent, uiPath);
                    LoadResourceForWindow(Path.Combine(uiPath, "Dictionary.xaml"), _windowContent);
                    return _windowContent;
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception loading window content! {0}", ex);
            }
            return null;
        }

        /// <summary>Loads recursivly the child in ContentControl or Decorator with Tag.</summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="uiPath">The UI path.</param>
        private void LoadChild(FrameworkElement parentControl, string uiPath)
        {
            try
            {
                // Loop in Children of parent control of type FrameworkElement 
                foreach (FrameworkElement ctrl in LogicalTreeHelper.GetChildren(parentControl).OfType<FrameworkElement>())
                {
                    string contentName = ctrl.Tag as string;
                    // Tag contains a string end with ".xaml" : It's dymanic content 
                    if (!string.IsNullOrWhiteSpace(contentName) && contentName.EndsWith(".xaml"))
                    {
                        // Load content from XAML file
                        LoadDynamicContent(uiPath, ctrl, Path.Combine(uiPath, contentName));
                    }
                    else
                    {
                        // Try again with children of control
                        LoadChild(ctrl, uiPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception loading child {0}", ex);
            }
        }

        /// <summary>Loads the dynamic content from XAML file.</summary>
        /// <param name="uiPath">The UI path.</param>
        /// <param name="ctrl">The CTRL.</param>
        /// <param name="filename">Name of the content.</param>
        private void LoadDynamicContent(string uiPath, FrameworkElement ctrl, string filename)
        {
            try
            {
                if (File.Exists(filename))
                {
                    UserControl xamlContent = LoadAndTransformXamlFile<UserControl>(filename);

                    // Dynamic load of content is possible on Content control (UserControl, ...)
                    if (ctrl is ContentControl)
                    {
                        ((ContentControl)ctrl).Content = xamlContent;
                    }
                    // Or on Decorator control (Border, ...)
                    else if (ctrl is Decorator)
                    {
                        ((Decorator)ctrl).Child = xamlContent;
                    }
                    // Otherwise, log control where you try to put dynamic tag
                    else
                    {
                        Log.DebugFormat("Control of type '{0}' can't be used for dynamic loading.", ctrl.GetType().FullName);
                        return;
                    }
                    // Content added to parent control, try to search dynamic control in children
                    LoadChild(xamlContent, uiPath);
                }
                else
                {
                    Log.ErrorFormat("Error XAML file not found : '{0}'", filename);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception loading Dynamic Content {0}", ex);
            }
        }

        /// <summary>Loads the and transform xaml file.</summary>
        /// <param name="filename">The absolute path to xaml file.</param>
        /// <returns><see cref="Stream"/> which contains transformed XAML file.</returns>
        internal T LoadAndTransformXamlFile<T>(string filename)
        {
            try
            {
                string filecontent = File.ReadAllText(filename);

                // Change reference to custom TrinityPlugin class
                filecontent = filecontent.Replace("xmlns:ut=\"clr-namespace:SuperSaiyan.GUI.Components\"", "xmlns:ut=\"clr-namespace:SuperSaiyan.GUI.Components;assembly=" + Assembly.GetExecutingAssembly().GetName().Name + "\"");

                filecontent = Regex.Replace(filecontent, "<ResourceDictionary.MergedDictionaries>.*</ResourceDictionary.MergedDictionaries>", string.Empty, RegexOptions.Singleline | RegexOptions.Compiled);

                return (T)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(filecontent)));
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error loading/transforming XAML {0}", ex);
                return default(T);
            }
        }

        private void LoadResourceForWindow(string filename, UserControl control)
        {
            try
            {
                ResourceDictionary resource = LoadAndTransformXamlFile<ResourceDictionary>(filename);
                foreach (System.Collections.DictionaryEntry res in resource)
                {
                    if (!control.Resources.Contains(res.Key))
                        control.Resources.Add(res.Key, res.Value);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error loading resources {0}", ex);
            }
        }

    }
}