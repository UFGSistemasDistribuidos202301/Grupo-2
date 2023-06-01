import xmlrpc.client

altura = float(input("Digite a altura (em metros): "))
sexo = input("Digite o sexo (homem/mulher): ")

proxy = xmlrpc.client.ServerProxy("http://localhost:10101/")
peso = proxy.calcular_peso_ideal(altura, sexo)

print("O peso ideal é:", peso)