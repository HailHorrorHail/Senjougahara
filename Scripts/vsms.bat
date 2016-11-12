@echo off
REM Open any projects in the directory

for %%a IN (*.csproj) do (
	echo Opening: %%a
	vsmsbuild %%a
)