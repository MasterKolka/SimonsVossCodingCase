##########################################################
# build .NET Core app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ./SimonsVossCodingCase.LicenseSignatureGenerator.csproj .
RUN dotnet restore "SimonsVossCodingCase.LicenseSignatureGenerator.csproj"
COPY . .
RUN dotnet publish "SimonsVossCodingCase.LicenseSignatureGenerator.csproj" -c Release -r linux-x64 -o /app --self-contained true /p:PublishTrimmed=true
RUN dotnet tool install --global dotnet-dev-certs &&\
    dotnet dev-certs https --clean &&\
    dotnet dev-certs https -ep /app/https/aspnetapp.pfx -p "pw!@#344123123asdQ"


##########################################################
# create runtime image from self-contained trimmed app
FROM mcr.microsoft.com/dotnet/core/runtime-deps:3.1 AS final
EXPOSE 443
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=https://+:443
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="pw!@#344123123asdQ"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/https/aspnetapp.pfx
ENTRYPOINT ["./SimonsVossCodingCase.LicenseSignatureGenerator"]