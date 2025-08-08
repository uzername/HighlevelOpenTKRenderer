#version 330 core
layout(lines) in;
layout(triangle_strip, max_vertices = 4) out;

uniform float uThickness;
uniform vec2 uViewportSize;

vec4 offsetPosition(vec4 pos, vec2 offset, vec2 screenSize) {
    vec2 ndcXY = pos.xy / pos.w;
    vec2 offsetNDC = offset / screenSize * 2.0; // convert to NDC
    vec2 newNDC = ndcXY + offsetNDC;
    return vec4(newNDC * pos.w, pos.z, pos.w); // back to clip space
}

void main()
{
    vec4 p0_clip = gl_in[0].gl_Position;
    vec4 p1_clip = gl_in[1].gl_Position;

    // Convert clip space to NDC
    vec2 p0_ndc = p0_clip.xy / p0_clip.w;
    vec2 p1_ndc = p1_clip.xy / p1_clip.w;

    // Compute direction and perpendicular
    //vec2 dir = normalize(p1_ndc - p0_ndc);

    vec2 dir_raw = p1_ndc.xy - p0_ndc.xy;
    float len = length(dir_raw);
    if (len < 1e-6) {
        
        EmitVertex(); EndPrimitive(); return;
    }
    vec2 dir = dir_raw / len;

    vec2 normal = vec2(-dir.y, dir.x);

    // Compute thickness in NDC units
    float thickness_ndc = uThickness / uViewportSize.y * 2.0;
    vec2 offset = normal * thickness_ndc * 0.5;

    // Compute the offset NDC points
    vec2 ndc0a = p0_ndc + offset;
    vec2 ndc0b = p0_ndc - offset;
    vec2 ndc1a = p1_ndc + offset;
    vec2 ndc1b = p1_ndc - offset;

    // Reconstruct clip space positions using original .z and .w
    vec4 clip0a = vec4(ndc0a * p0_clip.w, p0_clip.z, p0_clip.w);
    vec4 clip0b = vec4(ndc0b * p0_clip.w, p0_clip.z, p0_clip.w);
    vec4 clip1a = vec4(ndc1a * p1_clip.w, p1_clip.z, p1_clip.w);
    vec4 clip1b = vec4(ndc1b * p1_clip.w, p1_clip.z, p1_clip.w);

    // Emit triangle strip
    gl_Position = clip0a;
    EmitVertex();
    gl_Position = clip0b;
    EmitVertex();
    gl_Position = clip1a;
    EmitVertex();
    gl_Position = clip1b;
    EmitVertex();

    EndPrimitive();
}
