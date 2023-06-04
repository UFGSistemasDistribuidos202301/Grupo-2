import java.rmi.RemoteException;
import java.rmi.server.UnicastRemoteObject;
import java.util.Scanner;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

public class ClassificacaoNadador extends UnicastRemoteObject implements ClassificacaoNadadorInterface {
    public ClassificacaoNadador() throws RemoteException {
        super();
    }

    public String getClassificacao(int idade) throws RemoteException {
        System.out.println("Idade recebida pelo servidor: " + idade);

        String classificacao = "";

        if (idade >= 5 && idade <= 7) {
            classificacao = "Infantil A";
        } else if (idade >= 8 && idade <= 10) {
            classificacao = "Infantil B";
        } else if (idade >= 11 && idade <= 13) {
            classificacao = "Juvenil A";
        } else if (idade >= 14 && idade <= 17) {
            classificacao = "Juvenil B";
        } else if (idade >= 18) {
            classificacao = "Adulto";
        }

        if (!classificacao.isEmpty()) {
            return "O nadador se enquadra na classificação: " + classificacao;
        } else {
            return "Idade inválida.";
        }
    }

    public static void main(String[] args) {
        try {
            ClassificacaoNadador classificacaoNadador = new ClassificacaoNadador();
            System.out.println("Servidor RMI pronto.");
            Registry registry = LocateRegistry.createRegistry(1099);
            registry.rebind("ClassificacaoNadador", classificacaoNadador);

            System.out.println("Serviço registrado no registro RMI.");

            // Configurar o servidor RMI aqui

        } catch (Exception e) {
            System.err.println("Erro no servidor RMI: " + e.getMessage());
        }
    }
}
