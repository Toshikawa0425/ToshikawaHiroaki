Shader "Toon/CutOut Outline"
{
	Properties
	{
		_Cutoff("Mask Clip Value", Float) = 0.5
		_MainTex("Main Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,0)

		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005

		[HideInInspector] _texcoord("", 2D) = "white" {}
		[HideInInspector] __dirty("", Int) = 1
}

CGINCLUDE
#include "UnityCG.cginc"

struct appdata {
			float4 vertex : POSITION;
			float3 normal : NORMAL;
		};

		struct v2f {
			float4 pos : SV_POSITION;
			UNITY_FOG_COORDS(0)
				fixed4 color : COLOR;
		};

		uniform float _Outline;
		uniform float4 _OutlineColor;

		v2f vert(appdata v) {
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);

			float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
			float2 offset = TransformViewToProjection(norm.xy);

#ifdef UNITY_Z_0_FAR_FROM_CLIPSPACE //to handle recent standard asset package on older version of unity (before 5.5)
			o.pos.xy += offset * UNITY_Z_0_FAR_FROM_CLIPSPACE(o.pos.z) * _Outline;
#else
			o.pos.xy += offset * o.pos.z * _Outline;
#endif
			o.color = _OutlineColor;
			UNITY_TRANSFER_FOG(o, o.pos);
			return o;
		}
		ENDCG

SubShader
		{
			Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" }
			UsePass "Toon/Basic/BASE"
				Pass{
					Name "OUTLINE"
					Tags { "LightMode" = "Always" }
					Cull Front
					ZWrite On
					ColorMask RGB
					Blend SrcAlpha OneMinusSrcAlpha

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma multi_compile_fog
					fixed4 frag(v2f i) : SV_Target
					{
						UNITY_APPLY_FOG(i.fogCoord, i.color);
						return i.color;
					}
					ENDCG
			}
			Pass{
			Stencil
			{
				Ref 1
				Comp Always    // 描画されるところ全て
				Pass Replace   // 1(参照値)をステンシルバッファに書き込む
			}
			Cull Off
			CGPROGRAM
			#pragma target 3.0
			#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 

			struct Input
			{
				float2 uv_texcoord;
			};

			uniform float4 _Color;
			uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			uniform float _Cutoff = 0.5;

			void surf(Input i , inout SurfaceOutputStandardSpecular o)
			{
				float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode3 = tex2D(_MainTex, uv_MainTex);
				o.Albedo = (_Color * tex2DNode3).rgb;
				float temp_output_1_0 = 0.0;
				float3 temp_cast_1 = (temp_output_1_0).xxx;
				o.Specular = temp_cast_1;
				o.Smoothness = temp_output_1_0;
				o.Alpha = 1;
				clip(tex2DNode3.a - _Cutoff);
			}

			ENDCG
				}

				
		}
			Fallback "Diffuse"
				CustomEditor "ASEMaterialInspector"
    }