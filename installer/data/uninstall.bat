@echo off

IF EXIST "%PROGRAMFILES(X86)%" (GOTO 64BIT) ELSE (GOTO 32BIT)

:64BIT
echo uninstall for 64-bit...
%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe /nologo /unregister %~dp0/HomeAssistantTaskbarWidget.dll
GOTO ReloadExplorer


:32BIT
echo uninstall for 32-bit...
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe /nologo /unregister %~dp0/HomeAssistantTaskbarWidget.dll
GOTO ReloadExplorer

:ReloadExplorer
taskkill /f /im explorer.exe
start explorer.exe
GOTO END

:END