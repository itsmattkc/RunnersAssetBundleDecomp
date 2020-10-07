Shader "Sonic Dash/TransparentDiffuseNoZWriteAdditive" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}
SubShader {
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend One One

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    float4 _Color;
    sampler2D _MainTex;

    struct appdata_t
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      float4 texcoord0 : TEXCOORD0;
    };

    struct v2f
    {
      float4 vertex : POSITION;
      float4 color : COLOR;
      float3 texcoord0 : TEXCOORD0;
    };

    v2f vert(appdata_t v)
    {
      v2f o;

      o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
      o.color = v.color;
      o.texcoord0 = v.texcoord0.xyz;

      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      fixed4 texCol = tex2D(_MainTex, f.texcoord0.xy);
      return (_Color * f.color.w) * texCol;
    }
  ENDCG
 }
}
Fallback "Transparent/VertexLit"
}
