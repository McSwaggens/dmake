--
--  ______   _______  _______  _        _______ 
-- (  __  \ (       )(  ___  )| \    /\(  ____ \
-- | (  \  )| () () || (   ) ||  \  / /| (    \/
-- | |   ) || || || || (___) ||  (_/ / | (__    
-- | |   | || |(_)| ||  ___  ||   _ (  |  __)   
-- | |   ) || |   | || (   ) ||  ( \ \ | (      
-- | (__/  )| )   ( || )   ( ||  /  \ \| (____/\
-- (______/ |/     \||/     \||_/    \/(_______/
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