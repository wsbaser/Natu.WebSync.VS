using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class ServiceDto : EmberDtoBase
    {
        public ServiceDto(IService service) : base(service.Id) {
        }
    }
}