#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["QiitaToWPWeb/TestProject.QiitaToWPWeb.csproj", "QiitaToWPWeb/"]
COPY ["QiitaToWP/TestProject.QiitaToWP.csproj", "QiitaToWP/"]
RUN dotnet restore "QiitaToWPWeb/TestProject.QiitaToWPWeb.csproj"
COPY . .
WORKDIR "/src/QiitaToWPWeb"
RUN dotnet build "TestProject.QiitaToWPWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestProject.QiitaToWPWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestProject.QiitaToWPWeb.dll"]