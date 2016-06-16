Shader "Custom/TransparentImageTransition"
{
	Properties
	{
		_MainTex("Image Texture", 2D) = "white" {}
		_TransitionTex("Transition Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_ColorOpacity("Color Opacity", Range(0, 1)) = 1
		[MaterialToggle] _Displace("Displace", Float) = 0
		_t("t", Range(0, 1)) = 0
	}
	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
		}
			Pass
		{
			AlphaToMask On
			BLEND SrcAlpha OneMinusSrcAlpha

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
				float2 uvScreen : TEXCOORD0;
				float2 uvTransition : TEXCOORD1;
			};

			float2 _MainTex_TexelSize;

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uvScreen = v.uv;

				o.uvTransition = v.uv;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
				{
					o.uvTransition.y = 1 - o.uvTransition.y;
				}
				#endif

				return o;
			}

			sampler2D _MainTex;
			sampler2D _TransitionTex;
			uniform float4 _Color;
			uniform float _ColorOpacity;
			uniform float _Displace;
			uniform float _t;

			float4 frag(v2f i) : SV_Target
			{
				float4 transitionColor = tex2D(_TransitionTex, i.uvTransition);

				float2 dir = float2(0, 0);
				if (_Displace) {
					dir = float2((transitionColor.r - 0.5) * 2, (transitionColor.g - 0.5) * 2);
				}

				float4 color = tex2D(_MainTex, i.uvScreen + dir * _t);

				if (transitionColor.b <= _t)
				{
					return lerp(color, _Color, _ColorOpacity);
				}
				else
				{
					return color;
				}
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
