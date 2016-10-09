if false then 
	local filename = "./../../../module/cat/proto.lua"
	local fd = io.open(filename, "r")
	local buffer = fd:read("a")
	fd:close()
	local c = {}
	local x = 1

	local b = string.find(buffer, "%[%[")
	local e = string.find(buffer, "%]%]")
	local c2s = string.sub(buffer, b+3, e-2)
	fd = io.open("c2s.sproto", "w")
	fd:write(c2s)
	fd:close()

	b = string.find(buffer, "%[%[", e)
	e = string.find(buffer, "%]%]", b)

	local s2c = string.sub(buffer, b+3, e-2)
	fd = io.open("s2c.sproto", "w")
	fd:write(s2c)
	fd:close()
else 
	local c2s = "./../../../module/ball/proto/proto.c2s.sproto"
	local fd = io.open(c2s)
	local buffer = fd:read("a")
	fd:close()
	fd = io.open("c2s.sproto", "w")
	fd:write(buffer)
	fd:close()

	local s2c = "./../../../module/ball/proto/proto.s2c.sproto"
	local fd = io.open(s2c)
	local buffer = fd:read("a")
	fd:close()
	fd = io.open("s2c.sproto", "w")
	fd:write(buffer)
	fd:close()	
end