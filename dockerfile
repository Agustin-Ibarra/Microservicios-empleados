# satge 1 build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source 

# copiar el archivo a /source/application_docker
COPY ["App/app.csproj","App/"]
# restaurar los paquetes nuget usando csproj
RUN dotnet restore "App/app.csproj"

# copiar archivos y modulos de la aplicacion
COPY . .

# compilar aplicacion
WORKDIR /source/App

# publicar el proyecto en modo release
RUN dotnet publish -c Release -o /app/publish

# stage 2 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# definir carpeta de trabajo
WORKDIR /app
EXPOSE 80

# copiar los archivos de stage 1
COPY --from=build /app/publish .

# comado de ejecucion
ENTRYPOINT ["dotnet", "App.dll"]