#include "Main.hpp"
#define GLEW_STATIC
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <GL/gl.h>

int main ()
{
	SayHello ();
	
	glewInit ();
	glfwInit ();
	
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
