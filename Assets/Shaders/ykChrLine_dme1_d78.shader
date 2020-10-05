Shader "Custom/ykChrLine_dme1" {
Properties {
 _OutlineColor ("OutLine Color", Color) = (0,0,0,1)
 _OutlineWidth ("OutLine Width", Float) = 0.05
 _OutlineZOffset ("OutLine ZOffset", Float) = 0
 _InnerZOffset ("Inner ZOffset", Float) = 0
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) EnvMask(A)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
 UsePass "Custom/ykOutLine/OUTLINE"
 UsePass "Custom/ykChr_dme1/ENVMAP"
}
Fallback "Toon/Basic"
}