﻿

#pragma checksum "C:\Users\Isabel\documents\visual studio 2013\Projects\SkiApp\SkiAppClient\SkiDayPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F57B12D36A232C9F12A0D23CC887F5CC"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SkiAppClient
{
    partial class SkiDayPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 71 "..\..\SkiDayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SeeHistory_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 81 "..\..\SkiDayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.cbDestinations_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 90 "..\..\SkiDayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.SaveSkiDay_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 91 "..\..\SkiDayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddLift_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 92 "..\..\SkiDayPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddSlope_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


