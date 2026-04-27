# 1. Aşama: Build (Derleme)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Proje dosyasını kopyala ve paketleri restore et
COPY ["bebegim.csproj", "./"]
RUN dotnet restore "bebegim.csproj"

# Kalan tüm dosyaları kopyala ve derle
COPY . .
RUN dotnet publish "bebegim.csproj" -c Release -o /app/publish

# 2. Aşama: Runtime (Çalıştırma)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Uygulamanın çalışacağı port
EXPOSE 8080
ENTRYPOINT ["dotnet", "bebegim.dll"]