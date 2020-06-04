Shader "Custom/Shader_QS004"
{
    Properties
    {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Tiling("Tiling" , float) = 1.0
		_Angle("Angle" , float) = 0.0
		[MaterialToggle] _IsStop("Stop" , float) = 0
		_Speed("Speed" , float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
		Cull off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

		sampler2D _MainTex;		//テクスチャ
		float _Tiling;		//タイリング

		float _Angle;		//角度(ラジアン)
		float _IsStop;		//停止フラグ
		float _Speed;		//速度

        struct Input
        {
            float2 uv_MainTex;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//回転行列の素　回転量(引数)＝回転角度＊回転速度(回転角度はラジアン角)
			half AngleCos = cos(_Angle);
			half AngleSin = sin(_Angle);
			//2次元の回転行列
			half2x2 RotateMatrix = half2x2(AngleCos, -AngleSin, AngleSin, AngleCos);
			//タイリング処理
			IN.uv_MainTex = frac(IN.uv_MainTex * _Tiling);
			//中心を起点としてUVを回転させる
			IN.uv_MainTex = mul(IN.uv_MainTex - 0.5f, RotateMatrix) + 0.5f;

			//角度で向きを決め、スピードでスクロール速度を決めてy方向でスクロール
			//あとは色。書くのも一つの手。

			if (_IsStop == 0)
			{
				IN.uv_MainTex.y += (_Speed * 0.075f) * _Time.y;
			}

			//ここまででUV座標を計算した
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex);

			o.Albedo = col.rgb;
            o.Alpha = col.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
