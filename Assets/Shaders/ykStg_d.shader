Shader "Custom/ykStg_d" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" {}
 _LitTex ("LightMap (RGB)", 2D) = "black" {}
}
SubShader {
 LOD 80
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Lighting On
  Material {
   Ambient (1,1,1,1)
   Diffuse (1,1,1,1)
  }
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
  }
  SetTexture [_LitTex] { Matrix [unity_LightmapMatrix] combine texture }
  SetTexture [_MainTex] { combine texture * previous double, texture alpha * primary alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLMRGBM" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "normal", Normal
   Bind "texcoord1", TexCoord0
   Bind "texcoord", TexCoord1
  }
  SetTexture [_LitTex] { Matrix [unity_LightmapMatrix] combine texture }
  SetTexture [_MainTex] { combine texture * previous double, texture alpha * primary alpha }
 }
}
SubShader {
 LOD 100
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
  Lighting On
  Material {
   Ambient (1,1,1,1)
   Diffuse (1,1,1,1)
  }
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord1", TexCoord0
  }
  SetTexture [_LitTex] { combine texture }
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
  ZWrite Off
  Fog { Mode Off }
  Blend DstColor Zero
  SetTexture [_MainTex] { combine texture }
 }
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
  Cull Off
  Fog { Mode Off }
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
  Tags { "LIGHTMODE"="SHADOWCOLLECTOR" "RenderType"="Opaque" }
  Fog { Mode Off }

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
