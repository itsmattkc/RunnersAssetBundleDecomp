Shader "Custom/ykStg_dve1_add_t" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "black" {}
 _LitTex ("LightMap (RGB)", 2D) = "black" {}
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
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform mediump vec4 unity_LightmapST;
varying highp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 envVec_2;
  highp vec4 tmpvar_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = _glesMultiTexCoord0.xy;
  tmpvar_4 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  tmpvar_5 = tmpvar_7;
  tmpvar_3 = tmpvar_1;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = normalize(_glesNormal);
  envVec_2.xy = (glstate_matrix_invtrans_modelview0 * tmpvar_8).xy;
  envVec_2.zw = vec2(0.0, 1.0);
  highp mat4 tmpvar_9;
  tmpvar_9[0].x = 0.5;
  tmpvar_9[0].y = 0.0;
  tmpvar_9[0].z = 0.0;
  tmpvar_9[0].w = 0.0;
  tmpvar_9[1].x = 0.0;
  tmpvar_9[1].y = 0.5;
  tmpvar_9[1].z = 0.0;
  tmpvar_9[1].w = 0.0;
  tmpvar_9[2].x = 0.0;
  tmpvar_9[2].y = 0.0;
  tmpvar_9[2].z = 1.0;
  tmpvar_9[2].w = 0.0;
  tmpvar_9[3].x = 0.5;
  tmpvar_9[3].y = 0.5;
  tmpvar_9[3].z = 0.0;
  tmpvar_9[3].w = 1.0;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD2 = (tmpvar_9 * envVec_2).xy;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform sampler2D _EnvTex;
uniform sampler2D _LitTex;
uniform highp vec4 _AmbientColor;
varying highp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
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
  highp vec3 tmpvar_4;
  tmpvar_4 = (c_1.xyz + xlv_COLOR.xyz);
  c_1.xyz = tmpvar_4;
  c_1.xyz = (c_1.xyz + (texture2D (_EnvTex, xlv_TEXCOORD2).xyz * tmpvar_2.w));
  lowp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = c_1.xyz;
  gl_FragData[0] = tmpvar_5;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform mediump vec4 unity_LightmapST;
out highp vec4 xlv_COLOR;
out mediump vec2 xlv_TEXCOORD0;
out mediump vec2 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 envVec_2;
  highp vec4 tmpvar_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = _glesMultiTexCoord0.xy;
  tmpvar_4 = tmpvar_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  tmpvar_5 = tmpvar_7;
  tmpvar_3 = tmpvar_1;
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = normalize(_glesNormal);
  envVec_2.xy = (glstate_matrix_invtrans_modelview0 * tmpvar_8).xy;
  envVec_2.zw = vec2(0.0, 1.0);
  highp mat4 tmpvar_9;
  tmpvar_9[0].x = 0.5;
  tmpvar_9[0].y = 0.0;
  tmpvar_9[0].z = 0.0;
  tmpvar_9[0].w = 0.0;
  tmpvar_9[1].x = 0.0;
  tmpvar_9[1].y = 0.5;
  tmpvar_9[1].z = 0.0;
  tmpvar_9[1].w = 0.0;
  tmpvar_9[2].x = 0.0;
  tmpvar_9[2].y = 0.0;
  tmpvar_9[2].z = 1.0;
  tmpvar_9[2].w = 0.0;
  tmpvar_9[3].x = 0.5;
  tmpvar_9[3].y = 0.5;
  tmpvar_9[3].z = 0.0;
  tmpvar_9[3].w = 1.0;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = tmpvar_3;
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = tmpvar_5;
  xlv_TEXCOORD2 = (tmpvar_9 * envVec_2).xy;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform sampler2D _EnvTex;
uniform sampler2D _LitTex;
uniform highp vec4 _AmbientColor;
in highp vec4 xlv_COLOR;
in mediump vec2 xlv_TEXCOORD0;
in mediump vec2 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
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
  highp vec3 tmpvar_4;
  tmpvar_4 = (c_1.xyz + xlv_COLOR.xyz);
  c_1.xyz = tmpvar_4;
  c_1.xyz = (c_1.xyz + (texture (_EnvTex, xlv_TEXCOORD2).xyz * tmpvar_2.w));
  lowp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = c_1.xyz;
  _glesFragData[0] = tmpvar_5;
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