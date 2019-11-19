Shader "Unlit/BeamShader"
{
    Properties
    {
        _MainTexx ("Texture", 2D) = "white" {}
		_Colour ("Colour", Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTexx;
			fixed4 _Colour;

            VertexOutput vert (VertexInput input)
            {
                VertexOutput output;
                output.vertex = UnityObjectToClipPos(input.vertex);
                output.texcoord = input.texcoord;
				
				output.texcoord.x += _Time[1]/2.;


				
                return output;
            }

            fixed4 frag (VertexOutput input) : COLOR
            {
                
                fixed4 col = tex2D(_MainTexx, input.texcoord.xy);

				if (col.a == 0){
					col.r = 0;
					col.g = 0;
					col.b = 0;
					col.a = 1;
					discard;
				}

                return col;
            }
            ENDCG
        }
    }
}
