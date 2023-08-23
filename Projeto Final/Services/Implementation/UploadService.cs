using SaveVault.Models;
using SaveVault.Repositories;

namespace SaveVault.Services.Implementation;

public class UploadService : IUploadService
{
	private readonly IUploadRepository _uploadRepository;

	public UploadService(IUploadRepository uploadRepository)
	{
		_uploadRepository = uploadRepository;
	}

	public async Task Upload(UniversalSave save) => await _uploadRepository.Upload(save);
}
