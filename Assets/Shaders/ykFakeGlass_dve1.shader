Shader "Custom/ykFakeGlass_dve1" {
Properties {
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) EnvMask(A)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "white" {}
}
SubShader {
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Name "ENVMAP"
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="Transparent" "RenderType"="Transparent" }
  Blend SrcAlpha One

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #include "UnityCG.cginc"

    float4 _DifCol;
    sampler2D _DifTex;
    sampler2D _EnvTex;
    float _InnerZOffset;
    float4 _DifTex_ST;
    float _RimPower;
    float4 _RimCol;

    struct appdata_t
    {
      float4 vertex : POSITION;
      fixed4 color : COLOR;
      float3 normal : NORMAL;
      float4 texcoord0 : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      float2 texcoord0 : TEXCOORD0;
      float2 texcoord1 : TEXCOORD1;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

      o.texcoord0 = ((v.texcoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);

      float4 tmp = float4(v.normal, 1.0);
      float4 tmp2 = mul(UNITY_MATRIX_IT_MV, tmp);

      float4 envVec = tmp2;

      o.color.w = dot (v.normal, normalize(tmp2.xyz));
      o.color.xyz = v.color.xyz;

      float4x4 tmpvar_9 = float4x4(
        float4(0.5, 0.0, 0.0, 0.5),
        float4(0.0, 0.5, 0.0, 0.5),
        float4(0.0, 0.0, 1.0, 0.0),
        float4(0.0, 0.0, 0.0, 1.0)
      );

      envVec.zw = float2(0.0, 1.0);

      o.texcoord1.xy = mul(tmpvar_9, envVec).xy;

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      float4 texCol = tex2D(_DifTex, f.texcoord0);
      float4 envCol = tex2D(_EnvTex, f.texcoord1);

      return float4(((f.color.xyz * (_DifCol.xyz * texCol.xyz)) + ((envCol.xyz * texCol.w) * f.color.w)), (texCol.w * envCol.w));
    }
  ENDCG
 }
}
}
