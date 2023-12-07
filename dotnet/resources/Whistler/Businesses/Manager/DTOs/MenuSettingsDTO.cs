using Whistler.Businesses;

namespace Whistler.DTOs.Businesses
{
    class MenuSettingsDTO
    {
        public string TypeName { get; set; }
        public ProductSettings[] Items { get; set; }
    }
}
