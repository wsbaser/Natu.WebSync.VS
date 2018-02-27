using System;
using Microsoft.CodeAnalysis;
using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.SessionWeb.RoslynModels {
    public class RoslynService : RoslynNamedTypeWrapper<IService>, IService {
        public RoslynService(INamedTypeSymbol serviceType) : base(serviceType) {
        }

        public override void SynchronizeTo(IService model) {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is RoslynService service2))
            {
                return false;
            }

            return true;
        }
    }
}