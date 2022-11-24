using Microsoft.AspNetCore.Mvc;
using RentACarAPI.Application.Repositories;
using RentACarAPI.Application.RequestParameters;
using RentACarAPI.Application.Services;
using RentACarAPI.Application.ViewModels.Cars;
using RentACarAPI.Domain.Entities;
using System.Net;

namespace RentACarAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ICarWriteRepository _carWriteService;
        private readonly ICarReadRepository _carReadService;
        private readonly IOrderReadRepository _orderReadService;
        private readonly IOrderWriteRepository _orderWriteService;
        private readonly ICustomerReadRepository _customerReadService;
        private readonly ICustomerWriteRepository _customerWriteervice;
        private readonly IFileService _fileService;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        
        private readonly ICarImageFileReadRepository _carReadRepository;
        private readonly ICarImageFileWriteRepository _carWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceWriteRepository;


        public CarsController(IFileService fileService, IWebHostEnvironment webHostEnvironment, ICarWriteRepository carWriteService, ICarReadRepository carReadService, IOrderReadRepository orderReadService, IOrderWriteRepository orderWriteService, ICustomerReadRepository customerReadService, ICustomerWriteRepository customerWriteervice, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, ICarImageFileReadRepository carReadRepository, ICarImageFileWriteRepository carWriteRepository, IInvoiceFileReadRepository invoiceReadRepository, IInvoiceFileWriteRepository invoiceWriteRepository)
        {
            this.webHostEnvironment = webHostEnvironment;
            _carWriteService = carWriteService;
            _carReadService = carReadService;
            _orderReadService = orderReadService;
            _orderWriteService = orderWriteService;
            _customerReadService = customerReadService;
            _customerWriteervice = customerWriteervice;
            _fileService = fileService;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _carReadRepository = carReadRepository;
            _carWriteRepository = carWriteRepository;
            _invoiceReadRepository = invoiceReadRepository;
            _invoiceWriteRepository = invoiceWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            var totalCount = _carReadService.GetAll(false).Count(); 
             var cars = _carReadService.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDateTime, 
                p.UpdatedDate
            }
            );

            return Ok(new
            {
                totalCount,
                cars
            });



        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _carReadService.GetByIdAsync(id, false));
        }
        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Car model)
        {
            await _carWriteService.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,

            });
            await _carWriteService.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }
        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Car model)
        {
            Car car = await _carReadService.GetByIdAsync(model.Id);
            car.Stock = model.Stock;
            car.Name = model.Name;
            car.Price = model.Price;
            await _carWriteService.SaveAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _carWriteService.RemoveAsync(id);
            await _carWriteService.SaveAsync();
            return Ok();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            var datas = await _fileService.UploadAsync("resource/car-images", Request.Form.Files);

            await _carWriteRepository.AddRangeAsync(datas.Select(d => new CarImageFile()
            {
                FileName = d.filename,
                Path = d.path

            }).ToList());
            await _carWriteRepository.SaveAsync();


            //Random r = new ();
            //string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, "resource/car-images"); //wwwroot/resource/car-images

            //if(!Directory.Exists(uploadPath))
            //    Directory.CreateDirectory(uploadPath);  
            //foreach (IFormFile item in Request.Form.Files)
            //{
            //    string fullPath = Path.Combine(uploadPath, $"{r.Next() }{Path.GetExtension(item.FileName)}");
            //    using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None,1024*1024,useAsync: false);
            //    await item.CopyToAsync(fileStream);
            //    await fileStream.FlushAsync();
            //}

            return Ok();
        }
    }
}
