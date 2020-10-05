Shader "Custom/ykStg_du_t" {
Properties {
 _MainTex ("テクスチャ", 2D) = "white" {}
 _LitTex ("LightMap (RGB)", 2D) = "black" {}
 _ScrollingSpeed ("UVスクロール速度", Vector) = (0,0,0,0)
 _AmbientColor ("AmbientColor", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ScrollingSpeed;
uniform mediump vec4 unity_LightmapST;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1 = tmpvar_3;
  highp vec2 tmpvar_4;
  tmpvar_4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  tmpvar_2 = tmpvar_4;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _LitTex;
uniform highp vec4 _AmbientColor;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (_LitTex, xlv_TEXCOORD1).xyz));
  highp vec3 tmpvar_3;
  tmpvar_3 = (c_1.xyz * _AmbientColor.xyz);
  c_1.xyz = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ScrollingSpeed;
uniform mediump vec4 unity_LightmapST;
out mediump vec2 xlv_TEXCOORD0;
out mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = (_glesMultiTexCoord0 + fract((_ScrollingSpeed * _Time.y))).xy;
  tmpvar_1 = tmpvar_3;
  highp vec2 tmpvar_4;
  tmpvar_4 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  tmpvar_2 = tmpvar_4;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _LitTex;
uniform highp vec4 _AmbientColor;
in mediump vec2 xlv_TEXCOORD0;
in mediump vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  c_1.w = tmpvar_2.w;
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture (_LitTex, xlv_TEXCOORD1).xyz));
  highp vec3 tmpvar_3;
  tmpvar_3 = (c_1.xyz * _AmbientColor.xyz);
  c_1.xyz = tmpvar_3;
  _glesFragData[0] = c_1;
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