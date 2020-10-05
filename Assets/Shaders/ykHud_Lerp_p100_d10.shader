Shader "Custom/UI/ykHud_Lerp_+100" {
Properties {
 _StOneCol ("Start WhiteColor", Color) = (1,1,1,1)
 _StZeroCol ("Start BlackColor", Color) = (0,0,0,1)
 _EdOneCol ("End WhiteColor", Color) = (1,1,1,1)
 _EdZeroCol ("End BlackColor", Color) = (0,0,0,1)
 _MainTex ("Base (RGB) Transparency (A)", 2D) = "" {}
 _ScrollingSpeed ("UVScrollSpeed", Vector) = (0,0,0,0)
}
SubShader { 
 LOD 100
 Tags { "QUEUE"="Transparent+100" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent+100" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ScrollingSpeed;
varying highp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = (_glesMultiTexCoord0.xy + fract((_ScrollingSpeed.xy * _Time.y)));
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _StOneCol;
uniform highp vec4 _EdOneCol;
uniform highp vec4 _StZeroCol;
uniform highp vec4 _EdZeroCol;
uniform sampler2D _MainTex;
varying highp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3 = vec4((((tmpvar_2.x + tmpvar_2.y) + tmpvar_2.z) / 3.0));
  highp vec4 tmpvar_4;
  tmpvar_4.xyz = mix (mix (_EdZeroCol, _StZeroCol, xlv_COLOR.xxxx), mix (_EdOneCol, _StOneCol, xlv_COLOR.yyyy), tmpvar_3).xyz;
  tmpvar_4.w = (xlv_COLOR.w * tmpvar_2.w);
  tmpvar_1 = tmpvar_4;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesColor;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _ScrollingSpeed;
out highp vec4 xlv_COLOR;
out mediump vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = (_glesMultiTexCoord0.xy + fract((_ScrollingSpeed.xy * _Time.y)));
  tmpvar_1 = tmpvar_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = tmpvar_1;
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
in mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3 = vec4((((tmpvar_2.x + tmpvar_2.y) + tmpvar_2.z) / 3.0));
  highp vec4 tmpvar_4;
  tmpvar_4.xyz = mix (mix (_EdZeroCol, _StZeroCol, xlv_COLOR.xxxx), mix (_EdOneCol, _StOneCol, xlv_COLOR.yyyy), tmpvar_3).xyz;
  tmpvar_4.w = (xlv_COLOR.w * tmpvar_2.w);
  tmpvar_1 = tmpvar_4;
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