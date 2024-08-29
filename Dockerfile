# Get base image from Microsoft (full .NET Core SDK) and create working directory where we store the app (Linux container)
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy .csproj file (contains dependencies and specifies build options, configurations) and restore dependencies via NUGET
COPY *.csproj ./
RUN dotnet restore

# Copy all the project files and build the release (out is the folder that contains built application)
COPY . ./
RUN dotnet publish -c Release -o out

# Generate runtime image. We only retrieve aspnet runtime image to keep image "lean". Set the working directory, let container listen on the specified network port at 80,  indicate where to put the output and specifiy the command that should be run when the container starts
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/out .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT [ "dotnet", "CLICommander.dll"]
