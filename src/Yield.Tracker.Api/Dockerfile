#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Yield.Tracker.Api/Yield.Tracker.Api.csproj", "Yield.Tracker.Api/"]
RUN dotnet restore "Yield.Tracker.Api/Yield.Tracker.Api.csproj"
COPY . .
WORKDIR "/src/Yield.Tracker.Api"
RUN dotnet build "Yield.Tracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Yield.Tracker.Api.csproj" -c Release -o /app/publish

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Yield.Tracker.Api.dll"]