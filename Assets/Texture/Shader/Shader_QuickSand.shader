Shader "Unlit/Shader_QuickSand"
{
    Properties
    {
		_MainTex("Albedo (RGB)" , 2D) = "white" {}
		_Tiling("Tiling" , float) = 1.0
		_Angle("Angle" , float) = 0.0
		[MaterialToggle] _IsStop("Stop" , float) = 0
		_Speed("Speed" , float) = 0.0
	}
    SubShader
    {
		Tags { "RenderType" = "Opaque"}
		LOD 100
		Cull off

        Pass
        {
			//Tags { "LightMode" = "FowardBase"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "Lighting.cginc" 
			#include "AutoLight.cginc"

			#define PI 3.141592		//π

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
				fixed4 diff : COLOR0;
                float4 pos : SV_POSITION;
				SHADOW_COORDS(1)
            };

            sampler2D _MainTex;		//テクスチャ
            float4 _MainTex_ST;	//?
			float _Tiling;		//タイリング

			float _Angle;		//角度(ラジアン)
			float _IsStop;		//停止フラグ
			float _Speed;		//速度

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half NdotL = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = NdotL * _LightColor0;
				TRANSFER_SHADOW(o)

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				//回転行列の素　回転量(引数)＝回転角度＊回転速度(回転角度はラジアン角)
				half AngleCos = cos(_Angle);
				half AngleSin = sin(_Angle);
				//2次元の回転行列
				half2x2 RotateMatrix = half2x2(AngleCos, -AngleSin, AngleSin, AngleCos);
				//タイリング処理
				i.uv = frac(i.uv * _Tiling);
				//中心を起点としてUVを回転させる
				i.uv = mul(i.uv - 0.5f, RotateMatrix) + 0.5f;

				//ここまででUV座標を計算した

				//角度で向きを決め、スピードでスクロール速度を決めてy方向でスクロール
				//あとは色。書くのも一つの手。

				if (_IsStop == 0)
				{
					i.uv.y += (_Speed * 0.075f) * _Time.y;
				}

                fixed4 col = tex2D(_MainTex, i.uv);

				fixed4 shadow = SHADOW_ATTENUATION(i) * 1.3f;
				col *= i.diff * shadow;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }

		//影落とす用のPass
		//Pass
		//{
		//	Name "CastShadow"
		//	Tags { "LightMode" = "ShadowCaster" }

		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#pragma multi_compile_shadowcaster
		//	#include "UnityCG.cginc"

		//	struct v2f
		//	{
		//		V2F_SHADOW_CASTER;
		//	};

		//	v2f vert(appdata_base v)
		//	{
		//		v2f o;
		//		TRANSFER_SHADOW_CASTER(o)
		//		return o;
		//	}

		//	float4 frag(v2f i) : COLOR
		//	{
		//		SHADOW_CASTER_FRAGMENT(i)
		//	}
		//	ENDCG
		//}
    }
}
