Shader "Custom/ykOutLine" {
Properties {
 _OutlineColor ("OutLine Color", Color) = (0,0,0,1)
 _OutlineWidth ("OutLine Width", Float) = 1
 _OutlineZOffset ("OutLine ZOffset", Float) = 0
}
SubShader {
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "OUTLINE"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Cull Front

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

    float4 _OutlineColor;
    float _OutlineWidth;
    float _OutlineZOffset;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float3 normal : NORMAL;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      float4 vpos;

      float4 tmp = mul(UNITY_MATRIX_MVP, v.vertex);

      vpos.w = tmp.w;

      float3x3 invtrans_mv_mat = UNITY_MATRIX_IT_MV;
      float2x2 projection_mat = UNITY_MATRIX_P;

      vpos.xy = (tmp.xy + (mul(projection_mat, mul(invtrans_mv_mat, v.normal).xy) * _OutlineWidth));
      vpos.z = (tmp.z + (_OutlineZOffset * 0.01));

      o.vertex = vpos;
      o.color = _OutlineColor;

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      return float4(_OutlineColor.xyz, 1.0);
    }
  ENDCG
 }
}
}
