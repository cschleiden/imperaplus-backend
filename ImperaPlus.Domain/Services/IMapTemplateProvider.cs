using ImperaPlus.Domain.Map;

namespace ImperaPlus.Domain.Services
{
    public interface IMapTemplateProvider
    {
        MapTemplate GetTemplate(string templateName);
    }
}
