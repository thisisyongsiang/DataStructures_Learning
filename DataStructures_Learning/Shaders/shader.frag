#version 330 core
in vec3 vertexColor;
out vec4 RragColor;

uniform vec4 u_Color;

void main()
{
    RragColor =vec4(vertexColor.rgb,0);
}