Shader "Custom/PlayerSand"
{
	Properties
	{
		_SandColor("SandColor", Color) = (1,1,1,1)
		_ColRange("ColorRange", float) = 0.0
		[MaterialToggle] _IsStop("Stop" , float) = 0
	}
    SubShader
    {
        Tags { "Queue"="Transparent" }
        LOD 200
		Cull off

        CGPROGRAM
        #pragma surface surf Standard alpha:fade
        #pragma target 3.0

        struct Input
        {
			float3 worldPos;	//ワールド座標
        };

        fixed4	_SandColor;		//砂の色
		float	_ColRange;		//範囲

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			//ローカル座標を求める
			float3 LocalPos = IN.worldPos - mul(unity_ObjectToWorld, float4(0, 0, 0, 1)).xyz;

			//ローカル座標Yがマイナスかプラスで場合分け(0はプラス側)
			if (LocalPos.y >= 0)
			{
				//_ColRangeより近かったら色を付ける
				if (abs(LocalPos.y) <= (_ColRange - 0.3f))
				{
					o.Albedo = _SandColor;
					o.Alpha = 1.0f;
				}
				else
				{
					o.Alpha = 0.0f;
				}
			}
			else if (LocalPos.y < 0)
			{
				//_ColRangeより遠かったら色を付ける
				if (abs(LocalPos.y) >= _ColRange &&
					abs(LocalPos.y) > 0.3f)
				{
					o.Albedo = _SandColor;
					o.Alpha = 1.0f;
				}
				else
				{
					o.Alpha = 0.0f;
				}
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}
