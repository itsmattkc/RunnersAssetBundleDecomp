Shader "Custom/ykChrLine_d" {
Properties {
 _OutlineColor ("OutLine Color", Color) = (0,0,0,1)
 _OutlineWidth ("OutLine Width", Float) = 0.05
 _OutlineZOffset ("OutLine ZOffset", Float) = 0
 _InnerZOffset ("Inner ZOffset", Float) = 0
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) Alpha(A)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Transparent" }
 Pass {
  Name "DIFFUSE"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha
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
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 litDir_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_3.xyw = tmpvar_5.xyw;
  tmpvar_3.z = (tmpvar_5.z + (_InnerZOffset * 0.01));
  highp vec2 tmpvar_6;
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);
  if ((unity_LightPosition[0].w == 0.0)) {
    litDir_2 = normalize((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz);
  } else {
    litDir_2 = normalize(((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz - _glesVertex.xyz));
  };
  tmpvar_4.xyz = (((
    (dot (tmpvar_1, normalize(litDir_2)) * 0.5)
   + 0.5) * unity_LightColor[0]).xyz + glstate_lightmodel_ambient.xyz);
  gl_Position = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_TEXCOORD0 = tmpvar_6;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _DifCol;
uniform sampler2D _DifTex;
varying highp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 texCol_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_DifTex, xlv_TEXCOORD0);
  texCol_1 = tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.xyz = (((xlv_COLOR.xyz * _DifCol.xyz) * texCol_1.xyz) * vec3(2.0, 2.0, 2.0));
  tmpvar_3.w = texCol_1.w;
  gl_FragData[0] = tmpvar_3;
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
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 litDir_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 tmpvar_5;
  tmpvar_5 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_3.xyw = tmpvar_5.xyw;
  tmpvar_3.z = (tmpvar_5.z + (_InnerZOffset * 0.01));
  highp vec2 tmpvar_6;
  tmpvar_6 = ((_glesMultiTexCoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);
  if ((unity_LightPosition[0].w == 0.0)) {
    litDir_2 = normalize((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz);
  } else {
    litDir_2 = normalize(((unity_LightPosition[0] * glstate_matrix_invtrans_modelview0).xyz - _glesVertex.xyz));
  };
  tmpvar_4.xyz = (((
    (dot (tmpvar_1, normalize(litDir_2)) * 0.5)
   + 0.5) * unity_LightColor[0]).xyz + glstate_lightmodel_ambient.xyz);
  gl_Position = tmpvar_3;
  xlv_COLOR = tmpvar_4;
  xlv_TEXCOORD0 = tmpvar_6;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _DifCol;
uniform sampler2D _DifTex;
in highp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 texCol_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_DifTex, xlv_TEXCOORD0);
  texCol_1 = tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.xyz = (((xlv_COLOR.xyz * _DifCol.xyz) * texCol_1.xyz) * vec3(2.0, 2.0, 2.0));
  tmpvar_3.w = texCol_1.w;
  _glesFragData[0] = tmpvar_3;
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
 UsePass "Custom/ykOutLine/OUTLINE"
}
Fallback "Toon/Basic"
}