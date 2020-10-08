Shader "Custom/ykCmnIgr_d" {
Properties {
 _DifCol ("Color", Color) = (1,1,1,1)
 _MainTex ("Diffuse(RGB)", 2D) = "white" {}
}
SubShader {
 Tags { "IGNOREPROJECTOR"="true" "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Always" "IGNOREPROJECTOR"="true" "RenderType"="Opaque" }

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

    float4 _DifCol;
    sampler2D _MainTex;
    float4 _MainTex_ST;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float2 texcoord0 : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float2 texcoord0 : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.texcoord0 = ((v.texcoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      float4 texCol = tex2D(_MainTex, f.texcoord0);
      return float4((_DifCol.xyz * texCol.xyz), 1.0);
    }
  ENDCG
 }
}
}
