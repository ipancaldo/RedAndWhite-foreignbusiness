using AutoMapper;
using System.Reflection;

namespace RedAndWhite.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        private const string AssembliesPrefixName = "RedAndWhite";

        public MappingProfile()
        {
            foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()
                                                           .Where(a => a.FullName!.Contains(AssembliesPrefixName)))
            {
                ApplyMappingsFromAssembly(assembly);
            }
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                                                                       i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                                .ToList();

            foreach(var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping") ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
