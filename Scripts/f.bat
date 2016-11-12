@echo off
REM Finalize gateway tool to Gerards scripts

IF [%1] == [] (
	echo Scripts:
	REM dir e:\Tools\Scripts /B -- need to relative path this
	echo Jumps: mt, v8, v9, mc, eds, nds, tfun, tlmc, tlapi, mp, scp, jp, db, dbg, ui
) ELSE IF [%1] == [scp] (
	cd %INETROOT%\private\ClientCenter\MT\Source\Tools\Scope\ScopeSamples
) ELSE (
REM need to relative path this as well
	IF EXIST e:\Tools\Scripts\%1.pl (
		perl e:\Tools\Scripts\%1.pl %cd% %2 %3 %4 %5 %6 %7 %8 %9
	) ELSE IF EXIST e:\Tools\Scripts\%1.ps1 (
		Powershell e:\Tools\Scripts\%1.ps1 %2 %3 %4 %5 %6 %7 %8 %9
	) ELSE (
		%1 "%cd%" %2 %3 %4 %5 %6 %7 %8 %9
	)
)