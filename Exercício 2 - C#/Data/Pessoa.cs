namespace DataLib;

public enum Sex
{
	Masculine,
	Feminine
}

public class Person
{
	public string? Name { get; set; }
	public int Age { get; set; }
	public Sex Sex { get; set; }

	public int CalculateRemainingYearsUntilAdulthood()
	{
		int anosRestantes = Sex == Sex.Masculine
		 ? 18 - Age
		 : 21 - Age;

		return anosRestantes < 0 ? 0 : anosRestantes;
	}
	public bool ReachedAdulthood() => CalculateRemainingYearsUntilAdulthood() == 0;
}
