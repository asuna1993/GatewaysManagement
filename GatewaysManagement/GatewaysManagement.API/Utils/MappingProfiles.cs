using GatewaysManagement.Common.DTO.Response;
using GatewaysManagement.Data.Entities;
using AutoMapper;
using System.Net;
using GatewaysManagement.Common.DTO.Request;
using System;
using GatewaysManagement.Data.Entities.Enums;

namespace GatewaysManagement.API.Utils
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Gateway, GatewayResponse>();

            CreateMap<CreateGatewayRequest, Gateway>();

            CreateMap<Device, DeviceResponse>()
                .ForMember(d => d.StatusId, opts => opts.MapFrom(source => (int)source.Status))
                .ForMember(d => d.Status, opts => opts.MapFrom(source => Enum.GetName(typeof(StatusEnum), source.Status)))
                .ForMember(d => d.Gateway, opts => opts.MapFrom(source => source.Gateway.Name))
                ;

            CreateMap<CreateDeviceRequest, Device>()
                .ForMember(d => d.Status, opts => opts.MapFrom(source => Enum.GetName(typeof(StatusEnum), source.StatusId)))
                ;
        }
    }
}