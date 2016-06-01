Shader "Custom/Blur"
{
	Properties
	{
		_MainTex("Screen Texture", 2D) = "white" {}
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
			float4 _MainTex_TexelSize;

			float4 blur(sampler2D tex, float2 uv, float4 size) {
				float4 color = tex2D(tex, uv + float2(-size.x, -size.y));
				color += tex2D(tex, uv + float2(0, -size.y));
				color += tex2D(tex, uv + float2(size.x, -size.y));

				color += tex2D(tex, uv + float2(-size.x, 0));
				color += tex2D(tex, uv);
				color += tex2D(tex, uv + float2(size.x, 0));
				
				color += tex2D(tex, uv + float2(-size.x, size.y));
				color += tex2D(tex, uv + float2(0, size.y));
				color += tex2D(tex, uv + float2(size.x, size.y));

				return color / 9;
			}

			float4 frag(v2f i) : SV_Target
			{
				return blur(_MainTex, i.uv, _MainTex_TexelSize);
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}