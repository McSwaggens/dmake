#define GLEW_STATIC
#include <GL/glew.h>
#include <GLFW/glfw3.h>

#if __APPLE__
	#include <OpenGL/gl.h>
#else
	#include <GL/gl.h>
#endif

int main ()
{
	if (!glewInit () || !glfwInit ())
	{
		return 0;
	}
	
	GLFWwindow* window = glfwCreateWindow (1920, 1080, "Hello World", NULL, NULL);
	glfwMakeContextCurrent (window);
	
	while (!glfwWindowShouldClose(window))
	{
		glfwPollEvents ();
		
		glClear(GL_COLOR_BUFFER_BIT);
		glClearColor (0.2f, 0.2f, 0.5f, 1.0f);
		
		glfwSwapBuffers (window);
	}
	
	glfwTerminate();
	
	return 0;
}
