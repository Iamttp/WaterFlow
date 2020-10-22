// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Unlit/SelectShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("color", Color) = (1.0,1.0,1.0,1.0)
        _RimColor("RimColor", Color) = (1,1,1,1)
        _RimPower("RimPower", Range(0.000001, 3.0)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct v2f
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldViewDir : TEXCOORD2;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float4 _MainTex_ST;

            fixed4 _RimColor;
            float _RimPower;

            v2f vert(appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);

                o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldViewDir = _WorldSpaceCameraPos.xyz - worldPos;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                //归一化法线，即使在vert归一化也不行，从vert到frag阶段有差值处理，传入的法线方向并不是vertex shader直接传出的
                fixed3 worldNormal = normalize(i.worldNormal);
                //把视线方向归一化
                float3 worldViewDir = normalize(i.worldViewDir);
                //计算视线方向与法线方向的夹角，夹角越大，dot值越接近0，说明视线方向越偏离该点，也就是平视，该点越接近边缘
                float rim = 1 - max(0, dot(worldViewDir, worldNormal));
                //计算rimLight
                fixed3 rimColor = _RimColor * pow(rim, 1 / _RimPower);

                fixed4 color;
                color.rgb = rimColor.rgb + (_Color.rgb * sin(_Time.y * 10) * 0.5 + 0.5);
                return color;
            }
            ENDCG
        }
    }
}

