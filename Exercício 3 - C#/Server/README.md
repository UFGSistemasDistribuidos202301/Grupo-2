#gRPC Server

-   Criar uma network com `docker network create [nome-network]`;
-   Criar container com `docker build --rm -t [nome-imagem]`;
-   Rodar com `docker run --rm --network [nome-network] --hostname grpc-client -p 7288:7288 [nome-imagem]`.
