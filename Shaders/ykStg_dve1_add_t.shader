Shader "Custom/ykStg_dve1_add_t" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "black" {}
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

    #include "UnityCG.cginc"

    half4 unity_LightmapST;
    sampler2D _MainTex;
    sampler2D _EnvTex;
    sampler2D _LitTex;
    float4 _AmbientColor;

    struct appdata_t
    {
      float4 vertex : POSITION;
      fixed4 color : COLOR;
      float3 normal : NORMAL;
      float4 texcoord0 : TEXCOORD0;
      float4 texcoord1 : TEXCOORD1;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      half2 texcoord0 : TEXCOORD0;
      half2 texcoord1 : TEXCOORD1;
      float2 texcoord2 : TEXCOORD2;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.texcoord0 = v.texcoord0;
      o.texcoord1 = ((v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
      o.color = v.color;

      float4 tmp = float4(v.normal, 1.0);

      float4 envVec = mul(UNITY_MATRIX_IT_MV, tmp);
      envVec.zw = float2(0.0, 1.0);

      float4x4 tmpvar_9 = float4x4(
        float4(0.5, 0.0, 0.0, 0.5),
        float4(0.0, 0.5, 0.0, 0.5),
        float4(0.0, 0.0, 1.0, 0.0),
        float4(0.0, 0.0, 0.0, 1.0)
      );

      o.texcoord2 = mul(tmpvar_9, envVec).xy;

      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      fixed4 c;

      fixed4 tmp = tex2D(_MainTex, f.texcoord0);

      c.w = tmp.w;
      c.xyz = (tmp.xyz * (2.0 * tex2D(_LitTex, f.texcoord1).xyz));
      c.xyz *= _AmbientColor.xyz;
      c.xyz += f.color;
      c.xyz += (tex2D(_EnvTex, f.texcoord2).xyz * tmp.w);

      return fixed4(c.xyz, 1.0);
    }
  ENDCG
 }
}
}
