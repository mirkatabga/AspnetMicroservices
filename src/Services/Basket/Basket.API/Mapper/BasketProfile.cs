using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;

namespace Basket.Basket.API.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>();
        }
    }
}