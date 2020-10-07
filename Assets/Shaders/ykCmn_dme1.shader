Shader "Custom/ykCmn_dme1" {
Properties {
 _DifCol ("Color", Color) = (1,1,1,1)
 _DifTex ("Diffuse(RGB) EnvMask(A)", 2D) = "white" {}
 _EnvTex ("Environ(RGB)", 2D) = "white" {}
}
SubShader {
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "ENVMAP"
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    float4 _DifCol;
    sampler2D _DifTex;
    sampler2D _EnvTex;
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
      float2 texcoord1 : TEXCOORD1;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

      o.texcoord0 = ((v.texcoord0.xy * _DifTex_ST.xy) + _DifTex_ST.zw);

      float4 tmp = float4(v.normal, 1.0);

      float4 envVec = mul(UNITY_MATRIX_IT_MV, tmp);
      envVec.zw = float2(0.0, 1.0);

      float3 litDir;
      if ((unity_LightPosition[0].w == 0.0)) {
        litDir = normalize(mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz);
      } else {
        litDir = normalize((mul(unity_LightPosition[0], UNITY_MATRIX_IT_MV).xyz - v.vertex.xyz));
      };

      o.color.w = dot(v.normal, normalize(litDir));
      o.color.xyz = (mul(o.color.w, unity_LightColor[0]).xyz + UNITY_LIGHTMODEL_AMBIENT.xyz);

      float4x4 tmpvar_9 = float4x4(
        float4(0.5, 0.0, 0.0, 0.5),
        float4(0.0, 0.5, 0.0, 0.5),
        float4(0.0, 0.0, 1.0, 0.0),
        float4(0.0, 0.0, 0.0, 1.0)
      );

      o.texcoord1 = mul(tmpvar_9, envVec).xy;

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      float4 texCol = tex2D(_DifTex, f.texcoord0);;
      float4 envCol = tex2D(_EnvTex, f.texcoord1);;
      float4 tmpvar_5 = float4((((f.color.xyz *
        (_DifCol.xyz * texCol.xyz)
      ) + (
        (envCol.xyz * texCol.w)
       * f.color.w)) * float3(2.0, 2.0, 2.0)), 1.0);
      return tmpvar_5;
    }
  ENDCG
 }
}
}
