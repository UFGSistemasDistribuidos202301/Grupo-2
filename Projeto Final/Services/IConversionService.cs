using SaveVault.Models;

namespace SaveVault.Services;

public interface IConversionService
{
	UniversalSave Convert(PlatformSave save);
	PlatformSave Convert(UniversalSave save, Platform platform);
	Task<T> ConvertFromFile<T>(IFormFile file) where T : ISave;
	Task<T> ConvertFromFile<T>(MemoryStream file) where T : ISave;
}