#version 330 core
// a simple fragment shader that just passes plain color to a line
out vec4 FragColor;

uniform vec4 uColor;

void main()
{
    FragColor = uColor;
}