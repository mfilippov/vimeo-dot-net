@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

REM Build
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe VimeoDotNet.sln /p:Configuration=%config% /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:true /p:BuildInParallel=true /p:RestorePackages=true /t:Clean,Rebuild
if not "%errorlevel%"=="0" goto failure

rd target /Q /S 

mkdir target
mkdir target\package
mkdir target\package\lib
mkdir target\package\lib\net45
mkdir target\package\lib\net45-client
mkdir target\package\lib\net451
mkdir target\package\lib\net451-client
mkdir target\package\lib\net452
mkdir target\package\lib\net452-client
mkdir target\package\lib\net46
mkdir target\package\lib\net46-client

copy LICENSE target\
copy readme.md target\package\

copy src\VimeoDotNet.Net45\bin\Release\VimeoDotNet.dll target\package\lib\net45\
copy src\VimeoDotNet.Net45\bin\Release\VimeoDotNet.pdb target\package\lib\net45\
copy src\VimeoDotNet.Net45\bin\Release\VimeoDotNet.xml target\package\lib\net45\
copy src\VimeoDotNet.Net45\bin\Release\VimeoDotNet.dll target\package\lib\net45-client\
copy src\VimeoDotNet.Net45\bin\Release\VimeoDotNet.pdb target\package\lib\net45-client\
copy src\VimeoDotNet.Net45\bin\Release\VimeoDotNet.xml target\package\lib\net45-client\

copy src\VimeoDotNet.Net451\bin\Release\VimeoDotNet.dll target\\package\lib\net451\
copy src\VimeoDotNet.Net451\bin\Release\VimeoDotNet.pdb target\\package\lib\net451\
copy src\VimeoDotNet.Net451\bin\Release\VimeoDotNet.xml target\\package\lib\net451\
copy src\VimeoDotNet.Net451\bin\Release\VimeoDotNet.dll target\\package\lib\net451-client\
copy src\VimeoDotNet.Net451\bin\Release\VimeoDotNet.pdb target\\package\lib\net451-client\
copy src\VimeoDotNet.Net451\bin\Release\VimeoDotNet.xml target\\package\lib\net451-client\

copy src\VimeoDotNet.Net452\bin\Release\VimeoDotNet.dll target\package\lib\net452\
copy src\VimeoDotNet.Net452\bin\Release\VimeoDotNet.pdb target\package\lib\net452\
copy src\VimeoDotNet.Net452\bin\Release\VimeoDotNet.xml target\package\lib\net452\
copy src\VimeoDotNet.Net452\bin\Release\VimeoDotNet.dll target\package\lib\net452-client\
copy src\VimeoDotNet.Net452\bin\Release\VimeoDotNet.pdb target\package\lib\net452-client\
copy src\VimeoDotNet.Net452\bin\Release\VimeoDotNet.xml target\package\lib\net452-client\

copy src\VimeoDotNet.Net46\bin\Release\VimeoDotNet.dll target\package\lib\net46\
copy src\VimeoDotNet.Net46\bin\Release\VimeoDotNet.pdb target\package\lib\net46\
copy src\VimeoDotNet.Net46\bin\Release\VimeoDotNet.xml target\package\lib\net46\
copy src\VimeoDotNet.Net46\bin\Release\VimeoDotNet.dll target\package\lib\net46-client\
copy src\VimeoDotNet.Net46\bin\Release\VimeoDotNet.pdb target\package\lib\net46-client\
copy src\VimeoDotNet.Net46\bin\Release\VimeoDotNet.xml target\package\lib\net46-client\

%nuget% pack "VimeoDotNet.nuspec" -BasePath target\package -Output target
%nuget% pack "VimeoDotNet.nuspec" -Symbols -BasePath target\package -Output target
if not "%errorlevel%"=="0" goto failure

:success

REM use github status API to indicate commit compile success

exit 0

:failure

REM use github status API to indicate commit compile success

exit -1
