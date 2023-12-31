﻿using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using IronOcr;
using System.Numerics;
using WebApi.Entities;
using static IronSoftware.Drawing.AnyBitmap;
using System.Drawing;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService dataService;

        public DataController(IDataService dataService)
        {
            this.dataService = dataService;
        }
        // GET: api/<DataController>
        [HttpGet]
        public ActionResult<List<Data>> Get()
        {
            return dataService.Get();
        }

        // GET api/<DataController>/5
        [HttpGet("{id}")]
        public ActionResult<Data> Get(string id)
        {
            var data = dataService.Get(id);
            if (data == null)
            {
                return NotFound("id not found");
            }
            return data;
        }

        // POST api/<DataController>
        [HttpPost]
        public ActionResult<Data> Post([FromBody] Data data)
        {
            dataService.Create(data);
            return CreatedAtAction(nameof(Get), new { id = data.Id }, data);
        }

        [HttpPost("upload-image")]
        public IActionResult UploadImage([FromForm] IFormFile file)
        {
            // Check if the file is valid
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            // Process the file and save it to a location
            // Replace 'filePath' with the actual path where the image will be saved
            var filePath = Path.Combine("C:\\ATOM-OCR\\Atom.OCR\\WebApi\\images", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return the image URL or path
            var imageUrl = "C:\\ATOM-OCR\\Atom.OCR\\WebApi\\images\\" + file.FileName; // Replace with your actual base URL
            return Ok(new { imageUrl });
        }

        [HttpPost("extract-text")]
        [Consumes("multipart/form-data")]
        public IActionResult ExtractText([FromForm] IFormFile file) 
        {

            // Check if the file is valid
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            var filePath = Path.Combine("C:\\ATOM-OCR\\Atom.OCR\\WebApi\\images", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            // Return the image URL or path
            var imageUrl = "C:\\ATOM-OCR\\Atom.OCR\\WebApi\\images\\" + file.FileName; // Replace with your actual base URL

            var ocr = new IronTesseract();

            using (var ocrInput = new OcrInput())
            {
                ocrInput.AddImage(imageUrl);
                // Optionally Apply Filters if needed:
                // ocrInput.Deskew();  // use only if image not straight
                // ocrInput.DeNoise(); // use only if image contains digital noise
                var ocrResult = ocr.Read(ocrInput);

                byte[] imageBytes = System.IO.File.ReadAllBytes(imageUrl);
                string base64Image = Convert.ToBase64String(imageBytes);

                // Construct the data URI
                var dataUri = $"data:image/png;base64,{base64Image}";

                return Ok(new { Text = ocrResult.Text, image_data_uri = dataUri, imageurl = imageUrl });

            }

        }

        // PUT api/<DataController>/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Data data)
        {
            var existingData = dataService.Get(id);
            if (existingData == null)
            {
                return NotFound("id not found");
            }
            dataService.Update(id, data);
            return NoContent();
        }

        // DELETE api/<DataController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var data = dataService.Get(id);
            if (data == null)
            {
                return NotFound("id not found");
            }
            dataService.Remove(data.Id);
            return Ok("entry deleted");
        }
    }
}
