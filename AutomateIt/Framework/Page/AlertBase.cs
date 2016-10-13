using System;
using selenium.core.Framework.PageElements;

namespace selenium.core.Framework.Page {
    public abstract class AlertBase : ContainerBase, IHtmlAlert {
        protected AlertBase(IPage parent) : base(parent) {
        }

        protected AlertBase(IPage parent, string rootScss) : base(parent, rootScss) {
        }

        #region IHtmlAlert Members

        public abstract void Dismiss();

        public virtual void Accept() {
            Dismiss();
        }

        public virtual void SendKeys(string keysToSend) {
            throw new NotImplementedException();
        }

        public void SetAuthenticationCredentials(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public virtual string Text {
            get { return GetType().Name; }
        }

        #endregion
    }
}