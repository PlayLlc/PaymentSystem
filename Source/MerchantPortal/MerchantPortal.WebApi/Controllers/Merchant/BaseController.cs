using AutoMapper;
using MerchantPortal.WebApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MerchantPortal.WebApi.Controllers.Merchant
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiFilterExceptionAttribute]
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
