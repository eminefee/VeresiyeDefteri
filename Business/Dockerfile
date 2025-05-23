# 1. Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Buisness/Buisness.csproj", "Buisness/"]
RUN dotnet restore "Buisness/Buisness.csproj"
COPY . .
WORKDIR "/src/Buisness"
RUN dotnet publish "Buisness.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Çalýþma ortamý
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Buisness.dll"]
