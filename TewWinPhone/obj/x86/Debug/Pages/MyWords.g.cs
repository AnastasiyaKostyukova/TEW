﻿

#pragma checksum "D:\Study\Programming\Projects\Tew_Project\TewWinPhone\Pages\MyWords.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EFFFD8CB9FC5572FD90211F1516D0AA6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TewWinPhone.Pages
{
    partial class MyWords : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 20 "..\..\..\Pages\MyWords.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.DeleteWord;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 22 "..\..\..\Pages\MyWords.xaml"
                ((global::Windows.UI.Xaml.Controls.MenuFlyoutItem)(target)).Click += this.CleanDb;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


