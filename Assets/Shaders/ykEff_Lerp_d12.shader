Shader "Custom/ykEff_Lerp" {
Properties {
 _StOneCol ("Start WhiteColor", Color) = (1,1,1,1)
 _StZeroCol ("Start BlackColor", Color) = (0,0,0,1)
 _EdOneCol ("End WhiteColor", Color) = (1,1,1,1)
 _EdZeroCol ("End BlackColor", Color) = (0,0,0,1)
 _MainTex ("Lerp (RGB) Transparency (A)", 2D) = "white" {}
 _ZOffset ("ZOffset", Float) = 0
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp float _ZOffset;
varying highp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 vpos_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  vpos_1.xyw = tmpvar_2.xyw;
  vpos_1.z = (tmpvar_2.z + (_ZOffset * 0.01));
  gl_Position = vpos_1;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _StOneCol;
uniform highp vec4 _EdOneCol;
uniform highp vec4 _StZeroCol;
uniform highp vec4 _EdZeroCol;
uniform sampler2D _MainTex;
varying highp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 texCol_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  texCol_1 = tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (mix (_EdZeroCol, _StZeroCol, xlv_COLOR.xxxx), mix (_EdOneCol, _StOneCol, xlv_COLOR.yyyy), vec4(((
    (texCol_1.x + texCol_1.y)
   + texCol_1.z) / 3.0))).xyz;
  tmpvar_3.w = (xlv_COLOR.w * texCol_1.w);
  gl_FragData[0] = tmpvar_3;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp float _ZOffset;
out highp vec4 xlv_COLOR;
out highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 vpos_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  vpos_1.xyw = tmpvar_2.xyw;
  vpos_1.z = (tmpvar_2.z + (_ZOffset * 0.01));
  gl_Position = vpos_1;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _StOneCol;
uniform highp vec4 _EdOneCol;
uniform highp vec4 _StZeroCol;
uniform highp vec4 _EdZeroCol;
uniform sampler2D _MainTex;
in highp vec4 xlv_COLOR;
in highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 texCol_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  texCol_1 = tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (mix (_EdZeroCol, _StZeroCol, xlv_COLOR.xxxx), mix (_EdOneCol, _StOneCol, xlv_COLOR.yyyy), vec4(((
    (texCol_1.x + texCol_1.y)
   + texCol_1.z) / 3.0))).xyz;
  tmpvar_3.w = (xlv_COLOR.w * texCol_1.w);
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
}
Fallback "Diffuse"
}