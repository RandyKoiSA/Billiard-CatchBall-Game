echo First removing old binary files
rm *.dll
rm *.exe
echo view the list of source files
ls -ls
echo Compile billiardlogic.cs to create the file
mcs -target:library billiardlogic.cs -r:System.Drawing.dll -out:billiardlogic.dll

echo Compile billiardframe.cs to create the file
mcs -target:library billiardframe.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:billiardlogic.dll -out:billiardframe.dll
echo Compile billiardmain.cs to create the exe
mcs billiardmain.cs -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:billiardframe.dll -r:billiardlogic.dll -out:billiardmain.exe

./billiardmain.exe
echo the scripe has terminated