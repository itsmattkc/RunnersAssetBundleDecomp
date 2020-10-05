Shader "Custom/ykFallOff_dve1" {
Properties {
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) EnvMask(A)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "black" {}
 _RimPower ("Rim Power", Range(0.5,8)) = 3
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
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 _DifTex_ST;
uniform highp float _RimPower;
varying highp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 envVec_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4.w = 0.0;
  tmpvar_4.xyz = tmpvar_1;
  tmpvar_3.w = pow ((1.0 - dot (
    normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz))
  , 
    (_Object2World * tmpvar_4)
  .xyz)), _RimPower);
  lowp vec3 tmpvar_5;
  tmpvar_5 = _glesColor.xyz;
  tmpvar_3.xyz = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_1;
  envVec_2.xy = (glstate_matrix_invtrans_modelview0 * tmpvar_6).xy;
  envVec_2.zw = vec2(0.0, 1.0);
  highp mat4 tmpvar_7;
  tmpvar_7[0].x = 0.5;
  tmpvar_7[0].y = 0.0;
  tmpvar_7[0].z = 0.0;
  tmpvar_7[0].w = 0.0;
  tmpvar_7[1].x = 0.0;
  tmpvar_7[1].y = 0.5;
  tmpvar_7[1].z = 0.0;
  tmpvar_7[1].w = 0.0;
  tmpvar_7[2].x = 0.0;
  tmpvar_7[2].y = 0.0;
  tmpvar_7[2].z = 1.0;
  tmpvar_7[2].w = 0.0;
  tmpvar_7[3].x = 0.5;
  tmpvar_7[3].y = 0.5;
  tmpvar_7[3].z = 0.0;
  tmpvar_7[3].w = 1.0;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);
  xlv_TEXCOORD1 = (tmpvar_7 * envVec_2).xy;
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
  tmpvar_5.xyz = (_DifCol.xyz * mix ((texCol_2.xyz + 
    (envCol_1.xyz * texCol_2.w)
  ), xlv_COLOR.xyz, xlv_COLOR.www));
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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 _DifTex_ST;
uniform highp float _RimPower;
out highp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec4 envVec_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4.w = 0.0;
  tmpvar_4.xyz = tmpvar_1;
  tmpvar_3.w = pow ((1.0 - dot (
    normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz))
  , 
    (_Object2World * tmpvar_4)
  .xyz)), _RimPower);
  lowp vec3 tmpvar_5;
  tmpvar_5 = _glesColor.xyz;
  tmpvar_3.xyz = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_1;
  envVec_2.xy = (glstate_matrix_invtrans_modelview0 * tmpvar_6).xy;
  envVec_2.zw = vec2(0.0, 1.0);
  highp mat4 tmpvar_7;
  tmpvar_7[0].x = 0.5;
  tmpvar_7[0].y = 0.0;
  tmpvar_7[0].z = 0.0;
  tmpvar_7[0].w = 0.0;
  tmpvar_7[1].x = 0.0;
  tmpvar_7[1].y = 0.5;
  tmpvar_7[1].z = 0.0;
  tmpvar_7[1].w = 0.0;
  tmpvar_7[2].x = 0.0;
  tmpvar_7[2].y = 0.0;
  tmpvar_7[2].z = 1.0;
  tmpvar_7[2].w = 0.0;
  tmpvar_7[3].x = 0.5;
  tmpvar_7[3].y = 0.5;
  tmpvar_7[3].z = 0.0;
  tmpvar_7[3].w = 1.0;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);
  xlv_TEXCOORD1 = (tmpvar_7 * envVec_2).xy;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
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
  tmpvar_5.xyz = (_DifCol.xyz * mix ((texCol_2.xyz + 
    (envCol_1.xyz * texCol_2.w)
  ), xlv_COLOR.xyz, xlv_COLOR.www));
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