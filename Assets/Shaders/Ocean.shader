Shader "Unlit/Ocean"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		//噪音图
		_NoiseTex("Texture", 2D) = "white" {}
		//叠加颜色
		_Color("Color",Color) = (1,1,1,1)
		//亮度
		_Light("Light", Range(0, 10)) = 2
		//扭曲强度
		_Intensity("intensity", float) = 0.1
		//偏移速度
		_XSpeed("XSpeed", float) = 0.1
		_YSpeed("YSpeed", float) = 0.1

		// 波纹
		_Amount("_Amount",Range(0,1)) = 0.5
		_W("_W",Range(0,100)) = 50
		_Speed("_Speed",Range(0,10)) = 5
		_Flag("_Flag",Range(0,1)) = 0.1
		_Pow("_Pow",Range(0,1)) = 0.1

		_CenterU("_CenterU",Range(0,1)) = 0.2
		_CenterV("_CenterV",Range(0,1)) = 0.3

		_RS("_RS",float) = 0.5
		_RE("_RE",float) = 1
	}

		SubShader
	{
		Tags{ "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float4 _Color;
			float _Light;
			float _Intensity;
			float _XSpeed;
			float _YSpeed;

			float _Amount;
			float _Flag;
			float _Pow;
			float _Speed;
			float _W;

			float _CenterV;
			float _CenterU;

			float _RS;
			float _RE;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 center_uv = float2(_CenterU, _CenterV);
				float2 dt = center_uv - i.uv;
				float len = sqrt(dot(dt, dt));
				if (len > _RS && len < _RE) {
					float amount = max(_Amount, _Amount / pow(0.01 + len * _Speed, _Pow)); // + 0.01 防止为0 衰减
					if (amount < _Flag) amount = 0; // 振幅小于0.1表示没有

					i.uv.x += amount * cos(len * _W - _Time.y);
				}
				//根据时间和偏移速度获取噪音图的颜色作为uv偏移
				fixed4 noise_col = tex2D(_NoiseTex, i.uv + fixed2(_Time.y * _XSpeed, _Time.y * _YSpeed));
				//计算uv偏移的颜色和亮度和附加颜色计算
				fixed4 col = tex2D(_MainTex, i.uv + _Intensity * noise_col.rg) * _Light * _Color;
				UNITY_APPLY_FOG(i.fogCoord, col);
				col.a = _Color.a;
				return col;
			}
			ENDCG
		}
	}
}
