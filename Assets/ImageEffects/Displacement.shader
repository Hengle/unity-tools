Shader "Custom/Displacement"
{
	Properties
	{
		_MainTex("Screen Texture", 2D) = "white" {}
		_DisplaceTex("Displacement Texture", 2D) = "white" {}
		_Magnitude("Magnitude", Range(0, 0.1)) = 0.1
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			sampler2D _DisplaceTex;
			uniform float _Magnitude;

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
				float2 disp = tex2D(_DisplaceTex, i.uv).xy;
				disp = ((disp * 2) - 1) * _Magnitude;

				return tex2D(_MainTex, i.uv + disp);
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}
