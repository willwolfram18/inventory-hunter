FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

ARG MSBuildConfiguration=Debug
ARG Version=1.0.0

COPY . "/app"

WORKDIR "/app/WebApi"

RUN dotnet restore
RUN dotnet publish --configuration ${MSBuildConfiguration} -p:Version="${Version}" --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine

COPY --from=build "/app/publish" "/app"

EXPOSE 80
EXPOSE 443

WORKDIR "/app"

ENTRYPOINT [ "dotnet", "SmtpSender.WebApi.dll" ]