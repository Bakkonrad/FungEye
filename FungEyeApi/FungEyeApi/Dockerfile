FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["FungEyeApi.csproj", "./"]
RUN dotnet restore "FungEyeApi.csproj"

COPY . .

COPY Resources /app/Resources
COPY secrets.json /app/secrets.json

RUN dotnet build "FungEyeApi.csproj" -c Release -o /app/build
RUN dotnet publish "FungEyeApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=build /app/Resources ./Resources
COPY --from=build /app/secrets.json .
ENTRYPOINT ["dotnet", "FungEyeApi.dll"]