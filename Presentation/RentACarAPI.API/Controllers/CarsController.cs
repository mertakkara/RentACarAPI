using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentACarAPI.Application.Abstractions.Storage;
using RentACarAPI.Application.Features.Commands.Car.CreateCar;
using RentACarAPI.Application.Features.Commands.Car.DeleteCar;
using RentACarAPI.Application.Features.Commands.Car.UpdateCar;
using RentACarAPI.Application.Features.Commands.CarImageFile.RemoveCarImage;
using RentACarAPI.Application.Features.Commands.CarImageFile.UploadCarImage;
using RentACarAPI.Application.Features.Queries.Car.GetAllCar;
using RentACarAPI.Application.Features.Queries.Car.GetByIdCar;
using RentACarAPI.Application.Features.Queries.CarImageFile.GetCarImages;
using RentACarAPI.Application.Repositories;

using RentACarAPI.Application.ViewModels.Cars;
using RentACarAPI.Domain.Entities;
using System.Net;

namespace RentACarAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        readonly IMediator _mediator;
        public CarsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCarQueryRequest getAllCarQueryRequest)
        {
            GetAllCarQueryResponse getAllCarQueryResponse =  await _mediator.Send(getAllCarQueryRequest);
            return Ok(getAllCarQueryResponse);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Get([FromQuery] GetByIdCarQueryRequest model)
        {
            GetByIdCarQueryResponse getByIdCarQueryResponse = await _mediator.Send(model);
            return Ok(getByIdCarQueryResponse);
         
        }
        [HttpPost]
        public async Task<IActionResult> Post(CreateCarCommandRequest createCarCommandRequest)
        {
            CreateCarCommandResponse response = await _mediator.Send(createCarCommandRequest);
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCarCommandRequest model)
        {
            UpdateCarCommandResponse response = await _mediator.Send(model);
            return Ok();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteCarCommandRequest deleteCarCommandRequest)
        {
            DeleteCarCommandResponse response = await _mediator.Send(deleteCarCommandRequest);
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromQuery] UploadCarImageCommandRequest uploadCarImageCommandRequest)
        {
            uploadCarImageCommandRequest.Files = Request.Form.Files;
            UploadCarImageCommandResponse response = await _mediator.Send(uploadCarImageCommandRequest);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetImages([FromQuery]  GetCarImagesQueryRequest getCarImagesQueryRequest)
        {

            List<GetCarImagesQueryResponse> response = await _mediator.Send(getCarImagesQueryRequest);

            return Ok(response);  
            
           
        
        } 
        [HttpGet("[action]/{Id}")]
        public async Task<IActionResult> DeleteCarImage([ FromRoute] RemoveCarImageCommandRequest removeCarImageCommandRequest,[FromQuery] string imageId)
        {
            removeCarImageCommandRequest.ImageId = imageId;
                RemoveCarImageCommandResponse response = await _mediator.Send(removeCarImageCommandRequest);
            return Ok();
        }
    }
}
