using RedAndWhite.Domain.ValueObjects.Informations;
using RedAndWhite.Infrastructure.Mapping;
using RedAndWhite.Model.Images;

namespace RedAndWhite.Model.Informations
{
    public class NewInformationModel : IMapFrom<NewInformation>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public List<ImageModel> Images { get; set; }
    }
}
