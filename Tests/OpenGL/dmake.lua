--  ____  __  __       _        
-- |  _ \|  \/  | __ _| | _____ 
-- | | | | |\/| |/ _` | |/ / _ \
-- | |_| | |  | | |_| |   <  __/
-- |____/|_|  |_|\__,_|_|\_\___|
--
-- This is the default dmake.lua file!
--
-- Global Variables:
--		
--		project:
--			path
--			cxxFlags
--			cFlags
--			linkerFlags
--			libraries
--			files
--



                                                                                                          
function SetupOpenGL ()
	if Linux then
		AddLib("GL")
	elseif Windows then
		AddLib("opengl32")
	elseif OSX then
		AddFlag("-framework OpenGL")
	end
end


AddLib("glfw3")

AddLibIW("gdi32")
AddLibMP("glew")

SetupOpenGL ();