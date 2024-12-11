#version 330 core
out vec4 FragColor;

in vec4 OurColor;
in vec4 Position;
in vec2 TexCoord;

uniform float blend;

uniform sampler2D tex1;
uniform sampler2D tex2;

void main()
{
    FragColor = mix(texture(tex1, TexCoord), texture(tex2, vec2(TexCoord.x, TexCoord.y)), blend);
}