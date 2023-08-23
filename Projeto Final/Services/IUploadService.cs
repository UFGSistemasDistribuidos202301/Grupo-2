using SaveVault.Models;

namespace SaveVault.Services;

public interface IUploadService
{
	Task Upload(UniversalSave save);
}