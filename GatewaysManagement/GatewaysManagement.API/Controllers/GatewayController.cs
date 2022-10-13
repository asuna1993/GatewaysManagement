using GatewaysManagement.Common.DTO.Request;
using GatewaysManagement.Common.DTO.Response;
using GatewaysManagement.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using GatewaysManagement.Data.Entities;
using System.Collections.Generic;
using GatewaysManagement.Services.Utils;
using Newtonsoft.Json;

namespace GatewaysManagement.API.Controllers
{
    [Route("api/gateway")]
    [ApiController]
    [AllowAnonymous]
    public class GatewayController : Controller
    {
        private readonly IGatewayService _gatewayService;
        private readonly IMapper _mapper;

        public GatewayController(IGatewayService gatewayService, IMapper mapper)
        {
            _gatewayService = gatewayService ?? throw new ArgumentNullException(nameof(gatewayService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("{gatewayId}", Name = "GetGateway")]
        //[Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<GatewayResponse>> GetGateway(Guid gatewayId)
        {
            Gateway gateway = await _gatewayService.GetGatewayAsync(gatewayId);
            if (gateway == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<GatewayResponse>(gateway));
        }

        [HttpGet()]
        //[Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<GatewayResponse>>> GetGateways([FromQuery] ResourceParameters resourceParameters)
        {            
            PageList<Gateway> result = await _gatewayService.GetGatewaysAsync(resourceParameters);
            var paginationMetadata = result.GetPaginationData;
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(_mapper.Map<IEnumerable<GatewayResponse>>(result));
        }

        /// <summary>
        /// Create a new Gateway.
        /// </summary>
        /// <param name="gateway">
        /// Add Gateway Request object. Include SerialNumber, Name, and IPAdress. 
        /// </param>
        [HttpPost]
        //[Authorize]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateGateway([FromBody] CreateGatewayRequest gateway)
        {
            Gateway newGateway = _mapper.Map<Gateway>(gateway);
            await _gatewayService.AddGatewayAsync(newGateway);
            return Created("", true);
        }

        /// <summary>
        /// Update Gateway information. Requires authentication.
        /// </summary>
        /// <param name="gateway">
        /// Add Gateway Request object. Include SerialNumber, Name, and IPAdress.
        /// </param>
        //[Authorize]
        [HttpPut("{gatewayId}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateGateway(Guid gatewayId, [FromBody] CreateGatewayRequest gateway)
        {
            if (!_gatewayService.GatewayExists(gatewayId))
            {
                return NotFound("Gateway");
            }

            Gateway gatewayToBeUpdated = _mapper.Map<Gateway>(gateway);
            gatewayToBeUpdated.Id = gatewayId;
            await _gatewayService.UpdateGatewayAsync(gatewayToBeUpdated);
            return Ok();
        }

        /// <summary>
        /// Remove the provided Gateway
        /// </summary>
        /// <param name="gatewayId"></param>
        //[Authorize]
        [HttpDelete("{gatewayId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteGateway(Guid gatewayId)
        {
            Gateway gateway = await _gatewayService.GetGatewayAsync(gatewayId);
            if (gateway == null)
            {
                return NotFound("Gateway");
            }
            await _gatewayService.DeleteGatewayAsync(gateway);
            return NoContent();            
        }


    }
}
