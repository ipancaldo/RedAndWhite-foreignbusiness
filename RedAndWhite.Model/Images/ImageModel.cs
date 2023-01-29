using RedAndWhite.Domain.ValueObjects.Image;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Images
{
    public class ImageModel : IMapFrom<CreateImage>
    {
        public string Image { get; set; }
    }
}
