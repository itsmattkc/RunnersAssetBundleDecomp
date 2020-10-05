Shader "Reflective/Diffuse" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
 _MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {}
 _Cube ("Reflection Cubemap", CUBE) = "_Skybox" { TexGen CubeReflect }
}
SubShader { 
 LOD 200
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp vec4 c_6;
  c_6.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_6.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_6.w;
  c_1.xyz = (c_6.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp vec4 c_6;
  c_6.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_6.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_6.w;
  c_1.xyz = (c_6.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  c_1.xyz = ((tmpvar_3 * _Color).xyz * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz));
  c_1.w = (tmpvar_4.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_4.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  c_1.xyz = ((tmpvar_3 * _Color).xyz * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz));
  c_1.w = (tmpvar_4.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_4.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  mediump vec3 lm_6;
  lowp vec3 tmpvar_7;
  tmpvar_7 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz);
  lm_6 = tmpvar_7;
  mediump vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_4.xyz * lm_6);
  c_1.xyz = tmpvar_8;
  c_1.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  mediump vec3 lm_6;
  lowp vec3 tmpvar_7;
  tmpvar_7 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz);
  lm_6 = tmpvar_7;
  mediump vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_4.xyz * lm_6);
  c_1.xyz = tmpvar_8;
  c_1.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float tmpvar_6;
  mediump float lightShadowDataX_7;
  highp float dist_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD4).x;
  dist_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = _LightShadowData.x;
  lightShadowDataX_7 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = max (float((dist_8 > 
    (xlv_TEXCOORD4.z / xlv_TEXCOORD4.w)
  )), lightShadowDataX_7);
  tmpvar_6 = tmpvar_11;
  lowp vec4 c_12;
  c_12.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz))
   * tmpvar_6) * 2.0));
  c_12.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_12.w;
  c_1.xyz = (c_12.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _LightShadowData;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float tmpvar_5;
  mediump float lightShadowDataX_6;
  highp float dist_7;
  lowp float tmpvar_8;
  tmpvar_8 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD3).x;
  dist_7 = tmpvar_8;
  highp float tmpvar_9;
  tmpvar_9 = _LightShadowData.x;
  lightShadowDataX_6 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = max (float((dist_7 > 
    (xlv_TEXCOORD3.z / xlv_TEXCOORD3.w)
  )), lightShadowDataX_6);
  tmpvar_5 = tmpvar_10;
  c_1.xyz = ((tmpvar_3 * _Color).xyz * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((tmpvar_5 * 2.0))));
  c_1.w = (tmpvar_4.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_4.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _LightShadowData;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float tmpvar_6;
  mediump float lightShadowDataX_7;
  highp float dist_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD3).x;
  dist_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = _LightShadowData.x;
  lightShadowDataX_7 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = max (float((dist_8 > 
    (xlv_TEXCOORD3.z / xlv_TEXCOORD3.w)
  )), lightShadowDataX_7);
  tmpvar_6 = tmpvar_11;
  mediump vec3 lm_12;
  lowp vec3 tmpvar_13;
  tmpvar_13 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz);
  lm_12 = tmpvar_13;
  lowp vec3 tmpvar_14;
  tmpvar_14 = vec3((tmpvar_6 * 2.0));
  mediump vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_4.xyz * min (lm_12, tmpvar_14));
  c_1.xyz = tmpvar_15;
  c_1.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
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
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  highp vec3 tmpvar_28;
  tmpvar_28 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_29;
  tmpvar_29 = (unity_4LightPosX0 - tmpvar_28.x);
  highp vec4 tmpvar_30;
  tmpvar_30 = (unity_4LightPosY0 - tmpvar_28.y);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosZ0 - tmpvar_28.z);
  highp vec4 tmpvar_32;
  tmpvar_32 = (((tmpvar_29 * tmpvar_29) + (tmpvar_30 * tmpvar_30)) + (tmpvar_31 * tmpvar_31));
  highp vec4 tmpvar_33;
  tmpvar_33 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_29 * tmpvar_11.x) + (tmpvar_30 * tmpvar_11.y)) + (tmpvar_31 * tmpvar_11.z))
   * 
    inversesqrt(tmpvar_32)
  )) * (1.0/((1.0 + 
    (tmpvar_32 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_5 + ((
    ((unity_LightColor[0].xyz * tmpvar_33.x) + (unity_LightColor[1].xyz * tmpvar_33.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_33.z)
  ) + (unity_LightColor[3].xyz * tmpvar_33.w)));
  tmpvar_5 = tmpvar_34;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp vec4 c_6;
  c_6.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_6.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_6.w;
  c_1.xyz = (c_6.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
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
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  highp vec3 tmpvar_28;
  tmpvar_28 = (_Object2World * _glesVertex).xyz;
  highp vec4 tmpvar_29;
  tmpvar_29 = (unity_4LightPosX0 - tmpvar_28.x);
  highp vec4 tmpvar_30;
  tmpvar_30 = (unity_4LightPosY0 - tmpvar_28.y);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosZ0 - tmpvar_28.z);
  highp vec4 tmpvar_32;
  tmpvar_32 = (((tmpvar_29 * tmpvar_29) + (tmpvar_30 * tmpvar_30)) + (tmpvar_31 * tmpvar_31));
  highp vec4 tmpvar_33;
  tmpvar_33 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_29 * tmpvar_11.x) + (tmpvar_30 * tmpvar_11.y)) + (tmpvar_31 * tmpvar_11.z))
   * 
    inversesqrt(tmpvar_32)
  )) * (1.0/((1.0 + 
    (tmpvar_32 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_5 + ((
    ((unity_LightColor[0].xyz * tmpvar_33.x) + (unity_LightColor[1].xyz * tmpvar_33.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_33.z)
  ) + (unity_LightColor[3].xyz * tmpvar_33.w)));
  tmpvar_5 = tmpvar_34;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _WorldSpaceLightPos0;
uniform lowp vec4 _LightColor0;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp vec4 c_6;
  c_6.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz)
  ) * 2.0));
  c_6.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_6.w;
  c_1.xyz = (c_6.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
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
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  highp vec4 cse_28;
  cse_28 = (_Object2World * _glesVertex);
  highp vec4 tmpvar_29;
  tmpvar_29 = (unity_4LightPosX0 - cse_28.x);
  highp vec4 tmpvar_30;
  tmpvar_30 = (unity_4LightPosY0 - cse_28.y);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosZ0 - cse_28.z);
  highp vec4 tmpvar_32;
  tmpvar_32 = (((tmpvar_29 * tmpvar_29) + (tmpvar_30 * tmpvar_30)) + (tmpvar_31 * tmpvar_31));
  highp vec4 tmpvar_33;
  tmpvar_33 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_29 * tmpvar_11.x) + (tmpvar_30 * tmpvar_11.y)) + (tmpvar_31 * tmpvar_11.z))
   * 
    inversesqrt(tmpvar_32)
  )) * (1.0/((1.0 + 
    (tmpvar_32 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_5 + ((
    ((unity_LightColor[0].xyz * tmpvar_33.x) + (unity_LightColor[1].xyz * tmpvar_33.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_33.z)
  ) + (unity_LightColor[3].xyz * tmpvar_33.w)));
  tmpvar_5 = tmpvar_34;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = (unity_World2Shadow[0] * cse_28);
}



#endif
#ifdef FRAGMENT

uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float tmpvar_6;
  mediump float lightShadowDataX_7;
  highp float dist_8;
  lowp float tmpvar_9;
  tmpvar_9 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD4).x;
  dist_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = _LightShadowData.x;
  lightShadowDataX_7 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = max (float((dist_8 > 
    (xlv_TEXCOORD4.z / xlv_TEXCOORD4.w)
  )), lightShadowDataX_7);
  tmpvar_6 = tmpvar_11;
  lowp vec4 c_12;
  c_12.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz))
   * tmpvar_6) * 2.0));
  c_12.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_12.w;
  c_1.xyz = (c_12.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_6;
  lowp float tmpvar_7;
  tmpvar_7 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD4.xyz);
  highp float tmpvar_8;
  tmpvar_8 = (_LightShadowData.x + (tmpvar_7 * (1.0 - _LightShadowData.x)));
  shadow_6 = tmpvar_8;
  lowp vec4 c_9;
  c_9.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz))
   * shadow_6) * 2.0));
  c_9.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_6;
  mediump float tmpvar_7;
  tmpvar_7 = texture (_ShadowMapTexture, xlv_TEXCOORD4.xyz);
  lowp float tmpvar_8;
  tmpvar_8 = tmpvar_7;
  highp float tmpvar_9;
  tmpvar_9 = (_LightShadowData.x + (tmpvar_8 * (1.0 - _LightShadowData.x)));
  shadow_6 = tmpvar_9;
  lowp vec4 c_10;
  c_10.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz))
   * shadow_6) * 2.0));
  c_10.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_10.w;
  c_1.xyz = (c_10.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_5;
  lowp float tmpvar_6;
  tmpvar_6 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  highp float tmpvar_7;
  tmpvar_7 = (_LightShadowData.x + (tmpvar_6 * (1.0 - _LightShadowData.x)));
  shadow_5 = tmpvar_7;
  c_1.xyz = ((tmpvar_3 * _Color).xyz * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((shadow_5 * 2.0))));
  c_1.w = (tmpvar_4.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_4.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_5;
  mediump float tmpvar_6;
  tmpvar_6 = texture (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  lowp float tmpvar_7;
  tmpvar_7 = tmpvar_6;
  highp float tmpvar_8;
  tmpvar_8 = (_LightShadowData.x + (tmpvar_7 * (1.0 - _LightShadowData.x)));
  shadow_5 = tmpvar_8;
  c_1.xyz = ((tmpvar_3 * _Color).xyz * min ((2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((shadow_5 * 2.0))));
  c_1.w = (tmpvar_4.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_4.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_6;
  lowp float tmpvar_7;
  tmpvar_7 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  highp float tmpvar_8;
  tmpvar_8 = (_LightShadowData.x + (tmpvar_7 * (1.0 - _LightShadowData.x)));
  shadow_6 = tmpvar_8;
  mediump vec3 lm_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz);
  lm_9 = tmpvar_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = vec3((shadow_6 * 2.0));
  mediump vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_4.xyz * min (lm_9, tmpvar_11));
  c_1.xyz = tmpvar_12;
  c_1.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_4;
  tmpvar_4 = (_glesVertex.xyz - ((_World2Object * tmpvar_3).xyz * unity_Scale.w));
  highp mat3 tmpvar_5;
  tmpvar_5[0] = _Object2World[0].xyz;
  tmpvar_5[1] = _Object2World[1].xyz;
  tmpvar_5[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * (tmpvar_4 - (2.0 * 
    (dot (tmpvar_1, tmpvar_4) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_6;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_6;
  mediump float tmpvar_7;
  tmpvar_7 = texture (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  lowp float tmpvar_8;
  tmpvar_8 = tmpvar_7;
  highp float tmpvar_9;
  tmpvar_9 = (_LightShadowData.x + (tmpvar_8 * (1.0 - _LightShadowData.x)));
  shadow_6 = tmpvar_9;
  mediump vec3 lm_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz);
  lm_10 = tmpvar_11;
  lowp vec3 tmpvar_12;
  tmpvar_12 = vec3((shadow_6 * 2.0));
  mediump vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_4.xyz * min (lm_10, tmpvar_12));
  c_1.xyz = tmpvar_13;
  c_1.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
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
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  highp vec4 cse_28;
  cse_28 = (_Object2World * _glesVertex);
  highp vec4 tmpvar_29;
  tmpvar_29 = (unity_4LightPosX0 - cse_28.x);
  highp vec4 tmpvar_30;
  tmpvar_30 = (unity_4LightPosY0 - cse_28.y);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosZ0 - cse_28.z);
  highp vec4 tmpvar_32;
  tmpvar_32 = (((tmpvar_29 * tmpvar_29) + (tmpvar_30 * tmpvar_30)) + (tmpvar_31 * tmpvar_31));
  highp vec4 tmpvar_33;
  tmpvar_33 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_29 * tmpvar_11.x) + (tmpvar_30 * tmpvar_11.y)) + (tmpvar_31 * tmpvar_11.z))
   * 
    inversesqrt(tmpvar_32)
  )) * (1.0/((1.0 + 
    (tmpvar_32 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_5 + ((
    ((unity_LightColor[0].xyz * tmpvar_33.x) + (unity_LightColor[1].xyz * tmpvar_33.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_33.z)
  ) + (unity_LightColor[3].xyz * tmpvar_33.w)));
  tmpvar_5 = tmpvar_34;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = (unity_World2Shadow[0] * cse_28);
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (textureCube (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_6;
  lowp float tmpvar_7;
  tmpvar_7 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD4.xyz);
  highp float tmpvar_8;
  tmpvar_8 = (_LightShadowData.x + (tmpvar_7 * (1.0 - _LightShadowData.x)));
  shadow_6 = tmpvar_8;
  lowp vec4 c_9;
  c_9.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz))
   * shadow_6) * 2.0));
  c_9.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
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
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  highp vec3 shlight_2;
  mediump vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - ((_World2Object * tmpvar_6).xyz * unity_Scale.w));
  highp mat3 tmpvar_8;
  tmpvar_8[0] = _Object2World[0].xyz;
  tmpvar_8[1] = _Object2World[1].xyz;
  tmpvar_8[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * (tmpvar_7 - (2.0 * 
    (dot (tmpvar_1, tmpvar_7) * tmpvar_1)
  )));
  tmpvar_3 = tmpvar_9;
  highp mat3 tmpvar_10;
  tmpvar_10[0] = _Object2World[0].xyz;
  tmpvar_10[1] = _Object2World[1].xyz;
  tmpvar_10[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 * (tmpvar_1 * unity_Scale.w));
  tmpvar_4 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_11;
  mediump vec3 tmpvar_13;
  mediump vec4 normal_14;
  normal_14 = tmpvar_12;
  highp float vC_15;
  mediump vec3 x3_16;
  mediump vec3 x2_17;
  mediump vec3 x1_18;
  highp float tmpvar_19;
  tmpvar_19 = dot (unity_SHAr, normal_14);
  x1_18.x = tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAg, normal_14);
  x1_18.y = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAb, normal_14);
  x1_18.z = tmpvar_21;
  mediump vec4 tmpvar_22;
  tmpvar_22 = (normal_14.xyzz * normal_14.yzzx);
  highp float tmpvar_23;
  tmpvar_23 = dot (unity_SHBr, tmpvar_22);
  x2_17.x = tmpvar_23;
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBg, tmpvar_22);
  x2_17.y = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBb, tmpvar_22);
  x2_17.z = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = ((normal_14.x * normal_14.x) - (normal_14.y * normal_14.y));
  vC_15 = tmpvar_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (unity_SHC.xyz * vC_15);
  x3_16 = tmpvar_27;
  tmpvar_13 = ((x1_18 + x2_17) + x3_16);
  shlight_2 = tmpvar_13;
  tmpvar_5 = shlight_2;
  highp vec4 cse_28;
  cse_28 = (_Object2World * _glesVertex);
  highp vec4 tmpvar_29;
  tmpvar_29 = (unity_4LightPosX0 - cse_28.x);
  highp vec4 tmpvar_30;
  tmpvar_30 = (unity_4LightPosY0 - cse_28.y);
  highp vec4 tmpvar_31;
  tmpvar_31 = (unity_4LightPosZ0 - cse_28.z);
  highp vec4 tmpvar_32;
  tmpvar_32 = (((tmpvar_29 * tmpvar_29) + (tmpvar_30 * tmpvar_30)) + (tmpvar_31 * tmpvar_31));
  highp vec4 tmpvar_33;
  tmpvar_33 = (max (vec4(0.0, 0.0, 0.0, 0.0), (
    (((tmpvar_29 * tmpvar_11.x) + (tmpvar_30 * tmpvar_11.y)) + (tmpvar_31 * tmpvar_11.z))
   * 
    inversesqrt(tmpvar_32)
  )) * (1.0/((1.0 + 
    (tmpvar_32 * unity_4LightAtten0)
  ))));
  highp vec3 tmpvar_34;
  tmpvar_34 = (tmpvar_5 + ((
    ((unity_LightColor[0].xyz * tmpvar_33.x) + (unity_LightColor[1].xyz * tmpvar_33.y))
   + 
    (unity_LightColor[2].xyz * tmpvar_33.z)
  ) + (unity_LightColor[3].xyz * tmpvar_33.w)));
  tmpvar_5 = tmpvar_34;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_TEXCOORD4 = (unity_World2Shadow[0] * cse_28);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp vec4 _LightColor0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  highp vec3 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_3 * _Color);
  lowp vec4 tmpvar_5;
  tmpvar_5 = (texture (_Cube, tmpvar_2) * tmpvar_3.w);
  lowp float shadow_6;
  mediump float tmpvar_7;
  tmpvar_7 = texture (_ShadowMapTexture, xlv_TEXCOORD4.xyz);
  lowp float tmpvar_8;
  tmpvar_8 = tmpvar_7;
  highp float tmpvar_9;
  tmpvar_9 = (_LightShadowData.x + (tmpvar_8 * (1.0 - _LightShadowData.x)));
  shadow_6 = tmpvar_9;
  lowp vec4 c_10;
  c_10.xyz = ((tmpvar_4.xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz))
   * shadow_6) * 2.0));
  c_10.w = (tmpvar_5.w * _ReflectColor.w);
  c_1.w = c_10.w;
  c_1.xyz = (c_10.xyz + (tmpvar_4.xyz * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + (tmpvar_5.xyz * _ReflectColor.xyz));
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_ON" }
"!!GLES3"
}
}
 }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardAdd" "RenderType"="Opaque" }
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture2D (_LightTexture0, vec2(tmpvar_6)).w) * 2.0));
  c_7.w = ((textureCube (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_7.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0);
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture (_LightTexture0, vec2(tmpvar_6)).w) * 2.0));
  c_7.w = ((texture (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_7.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, lightDir_2)
  ) * 2.0));
  c_5.w = ((textureCube (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_5.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0);
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * (max (0.0, 
    dot (xlv_TEXCOORD1, lightDir_2)
  ) * 2.0));
  c_5.w = ((texture (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_5.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
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
  c_9.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * atten_8) * 2.0));
  c_9.w = ((textureCube (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_9.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0);
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
  c_9.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * atten_8) * 2.0));
  c_9.w = ((texture (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_9.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * 
    (texture2D (_LightTextureB0, vec2(tmpvar_6)).w * textureCube (_LightTexture0, xlv_TEXCOORD3).w)
  ) * 2.0));
  c_7.w = ((textureCube (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_7.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0);
  mediump vec3 tmpvar_5;
  tmpvar_5 = normalize(xlv_TEXCOORD2);
  lightDir_2 = tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = dot (xlv_TEXCOORD3, xlv_TEXCOORD3);
  lowp vec4 c_7;
  c_7.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * 
    (texture (_LightTextureB0, vec2(tmpvar_6)).w * texture (_LightTexture0, xlv_TEXCOORD3).w)
  ) * 2.0));
  c_7.w = ((texture (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_7.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
varying highp vec2 xlv_TEXCOORD0;
varying lowp vec3 xlv_TEXCOORD1;
varying mediump vec3 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0);
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture2D (_LightTexture0, xlv_TEXCOORD3).w) * 2.0));
  c_5.w = ((textureCube (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_5.xyz;
  c_1.w = 0.0;
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
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
in highp vec2 xlv_TEXCOORD0;
in lowp vec3 xlv_TEXCOORD1;
in mediump vec3 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 lightDir_2;
  highp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0);
  lightDir_2 = xlv_TEXCOORD2;
  lowp vec4 c_5;
  c_5.xyz = (((tmpvar_4 * _Color).xyz * _LightColor0.xyz) * ((
    max (0.0, dot (xlv_TEXCOORD1, lightDir_2))
   * texture (_LightTexture0, xlv_TEXCOORD3).w) * 2.0));
  c_5.w = ((texture (_Cube, tmpvar_3) * tmpvar_4.w).w * _ReflectColor.w);
  c_1.xyz = c_5.xyz;
  c_1.w = 0.0;
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
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassBase" "RenderType"="Opaque" }
  Fog { Mode Off }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
varying lowp vec3 xlv_TEXCOORD0;
void main ()
{
  lowp vec3 tmpvar_1;
  highp mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT

varying lowp vec3 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 res_1;
  res_1.xyz = ((xlv_TEXCOORD0 * 0.5) + 0.5);
  res_1.w = 0.0;
  gl_FragData[0] = res_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
out lowp vec3 xlv_TEXCOORD0;
void main ()
{
  lowp vec3 tmpvar_1;
  highp mat3 tmpvar_2;
  tmpvar_2[0] = _Object2World[0].xyz;
  tmpvar_2[1] = _Object2World[1].xyz;
  tmpvar_2[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_3;
  tmpvar_3 = (tmpvar_2 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_1 = tmpvar_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
in lowp vec3 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 res_1;
  res_1.xyz = ((xlv_TEXCOORD0 * 0.5) + 0.5);
  res_1.w = 0.0;
  _glesFragData[0] = res_1;
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
 Pass {
  Name "PREPASS"
  Tags { "LIGHTMODE"="PrePassFinal" "RenderType"="Opaque" }
  ZWrite Off
Program "vp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * (tmpvar_1 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  tmpvar_3 = tmpvar_14;
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (textureCube (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_10 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
  light_3.w = tmpvar_10.w;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_11;
  lowp vec4 c_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_6.xyz * light_3.xyz);
  c_12.xyz = tmpvar_13;
  c_12.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_12;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * (tmpvar_1 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  tmpvar_3 = tmpvar_14;
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (texture (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_10 = -(log2(max (light_3, vec4(0.001, 0.001, 0.001, 0.001))));
  light_3.w = tmpvar_10.w;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_11;
  lowp vec4 c_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_6.xyz * light_3.xyz);
  c_12.xyz = tmpvar_13;
  c_12.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_12;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  tmpvar_3.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_3.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * _Color);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (textureCube (_Cube, tmpvar_7) * tmpvar_8.w);
  lowp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13 = -(log2(max (light_6, vec4(0.001, 0.001, 0.001, 0.001))));
  light_6.w = tmpvar_13.w;
  highp float tmpvar_14;
  tmpvar_14 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_15;
  lowp vec3 tmpvar_16;
  tmpvar_16 = (2.0 * texture2D (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_16;
  light_6.xyz = (tmpvar_13.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  lowp vec4 c_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = (tmpvar_9.xyz * light_6.xyz);
  c_17.xyz = tmpvar_18;
  c_17.w = (tmpvar_10.w * _ReflectColor.w);
  c_2 = c_17;
  c_2.xyz = (c_2.xyz + tmpvar_11);
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  tmpvar_3.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_3.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * _Color);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (texture (_Cube, tmpvar_7) * tmpvar_8.w);
  lowp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_12;
  tmpvar_12 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13 = -(log2(max (light_6, vec4(0.001, 0.001, 0.001, 0.001))));
  light_6.w = tmpvar_13.w;
  highp float tmpvar_14;
  tmpvar_14 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_15;
  lowp vec3 tmpvar_16;
  tmpvar_16 = (2.0 * texture (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_16;
  light_6.xyz = (tmpvar_13.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  lowp vec4 c_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = (tmpvar_9.xyz * light_6.xyz);
  c_17.xyz = tmpvar_18;
  c_17.w = (tmpvar_10.w * _ReflectColor.w);
  c_2 = c_17;
  c_2.xyz = (c_2.xyz + tmpvar_11);
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_5;
  tmpvar_5 = (_glesVertex.xyz - ((_World2Object * tmpvar_4).xyz * unity_Scale.w));
  highp mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (tmpvar_5 - (2.0 * 
    (dot (tmpvar_1, tmpvar_5) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_7;
  highp vec4 o_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_9.x;
  tmpvar_10.y = (tmpvar_9.y * _ProjectionParams.x);
  o_8.xy = (tmpvar_10 + tmpvar_9.w);
  o_8.zw = tmpvar_3.zw;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_8;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (textureCube (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec3 lm_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lm_10 = tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = lm_10;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (-(log2(
    max (light_3, vec4(0.001, 0.001, 0.001, 0.001))
  )) + tmpvar_12);
  light_3 = tmpvar_13;
  lowp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_6.xyz * tmpvar_13.xyz);
  c_14.xyz = tmpvar_15;
  c_14.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_14;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_5;
  tmpvar_5 = (_glesVertex.xyz - ((_World2Object * tmpvar_4).xyz * unity_Scale.w));
  highp mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (tmpvar_5 - (2.0 * 
    (dot (tmpvar_1, tmpvar_5) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_7;
  highp vec4 o_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_9.x;
  tmpvar_10.y = (tmpvar_9.y * _ProjectionParams.x);
  o_8.xy = (tmpvar_10 + tmpvar_9.w);
  o_8.zw = tmpvar_3.zw;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_8;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (texture (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec3 lm_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lm_10 = tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = lm_10;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (-(log2(
    max (light_3, vec4(0.001, 0.001, 0.001, 0.001))
  )) + tmpvar_12);
  light_3 = tmpvar_13;
  lowp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_6.xyz * tmpvar_13.xyz);
  c_14.xyz = tmpvar_15;
  c_14.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_14;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * (tmpvar_1 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  tmpvar_3 = tmpvar_14;
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (textureCube (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_10 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
  light_3.w = tmpvar_10.w;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_11;
  lowp vec4 c_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_6.xyz * light_3.xyz);
  c_12.xyz = tmpvar_13;
  c_12.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_12;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_SHAr;
uniform highp vec4 unity_SHAg;
uniform highp vec4 unity_SHAb;
uniform highp vec4 unity_SHBr;
uniform highp vec4 unity_SHBg;
uniform highp vec4 unity_SHBb;
uniform highp vec4 unity_SHC;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec3 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  highp mat3 tmpvar_12;
  tmpvar_12[0] = _Object2World[0].xyz;
  tmpvar_12[1] = _Object2World[1].xyz;
  tmpvar_12[2] = _Object2World[2].xyz;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = (tmpvar_12 * (tmpvar_1 * unity_Scale.w));
  mediump vec3 tmpvar_14;
  mediump vec4 normal_15;
  normal_15 = tmpvar_13;
  highp float vC_16;
  mediump vec3 x3_17;
  mediump vec3 x2_18;
  mediump vec3 x1_19;
  highp float tmpvar_20;
  tmpvar_20 = dot (unity_SHAr, normal_15);
  x1_19.x = tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = dot (unity_SHAg, normal_15);
  x1_19.y = tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = dot (unity_SHAb, normal_15);
  x1_19.z = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23 = (normal_15.xyzz * normal_15.yzzx);
  highp float tmpvar_24;
  tmpvar_24 = dot (unity_SHBr, tmpvar_23);
  x2_18.x = tmpvar_24;
  highp float tmpvar_25;
  tmpvar_25 = dot (unity_SHBg, tmpvar_23);
  x2_18.y = tmpvar_25;
  highp float tmpvar_26;
  tmpvar_26 = dot (unity_SHBb, tmpvar_23);
  x2_18.z = tmpvar_26;
  mediump float tmpvar_27;
  tmpvar_27 = ((normal_15.x * normal_15.x) - (normal_15.y * normal_15.y));
  vC_16 = tmpvar_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (unity_SHC.xyz * vC_16);
  x3_17 = tmpvar_28;
  tmpvar_14 = ((x1_19 + x2_18) + x3_17);
  tmpvar_3 = tmpvar_14;
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (texture (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec4 tmpvar_10;
  tmpvar_10 = max (light_3, vec4(0.001, 0.001, 0.001, 0.001));
  light_3.w = tmpvar_10.w;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz + xlv_TEXCOORD3);
  light_3.xyz = tmpvar_11;
  lowp vec4 c_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_6.xyz * light_3.xyz);
  c_12.xyz = tmpvar_13;
  c_12.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_12;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  tmpvar_3.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_3.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_3;
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
varying highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * _Color);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (textureCube (_Cube, tmpvar_7) * tmpvar_8.w);
  lowp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13 = max (light_6, vec4(0.001, 0.001, 0.001, 0.001));
  light_6.w = tmpvar_13.w;
  highp float tmpvar_14;
  tmpvar_14 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_15;
  lowp vec3 tmpvar_16;
  tmpvar_16 = (2.0 * texture2D (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_16;
  light_6.xyz = (tmpvar_13.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  lowp vec4 c_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = (tmpvar_9.xyz * light_6.xyz);
  c_17.xyz = tmpvar_18;
  c_17.w = (tmpvar_10.w * _ReflectColor.w);
  c_2 = c_17;
  c_2.xyz = (c_2.xyz + tmpvar_11);
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
out highp vec4 xlv_TEXCOORD4;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - ((_World2Object * tmpvar_5).xyz * unity_Scale.w));
  highp mat3 tmpvar_7;
  tmpvar_7[0] = _Object2World[0].xyz;
  tmpvar_7[1] = _Object2World[1].xyz;
  tmpvar_7[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * (tmpvar_6 - (2.0 * 
    (dot (tmpvar_1, tmpvar_6) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_8;
  highp vec4 o_9;
  highp vec4 tmpvar_10;
  tmpvar_10 = (tmpvar_4 * 0.5);
  highp vec2 tmpvar_11;
  tmpvar_11.x = tmpvar_10.x;
  tmpvar_11.y = (tmpvar_10.y * _ProjectionParams.x);
  o_9.xy = (tmpvar_11 + tmpvar_10.w);
  o_9.zw = tmpvar_4.zw;
  tmpvar_3.xyz = (((_Object2World * _glesVertex).xyz - unity_ShadowFadeCenterAndType.xyz) * unity_ShadowFadeCenterAndType.w);
  tmpvar_3.w = (-((glstate_matrix_modelview0 * _glesVertex).z) * (1.0 - unity_ShadowFadeCenterAndType.w));
  gl_Position = tmpvar_4;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_9;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD4 = tmpvar_3;
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
uniform sampler2D unity_LightmapInd;
uniform highp vec4 unity_LightmapFade;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
in highp vec4 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec3 lmIndirect_3;
  mediump vec3 lmFull_4;
  mediump float lmFade_5;
  mediump vec4 light_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_8;
  tmpvar_8 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_8 * _Color);
  lowp vec4 tmpvar_10;
  tmpvar_10 = (texture (_Cube, tmpvar_7) * tmpvar_8.w);
  lowp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_12;
  tmpvar_12 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_6 = tmpvar_12;
  mediump vec4 tmpvar_13;
  tmpvar_13 = max (light_6, vec4(0.001, 0.001, 0.001, 0.001));
  light_6.w = tmpvar_13.w;
  highp float tmpvar_14;
  tmpvar_14 = ((sqrt(
    dot (xlv_TEXCOORD4, xlv_TEXCOORD4)
  ) * unity_LightmapFade.z) + unity_LightmapFade.w);
  lmFade_5 = tmpvar_14;
  lowp vec3 tmpvar_15;
  tmpvar_15 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lmFull_4 = tmpvar_15;
  lowp vec3 tmpvar_16;
  tmpvar_16 = (2.0 * texture (unity_LightmapInd, xlv_TEXCOORD3).xyz);
  lmIndirect_3 = tmpvar_16;
  light_6.xyz = (tmpvar_13.xyz + mix (lmIndirect_3, lmFull_4, vec3(clamp (lmFade_5, 0.0, 1.0))));
  lowp vec4 c_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = (tmpvar_9.xyz * light_6.xyz);
  c_17.xyz = tmpvar_18;
  c_17.w = (tmpvar_10.w * _ReflectColor.w);
  c_2 = c_17;
  c_2.xyz = (c_2.xyz + tmpvar_11);
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_ON" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_5;
  tmpvar_5 = (_glesVertex.xyz - ((_World2Object * tmpvar_4).xyz * unity_Scale.w));
  highp mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (tmpvar_5 - (2.0 * 
    (dot (tmpvar_1, tmpvar_5) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_7;
  highp vec4 o_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_9.x;
  tmpvar_10.y = (tmpvar_9.y * _ProjectionParams.x);
  o_8.xy = (tmpvar_10 + tmpvar_9.w);
  o_8.zw = tmpvar_3.zw;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_8;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
varying highp vec2 xlv_TEXCOORD0;
varying mediump vec3 xlv_TEXCOORD1;
varying highp vec4 xlv_TEXCOORD2;
varying highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (textureCube (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2DProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec3 lm_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lm_10 = tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = lm_10;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (max (light_3, vec4(0.001, 0.001, 0.001, 0.001)) + tmpvar_12);
  light_3 = tmpvar_13;
  lowp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_6.xyz * tmpvar_13.xyz);
  c_14.xyz = tmpvar_15;
  c_14.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_14;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3#version 300 es


#ifdef VERTEX


in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp vec4 unity_Scale;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
out highp vec2 xlv_TEXCOORD0;
out mediump vec3 xlv_TEXCOORD1;
out highp vec4 xlv_TEXCOORD2;
out highp vec2 xlv_TEXCOORD3;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = normalize(_glesNormal);
  mediump vec3 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = _WorldSpaceCameraPos;
  highp vec3 tmpvar_5;
  tmpvar_5 = (_glesVertex.xyz - ((_World2Object * tmpvar_4).xyz * unity_Scale.w));
  highp mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (tmpvar_5 - (2.0 * 
    (dot (tmpvar_1, tmpvar_5) * tmpvar_1)
  )));
  tmpvar_2 = tmpvar_7;
  highp vec4 o_8;
  highp vec4 tmpvar_9;
  tmpvar_9 = (tmpvar_3 * 0.5);
  highp vec2 tmpvar_10;
  tmpvar_10.x = tmpvar_9.x;
  tmpvar_10.y = (tmpvar_9.y * _ProjectionParams.x);
  o_8.xy = (tmpvar_10 + tmpvar_9.w);
  o_8.zw = tmpvar_3.zw;
  gl_Position = tmpvar_3;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = o_8;
  xlv_TEXCOORD3 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT


layout(location=0) out mediump vec4 _glesFragData[4];
uniform sampler2D _MainTex;
uniform lowp samplerCube _Cube;
uniform lowp vec4 _Color;
uniform lowp vec4 _ReflectColor;
uniform sampler2D _LightBuffer;
uniform sampler2D unity_Lightmap;
in highp vec2 xlv_TEXCOORD0;
in mediump vec3 xlv_TEXCOORD1;
in highp vec4 xlv_TEXCOORD2;
in highp vec2 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 c_2;
  mediump vec4 light_3;
  highp vec3 tmpvar_4;
  tmpvar_4 = xlv_TEXCOORD1;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_MainTex, xlv_TEXCOORD0);
  lowp vec4 tmpvar_6;
  tmpvar_6 = (tmpvar_5 * _Color);
  lowp vec4 tmpvar_7;
  tmpvar_7 = (texture (_Cube, tmpvar_4) * tmpvar_5.w);
  lowp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_7.xyz * _ReflectColor.xyz);
  lowp vec4 tmpvar_9;
  tmpvar_9 = textureProj (_LightBuffer, xlv_TEXCOORD2);
  light_3 = tmpvar_9;
  mediump vec3 lm_10;
  lowp vec3 tmpvar_11;
  tmpvar_11 = (2.0 * texture (unity_Lightmap, xlv_TEXCOORD3).xyz);
  lm_10 = tmpvar_11;
  mediump vec4 tmpvar_12;
  tmpvar_12.w = 0.0;
  tmpvar_12.xyz = lm_10;
  mediump vec4 tmpvar_13;
  tmpvar_13 = (max (light_3, vec4(0.001, 0.001, 0.001, 0.001)) + tmpvar_12);
  light_3 = tmpvar_13;
  lowp vec4 c_14;
  mediump vec3 tmpvar_15;
  tmpvar_15 = (tmpvar_6.xyz * tmpvar_13.xyz);
  c_14.xyz = tmpvar_15;
  c_14.w = (tmpvar_7.w * _ReflectColor.w);
  c_2 = c_14;
  c_2.xyz = (c_2.xyz + tmpvar_8);
  tmpvar_1 = c_2;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_ON" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "LIGHTMAP_ON" "DIRLIGHTMAP_ON" "HDR_LIGHT_PREPASS_ON" }
"!!GLES3"
}
}
 }
}
Fallback "Reflective/VertexLit"
}