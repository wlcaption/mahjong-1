package.path = "./../3rd/sproto-Csharp/tools/?.lua;" .. package.path
dofile("mk_sproto.lua")
os.execute("cd ./../3rd/sproto-Csharp/tools && ./lua sprotodump.lua -cs ./../../../tool/c2s.sproto -d ./../../../tool/ -p c2s")
os.execute("cd ./../3rd/sproto-Csharp/tools && ./lua sprotodump.lua -cs ./../../../tool/s2c.sproto -d ./../../../tool/ -p s2c")