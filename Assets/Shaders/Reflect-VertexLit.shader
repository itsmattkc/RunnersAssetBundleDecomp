Shader "Reflective/VertexLit" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _SpecColor ("Spec Color", Color) = (1,1,1,1)
 _Shininess ("Shininess", Range(0.03,1)) = 0.7
 _ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
 _MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {}
 _Cube ("Reflection Cubemap", CUBE) = "_Skybox" { TexGen CubeReflect }
}
SubShader { 
 LOD 150
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "BASE"
  Tags { "LIGHTMODE"="Always" "RenderType"="Opaque" }
Program "vp" {
}
Program "fp" {
}
 }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Lighting On
  SeparateSpecular On
  Material {
   Diffuse [_Color]
   Emission [_PPLAmbient]
   Specular [_SpecColor]
   Shininess [_Shininess]
  }
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
Program "fp" {
}
  SetTexture [_MainTex] { combine texture, texture alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
  }
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
  ColorMask RGB
  SetTexture [unity_Lightmap] { Matrix [unity_LightmapMatrix] ConstantColor [_Color] combine texture * constant }
  SetTexture [_MainTex] { combine texture * previous double, texture alpha * primary alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLMRGBM" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord1", TexCoord1
   Bind "texcoord", TexCoord2
  }
  ZWrite Off
  Fog {
   Color (0,0,0,0)
  }
  Blend One One
  ColorMask RGB
  SetTexture [unity_Lightmap] { Matrix [unity_LightmapMatrix] combine texture * texture alpha double }
  SetTexture [unity_Lightmap] { ConstantColor [_Color] combine previous * constant }
  SetTexture [_MainTex] { combine texture * previous quad, texture alpha * primary alpha }
 }
}
SubShader { 
 LOD 150
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "BASE"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Lighting On
  SeparateSpecular On
  Material {
   Ambient (1,1,1,1)
   Diffuse [_Color]
   Specular [_SpecColor]
   Shininess [_Shininess]
  }
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
  SetTexture [_Cube] { combine texture * previous alpha + previous, previous alpha }
 }
}
Fallback "VertexLit"
}