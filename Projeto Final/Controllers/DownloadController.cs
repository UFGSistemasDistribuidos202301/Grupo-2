using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using SaveVault.Models;
using SaveVault.Services;

namespace SaveVault.Controllers;

[ApiController]
[Route("[controller]")]
public class DownloadController : ControllerBase
{
	private readonly ILogger<DownloadController> _logger;
	private readonly IUserService _userService;
	private readonly IGameService _gameService;
	private readonly IDownloadService _downloadService;
	private readonly IConversionService _conversionService;

	public DownloadController(ILogger<DownloadController> logger,
							  IUserService userService,
							  IGameService gameService,
							  IDownloadService downloadService,
							  IConversionService conversionService)
	{
		_logger = logger;
		_userService = userService;
		_gameService = gameService;
		_downloadService = downloadService;
		_conversionService = conversionService;
	}

	[HttpPost("Latest")]
	public async Task<IActionResult> GetLatest([FromBody] QueryDto query)
	{
		try
		{
			User user = _userService.GetById(query.UserId);
			Game game = await _gameService.GetById(query.GameId);

			UniversalSave save = (UniversalSave)await _downloadService.DownloadLatest(game, user);

			if (save == null)
			{
				return NotFound();
			}

			PlatformSave platformSave = _conversionService.Convert(save, query.TargetPlatform);
			SVFile file = _downloadService.CreatePlatformFile(platformSave.Id.ToString(), platformSave.ToFileString());
			return File(file.Content, file.Type, file.Name);
		}
		catch (Exception exception)
		{
			return BadRequest(exception);
		}
	}

	[HttpPost("All")]
	public async Task<IActionResult> GetAll([FromBody] QueryDto query)
	{
		try
		{
			User user = _userService.GetById(query.UserId);
			Game game = await _gameService.GetById(query.GameId);

			IEnumerable<UniversalSave> saves = (await _downloadService.DownloadAllSaves(game, user))
				.Select(s => (UniversalSave)s);

			if (!saves.Any())
			{
				return NotFound();
			}

			using MemoryStream outputStream = new();
			using ZipArchive archive = new(outputStream, ZipArchiveMode.Create, true);

			foreach (UniversalSave save in saves)
			{
				PlatformSave platformSave = _conversionService.Convert(save, query.TargetPlatform);
				SVFile file = _downloadService.CreatePlatformFile(platformSave.Id.ToString(), platformSave.ToFileString());

				ZipArchiveEntry entry = archive.CreateEntry(file.Name);
				using var fileStream = new MemoryStream(file.Content);
				using var entryStream = entry.Open();
				fileStream.CopyTo(entryStream);
			}

			archive.Dispose();

			return File(outputStream.ToArray(), "application/zip", "saves.zip");
		}
		catch (Exception exception)
		{
			return BadRequest(exception);
		}
	}


	[HttpPost("ById")]
	public async Task<IActionResult> ById([FromBody] QueryDto query)
	{
		try
		{
			UniversalSave save = (UniversalSave)await _downloadService.DownloadById(query.SaveId);

			if (save == null)
			{
				return NotFound();
			}

			PlatformSave platformSave = _conversionService.Convert(save, query.TargetPlatform);
			SVFile file = _downloadService.CreatePlatformFile(platformSave.Id.ToString(), platformSave.ToFileString());
			return File(file.Content, file.Type, file.Name);
		}
		catch (Exception exception)
		{
			return BadRequest(exception);
		}
	}
}