Shader "Custom/ykEff_Lerp" {
Properties {
 _StOneCol ("Start WhiteColor", Color) = (1,1,1,1)
 _StZeroCol ("Start BlackColor", Color) = (0,0,0,1)
 _EdOneCol ("End WhiteColor", Color) = (1,1,1,1)
 _EdZeroCol ("End BlackColor", Color) = (0,0,0,1)
 _MainTex ("Lerp (RGB) Transparency (A)", 2D) = "white" {}
 _ZOffset ("ZOffset", Float) = 0
}
SubShader {
 LOD 200
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    float4 _StOneCol;
    float4 _EdOneCol;
    float4 _StZeroCol;
    float4 _EdZeroCol;
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

      float tmp = (((texCol.x + texCol.y) + texCol.z) / 3.0);

      return float4(
        lerp (
          lerp(_EdZeroCol, _StZeroCol, f.color.xxxx),
          lerp (_EdOneCol, _StOneCol, f.color.yyyy),
          float4(tmp, tmp, tmp, tmp)
        ).xyz,
      f.color.w * texCol.w);
    }
  ENDCG
 }
}
Fallback "Diffuse"
}
