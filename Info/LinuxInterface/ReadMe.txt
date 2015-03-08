The i2c.so file can be placed in the same folder as the mono assembly (bin folder) or copied to the /usr/lib/ folder
to be available to all mono assemblies. In some cases you may see DLL File Not Found exceptions, but placing in
/usr/lib should resolve that.

So copy the i2c.so file to /usr/lib/ directory

linux command:

cp i2c.so /usr/lib/

To rebuild the i2c.so file, from shell, "CD" to the location of the files, and run the "make" command.

linux command:

make

"make" will run the build defined in the makefile definition.

Code borrowed from i2ctools, v3.1.0, see Credits.txt and Licence.txt