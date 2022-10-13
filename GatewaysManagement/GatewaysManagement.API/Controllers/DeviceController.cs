using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using GatewaysManagement.Data.Entities;
using GatewaysManagement.Common.DTO.Response;
using GatewaysManagement.Services;
using GatewaysManagement.Services.Utils;
using GatewaysManagement.Common.DTO.Request;

namespace DevicesManagement.API.Controllers
{
    [Route("api/")]
    [ApiController]
    [AllowAnonymous]
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        private readonly IGatewayService _gatewayService;
        private readonly IMapper _mapper;

        public DeviceController(IDeviceService deviceService, IGatewayService gatewayService, IMapper mapper)
        {
            _deviceService = deviceService ?? throw new ArgumentNullException(nameof(deviceService));
            _gatewayService = gatewayService ?? throw new ArgumentNullException(nameof(gatewayService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("device/{deviceId}", Name = "GetDevice")]
        //[Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<DeviceResponse>> GetDevice(Guid deviceId)
        {
            Device device = await _deviceService.GetDeviceAsync(deviceId);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<DeviceResponse>(device));
        }

        [HttpGet("gateway/{gatewayId}/device", Name = "GetDevices")]
        //[Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDevices(Guid gatewayId, [FromQuery] ResourceParameters resourceParameters)
        {
            PageList<Device> result = await _deviceService.GetDevicesAsync(gatewayId, resourceParameters);
            var paginationMetadata = result.GetPaginationData;
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));
            return Ok(_mapper.Map<IEnumerable<DeviceResponse>>(result));
        }

        /// <summary>
        /// Create a new Device.
        /// </summary>
        /// <param name="device">
        /// Create Device Request object. Include SerialNumber, Name, and IPAdress. 
        /// </param>
        [HttpPost("gateway/{gatewayId}/device", Name = "CreateDevice")]
        //[Authorize]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> CreateDevice(Guid gatewayId, [FromBody] CreateDeviceRequest device)
        {
            if (!_gatewayService.GatewayExists(gatewayId))
            {
                return NotFound("Gateway");
            }

            if(await _gatewayService.CountAssociatedDevices(gatewayId) == 10)
            {
                return Conflict("No more that 10 peripheral devices are allowed for a gateway.");
            }

            if (!await _deviceService.ValidUID(device.UID))
            {
                return Conflict("The UID must be unique.");
            }

            Device newDevice = _mapper.Map<Device>(device);
            newDevice.GatewayId = gatewayId;
            await _deviceService.AddDeviceAsync(newDevice);
            return Created("", true);
        }

        /// <summary>
        /// Update Device information. Requires authentication.
        /// </summary>
        /// <param name="device">
        /// Create Device Request object. Include SerialNumber, Name, and IPAdress.
        /// </param>
        //[Authorize]
        [HttpPut("gateway/{gatewayId}/device/{deviceId}", Name = "UpdateDevice")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateDevice(Guid gatewayId, Guid deviceId, [FromBody] CreateDeviceRequest device)
        {
            if (!_deviceService.DeviceExists(deviceId))
            {
                return NotFound("Device");
            }
            if (!_gatewayService.GatewayExists(gatewayId))
            {
                return NotFound("Gateway");
            }

            if (!await _deviceService.ValidUID(device.UID))
            {
                return Conflict("The UID must be unique.");
            }
            Device deviceToBeUpdated = _mapper.Map<Device>(device);
            deviceToBeUpdated.Id = deviceId;
            deviceToBeUpdated.GatewayId = gatewayId;
            await _deviceService.UpdateDeviceAsync(deviceToBeUpdated);
            return Ok();
        }

        /// <summary>
        /// Remove the provided Device
        /// </summary>
        /// <param name="deviceId"></param>
        //[Authorize]
        [HttpDelete("device/{deviceId}", Name = "DeleteDevice")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteDevice(Guid deviceId)
        {
            Device device = await _deviceService.GetDeviceAsync(deviceId);
            if (device == null)
            {
                return NotFound("Device");
            }
            await _deviceService.DeleteDeviceAsync(device);
            return NoContent();            
        }


    }
}
