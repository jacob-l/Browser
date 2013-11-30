using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace Browser
{
    public partial class MainWindowController : MonoMac.AppKit.NSWindowController
    {
        #region Constructors
        public MainWindowController (IntPtr handle) : base (handle)
        {
            Initialize ();
        }
        [Export ("initWithCoder:")]
        public MainWindowController (NSCoder coder) : base (coder)
        {
            Initialize ();
        }
        public MainWindowController () : base ("MainWindow")
        {
            Initialize ();
        }
        void Initialize ()
        {
        }
        #endregion
        public new MainWindow Window
        {
            get
            {
                return (MainWindow)base.Window;
            }
        }

        //Интерфейс из xib(nib) построен, инициализорован и все ссылки на UI компоненты установлены
        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            // Создаем объект через который js сможет обращаться к C#. Назовем его interaction
            // window.interaction.callFromJs(param1, param2, param3) - вызываем метод из js.
            webView.WindowScriptObject.SetValueForKey(this, new NSString("interaction"));

            webView.MainFrame.LoadHtmlString (@"
                <html>
                    <head></head>
                    <body id=body>
                        <h1>Интерфейс</h1>
                        <button id=btn>Вызвать C#</button>
                        <p id=msg></p>

                        <script>
                            function buttonClick() {
                                interaction.callFromJs('Вызов из js.');
                            }
                            function showMessage(msg) {
                                document.getElementById('msg').innerHTML = msg;
                            }

                            document.getElementById('btn').onclick = buttonClick;
                        </script>
                    </body>
                </html>", null);

        }

        // Из соображений безопасности указываем, какие методы могут быть вызваны из js
        [Export ("isSelectorExcludedFromWebScript:")]
        public static bool IsSelectorExcludedFromWebScript(MonoMac.ObjCRuntime.Selector aSelector)
        {
            if (aSelector.Name == "callFromJs")
                return false;

            return true; // Запрещаем вызов всех остальных методов
        }

        [Export("callFromJs")]
        public void CallFromJs(NSString message)
        {
            CallJs("showMessage", new NSObject[] { new NSString(message + " Ответ из C#") });
        }

        public void CallJs(string function, NSObject[] arguments)
        {
            this.InvokeOnMainThread(() =>
            {
                webView.WindowScriptObject.CallWebScriptMethod(function, arguments);
            });
        }
    }
}

