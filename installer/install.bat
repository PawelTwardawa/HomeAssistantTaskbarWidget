@echo off

rem installation path
set filePath="%PROGRAMFILES%\HA Taskbar Widget\"
set srcPath=".\data"

rem Check permissions
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Administrative permissions confirmed.
    GOTO CreateDirectory
) else (
    echo Please run this script with administrator permissions.
	pause
    goto END
)

:CreateDirectory
echo Creating directory...
if not exist %filePath% (
    mkdir %filePath%
    GOTO CopyFiles
) else (
    echo Directory exist
    goto END
)

:CopyFiles
echo Copying files...
xcopy /s %srcPath% %filePath%
echo Copy completed
goto install

:install
cd %filePath%
call .\install.bat
goto END

:END
echo Installation completed