using SaveVault.Models;

namespace SaveVault.Repositories;

public interface IUploadRepository
{
	Task Upload(UniversalSave save);
}