Shader "Custom/DrillTrackIgnoreLight" {
Properties {
 _MainTex ("Base", 2D) = "white" {}
 _OffsetZ ("OffsetZ", Float) = 1
}
SubShader {
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    half4 _MainTex_ST;
    float _OffsetZ;

    struct appdata_t
    {
      float4 vertex : POSITION;
      fixed4 color : COLOR;
      float2 texcoord : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      half2 texcoord0 : TEXCOORD0;
      half texcoord1 : TEXCOORD1;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.vertex.z += (_OffsetZ * 0.01);
      o.texcoord0 = (v.texcoord.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
      o.texcoord1 = v.color.w;

      return o;
    }

    half4 frag(v2f f) : COLOR
    {
      fixed4 texCol = tex2D(_MainTex, f.texcoord0);
      return half4(texCol.rgb, f.texcoord1);
    }
  ENDCG
 }
}
Fallback Off
}
