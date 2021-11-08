#version 330 core
in vec3 aPosition;
in vec3 aColor;
out vec3 vertexColor;
out vec2 vertexCoord;
uniform mat4 u_model;
uniform mat4 u_view;
uniform mat4 u_projection;

void main()
{
    gl_Position  = vec4(aPosition, 1.0) * u_model * u_view * u_projection;
    vertexColor = aColor;
    vertexCoord=gl_Position.xy;
}