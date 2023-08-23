using Microsoft.AspNetCore.Mvc;
using SaveVault.Models;
using SaveVault.Services;

namespace SaveVault.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
	private readonly ILogger<UploadController> _logger;
	private readonly IUploadService _uploadService;
	private readonly IConversionService _conversionService;
	private readonly IPublishService _publishService;

	public UploadController(ILogger<UploadController> logger,
							IUploadService uploadService,
							IConversionService conversionService,
							IPublishService publishService)
	{
		_logger = logger;
		_uploadService = uploadService;
		_conversionService = conversionService;
		_publishService = publishService;
	}

	[HttpPost]
	public async Task<IActionResult> Upload([FromForm] List<IFormFile> files)
	{
		try
		{
			if (!files.Any())
			{
				return BadRequest();
			}

			foreach (IFormFile file in files)
			{
				PlatformSave save = await _conversionService.ConvertFromFile<PlatformSave>(file);
				UniversalSave universalSave = _conversionService.Convert(save);
				await _uploadService.Upload(universalSave);
			}

			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(exception);
		}
	}

	[HttpPost("UploadWithSync")]
	public async Task<IActionResult> UploadWithSync([FromForm] List<IFormFile> files)
	{
		try
		{
			if (!files.Any())
			{
				return BadRequest();
			}

			foreach (IFormFile file in files)
			{
				PlatformSave save = await _conversionService.ConvertFromFile<PlatformSave>(file);
				UniversalSave universalSave = _conversionService.Convert(save);
				await _uploadService.Upload(universalSave);
				_publishService.PublishMessage(universalSave);
			}

			return Ok();
		}
		catch (Exception exception)
		{
			return BadRequest(exception);
		}
	}
}