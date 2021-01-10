using System;
using System.Windows.Forms;

namespace CustomCapes.Common.Native {

    public class NativeWindowWrapper : NativeWindow, IDisposable {

        #region Fields

        public event EventHandler<Message> MessageReceived;

        #endregion
        
        #region Constructor

        public NativeWindowWrapper() {
            base.CreateHandle(new CreateParams());
        }
        
        #endregion
        
        #region Methods
        
        protected override void WndProc(ref Message m) {
            MessageReceived?.Invoke(this, m);
            base.WndProc(ref m);
        }

        public void Dispose() {
            DestroyHandle();
        }
        
        #endregion
    }

}