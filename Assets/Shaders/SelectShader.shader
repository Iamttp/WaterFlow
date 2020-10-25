Shader "Unlit/SelectShader"
{
	Properties
	{
		_Color("color", Color) = (1.0,1.0,1.0,1.0)
		_Rate("rate", Float) = 10
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
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			float _Rate;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				_Color.rgb = _Color.rgb * (sin(_Time.y * _Rate) * 0.2 + 0.8);
				return _Color;
			}
			ENDCG
		}
	}
}
