using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcClient;

using var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())
{
	HttpVersion = new Version(1, 1)
};

using var channel = GrpcChannel.ForAddress("http://grpc-server:7288", new GrpcChannelOptions
{
	HttpClient = new HttpClient(handler)
});

var client = new Result.ResultClient(channel);

Console.Clear();
Console.WriteLine("Calcule se o aluno está aprovado ou reprovado");

string grade1;
float grade1Parsed;
do
{
	Console.WriteLine("\nInsira a primeira nota:");
	grade1 = Console.ReadLine()!;
}
while (!float.TryParse(grade1, out grade1Parsed));

string grade2;
float grade2Parsed;
do
{
	Console.WriteLine("\nInsira a segunda nota:");
	grade2 = Console.ReadLine()!;
}
while (!float.TryParse(grade2, out grade2Parsed));

var request = new ResultRequest
{
	Grades = new Grades
	{
		FirstGrade = grade1Parsed,
		SecondGrade = grade2Parsed
	}
};

ResultResponse reply = await client.CalculatePartialAsync(request);

ResultResponse finalResult;
if (reply.Status == GrpcClient.Status.NeedsThirdGrade)
{
	string grade3;
	float grade3Parsed;
	do
	{
		Console.WriteLine("\nInsira a terceira nota:");
		grade3 = Console.ReadLine()!;
	}
	while (!float.TryParse(grade3, out grade3Parsed));

	request = new ResultRequest
	{
		Grades = new Grades
		{
			FirstGrade = reply.FinalGrade,
			SecondGrade = grade3Parsed
		}
	};

	finalResult = await client.CalculateFinalAsync(request);
}
else
{
	finalResult = reply;
}

Console.Write("\nO aluno está ");
Console.ForegroundColor = finalResult.Status == GrpcClient.Status.Approved ? ConsoleColor.Green : ConsoleColor.Red;
Console.Write(finalResult.Status == GrpcClient.Status.Approved ? "aprovado" : "reprovado");
Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine(" com nota " + finalResult.FinalGrade.ToString() + ".");
Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey(true);