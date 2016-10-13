/**
 * Created by VolkovA on 26.02.14.
 */

using System;
using System.Reflection;
using selenium.core.Framework.Browser;
using selenium.core.Framework.Service;
using selenium.core.Logging;

namespace selenium.core {
    public class SeleniumContext<T> : ISeleniumContext where T : ISeleniumContext {
        private readonly BrowsersCache _browsersCache;

        protected SeleniumContext() {
            Log = new TestLoggerImpl();
            Web = new Web();
            _browsersCache = new BrowsersCache(Web, Log);
        }

        public static T Inst {
            get { return SingletonCreator<T>.CreatorInstance; }
        }

        #region ISeleniumContext Members

        public Web Web { get; private set; }

        public TestLogger Log { get; private set; }

        public Browser Browser {
            get { return _browsersCache.GetBrowser(BrowserType.CHROME); }
        }

        #endregion

        protected virtual void InitWeb() {
        }

        public virtual void Init() {
            InitWeb();
        }

        public void Destroy() {
            Inst.Browser.Destroy();
        }

        #region Nested type: SingletonCreator

        /// ‘абрика используетс€ дл€ отложенной инициализации экземпл€ра класса
        private sealed class SingletonCreator<S> where S : ISeleniumContext {
            //»спользуетс€ Reflection дл€ создани€ экземпл€ра класса без публичного конструктора
            private static readonly S instance = (S) Activator.CreateInstance(typeof (S));
//            (S).GetConstructor(
//                BindingFlags.Instance | BindingFlags.NonPublic,
//                null,
//                new Type[0],
//                new ParameterModifier[0]).Invoke(null);

            public static S CreatorInstance {
                get { return instance; }
            }
        }

        #endregion
    }
}