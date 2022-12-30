using RedAndWhite.Domain;
using RedAndWhite.Infrastructure.Mapping;

namespace RedAndWhite.Model.Informations
{
    public class InformationModel : IMapFrom<Information>
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public string? Image { get; set; }
    }
}
