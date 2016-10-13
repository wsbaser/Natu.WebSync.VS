namespace RoslynSpike.Ember.DTO {
    public class EmberDtoBase {
        public string id { get; set; }

        public EmberDtoBase(string id) {
            this.id = id;
        }
    }
}