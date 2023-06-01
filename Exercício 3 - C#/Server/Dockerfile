FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build as publish
RUN dotnet publish -c Release -o /app/publish

FROM publish as final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 7288
ENTRYPOINT ["dotnet", "GrpcServer.dll"]
