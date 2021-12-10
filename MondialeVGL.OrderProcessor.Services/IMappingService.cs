namespace MondialeVGL.OrderProcessor.Services
{
    public interface IMappingService
    {
        TDestination Map<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class;
    }
}