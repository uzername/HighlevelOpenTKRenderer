#version 330 core
// Geometry shader accepts lines and emits triangle_strip. Such move makes line look not 1px wide but as much px wide as you need
// geometry shader. good for world-space lines with screen space thickness.
// This method ensures lines appear the same width no matter how far the camera is.
layout(lines) in;
layout(triangle_strip, max_vertices = 4) out;

uniform mat4 uProjection;
uniform mat4 uView;
uniform mat4 uModel;

// uThickness is the line width in pixels (e.g. 2.0, 4.0).
uniform float uThickness; // in pixels
// uViewportSize must be set manually on each frame (e.g. (800, 600))
uniform vec2 uViewportSize; // viewport size in pixels

void main()
{
    // Convert clip-space, calculated by vertex shader, to NDC (Normalized Device Coordinates , from -1 to +1)
    vec4 p0 = gl_in[0].gl_Position;
    vec4 p1 = gl_in[1].gl_Position;

    vec2 ndc0 = p0.xy / p0.w;
    vec2 ndc1 = p1.xy / p1.w;

    // Convert to screen space
    vec2 screen0 = ndc0 * 0.5 * uViewportSize;
    vec2 screen1 = ndc1 * 0.5 * uViewportSize;

    // Compute the direction and perpendicular
    vec2 dir = normalize(screen1 - screen0);
    vec2 normal = vec2(-dir.y, dir.x);

    // Compute offset
    vec2 offset = (uThickness * 0.5) * normal;

    // Create quad in screen space
    vec2 s0a = screen0 + offset;
    vec2 s0b = screen0 - offset;
    vec2 s1a = screen1 + offset;
    vec2 s1b = screen1 - offset;

    // Convert back to NDC
    vec2 ndc0a = (s0a / (0.5 * uViewportSize));
    vec2 ndc0b = (s0b / (0.5 * uViewportSize));
    vec2 ndc1a = (s1a / (0.5 * uViewportSize));
    vec2 ndc1b = (s1b / (0.5 * uViewportSize));

    float z0 = p0.z / p0.w;
    float z1 = p1.z / p1.w;

    // Emit 4 vertices
    gl_Position = vec4(ndc0a, z0, 1.0);
    EmitVertex();

    gl_Position = vec4(ndc0b, z0, 1.0);
    EmitVertex();

    gl_Position = vec4(ndc1a, z1, 1.0);
    EmitVertex();

    gl_Position = vec4(ndc1b, z1, 1.0);
    EmitVertex();

    EndPrimitive();
}