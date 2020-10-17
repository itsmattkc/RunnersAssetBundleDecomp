Shader "Custom/ykStg_d_at" {
Properties {
 _MainTex ("Base (RGB) Transparency (A)", 2D) = "white" {}
 _LitTex ("LightMap (RGB)", 2D) = "black" {}
 _AmbientColor ("AmbientColor", Color) = (1,1,1,1)
}
SubShader {
 LOD 80
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="Transparent" "RenderType"="Transparent" }
  Lighting On
  Material {
   Ambient (1,1,1,1)
   Diffuse (1,1,1,1)
  }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * primary double, texture alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "QUEUE"="Transparent" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
  }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_LitTex] { Matrix [unity_LightmapMatrix] combine texture }
  SetTexture [_MainTex] { combine texture * previous double, texture alpha }
  SetTexture [_LitTex] { ConstantColor [_AmbientColor] combine previous * constant }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLMRGBM" "QUEUE"="Transparent" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
  }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_LitTex] { Matrix [unity_LightmapMatrix] combine texture }
  SetTexture [_MainTex] { combine texture * previous double, texture alpha }
  SetTexture [_LitTex] { ConstantColor [_AmbientColor] combine previous * constant }
 }
}
SubShader {
 LOD 100
 Tags { "QUEUE"="Transparent" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "QUEUE"="Transparent" "RenderType"="Transparent" }
  Lighting On
  Material {
   Ambient (1,1,1,1)
   Diffuse (1,1,1,1)
  }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_MainTex] { combine texture * primary double, texture alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "QUEUE"="Transparent" "RenderType"="Transparent" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord1", TexCoord0
  }
  Blend SrcAlpha OneMinusSrcAlpha
  SetTexture [_LitTex] { combine texture, previous alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "QUEUE"="Transparent" "RenderType"="Transparent" }
  ZWrite Off
  Fog { Mode Off }
  Blend DstColor Zero
  SetTexture [_MainTex] { combine texture, previous alpha }
 }
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" "QUEUE"="Transparent" "RenderType"="Transparent" }
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  Offset 1, 1

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma multi_compile_shadowcaster

    #include "UnityCG.cginc"

    struct appdata_t
    {
      float4 vertex : POSITION;
    };

    struct v2f
    {
      V2F_SHADOW_CASTER;
    };

    v2f vert(appdata_t v)
    {
      v2f o;
      TRANSFER_SHADOW_CASTER(o)
      return o;
    }

    float4 frag(v2f f) : COLOR
    {
      SHADOW_CASTER_FRAGMENT(f)
    }
  ENDCG
 }
 Pass {
  Name "SHADOWCOLLECTOR"
  Tags { "LIGHTMODE"="SHADOWCOLLECTOR" "QUEUE"="Transparent" "RenderType"="Transparent" }
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha

  CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag

    #define SHADOW_COLLECTOR_PASS

    #pragma multi_compile_shadowcollector

    #include "UnityCG.cginc"

    struct appdata_t
    {
      float4 vertex : POSITION;
    };

    struct v2f
    {
      V2F_SHADOW_COLLECTOR;
    };

    v2f vert(appdata_t v)
    {
      v2f o;
      TRANSFER_SHADOW_COLLECTOR(o)
      return o;
    }

    fixed4 frag(v2f f) : COLOR
    {
      SHADOW_COLLECTOR_FRAGMENT(f)
    }
  ENDCG
 }
}
}
