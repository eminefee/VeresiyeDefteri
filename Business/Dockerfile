# 1. Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasýný kopyala (ayný dizindeyiz, Business klasörü yok)
COPY ["Business.csproj", "./"]

RUN dotnet restore "Business.csproj"

# Tüm dosyalarý kopyala (Business dizinindeyiz, bu dizindeki her þey)
COPY . .

WORKDIR "/src"

RUN dotnet publish "Business.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Çalýþma ortamý
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Business.dll"]
