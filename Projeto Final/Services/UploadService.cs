using System.Text;

public class UploadService : IUploadService
{
	private readonly IUploadRepository _uploadRepository;

	public UploadService(IUploadRepository uploadRepository)
	{
		_uploadRepository = uploadRepository;
	}

	public void Upload(UniversalSave save)
	{
		var saveString = SaveIOHelper.Deserialize(save);
		SaveIOHelper.WriteUniversalFile(saveString);

		//Passar pro arquivo aqui e alterar pra subir o arquivo pro banco
		_uploadRepository.Upload(save);
	}
}
