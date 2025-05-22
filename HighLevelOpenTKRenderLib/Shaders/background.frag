#version 330 core

in float vUV;
out vec4 FragColor;

uniform vec4 color1;
uniform vec4 color2;

void main()
{
    FragColor = mix(color2, color1, vUV);
}