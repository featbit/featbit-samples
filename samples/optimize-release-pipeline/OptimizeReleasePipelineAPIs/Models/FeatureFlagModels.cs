using Microsoft.AspNetCore.Hosting.Server;

namespace OptimizeReleasePipelineAPIs.Models
{
    public class FeatureFlagSimple
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Key { get; set; }

        public bool IsEnabled { get; set; }

        public string VariationType { get; set; }

        public ICollection<Variation> Variations { get; set; }

        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The possible variation value(s) that would be returned. 
        /// </summary>
        public Serves Serves { get; set; }

        public ICollection<string> Tags { get; set; }
    }

    public class Variation
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Id) &&
                   !string.IsNullOrWhiteSpace(Name);
        }

        public void Assign(Variation source)
        {
            if (source.Id != Id)
            {
                return;
            }

            Name = source.Name;
            Value = source.Value;
        }

        public bool ValueEquals(object obj)
        {
            return obj is Variation variation &&
                   Id == variation.Id &&
                   Name == variation.Name &&
                   Value == variation.Value;
        }
    }

    public class Serves
    {
        public IEnumerable<string> EnabledVariations { get; set; }

        public string DisabledVariation { get; set; }
    }
}
