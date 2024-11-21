FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FungEyeApi.csproj", "./"]
RUN dotnet restore "FungEyeApi.csproj"

COPY . .

RUN dotnet build "FungEyeApi.csproj" -c Release -o /app/build
RUN dotnet publish "FungEyeApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FungEyeApi.dll"]