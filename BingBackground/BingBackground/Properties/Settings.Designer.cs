﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BingBackground.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Proxy {
            get {
                return ((string)(this["Proxy"]));
            }
            set {
                this["Proxy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Fill")]
        public string Position {
            get {
                return ((string)(this["Position"]));
            }
            set {
                this["Position"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("../BackgroundRec.txt")]
        public string BackgroundRecFile {
            get {
                return ((string)(this["BackgroundRecFile"]));
            }
            set {
                this["BackgroundRecFile"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("BackGroundImages")]
        public string ImgSaveFolder {
            get {
                return ((string)(this["ImgSaveFolder"]));
            }
            set {
                this["ImgSaveFolder"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US")]
        public string DownloadSourcePath {
            get {
                return ((string)(this["DownloadSourcePath"]));
            }
            set {
                this["DownloadSourcePath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://www.bing.com")]
        public string DownloadSite {
            get {
                return ((string)(this["DownloadSite"]));
            }
            set {
                this["DownloadSite"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1")]
        public int RunMode {
            get {
                return ((int)(this["RunMode"]));
            }
            set {
                this["RunMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int ChangeInterval {
            get {
                return ((int)(this["ChangeInterval"]));
            }
            set {
                this["ChangeInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\leta\\AppData\\Local\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5" +
            "n1h2txyewy\\LocalState\\Assets")]
        public string LockScreenBgDir {
            get {
                return ((string)(this["LockScreenBgDir"]));
            }
            set {
                this["LockScreenBgDir"] = value;
            }
        }
    }
}
