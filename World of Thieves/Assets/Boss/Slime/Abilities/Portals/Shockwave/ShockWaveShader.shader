// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ShockWaveShader"
{
    Properties
    {
		_Colour ("Colour", Color) = (1,1,1,1)

		_BumpMap ("Noise text", 2D) = "bump" {}
		_Magnitude ("Magnitude", Range(0,1)) = 0.05
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque" }
		ZWrite On Lighting Off Cull Off Fog { Mode Off } Blend One Zero
        
		GrabPass { "_GrabTexture" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _GrabTexture;

			fixed4 _Colour;

			sampler2D _BumpMap;
			float _Magnitude;

            struct VertexInput
            {
                float4 vertex : POSITION;

                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;               
				float4 uvgrab : TEXTCOORD1;
            };

            VertexOutput vert (VertexInput input)
            {
                VertexOutput output;
				output.vertex = UnityObjectToClipPos(input.vertex);
 
				output.texcoord = input.texcoord;
 
				output.uvgrab = ComputeGrabScreenPos(output.vertex);
				return output;
            }

            fixed4 frag (VertexOutput input) : COLOR
            {
				half4 bump = tex2D(_BumpMap, input.texcoord);
				half2 distortion = UnpackNormal(bump).rg;
 
				input.uvgrab.xy += distortion * _Magnitude;
 
				fixed4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(input.uvgrab));
				return col * _Colour;
            }
            ENDCG
        }
    }
}
