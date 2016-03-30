@echo off
IF NOT "%1"=="" PUSHD "%1"
del source.7z > nul
7z.exe a -mx9 Source.7z @files.lst 
IF NOT "%1"=="" POPD