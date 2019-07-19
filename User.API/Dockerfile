FROM  microsoft/aspnetcore-build as build-env
# code目录
WORKDIR /code  

#项目拷贝至code
COPY *.csproj ./ 
RUN dotnet restore

# #代码拷贝至codde
COPY  . ./
# #发布文件在code/out文件夹
RUN dotnet publish -c Release -o out
# #找到runtime
FROM microsoft/aspnetcore
# #新建一个目录app
WORKDIR /app
# #code目录发布的代码文件放到app
COPY --from=build-env /code/out ./

EXPOSE 80
ENTRYPOINT [ "dotnet","User.API.dll" ]