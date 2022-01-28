Shader "Unlit/SieggesShader"
{
        Properties {
            _Color ("Tint", Color) = (0, 0, 0, 1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
  SubShader {
        Tags { "RenderType" = "Opaque" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            struct appdata_t {
                float4 vertex   : POSITION;
                float2 uv : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f {
                float4 vertex   : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color    : COLOR;
            }; 

            struct MeshProperties {
                float4x4 mat;
                float2 uvs;
                //float4 color;
            };

            StructuredBuffer<MeshProperties> _Properties;

            v2f vert(appdata_t i, uint instanceID: SV_InstanceID) {
                v2f o;

                float4 pos = mul(_Properties[instanceID].mat, i.vertex);
                o.vertex = UnityObjectToClipPos(pos);
                //o.color = _Properties[instanceID].color;
                //o.uv = _Properties[instanceID].uvs;
                //float2 uv = mul(_Properties[instanceID].uvs, i.uv);
                //_MainTex_ST.xy = _Properties[instanceID].uvs.xy;
                _MainTex_ST.zw = _Properties[instanceID].uvs.xy;
                o.uv = TRANSFORM_TEX(i.uv, _MainTex);
                //o.uv = TRANSFORM_TEX(_Properties[instanceID].uvs, _MainTex);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv);
                //col *= _Color;
                return col;
            }

            ENDCG
        }
    }
}
