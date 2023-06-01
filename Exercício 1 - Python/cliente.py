import socket

cliente = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
endereco_servidor = ('localhost', 10101)

try:
    cliente.connect(endereco_servidor)
    
    nome = input("Digite o nome do funcionário: ")
    cargo = input("Digite o cargo do funcionário (operador ou programador): ")
    salario = float(input("Digite o salário do funcionário: "))

    dados = f"{nome};{cargo};{salario}"
    cliente.send(dados.encode())

    resposta = cliente.recv(1024).decode()

    print(resposta)

except ConnectionRefusedError:
    print("Não foi possível conectar ao servidor.")

finally:
    cliente.close()