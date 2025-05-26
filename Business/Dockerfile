# 1. Build aþamasý
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Business/Business.csproj", "Business/"]
RUN dotnet restore "Business/Business.csproj"
COPY . .
WORKDIR "/src/Business"
RUN dotnet publish "Business.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Çalýþma ortamý
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Business.dll"]
