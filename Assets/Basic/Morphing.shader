Shader "Custom/Morphing"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Tex1("Texture 1", 2D) = "white" {}
		_Tex2("Texture 2", 2D) = "white" {}
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

			uniform float4 _Color;
			sampler2D _Tex1;
			sampler2D _Tex2;
			uniform float _t;

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

			float4 frag(v2f i) : SV_Target
			{
				float4 combinedColor = lerp(tex2D(_Tex1, i.uv), tex2D(_Tex2, i.uv), _t);
				return _Color * combinedColor;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
