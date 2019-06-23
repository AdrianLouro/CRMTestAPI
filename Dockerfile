FROM microsoft/dotnet:2.2-sdk as build-image
WORKDIR /home/app
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done 
RUN dotnet restore
COPY . .
#RUN dotnet test --verbosity=normal ./Tests/Tests.csproj
RUN dotnet publish ./CRMTestAPI/CRMTestAPI.csproj -o /publish/

FROM microsoft/dotnet:2.2.0-aspnetcore-runtime
WORKDIR /publish
COPY ./Temp/cert-aspnetcore.pfx /root/.dotnet/https/ 
COPY --from=build-image /publish .
ENTRYPOINT ["dotnet", "CRMTestAPI.dll"]