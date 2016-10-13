using RoslynSpike.SessionWeb.Models;

namespace RoslynSpike.Ember.DTO {
    public class ElementInstanceDto : EmberDtoBase {
        public string name { get; }
        public string rootScss { get; }
        public string type { get; }
        public string parentPage { get; }
        public string parentComponent { get; }

        public ElementInstanceDto(IElementInstance elementInstance, string parentPageId,
            string parentComponentId) : base(elementInstance.Id) {
            name = elementInstance.Name;
            rootScss = elementInstance.RootScss;
            type = elementInstance.Type;

            parentPage = parentPageId;
            parentComponent = parentComponentId;
        }
    }
}