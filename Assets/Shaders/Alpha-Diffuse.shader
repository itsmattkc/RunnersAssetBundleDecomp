Shader "Transparent/Diffuse" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  AlphaTest Greater 0
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  highp mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp vec4 c_4;
  c_4.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_4.w = tmpvar_3;
  c_1.xyz = (c_4.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  highp mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp vec4 c_4;
  c_4.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_4.w = tmpvar_3;
  c_1.xyz = (c_4.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  c_1.xyz = (tmpvar_2.xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz));
  c_1.w = tmpvar_2.w;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  mediump vec3 lm_3;
  lowp vec3 tmpvar_4;
  tmpvar_4 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD1).xyz);
  lm_3 = tmpvar_4;
  mediump vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_2.xyz * lm_3);
  c_1.xyz = tmpvar_5;
  c_1.w = tmpvar_2.w;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  mediump vec3 lm_3;
  lowp vec3 tmpvar_4;
  tmpvar_4 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD1).xyz);
  lm_3 = tmpvar_4;
  mediump vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_2.xyz * lm_3);
  c_1.xyz = tmpvar_5;
  c_1.w = tmpvar_2.w;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  highp mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  highp vec3 tmpvar_22;
  tmpvar_22 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_4LightPosX0 - tmpvar_22.x);
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_4LightPosY0 - tmpvar_22.y);
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_4LightPosZ0 - tmpvar_22.z);
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_23 * tmpvar_23) + (tmpvar_24 * tmpvar_24)) + (tmpvar_25 * tmpvar_25));
  highp vec4 tmpvar_27;
  tmpvar_27 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_23 * tmpvar_5.x) + (tmpvar_24 * tmpvar_5.y)) + (tmpvar_25 * tmpvar_5.z))
   * 
    inversesqrt(tmpvar_26)
  )) * (1.0/((1.0 + 
    (tmpvar_26 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_3 + ((
    ((unity_LightColor[0].xyz * tmpvar_27.x) + (unity_LightColor[1].xyz * tmpvar_27.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_27.z)
  ) + (unity_LightColor[3].xyz * tmpvar_27.w)));
  tmpvar_3 = tmpvar_28;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp vec4 c_4;
  c_4.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_4.w = tmpvar_3;
  c_1.xyz = (c_4.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 unity_4LightPosX0;
uniform highp vec4 unity_4LightPosY0;
uniform highp vec4 unity_4LightPosZ0;
uniform highp vec4 unity_4LightAtten0;
uniform highp vec4 unity_LightColor[8];
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
void main ()
{
  highp vec3 shlight_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  highp mat3 tmpvar_4;
  tmpvar_4[0] = _Object2World[0].xyz;
  tmpvar_4[1] = _Object2World[1].xyz;
  tmpvar_4[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (tmpvar_4 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_2 = tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  mediump vec3 tmpvar_7;
  mediump vec4 normal_8;
  normal_8 = tmpvar_6;
  highp float vC_9;
  mediump vec3 x3_10;
  mediump vec3 x2_11;
  mediump vec3 x1_12;
  highp float tmpvar_13;
  tmpvar_13 = dot (unity_SHAr, normal_8);
  x1_12.x = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = dot (unity_SHAg, normal_8);
  x1_12.y = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = dot (unity_SHAb, normal_8);
  x1_12.z = tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = (normal_8.xyzz * normal_8.yzzx);
  highp float tmpvar_17;
  tmpvar_17 = dot (unity_SHBr, tmpvar_16);
  x2_11.x = tmpvar_17;
  highp float tmpvar_18;
  tmpvar_18 = dot (unity_SHBg, tmpvar_16);
  x2_11.y = tmpvar_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHBb, tmpvar_16);
  x2_11.z = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = ((normal_8.x * normal_8.x) - (normal_8.y * normal_8.y));
  vC_9 = tmpvar_20;
  highp vec3 tmpvar_21;
  tmpvar_21 = (unity_SHC.xyz * vC_9);
  x3_10 = tmpvar_21;
  tmpvar_7 = ((x1_12 + x2_11) + x3_10);
  shlight_1 = tmpvar_7;
  tmpvar_3 = shlight_1;
  highp vec3 tmpvar_22;
  tmpvar_22 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_4LightPosX0 - tmpvar_22.x);
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_4LightPosY0 - tmpvar_22.y);
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_4LightPosZ0 - tmpvar_22.z);
  highp vec4 tmpvar_26;
  tmpvar_26 = (((tmpvar_23 * tmpvar_23) + (tmpvar_24 * tmpvar_24)) + (tmpvar_25 * tmpvar_25));
  highp vec4 tmpvar_27;
  tmpvar_27 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_23 * tmpvar_5.x) + (tmpvar_24 * tmpvar_5.y)) + (tmpvar_25 * tmpvar_5.z))
   * 
    inversesqrt(tmpvar_26)
  )) * (1.0/((1.0 + 
    (tmpvar_26 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_3 + ((
    ((unity_LightColor[0].xyz * tmpvar_27.x) + (unity_LightColor[1].xyz * tmpvar_27.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_27.z)
  ) + (unity_LightColor[3].xyz * tmpvar_27.w)));
  tmpvar_3 = tmpvar_28;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_3;
  tmpvar_3 = tmpvar_2.w;
  lowp vec4 c_4;
  c_4.xyz = ((tmpvar_2.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_4.w = tmpvar_3;
  c_1.xyz = (c_4.xyz + (tmpvar_2.xyz * xlv_TEXCOORD2));
  c_1.w = tmpvar_3;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES3"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend SrcAlpha One
  AlphaTest Greater 0
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 cse_6;
  cse_6 = (_Object2World * _glesVertex);
  tmpvar_5 = (_WorldSpaceLightPos0.xyz - cse_6.xyz);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * cse_6).xyz;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture2D (_LightTexture0, vec2(tmpvar_6)).w) * 2.0));
  c_7.w = tmpvar_4;
  c_1.xyz = c_7.xyz;
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 cse_6;
  cse_6 = (_Object2World * _glesVertex);
  tmpvar_5 = (_WorldSpaceLightPos0.xyz - cse_6.xyz);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * cse_6).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture (_LightTexture0, vec2(tmpvar_6)).w) * 2.0));
  c_7.w = tmpvar_4;
  c_1.xyz = c_7.xyz;
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = _WorldSpaceLightPos0.xyz;
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, lightDir_2)
  ) * 2.0));
  c_5.w = tmpvar_4;
  c_1.xyz = c_5.xyz;
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = _WorldSpaceLightPos0.xyz;
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, lightDir_2)
  ) * 2.0));
  c_5.w = tmpvar_4;
  c_1.xyz = c_5.xyz;
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 cse_6;
  cse_6 = (_Object2World * _glesVertex);
  tmpvar_5 = (_WorldSpaceLightPos0.xyz - cse_6.xyz);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * cse_6);
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp vec2 P_6;
  P_6 = ((xlv_TEXCOORD3.xy / xlv_TEXCOORD3.w) + 0.5);
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD3.xyz, xlv_TEXCOORD3.xyz);
  lowp float atten_8;
  atten_8 = ((float(
    (xlv_TEXCOORD3.z > 0.0)
  ) * texture2D (_LightTexture0, P_6).w) * texture2D (_LightTextureB0, vec2(tmpvar_7)).w);
  lowp vec4 c_9;
  c_9.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * atten_8) * 2.0));
  c_9.w = tmpvar_4;
  c_1.xyz = c_9.xyz;
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 cse_6;
  cse_6 = (_Object2World * _glesVertex);
  tmpvar_5 = (_WorldSpaceLightPos0.xyz - cse_6.xyz);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * cse_6);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp vec2 P_6;
  P_6 = ((xlv_TEXCOORD3.xy / xlv_TEXCOORD3.w) + 0.5);
  highp float tmpvar_7;
  tmpvar_7 = dot (xlv_TEXCOORD3.xyz, xlv_TEXCOORD3.xyz);
  lowp float atten_8;
  atten_8 = ((float(
    (xlv_TEXCOORD3.z > 0.0)
  ) * texture (_LightTexture0, P_6).w) * texture (_LightTextureB0, vec2(tmpvar_7)).w);
  lowp vec4 c_9;
  c_9.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * atten_8) * 2.0));
  c_9.w = tmpvar_4;
  c_1.xyz = c_9.xyz;
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 cse_6;
  cse_6 = (_Object2World * _glesVertex);
  tmpvar_5 = (_WorldSpaceLightPos0.xyz - cse_6.xyz);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * cse_6).xyz;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _LightColor0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * 
    (texture2D (_LightTextureB0, vec2(tmpvar_6)).w * textureCube (_LightTexture0, xlv_TEXCOORD3).w)
  ) * 2.0));
  c_7.w = tmpvar_4;
  c_1.xyz = c_7.xyz;
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  highp vec4 cse_6;
  cse_6 = (_Object2World * _glesVertex);
  tmpvar_5 = (_WorldSpaceLightPos0.xyz - cse_6.xyz);
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * cse_6).xyz;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _LightColor0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * 
    (texture (_LightTextureB0, vec2(tmpvar_6)).w * texture (_LightTexture0, xlv_TEXCOORD3).w)
  ) * 2.0));
  c_7.w = tmpvar_4;
  c_1.xyz = c_7.xyz;
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = _WorldSpaceLightPos0.xyz;
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * (_Object2World * _glesVertex)).xy;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture2D (_LightTexture0, xlv_TEXCOORD3).w) * 2.0));
  c_5.w = tmpvar_4;
  c_1.xyz = c_5.xyz;
  c_1.w = tmpvar_4;
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp mat4 _LightMatrix0;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out lowp vec3 xlv_TEXCOORD1;
out mediump vec3 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 tmpvar_1;
  mediump vec3 tmpvar_2;
  highp mat3 tmpvar_3;
  tmpvar_3[0] = _Object2World[0].xyz;
  tmpvar_3[1] = _Object2World[1].xyz;
  tmpvar_3[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = _WorldSpaceLightPos0.xyz;
  tmpvar_2 = tmpvar_5;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = tmpvar_2;
  xlv_TEXCOORD3 = (_LightMatrix0 * (_Object2World * _glesVertex)).xy;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _LightColor0;
uniform sampler2D _LightTexture0;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = (texture (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp float tmpvar_4;
  tmpvar_4 = tmpvar_3.w;
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = ((tmpvar_3.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture (_LightTexture0, xlv_TEXCOORD3).w) * 2.0));
  c_5.w = tmpvar_4;
  c_1.xyz = c_5.xyz;
  c_1.w = tmpvar_4;
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "SPOT" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "SPOT" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "POINT_COOKIE" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL_COOKIE" }
"!!GLES3"
}
}
 }
}
Fallback "Transparent/VertexLit"
}