#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoord;

uniform float hOffset;
uniform mat4 model;
uniform mat4 projection;
uniform mat4 view;

out vec4 Position;
out vec2 TexCoord;

void main()
{
    vec4 finalPosition = vec4(aPos.x + hOffset, aPos.y, aPos.z, 1.0f);
    gl_Position = projection * view * model * finalPosition;
    Position = finalPosition;
    TexCoord = aTexCoord;
}