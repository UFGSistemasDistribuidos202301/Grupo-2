using Grpc.Core;
using GrpcServer;

namespace GrpcServer.Services;

public class ResultService : Result.ResultBase
{
	private readonly ILogger<ResultService> _logger;
	public ResultService(ILogger<ResultService> logger)
	{
		_logger = logger;
	}

	private float CalculateAverage(Grades grades) => (grades.FirstGrade + grades.SecondGrade) / 2;

	public override Task<ResultResponse> CalculatePartial(ResultRequest request, ServerCallContext context)
	{
		float result = CalculateAverage(request.Grades);

		Status status;
		if (result > 3 && result < 7)
		{
			status = Status.NeedsThirdGrade;
		}
		else
		{
			status = result >= 7
			? Status.Approved
			: Status.Reproved;
		}

		return Task.FromResult(new ResultResponse
		{
			FinalGrade = result,
			Status = status
		});
	}

	public override Task<ResultResponse> CalculateFinal(ResultRequest request, ServerCallContext context)
	{
		float result = CalculateAverage(request.Grades);
		return Task.FromResult(new ResultResponse
		{
			FinalGrade = result,
			Status = result >= 5
			 ? Status.Approved
			 : Status.Reproved
		});
	}
}
