using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Play.MerchantPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("MerchantPortalApiScope")]
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