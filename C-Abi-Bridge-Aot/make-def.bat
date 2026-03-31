@echo off
set DLL=C-Abi-Bridge.Aot.dll
set DEF=C-Abi-Bridge.Aot.def

echo LIBRARY %DLL% > %DEF%
echo EXPORTS >> %DEF%

for /f "tokens=4" %%A in ('dumpbin /exports %DLL% ^| findstr /R "^[ ]*[0-9][ ]*[0-9A-F][ ]*[0-9A-F]"') do (
    echo %%A >> %DEF%
)
