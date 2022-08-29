using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MerchantPortal.WebApi.Controllers.Merchant
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMapper _mapper;

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
