public enum Rank {
	ACE(1, "Ás"),
	TWO(2, "Dois"),
	THREE(3, "Três"),
	FOUR(4, "Quatro"),
	FIVE(5, "Cinco"),
	SIX(6, "Seis"),
	SEVEN(7, "Sete"),
	EIGHT(8, "Oito"),
	NINE(9, "Nove"),
	TEN(10, "Dez"),
	JACK(11, "Valete"),
	QUEEN(12, "Dama"),
	KING(13, "Rei");

	private final int number;
	public final String description;

	Rank(int number, String description) {
		this.number = number;
		this.description = description;
	}

	public static Rank get(int number) {
		for (Rank rank : Rank.values()) {
			if (rank.number == number) {
				return rank;
			}
		}
		throw new IllegalArgumentException("No rank found for number: " + number);
	}
}