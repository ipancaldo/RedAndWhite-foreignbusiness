using RedAndWhite.Domain.ValueObjects.Informations;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Informations
{
    public class ModifyInformationModel : IMapFrom<NewInformation>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string? Image { get; set; }
    }
}
