Shader "Custom/celshading"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			

			struct v2f
			{
				float2 uv : TEXCOORD0; // texture coordinate
				float4 vertex : SV_POSITION; // clip space position
				half3 worldNormal : TEXCOORD1;
			};

			// vertex shader
			v2f vert(appdata_base v)
			{
				v2f o;
				// transform position to clip space
				// (multiply with model*view*projection matrix)
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				// just pass the texture coordinate
				o.uv = v.texcoord;
				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				return o;
			}

			// texture we will sample
			sampler2D _MainTex;

			// pixel shader; returns low precision ("fixed4" type)
			fixed4 frag(v2f i) : SV_Target
			{
				// sample texture and return it
				fixed4 col = tex2D(_MainTex, i.uv);
				float shadow = dot(i.worldNormal, normalize(_WorldSpaceLightPos0.xyz));
				
				col *= smoothstep(0.0, 0.1, shadow) * 0.4 + 0.6;
				//fixed4 col = 0;
				//col.rgb = i.worldNormal*0.5 + 0.5;
				//col.rgb = _WorldSpaceLightPos0.xyz;
				return col;
			}
			ENDCG
		}
	}
}