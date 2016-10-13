using System;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public abstract class RoslynModelWithId<T> : RoslynModel<T>, ICodeModelWithId {
        public string Id { get; protected set; }
    }
}