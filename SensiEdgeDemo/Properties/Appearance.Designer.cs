﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SensiEdgeDemo.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Appearance : global::System.Configuration.ApplicationSettingsBase {
        
        private static Appearance defaultInstance = ((Appearance)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Appearance())));
        
        public static Appearance Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string selectedAccentColor {
            get {
                return ((string)(this["selectedAccentColor"]));
            }
            set {
                this["selectedAccentColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string selectedTheme {
            get {
                return ((string)(this["selectedTheme"]));
            }
            set {
                this["selectedTheme"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string selectedFontSize {
            get {
                return ((string)(this["selectedFontSize"]));
            }
            set {
                this["selectedFontSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string selectedPalette {
            get {
                return ((string)(this["selectedPalette"]));
            }
            set {
                this["selectedPalette"] = value;
            }
        }
    }
}