Shader "Custom/WhiteRimLight"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
			float3 worldNormal;
			float3 viewDir;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			fixed4 BaseColor = fixed4(0.0f, 0.0f, 0.0f, 1);
			fixed4 RimColor = fixed4(1.0f, 1.0f, 1.0f, 1);

			o.Albedo = BaseColor;
			float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
			o.Emission = RimColor * pow(rim, 2.5);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
