for /f "delims=" %%i in ('dir /ad/s/b') do rmdir /S /Q "%%i\bin"
for /f "delims=" %%i in ('dir /ad/s/b') do rmdir /S /Q "%%i\obj"