g95 -c wrapper4.f
g95 -shared -mrtd -o wrapper4.dll wrapper4.o dassl.dll
