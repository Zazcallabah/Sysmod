﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3074
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HighwaySimulation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HighwaySimulation.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Numerical input argument was negativ when it wasn&apos;t allowed to be..
        /// </summary>
        internal static string ArgumentNegative {
            get {
                return ResourceManager.GetString("ArgumentNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tried to get call position with invalid time input parameter. Must be while call is in progress..
        /// </summary>
        internal static string CallPositionForInvalidTime {
            get {
                return ResourceManager.GetString("CallPositionForInvalidTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to When trying to find out time for a call position, the position given was out of range..
        /// </summary>
        internal static string CallTimeForInvalidPosition {
            get {
                return ResourceManager.GetString("CallTimeForInvalidPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tried to find a station covering a position beyond the range of the highway..
        /// </summary>
        internal static string GetStationForInvalidPosition {
            get {
                return ResourceManager.GetString("GetStationForInvalidPosition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Given highway length is negative..
        /// </summary>
        internal static string HighwayLengthNegative {
            get {
                return ResourceManager.GetString("HighwayLengthNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number of channels in a station can&apos;t be negative.
        /// </summary>
        internal static string NumberChannelsNegative {
            get {
                return ResourceManager.GetString("NumberChannelsNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Given number of stations is zero. Must be positive non-zero integer..
        /// </summary>
        internal static string NumberOfStationsZero {
            get {
                return ResourceManager.GetString("NumberOfStationsZero", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Release Channel was called on a station that didn&apos;t have any claimed channels..
        /// </summary>
        internal static string ReleasedChannelNotHeld {
            get {
                return ResourceManager.GetString("ReleasedChannelNotHeld", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Station length value can&apos;t be negative.
        /// </summary>
        internal static string StationLengthNegative {
            get {
                return ResourceManager.GetString("StationLengthNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Station starting position can&apos;t be negative..
        /// </summary>
        internal static string StationStartNegative {
            get {
                return ResourceManager.GetString("StationStartNegative", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number of reserved channels can&apos;t exceed number of channels in a base station..
        /// </summary>
        internal static string TooManyReservedChannels {
            get {
                return ResourceManager.GetString("TooManyReservedChannels", resourceCulture);
            }
        }
    }
}