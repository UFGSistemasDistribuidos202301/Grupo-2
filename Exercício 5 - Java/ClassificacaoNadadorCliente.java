import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;
import java.util.Scanner;

public class ClassificacaoNadadorCliente {
    public static void main(String[] args) {
        try {
            Registry registry = LocateRegistry.getRegistry("localhost", 1099);
            ClassificacaoNadadorInterface classificacaoNadador = (ClassificacaoNadadorInterface) registry.lookup("ClassificacaoNadador");

            // Chamada remota do método
            Scanner scanner = new Scanner(System.in);
            System.out.print("Digite a idade do nadador: ");
            int idade = scanner.nextInt();
            scanner.close();

            String resultado = classificacaoNadador.getClassificacao(idade);
            System.out.println(resultado);
            System.exit(0);
        } catch (Exception e) {
            System.err.println("Erro no cliente RMI: " + e.getMessage());
        }
    }
}
