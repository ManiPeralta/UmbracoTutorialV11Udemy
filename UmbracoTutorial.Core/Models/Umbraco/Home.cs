namespace UmbracoTutorial.Core.UmbracoModels
{
    public partial class Home
    {
        public string? ShortHeroDescription {
            get
            {
                if (string.IsNullOrEmpty(this.HeroDescription))
                {
                    return "";
                }
                return $"{this.HeroDescription.Substring(0,30)}...";
            } 
         }
    }
}
