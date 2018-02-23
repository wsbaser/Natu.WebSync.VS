using System.Collections.Generic;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public interface IRoslynModel<T> : ICodeModel {
        void SynchronizeTo(T model);
        void Fill();
    }

    public abstract class RoslynModel<T> : IRoslynModel<T> {
        protected RoslynModel() {
            CodeErrors = new List<ICodeError>();
        }

        public List<ICodeError> CodeErrors { get; }
        public abstract void SynchronizeTo(T model);
        public abstract void Fill();
    }
}