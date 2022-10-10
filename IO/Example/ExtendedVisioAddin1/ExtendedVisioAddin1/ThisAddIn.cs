using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ExtendedVisioAddin1.Properties;
using Visio = Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1
{
    [ComVisible(true)]
    [Guid("e808c3ff-2936-479e-adef-926fe3353f5b")]
    [ProgId("ExtendedVisioAddin1.Addin")]
    public partial class ThisAddIn : Extensibility.IDTExtensibility2
    {
        public Visio.Application Application { get; set; }

        /// <summary>
        /// A simple command
        /// </summary>
        public void Command1()
        {
            MessageBox.Show(
                "Hello from command 1!",
                "ExtendedVisioAddin1");
        }

        /// <summary>
        /// A command to demonstrate conditionally enabling/disabling.
        /// The command gets enabled only when a shape is selected
        /// </summary>
        public void Command2()
        {
            if (Application == null || Application.ActiveWindow == null || Application.ActiveWindow.Selection == null)
                return;

            MessageBox.Show(
                string.Format("Hello from (conditional) command 2! You have {0} shapes selected.", Application.ActiveWindow.Selection.Count),
                "ExtendedVisioAddin1");
        }

        /// <summary>
        /// Callback called by the UI manager when user clicks a button
        /// Should do something meaningful when corresponding action is called.
        /// </summary>
        public void OnCommand(string commandId)
        {
            switch (commandId)
            {
                case "Command1":
                    Command1();
                    return;

                case "Command2":
                    Command2();
                    return;
            }
        }

        /// <summary>
        /// Callback called by UI manager.
        /// Should return if corresponding command should be enabled in the user interface.
        /// By default, all commands are enabled.
        /// </summary>
        public bool IsCommandEnabled(string commandId)
        {
            switch (commandId)
            {
                case "Command1":    // make command1 always enabled
                    return true;

                case "Command2":    // make command2 enabled only if a drawing is opened
                    return Application != null
                        && Application.ActiveWindow != null
                        && Application.ActiveWindow.Selection.Count > 0;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Callback called by UI manager.
        /// Should return if corresponding command (button) is pressed or not (makes sense for toggle buttons)
        /// </summary>
        public bool IsCommandChecked(string command)
        {
            return false;
        }
        /// <summary>
        /// Callback called by UI manager.
        /// Returns a label associated with given command.
        /// We assume for simplicity taht command labels are named simply named as [commandId]_Label (see resources)
        /// </summary>
        public string GetCommandLabel(string command)
        {
            return Resources.ResourceManager.GetString(command + "_Label");
        }

        /// <summary>
        /// Returns a bitmap associated with given command.
        /// We assume for simplicity that bitmap ids are named after command id.
        /// </summary>
        public Bitmap GetCommandBitmap(string id)
        {
            return (Bitmap)Resources.ResourceManager.GetObject(id);
        }

        internal void UpdateUI()
        {
            UpdateRibbon();
        }

        private void Application_SelectionChanged(Visio.Window window)
        {
            UpdateUI();
        }

        private void ThisAddIn_Startup()
        {
            Application.SelectionChanged += Application_SelectionChanged;

        }

        private void ThisAddIn_Shutdown()
        {
            Application.SelectionChanged -= Application_SelectionChanged;

        }


        public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            Application = (Visio.Application)application;
            ThisAddIn_Startup();
        }

        public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref Array custom)
        {
            ThisAddIn_Shutdown();
        }

        public void OnAddInsUpdate(ref Array custom)
        {
        }

        public void OnStartupComplete(ref Array custom)
        {
        }

        public void OnBeginShutdown(ref Array custom)
        {
        }

    }
}
