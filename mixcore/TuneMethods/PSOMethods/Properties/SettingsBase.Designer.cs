﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSOMethods.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    public sealed partial class SettingsBase : global::System.Configuration.ApplicationSettingsBase {
        
        private static SettingsBase defaultInstance = ((SettingsBase)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SettingsBase())));
        
        public static SettingsBase Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int PSO_iter {
            get {
                return ((int)(this["PSO_iter"]));
            }
            set {
                this["PSO_iter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int Term_Config_Random_Search_count_generate_by_iteration {
            get {
                return ((int)(this["Term_Config_Random_Search_count_generate_by_iteration"]));
            }
            set {
                this["Term_Config_Random_Search_count_generate_by_iteration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("40")]
        public int PSO_population {
            get {
                return ((int)(this["PSO_population"]));
            }
            set {
                this["PSO_population"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.6")]
        public double PSO_c1 {
            get {
                return ((double)(this["PSO_c1"]));
            }
            set {
                this["PSO_c1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.8")]
        public double PSO_c2 {
            get {
                return ((double)(this["PSO_c2"]));
            }
            set {
                this["PSO_c2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool PSO_Used {
            get {
                return ((bool)(this["PSO_Used"]));
            }
            set {
                this["PSO_Used"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int PSO_Used_times {
            get {
                return ((int)(this["PSO_Used_times"]));
            }
            set {
                this["PSO_Used_times"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int SendByPSO {
            get {
                return ((int)(this["SendByPSO"]));
            }
            set {
                this["SendByPSO"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int SendByBactery {
            get {
                return ((int)(this["SendByBactery"]));
            }
            set {
                this["SendByBactery"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int IteratePSOtoSend {
            get {
                return ((int)(this["IteratePSOtoSend"]));
            }
            set {
                this["IteratePSOtoSend"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int HybrideSendEach {
            get {
                return ((int)(this["HybrideSendEach"]));
            }
            set {
                this["HybrideSendEach"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int HybrideGetEach {
            get {
                return ((int)(this["HybrideGetEach"]));
            }
            set {
                this["HybrideGetEach"] = value;
            }
        }
    }
}
