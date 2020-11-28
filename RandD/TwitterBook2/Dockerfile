FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /app

COPY ["TwitterBook2/*.csproj", "TwitterBook2/"]
COPY ["TwitterBook2.Contracts/*.csproj", "TwitterBook2.Contracts/"]
COPY ["TwitterBook2.Sdk/*.csproj", "TwitterBook2.Sdk/"]
COPY ["TwitterBook2.Sdk.Sample/*.csproj", "TwitterBook2.Sdk.Sample/"]
RUN dotnet restore "TwitterBook2/TwitterBook2.csproj"

COPY . .
RUN dotnet publish ./TwitterBook2/TwitterBook2.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "TwitterBook2.dll"]