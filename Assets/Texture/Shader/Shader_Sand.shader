Shader "Custom/Shader_Sand"
{
    Properties
    {
		_MainTex("Sand Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;	//メインテクスチャ
		
        struct Input
        {
            float2 uv_MainTex;
        };

		float2 _Test;		//テクスチャのオフセット

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float2 uv = IN.uv_MainTex;

			//受け取ったオフセットに時間をかける
			uv.x += _Test.x * _Time.y;
			uv.y += _Test.y * _Time.y;

			o.Albedo = tex2D(_MainTex, uv);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
