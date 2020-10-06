Shader "Custom/ykSky_dvu" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "" {}
 _ScrollingSpeed ("UVScrollSpeed", Vector) = (0,0,0,0)
}
SubShader {
 Pass {
  Cull Off
  ColorMaterial AmbientAndDiffuse

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    sampler2D _MainTex;
    float4 _ScrollingSpeed;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float4 texcoord : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      half2 texcoord : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.texcoord = (v.texcoord + frac((_ScrollingSpeed * _Time.y))).xy;

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      return tex2D(_MainTex, f.texcoord);
    }
  ENDCG
 }
}
}
