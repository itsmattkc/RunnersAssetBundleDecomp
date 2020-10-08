Shader "Custom/UI/ykHud_dau" {
Properties {
 _MainTex ("Base (RGB) Transparency (A)", 2D) = "" {}
 _ScrollingSpeed ("UVScrollSpeed", Vector) = (0,0,0,0)
}
SubShader {
 LOD 100
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

    float4 _DifCol;
    sampler2D _MainTex;
    float4 _MainTex_ST;
    float4 _ScrollingSpeed;

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
      half2 texcoord0 : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.color = v.color;
      o.texcoord0 = (v.texcoord0.xy + frac((_ScrollingSpeed * _Time.y)).xy);

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      fixed4 c = tex2D(_MainTex, f.texcoord0) * f.color;
      return c;
    }
  ENDCG
 }
}
}
