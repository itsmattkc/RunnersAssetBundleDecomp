Shader "Custom/ykStg_dvau_ca_t" {
Properties {
 _MainTex ("Base (RGB) Transparency (A)", 2D) = "white" {}
 _AmbientColor ("AmbientColor", Color) = (1,1,1,1)
 _ScrollingSpeed ("UVScrollSpeed", Vector) = (0,0,0,0)
}
SubShader {
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    sampler2D _MainTex;
    float4 _ScrollingSpeed;
    float4 _AmbientColor;

    struct appdata_t
    {
      float4 vertex : POSITION;
      fixed4 color : COLOR;
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
      o.texcoord0 = (v.texcoord0 + frac((_ScrollingSpeed * _Time.y))).xy;
      o.color = v.color;

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      fixed4 c;

      fixed4 tmp = tex2D(_MainTex, f.texcoord0);

      c.w = tmp.w;
      c.xyz = tmp.xyz * _AmbientColor.xyz;
      c.xyz *= f.color.xyz;
      c.w *= f.color.w;

      return c;
    }
  ENDCG
 }
}
}
