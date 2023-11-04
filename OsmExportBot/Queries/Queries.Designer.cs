﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OsmExportBot.Queries {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Queries {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Queries() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("OsmExportBot.Queries.Queries", typeof(Queries).Assembly);
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
        ///   Looks up a localized string similar to [out:csv(::lat,::lon)][timeout:25];
        ///(
        ///  nwr[&quot;building&quot;][!&quot;addr:housenumber&quot;]({{bbox}});
        ///);
        ///out center;.
        /// </summary>
        internal static string noaddr {
            get {
                return ResourceManager.GetString("noaddr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [out:csv(::lat,::lon)][timeout:25];
        ///(way[&quot;building&quot;=&quot;apartments&quot;]({{bbox}});)-&gt;.builds;
        ///(node[&quot;entrance&quot;]({{bbox}});)-&gt;.ents;
        ///(way(bn.ents);)-&gt;.entbuilds;
        ///(.builds - .entbuilds);
        ///out center;.
        /// </summary>
        internal static string noentrance {
            get {
                return ResourceManager.GetString("noentrance", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [out:csv(::lat,::lon)][timeout:25];
        ///(
        ///  node[&quot;entrance&quot;][&quot;ref&quot;][!&quot;addr:flats&quot;]({{bbox}});
        ///  node[&quot;entrance&quot;=&quot;staircase&quot;][!&quot;ref&quot;]({{bbox}});
        ///  node[&quot;entrance&quot;=&quot;staircase&quot;][!&quot;addr:flats&quot;]({{bbox}});
        ///);
        ///out center;.
        /// </summary>
        internal static string noflats {
            get {
                return ResourceManager.GetString("noflats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [out:csv(::lat,::lon)][timeout:25];
        ///(
        ///  nwr[&quot;shop&quot;][!&quot;opening_hours&quot;]({{bbox}});
        ///);
        ///out center;.
        /// </summary>
        internal static string nohours {
            get {
                return ResourceManager.GetString("nohours", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [out:csv(::lat,::lon)][timeout:25];
        ///(
        ///  nwr[&quot;building&quot;][!&quot;building:levels&quot;]({{bbox}});
        ///);
        ///out center;.
        /// </summary>
        internal static string nolvl {
            get {
                return ResourceManager.GetString("nolvl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [out:xml][timeout:25];
        ///(
        ///  nwr[&quot;amenity&quot;=&quot;parking&quot;][&quot;fee&quot;!=&quot;yes&quot;][&quot;access&quot;!=&quot;private&quot;][&quot;access&quot;!=&quot;customers&quot;][&quot;access&quot;!=&quot;no&quot;]({{bbox}});
        ///  
        ///  node[&quot;amenity&quot;=&quot;parking_entrance&quot;][&quot;fee&quot;!=&quot;yes&quot;][&quot;access&quot;!=&quot;private&quot;][&quot;access&quot;!=&quot;customers&quot;][&quot;access&quot;!=&quot;no&quot;]({{bbox}});
        ///  
        ///  way[&quot;parking:condition:left&quot;=&quot;free&quot;]({{bbox}});
        ///  way[&quot;parking:condition:both&quot;=&quot;free&quot;]({{bbox}});
        ///  way[&quot;parking:condition:right&quot;=&quot;free&quot;]({{bbox}});
        ///);
        ///out body;
        ///&gt;;
        ///out skel qt;.
        /// </summary>
        internal static string parkingfree {
            get {
                return ResourceManager.GetString("parkingfree", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [out:xml][timeout:25];
        ///area({{city}})-&gt;.searchArea;
        ///(
        ///  way[~&quot;^parking:lane:.*&quot;~&quot;.&quot;](area.searchArea);
        ///);
        ///out body;
        ///&gt;;
        ///out skel qt;.
        /// </summary>
        internal static string parkinglane {
            get {
                return ResourceManager.GetString("parkinglane", resourceCulture);
            }
        }
    }
}
