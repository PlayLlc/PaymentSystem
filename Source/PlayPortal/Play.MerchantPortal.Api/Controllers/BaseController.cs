using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace MerchantPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class BaseController : ControllerBase
    {
        #region Instance Values

        protected readonly IMapper _Mapper;

        #endregion

        #region Constructor

        public BaseController(IMapper mapper)
        {
            _Mapper = mapper;
        }

        #endregion
    }
}