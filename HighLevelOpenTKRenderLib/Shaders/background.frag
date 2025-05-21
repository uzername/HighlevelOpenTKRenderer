#version 330 core
out vec4 FragColor;
in float vUV;

uniform vec4 color1; // top
uniform vec4 color2; // bottom

void main()
{
    FragColor = mix(color2, color1, vUV); // Blend vertically
}