Shader "Custom/ykEff_da_Add" {
Properties {
 _MainTex ("Base (RGB) Transparency (A)", 2D) = "white" {}
 _ZOffset ("ZOffset", Float) = 0
}
SubShader {
 LOD 200
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
    float4 _MainTex_ST;
    float _ZOffset;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      float2 texcoord0 : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      float2 texcoord0 : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      float4 vpos = mul(UNITY_MATRIX_MVP, v.vertex);
      vpos.z += (_ZOffset * 0.01);

      o.vertex = vpos;
      o.color = v.color;
      o.texcoord0 = ((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      float4 texCol = tex2D(_MainTex, f.texcoord0);

      texCol *= f.color;

      return float4(texCol.xyz, texCol.w);
    }
  ENDCG
 }
}
Fallback "Diffuse"
}
