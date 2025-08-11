#version 330 core

layout(lines) in;
layout(triangle_strip, max_vertices = 4) out;

uniform float uThickness;      // thickness in pixels
uniform vec2 uViewportSize;    // (width, height) in pixels

// Convert a position in clip space to a new position offset by 'offset' in pixels
vec4 offsetPosition(vec4 clipPos, vec2 offsetPixels) {
    // Convert clip space to NDC
    vec2 ndc = clipPos.xy / clipPos.w;

    // Convert pixel offset to NDC offset
    vec2 offsetNDC = (offsetPixels / uViewportSize) * 2.0;

    // Apply offset in NDC
    vec2 newNDC = ndc + offsetNDC;

    // Convert back to clip space
    return vec4(newNDC * clipPos.w, clipPos.z, clipPos.w);
}

void main()
{
    vec4 p0 = gl_in[0].gl_Position;
    vec4 p1 = gl_in[1].gl_Position;

    // Direction in NDC space
    vec2 p0_ndc = p0.xy / p0.w;
    vec2 p1_ndc = p1.xy / p1.w;
    vec2 dir = normalize(p1_ndc - p0_ndc);
    vec2 normal = vec2(-dir.y, dir.x); // perpendicular

    // Half thickness in pixels
    float halfThickness = uThickness * 0.5;

    // Emit vertices as a quad (triangle strip)
    gl_Position = offsetPosition(p0,  normal * halfThickness);
    EmitVertex();

    gl_Position = offsetPosition(p0, -normal * halfThickness);
    EmitVertex();

    gl_Position = offsetPosition(p1,  normal * halfThickness);
    EmitVertex();

    gl_Position = offsetPosition(p1, -normal * halfThickness);
    EmitVertex();

    EndPrimitive();
}