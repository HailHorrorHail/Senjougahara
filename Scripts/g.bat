@echo off
setlocal EnableDelayedExpansion
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set "DEL=%%a"
)

rem Prepare a file "X" with only one dot
<nul > X set /p ".=."

REM Gitalize gateway tool to Gerards scripts

for /f "delims=" %%i in ('git branch --list master') DO (
	IF "%%i" == "* master" (
		call :color E4 "Careful you are in the Master Branch"
		echo.
		echo.
	)
)

IF [%1] == [h] (
	echo This will run git commands naturally. These commands are modified for your pleasure:
	
	echo pom ^-^> pull origin master
	echo b ^-^> branch
	echo n ^-^> create and enter new branch
	echo j ^-^> jump into branch
	echo pc ^-^> push change and then open codeflow
) ELSE IF [%1] == [pom] (
	git pull origin master:master
) ELSE IF [%1] == [b] (
	git branch
) ELSE IF [%1] == [pc] (	
	git push & git cf
) ELSE IF [%1] == [n] (
	git checkout master
	git branch
	git checkout -b "gjwoods/%2"
	git branch
) ELSE IF [%1] == [j] (
	git checkout "%2"
	git branch
) ELSE IF [%1] == [d] (
	git diff master "%2"
) ELSE IF [%1] == [-] (
	REM
) ELSE (
	git %*
)

exit /b

:color
set "param=^%~2" !
set "param=!param:"=\"!"
findstr /p /A:%1 "." "!param!\..\X" nul
<nul set /p ".=%DEL%%DEL%%DEL%%DEL%%DEL%%DEL%%DEL%"
exit /b