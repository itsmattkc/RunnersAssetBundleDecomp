Shader "Custom/ykChrLine_d" {
Properties {
 _OutlineColor ("OutLine Color", Color) = (0,0,0,1)
 _OutlineWidth ("OutLine Width", Float) = 0.05
 _OutlineZOffset ("OutLine ZOffset", Float) = 0
 _InnerZOffset ("Inner ZOffset", Float) = 0
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) Alpha(A)", 2D) = "white" {}
}
SubShader {
 Tags { "RenderType"="Transparent" }
 Pass {
  Name "DIFFUSE"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    float4 _DifCol;
    sampler2D _DifTex;
    float _InnerZOffset;
    float4 _DifTex_ST;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float3 normal : NORMAL;
      float4 texcoord0 : TEXCOORD0;
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

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.vertex.z += (_InnerZOffset * 0.01);

      float2 tmp = ((v.texcoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);

      float3 litDir;
      if ((unity_LightPosition[0].w == 0.0)) {
        litDir = normalize(mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz);
      } else {
        litDir = normalize((mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz - v.vertex.xyz));
      };

      o.color.xyz = (((
          (dot (v.normal, normalize(litDir)) * 0.5)
         + 0.5) * unity_LightColor[0]).xyz + UNITY_LIGHTMODEL_AMBIENT.xyz);

      o.texcoord0 = tmp;

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      float4 texCol = tex2D(_DifTex, f.texcoord0);
      return float4((((f.color.xyz * _DifCol.xyz) * texCol.xyz) * float3(2.0, 2.0, 2.0)), texCol.w);
    }
  ENDCG
 }
 UsePass "Custom/ykOutLine/OUTLINE"
}
Fallback "Toon/Basic"
}
