Shader "Custom/Transition"
{
	Properties
	{
		_MainTex("Screen Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_t("t", Range(0, 1)) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			uniform float4 _Color;
			uniform float _t;

			float4 frag(v2f i) : SV_Target
			{
				if (i.uv.x <= _t)
				{
					return _Color;
				}
				else
				{
					return tex2D(_MainTex, i.uv);
				}
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
