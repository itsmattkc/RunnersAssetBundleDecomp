Shader "CriMana/Yuv2Rgb" {
Properties {
 _MainTex ("Texture", 2D) = "white" {}
 Texture_y ("Texture Y", 2D) = "white" {}
 Texture_u ("Texture U", 2D) = "white" {}
 Texture_v ("Texture V", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform sampler2D Texture_y;
uniform sampler2D Texture_u;
uniform sampler2D Texture_v;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 color_1;
  lowp vec3 tmpvar_2;
  tmpvar_2.x = (texture2D (Texture_y, xlv_TEXCOORD0).w - 0.06275);
  tmpvar_2.y = (texture2D (Texture_u, xlv_TEXCOORD0).w - 0.50196);
  tmpvar_2.z = (texture2D (Texture_v, xlv_TEXCOORD0).w - 0.50196);
  color_1.xyz = (mat3(1.16438, 1.16438, 1.16438, 0.0, -0.39176, 2.01723, 1.59603, -0.81297, 0.0) * tmpvar_2);
  color_1.w = 1.0;
  gl_FragData[0] = color_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
out mediump vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D Texture_y;
uniform sampler2D Texture_u;
uniform sampler2D Texture_v;
in mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 color_1;
  lowp vec3 tmpvar_2;
  tmpvar_2.x = (texture (Texture_y, xlv_TEXCOORD0).w - 0.06275);
  tmpvar_2.y = (texture (Texture_u, xlv_TEXCOORD0).w - 0.50196);
  tmpvar_2.z = (texture (Texture_v, xlv_TEXCOORD0).w - 0.50196);
  color_1.xyz = (mat3(1.16438, 1.16438, 1.16438, 0.0, -0.39176, 2.01723, 1.59603, -0.81297, 0.0) * tmpvar_2);
  color_1.w = 1.0;
  _glesFragData[0] = color_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
}