using System.Collections.Generic;

namespace DodoPizza.Mappers
{
    public interface IMapper<TSource,TDestination>
    {
        TDestination Map(TSource value);
        TSource Map(TDestination value);
        ICollection<TSource> Map(ICollection<TDestination> value);
        ICollection<TDestination> Map(ICollection<TSource> value);
    }
}
