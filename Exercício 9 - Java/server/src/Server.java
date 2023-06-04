import java.rmi.registry.Registry;
import java.net.InetAddress;
import java.rmi.registry.LocateRegistry;

public class Server {
    public static void main(String[] args) {
        System.out.print("\033[H\033[2J");
        System.out.flush();

        try {
            ICardService cardService = new CardService();
            Registry registry = LocateRegistry.createRegistry(506);
            registry.bind("CardService", cardService);
            System.out.println("Aguardando invocação remota em " + InetAddress.getLocalHost().getHostAddress() + "...");
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
