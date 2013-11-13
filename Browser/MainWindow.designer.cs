// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;

namespace Browser
{
    [Register ("MainWindowController")]
    partial class MainWindowController
    {
        [Outlet]
        MonoMac.WebKit.WebView webView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (webView != null)
            {
                webView.Dispose ();
                webView = null;
            }
        }
    }

    [Register ("MainWindow")]
    partial class MainWindow
    {

        void ReleaseDesignerOutlets ()
        {
        }
    }
}
