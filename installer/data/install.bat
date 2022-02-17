@echo off

IF EXIST "%PROGRAMFILES(X86)%" (GOTO 64BIT) ELSE (GOTO 32BIT)

:64BIT
echo install for 64-bit...
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /nologo /codebase %~dp0/HomeAssistantTaskbarWidget.dll
GOTO END


:32BIT
echo install for 32-bit...
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /nologo /codebase %~dp0/HomeAssistantTaskbarWidget.dll
GOTO END

:END