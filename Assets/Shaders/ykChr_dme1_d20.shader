Shader "Custom/ykChr_dme1" {
Properties {
 _InnerZOffset ("Inner ZOffset", Float) = 0
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) EnvMask(A)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "ENVMAP"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp float _InnerZOffset;
uniform highp vec4 _DifTex_ST;
varying highp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 litDir_2;
  highp vec4 envVec_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_4.xyw = tmpvar_6.xyw;
  tmpvar_4.z = (tmpvar_6.z + (_InnerZOffset * 0.01));
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = tmpvar_1;
  envVec_3.xy = (glstate_matrix_invtrans_modelview0 * tmpvar_8).xy;
  envVec_3.zw = vec2(0.0, 1.0);
  if ((unity_LightPosition[0].w == 0.0)) {
    litDir_2 = normalize((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz);
  } else {
    litDir_2 = normalize(((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz - _glesVertex.xyz));
  };
  tmpvar_5.w = ((dot (tmpvar_1, normalize(litDir_2)) * 0.5) + 0.5);
  tmpvar_5.xyz = ((tmpvar_5.w * unity_LightColor[0]).xyz + glstate_lightmodel_ambient.xyz);
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
  gl_Position = tmpvar_4;
  xlv_COLOR = tmpvar_5;
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = (tmpvar_9 * envVec_3).xy;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _DifCol;
uniform sampler2D _DifTex;
uniform sampler2D _EnvTex;
varying highp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 envCol_1;
  highp vec4 texCol_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_DifTex, xlv_TEXCOORD0);
  texCol_2 = tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_EnvTex, xlv_TEXCOORD1);
  envCol_1 = tmpvar_4;
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (((xlv_COLOR.xyz * (_DifCol.xyz * texCol_2.xyz)) + ((envCol_1.xyz * texCol_2.w) * xlv_COLOR.w)) * vec3(2.0, 2.0, 2.0));
  gl_FragData[0] = tmpvar_5;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp float _InnerZOffset;
uniform highp vec4 _DifTex_ST;
out highp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 litDir_2;
  highp vec4 envVec_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_4.xyw = tmpvar_6.xyw;
  tmpvar_4.z = (tmpvar_6.z + (_InnerZOffset * 0.01));
  highp vec2 tmpvar_7;
  tmpvar_7 = ((_glesMultiTexCoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = tmpvar_1;
  envVec_3.xy = (glstate_matrix_invtrans_modelview0 * tmpvar_8).xy;
  envVec_3.zw = vec2(0.0, 1.0);
  if ((unity_LightPosition[0].w == 0.0)) {
    litDir_2 = normalize((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz);
  } else {
    litDir_2 = normalize(((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz - _glesVertex.xyz));
  };
  tmpvar_5.w = ((dot (tmpvar_1, normalize(litDir_2)) * 0.5) + 0.5);
  tmpvar_5.xyz = ((tmpvar_5.w * unity_LightColor[0]).xyz + glstate_lightmodel_ambient.xyz);
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
  gl_Position = tmpvar_4;
  xlv_COLOR = tmpvar_5;
  xlv_TEXCOORD0 = tmpvar_7;
  xlv_TEXCOORD1 = (tmpvar_9 * envVec_3).xy;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _DifCol;
uniform sampler2D _DifTex;
uniform sampler2D _EnvTex;
in highp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 envCol_1;
  highp vec4 texCol_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_DifTex, xlv_TEXCOORD0);
  texCol_2 = tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_EnvTex, xlv_TEXCOORD1);
  envCol_1 = tmpvar_4;
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = (((xlv_COLOR.xyz * (_DifCol.xyz * texCol_2.xyz)) + ((envCol_1.xyz * texCol_2.w) * xlv_COLOR.w)) * vec3(2.0, 2.0, 2.0));
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