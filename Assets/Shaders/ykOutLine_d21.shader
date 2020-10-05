Shader "Custom/ykOutLine" {
Properties {
 _OutlineColor ("OutLine Color", Color) = (0,0,0,1)
 _OutlineWidth ("OutLine Width", Float) = 1
 _OutlineZOffset ("OutLine ZOffset", Float) = 0
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "OUTLINE"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Cull Front
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _OutlineZOffset;
varying highp vec4 xlv_COLOR;
void main ()
{
  highp vec4 vpos_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  vpos_1.w = tmpvar_2.w;
  mat3 tmpvar_3;
  tmpvar_3[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_3[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_3[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  mat2 tmpvar_4;
  tmpvar_4[0] = glstate_matrix_projection[0].xy;
  tmpvar_4[1] = glstate_matrix_projection[1].xy;
  vpos_1.xy = (tmpvar_2.xy + ((tmpvar_4 * (tmpvar_3 * normalize(_glesNormal)).xy) * _OutlineWidth));
  vpos_1.z = (tmpvar_2.z + (_OutlineZOffset * 0.01));
  gl_Position = vpos_1;
  xlv_COLOR = _OutlineColor;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _OutlineColor;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.xyz = _OutlineColor.xyz;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 glstate_matrix_projection;
uniform highp vec4 _OutlineColor;
uniform highp float _OutlineWidth;
uniform highp float _OutlineZOffset;
out highp vec4 xlv_COLOR;
void main ()
{
  highp vec4 vpos_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  vpos_1.w = tmpvar_2.w;
  mat3 tmpvar_3;
  tmpvar_3[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_3[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_3[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  mat2 tmpvar_4;
  tmpvar_4[0] = glstate_matrix_projection[0].xy;
  tmpvar_4[1] = glstate_matrix_projection[1].xy;
  vpos_1.xy = (tmpvar_2.xy + ((tmpvar_4 * (tmpvar_3 * normalize(_glesNormal)).xy) * _OutlineWidth));
  vpos_1.z = (tmpvar_2.z + (_OutlineZOffset * 0.01));
  gl_Position = vpos_1;
  xlv_COLOR = _OutlineColor;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _OutlineColor;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.xyz = _OutlineColor.xyz;
  _glesFragData[0] = tmpvar_1;
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