Shader "Custom/ykStg_dva_col_t" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _LitTex ("LightMap (RGB)", 2D) = "black" {}
 _AmbientColor ("AmbientColor", Color) = (1,1,1,1)
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
    sampler2D _LitTex;
    float4 _AmbientColor;
    half4 unity_LightmapST;

    struct appdata_t
    {
      float4 vertex : POSITION;
      fixed4 color : COLOR;
      float4 texcoord0 : TEXCOORD0;
      float4 texcoord1 : TEXCOORD1;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      half2 texcoord0 : TEXCOORD0;
      half2 texcoord1 : TEXCOORD1;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.texcoord0 = v.texcoord0;
      o.texcoord1 = (mul(v.texcoord1.xy, unity_LightmapST.xy) + unity_LightmapST.zw);
      o.color = v.color;

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      fixed4 c;

      fixed4 tmp = tex2D(_MainTex, f.texcoord0);

      c.w = tmp.w;
      c.xyz = (tmp.xyz * (2.0 * tex2D(_LitTex, f.texcoord1).xyz));
      c.xyz *= _AmbientColor.xyz;
      c *= f.color;

      return c;
    }
  ENDCG
 }
}
}
