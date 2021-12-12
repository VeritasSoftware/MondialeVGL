using AutoMapper;
using MondialeVGL.OrderProcessor.Entities;
using MondialeVGL.OrderProcessor.Models;

namespace MondialeVGL.OrderProcessor.Services
{
    public class MappingService : IMappingService
    {
        IMapper _mapper;

        public MappingService()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderEntityCollection, OrderModelCollection>();

                cfg.CreateMap<OrderEntity, OrderModel>()
                    .ForMember(d => d.PurchaseOrderNumber, src => src.MapFrom(s => s.Header.PurchaseOrderNumber))
                    .ForMember(d => d.Supplier, src => src.MapFrom(s => GetSupplierCode(s.Header.Supplier)))
                    .ForMember(d => d.Origin, src => src.MapFrom(s => s.Header.Origin))
                    .ForMember(d => d.Destination, src => src.MapFrom(s => string.IsNullOrEmpty(s.Header.Destination) ? 
                                                                            GetDestination(s.Header.Supplier) :
                                                                            s.Header.Destination))
                    .ForMember(d => d.CargoReady, src => src.MapFrom(s => s.Header.CargoReadyDate.ToString("yyyy-MM-dd")))
                    .ForMember(d => d.Lines, src => src.MapFrom(s => s.Details));

                cfg.CreateMap<OrderDetailEntity, OrderLineModel>()
                    .ForMember(d => d.LineNumber, src => src.MapFrom(s => s.LineNumber))
                    .ForMember(d => d.ProductDescription, src => src.MapFrom(s => s.ItemDescription))
                    .ForMember(d => d.OrderQty, src => src.MapFrom(s => s.OrderQty));
            });

            _mapper = config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TSource: class
            where TDestination: class
        {
            return _mapper.Map<TDestination>(source);
        }

        private static string GetSupplierCode(string supplier)
        {
            return supplier switch
            {
                "SHANGHAI FURNITURE COMPANY" => "SFC01",
                "YANTIAN INDUSTRIAL PRODUCTS" => "YIP-1",
                _ => null
            };
        }

        private static string GetDestination(string supplier)
        {
            return supplier switch
            {
                "SHANGHAI FURNITURE COMPANY" => "AUMEL",
                "YANTIAN INDUSTRIAL PRODUCTS" => "AUSYD",
                _ => null
            };
        }
    }
}
