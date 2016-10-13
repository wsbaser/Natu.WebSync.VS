using System;
using System.IO;

namespace selenium.core.Framework.Page {
    public abstract class EmailPageBase : PageBase, IEmailPage {
        private string FileName {
            get { return GetType().Name + ".html"; }
        }

        #region IEmailPage Members

        public bool Match(Uri uri) {
            string name = new FileInfo(uri.AbsolutePath).Name;
            return string.Compare(name, FileName, StringComparison.InvariantCulture) == 0;
        }

        #endregion
    }

    public interface IEmailPage {
        bool Match(Uri uri);
    }
}