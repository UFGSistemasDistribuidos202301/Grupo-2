using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace SaveVault.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
	private readonly ILogger<UploadController> _logger;
	private readonly IUploadService _uploadService;
	private readonly IConversionService _conversionService;

	public UploadController(ILogger<UploadController> logger,
							IUploadService uploadService,
							IConversionService conversionService)
	{
		_logger = logger;
		_uploadService = uploadService;
		_conversionService = conversionService;
	}

	[HttpPost]
	public IActionResult Upload(IFormFile file)
	{
		try
		{
			var save = _conversionService.ConvertFromFile<PlatformSave>(file);
			var universalSave = _conversionService.Convert<PlatformSave, UniversalSave>(save);
			_uploadService.Upload(universalSave);
			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(exception);
		}
	}
}