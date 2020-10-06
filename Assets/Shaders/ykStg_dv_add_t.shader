Shader "Custom/ykStg_dv_add_t" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _LitTex ("LightMap (RGB)", 2D) = "black" {}
 _AmbientColor ("AmbientColor", Color) = (1,1,1,1)
}
SubShader {
 Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }
 Pass {
  Tags { "QUEUE"="Geometry" "RenderType"="Opaque" }

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
      o.color = v.color;
      o.texcoord0 = v.texcoord0;
      o.texcoord1 = (v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw;

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      fixed4 c;
      fixed4 tmpvar_2;
      tmpvar_2 = tex2D(_MainTex, f.texcoord0);
      c.w = tmpvar_2.w;
      c.xyz = (tmpvar_2.xyz * (2.0 * tex2D(_LitTex, f.texcoord1).xyz));
      float3 tmpvar_3;
      tmpvar_3 = (c.xyz * _AmbientColor.xyz);
      c.xyz = tmpvar_3;
      float3 tmpvar_4;
      tmpvar_4 = (c.xyz + f.color.xyz);
      c.xyz = tmpvar_4;
      return c;
    }
  ENDCG
 }
}
}
