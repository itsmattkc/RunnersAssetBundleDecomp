Shader "Custom/ykStg_dv" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
}
SubShader {
 Pass {
  ColorMaterial AmbientAndDiffuse
  SetTexture [_MainTex] { combine texture * primary double }
  SetTexture [_MainTex] { ConstantColor (0.5,0.5,0.5,1) combine previous * constant }
 }
}
}
