Shader "Custom/ykSky_dau_add" {
Properties {
 _MainTex ("Base (RGB) Transparency (A)", 2D) = "" {}
 _ScrollingSpeed ("UVScrollSpeed", Vector) = (0,0,0,0)
}
SubShader {
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha One

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    float4 _ScrollingSpeed;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float2 texcoord0 : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      half2 texcoord0 : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.texcoord0 = (v.texcoord0 + frac((_ScrollingSpeed * _Time.y))).xy;

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      return tex2D(_MainTex, f.texcoord0);
    }
  ENDCG
 }
}
}
