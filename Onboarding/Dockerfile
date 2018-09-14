FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 51453
EXPOSE 44360

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /Onboarding
COPY *.csproj ./
RUN dotnet restore 
COPY . .
WORKDIR /Onboarding
RUN dotnet build Onboarding.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Onboarding.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Onboarding.dll"]
