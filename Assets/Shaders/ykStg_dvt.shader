Shader "Custom/ykStg_dvt" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _AmbientColor ("AmbientColor", Color) = (1,1,1,1)
}
SubShader { 
 Pass {
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary double }
  SetTexture [_MainTex] { ConstantColor (0.5,0.5,0.5,1) combine previous * constant }
  SetTexture [_MainTex] { ConstantColor [_AmbientColor] combine previous * constant }
 }
}
}