import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.util.Scanner;

public class Client {
    public static void main(String[] args) {
        System.out.print("\033[H\033[2J");
        System.out.flush();
        Scanner scanner = new Scanner(System.in);

        System.out.println("Digite o número do naipe: [1] - Ouros, [2] - Paus, [3] - Copas, [4] - Espadas");
        int suitNumber = scanner.nextInt();

        System.out
                .println("Digite o valor da carta: [1] - Ás, [2-10] - Números, [11] - Valete, [12] - Dama, [13] - Rei");
        int rankNumber = scanner.nextInt();

        scanner.close();
        try {
            Registry registry = LocateRegistry.getRegistry("rmi-server", 506);
            ICardService cardService = (ICardService) registry.lookup("CardService");

            String cardName = cardService.getCardName(suitNumber, rankNumber);
            System.out.println("A carta é: " + cardName);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}