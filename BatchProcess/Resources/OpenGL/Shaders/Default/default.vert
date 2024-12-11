#version 330 core
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec3 aColor;
layout(location = 2) in vec2 aTexCoord;

uniform float hOffset;
uniform mat4 transform;

out vec3 OurColor;
out vec4 Position;
out vec2 TexCoord;

void main()
{
    vec4 finalPosition = vec4(aPos.x + hOffset, aPos.y, aPos.z, 1.0f);
    gl_Position = transform * finalPosition;
    OurColor = aColor;
    Position = finalPosition;
    TexCoord = aTexCoord;
}